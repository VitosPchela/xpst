package org.clearsighted.tutorengine.NAT;

import java.util.ArrayList;
import java.util.regex.Matcher;
import java.util.regex.Pattern;
import java.lang.String;
import java.net.MalformedURLException;

public class Start
{	
	public String response;
	public String stopPhrases;
	public String checktype;
	
	public int mismatchChecktypePos;
	public int mismatchWordPos;
	
	//public String responseFiltered;
	public ArrayList<String> responseWordsFiltered;
	public ArrayList<String> functionDefinitions;
	public ArrayList<String> functionNames;
	public ArrayList<ArrayList<String>> functionParameters;
	

	public Start(String r, String s, String c)
	{
		response = r;
		stopPhrases = s;
		checktype = c;
		
		responseWordsFiltered = new ArrayList<String>();
		functionNames = new ArrayList<String>();
		functionParameters = new ArrayList<ArrayList<String>>();
		
		responseWordsFiltered = removeStopPhrases(response, stopPhrases);
		functionDefinitions = extractAllFunctionDefinitions(checktype);
		
		for(int i=0; i<functionDefinitions.size(); i++)
		{
			functionNames.add(extractFunctionName(functionDefinitions.get(i)));
			functionParameters.add(extractFunctionParameters(functionDefinitions.get(i)));
		}
	}
	// Extract function definitions
	public  ArrayList<String> extractAllFunctionDefinitions(String input)
	{
		ArrayList<String> functions = new ArrayList<String>();
		String sourcestring = input;

		Pattern re = Pattern.compile("(?:\\s*)([^(]+\\((?:'(?:[^'\\\\]|\\\\.)*'|[^,'\\s)]+)(?:\\s*,\\s*(?:'(?:[^'\\\\]|\\\\.)*'|[^,'\\s)]+))*\\s*\\))");
		Matcher m = re.matcher(sourcestring);
		int mIdx = 0;

		while (m.find())
		{
			for( int groupIdx = 1; groupIdx < m.groupCount()+1; groupIdx+=2 )
			{
				functions.add(m.group(groupIdx).trim());
		    }
		    mIdx++;
		}
		
		return functions;
	}
	// Extract function names
	public  String extractFunctionName(String input)
	{
		return input.substring(0, input.indexOf("("));		
	}	
	// Extract function parameters
	public  ArrayList<String> extractFunctionParameters(String input)
	{
		ArrayList<String> parameters = new ArrayList<String>();
		
		String sourcestring = input;
		// Remove the function name and it's round brackets at the start and end:
		Pattern re = Pattern.compile("[^(]*\\((.*)\\)",Pattern.MULTILINE);
		Matcher m = re.matcher(sourcestring);
		String result = m.replaceAll("$1");
		// Then look for comma separated values:
		sourcestring = result;
		re = Pattern.compile("('(?:[^'\\\\]|\\\\.)*'|[^,'\\s)]+)");
		m = re.matcher(sourcestring);
		int mIdx = 0;
		while (m.find())
		{
			for( int groupIdx = 1; groupIdx < m.groupCount()+1; groupIdx+=2 )
			{
				//String temp = m.group(groupIdx).replaceAll("'", "");
				String temp = m.group(groupIdx);
				parameters.add(temp);
		    }
		    mIdx++;
		}
		return parameters;
		
	}
	// Remove stop phrases
	public  ArrayList<String> removeStopPhrases(String input, String stopphrases)
	{
		String source = input.replaceAll("[,;:.?!]","");
		String strippedWords = new String(source);
		String listOfStopWords = stopphrases;
		String[] stopWords = listOfStopWords.split(",");
		
		for (int i = 0; i < stopWords.length; i++) {
			stopWords[i] = stopWords[i].trim();
			strippedWords = strippedWords.replaceAll("(?i)" + stopWords[i].replaceAll("'", ""), "").trim();
			strippedWords = strippedWords.replace("  ", " ");
		}
		
		ArrayList<String> srtippedWordsList = new ArrayList<String>();
		String[] strippedWordsArray = strippedWords.split("\\s+");
		for(int i=0; i<strippedWordsArray.length; i++)
		{
			srtippedWordsList.add(strippedWordsArray[i]);
		}
		
		return srtippedWordsList;
	}
	// Print results
	public  void printResults() throws MalformedURLException
	{
		System.out.println("Response             :" + response);
		System.out.println("Stop Phrases         :" + stopPhrases);
		System.out.println("Checktype            :" + checktype);
		System.out.println();
		
		System.out.println("Filtered Response    :" + responseWordsFiltered);
		System.out.println("Function Definitions :" + functionDefinitions);
		System.out.println("Function Names       :" + functionNames);
		System.out.println("Function Parameters  :" + functionParameters);
		
		System.out.println();
		if(isresponseMatched())
		{
			System.out.println("RESULT               : Checktype matched.");
		}
		else
		{
			System.out.println("RESULT               : No match.");
			//System.out.println("Word Position     : " + mismatchWordPos);
			//System.out.println("Checktype Position: " + mismatchChecktypePos);
			
		}	
	}
	// Checktype satisfied?
	public  boolean checktypeSatisfied(String function, ArrayList<String> arrayList, String word) throws MalformedURLException
	{
		if(function.equals("Almost"))
			return Almost.Compare(word, arrayList);
		if(function.equals("Exact"))
			return Exact.Compare(word, arrayList);
		if(function.equals("Hamming"))
			return Hamming.Compare(word, arrayList);
		if(function.equals("Levenshtein"))
			return Levenshtein.Compare(word, arrayList);
		if(function.equals("RegEx"))
			return RegEx.Compare(word, arrayList);
		if(function.equals("Soundex"))
			return Soundex.Compare(word, arrayList);
		if(function.equals("Stemmer"))
			return Stemmer.Compare(word, arrayList);
		if(function.equals("Synonym"))
			return Synonym.Compare(word, arrayList);
			
		return false;
	}
	// Answer matcched?
	public  boolean isresponseMatched() throws MalformedURLException
	{
		ArrayList<anyChecktype> anyList = new ArrayList<anyChecktype>();		
		for(int i=0; i<functionNames.size(); i++)
		{
			if(functionNames.get(i).equals("Any"))
			{
				anyChecktype temp = new anyChecktype(functionParameters.get(i), i);
				anyList.add(temp);
			}
		}
		
		int checktypeCounter = 0;     // No of checktypes satisfied so far
		int wordCounter = 0;          // No of words consumed so far
		int anyCounter = 0;           // No of 'Any' checktpyes satisfied so far		
		boolean consumeMore = false;  // When true, asks the previous 'Any' checktype to consume 1 more word
		
		while(checktypeCounter<functionNames.size() && wordCounter<responseWordsFiltered.size())
		{
			// Not -> Proceed, we'll handle them later
			if(functionNames.get(checktypeCounter).equals("Not"))
			{
				checktypeCounter++;
				continue;
			}
			// Any
			else if(functionNames.get(checktypeCounter).equals("Any"))
			{
				while( (!anyList.get(anyCounter).isSatisfied() || consumeMore) && wordCounter<responseWordsFiltered.size() )
				{
					// If the first 'Any' checktype has reached Max, return false since you can't backtrack anymore
					if(anyCounter==0 && anyList.get(anyCounter).isMax())		
					{
						mismatchWordPos = wordCounter;
						mismatchChecktypePos = checktypeCounter  + 1;
						return false;
					}										
					// If the 'Any' checktpye has reached Max, reset the current one, and backtrack to previous 'Any' checktype 
					else if(anyList.get(anyCounter).isMax())
					{
						anyList.get(anyCounter).reset();
						consumeMore = true;
						anyCounter--;
						checktypeCounter = anyList.get(anyCounter).getchecktypePos();
						wordCounter = anyList.get(anyCounter).getlastWord() + 1;						
						continue;
					}
					// Consume 1 word
					else
					{
						anyList.get(anyCounter).consumeWord(wordCounter);
						wordCounter++;
						consumeMore = false;
					}
				}
				// Set lastWord consumed by current 'Any' checktype
				if(wordCounter>0)
					anyList.get(anyCounter).setlastWord(wordCounter-1);
				else
					anyList.get(anyCounter).setlastWord(-1);
				
				anyCounter++;
				checktypeCounter++;				
			}
			// Exact, Almost, etc...
			else
			{
				// Check if the previous checktype is 'Not'. If yes, check if the conditions are met. (Exempt if it is the first checktype)
				boolean notFlag = false;
				if(checktypeCounter==0)
					notFlag = true;
				else if( !functionNames.get(checktypeCounter-1).equals("Not") || (functionNames.get(checktypeCounter-1).equals("Not") && Not.Compare(functionParameters.get(checktypeCounter-1), wordCounter, responseWordsFiltered)) )
					notFlag = true;
				
				if( (checktypeSatisfied(functionNames.get(checktypeCounter), functionParameters.get(checktypeCounter), responseWordsFiltered.get(wordCounter))) && notFlag)
				{					
					checktypeCounter++;
					wordCounter++;					
				}
				else // Backtrack to previous 'Any'
				{
					mismatchWordPos = wordCounter;
					mismatchChecktypePos = checktypeCounter;
					
					if(anyCounter==0) // If no 'Any' checktype has been encountered so far, we can't backtrack. Return false.
					{
						return false;
					}
					else
					{
						consumeMore = true;
						anyCounter--;
						checktypeCounter = anyList.get(anyCounter).getchecktypePos();						
						wordCounter = anyList.get(anyCounter).getlastWord() + 1;
					}
				}
			}
		}
		
		// If we are at the last checktype, and it is 'Any', let it consume as many words as it can
		if( (( (checktypeCounter==functionNames.size()-1) ) && functionNames.get(functionNames.size()-1).equals("Any"))  )
		{
			while( (wordCounter<responseWordsFiltered.size()) && anyList.get(anyCounter).canConsumeMore() )
			{
				anyList.get(anyCounter).consumeWord(wordCounter);
				wordCounter++;
			}
			checktypeCounter++;
			anyCounter++;
			if((wordCounter == responseWordsFiltered.size()) && anyList.get(anyList.size()-1).isSatisfied())
				return true;
			else
			{
				mismatchWordPos = responseWordsFiltered.size();
				mismatchChecktypePos = functionNames.size();
				return false;
			}									
		}
		// **If we are at the last checktype, and it is 'Any', let it consume as many words as it can
		else if( (( (checktypeCounter==functionNames.size()) ) && functionNames.get(functionNames.size()-1).equals("Any"))  )
		{
			while( (wordCounter<responseWordsFiltered.size()) && anyList.get(anyList.size()-1).canConsumeMore() )
			{
				anyList.get(anyList.size()-1).consumeWord(wordCounter);
				wordCounter++;
			}
			checktypeCounter++;
			anyCounter++;
			if((wordCounter == responseWordsFiltered.size()) && anyList.get(anyList.size()-1).isSatisfied())
				return true;
			else
			{
				mismatchWordPos = responseWordsFiltered.size();
				mismatchChecktypePos = functionNames.size();
				return false;
			}								
		}
		
		// If we have only 1 checktype, and thats 'Any', do the same as above
		if( functionNames.size()==1 &&  functionNames.get(0).equals("Any"))
		{
			while( (wordCounter<responseWordsFiltered.size()) && anyList.get(0).canConsumeMore() )
			{
				anyList.get(0).consumeWord(wordCounter);
				wordCounter++;
			}
			if( wordCounter == responseWordsFiltered.size() && anyList.get(anyList.size()-1).isSatisfied() )
				return true;
			else
			{
				mismatchWordPos = responseWordsFiltered.size();
				mismatchChecktypePos = functionNames.size();
				return false;
			}									
		}
		
		// Verify conditions and return result
		if( checktypeCounter==functionNames.size() && wordCounter == responseWordsFiltered.size() )
			return true;
		else
		{
			mismatchWordPos = wordCounter;
			mismatchChecktypePos = checktypeCounter;
			return false;
		}
		
	}

