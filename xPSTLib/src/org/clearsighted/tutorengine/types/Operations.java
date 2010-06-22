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

import java.util.HashMap;

import org.clearsighted.tutorengine.GoalNode;

public class Operations
{
	public enum Op {LThanOp, GThanOp, LEqOp, GEqOp, EqOp, NotEqOp};

	public static boolean doOp(Op op, Object l, Object r,HashMap<String, GoalNode> gns)
	{
		// TODO: I know this isn't very pluggable, but I can't see the structure of the
		// necessary interfaces until I see the requirements, so I'll just code it in
		// a big nasty lump for now.

		if (op == Op.EqOp && ((l instanceof RegEx && r instanceof String) || (r instanceof RegEx && l instanceof String)))
		{
			if (l instanceof RegEx)
				return ((RegEx)l).check(gns,(String)r);
			if (r instanceof RegEx)
				return ((RegEx)r).check(gns,(String)l);
		}
		
		if((l instanceof Sum && r instanceof String))
		{
			switch (op)
			{
				case GEqOp:
					return ((Sum)l).compare(gns,(String)r,"ge");
				case GThanOp:
					return ((Sum)l).compare(gns,(String)r,"g");
				case LEqOp:
					return ((Sum)l).compare(gns,(String)r,"le");
				case LThanOp:
					return ((Sum)l).compare(gns,(String)r,"l");
				case EqOp:
					return ((Sum)l).compare(gns,(String)r,"e");
				case NotEqOp:
					return ((Sum)l).compare(gns,(String)r,"ne");
				default:
					return false;
			}
		}

		if((r instanceof Sum && l instanceof String))
		{
			switch (op)
			{
				case GEqOp:
					return ((Sum)r).compare(gns,(String)l,"ge");
				case GThanOp:
					return ((Sum)r).compare(gns,(String)l,"g");
				case LEqOp:
					return ((Sum)r).compare(gns,(String)l,"le");
				case LThanOp:
					return ((Sum)r).compare(gns,(String)l,"l");
				case EqOp:
					return ((Sum)r).compare(gns,(String)l,"e");
				case NotEqOp:
					return ((Sum)r).compare(gns,(String)l,"ne");
				default:
					return false;
			}
		}		
		
		if((l instanceof Multiply && r instanceof String))
		{
			switch (op)
			{
				case GEqOp:
					return ((Multiply)l).compare(gns,(String)r,"ge");
				case GThanOp:
					return ((Multiply)l).compare(gns,(String)r,"g");
				case LEqOp:
					return ((Multiply)l).compare(gns,(String)r,"le");
				case LThanOp:
					return ((Multiply)l).compare(gns,(String)r,"l");
				case EqOp:
					return ((Multiply)l).compare(gns,(String)r,"e");
				case NotEqOp:
					return ((Multiply)l).compare(gns,(String)r,"ne");
				default:
					return false;
			}
		}

		if((r instanceof Multiply && l instanceof String))
		{
			switch (op)
			{
				case GEqOp:
					return ((Multiply)r).compare(gns,(String)l,"ge");
				case GThanOp:
					return ((Multiply)r).compare(gns,(String)l,"g");
				case LEqOp:
					return ((Multiply)r).compare(gns,(String)l,"le");
				case LThanOp:
					return ((Multiply)r).compare(gns,(String)l,"l");
				case EqOp:
					return ((Multiply)r).compare(gns,(String)l,"e");
				case NotEqOp:
					return ((Multiply)r).compare(gns,(String)l,"ne");
				default:
					return false;
			}
		}
		
		if((l instanceof Divide && r instanceof String))
		{
			switch (op)
			{
				case GEqOp:
					return ((Divide)l).compare(gns,(String)r,"ge");
				case GThanOp:
					return ((Divide)l).compare(gns,(String)r,"g");
				case LEqOp:
					return ((Divide)l).compare(gns,(String)r,"le");
				case LThanOp:
					return ((Divide)l).compare(gns,(String)r,"l");
				case EqOp:
					return ((Divide)l).compare(gns,(String)r,"e");
				case NotEqOp:
					return ((Divide)l).compare(gns,(String)r,"ne");
				default:
					return false;
			}
		}

		if((r instanceof Divide && l instanceof String))
		{
			switch (op)
			{
				case GEqOp:
					return ((Divide)r).compare(gns,(String)l,"ge");
				case GThanOp:
					return ((Divide)r).compare(gns,(String)l,"g");
				case LEqOp:
					return ((Divide)r).compare(gns,(String)l,"le");
				case LThanOp:
					return ((Divide)r).compare(gns,(String)l,"l");
				case EqOp:
					return ((Divide)r).compare(gns,(String)l,"e");
				case NotEqOp:
					return ((Divide)r).compare(gns,(String)l,"ne");
				default:
					return false;
			}
		}	
		
		if((l instanceof Subtract && r instanceof String))
		{
			switch (op)
			{
				case GEqOp:
					return ((Subtract)l).compare(gns,(String)r,"ge");
				case GThanOp:
					return ((Subtract)l).compare(gns,(String)r,"g");
				case LEqOp:
					return ((Subtract)l).compare(gns,(String)r,"le");
				case LThanOp:
					return ((Subtract)l).compare(gns,(String)r,"l");
				case EqOp:
					return ((Subtract)l).compare(gns,(String)r,"e");
				case NotEqOp:
					return ((Subtract)l).compare(gns,(String)r,"ne");
				default:
					return false;
			}
		}

		if((r instanceof Subtract && l instanceof String))
		{
			switch (op)
			{
				case GEqOp:
					return ((Subtract)r).compare(gns,(String)l,"ge");
				case GThanOp:
					return ((Subtract)r).compare(gns,(String)l,"g");
				case LEqOp:
					return ((Subtract)r).compare(gns,(String)l,"le");
				case LThanOp:
					return ((Subtract)r).compare(gns,(String)l,"l");
				case EqOp:
					return ((Subtract)r).compare(gns,(String)l,"e");
				case NotEqOp:
					return ((Subtract)r).compare(gns,(String)l,"ne");
				default:
					return false;
			}
		}	
		
		if((l instanceof Ans && r instanceof String))
		{
			switch (op)
			{
				case EqOp:
					return ((Ans)l).compare(gns,(String)r,"e");
				case NotEqOp:
					return ((Ans)l).compare(gns,(String)r,"ne");
				default:
					return false;
			}
		}

		if((r instanceof Ans && l instanceof String))
		{
			switch (op)
			{
				case EqOp:
					return ((Ans)r).compare(gns,(String)l,"e");
				case NotEqOp:
					return ((Ans)r).compare(gns,(String)l,"ne");
				default:
					return false;
			}
		}	
		
		if((l instanceof NumSum && r instanceof String))
		{
			switch (op)
			{
				case GEqOp:
					return ((NumSum)l).compare(gns,(String)r,"ge");
				case GThanOp:
					return ((NumSum)l).compare(gns,(String)r,"g");
				case LEqOp:
					return ((NumSum)l).compare(gns,(String)r,"le");
				case LThanOp:
					return ((NumSum)l).compare(gns,(String)r,"l");
				case EqOp:
					return ((NumSum)l).compare(gns,(String)r,"e");
				case NotEqOp:
					return ((NumSum)l).compare(gns,(String)r,"ne");
				default:
					return false;
			}
		}

		if((r instanceof NumSum && l instanceof String))
		{
			switch (op)
			{
				case GEqOp:
					return ((NumSum)r).compare(gns,(String)l,"ge");
				case GThanOp:
					return ((NumSum)r).compare(gns,(String)l,"g");
				case LEqOp:
					return ((NumSum)r).compare(gns,(String)l,"le");
				case LThanOp:
					return ((NumSum)r).compare(gns,(String)l,"l");
				case EqOp:
					return ((NumSum)r).compare(gns,(String)l,"e");
				case NotEqOp:
					return ((NumSum)r).compare(gns,(String)l,"ne");
				default:
					return false;
			}
		}
		
		if((l instanceof DenomSum && r instanceof String))
		{
			switch (op)
			{
				case GEqOp:
					return ((DenomSum)l).compare(gns,(String)r,"ge");
				case GThanOp:
					return ((DenomSum)l).compare(gns,(String)r,"g");
				case LEqOp:
					return ((DenomSum)l).compare(gns,(String)r,"le");
				case LThanOp:
					return ((DenomSum)l).compare(gns,(String)r,"l");
				case EqOp:
					return ((DenomSum)l).compare(gns,(String)r,"e");
				case NotEqOp:
					return ((DenomSum)l).compare(gns,(String)r,"ne");
				default:
					return false;
			}
		}

		if((r instanceof NumSum && l instanceof String))
		{
			switch (op)
			{
				case GEqOp:
					return ((DenomSum)r).compare(gns,(String)l,"ge");
				case GThanOp:
					return ((DenomSum)r).compare(gns,(String)l,"g");
				case LEqOp:
					return ((DenomSum)r).compare(gns,(String)l,"le");
				case LThanOp:
					return ((DenomSum)r).compare(gns,(String)l,"l");
				case EqOp:
					return ((DenomSum)r).compare(gns,(String)l,"e");
				case NotEqOp:
					return ((DenomSum)r).compare(gns,(String)l,"ne");
				default:
					return false;
			}
		}
		
		if((l instanceof IsMultiple && r instanceof String))
		{
			switch (op)
			{
				case EqOp:
					return ((IsMultiple)l).compare(gns,(String)r,"e");
				case NotEqOp:
					return ((IsMultiple)l).compare(gns,(String)r,"ne");
				default:
					return false;
			}
		}

		if((r instanceof IsMultiple && l instanceof String))
		{
			switch (op)
			{
				case EqOp:
					return ((IsMultiple)r).compare(gns,(String)l,"e");
				case NotEqOp:
					return ((IsMultiple)r).compare(gns,(String)l,"ne");
				default:
					return false;
			}
		}	

		if((l instanceof IsNotMultiple && r instanceof String))
		{
			switch (op)
			{
				case EqOp:
					return ((IsNotMultiple)l).compare(gns,(String)r,"e");
				case NotEqOp:
					return ((IsNotMultiple)l).compare(gns,(String)r,"ne");
				default:
					return false;
			}
		}

		if((r instanceof IsNotMultiple && l instanceof String))
		{
			switch (op)
			{
				case EqOp:
					return ((IsNotMultiple)r).compare(gns,(String)l,"e");
				case NotEqOp:
					return ((IsNotMultiple)r).compare(gns,(String)l,"ne");
				default:
					return false;
			}
		}
		
		if((l instanceof Lcm && r instanceof String))
		{
			switch (op)
			{
				case GEqOp:
					return ((Lcm)l).compare(gns,(String)r,"ge");
				case GThanOp:
					return ((Lcm)l).compare(gns,(String)r,"g");
				case LEqOp:
					return ((Lcm)l).compare(gns,(String)r,"le");
				case LThanOp:
					return ((Lcm)l).compare(gns,(String)r,"l");
				case EqOp:
					return ((Lcm)l).compare(gns,(String)r,"e");
				case NotEqOp:
					return ((Lcm)l).compare(gns,(String)r,"ne");
				default:
					return false;
			}
		}

		if((r instanceof Lcm && l instanceof String))
		{
			switch (op)
			{
				case GEqOp:
					return ((Lcm)r).compare(gns,(String)l,"ge");
				case GThanOp:
					return ((Lcm)r).compare(gns,(String)l,"g");
				case LEqOp:
					return ((Lcm)r).compare(gns,(String)l,"le");
				case LThanOp:
					return ((Lcm)r).compare(gns,(String)l,"l");
				case EqOp:
					return ((Lcm)r).compare(gns,(String)l,"e");
				case NotEqOp:
					return ((Lcm)r).compare(gns,(String)l,"ne");
				default:
					return false;
			}
		}
		
		
		if((l instanceof EqNumerator && r instanceof String))
		{
			switch (op)
			{
				case GEqOp:
					return ((EqNumerator)l).compare(gns,(String)r,"ge");
				case GThanOp:
					return ((EqNumerator)l).compare(gns,(String)r,"g");
				case LEqOp:
					return ((EqNumerator)l).compare(gns,(String)r,"le");
				case LThanOp:
					return ((EqNumerator)l).compare(gns,(String)r,"l");
				case EqOp:
					return ((EqNumerator)l).compare(gns,(String)r,"e");
				case NotEqOp:
					return ((EqNumerator)l).compare(gns,(String)r,"ne");
				default:
					return false;
			}
		}

		if((r instanceof EqNumerator && l instanceof String))
		{
			switch (op)
			{
				case GEqOp:
					return ((EqNumerator)r).compare(gns,(String)l,"ge");
				case GThanOp:
					return ((EqNumerator)r).compare(gns,(String)l,"g");
				case LEqOp:
					return ((EqNumerator)r).compare(gns,(String)l,"le");
				case LThanOp:
					return ((EqNumerator)r).compare(gns,(String)l,"l");
				case EqOp:
					return ((EqNumerator)r).compare(gns,(String)l,"e");
				case NotEqOp:
					return ((EqNumerator)r).compare(gns,(String)l,"ne");
				default:
					return false;
			}
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
