package org.clearsighted.tutorbase;

/**
 * A general exception for messages that should reach the xPST author as feedback, like parse errors in the .xpst file.
 *
 */
public class TutorException extends Exception
{
	private static final long serialVersionUID = 7330817162482652171L;
	
	public TutorException(String message, Throwable cause)
	{
		super(message, cause);
	}
}
