package org.clearsighted.tutorengine.NAT;

import java.util.ArrayList;
import java.util.regex.Pattern;

public class RegEx {

	  public static boolean Compare(String a, ArrayList<String> b)
	  {
		  for(int i=0; i<b.size(); i++)
		  {
			  String regex = b.get(i);	
			  Pattern pattern_css = Pattern.compile(regex);

			  if(pattern_css.matcher(a).matches())
				  return true;
		  } 	 
		  return false;
	  }  
	  
	  /** main */
	  public static void main(String[] args) { 
		  
		  /*String[] names = {"[SW]hrenik", "t", "t"}; 
	    
		  if(RegEx.Compare("Ahrenik",names))
		  {
			  System.out.println("Yes");
		  }
		  else
			  System.out.println("No");*/  
	    
	  }

	}


