package org.clearsighted.tutorengine.types;

import java.util.HashMap;

import org.clearsighted.tutorengine.GoalNode;
import org.clearsighted.tutorengine.Type;

public class IsRange extends Type {
	
	Double myval = null;
	String r, s1, s2;

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
		
		double i1,i2;
		
		//if (!Character.isDigit(s1.charAt(s1.length()-1)))
		//if( ((int)s1.charAt(0)>=(int)'0' && (int)s1.charAt(0)<=(int)'9') || (int)s1.charAt(0)==(int)'-')
		//if( ((int)s1.charAt(0)>=(int)'0' && (int)s1.charAt(0)<=(int)'9') || s1.charAt(0)=='-' )
		if( !((s1.charAt(0)>='0' && s1.charAt(0)<='9') || s1.charAt(0)=='-'))
		//if(!( ((int)s1.charAt(0)>=(int)'a' && (int)s1.charAt(0)<=(int)'z') || ((int)s1.charAt(0)>=(int)'A' && (int)s1.charAt(0)<=(int)'Z') || s1.charAt(0)=='_' ))
		//try
		//{
			i1 = Double.parseDouble(gns.get(s1).getAnsString());
		//}
		else
			i1 = Double.parseDouble(s1);
			
		if( !((s1.charAt(0)>='0' && s1.charAt(0)<='9') || s1.charAt(0)=='-'))
			i2 = Double.parseDouble(gns.get(s2).getAnsString());
		else
			i2 = Double.parseDouble(s2);
		
		return isRange(r,i1,i2,si);
	}
	
	public boolean isRange(String rr, double i1, double i2, double ans)
	{
		if(rr.equals("[]"))
			if(ans>=i1 && ans<=i2)
				return true;
				
		if(rr.equals("()"))				
			if(ans>i1 && ans<i2)
				return true;
				
		if(rr.equals("[)"))
			if(ans>=i1 && ans<i2)
				return true;
				
		if(rr.equals("(]"))
			if(ans>i1 && ans<=i2)
				return true;
				
		return false;
	}

	@Override
	public void construct(Object[] args)
	{
		r = (String)args[0];
		s1 = (String)args[2];
		s2 = (String)args[4];
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
		
		//String r = Double.parseDouble(gns.get(r).getAnsString());
		double i1,i2;
		
		if( !((s1.charAt(0)>='0' && s1.charAt(0)<='9') || s1.charAt(0)=='-'))
			i1 = Double.parseDouble(gns.get(s1).getAnsString());
		else
			i1 = Double.parseDouble(s1);
			
		if( !((s1.charAt(0)>='0' && s1.charAt(0)<='9') || s1.charAt(0)=='-'))
			i2 = Double.parseDouble(gns.get(s2).getAnsString());
		else
			i2 = Double.parseDouble(s2);
		
		if(op.equals("e"))
			return isRange(r,i1,i2,si);
		if(op.equals("ne"))
			return !isRange(r,i1,i2,si);
		return false;
	}
}
