$numNextMessageNum = 0;

function DorminParameter::create(%this,%name,%value)
{
   %this.strValueName = "VALUE";
	%this.strName = %name;
	%this.objValue = %value;
	return %this;
}

function TypeHolder::create(%this,%typeChar, %value){
	%this.typeChar = %typeChar;
	%this.value = %value;
	return %this;
}

function escapeString(%str)
{
	%str = strreplace(%str,"\\", "\\\\");
	%str = strreplace(%str,"\n", "\\n");
	return %str;
}

function DorminAddress::ConvertToBrackets(%this)
{
   %output = "";
   %temp = "";
   if(%this.strArrNames.count()>0){
	   %output = %this.strArrNames.getValue(0);
   }
   for(%i = 1; %i<%this.strArrNames.count();%i++){
	   if(strpos(%this.strArrNames.getValue(i),";")==-1)
	   {
   		%output = %output @ "." @ %this.strArrNames.getValue(i);
	   }
	   else
	   {
   		%temp = strreplace(%this.strArrNames.getValue(i),";","[") @ "]";
	   }
   }
   return %output;
}

function DorminAddress::ToDottedString(%this)
{
    %output = "";
    if(%this.strArrNames.count()>0){
      %output = %this.strArrNames.getValue(0);
    }
    for(%i = 1; %i<this.strArrNames.count();%i++){
      %output = %output @ "." @ %this.getValue(i);
    }
    return %output;
}

function splitString(%parse, %delim)
{
   %temp = new array();
   if(%parse $="") 
   {
      %temarr = new array();
	   %temarr.add(0,"");
	   return %temarr;
   }
   if(strpos(%parse,%delim) == -1)
   {
      %temarr = new array();
	   %temarr.add(0,%parse);
	   return %temarr;
   }
	if (%delim !$= ""){
		%posDelim = strpos(%parse, %delim);
		if (%posDelim == 0)
			return -1;
		%len = strlen(%parse);
		%temp.add(0,getSubStr (%parse, 0, %posDelim));
		%parse = getSubStr (%parse, %posDelim + 1, %len - %posDelim);
		%posDelim = strpos (%parse, %delim);
		%x = 1;
		while (%posDelim > 0) {
			%len = strlen(%parse);
			%temp.add(%x,getSubStr (%parse, 0, %posDelim));
			%parse = getSubStr (%parse, %posDelim + 1, %len - %posDelim);
			%posDelim = strpos (%parse, %delim);
			%x++;
		}
      %temp.add(%x,%parse);
		return %temp;
	} else {
		return new array();
	}
}

function DorminAddress::create(%this,%objNames,%bArray)
{
   if(%bArray == false)
      %this.strArrNames = splitString(%objNames,".");
   else
      %this.strArrNames = %objNames;
   return %this;
}

