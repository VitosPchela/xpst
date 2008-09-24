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

import java.io.BufferedInputStream;
import java.io.File;
import java.io.FileInputStream;
import java.io.FileNotFoundException;
import java.io.IOException;
import java.io.InputStream;
import java.io.OutputStream;
import java.net.HttpURLConnection;
import java.net.URL;
import java.util.Properties;

public class LMSLink
{
	private String username, pwhash, application;
	private int lessonID = 0;

	private LMSLink(String[] args)
	{
		try
		{
			Properties p = new Properties();
			p.load(new FileInputStream(args[0]));
			username = (String)p.get("username");
			pwhash = (String)p.get("pwhash");
			lessonID = Integer.parseInt((String)p.get("lessonid"));
			application = (String)p.get("application");
		} catch (FileNotFoundException e)
		{
			// TODO Auto-generated catch block
			e.printStackTrace();
		} catch (IOException e)
		{
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
	}

	private void submit(String result) throws Exception
	{
		URL serverURL = new URL("http://its.clearsighted.org/lms/submitresults.php");
		HttpURLConnection connection = (HttpURLConnection)serverURL.openConnection();
		connection.setRequestMethod("POST");
		connection.setDoOutput(true);
		connection.setDoInput(true);
		connection.connect();
		OutputStream out = connection.getOutputStream();
		out.write("username=".getBytes("UTF-8"));
		out.write(username.getBytes("UTF-8"));
		out.write("&password=".getBytes("UTF-8"));
		out.write(pwhash.getBytes("UTF-8"));
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

	private void go() throws Exception
	{
		final String[] tasks = { "", "1-1", "1-2", "1-3", "2-1" };
		String path = System.getProperty("user.dir") + "/../PaintDotNetTutor/src/bin/debug/";
		Process p = Runtime.getRuntime().exec(new String[] { path + "PaintDotNet.exe", "/tutor", "/tutorargs=t:Task" + tasks[lessonID] + "Cfg" }, null, new File(path));
	}

	public static void main(String[] args) throws Exception
	{
		LMSLink cl = new LMSLink(args);
		cl.go();
	}
}
