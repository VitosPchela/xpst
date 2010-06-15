package org.clearsighted.tutorengine.types;

import java.util.HashMap;

import org.clearsighted.tutorengine.GoalNode;
import org.clearsighted.tutorengine.Type;

public class IsMultiple extends Type {
	
	Double myval = null;
	String s1;

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
		myval = Double.parseDouble(gns.get(s1).getAnsString());
		return isMultiple(myval,si);
	}
	
	public boolean isMultiple(double a,double b)
	{
		if(a==0 || b==0)
			return false;
		if(b%a == 0)
			return true;
		else
			return false;
	}

	@Override
	public void construct(Object[] args)
	{
		s1 = (String)args[0];
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
		myval = Double.parseDouble(gns.get(s1).getAnsString());
		
		if(op.equals("e"))
			return isMultiple(myval,si);
		if(op.equals("ne"))
			return !isMultiple(myval,si);
		return false;
	}
}
