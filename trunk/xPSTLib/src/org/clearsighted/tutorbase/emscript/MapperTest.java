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

package org.clearsighted.tutorbase.emscript;

import java.io.BufferedReader;
import java.io.FileReader;
import java.util.Vector;

import org.clearsighted.tutorbase.HalfEventMapper;
import org.clearsighted.tutorbase.dormin.DorminMessage;
import org.clearsighted.tutorbase.dormin.DorminReceiver;
import org.clearsighted.tutorbase.stockeventmappers.EMScriptEventMapper;


import junit.framework.TestCase;

public class MapperTest extends TestCase
{
	private final class TestReceiver
	{
		public Vector<String> Lines = new Vector<String>();
		public TestReceiverHalf ToApp = new TestReceiverHalf(true);
		public TestReceiverHalf ToTut = new TestReceiverHalf(false);
		private final class TestReceiverHalf implements DorminReceiver
		{
			public TestReceiverHalf(boolean toapp)
			{
				ToApp = toapp;
			}
			public boolean ToApp;
			public void receive(DorminMessage msg)
			{
				Lines.add((ToApp ? "A" : "T") + "|M|" + msg.toString());
			}
		}
	}

	@Override
	protected void setUp() throws Exception
	{
		super.setUp();
		DorminMessage.resetMessageNum();
	}

	@Override
	protected void tearDown() throws Exception
	{
		super.tearDown();
	}

	private final String rootdir = "src/org/clearsighted/tre/emscript/";
	private void testMap(String fname) throws Exception
	{
		String line = null;
		BufferedReader br = new BufferedReader(new FileReader(rootdir + "testscripts/" + fname + "in.txt"));
		TestReceiver testrec = new TestReceiver();
		EMScriptEventMapper emem = new EMScriptEventMapper(rootdir + "testscripts/" + fname + ".em", "PDN");
		HalfEventMapper toapp = emem.getToAppHalf(), totut = emem.getToTutorHalf();
		toapp.addReceiver(testrec.ToApp);
		totut.addReceiver(testrec.ToTut);
		while ((line = br.readLine()) != null)
		{
			String[] cols = line.split("\\|");
			if ((cols[0].equals("T") || cols[0].equals("A")) && cols[1].equals("R"))
			{
				DorminMessage dm = DorminMessage.createFromString(cols[2]);
				if (cols[0].equals("T"))
					totut.receive(dm);
				else if (cols[0].equals("A"))
					toapp.receive(dm);
			}
		}
		br.close();

		int lno = 0;
		br = new BufferedReader(new FileReader(rootdir + "testscripts/" + fname + "out.txt"));
		while ((line = br.readLine()) != null)
		{
			assertEquals(line, testrec.Lines.get(lno));
			lno++;
		}
		assertEquals(lno, testrec.Lines.size());
		br.close();
	}

	public void testMap0() throws Exception
	{
		testMap("map0");
	}

	public void testMap1() throws Exception
	{
		testMap("map1");
	}

	public void testMapConcat0() throws Exception
	{
		testMap("mapconcat0");
	}

	public void testMapPrio0() throws Exception
	{
		testMap("mapprio0");
	}

	public void testMapFocusedOnly0() throws Exception
	{
		testMap("mapfocusedonly0");
	}

	public void testMapUncomp0() throws Exception
	{
		testMap("mapuncomp0");
	}

	public void testMapUntil0() throws Exception
	{
		testMap("mapuntil0");
	}

	public void testMapUntil1() throws Exception
	{
		testMap("mapuntil1");
	}
}
