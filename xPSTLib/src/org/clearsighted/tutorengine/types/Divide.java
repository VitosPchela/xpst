package org.clearsighted.tutorengine.types;

import java.util.HashMap;

import org.clearsighted.tutorengine.GoalNode;
import org.clearsighted.tutorengine.Type;

public class Divide extends Type {
	
	Double myval = null;
	String s1,s2;

	@Override
	public boolean check(HashMap<String, GoalNode> gns,String value)
	{
		if (myval == null)
			return false;
		double si;
		try
		{
			si = Double.parseDouble(value);
		}
		catch (NumberFormatException e)
		{
			return false;
		}
		String t1 = gns.get(s1).getProperty("answer").toString();
		String t2 = gns.get(s2).getProperty("answer").toString();
		myval = Double.parseDouble(t1) / Double.parseDouble(t2);
		return myval == si;
	}

	@Override
	public void construct(Object[] args)
	{
		s1 = (String)args[0];
		s2 = (String)args[2];
		myval = 0.0;
	}

	@Override
	public Object getRepresentativeValue()
	{
		return myval;
	}

	@Override
	public String toString()
	{
		return myval.toString();
	}
	
	public boolean compare(HashMap<String, GoalNode> gns,String value,String op)
	{
		if (myval == null)
			return false;
		double si;
		try
		{
			si = Double.parseDouble(value);
		}
		catch (NumberFormatException e)
		{
			return false;
		}
		String t1 = gns.get(s1).getProperty("answer").toString();
		String t2 = gns.get(s2).getProperty("answer").toString();
		myval = Double.parseDouble(t1) / Double.parseDouble(t2);
		
		if(op.equals("l"))
			return si < myval;
		if(op.equals("g"))
			return si > myval;
		if(op.equals("le"))
			return si <= myval;
		if(op.equals("ge"))
			return si >= myval;
		if(op.equals("e"))
			return si == myval;
		if(op.equals("ne"))
			return si != myval;
		return false;
	}
}
