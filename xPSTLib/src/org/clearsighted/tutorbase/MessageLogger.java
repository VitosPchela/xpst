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

import java.io.FileNotFoundException;
import java.io.FileOutputStream;
import java.io.PrintWriter;

import org.clearsighted.tutorbase.dormin.DorminMessage;


/**
 * Logger for Dormin messages and event mapper trails.
 * output log format:
 * 00007234|D|A|R|SE/1.2&VERB...
 * Col 0: 8-character padded milliseconds from start
 * Col 1: D for Dormin, E for EventMapper
 * Col 2: A for to Application, T for to Tutor
 * Col 3: R for Raw, M for Mapped
 * Col 4: Dormin message string or EventMapper message
 *   (Col 4 is not escaped for |'s since the whole line can be parsed with fixed offsets).
 */
public class MessageLogger
{
	boolean Logging = false;
	String Filename;
	PrintWriter Writer;
	long InitTime;
	static MessageLogger Instance = null;
	
	public static MessageLogger getInstance()
	{
		if (Instance == null)
			Instance = new MessageLogger();
		return Instance;
	}

	public void open(String filename)
	{
		Filename = filename;
		Logging = (Filename != null);
		if (Logging)
		{
			InitTime = System.currentTimeMillis();
			try
			{
				Writer = new PrintWriter(new FileOutputStream(Filename, true));
			}
			catch (FileNotFoundException e)
			{
				// TODO Auto-generated catch block
				e.printStackTrace();
			}
		}
	}

	public void logDormin(boolean toapp, boolean raw, DorminMessage msg)
	{
		if (Logging)
		{
			StringBuilder sb = new StringBuilder();
			sb.append("D|");
			sb.append(toapp ? "A" : "T");
			sb.append("|");
			sb.append(raw ? "R" : "M");
			sb.append("|");
			sb.append(msg.toString());
			write(sb.toString());
		}
	}

	private void write(String line)
	{
		String time = Long.toString(System.currentTimeMillis() - InitTime);
		Writer.println("00000000".substring(time.length()) + time + "|" + line);
		Writer.flush();
	}

	public void close()
	{
		if (Writer != null)
		{
			Writer.close();
			Writer = null;
		}
	}
}
