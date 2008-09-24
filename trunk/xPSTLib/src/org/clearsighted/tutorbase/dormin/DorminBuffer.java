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

package org.clearsighted.tutorbase.dormin;

import java.util.ArrayList;

public class DorminBuffer extends DorminAdapter
{
	protected ArrayList<DorminMessage> bufferedMessages = new ArrayList<DorminMessage>();

	@Override
	public void receive(DorminMessage msg)
	{
		bufferedMessages.add(msg);
	}

	public void sendOut(String[] strmessages)
	{
		for (String strmessage: strmessages)
		{
			try
			{
				sendOut(DorminMessage.createFromString(strmessage));
			}
			catch (Exception e)
			{
				e.printStackTrace();
			}
		}
	}
	
	public String[] getPendingMessages()
	{
		int nummsg = bufferedMessages.size();
		String[] ret = new String[nummsg];
		for (int i = 0; i < nummsg; i++)
			ret[i] = bufferedMessages.get(i).toString();
		bufferedMessages.clear();
		return ret;
	}

	@Override
	public void start()
	{
	}

	@Override
	public void waitUntilDone()
	{
	}
}
