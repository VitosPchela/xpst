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

package org.clearsighted.tutorengine.types;

import java.util.regex.Pattern;

public class FilePathTypeCore
{
	private String ans = null;
	private Pattern ansPattern = null;
	private static final boolean IsWin = System.getProperty("os.name").toUpperCase().startsWith("WIN");
	
	private String makeRegex(String str)
	{
		return ".*" + str.replace("\\", "(\\\\|/)").replace("/", "(\\\\|/)").replace(".", "\\.").replace("*", "[^\\/.]*").replace("?", "[^\\/.]");
	}

	public FilePathTypeCore(String str)
	{
		ans = str;
		int flags = 0;
		if (IsWin)
			flags |= Pattern.CASE_INSENSITIVE;
		ansPattern = Pattern.compile(makeRegex(str), flags);
	}

	public String getRepresentativeAnswer()
	{
		return ans.replace("*", "").replace("?", "x");
	}
	
	public boolean check(String input)
	{
		return ansPattern.matcher(input).matches();
	}
}
