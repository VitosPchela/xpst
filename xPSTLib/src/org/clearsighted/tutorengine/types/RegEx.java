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

import java.util.HashMap;

import org.clearsighted.tutorengine.GoalNode;
import org.clearsighted.tutorengine.Type;

/**
 * Regular expression type. xPST constructor takes one argument in standard Java regex syntax.
 * Split into this class and RegExTypeCore for historical reuse purposes.
 */
public class RegEx extends Type
{
	private RegExTypeCore core;

	@Override
	public boolean check(HashMap<String, GoalNode> gns,String value)
	{
		return core.check(value);
	}

	@Override
	public void construct(Object[] args)
	{
		core = new RegExTypeCore((String)args[0]);
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
		return core.getRepresentativeAnswer();
	}
}
