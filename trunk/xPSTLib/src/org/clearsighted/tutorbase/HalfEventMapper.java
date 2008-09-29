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

package org.clearsighted.tutorbase;

import java.util.ArrayList;

import org.clearsighted.tutorbase.dormin.DorminList;
import org.clearsighted.tutorbase.dormin.DorminMessage;
import org.clearsighted.tutorbase.dormin.DorminReceiver;
import org.clearsighted.tutorbase.dormin.DorminSender;


public class HalfEventMapper implements DorminReceiver, DorminSender
{
	protected boolean ToApp;
	protected ArrayList<DorminReceiver> Receivers;
	private EventMapper Mapper;

	protected HalfEventMapper(boolean toapp, EventMapper mapper)
	{
		ToApp = toapp;
		Receivers = new ArrayList<DorminReceiver>();
		Mapper = mapper;
	}

	public void addReceiver(DorminReceiver receiver)
	{
		Receivers.add(receiver);
	}

	public void receive(DorminMessage dm)
	{
		if (!ToApp && dm.Verb.equals(DorminMessage.NoteAvailableNodesVerb))
		{
			String[] nodes = ((String)dm.Parameters[0].Value).split(",");
			ArrayList<String> alnodes = new ArrayList<String>();
			for (String node: nodes)
				alnodes.add(node);
			Mapper.setAppNodes(alnodes);
			return;
		}

		if (!ToApp && dm.Verb.equals(DorminMessage.NoteValueSetVerb))
		{
			String name = dm.Address.toDottedString();
			if (name.endsWith(":focus"))
			{
				boolean hasfocus = !((Integer)(dm.Parameters[0].Value)).equals(0);
				if (hasfocus)
					Mapper.focusedNode = name.substring(0, name.length() - 6);
				// TODO: we're going with sloppy focus because of event order issues. Is this a good thing?
//				else
//					Mapper.focusedNode = null;
			}
		}

		if (ToApp && dm.Verb.equals(DorminMessage.CreateVerb))
		{
			if (dm.Parameters[0].Value.equals("Skillometer"))
				return;

			DorminList propnames = (DorminList)dm.Parameters[0].Value;
			DorminList propvalues = (DorminList)dm.Parameters[1].Value;

			for (int i = 0; i < propnames.Items.length; i++)
			{
				if (((String)propnames.Items[i]).equals("goalnodes"))
				{
					DorminList nodes = (DorminList)propvalues.Items[i];
					for (int j = 0; j < nodes.Items.length; j++)
					{
						String nodename = (String)nodes.Items[j];
						Mapper.getGoalNodes().put(nodename, new GoalNodeStatus(true));
						Mapper.onEnabledChanged(nodename, true);
					}
				}
			}
		}
		
		if (ToApp && dm.Verb.equals(DorminMessage.SetPropertyVerb))
		{
			String nodename = dm.Address.toDottedString();
			// chop off 'tool' and tutor name
			nodename = nodename.substring(nodename.indexOf('.', nodename.indexOf('.') + 1) + 1);
			DorminList propnames = (DorminList)dm.Parameters[0].Value;
			DorminList propvalues = (DorminList)dm.Parameters[1].Value;

			for (int i = 0; i < propnames.Items.length; i++)
			{
				if (((String)propnames.Items[i]).equals("isenabled"))
				{
					String val = (String)propvalues.Items[i];
					boolean enabled = val.toLowerCase().equals("true");
					GoalNodeStatus stat = Mapper.getGoalNodes().get(nodename);
					stat.enabled = enabled;
					Mapper.onEnabledChanged(nodename, enabled);
				}
			}
		}
		
		Mapper.map(ToApp, dm);
	}

	public void receiveNoMapping(DorminMessage dm)
	{
		for (DorminReceiver dr: Receivers)
			dr.receive(dm);
	}
}
