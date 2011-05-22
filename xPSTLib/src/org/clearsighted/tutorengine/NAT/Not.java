package org.clearsighted.tutorengine.NAT;

import java.util.ArrayList;

public class Not {

  // Parameters passed by user -> [notList, n, direction]; I will send Index, response  
  //public static boolean Compare(ArrayList<String> notList, int Index, int n, String direction, ArrayList<String> response)
  public static boolean Compare(ArrayList<String> parameters, int Index, ArrayList<String> response)
  {
	  /*ArrayList<String> notList = new ArrayList<String>(); 
	  int not_count = 0;
	  for(int i=0;i<parameters.size(); i++)
	  {
		  not_count++;
		  if(parameters.get(i).startsWith("'"))
			  notList.add(parameters.get(i).replaceAll("[,;:.?!']",""));
		  else
			  break;
	  }
	  
	  int n = (int) Double.parseDouble(parameters.get(not_count-1));
	  String direction = parameters.get(not_count).replaceAll("'", "");*/
	  
	  
	  int n = (int) Double.parseDouble(parameters.get(0));
	  String direction = parameters.get(1).replaceAll("'", "");
	  ArrayList<String> notList = new ArrayList<String>(); 
	  for(int i=2;i<parameters.size(); i++)
	  {
		  notList.add(parameters.get(i).replaceAll("[,;:.?!']",""));
	  }	  
	  
	  /*System.out.println("Not list  : " + notList);
	  System.out.println("Index     : " + Index);
	  System.out.println("n         : " + n);
	  System.out.println("Direction : " + direction);*/
	  
	  if(direction.equals("<-"))
	  {		  
		  for(int i=Index-1; i>=Index-n; i--)
		  {
			  if(i<0)
				  return true;
			  
			  for(int j=0;j<notList.size(); j++)
				  if(notList.get(j).equals(response.get(i).replaceAll("[,;:.?!']","")))
					  return false;
		  } 	 
		  return true;
	  }
	  
	  else
	  {
		  for(int i=Index+1; i<=Index+n; i++)
		  {
			  if(i>response.size()-1)
				  return true;
			  
			  for(int j=0;j<notList.size(); j++)
				  if(notList.get(j).equals(response.get(i)))
					  return false;
		  } 	 
		  return true;
	  }

  }  
  
  /** main */
  public static void main(String[] args) { 
	  
	  ArrayList<String> response = new ArrayList<String>(); 
	  response.add("a");
	  response.add("b");
	  response.add("c");
	  response.add("d");
	  response.add("e");
	  response.add("f");
	  
	  ArrayList<String> parameters = new ArrayList<String>(); 
	  parameters.add("'a'");
	  //notList.add("b");
	  //notList.add("c");
	  parameters.add("4");
	  parameters.add("'<-'");
	  
	  System.out.println("Response  : " + response);
	  
	  
	  if(Not.Compare(parameters, 4, response))
	  {
		  System.out.println("Yes");
	  }
	  else
		  System.out.println("No");
    
    
  }

}