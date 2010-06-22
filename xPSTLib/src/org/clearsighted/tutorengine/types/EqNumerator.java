package org.clearsighted.tutorengine.types;

import java.util.HashMap;

import org.clearsighted.tutorengine.GoalNode;
import org.clearsighted.tutorengine.Type;

public class EqNumerator extends Type {
	
	Double myval = null;
	String s1,s2,s3;

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
		String t1 = gns.get(s1).getAnsString();
		String t2 = gns.get(s2).getAnsString();
		String t3 = gns.get(s3).getAnsString();
		myval = getEqNumerator(Double.parseDouble(t1), Double.parseDouble(t2),Double.parseDouble(t3));
		return myval == si;
	}
	
	public double getEqNumerator(double d1,double d2,double d3)
	{
		return (d3/d2)*d1;
	}

	@Override
	public void construct(Object[] args)
	{
		s1 = (String)args[0];
		s2 = (String)args[2];
		s3 = (String)args[4];
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
		String t1 = gns.get(s1).getAnsString();
		String t2 = gns.get(s2).getAnsString();
		String t3 = gns.get(s3).getAnsString();
		myval = getEqNumerator(Double.parseDouble(t1), Double.parseDouble(t2),Double.parseDouble(t3));
		
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
