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

package org.clearsighted.xpstengine;

import java.util.HashMap;
import java.util.List;

import org.clearsighted.tutorbase.MessageLogger;
import org.clearsighted.tutorbase.TutorEngine;
import org.clearsighted.tutorbase.TutorException;
import org.clearsighted.tutorbase.dormin.DorminAdapter;
import org.clearsighted.tutorbase.dormin.DorminMessage;
import org.clearsighted.tutorbase.dormin.DorminReceiver;
import org.clearsighted.tutorbase.emscript.EMScript;
import org.clearsighted.tutorbase.emscript.mappingtree.Tree;
import org.clearsighted.tutorbase.stockeventmappers.EMScriptEventMapper;
import org.clearsighted.tutorengine.Tutor;

import antlr.RecognitionException;


public class XPSTTutorEngine extends TutorEngine
{
	private DorminAdapter adapter;
	private Tutor tutor;
	
	@Override
	public Object getRepresentativeValue(String gnname)
	{
		return tutor.getGoalNode(gnname).getRepresentativeValue();
	}

	@Override
	public boolean testAnswer(DorminMessage dm)
	{
		String gnname = dm.Address.Names[2];
		return tutor.getGoalNode(gnname).check(dm.Parameters[0].Value);
	}

	@Override
	public void start(HashMap<String, String> properties, DorminAdapter adapter, List<DorminReceiver> premappertoolrecs, List<DorminReceiver> postmappertoolrecs, List<DorminReceiver> premappertutorrecs, List<DorminReceiver> postmappertutorrecs) throws TutorException
	{
		this.adapter = adapter;
		Object[] t = null;
		try
		{
			t = EMScript.createMappingAndTutor(properties.get("emfile"));
		}
		catch (Exception e)
		{
			throw new TutorException(e.toString(), e);
		}
		Tree tree = (Tree)t[0];
		tutor = (org.clearsighted.tutorengine.Tutor)t[1];
		setEventMapper(new EMScriptEventMapper(tree, tree.getTutorName()));
		setTutorName("emetutor");
		getEventMapper().tutorEngine = this;
	
		adapter.addReceiver(getEventMapper().getToTutorHalf());
		getEventMapper().getToAppHalf().addReceiver(adapter);
		getEventMapper().getToTutorHalf().addReceiver(tutor);
		tutor.addReceiver(getEventMapper().getToAppHalf());
	
		if (premappertoolrecs != null)
		{
			for (DorminReceiver dr: premappertoolrecs)
				tutor.addReceiver(dr);
		}
		if (postmappertoolrecs != null)
		{
			for (DorminReceiver dr: postmappertoolrecs)
				getEventMapper().getToAppHalf().addReceiver(dr);
		}
		if (premappertutorrecs != null)
		{
			for (DorminReceiver dr: premappertutorrecs)
				adapter.addReceiver(dr);
		}
		if (postmappertutorrecs != null)
		{
			for (DorminReceiver dr: postmappertutorrecs)
				getEventMapper().getToTutorHalf().addReceiver(dr);
		}
	
		adapter.start();
	}

	@Override
	public void waitUntilDone()
	{
		MessageLogger loginstance = MessageLogger.getInstance();

		adapter.waitUntilDone();
		loginstance.close();
	}
}
