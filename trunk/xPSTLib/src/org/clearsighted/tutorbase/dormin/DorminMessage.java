/*
Copyright (c) Clearsighted 2006-08 stephen@clearsighted.net

This library is free software; you can redistribute it and/or
modify it under the terms of the GNU Lesser General Public
License as published by the Free Software Foundation; either
version 2.1 of the License, or (at your option) any later version.

This library is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
Lesser General Public License for more details.

You should have received a copy of the GNU Lesser General Public
License along with this library; if not, write to the Free Software
Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301  USA
*/

package org.clearsighted.tutorbase.dormin;

import java.io.IOException;
import java.io.StringReader;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.Map;
import java.util.Map.Entry;

/**
 * Represents a Dormin message as Java objects.
 */
public class DorminMessage
{
	/** static String to use for a "NOTEVALUESET" verb */
	public static String NoteValueSetVerb = "NOTEVALUESET";
	/** static String to use for a "FLAG" verb */
	public static String FlagVerb = "FLAG";
	/** static String to use for a "APPROVE" verb */
	public static String ApproveVerb = "APPROVE";
	/** static String to use for a "HINTMESSAGE" verb */
	public static String HintMessageVerb = "HINTMESSAGE";
	/** static String to use for a "JITMESSAGE" verb */
	public static String JITMessageVerb = "JITMESSAGE";
	/** static String to use for a "GETHINT" verb */
	public static String GetHintVerb = "GETHINT";
	/** static String to use for a "SETPROPERTY" verb */
	public static String SetPropertyVerb = "SETPROPERTY";
	/** static String to use for a "CREATE" verb */
	public static String CreateVerb = "CREATE";
	/** static String to use for a "ENUMAVAILABLENODES" verb (Clearsighted-specific) */
	public static String EnumAvailableNodesVerb = "ENUMAVAILABLENODES";
	/** static String to use for a "NOTEAVAILABLENODES" verb (Clearsighted-specific) */
	public static String NoteAvailableNodesVerb = "NOTEAVAILABLENODES";
	/** static String to use for a "STOPTRE" verb (Clearsighted-specific) */
	public static String StopTREVerb = "STOPTRE";
	/** static String to use for a "NOTEINITIALVALUE" verb (Clearsighted-specific) */
	public static String NoteInitialValue = "NOTEINITIALVALUE";
	/** static String to use for a "QUERYINITIALVALUE" verb (Clearsighted-specific) */
	public static String QueryInitialValue = "QUERYINITIALVALUE";

	public static String ToolNode = "tool";
	public static String TutorNode = "tutor";

	/**
	 *  The Dormin verb.
	 */
	public String Verb;
	/**
	 *  A message sequence number that should usually be incremented for each message.
	 *  The constructor does this automatically, but it can be overwritten if necessary.
	 */
	public int MessageNum;
	/** The address of the target of the message. */
	public DorminAddress Address;
	/** Parameters for the message. */
	public DorminParameter[] Parameters;

	static int NextMessageNum = 0;

	/** Mostly for setting up in unit tests. */
	public static void resetMessageNum()
	{
		NextMessageNum = 0;
	}

	/**
	 *  Construct a fresh Dormin message. Fill in the necessary fields before
	 *  sending (MessageNumber will be initialized for you unless you need to override it)
	 */
	public DorminMessage()
	{
		MessageNum = NextMessageNum++;
	}

	/** Construct a fresh Dormin message with address, verb, and value */
	public DorminMessage(String address, String verb, String value)
	{
		MessageNum = NextMessageNum++;
		Address = new DorminAddress(address);
		Verb = verb;
		if (value != null)
			Parameters = new DorminParameter[] { new DorminParameter("VALUE", value) };
	}

	/** Copy constructor */
	public DorminMessage(DorminMessage in)
	{
		Address = new DorminAddress(in.Address);
		Verb = in.Verb;
		Parameters = null;
		if (in.Parameters != null)
			Parameters = in.Parameters.clone();
	}

