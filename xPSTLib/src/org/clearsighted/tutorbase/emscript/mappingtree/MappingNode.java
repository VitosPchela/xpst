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

/**
 * Abstract class that implements binary tree functionality and defines
 * the methods that operator and leaf nodes must implement.
 *
 */
public abstract class MappingNode
{
	protected MappingNode leftChild = null, rightChild = null;
	protected MappingNode parent = null;
	protected Tree myTree = null;
	protected boolean mapped = false;
	protected int column, line;
	public String name = null;

	/**
	 * Message sent up from a node designating that it is completed (received an APPROVE message).
	 * @param self the node sending the message
	 */
	public abstract void amCompleted(MappingNode self);
	/**
	 * Message sent up from a node designating that it is started. This is a little complicated, but
	 * it's basically used to implement a specific behavior of the 'or' operator. When the first node
	 * of a subexpression of an 'or' operator begins, the 'or' operator should turn off the other
	 * subexpression (rather than waiting until an entire subexpression is completed). So leaf nodes
	 * send this when they're completed, and all nodes except 'or' just pass it up, until the 'or'
	 * uses it to turn off its other branch.
	 * @param self the node sending the message
	 */
	public abstract void amStarted(MappingNode self);
	/**
	 * Message sent up from a node designating that it is uncompleted (was completed, but then got FLAGged again).
	 * @param self the node sending the message
	 */
	public abstract void amUncompleted(MappingNode self);
	/**
	 * Sets the 'mapped' status of the node, unless this is overridden by the 'hold' status.
	 * @see MappingNode#setHold(boolean)
	 * @param mapped
	 */
	public abstract void setMapped(boolean mapped);

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

	/**
	 * Sets the 'hold' status on the node, which is used to implement the 'until' operator.
	 * If 'hold' status is on, then nodes should remember whether they are set to mapped or
	 * unmapped status, but not act on such changes until the hold is released. The 'until'
	 * operator releases the 'hold' status on its children when both of its subexpressions
	 * are completed. 
	 * @param hold
	 */
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
