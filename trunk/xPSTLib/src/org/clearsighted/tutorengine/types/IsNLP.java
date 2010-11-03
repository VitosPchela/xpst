package org.clearsighted.tutorengine.types;

import java.util.HashMap;
import java.io.*;

import org.clearsighted.tutorengine.GoalNode;
import org.clearsighted.tutorengine.Type;
import org.clearsighted.tutorengine.NAT.Start;
import java.util.ArrayList;
import java.io.IOException;


public class IsNLP extends Type {
	
	String myval = null;
	String checktype;

	@Override
	public boolean check(HashMap<String, GoalNode> gns,String value)
	{
		if (myval == null)
			return false;

		String si;
		si = value;

		String i1;		
		i1 = checktype;
		
		return IsNLP(i1,si);
	}
	
	public boolean IsNLP(String xi1, String xs1)
	{
        try
        {
			String c = xi1;
			String r = xs1;
			String s ="";
			boolean flag = false;

			String[] individualArray = c.split("(?<=\\))\\s+or\\s+");
			ArrayList<Start> individualChecktypeList = new ArrayList<Start>();

			for (int i=0; i<individualArray.length; i++) 
			{
				Start tempCheck= new Start(r,s,individualArray[i]);
				individualChecktypeList.add(tempCheck);
				
				if(tempCheck.isresponseMatched())
					flag = true;
			}	
			if(flag)
				return true;
			else
				return false;
        } 
        catch (Exception e) 
        {
            return false;
        }
	}

	@Override
	public void construct(Object[] args)
	{
		checktype = (String)args[0];
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

		String si;
		si = value;

		String i1;
		i1 = checktype;
		
		if(op.equals("e"))
			return IsNLP(i1, si);
		if(op.equals("ne"))
			return !IsNLP(i1, si);
		return false;
	}
}
