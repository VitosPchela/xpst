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

import org.clearsighted.tutorbase.ParseUtil;

/// <summary>
/// Describes a message target for a Dormin message. Basically a string of names
/// running down a heirarchy.
/// </summary>
public class DorminAddress
{
	/** Special node that means that the event mapper script is trying to send to a node that doesn't exist */
	public static String SpecialNonExistent = "Special.NonExistent"; 
	/** Special node that means that the event mapper script is trying to send to a node is currently disabled */
	public static String SpecialDisabled = "Special.Disabled"; 
	///
	public String[] Names;
	/// <summary>
	/// Constructor, given an array of individual names
	/// </summary>
	/// <param name="names">the individual names</param>
	public DorminAddress(String[] names)
	{
		Names = names;
	}

	/// <summary>
	/// Constructor, given a dotten name like foo.bar.baz
	/// </summary>
	/// <param name="dottedname">the dotted name</param>
	public DorminAddress(String dottedname)
	{
		Names = dottedname.split("\\.");
	}

	public DorminAddress(DorminAddress addr)
	{
		Names = addr.Names.clone();
	}
	
	/// <summary>
	/// Convert the address to a nice string representation, like foo.bar.baz
	/// </summary>
	/// <returns>Dotted string representation of address</returns>
	public String toDottedString()
	{
		return ParseUtil.join(Names, ".");
	}
}
