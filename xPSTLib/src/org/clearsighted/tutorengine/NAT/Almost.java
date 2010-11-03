package org.clearsighted.tutorengine.NAT;

import java.util.ArrayList;

public class Almost {

  public static boolean Compare(String a, ArrayList<String> arrayList)
  {
	  for(int i=0; i<arrayList.size(); i++)
	  {
		  String temp = arrayList.get(i).replaceAll("[,;:.?!']","");
		  if(a.replaceAll("(?i)[aeiouAEIOU]", "").equals(temp.replaceAll("(?i)[aeiouAEIOU]", "")))
			  return true;
	  } 	 
	  return false;
  }  
  
  /** main */
  public static void main(String[] args) { 
	  
	  /*String[] names = {"Shrenk"}; 
    
	  if(Almost.Compare("Shrenik",names))
	  {
		  System.out.println("Yes");
	  }
	  else
		  System.out.println("No");*/
    
    
  }

}