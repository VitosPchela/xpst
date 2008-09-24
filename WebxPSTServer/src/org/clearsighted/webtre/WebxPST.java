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

package org.clearsighted.webtre;

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

import javax.servlet.ServletException;
import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;
import javax.xml.parsers.DocumentBuilder;
import javax.xml.parsers.DocumentBuilderFactory;
import javax.xml.transform.OutputKeys;
import javax.xml.transform.Transformer;
import javax.xml.transform.TransformerFactory;
import javax.xml.transform.dom.DOMSource;
import javax.xml.transform.stream.StreamResult;

import org.w3c.dom.Document;
import org.w3c.dom.Element;

import org.clearsighted.tutorbase.GoalNodeStatus;
import org.clearsighted.tutorbase.Interface;
import org.clearsighted.tutorbase.TutorEngine;
import org.clearsighted.tutorbase.WebDebugFrame;
import org.clearsighted.tutorbase.dormin.DorminBuffer;
import org.clearsighted.tutorbase.dormin.DorminReceiver;
import org.clearsighted.tutorbase.emscript.mappingtree.Tree.MappingSummaryNode;
import org.clearsighted.tutorbase.stockeventmappers.EMScriptEventMapper;

/**
 * Servlet implementation class for Servlet: WebxPST
 * 
 */
public class WebxPST extends javax.servlet.http.HttpServlet implements javax.servlet.Servlet
{
	private static String additionalDir = "/var/local/WebxPST/";
	private class InterfaceObjs
	{
		TutorEngine tutorEngine;
		DorminBuffer adapter;
		List<WebLoggerRecord> loggerBuffer;
		long startTime;
		String treName, emFile, treFile, task;
		Logger studentLog;
	};
	private static final long serialVersionUID = 2124975208978530881L;
	private Map<String, InterfaceObjs> treInterfaces = new HashMap<String, InterfaceObjs>();
	private static Logger logger = Logger.getLogger(WebxPST.class.getName());

	/*
	 * (non-Java-doc)
	 * 
	 * @see javax.servlet.http.HttpServlet#HttpServlet()
	 */
	public WebxPST()
	{
		super();
	}

