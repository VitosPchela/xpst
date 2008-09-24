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

/// <summary>
/// A list of items for a Dormin message.
/// </summary>
public class DorminList
{
	///
	public Object[] Items;
	///
	public DorminList(Object[] items)
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
		for (int i = 0; i < Items.length; i++)
		{
			Object item = Items[i];
			sb.append(item);
			if (i < Items.length - 1)
				sb.append(',');
		}
		sb.append(']');
		return sb.toString();
	}
}
