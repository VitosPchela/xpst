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
 * Servlet implementation class for Servlet: GamexPST
 * 
 */
public class GamexPST extends javax.servlet.http.HttpServlet implements javax.servlet.Servlet
{
	private static String additionalDir = "/var/local/GamexPST/";
	private class InterfaceObjs
	{
		TutorEngine tutorEngine;
		DorminBuffer adapter;
		List<GameLoggerRecord> loggerBuffer;
		long startTime;
		String treName, emFile, treFile, task;
		Logger studentLog;
	};
	private static final long serialVersionUID = 2124975208978530881L;
	private Map<String, InterfaceObjs> treInterfaces = new HashMap<String, InterfaceObjs>();
	private static Logger logger = Logger.getLogger(GamexPST.class.getName());

	/*
	 * (non-Java-doc)
	 * 
	 * @see javax.servlet.http.HttpServlet#HttpServlet()
	 */
	public GamexPST()
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
	}
}