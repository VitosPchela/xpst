header
{
package org.clearsighted.tutorbase.emscript;

import java.util.HashMap;
import java.util.List;
import java.util.ArrayList;
import org.clearsighted.tutorbase.emscript.mappingtree.*;
import org.clearsighted.tutorbase.emscript.exprtree.*;
import org.clearsighted.tutorengine.*;
import org.clearsighted.tutorengine.types.Operations.Op;
}
/** Parses EMScript/xPST language */
class EMScriptParser extends Parser;
options
{
	buildAST = true;
	k = 2;
	defaultErrorHandler = false;
}

{
	HashMap<String, AST> Defines = new HashMap<String, AST>();
	String TutorName = null;
}

/** all sections are basically optional and can basically be in any order, but you're best
 * off if you include at least a 'mappings', at most one of 'option', 'sequence', and 'mappings'
 * and put things in the below-listed canonical order. */
top: (defines | sequence | mappings | include | option | feedback)* EOF! {#top = #([TOP, "top"], #top);};

/** defines group predefined elements, mostly for use in common include files */
defines!: DEFINES^ LBRACE! (defineexpr SEMI!)* RBRACE!;
defineexpr!: id:ID EQ! sw:switcht
{
	Defines.put(id.getText(), #sw);
};

/** include another file wholesale */
include: INCLUDE fname:STRINGLIT SEMI!
{
	try
	{
		EMScriptParser esp = EMScript.parse(getFilename(), fname.getText());
		#include = esp.getAST().getFirstChild();
		Defines.putAll(esp.Defines);
	}
	catch (Exception e)
	{
		e.printStackTrace();
	}
};

/** generic settings that apply to the whole file */
option: OPTIONS^ LBRACE! (optionexpr SEMI!)* RBRACE!;
/** the only option currently defined is to set the tutor name */
optionexpr: TUTORNAME EQ! stringlit;

/** description of the sequence in which goal nodes should be completed */
sequence: SEQUENCE^ LBRACE! seqexpr SEMI! RBRACE!;
/** 'then' and 'until' have higher precedence than 'and' and 'or' */
seqexpr: minseqexpr ((THEN^ | UNTIL^) minseqexpr)*;
minseqexpr: (atom | groupexpr) ((AND^ | OR^) (atom | groupexpr))*;
/** groups of nodes within brackets are mapped together, and can be cancelled
 * or OK'd by nodes marked as Cancel and OK in the group */
groupexpr: LBRACK! seqexpr RBRACK! { #groupexpr = #([GROUP, "GROUP"], #groupexpr); };
/** can group with parens */
atom: nodeid | LPAREN! seqexpr RPAREN!;

/** this serves for appnode IDs, goalnode IDs, and event IDs (appnode IDs plus a colon and an event name) */
 nodeid {String fullname = null;}:
	i1:ID {fullname = i1.getText();}
	(DOT i2:ID {fullname = fullname + "." + i2.getText();})*
	(COLON i3:ID {fullname = fullname + ":" + i3.getText();})?
{
 	#nodeid = #([NODEID, fullname]);
 	((EMAST)#nodeid).line = i1.getLine();
 	((EMAST)#nodeid).column = i1.getColumn();
}
  | s:STRINGLIT
{
  #nodeid = #([NODEID, s.getText()]);
  ((EMAST)#nodeid).line = s.getLine();
  ((EMAST)#nodeid).column = s.getColumn();
 };
 
/** the actual mappings from appnodes to goalnodes */
mappings: MAPPINGS^ LBRACE! (mapexpr)* RBRACE!;
mapexpr: (mapoptions)? mapexprhalf MAPTO^ mapexprhalf SEMI!;
/** can be just a goalnode ID, a 'switch' construct, or a 'concat' construct. */
mapexprhalf: fn:nodeid
{
	#mapexprhalf = #([MAPEXPRHALF, "MAPEXPRHALF"], #mapexprhalf);
}
	| switcht { #mapexprhalf = #([MAPEXPRHALF, "MAPEXPRHALF"], #mapexprhalf); }
	| concat { #mapexprhalf = #([MAPEXPRHALF, "MAPEXPRHALF"], #mapexprhalf); };

/** 'switch' maps a set of events to strings, which are then sent to a goalnode */
switcht: SWITCH^ LBRACE! switchexpr (COMMA! switchexpr)* (COMMA!)? RBRACE!;
/** each expression in the switch is a mapping from an event to a literal,
 * or a bundle already defined in a 'defines' section */
switchexpr: nodeid EQ! literal |
(PLUS! id:ID!)
{
	AST r = Defines.get(id.getText());
	// TODO: isn't there an easier way to clone some children into another tree?
	ASTFactory fact = new ASTFactory();
	if (r.getType() == SWITCH)
	{
		r = r.getFirstChild();
		#switchexpr = fact.dupTree(r);
		AST n = #switchexpr;
		r = r.getNextSibling();
		while (r != null)
		{
			AST rc = fact.dupTree(r);
			n.setNextSibling(rc);
			n = rc;
			r = r.getNextSibling();
		}
	}
};

/** 'concat' maps a set of values to a comma-delimited string, sent to a goalnode
 * after all the values have been sent and whenever one of them changes after that. */
concat: CONCAT^ LBRACE! concatexpr (COMMA! concatexpr)* (COMMA!)? RBRACE!;
concatexpr: nodeid;

/** options for an individual mapping */
mapoptions: LBRACK! mapoption (COMMA! mapoption)* RBRACK!
{
	#mapoptions = #([MAPOPTIONS, "MAPOPTIONS"], #mapoptions);
};
/** a single option for an individual mapping */
mapoption: PRIORITY^ EQ! INT | FOCUSEDONLY | NOQIV;

/** simple float literal (xyz.abc) */
floatlit: i:INT DOT m:INT
{
	String str = i.getText() + "." + m.getText();
	#floatlit = #([FLOATLIT, str]);
};
/** any of the built-in literal types */
literal: stringlit | INT | floatlit;
/** string literal, with some backslash-escaping */
stringlit: s:STRINGLIT
{
	String olds = s.getText();
	String news = "";
	int i = 0;
	while (i < olds.length())
	{
		int ni = olds.indexOf("\\", i);
		if (ni == -1)
		{
			news += olds.substring(i, olds.length());
			break;
		}
		else
		{
			news += olds.substring(i, ni) + olds.substring(ni + 1, ni + 2);
			i = ni + 2;
		}
	}
	#stringlit = #([STRINGLIT, news], #stringlit);
};

/** feedback section */
feedback: FEEDBACK^ LBRACE! feedbackstmt RBRACE!;
feedbackstmt: (variabledecls | nodefeedback)*;
/** variable declarations, which are really constant declarations at this point */
variabledecls: VARIABLES^ LBRACE! (variabledecl SEMI!)* RBRACE!;
/** feedback for one goalnode */
nodefeedback: nodeid LBRACE! ((hintdecl | jitdecl | compdecl | answerdecl | propertydecl) SEMI!)* RBRACE!
{
	#nodefeedback = #([NODEFEEDBACK, "NODEFEEDBACK"], #nodefeedback);
};
variabledecl: ID^ COLON! literal;
propertydecl: ID^ COLON! literal;
hintdecl: HINT^ COLON! stringlit;
jitdecl:
	JIT^ (LBRACE! valexpr RBRACE!) COLON! stringlit
	| JIT^ COLON! s:stringlit { #jitdecl = #(JIT, s); };
compdecl: ONCOMPLETE^ COLON! stringlit;
answerdecl: ANSWER^ COLON! constructor;
constructor: literal | callconstructor;
callconstructor: ID^ LPAREN! literal (COMMA literal)* RPAREN!;
valexpr: valcmpop ((DBLAND^ | DBLOR^) valcmpop)*;
valcmpop: valcalc ((LTHAN^ | GTHAN^ | DBLEQ^ | NOTEQ^ | LEQ^ | GEQ^) valcalc)*;
valcalc: valatom;
valatom: literal | ID | ANSWER | LPAREN! valexpr RPAREN! |
	callconstructor { #valatom = #([CONSTRUCTOR, "CONSTRUCTOR"], #valatom); };

class EMScriptLexer extends Lexer;
options
{
	k = 2;
}

tokens
{
	TOP;
	NODEID;
	MAPEXPRHALF;
	MAPOPTIONS;
	DEFINE;
	GROUP;
	NODEFEEDBACK;
	FLOATLIT;
	CONSTRUCTOR;
	SEQUENCE="sequence";
	MAPPINGS="mappings";
	PRIORITY="priority";
	FOCUSEDONLY="focusedonly";
	NOQIV="noqiv";
	DEFINES="defines";
	INCLUDE="include";
	OPTIONS="options";
	FEEDBACK="feedback";
	HINT="Hint";
	JIT="JIT";
	ANSWER="answer";
	VARIABLES="variables";
	TUTORNAME="tutorname";
	SWITCH="switch";
	CONCAT="concat";
	THEN="then";
	UNTIL="until";
	AND="and";
	OR="or";
	ONCOMPLETE="OnComplete";
}

// ID component
ID: ('A'..'Z'|'a'..'z'|'_'|'-')('A'..'Z'|'a'..'z'|'0'..'9'|'_'|'-')*;
LBRACE: '{';
RBRACE: '}';
LPAREN: '(';
RPAREN: ')';
LBRACK: '[';
RBRACK: ']';
PLUS: '+';
SEMI: ';';
DOT: '.';
STAR: '*';
LTHAN: '<';
GTHAN: '>';
COLON: ':';
COMMA: ',';
MAPTO: "=>";
EQ: '=';
DBLEQ: "==";
DBLAND: "&&";
DBLOR: "||";
NOTEQ: "!=";
LEQ: "<=";
GEQ: ">=";

// standard string literal
STRINGLIT: '"' (~('"' | '\r' | '\n' | '\\') | "\\\"" | "\\\\")* '"' { String t = getText(); setText(t.substring(1, t.length() - 1)); };
// standard int
INT: ('0'..'9')+;
// whitespace, ignored
WS: (' ' | '\r' {newline();} | '\n' {newline();} | "\r\n" {newline();} |'\t') {$setType(Token.SKIP);};
// pound-comment, ignored
COMMENT1: "#" (~('\n' | '\r'))* {$setType(Token.SKIP);};
// C-comment, ignored
COMMENT2: "/*" (
      options {
        generateAmbigWarnings=false;
      }
      :  { LA(2)!='/' }? '*'
      | '\r' '\n' {newline();}
      | '\r' {newline();}
      | '\n' {newline();}
      | ~('*'|'\n'|'\r')
    )*
    "*/"
    {$setType(Token.SKIP);}
;

/** parses the AST from EMScriptParser into our internal Java objects */
class EMScriptTreeParser extends TreeParser;
{
	Tree MTree;
	Tutor MTutor;
	RuleOptions Options;   // this gets set to the default if no options specified, or set accordingly if there are options
	                // it's then used by the subsequent mapping. (see 'mapoptions', 'renameapp', etc.)
	List<LeafNode> CurGroup = null; // nodes mentioned inside brackets in a sequence are added to a group.
}

/** parses the AST from EMScriptParser into our internal Java objects. As is standard
 * for ANTLR tree parsers, the structure of this parser pretty closely mimics that of the
 * text parser. */
createMappingAndTutor returns [Object[] t = null] { MTree = new Tree(); MTutor = new Tutor(); }: #(TOP (seq | map | opt | feedback)*) { MTree.finishConstruction(); t = new Object[2]; t[0] = MTree; t[1] = MTutor; };
opt: #(OPTIONS (option)*);
option: TUTORNAME tutorname:STRINGLIT { /*MTree.setTutorName(tutorname.getText());*/ };
seq: #(SEQUENCE { CurGroup = null; } expr);
expr returns [MappingNode n = null] {MappingNode l = null, r = null;}:
	#(AND l = expr r = expr)
{
	n = MTree.createAndNode(l, r);
}
	| #(OR l = expr r = expr)
{
	n = MTree.createOrNode(l, r);
}
	| #(THEN l = expr r = expr)
{
	n = MTree.createThenNode(l, r);
}
	| #(UNTIL l = expr r = expr)
{
	n = MTree.createUntilNode(l, r);
}
	| node:NODEID
{
	LeafNode ln = MTree.createLeafNode(node.getText(), node.getLine(), node.getColumn());
	n = ln;
	if (CurGroup != null)
		CurGroup.add(ln);
}
	| #(GROUP { CurGroup = new ArrayList<LeafNode>(); } n = expr { MTree.groups.add(CurGroup); CurGroup = null; })
;
map: #(MAPPINGS (mapto)*);
mapto:
	(#(MAPTO (MAPOPTIONS)? #(MAPEXPRHALF NODEID))) => #(MAPTO mapoptions renameapp) |
	(#(MAPTO (MAPOPTIONS)? #(MAPEXPRHALF SWITCH))) => #(MAPTO mapoptions switcht) |
	#(MAPTO mapoptions concat);
renameapp:
	#(MAPEXPRHALF app:NODEID) #(MAPEXPRHALF tut:NODEID)
	{ MTree.getOrCreateMapping(tut.getText()).putRenameAction(Options, app.getText(), tut.getText()); };
switcht:
	#(MAPEXPRHALF s:SWITCH) #(MAPEXPRHALF tut:NODEID)
	{ switchint(s, tut.getText()); };
switchint [String tut] { SwitchAction ga = null; }:
	#(SWITCH { ga = MTree.getOrCreateMapping(tut).putSwitchAction(Options, tut); }
	(app:NODEID val:literal { ga.putValuePair(Options, app.getText(), val.getText()); } )+);
concat:
	#(MAPEXPRHALF c:CONCAT) #(MAPEXPRHALF tut:NODEID)
	{ concatint(c, tut.getText()); };
concatint [String tut] { ConcatAction ca = null; }:
	#(CONCAT { ca = MTree.getOrCreateMapping(tut).putConcatAction(Options, tut); }
	(app:NODEID { ca.putAppNode(Options, app.getText()); } )+);
// This is a little weird. I need to set the options back to defaults whether or not there is a
// mapoptions for this mapping rule. So this parsing rule runs no matter what, and therefore has the ?, rather than
// applications of the parsing rule having ?'s.
mapoptions: { Options = new RuleOptions(); } (#(MAPOPTIONS (mapoption)*))?;
mapoption: #(PRIORITY pr:INT) { Options.priority = Integer.parseInt(pr.getText()); }
	| FOCUSEDONLY { Options.focusedOnly = true; }
	| NOQIV { Options.noQIV = true; };
literal: STRINGLIT | INT | FLOATLIT;

feedback: #(FEEDBACK (feedbackstmt)*);
feedbackstmt: (variabledecls | nodefeedback);
nodefeedback {ExprNode c = null;}: #(NODEFEEDBACK n:NODEID
	(
		#(HINT h:STRINGLIT)
		{
			MTutor.getOrCreateGoalNode(n.getText()).addHint(h.getText());
		}
		| #(ONCOMPLETE o:STRINGLIT)
		{
		  MTutor.getOrCreateGoalNode(n.getText()).addOnComplete(o.getText());
		}
		| (#(JIT STRINGLIT)) => #(JIT j2:STRINGLIT)
		{
			MTutor.getOrCreateGoalNode(n.getText()).addJIT(null, j2.getText());
		}
		| #(JIT c=valexpr j:STRINGLIT)
		{
			MTutor.getOrCreateGoalNode(n.getText()).addJIT(c, j.getText());
		}
		| #(a:ANSWER .)
		{
			MTutor.getOrCreateGoalNode(n.getText()).setProperty(a.getText(), a);
		}
		| #(f:ID g:literal)
		{
			MTutor.getOrCreateGoalNode(n.getText()).setProperty(f.getText(), g.getText());
		}
	)*);
valexpr returns [ExprNode e = null] { ExprNode l = null, r = null; }:
	#(DBLAND l = valexpr r = valexpr)
	{
		e = new BoolOpNode(BoolOpNode.OpType.AndOp, l, r);
	}
	| #(DBLOR l = valexpr r = valexpr)
	{
		e = new BoolOpNode(BoolOpNode.OpType.OrOp, l, r);
	}
	| #(LTHAN l = valexpr r = valexpr)
	{
		e = new CondNode(Op.LThanOp, l, r);
	}
	| #(GTHAN l = valexpr r = valexpr)
	{
		e = new CondNode(Op.GThanOp, l, r);
	}
	| #(LEQ l = valexpr r = valexpr)
	{
		e = new CondNode(Op.LEqOp, l, r);
	}
	| #(GEQ l = valexpr r = valexpr)
	{
		e = new CondNode(Op.GEqOp, l, r);
	}
	| #(DBLEQ l = valexpr r = valexpr)
	{
		e = new CondNode(Op.EqOp, l, r);
	}
	| #(NOTEQ l = valexpr r = valexpr)
	{
		e = new CondNode(Op.NotEqOp, l, r);
	}
	| s:STRINGLIT
	{
		e = new StringLitNode(s.getText());
	}
	| i:INT
	{
		e = new IntLitNode(i.getText());
	}
	| f:FLOATLIT
	{
		e = new FloatLitNode(f.getText());
	}
	| a:ANSWER
	{
		e = new VarNode(a.getText());
	}
	| v:ID
	{
		e = new VarNode(v.getText());
	}
	| cn:CONSTRUCTOR
	{
		e = new ConstructorNode(cn);
	}
	;
variabledecls: #(VARIABLES variabledecl);
variabledecl: #(k:ID v:literal) { MTutor.addConstant(k.getText(), v.getText()); };
