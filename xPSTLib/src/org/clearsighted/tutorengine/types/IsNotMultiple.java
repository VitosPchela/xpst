package org.clearsighted.tutorengine.types;

import java.util.HashMap;

import org.clearsighted.tutorengine.GoalNode;
import org.clearsighted.tutorengine.Type;

public class IsNotMultiple extends Type {
	
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
		return isNotMultiple(myval,si);
	}
	
	public boolean isNotMultiple(double a,double b)
	{
		if(a == b)
			return false;
		if(a==0 ||b==0)
			return false;
		if(b%a == 0)
			return false;
		else
			return true;
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
			return isNotMultiple(myval,si);
		if(op.equals("ne"))
			return !isNotMultiple(myval,si);
		return false;
	}
}
