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


public abstract class EventMapper
{
	private HalfEventMapper ToApp = new HalfEventMapper(true, this), ToTutor = new HalfEventMapper(false, this);
	protected ArrayList<String> AppNodes = null;
	private ArrayList<PropertyChangeListener> PropertyChangeListeners = new ArrayList<PropertyChangeListener>();
	private ArrayList<IEventMapperListener> Listeners = new ArrayList<IEventMapperListener>();
	protected HashMap<String, GoalNodeStatus> GoalNodes = new HashMap<String, GoalNodeStatus>();
	protected String FocusedNode = null;
	private MessageLogger TheLogger;
	public TutorEngine tutorEngine;

	public EventMapper()
	{
		TheLogger = MessageLogger.getInstance();
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
		TheLogger.logDormin(toapp, true, dm);
	}
	
	public void sendOut(boolean toapp, DorminMessage dm)
	{
		TheLogger.logDormin(toapp, false, dm);
		if (toapp)
			ToApp.receiveNoMapping(dm);
		else
			ToTutor.receiveNoMapping(dm);
	}
	
	public HalfEventMapper getToAppHalf()
	{
		return ToApp;
	}

	public HalfEventMapper getToTutorHalf()
	{
		return ToTutor;
	}

	protected void setAppNodes(ArrayList<String> nodes)
	{
		Object old = AppNodes;
		AppNodes = nodes;
		firePropertyChange("AppNodes", old, nodes);
	}
	
	public void addPropertyChangeListener(PropertyChangeListener listener)
	{
		PropertyChangeListeners.add(listener);
	}
	
	public void removePropertyChangeListener(PropertyChangeListener listener)
	{
		PropertyChangeListeners.remove(listener);
	}
	
	protected void firePropertyChange(String name, Object oldvalue, Object newvalue)
	{
		for (PropertyChangeListener pcl: PropertyChangeListeners)
			pcl.propertyChange(new PropertyChangeEvent(this, name, oldvalue, newvalue));
	}

	public HashMap<String, GoalNodeStatus> getGoalNodes()
	{
		return GoalNodes;
	}
	
	public boolean isGoalNodeEnabled(String name)
	{
		// TODO: restore
		return true;
//		return GoalNodes == null || (GoalNodes.containsKey(name) && GoalNodes.get(name).enabled);
	}

	public boolean isGoalNodeCompleted(String name)
	{
		return GoalNodes != null && GoalNodes.containsKey(name) && GoalNodes.get(name).completed;
	}

	public void completeGoalNode(String name)
	{
		if (GoalNodes != null && GoalNodes.containsKey(name))
		{
			GoalNodeStatus gns = GoalNodes.get(name);
			gns.completed = true;
		}
	}

	public void uncompleteGoalNode(String name)
	{
		if (GoalNodes != null && GoalNodes.containsKey(name))
		{
			GoalNodeStatus gns = GoalNodes.get(name);
			gns.completed = false;
		}
	}

	public ArrayList<String> getAppNodes()
	{
		return AppNodes;
	}

	public String getFocusedNode()
	{
		return FocusedNode;
	}
	
	public void addListener(IEventMapperListener ieml)
	{
		Listeners.add(ieml);
	}

	protected void onEnabledChanged(String gnname, boolean enabled)
	{
		for (IEventMapperListener ieml: Listeners)
			ieml.enabledChanged(gnname, enabled);
	}
}
