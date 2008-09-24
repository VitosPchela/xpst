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

package org.clearsighted.tutorengine.types;

public class Operations
{
	public enum Op {LThanOp, GThanOp, LEqOp, GEqOp, EqOp, NotEqOp};

	public static boolean doOp(Op op, Object l, Object r)
	{
		// TODO: I know this isn't very pluggable, but I can't see the structure of the
		// necessary interfaces until I see the requirements, so I'll just code it in
		// a big nasty lump for now.

		if (op == Op.EqOp && ((l instanceof RegEx && r instanceof String) || (r instanceof RegEx && l instanceof String)))
		{
			if (l instanceof RegEx)
				return ((RegEx)l).check((String)r);
			if (r instanceof RegEx)
				return ((RegEx)r).check((String)l);
		}

		double lv = 0, rv = 0;
		try
		{
			if (l instanceof String)
				lv = Double.parseDouble((String)l);
			else if (l instanceof Integer)
				lv = new Double((Integer)l);
			else if (l instanceof Double)
				lv = (Double)l;

			if (r instanceof String)
				rv = Double.parseDouble((String)r);
			else if (r instanceof Integer)
				rv = new Double((Integer)r);
			else if (r instanceof Double)
				rv = (Double)r;
		}
		catch (NumberFormatException e)
		{
			String lvs = null, rvs = null;
			if (l instanceof Double)
				lvs = Double.toString((Double)l);
			else if (l instanceof Integer)
				lvs = Integer.toString((Integer)l);
			else if (l instanceof String)
				lvs = (String)l;

			if (r instanceof Double)
				rvs = Double.toString((Double)r);
			else if (r instanceof Integer)
				rvs = Integer.toString((Integer)r);
			else if (r instanceof String)
				rvs = (String)r;
			
			if (op == Op.EqOp)
				return lvs.equals(rvs);
			else
				;   // TODO
		}

		switch (op)
		{
			case GEqOp:
				return lv >= rv;
			case GThanOp:
				return lv > rv;
			case LEqOp:
				return lv <= rv;
			case LThanOp:
				return lv < rv;
			case EqOp:
				return lv == rv;
			case NotEqOp:
				return lv != rv;
			default:
				return false;
		}
	}
}
