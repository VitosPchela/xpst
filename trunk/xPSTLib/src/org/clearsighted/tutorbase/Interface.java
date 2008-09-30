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

import java.io.FileInputStream;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Properties;
import java.util.logging.Logger;

import org.clearsighted.lmslink.LMSReceiver;
import org.clearsighted.tresim.App;
import org.clearsighted.tutorbase.dormin.DorminAdapter;
import org.clearsighted.tutorbase.dormin.DorminReceiver;
import org.clearsighted.tutorbase.dormin.DorminSocket;

/**
 * Holds some functions to help embed xPSTLib in an application. launchEngine is the most useful one. See WebxPST in WebxPSTServer for an example of using this class.
 *
 */
public class Interface
{
	private TutorEngine tutorEngine = null;

	public TutorEngine getTutorEngine()
	{
		return tutorEngine;
	}

	/**
	 * instantiate an interface with an optional java.util.logging.Logger for logging
	 * @param lgr the Java logger, or null to use the CLI logger
	 */
	public Interface(Logger lgr, String tutorengineclass)
	{
		LogWrapper.setJavaLogger(lgr);
		try
		{
			tutorEngine = (TutorEngine)Class.forName(tutorengineclass).getConstructor((Class<?>[])null).newInstance((Object[])null);
		}
		catch (Exception e)
		{
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
	}

	/**
	 * Launches a tutor engine, hooking up some debugging and logging stuff at the same time. See WebxPST in WebxPSTServer for an example of using this function.
	 * @param ds
	 * @param logger
	 * @param logfile
	 * @param trefile
	 * @param emfile
	 * @param lmsfile
	 * @param debug
	 * @param premappertoolrecs
	 * @param postmappertoolrecs
	 * @param premappertutorrecs
	 * @param postmappertutorrecs
	 * @return a running TutorEngine
	 * @throws Exception
	 */
	public static TutorEngine launchEngine(DorminAdapter ds, Logger logger, String logfile, String trefile, String emfile, String lmsfile, boolean debug, List<DorminReceiver> premappertoolrecs, List<DorminReceiver> postmappertoolrecs, List<DorminReceiver> premappertutorrecs, List<DorminReceiver> postmappertutorrecs) throws Exception
	{
		if (premappertoolrecs == null)
			premappertoolrecs = new ArrayList<DorminReceiver>();
		if (postmappertoolrecs == null)
			postmappertoolrecs = new ArrayList<DorminReceiver>();
		if (premappertutorrecs == null)
			premappertutorrecs = new ArrayList<DorminReceiver>();
		if (postmappertutorrecs == null)
			postmappertutorrecs = new ArrayList<DorminReceiver>();

		MessageLogger loginstance = MessageLogger.getInstance();
		loginstance.open(logfile);

		Interface i = null;
		if (emfile != null && emfile.endsWith(".xpst"))
			i = new Interface(null, "org.clearsighted.xpstengine.XPSTTutorEngine");
		else
			i = new Interface(null, "org.clearsighted.cliengine.CLITutorEngine");
		
		if (debug)
		{
			App app = new App();
			DorminReceiver[] loggers = app.getLoggers();
			premappertoolrecs.add(loggers[0]);
			postmappertoolrecs.add(loggers[1]);
			premappertutorrecs.add(loggers[2]);
			postmappertutorrecs.add(loggers[3]);
		}
		
		if (lmsfile != null)
		{
			Properties p = new Properties();
			p.load(new FileInputStream(lmsfile));
			String username = (String)p.get("username");
			String password = (String)p.get("pwhash");
			int lessonid = Integer.parseInt((String)p.get("lessonid"));
			premappertutorrecs.add(new LMSReceiver(false, username, password, lessonid));
			postmappertoolrecs.add(new LMSReceiver(true, username, password, lessonid));
		}

		HashMap<String, String> props = new HashMap<String, String>();
		props.put("trefile", trefile);
		props.put("emfile", emfile);
		i.tutorEngine.start(props, ds, premappertoolrecs, postmappertoolrecs, premappertutorrecs, postmappertutorrecs);
		i.tutorEngine.getEventMapper().start();

		if (!debug)
		{
			i.tutorEngine.waitUntilDone();
			loginstance.close();
		}
		return i.tutorEngine;
	}

	public static void main(String[] args)
	{
		try
		{
			boolean debug = false;
			int port = 8000;
			String trefile = null, emfile = null, logfile = null, lmsfile = null;

			int argn = 0;
			while (argn < args.length)
			{
				String arg = args[argn];
				if (arg.equals("--debug"))
				{
					argn += 1;
					debug = true;
				}
				else if (arg.equals("--lms"))
				{
					argn += 1;
					lmsfile = args[argn];
					argn += 1;
				}
				else if (arg.equals("--tre"))
				{
					argn += 1;
					trefile = args[argn];
					argn += 1;
				}
				else if (arg.equals("--em"))
				{
					argn += 1;
					emfile = args[argn];
					argn += 1;
				}
				else if (arg.equals("--port"))
				{
					argn += 1;
					port = Integer.parseInt(args[argn]);
					argn += 1;
				}
				else if (arg.equals("--log"))
				{
					argn += 1;
					logfile = args[argn];
					argn += 1;
				}
				else
					argn += 1;
			}
			DorminSocket ds = new DorminSocket(true, port);
			launchEngine(ds, null, logfile, trefile, emfile, lmsfile, debug, null, null, null, null);
		}
		catch (Exception e)
		{
			e.printStackTrace();
		}
	}
}
