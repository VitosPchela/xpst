// $ANTLR : "emscript.g" -> "EMScriptParser.java"$

package org.clearsighted.tutorbase.emscript;

import java.util.HashMap;
import java.util.List;
import java.util.ArrayList;
import org.clearsighted.tutorbase.emscript.mappingtree.*;
import org.clearsighted.tutorbase.emscript.exprtree.*;
import org.clearsighted.tutorengine.*;
import org.clearsighted.tutorengine.types.Operations.Op;

import antlr.TokenBuffer;
import antlr.TokenStreamException;
import antlr.TokenStreamIOException;
import antlr.ANTLRException;
import antlr.LLkParser;
import antlr.Token;
import antlr.TokenStream;
import antlr.RecognitionException;
import antlr.NoViableAltException;
import antlr.MismatchedTokenException;
import antlr.SemanticException;
import antlr.ParserSharedInputState;
import antlr.collections.impl.BitSet;
import antlr.collections.AST;
import java.util.Hashtable;
import antlr.ASTFactory;
import antlr.ASTPair;
import antlr.collections.impl.ASTArray;

public class EMScriptParser extends antlr.LLkParser       implements EMScriptParserTokenTypes
 {

	HashMap<String, AST> Defines = new HashMap<String, AST>();
	String TutorName = null;

protected EMScriptParser(TokenBuffer tokenBuf, int k) {
  super(tokenBuf,k);
  tokenNames = _tokenNames;
  buildTokenTypeASTClassMap();
  astFactory = new ASTFactory(getTokenTypeToASTClassMap());
}

public EMScriptParser(TokenBuffer tokenBuf) {
  this(tokenBuf,2);
}

protected EMScriptParser(TokenStream lexer, int k) {
  super(lexer,k);
  tokenNames = _tokenNames;
  buildTokenTypeASTClassMap();
  astFactory = new ASTFactory(getTokenTypeToASTClassMap());
}

public EMScriptParser(TokenStream lexer) {
  this(lexer,2);
}

public EMScriptParser(ParserSharedInputState state) {
  super(state,2);
  tokenNames = _tokenNames;
  buildTokenTypeASTClassMap();
  astFactory = new ASTFactory(getTokenTypeToASTClassMap());
}

	public final void top() throws RecognitionException, TokenStreamException {
		
		returnAST = null;
		ASTPair currentAST = new ASTPair();
		AST top_AST = null;
		
		{
		_loop3:
		do {
			switch ( LA(1)) {
			case DEFINES:
			{
				defines();
				astFactory.addASTChild(currentAST, returnAST);
				break;
			}
			case SEQUENCE:
			{
				sequence();
				astFactory.addASTChild(currentAST, returnAST);
				break;
			}
			case MAPPINGS:
			{
				mappings();
				astFactory.addASTChild(currentAST, returnAST);
				break;
			}
			case INCLUDE:
			{
				include();
				astFactory.addASTChild(currentAST, returnAST);
				break;
			}
			case OPTIONS:
			{
				option();
				astFactory.addASTChild(currentAST, returnAST);
				break;
			}
			case FEEDBACK:
			{
				feedback();
				astFactory.addASTChild(currentAST, returnAST);
				break;
			}
			default:
			{
				break _loop3;
			}
			}
		} while (true);
		}
		match(Token.EOF_TYPE);
		top_AST = (AST)currentAST.root;
		top_AST = (AST)astFactory.make( (new ASTArray(2)).add(astFactory.create(TOP,"top")).add(top_AST));
		currentAST.root = top_AST;
		currentAST.child = top_AST!=null &&top_AST.getFirstChild()!=null ?
			top_AST.getFirstChild() : top_AST;
		currentAST.advanceChildToEnd();
		top_AST = (AST)currentAST.root;
		returnAST = top_AST;
	}
	
	public final void defines() throws RecognitionException, TokenStreamException {
		
		returnAST = null;
		ASTPair currentAST = new ASTPair();
		AST defines_AST = null;
		
		AST tmp55_AST = null;
		tmp55_AST = astFactory.create(LT(1));
		match(DEFINES);
		match(LBRACE);
		{
		_loop6:
		do {
			if ((LA(1)==ID)) {
				defineexpr();
				match(SEMI);
			}
			else {
				break _loop6;
			}
			
		} while (true);
		}
		match(RBRACE);
		returnAST = defines_AST;
	}
	
	public final void sequence() throws RecognitionException, TokenStreamException {
		
		returnAST = null;
		ASTPair currentAST = new ASTPair();
		AST sequence_AST = null;
		
		AST tmp59_AST = null;
		tmp59_AST = astFactory.create(LT(1));
		astFactory.makeASTRoot(currentAST, tmp59_AST);
		match(SEQUENCE);
		match(LBRACE);
		seqexpr();
		astFactory.addASTChild(currentAST, returnAST);
		match(SEMI);
		match(RBRACE);
		sequence_AST = (AST)currentAST.root;
		returnAST = sequence_AST;
	}
	
	public final void mappings() throws RecognitionException, TokenStreamException {
		
		returnAST = null;
		ASTPair currentAST = new ASTPair();
		AST mappings_AST = null;
		
		AST tmp63_AST = null;
		tmp63_AST = astFactory.create(LT(1));
		astFactory.makeASTRoot(currentAST, tmp63_AST);
		match(MAPPINGS);
		match(LBRACE);
		{
		_loop32:
		do {
			if ((_tokenSet_0.member(LA(1)))) {
				mapexpr();
				astFactory.addASTChild(currentAST, returnAST);
			}
			else {
				break _loop32;
			}
			
		} while (true);
		}
		match(RBRACE);
		mappings_AST = (AST)currentAST.root;
		returnAST = mappings_AST;
	}
	
	public final void include() throws RecognitionException, TokenStreamException {
		
		returnAST = null;
		ASTPair currentAST = new ASTPair();
		AST include_AST = null;
		Token  fname = null;
		AST fname_AST = null;
		
		AST tmp66_AST = null;
		tmp66_AST = astFactory.create(LT(1));
		astFactory.addASTChild(currentAST, tmp66_AST);
		match(INCLUDE);
		fname = LT(1);
		fname_AST = astFactory.create(fname);
		astFactory.addASTChild(currentAST, fname_AST);
		match(STRINGLIT);
		match(SEMI);
		include_AST = (AST)currentAST.root;
		
			try
			{
				EMScriptParser esp = EMScript.parse(getFilename(), fname.getText());
				include_AST = esp.getAST().getFirstChild();
				Defines.putAll(esp.Defines);
			}
			catch (Exception e)
			{
				e.printStackTrace();
			}
		
		currentAST.root = include_AST;
		currentAST.child = include_AST!=null &&include_AST.getFirstChild()!=null ?
			include_AST.getFirstChild() : include_AST;
		currentAST.advanceChildToEnd();
		include_AST = (AST)currentAST.root;
		returnAST = include_AST;
	}
	
	public final void option() throws RecognitionException, TokenStreamException {
		
		returnAST = null;
		ASTPair currentAST = new ASTPair();
		AST option_AST = null;
		
		AST tmp68_AST = null;
		tmp68_AST = astFactory.create(LT(1));
		astFactory.makeASTRoot(currentAST, tmp68_AST);
		match(OPTIONS);
		match(LBRACE);
		{
		_loop11:
		do {
			if ((LA(1)==TUTORNAME)) {
				optionexpr();
				astFactory.addASTChild(currentAST, returnAST);
				match(SEMI);
			}
			else {
				break _loop11;
			}
			
		} while (true);
		}
		match(RBRACE);
		option_AST = (AST)currentAST.root;
		returnAST = option_AST;
	}
	
	public final void feedback() throws RecognitionException, TokenStreamException {
		
		returnAST = null;
		ASTPair currentAST = new ASTPair();
		AST feedback_AST = null;
		
		AST tmp72_AST = null;
		tmp72_AST = astFactory.create(LT(1));
		astFactory.makeASTRoot(currentAST, tmp72_AST);
		match(FEEDBACK);
		match(LBRACE);
		feedbackstmt();
		astFactory.addASTChild(currentAST, returnAST);
		match(RBRACE);
		feedback_AST = (AST)currentAST.root;
		returnAST = feedback_AST;
	}
	
	public final void defineexpr() throws RecognitionException, TokenStreamException {
		
		returnAST = null;
		ASTPair currentAST = new ASTPair();
		AST defineexpr_AST = null;
		Token  id = null;
		AST id_AST = null;
		AST sw_AST = null;
		
		id = LT(1);
		id_AST = astFactory.create(id);
		match(ID);
		match(EQ);
		switcht();
		sw_AST = (AST)returnAST;
		
			Defines.put(id.getText(), sw_AST);
		
		returnAST = defineexpr_AST;
	}
	
	public final void switcht() throws RecognitionException, TokenStreamException {
		
		returnAST = null;
		ASTPair currentAST = new ASTPair();
		AST switcht_AST = null;
		
		AST tmp76_AST = null;
		tmp76_AST = astFactory.create(LT(1));
		astFactory.makeASTRoot(currentAST, tmp76_AST);
		match(SWITCH);
		match(LBRACE);
		switchexpr();
		astFactory.addASTChild(currentAST, returnAST);
		{
		_loop38:
		do {
			if ((LA(1)==COMMA) && (LA(2)==ID||LA(2)==PLUS)) {
				match(COMMA);
				switchexpr();
				astFactory.addASTChild(currentAST, returnAST);
			}
			else {
				break _loop38;
			}
			
		} while (true);
		}
		{
		switch ( LA(1)) {
		case COMMA:
		{
			match(COMMA);
			break;
		}
		case RBRACE:
		{
			break;
		}
		default:
		{
			throw new NoViableAltException(LT(1), getFilename());
		}
		}
		}
		match(RBRACE);
		switcht_AST = (AST)currentAST.root;
		returnAST = switcht_AST;
	}
	
	public final void optionexpr() throws RecognitionException, TokenStreamException {
		
		returnAST = null;
		ASTPair currentAST = new ASTPair();
		AST optionexpr_AST = null;
		
		AST tmp81_AST = null;
		tmp81_AST = astFactory.create(LT(1));
		astFactory.addASTChild(currentAST, tmp81_AST);
		match(TUTORNAME);
		match(EQ);
		stringlit();
		astFactory.addASTChild(currentAST, returnAST);
		optionexpr_AST = (AST)currentAST.root;
		returnAST = optionexpr_AST;
	}
	
	public final void stringlit() throws RecognitionException, TokenStreamException {
		
		returnAST = null;
		ASTPair currentAST = new ASTPair();
		AST stringlit_AST = null;
		Token  s = null;
		AST s_AST = null;
		
		s = LT(1);
		s_AST = astFactory.create(s);
		astFactory.addASTChild(currentAST, s_AST);
		match(STRINGLIT);
		stringlit_AST = (AST)currentAST.root;
		
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
			stringlit_AST = (AST)astFactory.make( (new ASTArray(2)).add(astFactory.create(STRINGLIT,news)).add(stringlit_AST));
		
		currentAST.root = stringlit_AST;
		currentAST.child = stringlit_AST!=null &&stringlit_AST.getFirstChild()!=null ?
			stringlit_AST.getFirstChild() : stringlit_AST;
		currentAST.advanceChildToEnd();
		stringlit_AST = (AST)currentAST.root;
		returnAST = stringlit_AST;
	}
	
	public final void seqexpr() throws RecognitionException, TokenStreamException {
		
		returnAST = null;
		ASTPair currentAST = new ASTPair();
		AST seqexpr_AST = null;
		
		minseqexpr();
		astFactory.addASTChild(currentAST, returnAST);
		{
		_loop17:
		do {
			if ((LA(1)==THEN||LA(1)==UNTIL)) {
				{
				switch ( LA(1)) {
				case THEN:
				{
					AST tmp83_AST = null;
					tmp83_AST = astFactory.create(LT(1));
					astFactory.makeASTRoot(currentAST, tmp83_AST);
					match(THEN);
					break;
				}
				case UNTIL:
				{
					AST tmp84_AST = null;
					tmp84_AST = astFactory.create(LT(1));
					astFactory.makeASTRoot(currentAST, tmp84_AST);
					match(UNTIL);
					break;
				}
				default:
				{
					throw new NoViableAltException(LT(1), getFilename());
				}
				}
				}
				minseqexpr();
				astFactory.addASTChild(currentAST, returnAST);
			}
			else {
				break _loop17;
			}
			
		} while (true);
		}
		seqexpr_AST = (AST)currentAST.root;
		returnAST = seqexpr_AST;
	}
	
	public final void minseqexpr() throws RecognitionException, TokenStreamException {
		
		returnAST = null;
		ASTPair currentAST = new ASTPair();
		AST minseqexpr_AST = null;
		
		{
		switch ( LA(1)) {
		case ID:
		case LPAREN:
		{
			atom();
			astFactory.addASTChild(currentAST, returnAST);
			break;
		}
		case LBRACK:
		{
			groupexpr();
			astFactory.addASTChild(currentAST, returnAST);
			break;
		}
		default:
		{
			throw new NoViableAltException(LT(1), getFilename());
		}
		}
		}
		{
		_loop23:
		do {
			if ((LA(1)==AND||LA(1)==OR)) {
				{
				switch ( LA(1)) {
				case AND:
				{
					AST tmp85_AST = null;
					tmp85_AST = astFactory.create(LT(1));
					astFactory.makeASTRoot(currentAST, tmp85_AST);
					match(AND);
					break;
				}
				case OR:
				{
					AST tmp86_AST = null;
					tmp86_AST = astFactory.create(LT(1));
					astFactory.makeASTRoot(currentAST, tmp86_AST);
					match(OR);
					break;
				}
				default:
				{
					throw new NoViableAltException(LT(1), getFilename());
				}
				}
				}
				{
				switch ( LA(1)) {
				case ID:
				case LPAREN:
				{
					atom();
					astFactory.addASTChild(currentAST, returnAST);
					break;
				}
				case LBRACK:
				{
					groupexpr();
					astFactory.addASTChild(currentAST, returnAST);
					break;
				}
				default:
				{
					throw new NoViableAltException(LT(1), getFilename());
				}
				}
				}
			}
			else {
				break _loop23;
			}
			
		} while (true);
		}
		minseqexpr_AST = (AST)currentAST.root;
		returnAST = minseqexpr_AST;
	}
	
	public final void atom() throws RecognitionException, TokenStreamException {
		
		returnAST = null;
		ASTPair currentAST = new ASTPair();
		AST atom_AST = null;
		
		switch ( LA(1)) {
		case ID:
		{
			nodeid();
			astFactory.addASTChild(currentAST, returnAST);
			atom_AST = (AST)currentAST.root;
			break;
		}
		case LPAREN:
		{
			match(LPAREN);
			seqexpr();
			astFactory.addASTChild(currentAST, returnAST);
			match(RPAREN);
			atom_AST = (AST)currentAST.root;
			break;
		}
		default:
		{
			throw new NoViableAltException(LT(1), getFilename());
		}
		}
		returnAST = atom_AST;
	}
	
	public final void groupexpr() throws RecognitionException, TokenStreamException {
		
		returnAST = null;
		ASTPair currentAST = new ASTPair();
		AST groupexpr_AST = null;
		
		match(LBRACK);
		seqexpr();
		astFactory.addASTChild(currentAST, returnAST);
		match(RBRACK);
		groupexpr_AST = (AST)currentAST.root;
		groupexpr_AST = (AST)astFactory.make( (new ASTArray(2)).add(astFactory.create(GROUP,"GROUP")).add(groupexpr_AST));
		currentAST.root = groupexpr_AST;
		currentAST.child = groupexpr_AST!=null &&groupexpr_AST.getFirstChild()!=null ?
			groupexpr_AST.getFirstChild() : groupexpr_AST;
		currentAST.advanceChildToEnd();
		groupexpr_AST = (AST)currentAST.root;
		returnAST = groupexpr_AST;
	}
	
	public final void nodeid() throws RecognitionException, TokenStreamException {
		
		returnAST = null;
		ASTPair currentAST = new ASTPair();
		AST nodeid_AST = null;
		Token  i1 = null;
		AST i1_AST = null;
		Token  i2 = null;
		AST i2_AST = null;
		Token  i3 = null;
		AST i3_AST = null;
		String fullname = null;
		
		i1 = LT(1);
		i1_AST = astFactory.create(i1);
		astFactory.addASTChild(currentAST, i1_AST);
		match(ID);
		fullname = i1.getText();
		{
		_loop28:
		do {
			if ((LA(1)==DOT)) {
				AST tmp91_AST = null;
				tmp91_AST = astFactory.create(LT(1));
				astFactory.addASTChild(currentAST, tmp91_AST);
				match(DOT);
				i2 = LT(1);
				i2_AST = astFactory.create(i2);
				astFactory.addASTChild(currentAST, i2_AST);
				match(ID);
				fullname = fullname + "." + i2.getText();
			}
			else {
				break _loop28;
			}
			
		} while (true);
		}
		{
		switch ( LA(1)) {
		case COLON:
		{
			AST tmp92_AST = null;
			tmp92_AST = astFactory.create(LT(1));
			astFactory.addASTChild(currentAST, tmp92_AST);
			match(COLON);
			i3 = LT(1);
			i3_AST = astFactory.create(i3);
			astFactory.addASTChild(currentAST, i3_AST);
			match(ID);
			fullname = fullname + ":" + i3.getText();
			break;
		}
		case LBRACE:
		case SEMI:
		case RBRACE:
		case EQ:
		case THEN:
		case UNTIL:
		case AND:
		case OR:
		case RBRACK:
		case RPAREN:
		case MAPTO:
		case COMMA:
		{
			break;
		}
		default:
		{
			throw new NoViableAltException(LT(1), getFilename());
		}
		}
		}
		nodeid_AST = (AST)currentAST.root;
		
			nodeid_AST = (AST)astFactory.make( (new ASTArray(1)).add(astFactory.create(NODEID,fullname)));
			((EMAST)nodeid_AST).line = i1.getLine();
			((EMAST)nodeid_AST).column = i1.getColumn();
		
		currentAST.root = nodeid_AST;
		currentAST.child = nodeid_AST!=null &&nodeid_AST.getFirstChild()!=null ?
			nodeid_AST.getFirstChild() : nodeid_AST;
		currentAST.advanceChildToEnd();
		nodeid_AST = (AST)currentAST.root;
		returnAST = nodeid_AST;
	}
	
	public final void mapexpr() throws RecognitionException, TokenStreamException {
		
		returnAST = null;
		ASTPair currentAST = new ASTPair();
		AST mapexpr_AST = null;
		
		{
		switch ( LA(1)) {
		case LBRACK:
		{
			mapoptions();
			astFactory.addASTChild(currentAST, returnAST);
			break;
		}
		case ID:
		case SWITCH:
		case CONCAT:
		{
			break;
		}
		default:
		{
			throw new NoViableAltException(LT(1), getFilename());
		}
		}
		}
		mapexprhalf();
		astFactory.addASTChild(currentAST, returnAST);
		AST tmp93_AST = null;
		tmp93_AST = astFactory.create(LT(1));
		astFactory.makeASTRoot(currentAST, tmp93_AST);
		match(MAPTO);
		mapexprhalf();
		astFactory.addASTChild(currentAST, returnAST);
		match(SEMI);
		mapexpr_AST = (AST)currentAST.root;
		returnAST = mapexpr_AST;
	}
	
	public final void mapoptions() throws RecognitionException, TokenStreamException {
		
		returnAST = null;
		ASTPair currentAST = new ASTPair();
		AST mapoptions_AST = null;
		
		match(LBRACK);
		mapoption();
		astFactory.addASTChild(currentAST, returnAST);
		{
		_loop49:
		do {
			if ((LA(1)==COMMA)) {
				match(COMMA);
				mapoption();
				astFactory.addASTChild(currentAST, returnAST);
			}
			else {
				break _loop49;
			}
			
		} while (true);
		}
		match(RBRACK);
		mapoptions_AST = (AST)currentAST.root;
		
			mapoptions_AST = (AST)astFactory.make( (new ASTArray(2)).add(astFactory.create(MAPOPTIONS,"MAPOPTIONS")).add(mapoptions_AST));
		
		currentAST.root = mapoptions_AST;
		currentAST.child = mapoptions_AST!=null &&mapoptions_AST.getFirstChild()!=null ?
			mapoptions_AST.getFirstChild() : mapoptions_AST;
		currentAST.advanceChildToEnd();
		mapoptions_AST = (AST)currentAST.root;
		returnAST = mapoptions_AST;
	}
	
	public final void mapexprhalf() throws RecognitionException, TokenStreamException {
		
		returnAST = null;
		ASTPair currentAST = new ASTPair();
		AST mapexprhalf_AST = null;
		AST fn_AST = null;
		
		switch ( LA(1)) {
		case ID:
		{
			nodeid();
			fn_AST = (AST)returnAST;
			astFactory.addASTChild(currentAST, returnAST);
			mapexprhalf_AST = (AST)currentAST.root;
			
				mapexprhalf_AST = (AST)astFactory.make( (new ASTArray(2)).add(astFactory.create(MAPEXPRHALF,"MAPEXPRHALF")).add(mapexprhalf_AST));
			
			currentAST.root = mapexprhalf_AST;
			currentAST.child = mapexprhalf_AST!=null &&mapexprhalf_AST.getFirstChild()!=null ?
				mapexprhalf_AST.getFirstChild() : mapexprhalf_AST;
			currentAST.advanceChildToEnd();
			mapexprhalf_AST = (AST)currentAST.root;
			break;
		}
		case SWITCH:
		{
			switcht();
			astFactory.addASTChild(currentAST, returnAST);
			mapexprhalf_AST = (AST)currentAST.root;
			mapexprhalf_AST = (AST)astFactory.make( (new ASTArray(2)).add(astFactory.create(MAPEXPRHALF,"MAPEXPRHALF")).add(mapexprhalf_AST));
			currentAST.root = mapexprhalf_AST;
			currentAST.child = mapexprhalf_AST!=null &&mapexprhalf_AST.getFirstChild()!=null ?
				mapexprhalf_AST.getFirstChild() : mapexprhalf_AST;
			currentAST.advanceChildToEnd();
			mapexprhalf_AST = (AST)currentAST.root;
			break;
		}
		case CONCAT:
		{
			concat();
			astFactory.addASTChild(currentAST, returnAST);
			mapexprhalf_AST = (AST)currentAST.root;
			mapexprhalf_AST = (AST)astFactory.make( (new ASTArray(2)).add(astFactory.create(MAPEXPRHALF,"MAPEXPRHALF")).add(mapexprhalf_AST));
			currentAST.root = mapexprhalf_AST;
			currentAST.child = mapexprhalf_AST!=null &&mapexprhalf_AST.getFirstChild()!=null ?
				mapexprhalf_AST.getFirstChild() : mapexprhalf_AST;
			currentAST.advanceChildToEnd();
			mapexprhalf_AST = (AST)currentAST.root;
			break;
		}
		default:
		{
			throw new NoViableAltException(LT(1), getFilename());
		}
		}
		returnAST = mapexprhalf_AST;
	}
	
	public final void concat() throws RecognitionException, TokenStreamException {
		
		returnAST = null;
		ASTPair currentAST = new ASTPair();
		AST concat_AST = null;
		
		AST tmp98_AST = null;
		tmp98_AST = astFactory.create(LT(1));
		astFactory.makeASTRoot(currentAST, tmp98_AST);
		match(CONCAT);
		match(LBRACE);
		concatexpr();
		astFactory.addASTChild(currentAST, returnAST);
		{
		_loop44:
		do {
			if ((LA(1)==COMMA) && (LA(2)==ID)) {
				match(COMMA);
				concatexpr();
				astFactory.addASTChild(currentAST, returnAST);
			}
			else {
				break _loop44;
			}
			
		} while (true);
		}
		{
		switch ( LA(1)) {
		case COMMA:
		{
			match(COMMA);
			break;
		}
		case RBRACE:
		{
			break;
		}
		default:
		{
			throw new NoViableAltException(LT(1), getFilename());
		}
		}
		}
		match(RBRACE);
		concat_AST = (AST)currentAST.root;
		returnAST = concat_AST;
	}
	
	public final void switchexpr() throws RecognitionException, TokenStreamException {
		
		returnAST = null;
		ASTPair currentAST = new ASTPair();
		AST switchexpr_AST = null;
		Token  id = null;
		AST id_AST = null;
		
		switch ( LA(1)) {
		case ID:
		{
			nodeid();
			astFactory.addASTChild(currentAST, returnAST);
			match(EQ);
			literal();
			astFactory.addASTChild(currentAST, returnAST);
			switchexpr_AST = (AST)currentAST.root;
			break;
		}
		case PLUS:
		{
			{
			match(PLUS);
			id = LT(1);
			id_AST = astFactory.create(id);
			match(ID);
			}
			switchexpr_AST = (AST)currentAST.root;
			
				AST r = Defines.get(id.getText());
				// TODO: isn't there an easier way to clone some children into another tree?
				ASTFactory fact = new ASTFactory();
				if (r.getType() == SWITCH)
				{
					r = r.getFirstChild();
					switchexpr_AST = fact.dupTree(r);
					AST n = switchexpr_AST;
					r = r.getNextSibling();
					while (r != null)
					{
						AST rc = fact.dupTree(r);
						n.setNextSibling(rc);
						n = rc;
						r = r.getNextSibling();
					}
				}
			
			currentAST.root = switchexpr_AST;
			currentAST.child = switchexpr_AST!=null &&switchexpr_AST.getFirstChild()!=null ?
				switchexpr_AST.getFirstChild() : switchexpr_AST;
			currentAST.advanceChildToEnd();
			switchexpr_AST = (AST)currentAST.root;
			break;
		}
		default:
		{
			throw new NoViableAltException(LT(1), getFilename());
		}
		}
		returnAST = switchexpr_AST;
	}
	
	public final void literal() throws RecognitionException, TokenStreamException {
		
		returnAST = null;
		ASTPair currentAST = new ASTPair();
		AST literal_AST = null;
		
		if ((LA(1)==STRINGLIT)) {
			stringlit();
			astFactory.addASTChild(currentAST, returnAST);
			literal_AST = (AST)currentAST.root;
		}
		else if ((LA(1)==INT) && (_tokenSet_1.member(LA(2)))) {
			AST tmp105_AST = null;
			tmp105_AST = astFactory.create(LT(1));
			astFactory.addASTChild(currentAST, tmp105_AST);
			match(INT);
			literal_AST = (AST)currentAST.root;
		}
		else if ((LA(1)==INT) && (LA(2)==DOT)) {
			floatlit();
			astFactory.addASTChild(currentAST, returnAST);
			literal_AST = (AST)currentAST.root;
		}
		else {
			throw new NoViableAltException(LT(1), getFilename());
		}
		
		returnAST = literal_AST;
	}
	
	public final void concatexpr() throws RecognitionException, TokenStreamException {
		
		returnAST = null;
		ASTPair currentAST = new ASTPair();
		AST concatexpr_AST = null;
		
		nodeid();
		astFactory.addASTChild(currentAST, returnAST);
		concatexpr_AST = (AST)currentAST.root;
		returnAST = concatexpr_AST;
	}
	
	public final void mapoption() throws RecognitionException, TokenStreamException {
		
		returnAST = null;
		ASTPair currentAST = new ASTPair();
		AST mapoption_AST = null;
		
		switch ( LA(1)) {
		case PRIORITY:
		{
			AST tmp106_AST = null;
			tmp106_AST = astFactory.create(LT(1));
			astFactory.makeASTRoot(currentAST, tmp106_AST);
			match(PRIORITY);
			match(EQ);
			AST tmp108_AST = null;
			tmp108_AST = astFactory.create(LT(1));
			astFactory.addASTChild(currentAST, tmp108_AST);
			match(INT);
			mapoption_AST = (AST)currentAST.root;
			break;
		}
		case FOCUSEDONLY:
		{
			AST tmp109_AST = null;
			tmp109_AST = astFactory.create(LT(1));
			astFactory.addASTChild(currentAST, tmp109_AST);
			match(FOCUSEDONLY);
			mapoption_AST = (AST)currentAST.root;
			break;
		}
		case NOQIV:
		{
			AST tmp110_AST = null;
			tmp110_AST = astFactory.create(LT(1));
			astFactory.addASTChild(currentAST, tmp110_AST);
			match(NOQIV);
			mapoption_AST = (AST)currentAST.root;
			break;
		}
		default:
		{
			throw new NoViableAltException(LT(1), getFilename());
		}
		}
		returnAST = mapoption_AST;
	}
	
	public final void floatlit() throws RecognitionException, TokenStreamException {
		
		returnAST = null;
		ASTPair currentAST = new ASTPair();
		AST floatlit_AST = null;
		Token  i = null;
		AST i_AST = null;
		Token  m = null;
		AST m_AST = null;
		
		i = LT(1);
		i_AST = astFactory.create(i);
		astFactory.addASTChild(currentAST, i_AST);
		match(INT);
		AST tmp111_AST = null;
		tmp111_AST = astFactory.create(LT(1));
		astFactory.addASTChild(currentAST, tmp111_AST);
		match(DOT);
		m = LT(1);
		m_AST = astFactory.create(m);
		astFactory.addASTChild(currentAST, m_AST);
		match(INT);
		floatlit_AST = (AST)currentAST.root;
		
			String str = i.getText() + "." + m.getText();
			floatlit_AST = (AST)astFactory.make( (new ASTArray(1)).add(astFactory.create(FLOATLIT,str)));
		
		currentAST.root = floatlit_AST;
		currentAST.child = floatlit_AST!=null &&floatlit_AST.getFirstChild()!=null ?
			floatlit_AST.getFirstChild() : floatlit_AST;
		currentAST.advanceChildToEnd();
		floatlit_AST = (AST)currentAST.root;
		returnAST = floatlit_AST;
	}
	
	public final void feedbackstmt() throws RecognitionException, TokenStreamException {
		
		returnAST = null;
		ASTPair currentAST = new ASTPair();
		AST feedbackstmt_AST = null;
		
		{
		_loop57:
		do {
			switch ( LA(1)) {
			case VARIABLES:
			{
				variabledecls();
				astFactory.addASTChild(currentAST, returnAST);
				break;
			}
			case ID:
			{
				nodefeedback();
				astFactory.addASTChild(currentAST, returnAST);
				break;
			}
			default:
			{
				break _loop57;
			}
			}
		} while (true);
		}
		feedbackstmt_AST = (AST)currentAST.root;
		returnAST = feedbackstmt_AST;
	}
	
	public final void variabledecls() throws RecognitionException, TokenStreamException {
		
		returnAST = null;
		ASTPair currentAST = new ASTPair();
		AST variabledecls_AST = null;
		
		AST tmp112_AST = null;
		tmp112_AST = astFactory.create(LT(1));
		astFactory.makeASTRoot(currentAST, tmp112_AST);
		match(VARIABLES);
		match(LBRACE);
		{
		_loop60:
		do {
			if ((LA(1)==ID)) {
				variabledecl();
				astFactory.addASTChild(currentAST, returnAST);
				match(SEMI);
			}
			else {
				break _loop60;
			}
			
		} while (true);
		}
		match(RBRACE);
		variabledecls_AST = (AST)currentAST.root;
		returnAST = variabledecls_AST;
	}
	
	public final void nodefeedback() throws RecognitionException, TokenStreamException {
		
		returnAST = null;
		ASTPair currentAST = new ASTPair();
		AST nodefeedback_AST = null;
		
		nodeid();
		astFactory.addASTChild(currentAST, returnAST);
		match(LBRACE);
		{
		_loop64:
		do {
			if ((_tokenSet_2.member(LA(1)))) {
				{
				switch ( LA(1)) {
				case HINT:
				{
					hintdecl();
					astFactory.addASTChild(currentAST, returnAST);
					break;
				}
				case JIT:
				{
					jitdecl();
					astFactory.addASTChild(currentAST, returnAST);
					break;
				}
				case ANSWER:
				{
					answerdecl();
					astFactory.addASTChild(currentAST, returnAST);
					break;
				}
				case ID:
				{
					propertydecl();
					astFactory.addASTChild(currentAST, returnAST);
					break;
				}
				default:
				{
					throw new NoViableAltException(LT(1), getFilename());
				}
				}
				}
				match(SEMI);
			}
			else {
				break _loop64;
			}
			
		} while (true);
		}
		match(RBRACE);
		nodefeedback_AST = (AST)currentAST.root;
		
			nodefeedback_AST = (AST)astFactory.make( (new ASTArray(2)).add(astFactory.create(NODEFEEDBACK,"NODEFEEDBACK")).add(nodefeedback_AST));
		
		currentAST.root = nodefeedback_AST;
		currentAST.child = nodefeedback_AST!=null &&nodefeedback_AST.getFirstChild()!=null ?
			nodefeedback_AST.getFirstChild() : nodefeedback_AST;
		currentAST.advanceChildToEnd();
		nodefeedback_AST = (AST)currentAST.root;
		returnAST = nodefeedback_AST;
	}
	
	public final void variabledecl() throws RecognitionException, TokenStreamException {
		
		returnAST = null;
		ASTPair currentAST = new ASTPair();
		AST variabledecl_AST = null;
		
		AST tmp119_AST = null;
		tmp119_AST = astFactory.create(LT(1));
		astFactory.makeASTRoot(currentAST, tmp119_AST);
		match(ID);
		match(COLON);
		literal();
		astFactory.addASTChild(currentAST, returnAST);
		variabledecl_AST = (AST)currentAST.root;
		returnAST = variabledecl_AST;
	}
	
	public final void hintdecl() throws RecognitionException, TokenStreamException {
		
		returnAST = null;
		ASTPair currentAST = new ASTPair();
		AST hintdecl_AST = null;
		
		AST tmp121_AST = null;
		tmp121_AST = astFactory.create(LT(1));
		astFactory.makeASTRoot(currentAST, tmp121_AST);
		match(HINT);
		match(COLON);
		stringlit();
		astFactory.addASTChild(currentAST, returnAST);
		hintdecl_AST = (AST)currentAST.root;
		returnAST = hintdecl_AST;
	}
	
	public final void jitdecl() throws RecognitionException, TokenStreamException {
		
		returnAST = null;
		ASTPair currentAST = new ASTPair();
		AST jitdecl_AST = null;
		AST s_AST = null;
		
		if ((LA(1)==JIT) && (LA(2)==LBRACE)) {
			AST tmp123_AST = null;
			tmp123_AST = astFactory.create(LT(1));
			astFactory.makeASTRoot(currentAST, tmp123_AST);
			match(JIT);
			{
			match(LBRACE);
			valexpr();
			astFactory.addASTChild(currentAST, returnAST);
			match(RBRACE);
			}
			match(COLON);
			stringlit();
			astFactory.addASTChild(currentAST, returnAST);
			jitdecl_AST = (AST)currentAST.root;
		}
		else if ((LA(1)==JIT) && (LA(2)==COLON)) {
			AST tmp127_AST = null;
			tmp127_AST = astFactory.create(LT(1));
			astFactory.makeASTRoot(currentAST, tmp127_AST);
			match(JIT);
			match(COLON);
			stringlit();
			s_AST = (AST)returnAST;
			astFactory.addASTChild(currentAST, returnAST);
			jitdecl_AST = (AST)currentAST.root;
			jitdecl_AST = (AST)astFactory.make( (new ASTArray(2)).add(tmp127_AST).add(s_AST));
			currentAST.root = jitdecl_AST;
			currentAST.child = jitdecl_AST!=null &&jitdecl_AST.getFirstChild()!=null ?
				jitdecl_AST.getFirstChild() : jitdecl_AST;
			currentAST.advanceChildToEnd();
			jitdecl_AST = (AST)currentAST.root;
		}
		else {
			throw new NoViableAltException(LT(1), getFilename());
		}
		
		returnAST = jitdecl_AST;
	}
	
	public final void answerdecl() throws RecognitionException, TokenStreamException {
		
		returnAST = null;
		ASTPair currentAST = new ASTPair();
		AST answerdecl_AST = null;
		
		AST tmp129_AST = null;
		tmp129_AST = astFactory.create(LT(1));
		astFactory.makeASTRoot(currentAST, tmp129_AST);
		match(ANSWER);
		match(COLON);
		constructor();
		astFactory.addASTChild(currentAST, returnAST);
		answerdecl_AST = (AST)currentAST.root;
		returnAST = answerdecl_AST;
	}
	
	public final void propertydecl() throws RecognitionException, TokenStreamException {
		
		returnAST = null;
		ASTPair currentAST = new ASTPair();
		AST propertydecl_AST = null;
		
		AST tmp131_AST = null;
		tmp131_AST = astFactory.create(LT(1));
		astFactory.makeASTRoot(currentAST, tmp131_AST);
		match(ID);
		match(COLON);
		literal();
		astFactory.addASTChild(currentAST, returnAST);
		propertydecl_AST = (AST)currentAST.root;
		returnAST = propertydecl_AST;
	}
	
	public final void valexpr() throws RecognitionException, TokenStreamException {
		
		returnAST = null;
		ASTPair currentAST = new ASTPair();
		AST valexpr_AST = null;
		
		valcmpop();
		astFactory.addASTChild(currentAST, returnAST);
		{
		_loop78:
		do {
			if ((LA(1)==DBLAND||LA(1)==DBLOR)) {
				{
				switch ( LA(1)) {
				case DBLAND:
				{
					AST tmp133_AST = null;
					tmp133_AST = astFactory.create(LT(1));
					astFactory.makeASTRoot(currentAST, tmp133_AST);
					match(DBLAND);
					break;
				}
				case DBLOR:
				{
					AST tmp134_AST = null;
					tmp134_AST = astFactory.create(LT(1));
					astFactory.makeASTRoot(currentAST, tmp134_AST);
					match(DBLOR);
					break;
				}
				default:
				{
					throw new NoViableAltException(LT(1), getFilename());
				}
				}
				}
				valcmpop();
				astFactory.addASTChild(currentAST, returnAST);
			}
			else {
				break _loop78;
			}
			
		} while (true);
		}
		valexpr_AST = (AST)currentAST.root;
		returnAST = valexpr_AST;
	}
	
	public final void constructor() throws RecognitionException, TokenStreamException {
		
		returnAST = null;
		ASTPair currentAST = new ASTPair();
		AST constructor_AST = null;
		
		switch ( LA(1)) {
		case STRINGLIT:
		case INT:
		{
			literal();
			astFactory.addASTChild(currentAST, returnAST);
			constructor_AST = (AST)currentAST.root;
			break;
		}
		case ID:
		{
			callconstructor();
			astFactory.addASTChild(currentAST, returnAST);
			constructor_AST = (AST)currentAST.root;
			break;
		}
		default:
		{
			throw new NoViableAltException(LT(1), getFilename());
		}
		}
		returnAST = constructor_AST;
	}
	
	public final void callconstructor() throws RecognitionException, TokenStreamException {
		
		returnAST = null;
		ASTPair currentAST = new ASTPair();
		AST callconstructor_AST = null;
		
		AST tmp135_AST = null;
		tmp135_AST = astFactory.create(LT(1));
		astFactory.makeASTRoot(currentAST, tmp135_AST);
		match(ID);
		match(LPAREN);
		literal();
		astFactory.addASTChild(currentAST, returnAST);
		{
		_loop74:
		do {
			if ((LA(1)==COMMA)) {
				AST tmp137_AST = null;
				tmp137_AST = astFactory.create(LT(1));
				astFactory.addASTChild(currentAST, tmp137_AST);
				match(COMMA);
				literal();
				astFactory.addASTChild(currentAST, returnAST);
			}
			else {
				break _loop74;
			}
			
		} while (true);
		}
		match(RPAREN);
		callconstructor_AST = (AST)currentAST.root;
		returnAST = callconstructor_AST;
	}
	
	public final void valcmpop() throws RecognitionException, TokenStreamException {
		
		returnAST = null;
		ASTPair currentAST = new ASTPair();
		AST valcmpop_AST = null;
		
		valcalc();
		astFactory.addASTChild(currentAST, returnAST);
		{
		_loop82:
		do {
			if (((LA(1) >= LTHAN && LA(1) <= GEQ))) {
				{
				switch ( LA(1)) {
				case LTHAN:
				{
					AST tmp139_AST = null;
					tmp139_AST = astFactory.create(LT(1));
					astFactory.makeASTRoot(currentAST, tmp139_AST);
					match(LTHAN);
					break;
				}
				case GTHAN:
				{
					AST tmp140_AST = null;
					tmp140_AST = astFactory.create(LT(1));
					astFactory.makeASTRoot(currentAST, tmp140_AST);
					match(GTHAN);
					break;
				}
				case DBLEQ:
				{
					AST tmp141_AST = null;
					tmp141_AST = astFactory.create(LT(1));
					astFactory.makeASTRoot(currentAST, tmp141_AST);
					match(DBLEQ);
					break;
				}
				case NOTEQ:
				{
					AST tmp142_AST = null;
					tmp142_AST = astFactory.create(LT(1));
					astFactory.makeASTRoot(currentAST, tmp142_AST);
					match(NOTEQ);
					break;
				}
				case LEQ:
				{
					AST tmp143_AST = null;
					tmp143_AST = astFactory.create(LT(1));
					astFactory.makeASTRoot(currentAST, tmp143_AST);
					match(LEQ);
					break;
				}
				case GEQ:
				{
					AST tmp144_AST = null;
					tmp144_AST = astFactory.create(LT(1));
					astFactory.makeASTRoot(currentAST, tmp144_AST);
					match(GEQ);
					break;
				}
				default:
				{
					throw new NoViableAltException(LT(1), getFilename());
				}
				}
				}
				valcalc();
				astFactory.addASTChild(currentAST, returnAST);
			}
			else {
				break _loop82;
			}
			
		} while (true);
		}
		valcmpop_AST = (AST)currentAST.root;
		returnAST = valcmpop_AST;
	}
	
	public final void valcalc() throws RecognitionException, TokenStreamException {
		
		returnAST = null;
		ASTPair currentAST = new ASTPair();
		AST valcalc_AST = null;
		
		valatom();
		astFactory.addASTChild(currentAST, returnAST);
		valcalc_AST = (AST)currentAST.root;
		returnAST = valcalc_AST;
	}
	
	public final void valatom() throws RecognitionException, TokenStreamException {
		
		returnAST = null;
		ASTPair currentAST = new ASTPair();
		AST valatom_AST = null;
		
		switch ( LA(1)) {
		case STRINGLIT:
		case INT:
		{
			literal();
			astFactory.addASTChild(currentAST, returnAST);
			valatom_AST = (AST)currentAST.root;
			break;
		}
		case ANSWER:
		{
			AST tmp145_AST = null;
			tmp145_AST = astFactory.create(LT(1));
			astFactory.addASTChild(currentAST, tmp145_AST);
			match(ANSWER);
			valatom_AST = (AST)currentAST.root;
			break;
		}
		case LPAREN:
		{
			match(LPAREN);
			valexpr();
			astFactory.addASTChild(currentAST, returnAST);
			match(RPAREN);
			valatom_AST = (AST)currentAST.root;
			break;
		}
		default:
			if ((LA(1)==ID) && (_tokenSet_3.member(LA(2)))) {
				AST tmp148_AST = null;
				tmp148_AST = astFactory.create(LT(1));
				astFactory.addASTChild(currentAST, tmp148_AST);
				match(ID);
				valatom_AST = (AST)currentAST.root;
			}
			else if ((LA(1)==ID) && (LA(2)==LPAREN)) {
				callconstructor();
				astFactory.addASTChild(currentAST, returnAST);
				valatom_AST = (AST)currentAST.root;
				valatom_AST = (AST)astFactory.make( (new ASTArray(2)).add(astFactory.create(CONSTRUCTOR,"CONSTRUCTOR")).add(valatom_AST));
				currentAST.root = valatom_AST;
				currentAST.child = valatom_AST!=null &&valatom_AST.getFirstChild()!=null ?
					valatom_AST.getFirstChild() : valatom_AST;
				currentAST.advanceChildToEnd();
				valatom_AST = (AST)currentAST.root;
			}
		else {
			throw new NoViableAltException(LT(1), getFilename());
		}
		}
		returnAST = valatom_AST;
	}
	
	
	public static final String[] _tokenNames = {
		"<0>",
		"EOF",
		"<2>",
		"NULL_TREE_LOOKAHEAD",
		"\"defines\"",
		"LBRACE",
		"SEMI",
		"RBRACE",
		"ID",
		"EQ",
		"\"include\"",
		"STRINGLIT",
		"\"options\"",
		"\"tutorname\"",
		"\"sequence\"",
		"\"then\"",
		"\"until\"",
		"\"and\"",
		"\"or\"",
		"LBRACK",
		"RBRACK",
		"LPAREN",
		"RPAREN",
		"DOT",
		"COLON",
		"\"mappings\"",
		"MAPTO",
		"\"switch\"",
		"COMMA",
		"PLUS",
		"\"concat\"",
		"\"priority\"",
		"INT",
		"\"focusedonly\"",
		"\"noqiv\"",
		"\"feedback\"",
		"\"variables\"",
		"\"Hint\"",
		"\"JIT\"",
		"\"answer\"",
		"DBLAND",
		"DBLOR",
		"LTHAN",
		"GTHAN",
		"DBLEQ",
		"NOTEQ",
		"LEQ",
		"GEQ",
		"TOP",
		"NODEID",
		"MAPEXPRHALF",
		"MAPOPTIONS",
		"DEFINE",
		"GROUP",
		"NODEFEEDBACK",
		"FLOATLIT",
		"CONSTRUCTOR",
		"STAR",
		"WS",
		"COMMENT1",
		"COMMENT2"
	};
	
	protected void buildTokenTypeASTClassMap() {
		tokenTypeToASTClassMap=null;
	};
	
	private static final long[] mk_tokenSet_0() {
		long[] data = { 1208484096L, 0L};
		return data;
	}
	public static final BitSet _tokenSet_0 = new BitSet(mk_tokenSet_0());
	private static final long[] mk_tokenSet_1() {
		long[] data = { 280375737712832L, 0L};
		return data;
	}
	public static final BitSet _tokenSet_1 = new BitSet(mk_tokenSet_1());
	private static final long[] mk_tokenSet_2() {
		long[] data = { 962072674560L, 0L};
		return data;
	}
	public static final BitSet _tokenSet_2 = new BitSet(mk_tokenSet_2());
	private static final long[] mk_tokenSet_3() {
		long[] data = { 280375469277312L, 0L};
		return data;
	}
	public static final BitSet _tokenSet_3 = new BitSet(mk_tokenSet_3());
	
	}
