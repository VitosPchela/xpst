package org.clearsighted.tutorengine;

import java.io.Writer;
import java.io.IOException;
import java.io.OutputStreamWriter;
import java.io.FileOutputStream;
import java.io.BufferedWriter;
import java.io.FileWriter;

public class SynchFile {

    Writer out;
    String name;

    public SynchFile(String name) {
        this.name = name;
    }

    public synchronized void write(String response, String checktype) throws IOException  {		
		StringList a = new StringList();
    	a.read(name);
    	boolean checktypeExist = false;
    	String newline = "Shrenik";
    	
    	for(int i=0; i<a.size(); i++)
    	{
			if(a.get(i).equals(checktype))
    		{
				a.add(i+2,response);
    			checktypeExist = true;
    			break;
    		}    			  
    	}
    	if(!checktypeExist)
    	{
			a.add("\nChecktype:\n" + checktype);
			a.add("Responses:\n" + response);
    	}
		
		if (out == null) out = new OutputStreamWriter(new FileOutputStream(name));
		for (int i = 0; i < a.size(); i++)
      	{
			out.write(a.get(i));
            out.write("\n");
      	}
    }

    public void close() throws IOException {
        if (out != null) out.close();
    }

      public static void main(String[] args) {
          /*SynchFile file = new SynchFile("file.txt");
          try {
              file.write("test 111");
              file.write("test 222 ");
              file.close();
          } catch (Exception e) {
              //e.printStackTrace();
          }*/

      }

}