function DorminMessage::create(%this,%strAddress, %strVerb, %objValue, %type1,%type2,%type3)
{
   $numNextMessageNum = $numNextMessageNum + 1;
   %this.strNoteValueSetVerb = "NOTEVALUESET";
	// static String to use for a "FLAG" verb
	%this.strFlagVerb = "FLAG";
	// static String to use for a "APPROVE" verb
	%this.strApproveVerb = "APPROVE";
	// static String to use for a "HINTMESSAGE" verb
	%this.strHintMessageVerb = "HINTMESSAGE";
	// static String to use for a "JITMESSAGE" verb
	%this.strJITMessageVerb = "JITMESSAGE";
	// static String to use for a "GETHINT" verb
	%this.strGetHintVerb = "GETHINT";
	// static String to use for a "ENUMAVAILABLENODES" verb (Clearsighted-specific)
	%this.strEnumAvailableNodesVerb = "ENUMAVAILABLENODES";
	// static String to use for a "NOTEAVAILABLENODES" verb (Clearsighted-specific)
	%this.strNoteAvailableNodesVerb = "NOTEAVAILABLENODES";
	// static String to use for a "STOPTRE" verb (Clearsighted-specific)
	%this.strStopTRE = "STOPTRE";
	// static String to use for a "NOTEINITIALVALUE" verb (Clearsighted-specific)
	%this.strNoteInitialValue = "NOTEINITIALVALUE";

	%this.strSkillVerb = "SkillVerb";
	
	%this.numMessageNum = $numNextMessageNum;
	
	if(%type2 !$= "Undefined")
	{
		%this.strVerb = %strVerb;
	}
	else
	{
		%this.strVerb = "";
	}	
	
	if(%type1 !$= "Undefined")
	{
		%this.DorminAddr = new ScriptObject(DorminAddress).create(%strAddress,false);
	}
	else
	{
	   %temarr = new array();
	   %temarr.add(0,"");
      %this.DorminAddr = new ScriptObject(DorminAddress).create(%temarr,true);
	}
	
	%this.arrParameters = new array();
	
	if(%type3 !$= "Undefined")
	{
		if(objValue !$= "")
		{
			if(%type3 $= "String")
			{
				%this.arrParameters.add(0,new ScriptObject(DorminParameter).create("VALUE",new ScriptObject(TypeHolder).create("S", escapeString(%objValue))));
			}
			else if(%type3 $= "Object")
			{
				if(%objValue.getClassName() $= "Array")
				{
					%this.arrParameters.add(0,new ScriptObject(DorminParameter).create("VALUE",new ScriptObject(TypeHolder).create("L", %objValue)));
				}
				else if(%objValue.getClassName() $= "DorminAddress")
				{
					%this.arrParameters.add(0,new ScriptObject(DorminParameter).create("VALUE",new ScriptObject(TypeHolder).create("O", %objValue)));
				}
				else
				{
					%this.arrParameters.add(0,new ScriptObject(DorminParameter).create("VALUE",new ScriptObject(TypeHolder).create("X", %objValue)));
				}
			}
			else if(%type3 $= "Boolean")
			{
				%this.arrParameters.add(0,new ScriptObject(DorminParameter).create("VALUE",new ScriptObject(TypeHolder).create("B", %objValue)));
			}
			else if(%type3 $= "Number")
			{
				if(mFloor(%objValue) == %objValue)
				{
					%this.arrParameters.add(0,new ScriptObject(DorminParameter).create("VALUE",new ScriptObject(TypeHolder).create("I", %objValue)));	
				}
				else
				{
					%this.arrParameters.add(0,new ScriptObject(DorminParameter).create("VALUE",new ScriptObject(TypeHolder).create("D", %objValue)));	
				}
			}
			else
			{
				%this.arrParameters.add(0,new ScriptObject(DorminParameter).create("VALUE",new ScriptObject(TypeHolder).create("N", %objValue)));	
			}
		}
	}	
	return %this;
}

function AppendDorminPrimitive(%objValue)
{
	if(%objValue.typeChar $= "S")
		return AppendString(%objValue.value);
	else if(%objValue.typeChar $= "I")
		return AppendInteger(%objValue.value);
	else if(%objValue.typeChar $= "D")
		return AppendDecimal(%objValue.value);
	else if(%objValue.typeChar $= "B")
	{
		if(%objValue.value == 0 || %objValue.value $= "" || %objValue.value == false)
			return AppendBoolean(false);
		else
			return AppendBoolean(true);
	}
	else if(%objValue.typeChar $= "N")
		return AppendNull();
	else if(%objValue.typeChar $= "L")
		return AppendList(%objValue.value);
	else if(%objValue.typeChar $= "O")
		return AppendAddress(%objValue.value);
	else if(%objValue.typeChar $= "R")
		return AppendRange(%objValue.value);
	else if(%objValue.typeChar $= "X")
		return AppendMapping(%objValue.value);
	else
		return "";
}

function AppendDecimal(%number){
	return "D:" @ strlen(%number) @ ":" @ %number;
}
function AppendBoolean(%bool){
	return "B:" @ strlen(%bool) @ ":" @ %bool;
}
function AppendNull(){
	return "N:0:";
}
function AppendList(%list){
	%str = "L:" @ %list.count() @ ":[";
	for(%i = 0; %i<%list.count();%i++){
		%str = %str @ AppendDorminPrimitive(%list.getValue(%i));
		if(%i<%list.count()-1){
			%str = %str @ ",";
		}
	}
	%str = %str @ "]";
	return %str;
}
function AppendRange(%range){
	//ToDo: not implemented
}
function AppendMapping(%map){
	%str = "X:" @ %map.count() @ ":[";
	for(%i = 0; %i<%map.count();%i++){
		%str = %str @ "[" @ AppendDorminPrimitive(%i) @ "," @ AppendDorminPrimitive(%map.getValue(i)) @ "],";
	}
	%str = getSubStr(%str,0,strlen(%str)-1);//remove extra trailing comma
	%str = %str @ "]";
}

function AppendString(%str){
	return "S:" @ strlen(%str) @ ":" @ %str;
}

function AppendInteger(%number){
	return "I:" @ strlen(%number) @ ":" @ %number;
}

