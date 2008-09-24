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

import java.util.HashMap;
import java.util.List;

import org.clearsighted.tutorbase.dormin.DorminAddress;
import org.clearsighted.tutorbase.dormin.DorminMessage;
import org.clearsighted.tutorbase.dormin.DorminParameter;


public class SwitchAction extends MappingAction
{
	private HashMap<String, String> Hash = new HashMap<String, String>();
	private String LastAppNodeSending = null, FirstAppNodeRegistered = null;

	public SwitchAction(RuleOptions options, Mapping mymapping, String goalnodename)
	{
		super(options, mymapping, goalnodename);
	}
	
	public void putValuePair(RuleOptions options, String appnodename, String value)
	{
		if (FirstAppNodeRegistered == null)
			FirstAppNodeRegistered = appnodename;
		Hash.put(appnodename, value);
		MyMapping.putAppNodeMapping(appnodename, this);
	}

	@Override
	public void map(boolean toapp, DorminMessage in, List<DorminMessage> ret, String tutorname)
	{
		DorminMessage msg = new DorminMessage(in);
		if (toapp)
		{
			if (msg.Verb.equals(DorminMessage.ApproveVerb))
			{
				for (String s: Hash.keySet())
				{
					DorminMessage nmsg = new DorminMessage(in);
					nmsg.Address = new DorminAddress(s);
					ret.add(nmsg);
				}
				return;
			}
			else
			{
				if (LastAppNodeSending != null)
					msg.Address = new DorminAddress(LastAppNodeSending);
				else
					msg.Address = new DorminAddress(FirstAppNodeRegistered);
			}
		}
		else
		{
			boolean todo = false;
			if (in.Parameters.length > 0)
			{
				Object o = in.Parameters[0].Value;
				if (o instanceof String)
				{
					if (!(o.equals("") || o.equals("0")))
						todo = true;
				}
				else if (o instanceof Integer)
				{
					if (((Integer)o).intValue() != 0)
						todo = true;
				}
			}
			if (todo)
			{
				String appnodename = in.Address.toDottedString();
				msg.Address = new DorminAddress("tutor." + tutorname + "." + GoalNodeName);
				msg.Parameters = new DorminParameter[] { new DorminParameter(DorminParameter.ValueName, Hash.get(appnodename)) };
				LastAppNodeSending = appnodename;
			}
			else
				return;
		}
		ret.add(msg);
	}
}
