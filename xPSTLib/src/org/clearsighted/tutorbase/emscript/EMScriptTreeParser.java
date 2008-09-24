// $ANTLR : "emscript.g" -> "EMScriptTreeParser.java"$

package org.clearsighted.tutorbase.emscript;

import java.util.HashMap;
import java.util.List;
import java.util.ArrayList;
import org.clearsighted.tutorbase.emscript.mappingtree.*;
import org.clearsighted.tutorbase.emscript.exprtree.*;
import org.clearsighted.tutorengine.*;
import org.clearsighted.tutorengine.types.Operations.Op;

import antlr.TreeParser;
import antlr.Token;
import antlr.collections.AST;
import antlr.RecognitionException;
import antlr.ANTLRException;
import antlr.NoViableAltException;
import antlr.MismatchedTokenException;
import antlr.SemanticException;
import antlr.collections.impl.BitSet;
import antlr.ASTPair;
import antlr.collections.impl.ASTArray;


public class EMScriptTreeParser extends antlr.TreeParser       implements EMScriptParserTokenTypes
 {

	Tree MTree;
	Tutor MTutor;
	RuleOptions Options;   // this gets set to the default if no options specified, or set accordingly if there are options
	                // it's then used by the subsequent mapping. (see 'mapoptions', 'renameapp', etc.)
	List<LeafNode> CurGroup = null; // nodes mentioned inside brackets in a sequence are added to a group.
public EMScriptTreeParser() {
	tokenNames = _tokenNames;
}

	public final Object[]  createMappingAndTutor(AST _t) throws RecognitionException {
		Object[] t = null;
		
		AST createMappingAndTutor_AST_in = (_t == ASTNULL) ? null : (AST)_t;
		MTree = new Tree(); MTutor = new Tutor();
		
		try {      // for error handling
			AST __t129 = _t;
			AST tmp1_AST_in = (AST)_t;
			match(_t,TOP);
			_t = _t.getFirstChild();
			{
			_loop131:
			do {
				if (_t==null) _t=ASTNULL;
				switch ( _t.getType()) {
				case SEQUENCE:
				{
					seq(_t);
					_t = _retTree;
					break;
				}
				case MAPPINGS:
				{
					map(_t);
					_t = _retTree;
					break;
				}
				case OPTIONS:
				{
					opt(_t);
					_t = _retTree;
					break;
				}
				case FEEDBACK:
				{
					feedback(_t);
					_t = _retTree;
					break;
				}
				default:
				{
					break _loop131;
				}
				}
			} while (true);
			}
			_t = __t129;
			_t = _t.getNextSibling();
			if ( inputState.guessing==0 ) {
				MTree.finishConstruction(); t = new Object[2]; t[0] = MTree; t[1] = MTutor;
			}
		}
		catch (RecognitionException ex) {
			if (inputState.guessing==0) {
				reportError(ex);
				if (_t!=null) {_t = _t.getNextSibling();}
			} else {
			  throw ex;
			}
		}
		_retTree = _t;
		return t;
	}
	
	public final void seq(AST _t) throws RecognitionException {
		
		AST seq_AST_in = (_t == ASTNULL) ? null : (AST)_t;
		
		try {      // for error handling
			AST __t138 = _t;
			AST tmp2_AST_in = (AST)_t;
			match(_t,SEQUENCE);
			_t = _t.getFirstChild();
			if ( inputState.guessing==0 ) {
				CurGroup = null;
			}
			expr(_t);
			_t = _retTree;
			_t = __t138;
			_t = _t.getNextSibling();
		}
		catch (RecognitionException ex) {
			if (inputState.guessing==0) {
				reportError(ex);
				if (_t!=null) {_t = _t.getNextSibling();}
			} else {
			  throw ex;
			}
		}
		_retTree = _t;
	}
	
	public final void map(AST _t) throws RecognitionException {
		
		AST map_AST_in = (_t == ASTNULL) ? null : (AST)_t;
		
		try {      // for error handling
			AST __t146 = _t;
			AST tmp3_AST_in = (AST)_t;
			match(_t,MAPPINGS);
			_t = _t.getFirstChild();
			{
			_loop148:
			do {
				if (_t==null) _t=ASTNULL;
				if ((_t.getType()==MAPTO)) {
					mapto(_t);
					_t = _retTree;
				}
				else {
					break _loop148;
				}
				
			} while (true);
			}
			_t = __t146;
			_t = _t.getNextSibling();
		}
		catch (RecognitionException ex) {
			if (inputState.guessing==0) {
				reportError(ex);
				if (_t!=null) {_t = _t.getNextSibling();}
			} else {
			  throw ex;
			}
		}
		_retTree = _t;
	}
	
	public final void opt(AST _t) throws RecognitionException {
		
		AST opt_AST_in = (_t == ASTNULL) ? null : (AST)_t;
		
		try {      // for error handling
			AST __t133 = _t;
			AST tmp4_AST_in = (AST)_t;
			match(_t,OPTIONS);
			_t = _t.getFirstChild();
			{
			_loop135:
			do {
				if (_t==null) _t=ASTNULL;
				if ((_t.getType()==TUTORNAME)) {
					option(_t);
					_t = _retTree;
				}
				else {
					break _loop135;
				}
				
			} while (true);
			}
			_t = __t133;
			_t = _t.getNextSibling();
		}
		catch (RecognitionException ex) {
			if (inputState.guessing==0) {
				reportError(ex);
				if (_t!=null) {_t = _t.getNextSibling();}
			} else {
			  throw ex;
			}
		}
		_retTree = _t;
	}
	
	public final void feedback(AST _t) throws RecognitionException {
		
		AST feedback_AST_in = (_t == ASTNULL) ? null : (AST)_t;
		
		try {      // for error handling
			AST __t189 = _t;
			AST tmp5_AST_in = (AST)_t;
			match(_t,FEEDBACK);
			_t = _t.getFirstChild();
			{
			_loop191:
			do {
				if (_t==null) _t=ASTNULL;
				if ((_t.getType()==VARIABLES||_t.getType()==NODEFEEDBACK)) {
					feedbackstmt(_t);
					_t = _retTree;
				}
				else {
					break _loop191;
				}
				
			} while (true);
			}
			_t = __t189;
			_t = _t.getNextSibling();
		}
		catch (RecognitionException ex) {
			if (inputState.guessing==0) {
				reportError(ex);
				if (_t!=null) {_t = _t.getNextSibling();}
			} else {
			  throw ex;
			}
		}
		_retTree = _t;
	}
	
	public final void option(AST _t) throws RecognitionException {
		
		AST option_AST_in = (_t == ASTNULL) ? null : (AST)_t;
		AST tutorname = null;
		
		try {      // for error handling
			AST tmp6_AST_in = (AST)_t;
			match(_t,TUTORNAME);
			_t = _t.getNextSibling();
			tutorname = (AST)_t;
			match(_t,STRINGLIT);
			_t = _t.getNextSibling();
			if ( inputState.guessing==0 ) {
				/*MTree.setTutorName(tutorname.getText());*/
			}
		}
		catch (RecognitionException ex) {
			if (inputState.guessing==0) {
				reportError(ex);
				if (_t!=null) {_t = _t.getNextSibling();}
			} else {
			  throw ex;
			}
		}
		_retTree = _t;
	}
	
	public final MappingNode  expr(AST _t) throws RecognitionException {
		MappingNode n = null;
		
		AST expr_AST_in = (_t == ASTNULL) ? null : (AST)_t;
		AST node = null;
		MappingNode l = null, r = null;
		
		try {      // for error handling
			if (_t==null) _t=ASTNULL;
			switch ( _t.getType()) {
			case AND:
			{
				AST __t140 = _t;
				AST tmp7_AST_in = (AST)_t;
				match(_t,AND);
				_t = _t.getFirstChild();
				l=expr(_t);
				_t = _retTree;
				r=expr(_t);
				_t = _retTree;
				_t = __t140;
				_t = _t.getNextSibling();
				if ( inputState.guessing==0 ) {
					
						n = MTree.createAndNode(l, r);
					
				}
				break;
			}
			case OR:
			{
				AST __t141 = _t;
				AST tmp8_AST_in = (AST)_t;
				match(_t,OR);
				_t = _t.getFirstChild();
				l=expr(_t);
				_t = _retTree;
				r=expr(_t);
				_t = _retTree;
				_t = __t141;
				_t = _t.getNextSibling();
				if ( inputState.guessing==0 ) {
					
						n = MTree.createOrNode(l, r);
					
				}
				break;
			}
			case THEN:
			{
				AST __t142 = _t;
				AST tmp9_AST_in = (AST)_t;
				match(_t,THEN);
				_t = _t.getFirstChild();
				l=expr(_t);
				_t = _retTree;
				r=expr(_t);
				_t = _retTree;
				_t = __t142;
				_t = _t.getNextSibling();
				if ( inputState.guessing==0 ) {
					
						n = MTree.createThenNode(l, r);
					
				}
				break;
			}
			case UNTIL:
			{
				AST __t143 = _t;
				AST tmp10_AST_in = (AST)_t;
				match(_t,UNTIL);
				_t = _t.getFirstChild();
				l=expr(_t);
				_t = _retTree;
				r=expr(_t);
				_t = _retTree;
				_t = __t143;
				_t = _t.getNextSibling();
				if ( inputState.guessing==0 ) {
					
						n = MTree.createUntilNode(l, r);
					
				}
				break;
			}
			case NODEID:
			{
				node = (AST)_t;
				match(_t,NODEID);
				_t = _t.getNextSibling();
				if ( inputState.guessing==0 ) {
					
						LeafNode ln = MTree.createLeafNode(node.getText(), node.getLine(), node.getColumn());
						n = ln;
						if (CurGroup != null)
							CurGroup.add(ln);
					
				}
				break;
			}
			case GROUP:
			{
				AST __t144 = _t;
				AST tmp11_AST_in = (AST)_t;
				match(_t,GROUP);
				_t = _t.getFirstChild();
				if ( inputState.guessing==0 ) {
					CurGroup = new ArrayList<LeafNode>();
				}
				n=expr(_t);
				_t = _retTree;
				if ( inputState.guessing==0 ) {
					MTree.groups.add(CurGroup); CurGroup = null;
				}
				_t = __t144;
				_t = _t.getNextSibling();
				break;
			}
			default:
			{
				throw new NoViableAltException(_t);
			}
			}
		}
		catch (RecognitionException ex) {
			if (inputState.guessing==0) {
				reportError(ex);
				if (_t!=null) {_t = _t.getNextSibling();}
			} else {
			  throw ex;
			}
		}
		_retTree = _t;
		return n;
	}
	
	public final void mapto(AST _t) throws RecognitionException {
		
		AST mapto_AST_in = (_t == ASTNULL) ? null : (AST)_t;
		
		try {      // for error handling
			boolean synPredMatched154 = false;
			if (_t==null) _t=ASTNULL;
			if (((_t.getType()==MAPTO))) {
				AST __t154 = _t;
				synPredMatched154 = true;
				inputState.guessing++;
				try {
					{
					AST __t151 = _t;
					AST tmp12_AST_in = (AST)_t;
					match(_t,MAPTO);
					_t = _t.getFirstChild();
					{
					if (_t==null) _t=ASTNULL;
					switch ( _t.getType()) {
					case MAPOPTIONS:
					{
						AST tmp13_AST_in = (AST)_t;
						match(_t,MAPOPTIONS);
						_t = _t.getNextSibling();
						break;
					}
					case MAPEXPRHALF:
					{
						break;
					}
					default:
					{
						throw new NoViableAltException(_t);
					}
					}
					}
					AST __t153 = _t;
					AST tmp14_AST_in = (AST)_t;
					match(_t,MAPEXPRHALF);
					_t = _t.getFirstChild();
					AST tmp15_AST_in = (AST)_t;
					match(_t,NODEID);
					_t = _t.getNextSibling();
					_t = __t153;
					_t = _t.getNextSibling();
					_t = __t151;
					_t = _t.getNextSibling();
					}
				}
				catch (RecognitionException pe) {
					synPredMatched154 = false;
				}
				_t = __t154;
inputState.guessing--;
			}
			if ( synPredMatched154 ) {
				AST __t155 = _t;
				AST tmp16_AST_in = (AST)_t;
				match(_t,MAPTO);
				_t = _t.getFirstChild();
				mapoptions(_t);
				_t = _retTree;
				renameapp(_t);
				_t = _retTree;
				_t = __t155;
				_t = _t.getNextSibling();
			}
			else {
				boolean synPredMatched160 = false;
				if (_t==null) _t=ASTNULL;
				if (((_t.getType()==MAPTO))) {
					AST __t160 = _t;
					synPredMatched160 = true;
					inputState.guessing++;
					try {
						{
						AST __t157 = _t;
						AST tmp17_AST_in = (AST)_t;
						match(_t,MAPTO);
						_t = _t.getFirstChild();
						{
						if (_t==null) _t=ASTNULL;
						switch ( _t.getType()) {
						case MAPOPTIONS:
						{
							AST tmp18_AST_in = (AST)_t;
							match(_t,MAPOPTIONS);
							_t = _t.getNextSibling();
							break;
						}
						case MAPEXPRHALF:
						{
							break;
						}
						default:
						{
							throw new NoViableAltException(_t);
						}
						}
						}
						AST __t159 = _t;
						AST tmp19_AST_in = (AST)_t;
						match(_t,MAPEXPRHALF);
						_t = _t.getFirstChild();
						AST tmp20_AST_in = (AST)_t;
						match(_t,SWITCH);
						_t = _t.getNextSibling();
						_t = __t159;
						_t = _t.getNextSibling();
						_t = __t157;
						_t = _t.getNextSibling();
						}
					}
					catch (RecognitionException pe) {
						synPredMatched160 = false;
					}
					_t = __t160;
inputState.guessing--;
				}
				if ( synPredMatched160 ) {
					AST __t161 = _t;
					AST tmp21_AST_in = (AST)_t;
					match(_t,MAPTO);
					_t = _t.getFirstChild();
					mapoptions(_t);
					_t = _retTree;
					switcht(_t);
					_t = _retTree;
					_t = __t161;
					_t = _t.getNextSibling();
				}
				else if ((_t.getType()==MAPTO)) {
					AST __t162 = _t;
					AST tmp22_AST_in = (AST)_t;
					match(_t,MAPTO);
					_t = _t.getFirstChild();
					mapoptions(_t);
					_t = _retTree;
					concat(_t);
					_t = _retTree;
					_t = __t162;
					_t = _t.getNextSibling();
				}
				else {
					throw new NoViableAltException(_t);
				}
				}
			}
			catch (RecognitionException ex) {
				if (inputState.guessing==0) {
					reportError(ex);
					if (_t!=null) {_t = _t.getNextSibling();}
				} else {
				  throw ex;
				}
			}
			_retTree = _t;
		}
		
	public final void mapoptions(AST _t) throws RecognitionException {
		
		AST mapoptions_AST_in = (_t == ASTNULL) ? null : (AST)_t;
		
		try {      // for error handling
			if ( inputState.guessing==0 ) {
				Options = new RuleOptions();
			}
			{
			if (_t==null) _t=ASTNULL;
			switch ( _t.getType()) {
			case MAPOPTIONS:
			{
				AST __t182 = _t;
				AST tmp23_AST_in = (AST)_t;
				match(_t,MAPOPTIONS);
				_t = _t.getFirstChild();
				{
				_loop184:
				do {
					if (_t==null) _t=ASTNULL;
					if ((_t.getType()==PRIORITY||_t.getType()==FOCUSEDONLY||_t.getType()==NOQIV)) {
						mapoption(_t);
						_t = _retTree;
					}
					else {
						break _loop184;
					}
					
				} while (true);
				}
				_t = __t182;
				_t = _t.getNextSibling();
				break;
			}
			case MAPEXPRHALF:
			{
				break;
			}
			default:
			{
				throw new NoViableAltException(_t);
			}
			}
			}
		}
		catch (RecognitionException ex) {
			if (inputState.guessing==0) {
				reportError(ex);
				if (_t!=null) {_t = _t.getNextSibling();}
			} else {
			  throw ex;
			}
		}
		_retTree = _t;
	}
	
	public final void renameapp(AST _t) throws RecognitionException {
		
		AST renameapp_AST_in = (_t == ASTNULL) ? null : (AST)_t;
		AST app = null;
		AST tut = null;
		
		try {      // for error handling
			AST __t164 = _t;
			AST tmp24_AST_in = (AST)_t;
			match(_t,MAPEXPRHALF);
			_t = _t.getFirstChild();
			app = (AST)_t;
			match(_t,NODEID);
			_t = _t.getNextSibling();
			_t = __t164;
			_t = _t.getNextSibling();
			AST __t165 = _t;
			AST tmp25_AST_in = (AST)_t;
			match(_t,MAPEXPRHALF);
			_t = _t.getFirstChild();
			tut = (AST)_t;
			match(_t,NODEID);
			_t = _t.getNextSibling();
			_t = __t165;
			_t = _t.getNextSibling();
			if ( inputState.guessing==0 ) {
				MTree.getOrCreateMapping(tut.getText()).putRenameAction(Options, app.getText(), tut.getText());
			}
		}
		catch (RecognitionException ex) {
			if (inputState.guessing==0) {
				reportError(ex);
				if (_t!=null) {_t = _t.getNextSibling();}
			} else {
			  throw ex;
			}
		}
		_retTree = _t;
	}
	
	public final void switcht(AST _t) throws RecognitionException {
		
		AST switcht_AST_in = (_t == ASTNULL) ? null : (AST)_t;
		AST s = null;
		AST tut = null;
		
		try {      // for error handling
			AST __t167 = _t;
			AST tmp26_AST_in = (AST)_t;
			match(_t,MAPEXPRHALF);
			_t = _t.getFirstChild();
			s = (AST)_t;
			match(_t,SWITCH);
			_t = _t.getNextSibling();
			_t = __t167;
			_t = _t.getNextSibling();
			AST __t168 = _t;
			AST tmp27_AST_in = (AST)_t;
			match(_t,MAPEXPRHALF);
			_t = _t.getFirstChild();
			tut = (AST)_t;
			match(_t,NODEID);
			_t = _t.getNextSibling();
			_t = __t168;
			_t = _t.getNextSibling();
			if ( inputState.guessing==0 ) {
				switchint(s, tut.getText());
			}
		}
		catch (RecognitionException ex) {
			if (inputState.guessing==0) {
				reportError(ex);
				if (_t!=null) {_t = _t.getNextSibling();}
			} else {
			  throw ex;
			}
		}
		_retTree = _t;
	}
	
	public final void concat(AST _t) throws RecognitionException {
		
		AST concat_AST_in = (_t == ASTNULL) ? null : (AST)_t;
		AST c = null;
		AST tut = null;
		
		try {      // for error handling
			AST __t174 = _t;
			AST tmp28_AST_in = (AST)_t;
			match(_t,MAPEXPRHALF);
			_t = _t.getFirstChild();
			c = (AST)_t;
			match(_t,CONCAT);
			_t = _t.getNextSibling();
			_t = __t174;
			_t = _t.getNextSibling();
			AST __t175 = _t;
			AST tmp29_AST_in = (AST)_t;
			match(_t,MAPEXPRHALF);
			_t = _t.getFirstChild();
			tut = (AST)_t;
			match(_t,NODEID);
			_t = _t.getNextSibling();
			_t = __t175;
			_t = _t.getNextSibling();
			if ( inputState.guessing==0 ) {
				concatint(c, tut.getText());
			}
		}
		catch (RecognitionException ex) {
			if (inputState.guessing==0) {
				reportError(ex);
				if (_t!=null) {_t = _t.getNextSibling();}
			} else {
			  throw ex;
			}
		}
		_retTree = _t;
	}
	
	public final void switchint(AST _t,
		String tut
	) throws RecognitionException {
		
		AST switchint_AST_in = (_t == ASTNULL) ? null : (AST)_t;
		AST app = null;
		AST val = null;
		SwitchAction ga = null;
		
		try {      // for error handling
			AST __t170 = _t;
			AST tmp30_AST_in = (AST)_t;
			match(_t,SWITCH);
			_t = _t.getFirstChild();
			if ( inputState.guessing==0 ) {
				ga = MTree.getOrCreateMapping(tut).putSwitchAction(Options, tut);
			}
			{
			int _cnt172=0;
			_loop172:
			do {
				if (_t==null) _t=ASTNULL;
				if ((_t.getType()==NODEID)) {
					app = (AST)_t;
					match(_t,NODEID);
					_t = _t.getNextSibling();
					val = _t==ASTNULL ? null : (AST)_t;
					literal(_t);
					_t = _retTree;
					if ( inputState.guessing==0 ) {
						ga.putValuePair(Options, app.getText(), val.getText());
					}
				}
				else {
					if ( _cnt172>=1 ) { break _loop172; } else {throw new NoViableAltException(_t);}
				}
				
				_cnt172++;
			} while (true);
			}
			_t = __t170;
			_t = _t.getNextSibling();
		}
		catch (RecognitionException ex) {
			if (inputState.guessing==0) {
				reportError(ex);
				if (_t!=null) {_t = _t.getNextSibling();}
			} else {
			  throw ex;
			}
		}
		_retTree = _t;
	}
	
	public final void literal(AST _t) throws RecognitionException {
		
		AST literal_AST_in = (_t == ASTNULL) ? null : (AST)_t;
		
		try {      // for error handling
			if (_t==null) _t=ASTNULL;
			switch ( _t.getType()) {
			case STRINGLIT:
			{
				AST tmp31_AST_in = (AST)_t;
				match(_t,STRINGLIT);
				_t = _t.getNextSibling();
				break;
			}
			case INT:
			{
				AST tmp32_AST_in = (AST)_t;
				match(_t,INT);
				_t = _t.getNextSibling();
				break;
			}
			case FLOATLIT:
			{
				AST tmp33_AST_in = (AST)_t;
				match(_t,FLOATLIT);
				_t = _t.getNextSibling();
				break;
			}
			default:
			{
				throw new NoViableAltException(_t);
			}
			}
		}
		catch (RecognitionException ex) {
			if (inputState.guessing==0) {
				reportError(ex);
				if (_t!=null) {_t = _t.getNextSibling();}
			} else {
			  throw ex;
			}
		}
		_retTree = _t;
	}
	
	public final void concatint(AST _t,
		String tut
	) throws RecognitionException {
		
		AST concatint_AST_in = (_t == ASTNULL) ? null : (AST)_t;
		AST app = null;
		ConcatAction ca = null;
		
		try {      // for error handling
			AST __t177 = _t;
			AST tmp34_AST_in = (AST)_t;
			match(_t,CONCAT);
			_t = _t.getFirstChild();
			if ( inputState.guessing==0 ) {
				ca = MTree.getOrCreateMapping(tut).putConcatAction(Options, tut);
			}
			{
			int _cnt179=0;
			_loop179:
			do {
				if (_t==null) _t=ASTNULL;
				if ((_t.getType()==NODEID)) {
					app = (AST)_t;
					match(_t,NODEID);
					_t = _t.getNextSibling();
					if ( inputState.guessing==0 ) {
						ca.putAppNode(Options, app.getText());
					}
				}
				else {
					if ( _cnt179>=1 ) { break _loop179; } else {throw new NoViableAltException(_t);}
				}
				
				_cnt179++;
			} while (true);
			}
			_t = __t177;
			_t = _t.getNextSibling();
		}
		catch (RecognitionException ex) {
			if (inputState.guessing==0) {
				reportError(ex);
				if (_t!=null) {_t = _t.getNextSibling();}
			} else {
			  throw ex;
			}
		}
		_retTree = _t;
	}
	
	public final void mapoption(AST _t) throws RecognitionException {
		
		AST mapoption_AST_in = (_t == ASTNULL) ? null : (AST)_t;
		AST pr = null;
		
		try {      // for error handling
			if (_t==null) _t=ASTNULL;
			switch ( _t.getType()) {
			case PRIORITY:
			{
				AST __t186 = _t;
				AST tmp35_AST_in = (AST)_t;
				match(_t,PRIORITY);
				_t = _t.getFirstChild();
				pr = (AST)_t;
				match(_t,INT);
				_t = _t.getNextSibling();
				_t = __t186;
				_t = _t.getNextSibling();
				if ( inputState.guessing==0 ) {
					Options.priority = Integer.parseInt(pr.getText());
				}
				break;
			}
			case FOCUSEDONLY:
			{
				AST tmp36_AST_in = (AST)_t;
				match(_t,FOCUSEDONLY);
				_t = _t.getNextSibling();
				if ( inputState.guessing==0 ) {
					Options.focusedOnly = true;
				}
				break;
			}
			case NOQIV:
			{
				AST tmp37_AST_in = (AST)_t;
				match(_t,NOQIV);
				_t = _t.getNextSibling();
				if ( inputState.guessing==0 ) {
					Options.noQIV = true;
				}
				break;
			}
			default:
			{
				throw new NoViableAltException(_t);
			}
			}
		}
		catch (RecognitionException ex) {
			if (inputState.guessing==0) {
				reportError(ex);
				if (_t!=null) {_t = _t.getNextSibling();}
			} else {
			  throw ex;
			}
		}
		_retTree = _t;
	}
	
	public final void feedbackstmt(AST _t) throws RecognitionException {
		
		AST feedbackstmt_AST_in = (_t == ASTNULL) ? null : (AST)_t;
		
		try {      // for error handling
			{
			if (_t==null) _t=ASTNULL;
			switch ( _t.getType()) {
			case VARIABLES:
			{
				variabledecls(_t);
				_t = _retTree;
				break;
			}
			case NODEFEEDBACK:
			{
				nodefeedback(_t);
				_t = _retTree;
				break;
			}
			default:
			{
				throw new NoViableAltException(_t);
			}
			}
			}
		}
		catch (RecognitionException ex) {
			if (inputState.guessing==0) {
				reportError(ex);
				if (_t!=null) {_t = _t.getNextSibling();}
			} else {
			  throw ex;
			}
		}
		_retTree = _t;
	}
	
	public final void variabledecls(AST _t) throws RecognitionException {
		
		AST variabledecls_AST_in = (_t == ASTNULL) ? null : (AST)_t;
		
		try {      // for error handling
			AST __t216 = _t;
			AST tmp38_AST_in = (AST)_t;
			match(_t,VARIABLES);
			_t = _t.getFirstChild();
			variabledecl(_t);
			_t = _retTree;
			_t = __t216;
			_t = _t.getNextSibling();
		}
		catch (RecognitionException ex) {
			if (inputState.guessing==0) {
				reportError(ex);
				if (_t!=null) {_t = _t.getNextSibling();}
			} else {
			  throw ex;
			}
		}
		_retTree = _t;
	}
	
	public final void nodefeedback(AST _t) throws RecognitionException {
		
		AST nodefeedback_AST_in = (_t == ASTNULL) ? null : (AST)_t;
		AST n = null;
		AST h = null;
		AST j2 = null;
		AST j = null;
		AST a = null;
		AST f = null;
		AST g = null;
		ExprNode c = null;
		
		try {      // for error handling
			AST __t195 = _t;
			AST tmp39_AST_in = (AST)_t;
			match(_t,NODEFEEDBACK);
			_t = _t.getFirstChild();
			n = (AST)_t;
			match(_t,NODEID);
			_t = _t.getNextSibling();
			{
			_loop205:
			do {
				if (_t==null) _t=ASTNULL;
				switch ( _t.getType()) {
				case HINT:
				{
					AST __t197 = _t;
					AST tmp40_AST_in = (AST)_t;
					match(_t,HINT);
					_t = _t.getFirstChild();
					h = (AST)_t;
					match(_t,STRINGLIT);
					_t = _t.getNextSibling();
					_t = __t197;
					_t = _t.getNextSibling();
					if ( inputState.guessing==0 ) {
						
									MTutor.getOrCreateGoalNode(n.getText()).addHint(h.getText());
								
					}
					break;
				}
				case ANSWER:
				{
					AST __t203 = _t;
					a = _t==ASTNULL ? null :(AST)_t;
					match(_t,ANSWER);
					_t = _t.getFirstChild();
					AST tmp41_AST_in = (AST)_t;
					if ( _t==null ) throw new MismatchedTokenException();
					_t = _t.getNextSibling();
					_t = __t203;
					_t = _t.getNextSibling();
					if ( inputState.guessing==0 ) {
						
									MTutor.getOrCreateGoalNode(n.getText()).setProperty(a.getText(), a);
								
					}
					break;
				}
				case ID:
				{
					AST __t204 = _t;
					f = _t==ASTNULL ? null :(AST)_t;
					match(_t,ID);
					_t = _t.getFirstChild();
					g = _t==ASTNULL ? null : (AST)_t;
					literal(_t);
					_t = _retTree;
					_t = __t204;
					_t = _t.getNextSibling();
					if ( inputState.guessing==0 ) {
						
									MTutor.getOrCreateGoalNode(n.getText()).setProperty(f.getText(), g.getText());
								
					}
					break;
				}
				default:
					boolean synPredMatched200 = false;
					if (_t==null) _t=ASTNULL;
					if (((_t.getType()==JIT))) {
						AST __t200 = _t;
						synPredMatched200 = true;
						inputState.guessing++;
						try {
							{
							AST __t199 = _t;
							AST tmp42_AST_in = (AST)_t;
							match(_t,JIT);
							_t = _t.getFirstChild();
							AST tmp43_AST_in = (AST)_t;
							match(_t,STRINGLIT);
							_t = _t.getNextSibling();
							_t = __t199;
							_t = _t.getNextSibling();
							}
						}
						catch (RecognitionException pe) {
							synPredMatched200 = false;
						}
						_t = __t200;
inputState.guessing--;
					}
					if ( synPredMatched200 ) {
						AST __t201 = _t;
						AST tmp44_AST_in = (AST)_t;
						match(_t,JIT);
						_t = _t.getFirstChild();
						j2 = (AST)_t;
						match(_t,STRINGLIT);
						_t = _t.getNextSibling();
						_t = __t201;
						_t = _t.getNextSibling();
						if ( inputState.guessing==0 ) {
							
										MTutor.getOrCreateGoalNode(n.getText()).addJIT(null, j2.getText());
									
						}
					}
					else if ((_t.getType()==JIT)) {
						AST __t202 = _t;
						AST tmp45_AST_in = (AST)_t;
						match(_t,JIT);
						_t = _t.getFirstChild();
						c=valexpr(_t);
						_t = _retTree;
						j = (AST)_t;
						match(_t,STRINGLIT);
						_t = _t.getNextSibling();
						_t = __t202;
						_t = _t.getNextSibling();
						if ( inputState.guessing==0 ) {
							
										MTutor.getOrCreateGoalNode(n.getText()).addJIT(c, j.getText());
									
						}
					}
				else {
					break _loop205;
				}
				}
			} while (true);
			}
			_t = __t195;
			_t = _t.getNextSibling();
		}
		catch (RecognitionException ex) {
			if (inputState.guessing==0) {
				reportError(ex);
				if (_t!=null) {_t = _t.getNextSibling();}
			} else {
			  throw ex;
			}
		}
		_retTree = _t;
	}
	
	public final ExprNode  valexpr(AST _t) throws RecognitionException {
		ExprNode e = null;
		
		AST valexpr_AST_in = (_t == ASTNULL) ? null : (AST)_t;
		AST s = null;
		AST i = null;
		AST f = null;
		AST a = null;
		AST v = null;
		AST cn = null;
		ExprNode l = null, r = null;
		
		try {      // for error handling
			if (_t==null) _t=ASTNULL;
			switch ( _t.getType()) {
			case DBLAND:
			{
				AST __t207 = _t;
				AST tmp46_AST_in = (AST)_t;
				match(_t,DBLAND);
				_t = _t.getFirstChild();
				l=valexpr(_t);
				_t = _retTree;
				r=valexpr(_t);
				_t = _retTree;
				_t = __t207;
				_t = _t.getNextSibling();
				if ( inputState.guessing==0 ) {
					
							e = new BoolOpNode(BoolOpNode.OpType.AndOp, l, r);
						
				}
				break;
			}
			case DBLOR:
			{
				AST __t208 = _t;
				AST tmp47_AST_in = (AST)_t;
				match(_t,DBLOR);
				_t = _t.getFirstChild();
				l=valexpr(_t);
				_t = _retTree;
				r=valexpr(_t);
				_t = _retTree;
				_t = __t208;
				_t = _t.getNextSibling();
				if ( inputState.guessing==0 ) {
					
							e = new BoolOpNode(BoolOpNode.OpType.OrOp, l, r);
						
				}
				break;
			}
			case LTHAN:
			{
				AST __t209 = _t;
				AST tmp48_AST_in = (AST)_t;
				match(_t,LTHAN);
				_t = _t.getFirstChild();
				l=valexpr(_t);
				_t = _retTree;
				r=valexpr(_t);
				_t = _retTree;
				_t = __t209;
				_t = _t.getNextSibling();
				if ( inputState.guessing==0 ) {
					
							e = new CondNode(Op.LThanOp, l, r);
						
				}
				break;
			}
			case GTHAN:
			{
				AST __t210 = _t;
				AST tmp49_AST_in = (AST)_t;
				match(_t,GTHAN);
				_t = _t.getFirstChild();
				l=valexpr(_t);
				_t = _retTree;
				r=valexpr(_t);
				_t = _retTree;
				_t = __t210;
				_t = _t.getNextSibling();
				if ( inputState.guessing==0 ) {
					
							e = new CondNode(Op.GThanOp, l, r);
						
				}
				break;
			}
			case LEQ:
			{
				AST __t211 = _t;
				AST tmp50_AST_in = (AST)_t;
				match(_t,LEQ);
				_t = _t.getFirstChild();
				l=valexpr(_t);
				_t = _retTree;
				r=valexpr(_t);
				_t = _retTree;
				_t = __t211;
				_t = _t.getNextSibling();
				if ( inputState.guessing==0 ) {
					
							e = new CondNode(Op.LEqOp, l, r);
						
				}
				break;
			}
			case GEQ:
			{
				AST __t212 = _t;
				AST tmp51_AST_in = (AST)_t;
				match(_t,GEQ);
				_t = _t.getFirstChild();
				l=valexpr(_t);
				_t = _retTree;
				r=valexpr(_t);
				_t = _retTree;
				_t = __t212;
				_t = _t.getNextSibling();
				if ( inputState.guessing==0 ) {
					
							e = new CondNode(Op.GEqOp, l, r);
						
				}
				break;
			}
			case DBLEQ:
			{
				AST __t213 = _t;
				AST tmp52_AST_in = (AST)_t;
				match(_t,DBLEQ);
				_t = _t.getFirstChild();
				l=valexpr(_t);
				_t = _retTree;
				r=valexpr(_t);
				_t = _retTree;
				_t = __t213;
				_t = _t.getNextSibling();
				if ( inputState.guessing==0 ) {
					
							e = new CondNode(Op.EqOp, l, r);
						
				}
				break;
			}
			case NOTEQ:
			{
				AST __t214 = _t;
				AST tmp53_AST_in = (AST)_t;
				match(_t,NOTEQ);
				_t = _t.getFirstChild();
				l=valexpr(_t);
				_t = _retTree;
				r=valexpr(_t);
				_t = _retTree;
				_t = __t214;
				_t = _t.getNextSibling();
				if ( inputState.guessing==0 ) {
					
							e = new CondNode(Op.NotEqOp, l, r);
						
				}
				break;
			}
			case STRINGLIT:
			{
				s = (AST)_t;
				match(_t,STRINGLIT);
				_t = _t.getNextSibling();
				if ( inputState.guessing==0 ) {
					
							e = new StringLitNode(s.getText());
						
				}
				break;
			}
			case INT:
			{
				i = (AST)_t;
				match(_t,INT);
				_t = _t.getNextSibling();
				if ( inputState.guessing==0 ) {
					
							e = new IntLitNode(i.getText());
						
				}
				break;
			}
			case FLOATLIT:
			{
				f = (AST)_t;
				match(_t,FLOATLIT);
				_t = _t.getNextSibling();
				if ( inputState.guessing==0 ) {
					
							e = new FloatLitNode(f.getText());
						
				}
				break;
			}
			case ANSWER:
			{
				a = (AST)_t;
				match(_t,ANSWER);
				_t = _t.getNextSibling();
				if ( inputState.guessing==0 ) {
					
							e = new VarNode(a.getText());
						
				}
				break;
			}
			case ID:
			{
				v = (AST)_t;
				match(_t,ID);
				_t = _t.getNextSibling();
				if ( inputState.guessing==0 ) {
					
							e = new VarNode(v.getText());
						
				}
				break;
			}
			case CONSTRUCTOR:
			{
				cn = (AST)_t;
				match(_t,CONSTRUCTOR);
				_t = _t.getNextSibling();
				if ( inputState.guessing==0 ) {
					
							e = new ConstructorNode(cn);
						
				}
				break;
			}
			default:
			{
				throw new NoViableAltException(_t);
			}
			}
		}
		catch (RecognitionException ex) {
			if (inputState.guessing==0) {
				reportError(ex);
				if (_t!=null) {_t = _t.getNextSibling();}
			} else {
			  throw ex;
			}
		}
		_retTree = _t;
		return e;
	}
	
	public final void variabledecl(AST _t) throws RecognitionException {
		
		AST variabledecl_AST_in = (_t == ASTNULL) ? null : (AST)_t;
		AST k = null;
		AST v = null;
		
		try {      // for error handling
			AST __t218 = _t;
			k = _t==ASTNULL ? null :(AST)_t;
			match(_t,ID);
			_t = _t.getFirstChild();
			v = _t==ASTNULL ? null : (AST)_t;
			literal(_t);
			_t = _retTree;
			_t = __t218;
			_t = _t.getNextSibling();
			if ( inputState.guessing==0 ) {
				MTutor.addConstant(k.getText(), v.getText());
			}
		}
		catch (RecognitionException ex) {
			if (inputState.guessing==0) {
				reportError(ex);
				if (_t!=null) {_t = _t.getNextSibling();}
			} else {
			  throw ex;
			}
		}
		_retTree = _t;
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
	
	}
	
