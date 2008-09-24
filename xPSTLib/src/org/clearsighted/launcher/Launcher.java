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

package org.clearsighted.launcher;

import java.io.File;
import java.io.FileInputStream;
import java.util.ArrayList;
import java.util.Properties;

import javax.xml.parsers.DocumentBuilder;
import javax.xml.parsers.DocumentBuilderFactory;
import javax.xml.parsers.ParserConfigurationException;

import org.w3c.dom.Document;
import org.w3c.dom.Element;

public class Launcher
{
	public static void main(String[] args)
	{
		String jarpath = "../../../../TRESim/lib/";
		String binpath = "../../../../TRESim/bin/";
		String port = "8000";
		String jre = "java.exe";
		boolean debug = true;
		boolean treonly = false, lms = false;
		String emfile = null;
		String trefile = null;
		String lmsfile = null;
		String[] tutorpaths = { "../../../../PaintDotNetTutor/tutor/", "../../../../../../tutorlink/PaintDotNetTutor/tutor/" };
		String tutorpath = null;
		String taskfilename = null;

		if (new File(tutorpaths[0] + "tasklist.xml").exists())
			tutorpath = tutorpaths[0];
		else
			tutorpath = tutorpaths[1];

		DocumentBuilderFactory dbf = DocumentBuilderFactory.newInstance();
		DocumentBuilder db = null;
		try
		{
			db = dbf.newDocumentBuilder();
		}
		catch (ParserConfigurationException e1)
		{
			// TODO Auto-generated catch block
			e1.printStackTrace();
		}

		int i = 0;
		while (i < args.length)
		{
			String arg = args[i];
			if (arg.equals("--tre"))
			{
				treonly = true;
				i++;
				taskfilename = args[i];
			}
			else
			{
				// assume it's an .lms file
				lms = true;
				lmsfile = arg;
				Properties p = new Properties();
				try
				{
					p.load(new FileInputStream(arg));
					int lessonid = Integer.parseInt((String)p.get("lessonid"));
					Document xmltasks = db.parse(new File(tutorpath, "tasklist.xml"));
					Element task = (Element)xmltasks.getElementsByTagName("Task").item(lessonid - 1);
					taskfilename = tutorpath + task.getAttribute("filename");
				}
				catch (Exception e)
				{
					// TODO Auto-generated catch block
					e.printStackTrace();
				}
			}

			i++;
		}
		
		try
		{
			Document xmltask = db.parse(taskfilename);
			Element taski = (Element)xmltask.getElementsByTagName("task").item(0);
			emfile = taski.getAttribute("em");
			trefile = taski.getAttribute("tre");
		}
		catch (Exception e)
		{
			// TODO Auto-generated catch block
			e.printStackTrace();
		}

		ArrayList<String> jreargs = new ArrayList<String>();
		
		jreargs.add(jre);
		jreargs.add("-cp");
		StringBuilder cp = new StringBuilder();
		String sep = System.getProperty("path.separator");
		cp.append(binpath);
		String[] jars = new String[] {"TREInterface.jar", "resources.jar", "XPlauncher.jar", "utilities.jar", "ui.jar", "tutors.jar", "communication.jar", "cl_common.jar", "LMSshared.jar", "aspectjrt.jar", "antlr.jar", "jai/jai_core.jar", "jai/jai_codec.jar", "jai/mlibwrapper_jai.jar"};
		for (String jar: jars)
		{
			cp.append(sep);
			cp.append(jarpath);
			cp.append(jar);
		}
		jreargs.add(cp.toString());
		int precln = jreargs.size();
		jreargs.add("org.clearsighted.tutorbase.Interface");
		jreargs.add("--port");
		jreargs.add(port);

		if (debug)
		{
			jreargs.add(precln, "-DLOGROOT=TEMP");
			jreargs.add(precln, "-DLOGFILE_NAME=CSTRE");
			jreargs.add(precln, "-Xrunjdwp:transport=dt_socket,address=8222,server=y,suspend=n");
			jreargs.add(precln, "-Xdebug");
			jreargs.add("--debug");
		}

		if (emfile.endsWith(".eme"))
		{
			jreargs.add("--eme");
			jreargs.add(tutorpath + emfile);
		}
		else
		{
			jreargs.add("--em");
			jreargs.add(tutorpath + emfile);
			jreargs.add("--tre");
			jreargs.add(tutorpath + trefile);
		}

		if (lms)
		{
			jreargs.add("--lms");
			jreargs.add(lmsfile);
		}
		
		try
		{
			Runtime.getRuntime().exec(jreargs.toArray(new String[0]));
		} catch (Exception e)
		{
			// TODO Auto-generated catch block
			e.printStackTrace();
		}

		if (!treonly)
		{
			ArrayList<String> pdnargs = new ArrayList<String>();
			pdnargs.add("PaintDotNet.exe");
			pdnargs.add("/tutor");
			pdnargs.add("/tutorargs=t:" + port + ",f:" + taskfilename);
			
			try
			{
				Runtime.getRuntime().exec(pdnargs.toArray(new String[0]));
			} catch (Exception e)
			{
				// TODO Auto-generated catch block
				e.printStackTrace();
			}
		}

//		JOptionPane.showMessageDialog(null, "");
	}
}
