/*
Copyright (c) Clearsighted 2006-08 stephen@clearsighted.net

This library is free software; you can redistribute it and/or
modify it under the terms of the GNU Lesser General Public
License as published by the Free Software Foundation; either
version 2.1 of the License, or (at your option) any later version.

This library is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
Lesser General Public License for more details.

You should have received a copy of the GNU Lesser General Public
License along with this library; if not, write to the Free Software
Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301  USA
*/

package org.clearsighted.tutorbase.emscript.mappingtree;

import java.util.ArrayList;
import java.util.Collections;
import java.util.Comparator;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

import org.clearsighted.tutorbase.DebugFrame;
import org.clearsighted.tutorbase.DebugStyle;
import org.clearsighted.tutorbase.EventMapper;
import org.clearsighted.tutorbase.IEventMapperListener;
import org.clearsighted.tutorbase.WebDebugFrame;
import org.clearsighted.tutorbase.dormin.DorminAddress;
import org.clearsighted.tutorbase.dormin.DorminMessage;
import org.clearsighted.tutorengine.Type;
import org.clearsighted.xpstengine.XPSTTutorEngine;

/**
 * Implements the sequence tree in EMScript. The sequence expression in an .xpst file is parsed
 * into objects, some representing operators and some representing goalnodes. The tree is something
 * like a Petri net, in that it holds the current state of completion of the sequence as flags (like tokens)
 * in a tree of objects (like nodes).
 * 
 * MappingNode is the base for all the nodes, and implements general binary tree functionality as well
 * as the methods that control changing the state of the tree. LeafNode implements the references to
 * goalnodes, and a variety of classes implement the operators.
 * 
 * @see MappingNode
 * @see LeafNode
 *
 */
public class Tree implements IEventMapperListener
{
	private MappingNode root = null;
	private Mapping masterMapping = new Mapping();
	private HashMap<String, Mapping> mappings = new HashMap<String, Mapping>();
	// because appnodes can now be simultaneously mapped to multiple goalnodes, we need to preserve the order in the merge process in finishConstruction
	private List<Mapping> orderedMappings = new ArrayList<Mapping>();
	private List<LeafNode> leafNodes = new ArrayList<LeafNode>();
	private EventMapper myEventMapper;
	private String tutorName = "tutor";
	private MappingAction lastActionToTutor = null;
	// managed by pushCompleted and popCompleted. This keeps track of completed nodes
	// so we can know the correct leaf to uncomplete when we get a flag back from the tutor.
	private ArrayList<LeafNode> mostRecentlyCompleted = new ArrayList<LeafNode>();
	private List<ITreeListener> listeners = new ArrayList<ITreeListener>();
	public List<List<LeafNode>> groups = new ArrayList<List<LeafNode>>();
	// TODO: at this point, done is useless, since we don't have a concept of 'required' in the event mapper.
	// i'll leave this code in for now, though.
	private boolean done = false;
	private ArrayList<String> script = null;

	private DebugFrame debugFrame = null;
	private WebDebugFrame webDebugFrame = new WebDebugFrame();

	public MappingNode getRoot()
	{
		return root;
	}

	// TODO: not exactly pretty, but we need to see some stuff in the EventMapper
	public void setEventMapper(EventMapper eventmapper)
	{
		myEventMapper = eventmapper;
		myEventMapper.addListener(this);
	}

	private void setupBiNode(MappingNode binode, MappingNode lchild, MappingNode rchild)
	{
		lchild.setParent(binode);
		rchild.setParent(binode);
		binode.setChildren(lchild, rchild);
		root = binode;
	}

	public AndNode createAndNode(MappingNode lchild, MappingNode rchild)
	{
		AndNode ret = new AndNode(this);
		setupBiNode(ret, lchild, rchild);
		return ret;
	}

	public OrNode createOrNode(MappingNode lchild, MappingNode rchild)
	{
		OrNode ret = new OrNode(this);
		setupBiNode(ret, lchild, rchild);
		return ret;
	}

	public ThenNode createThenNode(MappingNode lchild, MappingNode rchild)
	{
		ThenNode ret = new ThenNode(this);
		setupBiNode(ret, lchild, rchild);
		return ret;
	}

	public UntilNode createUntilNode(MappingNode lchild, MappingNode rchild)
	{
		UntilNode ret = new UntilNode(this);
		setupBiNode(ret, lchild, rchild);
		return ret;
	}

	private LeafNode getMappedLeafNode(MappingAction action)
	{
		for (LeafNode l: leafNodes)
		{
			// TODO: there should only be one, barf otherwise
			if (l.mapped && l.myMapping.contains(action))
				return l;
		}
		return null;
	}