	/*
	 * (non-Java-doc)
	 * 
	 * @see javax.servlet.http.HttpServlet#doGet(HttpServletRequest request, HttpServletResponse response)
	 */
	protected void doGet(HttpServletRequest request, HttpServletResponse response) throws ServletException, IOException
	{
		final SimpleDateFormat sdf = new SimpleDateFormat("yyyy-MMM-dd HH:mm:ss.SSS");
		Writer w = response.getWriter();
		String url = request.getPathInfo();
		if (url == null || url.length() == 0)
		{
			response.setContentType("text/html");
			w.write("<html><body><div>");
			w.write("Debug session:<br/>");
			InterfaceObjs[] iobs = treInterfaces.values().toArray(new InterfaceObjs[0]);
			final Comparator<InterfaceObjs> bystarttime = new Comparator<InterfaceObjs>()
			{
				public int compare(InterfaceObjs o1, InterfaceObjs o2)
				{
					return (int) (o2.startTime - o1.startTime);
				}
			};
			Arrays.sort(iobs, bystarttime);
			for (InterfaceObjs io : iobs)
			{
				w.write("<a href=\"debug.jsp?id=" + io.treName + "\">" + io.task + "</a> " + "(" + sdf.format(new Date(io.startTime)) + ", " + io.emFile + ", " + io.treFile + ", " + io.treName + ")<br/>");
			}
			w.write("<br/>Start test session:<br/>");
			Properties ps = getTREProperties();
			for (Entry p : ps.entrySet())
			{
				String key = (String) p.getKey();
				if (key.startsWith("tre.task.") && key.endsWith(".emfile"))
				{
					String taskname = key.substring(9, key.lastIndexOf('.'));
					w.write(taskname + ": <a href=\"gntest.jsp?task=" + taskname + "\">Goalnode test</a>");
					w.write(" <a href=\"appntest.jsp?task=" + taskname + "\">Appnode test</a><br/>");
				}
			}

			w.write("</div></body></html>");
		}
		else
		{
			try
			{
				String burl = url.substring(1);
				if (burl.endsWith("/log"))
				{
					response.setContentType("text/xml");
					int logpos = 0;
					String spos = request.getParameter("pos");
					if (spos != null)
						logpos = Integer.parseInt(spos);
					String guid = burl.substring(0, 36);
					System.setProperty("javax.xml.parsers.DocumentBuilderFactory", "com.sun.org.apache.xerces.internal.jaxp.DocumentBuilderFactoryImpl");
					DocumentBuilderFactory dbf = DocumentBuilderFactory.newInstance();
					DocumentBuilder db = null;
					db = dbf.newDocumentBuilder();
					Document d = db.newDocument();
					response.setContentType("text/xml");
					Element records = d.createElement("records");
					records.setAttribute("startpos", Integer.toString(logpos));
					InterfaceObjs iobs = treInterfaces.get(guid);
					for (int i = logpos; i < iobs.loggerBuffer.size(); i++)
					{
						WebLoggerRecord wlr = iobs.loggerBuffer.get(i);
						Element record = d.createElement("record");
						record.setAttribute("time", Long.toString(wlr.time - iobs.startTime));
						record.setAttribute("toapp", wlr.toApp ? "true" : "false");
						record.setAttribute("mapped", wlr.mapped ? "true" : "false");
						record.setAttribute("friendlymessage", wlr.friendlyMessage);
						record.setAttribute("fullmessage", wlr.fullMessage);
						records.appendChild(record);
					}
					d.appendChild(records);

					TransformerFactory tf = TransformerFactory.newInstance();
					Transformer t = tf.newTransformer();
					t.setOutputProperty(OutputKeys.INDENT, "yes");
					t.setOutputProperty("{http://xml.apache.org/xslt}indent-amount", "2");

					t.transform(new DOMSource(d), new StreamResult(w));
				}
				else if (burl.endsWith("/emdebug"))
				{
					response.setContentType("text/xml");
					w.write(WebDebugFrame.debugFrameContents);
				}
				else if (burl.endsWith("/goalnodes"))
				{
					response.setContentType("text/xml");
					String guid = burl.substring(0, 36);
					System.setProperty("javax.xml.parsers.DocumentBuilderFactory", "com.sun.org.apache.xerces.internal.jaxp.DocumentBuilderFactoryImpl");
					DocumentBuilderFactory dbf = DocumentBuilderFactory.newInstance();
					DocumentBuilder db = null;
					db = dbf.newDocumentBuilder();
					Document d = db.newDocument();
					response.setContentType("text/xml");
					Element goalnodes = d.createElement("goalnodes");
					InterfaceObjs io = treInterfaces.get(guid);
					HashMap<String, GoalNodeStatus> gns = io.tutorEngine.getEventMapper().getGoalNodes();
					for (String gn : gns.keySet())
					{
						Element goalnode = d.createElement("goalnode");
						goalnode.setAttribute("name", gn);
						goalnodes.appendChild(goalnode);
					}
					d.appendChild(goalnodes);

					TransformerFactory tf = TransformerFactory.newInstance();
					Transformer t = tf.newTransformer();
					t.setOutputProperty(OutputKeys.INDENT, "yes");
					t.setOutputProperty("{http://xml.apache.org/xslt}indent-amount", "2");

					t.transform(new DOMSource(d), new StreamResult(w));
				}
				else if (burl.endsWith("/appnodes"))
				{
					response.setContentType("text/xml");
					String guid = burl.substring(0, 36);
					System.setProperty("javax.xml.parsers.DocumentBuilderFactory", "com.sun.org.apache.xerces.internal.jaxp.DocumentBuilderFactoryImpl");
					DocumentBuilderFactory dbf = DocumentBuilderFactory.newInstance();
					DocumentBuilder db = null;
					db = dbf.newDocumentBuilder();
					Document d = db.newDocument();
					response.setContentType("text/xml");
					Element appnodes = d.createElement("appnodes");
					InterfaceObjs io = treInterfaces.get(guid);
					EMScriptEventMapper emem = (EMScriptEventMapper) io.tutorEngine.getEventMapper();
					Map<String, MappingSummaryNode> appns = emem.getMappingSummary();
					for (Entry<String, MappingSummaryNode> appn : appns.entrySet())
					{
						Element appnode = d.createElement("appnode");
						appnode.setAttribute("name", appn.getKey());
						String gn = appn.getValue().goalnode;
						if (gn != null)
						{
							appnode.setAttribute("goalnodename", gn);
							appnode.setAttribute("isnext", appn.getValue().isNextNode ? "true" : "false");
						}
						appnodes.appendChild(appnode);
					}
					d.appendChild(appnodes);

					TransformerFactory tf = TransformerFactory.newInstance();
					Transformer t = tf.newTransformer();
					t.setOutputProperty(OutputKeys.INDENT, "yes");
					t.setOutputProperty("{http://xml.apache.org/xslt}indent-amount", "2");

					t.transform(new DOMSource(d), new StreamResult(w));
				}
				else if (burl.endsWith("/tutorname"))
				{
					response.setContentType("text/plain");
					String guid = burl.substring(0, 36);
					InterfaceObjs io = treInterfaces.get(guid);
					String tutorname = io.tutorEngine.getTutorName();
					w.write(tutorname);
				}
				else
				{
					response.setContentType("text/html");
					w.write("<html><body><form action=\"" + burl + "\" method=\"POST\"><input type=\"text\" name=\"messages\"><input type=\"submit\" value=\"go\"></form></body></html>");
				}
			}
			catch (Exception e)
			{
				// TODO Auto-generated catch block
				e.printStackTrace();
			}
		}
	}

