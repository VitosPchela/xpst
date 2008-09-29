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

import java.awt.Color;
import java.awt.Dimension;
import java.awt.Font;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;
import java.util.ArrayList;
import java.util.Collections;
import java.util.HashMap;
import java.util.Iterator;
import java.util.LinkedList;

import javax.swing.BoxLayout;
import javax.swing.JButton;
import javax.swing.JFrame;
import javax.swing.JPanel;
import javax.swing.JScrollPane;
import javax.swing.JTextPane;
import javax.swing.ScrollPaneConstants;
import javax.swing.text.BadLocationException;
import javax.swing.text.DefaultStyledDocument;
import javax.swing.text.Style;
import javax.swing.text.StyleConstants;
import javax.swing.text.StyledDocument;

/**
 * Displays the sequence in a Swing pane to aid in debugging. Needs to be updated.
 *
 */
public class DebugFrame extends JFrame implements ActionListener
{
	public JTextPane textPane = new JTextPane();
	public JScrollPane scrollPane = new JScrollPane(textPane);
	private JPanel buttonPanel = new JPanel();
	private JScrollPane buttonScrollPane = new JScrollPane(buttonPanel);
	private LinkedList<DebugFrameListener> listeners = new LinkedList<DebugFrameListener>();

	public DebugFrame()
	{
		setSize(1000, 800);
		setAlwaysOnTop(true);

		getContentPane().setLayout(new BoxLayout(getContentPane(), BoxLayout.X_AXIS));
		buttonPanel.setLayout(new BoxLayout(buttonPanel, BoxLayout.Y_AXIS));
		
		Dimension d = new Dimension(300, 3000);
		buttonScrollPane.setMaximumSize(d);
		
		scrollPane.setVerticalScrollBarPolicy(ScrollPaneConstants.VERTICAL_SCROLLBAR_ALWAYS);
		add(scrollPane);

		buttonScrollPane.setVerticalScrollBarPolicy(ScrollPaneConstants.VERTICAL_SCROLLBAR_ALWAYS);
		add(buttonScrollPane);

		textPane.setEditable(false);
		textPane.setFont(new Font("Monospaced", 0, 12));
	}

	public void updateContents(ArrayList<String> script, ArrayList<DebugStyle> styles, HashMap<String, GoalNodeStatus> gns)
	{
		JTextPane tp = textPane;
		StyledDocument doc = new DefaultStyledDocument();
		Style megn = doc.addStyle(DebugStyle.MAPPED_ENABLED_GOAL_NODE, null);
		StyleConstants.setForeground(megn, Color.GREEN);

		Style mdgn = doc.addStyle(DebugStyle.MAPPED_DISABLED_GOAL_NODE, null);
		StyleConstants.setForeground(mdgn, Color.RED);

		Style uegn = doc.addStyle(DebugStyle.UNMAPPED_ENABLED_GOAL_NODE, null);
		StyleConstants.setForeground(uegn, Color.CYAN);

		Style udgn = doc.addStyle(DebugStyle.UNMAPPED_DISABLED_GOAL_NODE, null);
		StyleConstants.setForeground(udgn, Color.BLACK);

		try
		{
			Iterator<DebugStyle> dsi = styles.iterator();
			DebugStyle nextstyle = null;
			if (dsi.hasNext())
				nextstyle = dsi.next();
			int row = 1, col = 1;
			for (String line: script)
			{
				while (nextstyle != null && nextstyle.row <= row)
				{
					doc.insertString(doc.getLength(), line.substring(col - 1, nextstyle.column - 1), null);
					col = nextstyle.column + nextstyle.length;
					doc.insertString(doc.getLength(), line.substring(nextstyle.column - 1, col - 1), doc.getStyle(nextstyle.style));
					if (dsi.hasNext())
						nextstyle = dsi.next();
					else
						nextstyle = null;
				}
				doc.insertString(doc.getLength(), line.substring(col - 1), null);
				doc.insertString(doc.getLength(), "\n", null);
				row++;
				col = 1;
			}
			tp.setStyledDocument(doc);
		}
		catch (BadLocationException e)
		{
		}

		buttonPanel.removeAll();
		Font of = buttonPanel.getFont();
		Font f = new Font(of.getName(), Font.ITALIC, of.getSize());
		ArrayList<String> al = new ArrayList<String>();
		for (String s: gns.keySet())
			al.add(s);
		Collections.sort(al);
		for (String s: al)
		{
			JButton b = new JButton(s);
			b.addActionListener(this);
			if (!gns.get(s).enabled)
				b.setFont(f);
			buttonPanel.add(b);
		}
		buttonPanel.doLayout();
	}

	public void addListener(DebugFrameListener dfl)
	{
		listeners.add(dfl);
	}

	private void onCompleteGoalnode(String gnname)
	{
		for (DebugFrameListener dfl: listeners)
			dfl.completeGoalnode(gnname);
	}
	
	public void actionPerformed(ActionEvent e)
	{
		String gnname = ((JButton)e.getSource()).getText();
		onCompleteGoalnode(gnname);
	}
}
