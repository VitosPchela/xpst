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

package org.clearsighted.tresim;

import java.text.SimpleDateFormat;
import java.util.Date;

import javax.swing.table.DefaultTableModel;

import org.clearsighted.tutorbase.dormin.DorminMessage;
import org.clearsighted.tutorbase.dormin.DorminReceiver;


/***
 * A DorminReceiver to plug in to log messages to a TableModel.
 * @author SDO
 *
 */
public class AppLogDorminReceiver implements DorminReceiver
{
	private static final long serialVersionUID = -4406680035310777134L;
	private DefaultTableModel TModel = null;
	private String Dir;
	
	protected AppLogDorminReceiver(String dir, DefaultTableModel tmodel)
	{
		Dir = dir;
		TModel = tmodel;
	}

	public void receive(DorminMessage msg)
	{
		if (TModel != null)
		{
			final SimpleDateFormat sdf = new SimpleDateFormat("HH:mm:ss.SSS");
			Date d = new Date(System.currentTimeMillis());
			TModel.addRow(new Object[] { sdf.format(d), Dir, msg.toFriendlyString(), msg.toString() });
		}
	}
}
