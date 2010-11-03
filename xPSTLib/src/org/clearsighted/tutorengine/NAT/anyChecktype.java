package org.clearsighted.tutorengine.NAT;

import java.util.ArrayList;

public class anyChecktype
{
	private int n1, n2;
	private int checktypePos;
	private int lastWord;
	private int consumeCount;
	
	public  boolean isSatisfied()
	{
		if(consumeCount>=n1 && consumeCount<=n2)
			return true;
		else
			return false;
	}
	public anyChecktype(ArrayList<String> indices, int pos)
	{
		n1 = (int) Double.parseDouble(indices.get(0));
		n2 = (int) Double.parseDouble(indices.get(1));
		lastWord = -1;
		consumeCount = 0;
		checktypePos = pos;
	}
	
	public void consumeWord(int wordPos)
	{
		consumeCount++;
		lastWord = wordPos;
	}
	
	public int getchecktypePos()
	{
		return checktypePos;
	}
	public boolean isMax() 
	{
		if(consumeCount == n2)
			return true;
		else
			return false;
	}
	public void reset() 
	{
		lastWord = -1;
		consumeCount = 0;		
	}
	public int getlastWord() 
	{
		return lastWord;
	}
	public boolean canConsumeMore() 
	{
		if(consumeCount < n2)
			return true;
		else
			return false;
	}
	public void setlastWord(int wordCounter) 
	{
		lastWord = wordCounter;		
	}
}