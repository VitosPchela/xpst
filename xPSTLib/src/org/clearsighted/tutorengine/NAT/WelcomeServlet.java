package org.clearsighted.tutorengine.NAT;
//http://www.jsptube.com/servlet-tutorials/simple-servlet-example.html


import java.io.IOException;
import java.io.PrintWriter;
import java.util.ArrayList;
 
import javax.servlet.ServletConfig;
import javax.servlet.ServletException;
import javax.servlet.http.HttpServlet;
import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;
 
public class WelcomeServlet extends HttpServlet {
 
@Override
public void init(ServletConfig config) throws ServletException {
super.init(config);
}
 
 
protected void doPost(HttpServletRequest request, HttpServletResponse response) throws ServletException, IOException {
	
	response.setContentType("text/plain");
	PrintWriter out = response.getWriter();
		
	String c = request.getParameter("checktype");
	String r = request.getParameter("response");
	//String r = "Shrenik";
	String s ="";
	boolean flag = false;
	
	String[] individualArray = c.split("(?<=\\))\\s+or\\s+");
	ArrayList<Start> individualChecktypeList = new ArrayList<Start>();
	
	try
	{
		//out.println("Checktype            :" + c);
		//out.println("Response             :" + r);
		
		for (int i=0; i<individualArray.length; i++) 
		{
			Start tempCheck= new Start(r,s,individualArray[i]);
			individualChecktypeList.add(tempCheck);
			
			if(tempCheck.isresponseMatched())
				flag = true;
		}	
		if(flag)
			//out.println("RESULT               :Checktype matched.");
			out.println("true");
		else
			//out.println("RESULT               :No match.");
			out.println("false");
		
		out.close();
	}
	catch(ArrayIndexOutOfBoundsException e)
	{
		//out.println("ERROR                :Invalid input.");
		out.println("error");
	}
	
	}
	 
	 
	public void destroy() {
	 
	}
} 