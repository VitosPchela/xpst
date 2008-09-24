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

package org.clearsighted.tutorbase.emscript.mappingtree;

public class UntilNode extends MappingNode
{
	public UntilNode(Tree mytree)
	{
		super(mytree);
	}

	@Override
	public void amCompleted(MappingNode self)
	{
		if (self == leftChild)
			rightChild.setMapped(true);
		else if (self == rightChild)
		{
			leftChild.setHold(false);
			if (parent != null)
				parent.amCompleted(this);
			else
				myTree.setDone(true);
		}
	}

	@Override
	public void amStarted(MappingNode self)
	{
		if (parent != null)
			parent.amStarted(this);
	}

	@Override
	public void amUncompleted(MappingNode self)
	{
		if (self == leftChild)
			rightChild.setMapped(false);
		else if (self == rightChild)
		{
			leftChild.setHold(true);
			if (parent != null)
				parent.amUncompleted(this);
			else
				myTree.setDone(false);
		}
	}

	@Override
	public void setMapped(boolean mapped)
	{
		if (mapped)
		{
			leftChild.setHold(true);
			leftChild.setMapped(true);
		}
		else
		{
			leftChild.setHold(false);
			leftChild.setMapped(false);
			rightChild.setMapped(false);
		}
	}
}