	public boolean isGoalNodeMapped(String gnname)
	{
		Mapping m = mappings.get(gnname);
		for (LeafNode l: leafNodes)
		{
			if (l.mapped && l.myMapping == m)
				return true;
		}
		return false;
	}
	
	private String getNextGoalNode()
	{
		for (LeafNode l: leafNodes)
		{
			if (l.isActive() && !myEventMapper.isGoalNodeCompleted(l.name))
				return l.name;
		}
		return null;
	}

	private static final Comparator<MappingAction> PriorityComparator = new Comparator<MappingAction>()
	{
		@Override
		public int compare(MappingAction o1, MappingAction o2)
		{
			// descending
			return o1.Options.priority < o2.Options.priority ? 1 : (o1.Options.priority < o2.Options.priority ? 0 : -1);
		}
	};

	private List<MappingAction> getActiveActions(boolean toapp, String name)
	{
		List<MappingAction> acts = null;
		List<MappingAction> ret = new ArrayList<MappingAction>();

		acts = masterMapping.get(toapp, name);
		int toppri = -1;

		if (acts != null)
		{
			Collections.sort(acts, PriorityComparator);
			for (MappingAction act: acts)
			{
				String gnname = act.GoalNodeName;
				if (gnname != null && isGoalNodeMapped(gnname) && myEventMapper.isGoalNodeEnabled(gnname))
				{
					if (toapp && lastActionToTutor == act)
					{
						ret.add(act);
						break;
					}
					if (act.Options.priority >= toppri)
					{
						toppri = act.Options.priority;
						ret.add(act);
					}
					else if (act.Options.priority < toppri)
						break;
				}
			}
		}
		return ret;
	}

	private MappingAction getActiveAction(boolean toapp, String name)
	{
		List<MappingAction> all = getActiveActions(toapp, name);
		if (all.size() > 0)
			return all.get(0);
		else
			return null;
	}

	public class MappingSummaryNode
	{
		public String goalnode = null;
		public boolean isNextNode = false;
	}

	// returns all appnodes from the mappings, along with the goalnode that they're currently mapped to
	public Map<String, MappingSummaryNode> getMappingSummary()
	{
		HashMap<String, List<MappingAction>> anh = masterMapping.getAppNodeHash();
		Map<String, MappingSummaryNode> ret = new HashMap<String, MappingSummaryNode>();
		String nextnodename = getNextGoalNode();
		for (String appnode: anh.keySet())
		{
			MappingAction ma = getActiveAction(false, appnode);
			MappingSummaryNode msn = new MappingSummaryNode();
			if (ma != null)
			{
				msn.goalnode = ma.GoalNodeName;
				msn.isNextNode = nextnodename.equals(msn.goalnode);
			}
			ret.put(appnode, msn);
		}
		return ret;
	}

	private void pushCompleted(LeafNode n)
	{
		mostRecentlyCompleted.add(n);
	}
	
	private LeafNode popCompleted(String gnname)
	{
		LeafNode ret = null;
		for (int i = mostRecentlyCompleted.size() - 1; i >= 0; i--)
		{
			if (mostRecentlyCompleted.get(i).name.equals(gnname))
			{
				ret = mostRecentlyCompleted.get(i);
				mostRecentlyCompleted.remove(i);
			}
		}
		return ret;
	}

