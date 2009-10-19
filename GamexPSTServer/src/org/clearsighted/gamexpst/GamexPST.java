package org.clearsighted.gamexpst;

import java.io.File;
import java.io.FileInputStream;
import java.io.FileNotFoundException;
import java.io.FilenameFilter;
import java.io.IOException;
import java.io.InputStream;
import java.io.Writer;
import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.Comparator;
import java.util.Date;
import java.util.HashMap;
import java.util.List;
import java.util.Map;
import java.util.Properties;
import java.util.UUID;
import java.util.Map.Entry;
import java.util.logging.Logger;

import java.io.IOException;
import javax.servlet.ServletException;
import javax.servlet.http.HttpServlet;
import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;

import org.clearsighted.tutorbase.Interface;
import org.clearsighted.tutorbase.TutorEngine;
import org.clearsighted.tutorbase.dormin.DorminBuffer;
import org.clearsighted.tutorbase.dormin.DorminReceiver;
import org.clearsighted.gamexpst.GameLogger;
import org.clearsighted.gamexpst.GamexPST;
import org.clearsighted.gamexpst.GameLoggerRecord;

/**
 * Servlet implementation class GamexPST
 */
public class GamexPST extends HttpServlet {
	private static final long serialVersionUID = 1L;
	private class InterfaceObjs
	{
		TutorEngine tutorEngine;
		DorminBuffer adapter;
		List<GameLoggerRecord> loggerBuffer;
		long startTime;
		String treName, emFile, treFile, task;
		Logger studentLog;
	};
	private Map<String, InterfaceObjs> treInterfaces = new HashMap<String, InterfaceObjs>();
	private static Logger logger = Logger.getLogger(GamexPST.class.getName());

       
    /**
     * @see HttpServlet#HttpServlet()
     */
    public GamexPST() {
        super();
        // TODO Auto-generated constructor stub
    }

	/**
	 * @see HttpServlet#doGet(HttpServletRequest request, HttpServletResponse response)
	 */
	protected void doGet(HttpServletRequest request, HttpServletResponse response) throws ServletException, IOException {
		// TODO Auto-generated method stub
		System.out.println("Get method is invoked");

	}

