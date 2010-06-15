package org.clearsighted.tutorengine.types;

import java.util.HashMap;

import org.clearsighted.tutorengine.GoalNode;
import org.clearsighted.tutorengine.Type;

public class Ans extends Type {
	
	String myval = null;
	String s1;

	@Override
	public boolean check(HashMap<String, GoalNode> gns,String value)
	{
		if(myval == null)
			return false;
		String temp = gns.get(s1).getAnsString();
		if(!temp.equals(""))
		{
			myval = temp;
			return myval.equals(value);
		}
		return false;
	}

	@Override
	public void construct(Object[] args)
	{
		s1 = (String)args[0];
		myval = "";
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
		String temp = gns.get(s1).getAnsString();
		if(!temp.equals(""))
		{
			myval = temp;
			if(op.equals("e"))
				return value.equals(myval);
			if(op.equals("ne"))
				return !(value.equals(myval));
		}
		return false;
	}
}