	public List<DorminMessage> map(boolean toapp, DorminMessage in)
	{
		ArrayList<DorminMessage> ret = new ArrayList<DorminMessage>();
		MappingAction a = null;
		String name = in.Address.toDottedString();

		// TODO: restore?
//		if (!toapp && in.Verb.equals(DorminMessage.NoteValueSetVerb) && name.equals("TutorLink.GNDebug"))
//		{
//			if (debugFrame == null)
//			{
//				debugFrame = new DebugFrame();
//				debugFrame.addListener(this);
//			}
//			debugFrame.setVisible(true);
//			updateDebugFrame();
//			return null;
//		}

		if (toapp && in.Verb.equals(DorminMessage.HintMessageVerb))
		{
			in.Address = new DorminAddress("");
			ret.add(in);
			return ret;
		}
		else if (!toapp && in.Verb.equals(DorminMessage.GetHintVerb))
		{
			String nextgn = getNextGoalNode();
			System.out.println("asking for hint from " + nextgn);
			in.Address = new DorminAddress("tutor." + tutorName + (nextgn != null ? "." + nextgn : ""));
			ret.add(in);
			return ret;
		}

		try
		{
			if (toapp)
				name = name.substring(name.indexOf('.', name.indexOf('.') + 1) + 1);
		}
		catch (Exception e)
		{
			return null;
		}

		List<MappingAction> candidates = getActiveActions(toapp, name);
		
		if (candidates.size() == 0)
		{
			// uncomplete when a node is flagged that isn't active (that means the tutor triggered the change with a cycle goalnode trigger)
			if (toapp && in.Verb.equals(DorminMessage.FlagVerb))
			{
				// TODO: uncompletes can be spurious or out of order (see also *Node.java)
				// spurious is probably handled by the mostRecentlyCompleted mechanism
				LeafNode l = popCompleted(in.Address.Names[in.Address.Names.length - 1]);
				if (l != null)
				{
					System.out.println("uncomplete " + l.name);
					l.uncomplete();
					myEventMapper.uncompleteGoalNode(l.name);
					updateDebugFrame();
					System.out.println(getDebugSnapshot());
				}
			}

			// send message to Special.Disabled if app is trying to send a message to
			// a goalnode that is disabled.
			if (!toapp)
			{
				String disabledgn = getGoalNode(DorminAddress.SpecialDisabled);
				String nonexgn = getGoalNode(DorminAddress.SpecialNonExistent);
				// if there's a mapping set to receive this error message...
				if (disabledgn != null || nonexgn != null)
				{
					List<MappingAction> acts = masterMapping.get(false, name);
					// if the size of this list is greater than zero,
					// some inactive nodes are listed. (Otherwise, no mapping exists, so go on to send to Special.NonExistent.)
					if (acts != null && acts.size() > 0)
					{
						if (disabledgn != null)
						{
							StringBuilder nodelist = new StringBuilder("|");
							for (MappingAction act: acts)
							{
								nodelist.append(act.GoalNodeName);
								nodelist.append("|");
							}
							DorminMessage dm = new DorminMessage("tutor." + tutorName + "." + disabledgn, DorminMessage.NoteValueSetVerb, nodelist.toString());
							ret.add(dm);
							return ret;
						}
					}
					else
					{
						// no mapping now or ever
						if (nonexgn != null)
						{
							// it's ugly to send appnode names to the cogmodel, but what else is there to send here?
							DorminMessage dm = new DorminMessage("tutor." + tutorName + "." + nonexgn, DorminMessage.NoteValueSetVerb, in.Address.toDottedString());
							ret.add(dm);
							return ret;
						}
					}
				}
			}
			return null;
		}
		else   // candidates.size() != 0
		{
			a = candidates.get(0);
			if (candidates.size() > 1 && !toapp && in.Verb.equals(DorminMessage.NoteValueSetVerb))
			{
				// check candidates answers, choosing the one that matches or the first one if none match
				// TODO: it's not nice for this code to be looking at answers on goalnodes...
				for (MappingAction candidate: candidates)
				{
					XPSTTutorEngine engine = (XPSTTutorEngine)myEventMapper.tutorEngine;
					Type answer = (Type)engine.tutor.getGoalNode(candidate.GoalNodeName).getProperty("answer");
					if (answer.check(engine.tutor.getGoalNodes(), (String)in.Parameters[0].Value))
					{
						a = candidate;
						break;
					}
				}
			}
		}
		
		if (!toapp && a.Options.focusedOnly && !name.equals(myEventMapper.getFocusedNode()))
			return null;
		
		if (toapp && a.Options.noQIV && in.Verb.equals(DorminMessage.QueryInitialValue))
			return null;

		a.map(toapp, in, ret, tutorName);
		
		if (!toapp)
			lastActionToTutor = a;

		if (toapp)
		{
			// TODO: this will get trickier if there are multiple output messages or the mapping gets weirder
			if (in.Verb.equals(DorminMessage.ApproveVerb))
			{
				LeafNode l = getMappedLeafNode(a); 
				System.out.println("complete " + l.name);
				l.complete();
				myEventMapper.completeGoalNode(l.name);
				pushCompleted(l);
				updateDebugFrame();
				System.out.println(getDebugSnapshot());
			}
			else if (in.Verb.equals(DorminMessage.FlagVerb))
			{
				LeafNode l = getMappedLeafNode(a); 
				if (l.completed)
				{
					System.out.println("completed node flagged " + l.name);
					l.uncomplete();
					myEventMapper.uncompleteGoalNode(l.name);
					updateDebugFrame();
					System.out.println(getDebugSnapshot());
				}
			}

			// TODO: probably a better place to do this
			// messages sent out to the app may have event names (':click') appended. Strip these.
			for (DorminMessage dm: ret)
			{
				String lastname = dm.Address.Names[dm.Address.Names.length - 1];
				int colon = lastname.indexOf(':');
				if (colon != -1)
					dm.Address.Names[dm.Address.Names.length - 1] = lastname.substring(0, colon);
			}
		}
		else
		{
			// check for initial value messages
			// when an initial value is set, we check whether the answer is correct
			// if so, we send it along (as a NoteValueSet) to automatically complete the goalnode
			// otherwise, we drop it
			// TODO: again, this might not be quite the best place to do this.
			for (int ind = 0; ind < ret.size(); ind++)
			{
				DorminMessage dm = ret.get(ind);
				if (dm.Verb.equals(DorminMessage.NoteInitialValue))
				{
					org.clearsighted.tutorbase.TutorEngine te = myEventMapper.tutorEngine;
					boolean ivok = te.testAnswer(dm);
					if (ivok)
						dm.Verb = DorminMessage.NoteValueSetVerb;
					else
					{
						ret.remove(ind);
						ind--;
					}
				}
			}
		}
		return ret;
	}

