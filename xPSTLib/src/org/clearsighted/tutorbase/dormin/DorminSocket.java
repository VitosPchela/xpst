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

package org.clearsighted.tutorbase.dormin;

import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStream;
import java.io.InputStreamReader;
import java.io.OutputStream;
import java.io.UnsupportedEncodingException;
import java.net.InetAddress;
import java.net.InetSocketAddress;
import java.net.ServerSocket;
import java.net.Socket;

import org.clearsighted.tresim.App;

/**
 * Runs a socket that sends and receives Dormin messages.
 *
 */
public class DorminSocket extends DorminAdapter
{
	LoopThread Loop;
	boolean Server;
	int Port;

	public DorminSocket(boolean server, int port)
	{
		Server = server;
		Port = port;
	}
	
	@Override
	public void receive(DorminMessage dmsg)
	{
		String msg = dmsg.toString();
		try
		{
			Loop.OStream.write((msg + "\n").getBytes("UTF-8"));
		} catch (UnsupportedEncodingException e)
		{
			// TODO Auto-generated catch block
			e.printStackTrace();
		} catch (IOException e)
		{
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
	}
	
	private class LoopThread extends Thread
	{
		ServerSocket ServerSock = null;
		Socket Sock = null;
		InputStream IStream = null;
		OutputStream OStream = null;
		int Port;
		boolean Server;

		public LoopThread(boolean server, int port)
		{
			Port = port;
			Server = server;
		}

		private void connect()
		{
			try
			{
				if (Server)
				{
					ServerSock = new ServerSocket(Port);
					Sock = ServerSock.accept();
				}
				else
				{
					Sock = new Socket();
					Sock.connect(new InetSocketAddress(InetAddress.getByName("localhost"), Port));
				}
				IStream = Sock.getInputStream();
				OStream = Sock.getOutputStream();
			}
			catch (Exception e)
			{
				// TODO Auto-generated catch block
				e.printStackTrace();
			}
		}

		@Override
		public void run()
		{
			try
			{
				BufferedReader br = new BufferedReader(new InputStreamReader(IStream, "UTF-8"));
				String line = null;
				while ((line = br.readLine()) != null)
				{
					DorminMessage dm = DorminMessage.createFromString(line);
					if (dm.Verb.equals(DorminMessage.StopTREVerb))
					{
						br.close();
						Sock.close();
						// TODO: better mechanism needed here
						if (App.TheApp != null)
							App.TheApp.dispose();
						break;
					}
					sendOut(dm);
				}
			}
			catch (Exception e)
			{
				e.printStackTrace();
			}
		}
	}

	private void loop(boolean server, int port)
	{
		Loop = new LoopThread(server, port);
		Loop.connect();
		Loop.start();
	}

	@Override
	public void start()
	{
		loop(true, Port);
	}

	public void connect(String addr, int port)
	{
		loop(false, port);
	}
	
	public void joinThread()
	{
		try
		{
			Loop.join();
		}
		catch (InterruptedException e)
		{
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
	}

	@Override
	public void waitUntilDone()
	{
		joinThread();
	}
}
