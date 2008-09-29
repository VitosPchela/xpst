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

import java.beans.PropertyChangeEvent;
import java.beans.PropertyChangeListener;
import java.util.ArrayList;
import java.util.HashMap;

import org.clearsighted.tutorbase.dormin.DorminMessage;

/**
 * Implements the event mapper, which primarily consists of two HalfEventMappers.
 *
 */
public abstract class EventMapper
{
	private HalfEventMapper toApp = new HalfEventMapper(true, this), toTutor = new HalfEventMapper(false, this);
	protected ArrayList<String> appNodes = null;
	private ArrayList<PropertyChangeListener> propertyChangeListeners = new ArrayList<PropertyChangeListener>();
	private ArrayList<IEventMapperListener> listeners = new ArrayList<IEventMapperListener>();
	protected HashMap<String, GoalNodeStatus> goalNodes = new HashMap<String, GoalNodeStatus>();
	protected String focusedNode = null;
	private MessageLogger theLogger;
	public TutorEngine tutorEngine;

	public EventMapper()
	{
		theLogger = MessageLogger.getInstance();
	}
	
	public abstract void start();

	/**
	 * an event mapper should override this and eventually call sendOut(msg) to send
	 * messages out to receivers.
	 * to pass a message through: sendOut(dm);
	 * to kill a message: ;
	 * to morph a message: DorminMessage newdm = new DorminMessage(); &lt;copy/morph fields from dm to newdm&gt;; sendOut(newdm); 
	 */
	public void map(boolean toapp, DorminMessage dm)
	{
		theLogger.logDormin(toapp, true, dm);
	}
	
	public void sendOut(boolean toapp, DorminMessage dm)
	{
		theLogger.logDormin(toapp, false, dm);
		if (toapp)
			toApp.receiveNoMapping(dm);
		else
			toTutor.receiveNoMapping(dm);
	}
	
	public HalfEventMapper getToAppHalf()
	{
		return toApp;
	}

	public HalfEventMapper getToTutorHalf()
	{
		return toTutor;
	}

	protected void setAppNodes(ArrayList<String> nodes)
	{
		Object old = appNodes;
		appNodes = nodes;
		firePropertyChange("appNodes", old, nodes);
	}
	
	public void addPropertyChangeListener(PropertyChangeListener listener)
	{
		propertyChangeListeners.add(listener);
	}
	
	public void removePropertyChangeListener(PropertyChangeListener listener)
	{
		propertyChangeListeners.remove(listener);
	}
	
	protected void firePropertyChange(String name, Object oldvalue, Object newvalue)
	{
		for (PropertyChangeListener pcl: propertyChangeListeners)
			pcl.propertyChange(new PropertyChangeEvent(this, name, oldvalue, newvalue));
	}

	public HashMap<String, GoalNodeStatus> getGoalNodes()
	{
		return goalNodes;
	}
	
	public boolean isGoalNodeEnabled(String name)
	{
		// TODO: restore
		return true;
//		return goalNodes == null || (goalNodes.containsKey(name) && goalNodes.get(name).enabled);
	}

	public boolean isGoalNodeCompleted(String name)
	{
		return goalNodes != null && goalNodes.containsKey(name) && goalNodes.get(name).completed;
	}

	public void completeGoalNode(String name)
	{
		if (goalNodes != null && goalNodes.containsKey(name))
		{
			GoalNodeStatus gns = goalNodes.get(name);
			gns.completed = true;
		}
	}

	public void uncompleteGoalNode(String name)
	{
		if (goalNodes != null && goalNodes.containsKey(name))
		{
			GoalNodeStatus gns = goalNodes.get(name);
			gns.completed = false;
		}
	}

	public ArrayList<String> getAppNodes()
	{
		return appNodes;
	}

	public String getFocusedNode()
	{
		return focusedNode;
	}
	
	public void addListener(IEventMapperListener ieml)
	{
		listeners.add(ieml);
	}

	protected void onEnabledChanged(String gnname, boolean enabled)
	{
		for (IEventMapperListener ieml: listeners)
			ieml.enabledChanged(gnname, enabled);
	}
}