	private String getBasedir()
	{
		String basedir = getServletContext().getRealPath("/") + "/WEB-INF/tre/";
		return basedir;
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
		logger.info("additionaldir: " + additionalDir);
		p.putAll(getSomeTREProperties(basedir));
		p.putAll(getSomeTREProperties(additionalDir));
		return p;
	}

	/*
	 * (non-Java-doc)
	 * 
	 * @see javax.servlet.http.HttpServlet#doPost(HttpServletRequest request, HttpServletResponse response)
	 */
	protected void doPost(HttpServletRequest request, HttpServletResponse response) throws ServletException, IOException
	{
		String url = request.getPathInfo();
		if (url == null || url.length() == 0)
		{
			String command = request.getParameter("command");
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
				io.loggerBuffer = new ArrayList<WebLoggerRecord>();
				io.startTime = System.currentTimeMillis();
				io.treName = trename;
				io.emFile = emfile;
				io.treFile = trefile;
				io.task = taskname;

				List<DorminReceiver> premappertoolloggers = new ArrayList<DorminReceiver>();
				if (!nullem)
					premappertoolloggers.add(new WebLogger(true, false, io.loggerBuffer));
				List<DorminReceiver> postmappertoolloggers = new ArrayList<DorminReceiver>();
				postmappertoolloggers.add(new WebLogger(true, true, io.loggerBuffer));
				List<DorminReceiver> premappertutorloggers = new ArrayList<DorminReceiver>();
				if (!nullem)
					premappertutorloggers.add(new WebLogger(false, false, io.loggerBuffer));
				List<DorminReceiver> postmappertutorloggers = new ArrayList<DorminReceiver>();
				postmappertutorloggers.add(new WebLogger(false, true, io.loggerBuffer));

				String basedir = getBasedir();
//				i.launchTRE(basedir + trefile, extemfile, io.adapter, premappertoolloggers, postmappertoolloggers, premappertutorloggers, postmappertutorloggers);
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
				
				io.studentLog = Logger.getLogger("org.clearsighted.webtre.WebxPST.instance");
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