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
import java.io.File;
import java.io.FileNotFoundException;
import java.io.FileReader;
import java.io.IOException;
import java.util.ArrayList;

import org.clearsighted.tutorbase.emscript.mappingtree.Tree;


import antlr.ASTFactory;
import antlr.RecognitionException;
import antlr.TokenStreamException;

public class EMScript
{
	private static File getFile(String fromfname, String fname)
	{
		File dir = null;
		if (fromfname != null)
			dir = new File(fromfname).getParentFile();
		return new File(dir, fname);
	}

	private static ArrayList<String> getFileContents(String fromfname, String fname) throws IOException
	{
		String sep = System.getProperty("line.separator");
		ArrayList<String> ret = new ArrayList<String>();
		BufferedReader br = new BufferedReader(new FileReader(getFile(fromfname, fname)));
		String line;
		while ((line = br.readLine()) != null)
		{
			final int tabsize = 8;
			// expand tabs
			// TODO: better solution for column numbers
			StringBuilder sb = new StringBuilder();
			int col = 0, pos = 0;
			while (pos < line.length())
			{
				int tabpos = line.indexOf('\t', pos);
				if (tabpos == -1)
				{
					sb.append(line.substring(pos));
					pos = line.length();
					col += line.length() - pos;
				}
				else
				{
					sb.append(line.substring(pos, tabpos));
					col += tabpos - pos;
					pos = tabpos;
					final String spcs = "                 ";
					sb.append(spcs.substring(0, tabsize - (col % tabsize)));
					pos++;
					col += tabsize - (col % tabsize);
				}
			}
			ret.add(sb.toString());
		}
		br.close();
		return ret;
	}

	/**
	 * @param args
	 * @throws TokenStreamException 
	 * @throws RecognitionException 
	 * @throws FileNotFoundException 
	 */
	public static EMScriptParser parse(String fromfname, String fname) throws RecognitionException, TokenStreamException, IOException
	{
		EMScriptLexer esl = null;
		esl = new EMScriptLexer(new FileReader(getFile(fromfname, fname)));

		EMScriptParser esp = new EMScriptParser(esl);
		ASTFactory af = new ASTFactory();
		af.setASTNodeClass(EMAST.class);
		esp.setASTFactory(af);

		esp.setFilename(getFile(fromfname, fname).getAbsolutePath());
		esp.top();
		return esp;
	}
	
	public static Object[] createMappingAndTutor(String fname) throws RecognitionException, TokenStreamException, IOException
	{
		EMScriptParser p = parse(null, fname);
		EMScriptTreeParser tp = new EMScriptTreeParser();
		Object[] ret = tp.createMappingAndTutor(p.getAST());
		((Tree)ret[0]).setScript(getFileContents(null, fname));
		return ret;
	}
}
