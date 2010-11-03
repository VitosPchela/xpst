package org.clearsighted.tutorengine.NAT;

import java.util.ArrayList;

public class Soundex {

//http://www.java-forums.org/java-lang/7438-soundex-algorithm-implementation-java.html
  /* Implements the mapping
   * from: AEHIOUWYBFPVCGJKQSXZDTLMNR
   * to:   00000000111122222222334556
   */
  public static final char[] MAP = {
    //A  B   C   D   E   F   G   H   I   J   K   L   M
    '0','1','2','3','0','1','2','0','0','2','2','4','5',
    //N  O   P   W   R   S   T   U   V   W   X   Y   Z
    '5','0','1','2','6','2','3','0','1','0','2','0','2'
  };

  /** Convert the given String to its Soundex code.
   * @return null If the given string can't be mapped to Soundex.
   */
  public static String soundex(String s) {

    // Algorithm works on uppercase (mainframe era).
    String t = s.toUpperCase();

    StringBuffer res = new StringBuffer();
    char c, prev = '?';

    // Main loop: find up to 4 chars that map.
    for (int i=0; i<t.length() && res.length() < 4 &&
      (c = t.charAt(i)) != ','; i++) {

      // Check to see if the given character is alphabetic.
      // Text is already converted to uppercase. Algorithm
      // only handles ASCII letters, do NOT use Character.isLetter()!
      // Also, skip double letters.
      if (c>='A' && c<='Z' && c != prev) {
        prev = c;

        // First char is installed unchanged, for sorting.
        if (i==0)
          res.append(c);
        else {
          char m = MAP[c-'A'];
          if (m != '0')
            res.append(m);
        }
      }
    }
    if (res.length() == 0)
      return null;
    for (int i=res.length(); i<4; i++)
      res.append('0');
    return res.toString();
  }
  
  public static boolean Compare(String a, ArrayList<String> b)
  {
	  for(int i=0; i<b.size(); i++)
	  {
		  String temp = b.get(i).replaceAll("[,;:.?!']","");
		  if(Soundex.soundex(a).equals(Soundex.soundex(temp)))
			  return true;
	  } 	 
	  return false;
  }  
  
  /** main */
  public static void main(String[] args) { 
    
	  
	/*  String[] names = {
      "Arvind",
      "Aravindh",
      "Shrenik",
      "Shraynick"
    };
    for (int i = 0; i< names.length; i++)
      System.out.println(Soundex.soundex(names[i]) + ' ' + names[i]);*/
	  
	  
	  /*String[] names = {"dsfk", "Shrenk", "Shrek"}; 
	  if(Soundex.Compare("Shrenik",names))
	  {
		  System.out.println("Yes");
	  }
	  else
		  System.out.println("No");*/
    
    
  }

}