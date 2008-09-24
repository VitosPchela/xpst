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

public abstract class MappingNode
{
	protected MappingNode leftChild = null, rightChild = null;
	protected MappingNode parent = null;
	protected Tree myTree = null;
	protected boolean mapped = false;
	protected int column, line;
	public String name = null;

	public abstract void amCompleted(MappingNode self);
	public abstract void amStarted(MappingNode self);
	public abstract void amUncompleted(MappingNode self);
	public abstract void setMapped(boolean active);

	public MappingNode(Tree mytree)
	{
		myTree = mytree;
	}

	public void setParent(MappingNode iparent)
	{
		parent = iparent;
	}
	
	public void setChildren(MappingNode left, MappingNode right)
	{
		leftChild = left;
		rightChild = right;
	}

	public MappingNode getLeftChild()
	{
		return leftChild;
	}

	public MappingNode getRightChild()
	{
		return rightChild;
	}
	
	public void setHold(boolean hold)
	{
		if (leftChild != null)
			leftChild.setHold(hold);
		if (rightChild != null)
			rightChild.setHold(hold);
	}

	public boolean isMapped()
	{
		return mapped;
	}
}
