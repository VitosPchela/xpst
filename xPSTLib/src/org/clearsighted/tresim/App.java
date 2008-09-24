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

import javax.swing.JFileChooser;
import javax.swing.JPanel;
import javax.swing.JFrame;
import javax.swing.JButton;
import javax.swing.LookAndFeel;
import javax.swing.UIManager;
import javax.swing.UnsupportedLookAndFeelException;
import javax.swing.WindowConstants;

import java.awt.GridBagLayout;
import java.awt.GridBagConstraints;
import java.io.FileNotFoundException;
import java.io.FileOutputStream;
import java.io.PrintWriter;
import java.util.Vector;

import javax.swing.JLabel;
import javax.swing.JTable;
import javax.swing.table.DefaultTableModel;
import javax.swing.JScrollPane;

import org.clearsighted.tutorbase.MessageLogger;
import org.clearsighted.tutorbase.dormin.DorminReceiver;

import com.sun.java.swing.plaf.windows.WindowsLookAndFeel;

public class App extends JFrame
{
	private static final long serialVersionUID = 5990515025627525379L;
	public static App TheApp = null;

	private JPanel jContentPane = null;

	private JScrollPane LogPane = null;

	private JTable LogTable = null;

	private DefaultTableModel LogTableModel = null;  //  @jve:decl-index=0:visual-constraint=""

	private JLabel jLabel = null;
	private JButton SaveLogButton = null;
	static final String SUBENTRIES = "SubEntries";  //  @jve:decl-index=0:
	static final String TEXT = "Text";  //  @jve:decl-index=0:

	/**
	 * This method initializes LogPane	
	 * 	
	 * @return javax.swing.JScrollPane	
	 */
	private JScrollPane getLogPane()
	{
		if (LogPane == null)
		{
			LogPane = new JScrollPane();
			LogPane.setViewportView(getLogTable());
		}
		return LogPane;
	}

	/**
	 * This method initializes SentTable	
	 * 	
	 * @return javax.swing.JTable	
	 */
	private JTable getLogTable()
	{
		if (LogTable == null)
		{
			LogTable = new JTable();
			LogTable.setAutoResizeMode(javax.swing.JTable.AUTO_RESIZE_OFF);
			LogTable.setModel(getLogTableModel());
			LogTable.getColumnModel().getColumn(0).setPreferredWidth(100);
			LogTable.getColumnModel().getColumn(1).setPreferredWidth(20);
			LogTable.getColumnModel().getColumn(2).setPreferredWidth(1000);
			LogTable.getColumnModel().getColumn(3).setPreferredWidth(2500);
		}
		return LogTable;
	}

	/**
	 * This method initializes SentTableModel	
	 * 	
	 * @return javax.swing.table.DefaultTableModel	
	 */
	public DefaultTableModel getLogTableModel()
	{
		if (LogTableModel == null)
		{
			LogTableModel = new DefaultTableModel();
			LogTableModel.setColumnCount(4);
			Vector<String> ci = new Vector<String>();
			ci.add("Time");
			ci.add("Dir");
			ci.add("Friendly Message");
			ci.add("Dormin Message");
			LogTableModel.setColumnIdentifiers(ci);
		}
		return LogTableModel;
	}

	/**
	 * This method initializes SaveLogButton	
	 * 	
	 * @return javax.swing.JButton	
	 */
	private JButton getSaveLogButton()
	{
		if (SaveLogButton == null)
		{
			SaveLogButton = new JButton();
			SaveLogButton.setText("Save Log...");
			SaveLogButton.addActionListener(new java.awt.event.ActionListener()
			{
				public void actionPerformed(java.awt.event.ActionEvent e)
				{
					JFileChooser jfc = new JFileChooser(System.getProperty("user.dir"));
					jfc.setDialogTitle("Save message log as");
					if (jfc.showSaveDialog(TheApp) == JFileChooser.APPROVE_OPTION)
					{
						try
						{
							PrintWriter pw = new PrintWriter(new FileOutputStream(jfc.getSelectedFile()));
							for (int i = 0; i < LogTableModel.getRowCount(); i++)
							{
								pw.printf("%s\t%s\t%s\t%s", LogTableModel.getValueAt(i, 0), LogTableModel.getValueAt(i, 1), LogTableModel.getValueAt(i, 2), LogTableModel.getValueAt(i, 3));
								pw.println();
							}
							pw.close();
						} catch (FileNotFoundException e1)
						{
							showStackTrace(e1);
						} 
					}
				}
			});
		}
		return SaveLogButton;
	}

	public App()
	{
		super();
		LookAndFeel laf = new WindowsLookAndFeel();
		try
		{
			UIManager.setLookAndFeel(laf);
		}
		catch (UnsupportedLookAndFeelException e)
		{
			// oh well
		}
		TheApp = this;
		initialize();
		setVisible(true);
		setDefaultCloseOperation(WindowConstants.DISPOSE_ON_CLOSE);
	}

	/**
	 * This method initializes this
	 * 
	 * @return void
	 */
	private void initialize()
	{
		this.setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
		this.setSize(1000, 539);
		this.setContentPane(getJContentPane());
		this.setTitle("TRESim");
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
			GridBagConstraints gridBagConstraints21 = new GridBagConstraints();
			gridBagConstraints21.gridx = 4;
			gridBagConstraints21.gridy = 11;
			jLabel = new JLabel();
			jLabel.setText("Value:");
			GridBagConstraints gridBagConstraints31 = new GridBagConstraints();
			gridBagConstraints31.fill = java.awt.GridBagConstraints.BOTH;
			gridBagConstraints31.gridy = 10;
			gridBagConstraints31.weightx = 1.0;
			gridBagConstraints31.weighty = 1.0;
			gridBagConstraints31.gridx = 0;
			gridBagConstraints31.gridwidth = 5;
			jContentPane = new JPanel();
			jContentPane.setLayout(new GridBagLayout());
			jContentPane.add(getLogPane(), gridBagConstraints31);
			jContentPane.add(getSaveLogButton(), gridBagConstraints21);
		}
		return jContentPane;
	}

	public DorminReceiver[] getLoggers()
	{
		DorminReceiver[] loggers = new DorminReceiver[4];
		loggers[0] = new AppLogDorminReceiver(">M", App.TheApp.getLogTableModel());
		loggers[1] = new AppLogDorminReceiver(">", App.TheApp.getLogTableModel());
		loggers[2] = new AppLogDorminReceiver("<M", App.TheApp.getLogTableModel());
		loggers[3] = new AppLogDorminReceiver("<", App.TheApp.getLogTableModel());
		return loggers;
	}

	private void showStackTrace(Exception ef)
	{
		StackTraceDialog std = new StackTraceDialog(ef);
		std.setVisible(true);
	}

	@Override
	public void dispose()
	{
		super.dispose();
		MessageLogger.getInstance().close();
	}
}  //  @jve:decl-index=0:visual-constraint="10,10"
