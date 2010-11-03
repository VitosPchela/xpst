package org.clearsighted.tutorengine.NAT;

import java.util.ArrayList;

public class AnyExcept {

  public static boolean Compare(String a, ArrayList<String> b)
  {
	  for(int i=0; i<b.size(); i++)
	  {
		  if(a.equals(b.get(i)))
			  return false;
	  } 	 
	  return true;
  }  
  
  /** main */
  public static void main(String[] args) { 
	  
	  /*String[] names = {"Shrenik", "Shrenk", "Shrek"}; 
    
	  if(AnyExcept.Compare("Shrenik",names))
	  {
		  System.out.println("Yes");
	  }
	  else
		  System.out.println("No");*/
    
    
  }

}