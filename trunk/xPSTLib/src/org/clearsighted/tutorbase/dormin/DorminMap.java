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

package org.clearsighted.tutorbase.dormin;

import java.util.Map;
import java.util.Map.Entry;

public class DorminMap
{
	// this needs to be set in maps for Structured Student Input
	public static final String DisplayString = "display-string";

	public Map<String, Object> Items;
	public DorminMap(Map<String, Object> items)
	{
		Items = items;
	}

	@Override
	public String toString()
	{
		if (Items == null)
			return "[]";

		StringBuilder sb = new StringBuilder();
		sb.append('[');
		for (Entry<String, Object> ent: Items.entrySet())
		{
			sb.append('(');
			String key = ent.getKey();
			sb.append(key);
			sb.append(": ");
			Object item = ent.getValue();
			sb.append(item);
			sb.append(')');
			sb.append(',');
		}
		sb.deleteCharAt(sb.length() - 1);
		sb.append(']');
		return sb.toString();
	}
}
