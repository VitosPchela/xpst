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

import org.clearsighted.tutorengine.Type;

/**
 * FilePath type, which is similar to command-line globbing.
 * xPST constructor takes one argument with *, ? interpreted as usual for globbing.
 * On Windows, matching is not case-sensitive. On any platform, slash and backslashes are
 * considered equivalent.
 * Split into this class and FilePathTypeCore for historical reuse purposes.
 */
public class FilePath extends Type
{
	private FilePathTypeCore core;
	@Override
	public void construct(Object[] args)
	{
		core = new FilePathTypeCore((String)args[0]);
	}

	@Override
	public boolean check(String value)
	{
		return core.check(value);
	}

	@Override
	public Object getRepresentativeValue()
	{
		return core.getRepresentativeAnswer();
	}

	@Override
	public String toString()
	{
		// TODO
		return "TODO: FilePath.toString";
	}
}
