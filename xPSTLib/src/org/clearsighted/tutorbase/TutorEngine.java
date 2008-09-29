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

import java.util.HashMap;
import java.util.List;

import org.clearsighted.tutorbase.dormin.DorminAdapter;
import org.clearsighted.tutorbase.dormin.DorminMessage;
import org.clearsighted.tutorbase.dormin.DorminReceiver;

/**
 * Base class for tutor engine implementations. It would be rare to want to extend this; instead,
 * you'd probably extend the capabilities of the XPSTTutorEngine.
 * @see org.clearsighted.xpstengine.XPSTTutorEngine
 */
public abstract class TutorEngine
{
	private EventMapper eventMapper = null;
	private String tutorName = null;

	abstract public void start(HashMap<String, String> properties, DorminAdapter adapter, List<DorminReceiver> premappertoolrecs, List<DorminReceiver> postmappertoolrecs, List<DorminReceiver> premappertutorrecs, List<DorminReceiver> postmappertutorrecs) throws TutorException;
	abstract public void waitUntilDone();
	// TODO: probably don't want to take a DorminMessage argument here
	abstract public boolean testAnswer(DorminMessage dm);
	abstract public Object getRepresentativeValue(String gnname);

	public void setEventMapper(EventMapper eventMapper)
	{
		this.eventMapper = eventMapper;
	}

	public EventMapper getEventMapper()
	{
		return eventMapper;
	}

	public void setTutorName(String tutorName)
	{
		this.tutorName = tutorName;
	}

	public String getTutorName()
	{
		return tutorName;
	}
}