function AppendAddress(%DorminAddr){
	%rtn = "OBJECT=O:" @ %DorminAddr.strArrNames.count() @ ":";
	for(%i = 0; %i<%DorminAddr.strArrNames.count();%i++){
		%rtn = %rtn @ AppendString("OBJECT") @ "," @ AppendString("NAME") @ "," @ AppendString(%DorminAddr.strArrNames.getValue(%i));
		if(%i<%DorminAddr.strArrNames.count()-1){
			%rtn = %rtn @ ",";
		}
	}
	return %rtn;
}

function AppendParameter(%parameter){
	return %parameter.strName @ "=" @ AppendDorminPrimitive(%parameter.objValue) @ "&";
}

function DorminMessage::MakeString(%this)
{
   %output = "";
   %output = %output @ "SE/1.2&VERB=";
   %output= %output @ AppendString(%this.strVerb);
   %output= %output @ "&MESSAGENUMBER=";
   %output= %output @ AppendInteger(%this.numMessageNum);
   %output= %output @ "&";
   %output= %output @ AppendAddress(%this.DorminAddr);
   %output= %output @ "&";
   if(%this.arrParameters.count()>=1){
         for(%i = 0; %i<%this.arrParameters.count(); %i++){
               %output = %output @ AppendParameter(%this.arrParameters.getValue(%i));
         }
   }
   return %output;
}

function DorminStringReader::create(%this,%strDorminString){
	%this.numLocation = 0;
	%this.strDorminString = %strDorminString;
	return %this;
}

function DorminMsgFromString(%strDorminString){
	%message = new ScriptObject(DorminMessage).create("","","","Undefined","Undefined","Undefined");
	%DSR = new ScriptObject(DorminStringReader).create(%strDorminString);
   %DSR.numLocation = 7;
	%message.strVerb =  strReadVerb(%DSR);
	%message.numMessageNum = numReadMsgNum(%DSR);
	%message.DorminAddr = strReadAddr(%DSR);
	%message.arrParameters = arrReadParams(%DSR);
	return %message;
}

function strReadVerb(%reader){
	%reader.numLocation = %reader.numLocation + 5;
	%verb = strReadString(%reader);
	%reader.numLocation++; // Move an extra position for skipping &
	return %verb;
}


function strReadString(%reader){
	%reader.numLocation = %reader.numLocation + 2;
	%num = mFloor(getSubStr(%reader.strDorminString,%reader.numLocation,strpos(%reader.strDorminString,":",%reader.numLocation)));
	%reader.numLocation = strpos(%reader.strDorminString,":",%reader.numLocation)+1;
	%str = getSubStr(%reader.strDorminString,%reader.numLocation,%num);
	%reader.numLocation = %reader.numLocation + %num; 
	return %str;
}

function numReadMsgNum(%reader){
	%reader.numLocation = %reader.numLocation + 14;
	%msgnum = numReadInteger(%reader);
	%reader.numLocation++;
	return %msgnum;
}

function numReadInteger(%reader){
	%reader.numLocation = %reader.numLocation + 2;
	%num = mFloor(getSubStr(%reader.strDorminString,%reader.numLocation,strpos(%reader.strDorminString,":",%reader.numLocation)));
	%reader.numLocation = strpos(%reader.strDorminString,":",%reader.numLocation)+1;
	%integer = mFloor(getSubStr(%reader.strDorminString,%reader.numLocation,%num));
	%reader.numLocation = %reader.numLocation + %num; 
	return %integer;
}

function strReadAddr(%reader){
	%i = 0;
	%reader.numLocation = %reader.numLocation + 9;
	%numargs = mFloor(getSubStr(%reader.strDorminString,%reader.numLocation,strpos(%reader.strDorminString,":",%reader.numLocation)));
	%reader.numLocation = strpos(%reader.strDorminString,":",%reader.numLocation)+1;
	%arrIdents = new array();
	for(%i = 0; %i<%numargs;%i++){
		%arrIdents.add(%arrIdents.count(),numReadIdentifier(%reader));
	}
	return new ScriptObject(DorminAddress).create(%arrIdents, true);
}

function numReadIdentifier(%reader){
	%reader.numLocation = %reader.numLocation + 20; 
	%tmpStr = strReadString(%reader);
	%reader.numLocation++;
	return %tmpStr;
}

function arrReadParams(%reader){
   %params = new array();
   while(%reader.numLocation<strlen(%reader.strDorminString)-1 && charAt(%reader.strDorminString,%reader.numLocation)!$="\n"){
		%params.add(%params.count(),DPReadParameter(%reader));
	}
	return %params;
}

