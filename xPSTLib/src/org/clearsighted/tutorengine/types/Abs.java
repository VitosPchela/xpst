package org.clearsighted.tutorengine.types;

import java.util.HashMap;
import java.lang.Math;

import org.clearsighted.tutorengine.GoalNode;
import org.clearsighted.tutorengine.Type;

public class Abs extends Type {
	
	Double myval = null;
	String s1;

	@Override
	public boolean check(HashMap<String, GoalNode> gns,String value)
	{
		double temp;
		
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
		if( !((s1.charAt(0)>='0' && s1.charAt(0)<='9') || s1.charAt(0)=='-'))
			myval = Double.parseDouble(gns.get(s1).getAnsString());
		else
			myval = Double.parseDouble(s1);
		
		myval = getAbs(myval);
		temp = getAbs(si);
		return myval == temp;
	}
	
	public double getAbs(double a)
	{
		return Math.abs(a);	
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
		double temp;
		
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
		
		if( !((s1.charAt(0)>='0' && s1.charAt(0)<='9') || s1.charAt(0)=='-'))
			myval = Double.parseDouble(gns.get(s1).getAnsString());
		else
			myval = Double.parseDouble(s1);
		
		myval = getAbs(myval);
		temp = getAbs(si);
		
		if(op.equals("l"))
			return temp < myval;
		if(op.equals("g"))
			return temp > myval;
		if(op.equals("le"))
			return temp <= myval;
		if(op.equals("ge"))
			return temp >= myval;
		if(op.equals("e"))
			return temp == myval;
		if(op.equals("ne"))
			return temp != myval;
		return false;
	}
}