	private Properties getSomeTREProperties(String dir)
	{
		Properties p = new Properties();
		try
		{
			File[] propsfiles = new File(dir).listFiles(new FilenameFilter()
			{
				public boolean accept(File dir, String name)
				{
					return name.endsWith(".properties");
				};
			});
			Properties q = new Properties();
			if (propsfiles != null)
			{
				for (File propsfile : propsfiles)
				{
					FileInputStream fis = new FileInputStream(propsfile);
					q.load(fis);
					fis.close();
					Properties r = new Properties();
					for (Entry e: q.entrySet())
					{
						if (((String)e.getKey()).endsWith("file"))
							r.put(e.getKey(), dir + "/" + ((String)e.getValue()));
					}
					p.putAll(q);
					p.putAll(r);
				}
			}
		}
		catch (FileNotFoundException e)
		{
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
		catch (IOException e)
		{
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
		return p;
	}

	private Properties getTREProperties()
	{
		Properties p = new Properties();
		String basedir = getBasedir();
		logger.info("basedir: " + basedir);
		p.putAll(getSomeTREProperties(basedir));
		return p;
	}

	private String getBasedir()
	{
		String basedir = getServletContext().getRealPath("/") + "/WEB-INF/tre/";
		return basedir;
	}
	/**
	 * @see HttpServlet#doPost(HttpServletRequest request, HttpServletResponse response)
	 */
	protected void doPost(HttpServletRequest request, HttpServletResponse response) throws ServletException, IOException {
		// TODO Auto-generated method stub
		System.out.println("Post method is invoked.");
		String url = request.getPathInfo();
		if (url == null || url.length() == 0)
		{
			String command = request.getParameter("command");
			System.out.println(command);
			if (command.equals("starttre"))
			{
				response.setContentType("text/plain");
				logger.info("starttre command");
				String taskname = request.getParameter("task");
				logger.info("task: " + taskname);
				Properties p = getTREProperties();

				String emfile = null;
				String snullem = request.getParameter("nullem");
				boolean nullem = snullem != null && snullem.equals("1");
				if (!nullem)
					emfile = (String) p.get("tre.task." + taskname + ".emfile");
				logger.info("emfile: " + emfile);

				String trefile = null;
				trefile = (String) p.get("tre.task." + taskname + ".trefile");
				logger.info("trefile: " + trefile);

				String trename = null;
				trename = UUID.randomUUID().toString();
				logger.info("trename: " + trename);
				InterfaceObjs io = new InterfaceObjs();
				io.adapter = new DorminBuffer();
				io.loggerBuffer = new ArrayList<GameLoggerRecord>();
				io.startTime = System.currentTimeMillis();
				io.treName = trename;
				io.emFile = emfile;
				io.treFile = trefile;
				io.task = taskname;

				List<DorminReceiver> premappertoolloggers = new ArrayList<DorminReceiver>();
				if (!nullem)
					premappertoolloggers.add(new GameLogger(true, false, io.loggerBuffer));
				List<DorminReceiver> postmappertoolloggers = new ArrayList<DorminReceiver>();
				postmappertoolloggers.add(new GameLogger(true, true, io.loggerBuffer));
				List<DorminReceiver> premappertutorloggers = new ArrayList<DorminReceiver>();
				if (!nullem)
					premappertutorloggers.add(new GameLogger(false, false, io.loggerBuffer));
				List<DorminReceiver> postmappertutorloggers = new ArrayList<DorminReceiver>();
				postmappertutorloggers.add(new GameLogger(false, true, io.loggerBuffer));

				String basedir = getBasedir();
				TutorEngine engine = null;
				String errmsg = "";
				try
				{
					engine = Interface.launchEngine(io.adapter, logger, null, trefile, emfile, null, false, premappertoolloggers, postmappertoolloggers, premappertutorloggers, postmappertutorloggers);
				}
				catch (Exception e)
				{
					e.printStackTrace();
					errmsg = e.getMessage();
					System.out.println(errmsg);
				}
				io.tutorEngine = engine;
				
				io.studentLog = Logger.getLogger("org.clearsighted.gamexpst.GamexPST.instance");
				io.studentLog.info(trename + ":START:" + taskname + ":" + request.getRemoteAddr());
				
				logger.info("launched TRE");
				treInterfaces.put(trename, io);
				response.getWriter().write(trename + " " + errmsg);
			}
			else if (command.equals("stoptre"))
			{
				response.setContentType("text/plain");
				logger.info("stoptre command");
				String trename = request.getParameter("trename");
				logger.info("Interface = " + treInterfaces.get(trename));
				treInterfaces.get(trename).studentLog.info(trename + ":STOP");
				treInterfaces.remove(trename);
			}
			else if (command.equals("getrepresentativevalue"))
			{
				response.setContentType("text/plain");
				String gnname = request.getParameter("goalnode");
				String trename = request.getParameter("trename");
				logger.info("getrepresentativevalue command: trename " + trename + ", goalnode " + gnname);
				String rv = treInterfaces.get(trename).tutorEngine.getRepresentativeValue(gnname).toString();
				response.getWriter().write(rv);
			}
			else if (command.equals("log"))
			{
				response.setContentType("text/plain");
				String trename = request.getParameter("trename");
				String msg = request.getParameter("msg");
				treInterfaces.get(trename).studentLog.info(trename + ":" + msg);
			}
		}
		else
		{
			response.setContentType("text/plain");
			String trename = request.getPathInfo().substring(1);
			logger.info("message to TRE " + trename + " (" + treInterfaces.get(trename) + ")");
			DorminBuffer db = treInterfaces.get(trename).adapter;
			InputStream instm = request.getInputStream();
			// TODO: allow arbitrary length inputs
			byte[] buf = new byte[4096];
			int rd = instm.read(buf);
			instm.close();
			String inmsgs = "";
			String[] inmsgarr = {};
			if (rd != -1)
			{
				inmsgs = new String(buf, 0, rd);
				inmsgarr = inmsgs.split("\\n");
			}
			logger.info("input messages: " + inmsgs);
			Logger stulog = treInterfaces.get(trename).studentLog;
			for (String s: inmsgarr)
				stulog.info(trename + ":IN:" + s);
			db.sendOut(inmsgarr);
			String[] resps = db.getPendingMessages();
			StringBuilder resp = new StringBuilder();
			for (String s : resps)
			{
				stulog.info(trename + ":OUT:" + s);
				resp.append(s);
				resp.append('\n');
			}
			logger.info("output messages: " + resp.toString());
			response.getWriter().write(resp.toString());
			
		}
		

	}

}