	// BEGIN: Dormin parsing functions. This is a simple recursive descent parser.
	private static String readThrough(StringReader sr, char delim)
	{
		String ret = "";
		try
		{
			int c;
			c = sr.read();
			while (c != delim && c != -1)
			{
				ret += (char)c;
				c = sr.read();
			}
		} catch (IOException e)
		{
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
		return ret;
	}

	private static String readNext(StringReader sr, int num)
	{
		char[] buf = new char[num];
		try
		{
			sr.read(buf, 0, num);
		} catch (IOException e)
		{
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
		return new String(buf);
	}

	private static String readLengthString(StringReader sr)
	{
		int len = Integer.parseInt(readThrough(sr, ':'));
		String ret = readNext(sr, len);
		return ret;
	}

	private static boolean matchLiteral(StringReader sr, String literal)
	{
		char[] buf = new char[literal.length()];
		try
		{
			sr.read(buf, 0, literal.length());
		} catch (IOException e)
		{
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
		if (new String(buf).equals(literal))
			return true;
		else
			return false;
//		throw new Exception("expecting " + literal);
	}

	private static boolean matchAmp(StringReader sr)
	{
		return matchLiteral(sr, "&");
	}

	private static void matchPreamble(StringReader sr)
	{
		matchLiteral(sr, "SE/1.2");
	}

	private static String unescapeString(String str)
	{
		str = str.replace("\\n", "\n");
		str = str.replace("\\\\", "\\");
		return str;
	}

	private static String matchDorminString(StringReader sr)
	{
		matchLiteral(sr, "S:");
		String str = readLengthString(sr);
		return unescapeString(str);
	}

	private static String matchVerb(StringReader sr)
	{
		matchLiteral(sr, "VERB=");
		return matchDorminString(sr);
	}

	private static int matchDorminInteger(StringReader sr)
	{
		matchLiteral(sr, "I:");
		return Integer.parseInt(readLengthString(sr));
	}

	private static int matchMessageNumber(StringReader sr)
	{
		matchLiteral(sr, "MESSAGENUMBER=");
		return matchDorminInteger(sr);
	}

	private static String matchType(StringReader sr)
	{
		return matchDorminString(sr);
	}

	private static String matchDorminIdentifier(StringReader sr)
	{
		// TODO: maybe account for POSITION identifiers, but I've been told they aren't really used any more
		/*String type = */matchType(sr);
		matchLiteral(sr, ",");
		/*String idtype = */matchDorminString(sr);
		matchLiteral(sr, ",");
		return matchDorminString(sr);
	}

	private static DorminAddress matchDorminAddress(StringReader sr)
	{
		ArrayList<String> ret = new ArrayList<String>();
		matchLiteral(sr, "OBJECT=");
		matchLiteral(sr, "O:");
		int numels = Integer.parseInt(readThrough(sr, ':'));
		for (int i = 0; i < numels; i++)
		{
			ret.add(matchDorminIdentifier(sr));
			if (i < numels - 1)
				matchLiteral(sr, ",");
		}
		// DEBUG: shouldn't have to do this, but toArray seems to have a bug...
		ret.trimToSize();
		return new DorminAddress(ret.toArray(new String[1]));
	}

	private static Object matchDorminFloat(StringReader sr)
	{
		matchLiteral(sr, "F:");
		return (float)Double.parseDouble(readLengthString(sr));
	}

	private static Object matchDorminDouble(StringReader sr)
	{
		matchLiteral(sr, "D:");
		return Double.parseDouble(readLengthString(sr));
	}

	private static Object matchDorminBoolean(StringReader sr)
	{
		matchLiteral(sr, "B:");
		String s = readLengthString(sr);
		return s.equals("T");
	}

	private static Object matchDorminNull(StringReader sr)
	{
		matchLiteral(sr, "N:0:");
		return null;
	}

	private static DorminList matchDorminList(StringReader sr)
	{
		matchLiteral(sr, "L:");
		int numels = Integer.parseInt(readThrough(sr, ':'));
		matchLiteral(sr, "[");
		ArrayList<Object> ret = new ArrayList<Object>();
		for (int i = 0; i < numels; i++)
		{
			ret.add(matchDorminPrimitive(sr));
			if (i < numels - 1)
				matchLiteral(sr, ",");
		}
		matchLiteral(sr, "]");
		return new DorminList(ret.toArray());
	}

	private static Object matchDorminRange(StringReader sr)
	{
		// TODO
		return null;
	}
	
	private static Object matchDorminMap(StringReader sr)
	{
		matchLiteral(sr, "X:");
		int numels = Integer.parseInt(readThrough(sr, ':'));
		matchLiteral(sr, "[");
		Map<String, Object> ret = new HashMap<String, Object>();
		for (int i = 0; i < numels; i++)
		{
			matchLiteral(sr, "[");
			String key = matchDorminString(sr);
			matchLiteral(sr, ",");
			Object val = matchDorminPrimitive(sr); 
			ret.put(key, val);
			matchLiteral(sr, "]");
			if (i < numels - 1)
				matchLiteral(sr, ",");
		}
		matchLiteral(sr, "]");
		return new DorminMap(ret);
	}

	private static Object matchDorminPrimitive(StringReader sr)
	{
		int c = 0;
		try
		{
			c = stringReaderPeek(sr);
		} catch (IOException e)
		{
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
		switch (c)
		{
			case 'S':
				return matchDorminString(sr);
			case 'I':
				return matchDorminInteger(sr);
			case 'F':
				return matchDorminFloat(sr);
			case 'D':
				return matchDorminDouble(sr);
			case 'B':
				return matchDorminBoolean(sr);
			case 'N':
				return matchDorminNull(sr);
			case 'L':
				return matchDorminList(sr);
			case 'O':
				return matchDorminAddress(sr);
			case 'R':
				return matchDorminRange(sr);
			case 'X':
				return matchDorminMap(sr);
			default:
//				throw new Exception("bad dormin-primitive");
				return null;
		}
	}

	private static DorminParameter matchParameter(StringReader sr)
	{
		String name = readThrough(sr, '=');
		Object value = matchDorminPrimitive(sr);
		return new DorminParameter(name, value);
	}
	// END: Dormin parsing functions

	private static int stringReaderPeek(StringReader sr) throws IOException
	{
		sr.mark(1);
		int ret = sr.read();
		sr.reset();
		return ret;
	}

	private void fromString(String dorminstring)
	{
		StringReader sr = new StringReader(dorminstring);
		matchPreamble(sr);
		matchAmp(sr);
		Verb = matchVerb(sr);
		matchAmp(sr);
		MessageNum = matchMessageNumber(sr);
		matchAmp(sr);
		Address = matchDorminAddress(sr);
		matchAmp(sr);
		ArrayList<DorminParameter> parms = new ArrayList<DorminParameter>();
		try
		{
			while (stringReaderPeek(sr) != -1)
			{
				parms.add(matchParameter(sr));
				matchAmp(sr);
			}
		} catch (IOException e)
		{
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
		if (parms.size() > 0)
			Parameters = parms.toArray(new DorminParameter[0]);
	}

	/**
	 * Parse a Dormin string from the TRE and fill out a DorminMessage object.
	 */
	public static DorminMessage createFromString(String dorminstring)
	{
		DorminMessage ret = new DorminMessage();
		ret.fromString(dorminstring);
		return ret;
	}

	// BEGIN: Functions to translate DorminMessage objects into strings
	private static void appendDormin(StringBuilder sb, char type, String str)
	{
		sb.append(type);
		sb.append(":");
		sb.append(str.length());
		sb.append(":");
		sb.append(str);
	}

	private static void appendDorminString(StringBuilder sb, String str)
	{
		appendDormin(sb, 'S', str);
	}

	private static void appendDorminInteger(StringBuilder sb, int num)
	{
		appendDormin(sb, 'I', Integer.toString(num));
	}

	private static void appendDorminIdentifier(StringBuilder sb, String str)
	{
		appendDorminString(sb, "OBJECT");
		sb.append(",");
		appendDorminString(sb, "NAME");
		sb.append(",");
		appendDorminString(sb, str);
	}

	private static void appendDorminPrimitive(StringBuilder sb, Object val)
	{
		if (val == null)
			appendDorminNull(sb);
		else if (val instanceof String)
			appendDorminString(sb, (String)val);
		else if (val instanceof Integer)
			appendDorminInteger(sb, (Integer)val);
		else if (val instanceof Float)
			appendDorminFloat(sb, (Float)val);
		else if (val instanceof Double)
			appendDorminDouble(sb, (Double)val);
		else if (val instanceof Boolean)
			appendDorminBoolean(sb, (Boolean)val);
		else if (val instanceof DorminList)
			appendDorminList(sb, (DorminList)val);
		else if (val instanceof DorminMap)
			appendDorminMap(sb, (DorminMap)val);
		else if (val instanceof DorminAddress)
			appendDorminAddress(sb, (DorminAddress)val);
		else if (val instanceof DorminRange)
			appendDorminRange(sb, (DorminRange)val);
	}

	private static void appendDorminRange(StringBuilder sb, DorminRange dorminRange)
	{
		// TODO
//		throw new Exception("The method or operation is not implemented.");
	}

	private static void appendDorminAddress(StringBuilder sb, DorminAddress dorminAddress)
	{
		sb.append("OBJECT=");
		sb.append("O:");
		sb.append(Integer.toString(dorminAddress.Names.length));
		sb.append(":");
		for (int i = 0; i < dorminAddress.Names.length; i++)
		{
			appendDorminIdentifier(sb, dorminAddress.Names[i]);
			if (i < dorminAddress.Names.length - 1)
				sb.append(",");
		}
	}

	private static void appendDorminList(StringBuilder sb, DorminList dorminList)
	{
		sb.append("L:");
		sb.append(Integer.toString(dorminList.Items.length));
		sb.append(":");
		sb.append("[");
		for (int i = 0; i < dorminList.Items.length; i++)
		{
			appendDorminPrimitive(sb, dorminList.Items[i]);
			if (i < dorminList.Items.length - 1)
				sb.append(",");
		}
		sb.append("]");
	}
	
	private static void appendDorminMap(StringBuilder sb, DorminMap dorminMap)
	{
		sb.append("X:");
		sb.append(Integer.toString(dorminMap.Items.size()));
		sb.append(":");
		sb.append("[");
		for (Entry<String, Object> ent: dorminMap.Items.entrySet())
		{
			sb.append("[");
			appendDorminPrimitive(sb, ent.getKey());
			sb.append(",");
			appendDorminPrimitive(sb, ent.getValue());
			sb.append("],");
		}
		sb.deleteCharAt(sb.length() - 1);
		sb.append("]");
	}

	private static void appendDorminBoolean(StringBuilder sb, boolean p)
	{
		sb.append(p ? "TRUE" : "FALSE");
	}

	private static void appendDorminDouble(StringBuilder sb, double p)
	{
		appendDormin(sb, 'D', Double.toString(p));
	}

	private static void appendDorminFloat(StringBuilder sb, float p)
	{
		appendDormin(sb, 'F', Float.toString(p));
	}

	private static void appendDorminNull(StringBuilder sb)
	{
		sb.append("N:0:");
	}

	private static void appendDorminParameter(StringBuilder sb, DorminParameter dp)
	{
		sb.append(dp.Name);
		sb.append("=");
		appendDorminPrimitive(sb, dp.Value);
	}
	// END: Functions to translate DorminMessage objects into strings

	/**
	 * Get a string representation of the message, suitable for sending over the wire
	 * and for parsing by createFromString.
	 */
	@Override
	public String toString()
	{
		// TODO: generalize hardcoded values
		StringBuilder ret = new StringBuilder();
		ret.append("SE/1.2");
		ret.append("&");
		ret.append("VERB=");
		appendDorminString(ret, Verb);
		ret.append("&");
		ret.append("MESSAGENUMBER=");
		appendDorminInteger(ret, MessageNum);
		ret.append("&");
		appendDorminAddress(ret, Address);
		ret.append("&");
		if (Parameters != null)
		{
			for (int i = 0; i < Parameters.length; i++)
			{
				appendDorminParameter(ret, Parameters[i]);
				ret.append("&");
			}
		}
		return ret.toString();
	}

	/**
	 * Get a friendly string representation.
	 * @return less informative but more UI-friendly string representation of the message
	 */
	public String toFriendlyString()
	{
		StringBuilder sb = new StringBuilder();
		sb.append(Address.toDottedString());
		sb.append(':');
		sb.append(Verb);
		if (Parameters != null)
		{
			sb.append(',');
			for (int i = 0; i < Parameters.length; i++)
			{
				Object val = Parameters[i].Value;
				sb.append(val);
				if (i < Parameters.length - 1)
					sb.append(',');
			}
		}
		return sb.toString();
	}
}
