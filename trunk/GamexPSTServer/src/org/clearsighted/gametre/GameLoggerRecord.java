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

package org.clearsighted.gametre;

import org.clearsighted.tutorbase.dormin.DorminMessage;

public class GameLoggerRecord
{
	public long time;
	public String friendlyMessage, fullMessage;
	public boolean toApp, mapped;

	public GameLoggerRecord(boolean itoapp, boolean imapped, DorminMessage imsg)
	{
		time = System.currentTimeMillis();
		toApp = itoapp;
		mapped = imapped;
		friendlyMessage = imsg.toFriendlyString();
		fullMessage = imsg.toString();
	}
}
