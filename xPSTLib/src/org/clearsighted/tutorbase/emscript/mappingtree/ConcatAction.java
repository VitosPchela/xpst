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

import java.util.List;
import java.util.Vector;

import org.clearsighted.tutorbase.dormin.DorminAddress;
import org.clearsighted.tutorbase.dormin.DorminMessage;
import org.clearsighted.tutorbase.dormin.DorminParameter;


public class ConcatAction extends MappingAction
{
	private Vector<String> AppNodes = new Vector<String>();
	private Vector<String> Received = new Vector<String>();
	private String LastAppNodeSending = null, FirstAppNodeRegistered = null;
	
	public ConcatAction(RuleOptions options, Mapping mymapping, String goalnodename)
	{
		super(options, mymapping, goalnodename);
	}

	public void putAppNode(RuleOptions options, String appnodename)
	{
		AppNodes.add(appnodename);
		Received.add(null);
		MyMapping.putAppNodeMapping(appnodename, this);
		if (FirstAppNodeRegistered == null)
			FirstAppNodeRegistered = appnodename;
	}

	@Override
	public void map(boolean toapp, DorminMessage in, List<DorminMessage> ret, String tutorname)
	{
		DorminMessage msg = new DorminMessage(in);
		if (toapp)
		{
			if (msg.Verb.equals(DorminMessage.ApproveVerb))
			{
				for (String s: AppNodes)
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
			String appnodename = in.Address.toDottedString();
			String val = in.Parameters[0].Value.toString();

			boolean anynull = false;
			for (int i = 0; i < AppNodes.size(); i++)
			{
				if (AppNodes.get(i).equals(appnodename))
					Received.set(i, val);
				if (Received.get(i) == null)
					anynull = true;
			}

			if (anynull)
				return;
			else
			{
				StringBuilder newval = new StringBuilder();
				newval.append(Received.get(0));
				for (int i = 1; i < Received.size(); i++)
				{
					newval.append(',');
					newval.append(Received.get(i));
					// decided we want to send these whenever one value changes, not wait for all of them to accumulate again
					// Received.set(i, null);
				}
				msg.Address = new DorminAddress("tutor." + tutorname + "." + GoalNodeName);
				msg.Parameters = new DorminParameter[] { new DorminParameter(DorminParameter.ValueName, newval.toString()) };
				LastAppNodeSending = appnodename;
			}
		}
		ret.add(msg);
	}
}