	private ArrayList<DebugStyle> getDebugStyles()
	{
		ArrayList<DebugStyle> ret = new ArrayList<DebugStyle>();
		
		getDebugStyles(ret, root);

		Collections.sort(ret);
		return ret;
	}

	private void getDebugStyles(ArrayList<DebugStyle> ret, MappingNode rt)
	{
		if (rt.line > 0 && rt.column > 0)
		{
			String s = null;
			boolean en = myEventMapper.isGoalNodeEnabled(rt.name);
			if (rt.mapped)
			{
				if (en)
					s = DebugStyle.MAPPED_ENABLED_GOAL_NODE;
				else
					s = DebugStyle.MAPPED_DISABLED_GOAL_NODE;
			}
			else
			{
				if (en)
					s = DebugStyle.UNMAPPED_ENABLED_GOAL_NODE;
				else
					s = DebugStyle.UNMAPPED_DISABLED_GOAL_NODE;
			}
			ret.add(new DebugStyle(rt.line, rt.column, rt.name.length(), s));
		}
		if (rt.leftChild != null)
			getDebugStyles(ret, rt.leftChild);
		if (rt.rightChild != null)
			getDebugStyles(ret, rt.rightChild);
	}

	private void updateDebugFrame()
	{
		if (debugFrame != null)
			debugFrame.updateContents(script, getDebugStyles(), myEventMapper.getGoalNodes());
		if (webDebugFrame != null)
			webDebugFrame.updateContents(script, getDebugStyles(), myEventMapper.getGoalNodes());
	}

	private String getGoalNode(String name)
	{
		MappingAction a = null;
		a = getActiveAction(false, name);
		if (a == null)
			return null;
		return a.GoalNodeName;
	}

	public Mapping getOrCreateMapping(String name)
	{
		Mapping ret = mappings.get(name);
		if (ret == null)
		{
			ret = new Mapping();
			mappings.put(name, ret);
			orderedMappings.add(ret);
		}

		return ret;
	}

	public LeafNode createLeafNode(String name, int line, int col)
	{
		LeafNode ret = new LeafNode(this, getOrCreateMapping(name));
		// TODO: just for debugging
		ret.name = name;
		ret.line = line;
		ret.column = col;
		if (root == null)
			root = ret;
		leafNodes.add(ret);

		return ret;
	}

	public void finishConstruction()
	{
		for (Mapping m: orderedMappings)
			masterMapping.merge(m);
	}

	public void startSequence()
	{
		if (root != null)
			root.setMapped(true);
	}

	public void setTutorName(String tutorname)
	{
		tutorName = tutorname;
	}
	
	public String getTutorName()
	{
		return tutorName;
	}

	private void getDebugSnapshot(MappingNode node, StringBuilder sb)
	{
		String nv = node.toString();
		nv = nv.substring(nv.indexOf('@'));
		if (node instanceof LeafNode)
		{
			sb.append(((LeafNode)node).name);
			sb.append(nv);
			sb.append(':');
			sb.append(((LeafNode)node).mapped);
		}
		else
		{
			getDebugSnapshot(node.getLeftChild(), sb);
			sb.append(' ');
			if (node instanceof OrNode)
				sb.append("or");
			else if (node instanceof AndNode)
				sb.append("and");
			else if (node instanceof ThenNode)
				sb.append("then");
			sb.append(nv);
			sb.append(' ');
			getDebugSnapshot(node.getRightChild(), sb);
		}
	}

	public String getDebugSnapshot()
	{
		StringBuilder sb = new StringBuilder();
		getDebugSnapshot(root, sb);
		return sb.toString();
	}
	
	protected void onMappedStateChanged(LeafNode node, boolean mapped)
	{
		for (ITreeListener itl: listeners)
			itl.mappedStateChanged(node, mapped);
	}
	
	public void addListener(ITreeListener lnr)
	{
		listeners.add(lnr);
	}

	public boolean isDone()
	{
		return done;
	}

	public void setDone(boolean done)
	{
		this.done = done;
	}

	public void setScript(ArrayList<String> arrayList)
	{
		this.script = arrayList;
	}

	public void enabledChanged(String gnname, boolean enabled)
	{
		updateDebugFrame();
	}
}
