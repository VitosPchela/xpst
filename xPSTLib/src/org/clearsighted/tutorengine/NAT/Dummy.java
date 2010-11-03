package org.clearsighted.tutorengine.NAT;

import java.util.ArrayList;

public class Dummy {

  public static boolean Compare(String a, ArrayList<String> b)
  {
	  for(int i=0; i<b.size(); i++)
	  {
		  //System.out.println("X0" + a + " X1" + b.get(i));
		  String temp = b.get(i).replaceAll("[,;:.?!']","");
		  if(a.equals(temp))
			  return true;
	  } 	 
	return false;
  }  
  
  /** main */
  public static void main(String[] args) { 
	  
 
    
  }

}