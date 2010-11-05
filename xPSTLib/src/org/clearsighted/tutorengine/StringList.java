package org.clearsighted.tutorengine;

import java.io.*;
import java.util.ArrayList;

/**
 *  A utility class the allows a text file to be
 *  treated as a collection of Strings
 *
 * @author     CEHJ
 * @created    29 February 2004
 */
public class StringList extends ArrayList<String> {

    /**
     *Constructor for the StringList object
     */
    public StringList() {
	super();
    }

    public void read(InputStream in) {
	read(new InputStreamReader(in));
    }


    public void read(Reader r) {
	String line = null;
	BufferedReader in = null;
	try {
	    in = new BufferedReader(r);
	    while ((line = in.readLine()) != null) {
		add(line);
	    }
	}
	catch (IOException e) {
	    //e.printStackTrace();
	}
	finally {
	    try {
		in.close();
	    }
	    catch (IOException e) {
		/* ignore */
	    }
	}

    }

    /**
     *  Constructor for the StringList object
     *
     * @param  fileName  The file to open
     */
    public void read(String fileName) {
	try {
	    read(new FileReader(fileName));
	}
	catch(IOException e) {
	    //e.printStackTrace();
	}
    }

    /**
     *  Save the String to named file
     *
     * @param  fileName  The name of the file to save to
     */
    public void save(String fileName) {
	PrintWriter out = null;
	try {
	    out = new PrintWriter(new FileOutputStream(fileName), true);
	    for (int i = 0; i < size(); i++) {
		out.println((String) get(i));
	    }
	}
	catch (IOException e) {
	    //e.printStackTrace();
	}
	finally {
	    if (out != null) {
		out.close();
	    }
	}
    }

}