function DPReadParameter(%reader){
	%num = strpos(%reader.strDorminString,"=",%reader.numLocation);
	%name = getSubStr(%reader.strDorminString,%reader.numLocation,%num);
	%reader.numLocation=%num+1;
	%value = objReadPrimitive(%reader);
   %reader.numLocation++;
	return new ScriptObject(DorminParameter).create(%name, %value);
}

function charAt(%string, %int)
{
     return getSubStr(%string, %int, 1);
}

function objReadPrimitive(%reader){
   if(charAt(%reader.strDorminString,%reader.numLocation) $= "S")
      return new ScriptObject(TypeHolder).create("S", strReadString(%reader));
   else if(charAt(%reader.strDorminString,%reader.numLocation) $= "I")
      return new ScriptObject(TypeHolder).create("I", numReadInteger(%reader));
   else if(charAt(%reader.strDorminString,%reader.numLocation) $= "F")
      echo("unused");
   else if(charAt(%reader.strDorminString,%reader.numLocation) $= "D")
      return new ScriptObject(TypeHolder).create("D", numReadReal(%reader));
   else if(charAt(%reader.strDorminString,%reader.numLocation) $= "B")
      return new ScriptObject(TypeHolder).create("B", bReadBool(%reader));
   else if(charAt(%reader.strDorminString,%reader.numLocation) $= "N")
      return new ScriptObject(TypeHolder).create("N", nullReadNull(%reader));
   else if(charAt(%reader.strDorminString,%reader.numLocation) $= "L")
      return new ScriptObject(TypeHolder).create("L", listReadList(%reader));
   else if(charAt(%reader.strDorminString,%reader.numLocation) $= "O")
      return new ScriptObject(TypeHolder).create("O", strReadAddr(%reader));
   else if(charAt(%reader.strDorminString,%reader.numLocation) $= "R")
      return new ScriptObject(TypeHolder).create("R", rangeReadRange(%reader));
   else if(charAt(%reader.strDorminString,%reader.numLocation) $= "X")
      return new ScriptObject(TypeHolder).create("X", mapReadMapping(%reader));
   else
      return "";
}

function numReadReal(%reader){
	%reader.numLocation+=2;
	%num = mFloor(getSubStr(%reader.strDorminString,%reader.numLocation,strpos(%reader.strDorminString,":",%reader.numLocation)));
	%reader.numLocation = strpos(%reader.strDorminString,":",%reader.numLocation)+1;
	%real = mFloor(getSubStr(%reader.strDorminString,%reader.numLocation,%num));
	%reader.numLocation+=%num;
	return %real;
}

function bReadBool(%reader){
	%reader.numLocation+=2;
	%place = strpos(%reader.strDorminString,":",%reader.numLocation);
	%num = mFloor(getSubStr(%reader.strDorminString,%reader.numLocation,%place));
	%reader.numLocation = %place+1;
	%number = mFloor(parseInt(%reader.strDorminString.substr(reader.numLocation, num)));
	%reader.numLocation+=%num;
	if(%number==1)
	   return true;
   else
      return false;
}

function nullReadNull(%reader){
	%reader.numLocation+=4;
	return -1;
}

function listReadList(%reader){
	%reader.numLocation+=2;
	%place = strpos(%reader.strDorminString,":",%reader.numLocation);
	%num = mFloor(getSubStr(%reader.strDorminString,%reader.numLocation,%place));
	%reader.numLocation = %place+1+1;
	%list = new array();
   for(%i = 0; %i < %num; %i++)
	{
		%list.add(%list.count(),objReadPrimitive(%reader));
		if (%i < %num - 1)
		   %reader.numLocation++;//go past the ","
	}
	%reader.numLocation++;//go past the "]"
	return %list;
}

function rangeReadRange(%reader){
	//don't use this
}

function mapReadMapping(%reader){
	%reader.numLocation+=2;
	%place = strpos(%reader.strDorminString,":",%reader.numLocation);
	%num = mFloor(getSubStr(%reader.strDorminString,%reader.numLocation,%place));
	%reader.numLocation++;
	%dict = new array();
	for(%i = 0; %i < %num; %i++){
		%reader.numLocation++;
		%key = strReadString(%reader);
		%reader.numLocation++;
		%val = objReadPrimitive(%reader);
		%dict.add(%dict.count(),%key);
		%dict.add(%dict.count(),%val);
		%reader.numLocation++;
		if (%i < %num - 1)
		   %reader.numLocation++;
	}
		%reader.numLocation++;
	return %dict;
}