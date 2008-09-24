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

public class DebugStyle implements Comparable<DebugStyle>
{
	int row, column, length;
	String style;
	public static final String MAPPED_ENABLED_GOAL_NODE = "MappedEnabledGoalNode";
	public static final String MAPPED_DISABLED_GOAL_NODE = "MappedDisabledGoalNode";
	public static final String UNMAPPED_ENABLED_GOAL_NODE = "UnmappedEnabledGoalNode";
	public static final String UNMAPPED_DISABLED_GOAL_NODE = "UnmappedDisabledGoalNode";
	public int compareTo(DebugStyle r)
	{
		if (row < r.row)
			return -1;
		else if (row > r.row)
			return 1;
		else if (column < r.column)
			return -1;
		else if (column > r.column)
			return 1;
		else
			return 0;
	};
	
	public DebugStyle(int r, int c, int l, String s)
	{
		row = r;
		column = c;
		length = l;
		style = s;
	}
}