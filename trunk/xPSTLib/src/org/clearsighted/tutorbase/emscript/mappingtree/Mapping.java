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
import java.util.HashMap;
import java.util.List;

public class Mapping
{
	private HashMap<String, List<MappingAction>> AppNodeHash = new HashMap<String, List<MappingAction>>();
	private HashMap<String, List<MappingAction>> GoalNodeHash = new HashMap<String, List<MappingAction>>();

	public void merge(Mapping i)
	{
		for (String k: i.AppNodeHash.keySet())
		{
			// TODO: there better be only one action per list in i, but should we throw an error here if not?
			if (!AppNodeHash.containsKey(k))
				AppNodeHash.put(k, new ArrayList<MappingAction>());
			AppNodeHash.get(k).add(i.AppNodeHash.get(k).get(0));
		}
		for (String k: i.GoalNodeHash.keySet())
		{
			// TODO: there better be only one action per list in i, but should we throw an error here if not?
			if (!GoalNodeHash.containsKey(k))
				GoalNodeHash.put(k, new ArrayList<MappingAction>());
			GoalNodeHash.get(k).add(i.GoalNodeHash.get(k).get(0));
		}
	}

	protected void putAppNodeMapping(String appnodename, MappingAction act)
	{
		if (!AppNodeHash.containsKey(appnodename))
			AppNodeHash.put(appnodename, new ArrayList<MappingAction>());
		AppNodeHash.get(appnodename).add(act);
	}

	protected void putGoalNodeMapping(String goalnodename, MappingAction act)
	{
		if (!GoalNodeHash.containsKey(goalnodename))
			GoalNodeHash.put(goalnodename, new ArrayList<MappingAction>());
		GoalNodeHash.get(goalnodename).add(act);
	}

	public RenameAction putRenameAction(RuleOptions options, String appnodename, String goalnodename)
	{
		RenameAction m = new RenameAction(options, this, goalnodename);
		m.AppNodeName = appnodename;
		
		putGoalNodeMapping(goalnodename, m);
		putAppNodeMapping(appnodename, m);
		return m;
	}

	public SwitchAction putSwitchAction(RuleOptions options, String goalnodename)
	{
		SwitchAction m = new SwitchAction(options, this, goalnodename);

		putGoalNodeMapping(goalnodename, m);
		return m;
	}

	public ConcatAction putConcatAction(RuleOptions options, String goalnodename)
	{
		ConcatAction m = new ConcatAction(options, this, goalnodename);

		putGoalNodeMapping(goalnodename, m);
		return m;
	}

	public List<MappingAction> get(boolean toapp, String nodename)
	{
		return toapp ? GoalNodeHash.get(nodename) : AppNodeHash.get(nodename);
	}

	public boolean contains(MappingAction action)
	{
		// TODO: speed this up
		for (List<MappingAction> lma: AppNodeHash.values())
		{
			if (lma.contains(action))
				return true;
		}
		return false;
	}

	public HashMap<String, List<MappingAction>> getAppNodeHash()
	{
		return AppNodeHash;
	}
}
