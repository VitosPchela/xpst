package org.clearsighted.tutorengine.NAT;

//http://itssmee.wordpress.com/2010/06/28/java-example-of-hamming-distance/
import java.util.ArrayList;
import java.util.List;
public class Hamming {
  public List<String> iWords;
  
  public Hamming(String fileName)
  {
    System.err.print("Reading database ... ");
    iWords = new java.util.ArrayList<String>();
    try {
      java.io.BufferedReader in =
	new java.io.BufferedReader(new java.io.FileReader(fileName));
      String line = in.readLine();
      String prev = null;
      while (line != null) {
	String word = line.toUpperCase();
	if (!word.equals(prev))
	  iWords.add(word);
	prev = word;
	line = in.readLine();
      }
      in.close();
    } catch (java.io.IOException e) {
      System.out.println(e);
    }
    System.err.println();
  }

  // assumes strings have the same length!
  public static int distance(String lhs, String rhs)
  {
	    int d = 0;
	    for (int i = 0; i < lhs.length(); ++i) {
	      if (lhs.charAt(i) != rhs.charAt(i))
		++d;
	    }
	    return d;
  }
  
  public List<String> findMatches(String s, int maxDist)
  {
    List<String> m = new java.util.ArrayList<String>();
    for (String w : iWords) {
      if (!s.equals(w) && s.length() == w.length() 
	  && distance(s, w) <= maxDist) {
	m.add(w);
      }
    }
    return m;
  }
  
  public int countPairs(int maxDist)
  {
    int count = 0;
    for (int i = 0; i < iWords.size() - 1; ++i) {
      String s = iWords.get(i);
      for (int j = i+1; j < iWords.size(); ++j) {
	String s1 = iWords.get(j);
	if (s.length() == s1.length() && distance(s, s1) <= maxDist)
	  ++count;
      }
    }
    return count;
  }
  
  public static boolean Compare(String a, ArrayList<String> b)
  {
	  int n = Integer.parseInt(b.get(0));
	  
	  for(int i=1; i<b.size(); i++)
	  {
		  String temp = b.get(i).replaceAll("[,;:.?!']","");
		  if(a.length() != temp.length())
			  return false;			  
		  if(Hamming.distance(a, temp) <=n )
			  return true;
	  } 	 
	  return false;
  }  
  
  /** main */
  public static void main(String[] args) { 
	  
	  /*String[] names = {"Shrenik"}; 
    
	  if(Hamming.Compare("Shrenqq",names))
	  {
		  System.out.println("Yes");
	  }
	  else
		  System.out.println("No");*/
    
    
  }
}