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

package org.clearsighted.tutorbase.stockeventmappers;

import java.util.List;
import java.util.Map;

import org.clearsighted.tutorbase.EventMapper;
import org.clearsighted.tutorbase.LogWrapper;
import org.clearsighted.tutorbase.dormin.DorminAddress;
import org.clearsighted.tutorbase.dormin.DorminMessage;
import org.clearsighted.tutorbase.emscript.EMScript;
import org.clearsighted.tutorbase.emscript.mappingtree.ITreeListener;
import org.clearsighted.tutorbase.emscript.mappingtree.LeafNode;
import org.clearsighted.tutorbase.emscript.mappingtree.Tree;
import org.clearsighted.tutorbase.emscript.mappingtree.Tree.MappingSummaryNode;


public class EMScriptEventMapper extends EventMapper implements ITreeListener
{
	private Tree MTree;

	public EMScriptEventMapper(String scriptfilename, String tutorname)
	{
		try
		{
			MTree = (Tree)(EMScript.createMappingAndTutor(scriptfilename))[0];
			MTree.setEventMapper(this);
			MTree.addListener(this);
			MTree.setTutorName(tutorname);
		}
		catch (Exception e)
		{
			e.printStackTrace();
			LogWrapper.log(e);
		}
	}

	public EMScriptEventMapper(Tree mtree, String tutorname)
	{
		try
		{
			MTree = mtree;
			MTree.setEventMapper(this);
			MTree.addListener(this);
			MTree.setTutorName(tutorname);
		}
		catch (Exception e)
		{
			LogWrapper.log(e);
		}
	}

	@Override
	public void map(boolean toapp, DorminMessage dm)
	{
		super.map(toapp, dm);
		List<DorminMessage> tosend = MTree.map(toapp, dm);
		if (tosend != null)
		{
			for (DorminMessage msg : tosend)
				sendOut(toapp, msg);
		}
	}

	public void queryInitialValue(String nodename)
	{
		DorminMessage dm = new DorminMessage();
		dm.Verb = DorminMessage.QueryInitialValue;
		String names[] = new String[] { DorminMessage.ToolNode, MTree.getTutorName(), nodename };
		dm.Address = new DorminAddress(names);
		map(true, dm);
	}

	public void mappedStateChanged(LeafNode node, boolean mapped)
	{
		// if a node is mapped and enabled, send query initial value
		if (node.isMapped() && isGoalNodeEnabled(node.name))
			queryInitialValue(node.name);
	}
	
	@Override
	protected void onEnabledChanged(String gnname, boolean enabled)
	{
		super.onEnabledChanged(gnname, enabled);
		// if a node is mapped and enabled, send query initial value
		if (MTree.isGoalNodeMapped(gnname) && isGoalNodeEnabled(gnname))
			queryInitialValue(gnname);
	}
	
	public Map<String, MappingSummaryNode> getMappingSummary()
	{
		return MTree.getMappingSummary();
	}

	@Override
	public void start()
	{
		MTree.startSequence();
	}
}
