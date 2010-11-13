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

package org.clearsighted.tutorengine;

import java.io.StringReader;
import java.util.HashMap;
import java.util.LinkedList;
import java.util.regex.Matcher;
import java.util.regex.Pattern;

import org.clearsighted.tutorbase.dormin.DorminAddress;
import org.clearsighted.tutorbase.dormin.DorminList;
import org.clearsighted.tutorbase.dormin.DorminMessage;
import org.clearsighted.tutorbase.dormin.DorminParameter;
import org.clearsighted.tutorbase.dormin.DorminReceiver;
import org.clearsighted.tutorbase.dormin.DorminSender;
import org.clearsighted.tutorbase.emscript.EMScriptLexer;
import org.clearsighted.tutorbase.emscript.EMScriptParser;
import org.clearsighted.tutorbase.emscript.EMScriptTreeParser;
import org.clearsighted.tutorbase.emscript.exprtree.ExprEnv;
import org.clearsighted.tutorbase.emscript.exprtree.ExprNode;

public class Tutor implements DorminReceiver, DorminSender
{
	private LinkedList<DorminReceiver> receivers = new LinkedList<DorminReceiver>();
	private HashMap<String, GoalNode> goalnodes = new HashMap<String, GoalNode>();
	private HashMap<String, Object> constants = new HashMap<String, Object>();
	
	private String emFile = null;

	public String getEmFile()
	{
		return emFile;
	}
 
	public void setEmFile(String emFile)
	{
		this.emFile = emFile;
	}

	public GoalNode getGoalNode(String name)
	{
		return goalnodes.get(name);
	}
	
	public HashMap<String, GoalNode> getGoalNodes()
	{
		return goalnodes;
	}

	public GoalNode getOrCreateGoalNode(String name)
	{
		if (!goalnodes.containsKey(name))
			goalnodes.put(name, new GoalNode());
		return goalnodes.get(name);
	}

	private String runExpr(GoalNode gn, String str, Object val)
	{
		EMScriptLexer esl = new EMScriptLexer(new StringReader(str));
		EMScriptParser esp = new EMScriptParser(esl);
		EMScriptTreeParser est = new EMScriptTreeParser();
		Object ret = null;
		try
		{
			esp.valexpr();
			ExprNode n = est.valexpr(esp.getAST());
			ExprEnv ee = new ExprEnv();
			ee.putAll(gn.getProperties());
			ee.putAll(constants);
			ee.put("v", val);
			ret = n.eval(ee,goalnodes);
		}
		catch (Exception e)
		{
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
		
		return ret.toString();
	}

	private String runExprs(GoalNode gn, String str, Object val)
	{
		final Pattern brs = Pattern.compile("\\{.*?\\}");
		int i = 0;
		boolean more = true;

		StringBuilder sb = new StringBuilder();
		Matcher m = brs.matcher(str);
		while (more)
		{
			if (m.find(i))
			{
				sb.append(str.substring(i, m.start()));
				sb.append(runExpr(gn, str.substring(m.start() + 1, m.end()), val));
				i = m.end();
			}
			else
				more = false;
		}
		sb.append(str.substring(i));
		
		return sb.toString();
	}

	public void receive(DorminMessage msg)
	{
		GoalNode gn = null;
		if (msg.Address.Names != null)
		{
			String node = msg.Address.Names[msg.Address.Names.length - 1];
			if (goalnodes.containsKey(node))
				gn = goalnodes.get(node);
		}
		if (msg.Verb.equals(DorminMessage.GetHintVerb))
		{
			LinkedList<String> hintl = gn.getHints();
			String[] hinta = new String[hintl.size()];
			for (int i = 0; i < hintl.size(); i++)
				hinta[i] = runExprs(gn, hintl.get(i), null);
			DorminMessage outmsg = new DorminMessage();
			outmsg.Verb = DorminMessage.HintMessageVerb;
			String[] names = new String[] { "tool", msg.Address.Names[1], msg.Address.Names[2] };
			outmsg.Address = new DorminAddress(names);
			outmsg.Parameters = new DorminParameter[1];
			DorminList dl = new DorminList(hinta);
			outmsg.Parameters[0] = new DorminParameter("MESSAGE", dl);
			sendOut(outmsg);
		}
		else if (msg.Verb.equals(DorminMessage.NoteValueSetVerb))
		{
			Object val = msg.Parameters[0].Value;
			DorminMessage outmsg = new DorminMessage();
			
			String l_response = val.toString();
			String checktypeName = "";
			Object answer = gn.getProperty("answer");
			if (answer != null)
			{
				checktypeName = answer.getClass().getSimpleName();
				if( checktypeName.equals("IsNLP") || checktypeName.equals("IsNotNLP") )
				{
					SynchFile file = new SynchFile("C:/xPST/xSTATCorpus.txt");
					try {
						file.write(l_response, emFile);
						file.close();
					} catch (Exception e) {
						//e.printStackTrace();
					}
				}
			}	
				
				
			if (gn.check(goalnodes,val))
			{		
				gn.setAnsString(val.toString());
				LinkedList<String> oncompl = gn.getOnCompletes();
				String[] oncompa = new String[oncompl.size()];
				for (int i = 0; i < oncompl.size(); i++)
					oncompa[i] = runExprs(gn, oncompl.get(i), null);
				outmsg.Verb = DorminMessage.ApproveVerb;
				String[] names = new String[] { "tool", msg.Address.Names[1], msg.Address.Names[2] };
				outmsg.Address = new DorminAddress(names);
				outmsg.Parameters = new DorminParameter[1];
				DorminList dl = new DorminList(oncompa);
				outmsg.Parameters[0] = new DorminParameter("MESSAGE", dl);
				sendOut(outmsg);
			}
			else
			{
				outmsg.Verb = DorminMessage.FlagVerb;
				String[] names = new String[] { "tool", msg.Address.Names[1], msg.Address.Names[2] };
				outmsg.Address = new DorminAddress(names);
				sendOut(outmsg);
				String jit = null;
				if ((jit = gn.checkJITs(val,goalnodes)) != null)
				{
					outmsg.Verb = DorminMessage.JITMessageVerb;
					outmsg.Parameters = new DorminParameter[1];
					outmsg.Parameters[0] = new DorminParameter("MESSAGE", new DorminList(new Object[] {runExprs(gn, jit, val)}));
					sendOut(outmsg);
				}
			}
		}
	}

	public void addConstant(String name, String value)
	{
		constants.put(name, value);
	}

	public void addReceiver(DorminReceiver receiver)
	{
		receivers.add(receiver);
	}
	
	private void sendOut(DorminMessage msg)
	{
		for (DorminReceiver dr: receivers)
			dr.receive(msg);
	}
}