	public static void main(String[] args) throws MalformedURLException
	{	
		// Response
		//String r = "The  result is in the region rejection so this is a significant result";
		//String r = "reject the null hypothesis. There is a significant difference between rock music and no music.";
		String r = "abcd c a d shrenik 7";
		// Stop phrases
		String s = "";
		// Checktype
		//String c ="Any(0,4) Levenshtein('reject') Any(0,3) Levenshtein('null') Levenshtein('hypothesis') Any(0,5) Levenshtein('significant') Levenshtein('difference') Any(0,10)";
		//String c ="Not('not', 'isnt', 2, '->') Exact('result')";
		String c = "Any(0,3) Any(0,6) Almost('Shrenik') RegEx([2-5], [7-9]))";
		//c += "Any(0,2) Almost('result') Any(0,3) Not('not',3,'<-') Levenshtein('region') Levenshtein('rejection') Any(1,4) Not('not',3,'<-') Levenshtein('significant') Levenshtein('result') Any(0,4)";;
		//c+= " or Any(1,14)";
		
		String[] individualArray = c.split("(?<=\\))\\s+or\\s+");
		ArrayList<Start> individualChecktypeList = new ArrayList<Start>();
		for (int i=0; i<individualArray.length; i++) 
		{
			Start tempCheck= new Start(r,s,individualArray[i]);
			individualChecktypeList.add(tempCheck);
			tempCheck.printResults();
			System.out.println();
		}		
	}
}
