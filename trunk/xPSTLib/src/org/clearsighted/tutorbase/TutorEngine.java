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

	/** Start sending and receiving tutor messages. Runs on a separate thread until waitUntilDone is called */
	abstract public void start(HashMap<String, String> properties, DorminAdapter adapter, List<DorminReceiver> premappertoolrecs, List<DorminReceiver> postmappertoolrecs, List<DorminReceiver> premappertutorrecs, List<DorminReceiver> postmappertutorrecs) throws TutorException;
	/** Stop tutor thread and wait until done. */
	abstract public void waitUntilDone();
	// TODO: probably don't want to take a DorminMessage argument here
	/** Test given message to see if it is the correct answer for the goalnode. Doesn't modify any state. */
	abstract public boolean testAnswer(DorminMessage dm);
	/** Get a value that, when sent as an input, would complete the goalnode.
	 * Usually, this is some value that matches, but in the case of types where it is too difficult to guess
	 * a correct answer (like for general RegEx), it can be a sentinel value that is always correct.
	 * @param gnname
	 * @return a correct value, or a sentinel
	 */
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
