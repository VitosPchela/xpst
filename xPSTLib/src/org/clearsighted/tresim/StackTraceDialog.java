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

import java.io.PrintWriter;
import java.io.StringWriter;

import javax.swing.JFrame;
import javax.swing.JPanel;
import javax.swing.JDialog;
import javax.swing.JTextArea;
import javax.swing.BoxLayout;
import javax.swing.JScrollPane;

public class StackTraceDialog extends JDialog
{
	private static final long serialVersionUID = -2111587341724609897L;
	private JPanel jContentPane = null;
	private JScrollPane jScrollPane = null;
	private JTextArea StackTextArea = null;
	/**
	 * This is the default constructor
	 */
	public StackTraceDialog()
	{
		super();
		initialize();
	}

	public StackTraceDialog(Throwable t)
	{
		super((JFrame)null, true);
		initialize();
		StringWriter sw = new StringWriter();
		t.printStackTrace(new PrintWriter(sw));
		StackTextArea.setText(sw.toString());
	}
	/**
	 * This method initializes this
	 * 
	 * @return void
	 */
	private void initialize()
	{
		this.setSize(813, 593);
		this.setContentPane(getJContentPane());
	}

	/**
	 * This method initializes jContentPane
	 * 
	 * @return javax.swing.JPanel
	 */
	private JPanel getJContentPane()
	{
		if (jContentPane == null)
		{
			jContentPane = new JPanel();
			jContentPane.setLayout(new BoxLayout(getJContentPane(), BoxLayout.Y_AXIS));
			jContentPane.add(getJScrollPane(), null);
		}
		return jContentPane;
	}

	/**
	 * This method initializes jScrollPane	
	 * 	
	 * @return javax.swing.JScrollPane	
	 */
	private JScrollPane getJScrollPane()
	{
		if (jScrollPane == null)
		{
			jScrollPane = new JScrollPane();
			jScrollPane.setViewportView(getStackTextArea());
		}
		return jScrollPane;
	}

	/**
	 * This method initializes StackTextArea	
	 * 	
	 * @return javax.swing.JTextArea	
	 */
	private JTextArea getStackTextArea()
	{
		if (StackTextArea == null)
		{
			StackTextArea = new JTextArea();
		}
		return StackTextArea;
	}

}  //  @jve:decl-index=0:visual-constraint="10,10"
