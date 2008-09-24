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

import antlr.MismatchedTokenException;
import antlr.NoViableAltException;
import junit.framework.TestCase;

public class ParseTest extends TestCase
{
	EMScriptLexer Lexer = null;
	EMScriptParser Parser = null;

	public static void main(String[] args)
	{
	}

	public ParseTest(String name)
	{
		super(name);
	}

	@Override
	protected void setUp() throws Exception
	{
		super.setUp();
	}

	@Override
	protected void tearDown() throws Exception
	{
		super.tearDown();
	}

	private final String rootdir = "src/org/clearsighted/tre/emscript/testscripts/";
	private void parse(String name) throws Exception
	{
		Object[] t = EMScript.createMappingAndTutor(rootdir + name);
		t = null;
	}

	public void testSimple0() throws Exception
	{
		parse("simple0.em");
	}
	
	public void testInclude0() throws Exception
	{
		parse("include0.em");
	}

	public void testSwitches0() throws Exception
	{
		parse("switches0.em");
	}

	public void testDefines0() throws Exception
	{
		parse("defines0.em");
	}

	public void testBadcomma0() throws Exception
	{
		boolean gotit = false;
		try
		{
			parse("badcomma0.em");
		}
		catch (NoViableAltException e)
		{
			if (e.column == 17 && e.line == 6 && e.getMessage().equals("unexpected token: Image"))
				gotit = true;
		}
		assertTrue("wanted parsing exception on comma", gotit);
	}

	public void testIncludeDefines0() throws Exception
	{
		parse("includedefines0.em");
	}

	public void testConcat0() throws Exception
	{
		parse("concat0.em");
	}

	public void testPriority0() throws Exception
	{
		parse("priority0.em");
	}
	
	public void testFocusedOnly0() throws Exception
	{
		parse("focusedonly0.em");
	}

	public void testFocusedOnly1() throws Exception
	{
		boolean gotit = false;
		try
		{
			parse("focusedonly1.em");
		}
		catch (MismatchedTokenException e)
		{
			if (e.column == 33 && e.line == 4)
				gotit = true;
		}
		assertTrue("wanted parsing exception on =", gotit);
	}

	public void testGroup0() throws Exception
	{
		parse("group0.em");
	}

	public void testGroup1() throws Exception
	{
		// TODO: this should fail, because you aren't allowed to nest groups.
		// I don't want to introduce that parser complexity right now, though.
		parse("group1.em");
	}
}
