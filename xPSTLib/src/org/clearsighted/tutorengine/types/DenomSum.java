package org.clearsighted.tutorengine.types;

import java.util.HashMap;

import org.clearsighted.tutorengine.GoalNode;
import org.clearsighted.tutorengine.Type;

public class DenomSum extends Type {
	
	Double myval = null;
	String s1,s2,s3,s4;

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
		double t1 = Double.parseDouble(gns.get(s1).getProperty("answer").toString());
		double t2 = Double.parseDouble(gns.get(s2).getProperty("answer").toString());
		double t3 = Double.parseDouble(gns.get(s3).getProperty("answer").toString());
		double t4 = Double.parseDouble(gns.get(s4).getProperty("answer").toString());
		
		myval = getDenomSum(t1,t2,t3,t4);
		return myval == si;
	}
	
	public double getDenomSum(double n1,double d1,double n2,double d2)
	{
		double gcd = gcd(d1,d2);
		double multiplier1 = d2/gcd;
		double newDenominator = d1*multiplier1;
		return newDenominator;
	}
	
	public double gcd(double a, double b)
	{
		if (b==0) return a;
		   return gcd(b,a%b);
	}

	@Override
	public void construct(Object[] args)
	{
		s1 = (String)args[0];
		s2 = (String)args[2];
		s3 = (String)args[4];
		s4 = (String)args[6];
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
		double t1 = Double.parseDouble(gns.get(s1).getProperty("answer").toString());
		double t2 = Double.parseDouble(gns.get(s2).getProperty("answer").toString());
		double t3 = Double.parseDouble(gns.get(s3).getProperty("answer").toString());
		double t4 = Double.parseDouble(gns.get(s4).getProperty("answer").toString());
		
		myval = getDenomSum(t1,t2,t3,t4);
		
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
