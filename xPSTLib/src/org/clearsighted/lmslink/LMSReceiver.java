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

package org.clearsighted.lmslink;

import java.io.BufferedInputStream;
import java.io.InputStream;
import java.io.OutputStream;
import java.net.HttpURLConnection;
import java.net.URL;

import org.clearsighted.tutorbase.dormin.DorminMessage;
import org.clearsighted.tutorbase.dormin.DorminReceiver;


public class LMSReceiver implements DorminReceiver
{
	private boolean toApp;
	private String username, password;
	int lessonID;
	
	public LMSReceiver(boolean toapp, String uname, String pword, int lessonid)
	{
		toApp = toapp;
		username = uname;
		password = pword;
		lessonID = lessonid;
	}

	public void receive(DorminMessage msg)
	{
		if (toApp && msg.Address.toDottedString().equals("TutorLink.Done") && msg.Verb.equals(DorminMessage.ApproveVerb))
			submit("1");
	}

	private void submit(String result)
	{
		try
		{
			URL serverURL = new URL("http://its.clearsighted.org/lms/submitresults.php");
			HttpURLConnection connection = (HttpURLConnection) serverURL.openConnection();
			connection.setRequestMethod("POST");
			connection.setDoOutput(true);
			connection.setDoInput(true);
			connection.connect();
			OutputStream out = connection.getOutputStream();
			out.write("username=".getBytes("UTF-8"));
			out.write(username.getBytes("UTF-8"));
			out.write("&password=".getBytes("UTF-8"));
			out.write(password.getBytes("UTF-8"));
			out.write("&lesson=".getBytes("UTF-8"));
			out.write(String.valueOf(lessonID).getBytes("UTF-8"));
			out.write("&results=".getBytes("UTF-8"));
			out.write(String.valueOf(result).getBytes("UTF-8"));
			InputStream in = new BufferedInputStream(connection.getInputStream());
			int character = in.read();
			while (character != -1)
			{
				System.out.print((char) character);
				character = in.read();
			}
		}
		catch (Exception e)
		{
			e.printStackTrace();
		}
	}
}
