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

package org.clearsighted.tutorengine;

import java.util.HashMap;
import java.util.LinkedList;
import java.util.Map;

import org.clearsighted.tutorbase.emscript.exprtree.ConstructorNode;
import org.clearsighted.tutorbase.emscript.exprtree.ExprEnv;
import org.clearsighted.tutorbase.emscript.exprtree.ExprException;
import org.clearsighted.tutorbase.emscript.exprtree.ExprNode;

import antlr.collections.AST;


public class GoalNode
{
	private LinkedList<String> hints = new LinkedList<String>();
	private LinkedList<String> oncompletes = new LinkedList<String>();
	private class JIT
	{
		ExprNode cond = null;
		String message = null;
	}
	private LinkedList<JIT> jits = new LinkedList<JIT>();
	private HashMap<String, Object> properties = new HashMap<String, Object>();
	private Type answer = null;

	public LinkedList<String> getHints()
	{
		return hints;
	}
	
	public LinkedList<String> getOnCompletes()
	{
		return oncompletes;
	}
	
	public LinkedList<JIT> getJITs()
	{
		return jits;
	}

	public void addHint(String hinttext)
	{
		hints.add(hinttext);
	}
	
	public void addOnComplete(String otext)
	{
		oncompletes.add(otext);
	}
	
	public void addJIT(ExprNode cond, String jittext)
	{
		JIT j = new JIT();
		j.cond = cond;
		j.message = jittext;
		
		jits.add(j);
	}
	
	public void setProperty(String name, Object val)
	{
		if (val instanceof AST)
			val = ConstructorNode.constructObject((AST)val);
		properties.put(name, val);
		if (name.equals("answer"))
			answer = (Type)val;
	}

	public Object getProperty(String name)
	{
		return properties.get(name);
	}
	
	public Map<String, Object> getProperties()
	{
		return properties;
	}

	public boolean check(HashMap<String, GoalNode> gns,Object val)
	{
		if (answer != null)
			return ((Type)getProperty("answer")).check(gns,(String)val);
		else
			return false;
	}

	public Object getRepresentativeValue()
	{
		if (answer != null)
			return ((Type)getProperty("answer")).getRepresentativeValue();
		return
			"null";
	}

	public String checkJITs(Object value,HashMap<String, GoalNode> gns)
	{
		for (JIT jit: jits)
		{
			if (jit.cond == null)
				return jit.message;
			else
			{
				ExprEnv ee = new ExprEnv();
				ee.put("v", value);
				ee.putAll(properties);
				try
				{
					if (jit.cond.eval(ee,gns) == Boolean.TRUE)
						return jit.message;
				}
				catch (ExprException e)
				{
					// TODO Auto-generated catch block
					e.printStackTrace();
				}
			}
		}
		return null;
	}
}
