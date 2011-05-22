package org.clearsighted.tutorengine.NAT;

import java.io.File;
import java.net.MalformedURLException;
import java.net.URL;
import java.util.ArrayList;
import edu.mit.jwi.Dictionary;
import edu.mit.jwi.IDictionary;
import edu.mit.jwi.item.IIndexWord;
import edu.mit.jwi.item.ISynset;
import edu.mit.jwi.item.IWord;
import edu.mit.jwi.item.IWordID;
import edu.mit.jwi.item.POS;

public class Synonym {

  public static boolean Compare(String a, ArrayList<String> arrayList) throws MalformedURLException
  {
	  ArrayList<String> synonymList = new ArrayList<String>();
	  
	  String wnhome = System.getenv("WNHOME");
	  String path = wnhome + File.separator + "dict";
	  URL url;	  
	  path = "C:\\Program Files (x86)\\WordNet\\2.1\\dict";	  
	  url = new URL("file", null, path);
	  IDictionary dict = new Dictionary(url);
	  dict.open();
	  
	  // Synonyms
	  for(int i=0;i<arrayList.size(); i++)
	  {
		  String temp = arrayList.get(i).replaceAll("[,;:.?!']","");
		  IIndexWord idxWord = dict.getIndexWord(temp, POS.NOUN);
		  for(int synLevel = 0; synLevel<idxWord.getWordIDs().size(); synLevel++)
		  {
			  IWordID wordID = idxWord.getWordIDs().get(synLevel); // n'th meaning
			  IWord word = dict.getWord(wordID);
			  ISynset synset = word.getSynset();	  
			  // iterate over words associated with the synset
			  for(IWord w : synset.getWords()) {
				  synonymList.add(w.getLemma());
				  //System.out.println(w.getLemma());
			  }		
		  }	  
	  }
	  
	  for(int i=0; i<synonymList.size(); i++)
	  {
		  String temp = synonymList.get(i).replaceAll("[,;:.?!'_]","");
		  if(a.equals(temp))
			  return true;
	  } 	 
	  return false;
  }  
  
  /** main 
 * @throws MalformedURLException */
  public static void main(String[] args) throws MalformedURLException { 
	  
	  String wnhome = System.getenv("WNHOME");
	  String path = wnhome + File.separator + "dict";
	  URL url;	  
	  path = "C:\\Program Files (x86)\\WordNet\\2.1\\dict";	  
	  url = new URL("file", null, path);
	  System.out.println(path);	  
	  IDictionary dict = new Dictionary(url);
	  dict.open();
	  
	  ///*
	  // synonyms
	  // look up first sense of the word "dog"
	  IIndexWord idxWord = dict.getIndexWord("ball", POS.NOUN);
	  IWordID wordID = idxWord.getWordIDs().get(6); // n'th meaning
	  IWord word = dict.getWord(wordID);
	  ISynset synset = word.getSynset();	  
	  // iterate over words associated with the synset
	  for(IWord w : synset.getWords()) {
		  System.out.println(w.getLemma());
	  	}
	  //*/
	  
	  /*
	  // hypernyms
	  IIndexWord idxWord = dict.getIndexWord("dog", POS.NOUN);
	  IWordID wordID = idxWord.getWordIDs().get(0); // 1st meaning
	  IWord word = dict.getWord(wordID);
	  ISynset synset = word.getSynset();	  
	  // get the hypernyms
	  List hypernyms = (List) synset.getRelatedSynsets(Pointer.HYPERNYM);
	  // print out each hypernym’s id and synonyms
	  ArrayList<IWord> words;
	  for(ISynsetID sid : hypernyms){
	  words = (ArrayList<IWord>) dict.getSynset(sid).getWords();
	  System.out.print(sid + " {");
	  for(Iterator<IWord> i = words.iterator(); i.hasNext();){
	  System.out.print(i.next().getLemma());
	  if(i.hasNext()) System.out.print(", ");
	  }
	  System.out.println("}");
	  }
	  */
	  
	  /*String[] names = {"Shrenk"}; 
    
	  if(Almost.Compare("Shrenik",names))
	  {
		  System.out.println("Yes");
	  }
	  else
		  System.out.println("No");*/    
  }
}