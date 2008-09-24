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

import java.util.ArrayList;
import java.util.HashMap;
import java.util.Iterator;

// TODO: This whole debug thing shouldn't be implemented the way it is, it's a tad of a rush job to support the coggers...
public class WebDebugFrame
{
	public static String debugFrameContents = null;
	public void updateContents(ArrayList<String> script, ArrayList<DebugStyle> styles, HashMap<String, GoalNodeStatus> gns)
	{
		StringBuilder doc = new StringBuilder();
		doc.append("<div class=\"emdebug\" xmlns=\"http://www.w3.org/1999/xhtml\">\n");
		Iterator<DebugStyle> dsi = styles.iterator();
		DebugStyle nextstyle = null;
		if (dsi.hasNext())
			nextstyle = dsi.next();
		int row = 1, col = 1;
		for (String line: script)
		{
			while (nextstyle != null && nextstyle.row <= row)
			{
				doc.append(line.substring(col - 1, nextstyle.column - 1));
				col = nextstyle.column + nextstyle.length;
				doc.append("<span class=\"");
				doc.append(nextstyle.style);
				doc.append("\">");
				doc.append(line.substring(nextstyle.column - 1, col - 1));
				doc.append("</span>");
				if (dsi.hasNext())
					nextstyle = dsi.next();
				else
					nextstyle = null;
			}
			doc.append(line.substring(col - 1));
			doc.append("\n");
			row++;
			col = 1;
		}
		doc.append("</div>");
		debugFrameContents = doc.toString();
	}
}
