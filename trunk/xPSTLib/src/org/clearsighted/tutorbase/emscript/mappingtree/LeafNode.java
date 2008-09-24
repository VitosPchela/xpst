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

public class LeafNode extends MappingNode
{
	public Mapping myMapping;
	public boolean completed = false;
	private boolean hold = false;
	private boolean holdMapped = false;

	public LeafNode(Tree tree, Mapping mapping)
	{
		super(tree);
		myMapping = mapping;
	}

	@Override
	public void amCompleted(MappingNode self)
	{
		// can't happen
	}

	@Override
	public void amStarted(MappingNode self)
	{
		// probably can't happen
	}

	@Override
	public void amUncompleted(MappingNode self)
	{
		// can't happen
	}

	@Override
	public void setMapped(boolean imapped)
	{
		if (hold)
		{
			holdMapped = imapped;
			if (!imapped)
				return;
		}

		if (mapped != imapped)
		{
			mapped = imapped;
			myTree.onMappedStateChanged(this, imapped);
		}
	}

	@Override
	public void setHold(boolean ihold)
	{
		hold = ihold;
		if (ihold)
			holdMapped = mapped;
		else
			mapped = holdMapped;
	}

	public void complete()
	{
		if (completed)
			return;
		completed = true;
		setMapped(false);
		if (parent != null)
		{
			parent.amCompleted(this);
			parent.amStarted(this);
		}
		else
			myTree.setDone(true);
	}

	public void uncomplete()
	{
		if (!completed)
			return;
		completed = false;
		setMapped(true);
		if (parent != null)
			parent.amUncompleted(this);
		else
			myTree.setDone(false);
	}

	public boolean isActive()
	{
		if (hold)
			return holdMapped;
		else
			return mapped;
	}
}
