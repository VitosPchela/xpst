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

package org.clearsighted.tutorbase.emscript.exprtree;

import java.util.HashMap;

import org.clearsighted.tutorbase.emscript.EMScriptParserTokenTypes;
import org.clearsighted.tutorengine.GoalNode;
import org.clearsighted.tutorengine.Type;

import antlr.collections.AST;


public class ConstructorNode extends ExprNode
{
	private Type object = null;
	public ConstructorNode(AST args)
	{
		AST node = args.getFirstChild();
		object = ConstructorNode.constructObject(args);
		node = node.getNextSibling();
	}
	
	@Override
	public Object eval(ExprEnv ee,HashMap<String, GoalNode> gns) throws ExprException
	{
		return object;
	}

	public static Type constructObject(AST ast)
	{
		String typename = null;
		Object[] typeargs = null;
		ast = ast.getFirstChild();
		
		if (ast.getType() == EMScriptParserTokenTypes.ID)
		{
			typename = ast.getText();
			typeargs = new Object[ast.getNumberOfChildren()];
			int i = 0;
			AST child = ast.getFirstChild();
			while (child != null)
			{
				Object childval = null;
				if (child.getType() == EMScriptParserTokenTypes.STRINGLIT)
					childval = child.getText();
				else if (child.getType() == EMScriptParserTokenTypes.INT)
					childval = Integer.parseInt(child.getText());
				else if (child.getType() == EMScriptParserTokenTypes.FLOATLIT)
					childval = Double.parseDouble(child.getText());
				else if (child.getType() == EMScriptParserTokenTypes.COMMA)
					childval = child.getText();
				typeargs[i] = childval;
				i++;
				child = child.getNextSibling();
			}
		}
		else if (ast.getType() == EMScriptParserTokenTypes.STRINGLIT)
		{
			typename = "StringT";
			typeargs = new Object[1];
			typeargs[0] = ast.getText();
		}
		else if (ast.getType() == EMScriptParserTokenTypes.INT)
		{
			typename = "IntegerT";
			typeargs = new Object[1];
			typeargs[0] = Integer.parseInt(ast.getText());
		}
		else if (ast.getType() == EMScriptParserTokenTypes.FLOATLIT)
		{
			typename = "DoubleT";
			typeargs = new Object[1];
			typeargs[0] = Double.parseDouble(ast.getText());
		}

		Type obj = null;

		try
		{
			Class ctclass = Class.forName("org.clearsighted.tutorengine.types." + typename);
			obj = (Type)ctclass.getConstructor().newInstance();
			obj.construct(typeargs);
		}
		catch (Exception e)
		{
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
		
		return obj;
	}
}
