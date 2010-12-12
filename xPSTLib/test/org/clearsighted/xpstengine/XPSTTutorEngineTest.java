package org.clearsighted.xpstengine;

import static org.junit.Assert.assertEquals;

import java.io.BufferedReader;
import java.io.FileReader;
import java.util.HashMap;

import org.clearsighted.tutorbase.EventMapper;
import org.clearsighted.tutorbase.HalfEventMapper;
import org.clearsighted.tutorbase.dormin.DorminBuffer;
import org.clearsighted.tutorbase.dormin.DorminMessage;
import org.junit.Before;
import org.junit.Test;

public class XPSTTutorEngineTest
{
	final String scriptDir = "test/org/clearsighted/xpstengine/testscripts/";

	@Before public void resetMessageNumber()
	{
		DorminMessage.resetMessageNum();
	}

	private void testEngine(String xpstName, String scenarioName) throws Exception
	{
		XPSTTutorEngine engine = new XPSTTutorEngine();
		HashMap<String, String> properties = new HashMap<String, String>();
		properties.put("emfile", String.format("%s%s.xpst", scriptDir, xpstName));
		DorminBuffer buffer = new DorminBuffer();
		engine.start(properties, buffer, null, null, null, null);
		engine.getEventMapper().start();

		EventMapper em = engine.getEventMapper();
		HalfEventMapper totut = em.getToTutorHalf();

		BufferedReader br = new BufferedReader(new FileReader(String.format("%s%s_%s_in.txt", scriptDir, xpstName, scenarioName)));
		String l = null;
		while ((l = br.readLine()) != null)
		{
			DorminMessage dm = DorminMessage.createFromString(l);
			totut.receive(dm);
		}
		br.close();

		String[] outMessages = buffer.getPendingMessages();

		for (String outMessage: outMessages)
			System.out.println(outMessage);

		int lno = 0;
		br = new BufferedReader(new FileReader(String.format("%s%s_%s_out.txt", scriptDir, xpstName, scenarioName)));
		while ((l = br.readLine()) != null)
		{
			assertEquals(l, outMessages[lno]);
			lno++;
		}
		assertEquals(lno, outMessages.length);
		br.close();
	}
	
	@Test public void testSimpleApprove() throws Exception
	{
		testEngine("simple", "approve");
	}

	@Test public void testSimpleFlag() throws Exception
	{
		testEngine("simple", "flag");
	}

	@Test public void testSimpleOOOrder() throws Exception
	{
		testEngine("simple", "ooorder");
	}
	
	@Test public void testSplitAppnodeFirstBranch() throws Exception
	{
		testEngine("split_appnode", "first_branch");
	}

	@Test public void testSplitAppnodeSecondBranch() throws Exception
	{
		testEngine("split_appnode", "second_branch");
	}
}
