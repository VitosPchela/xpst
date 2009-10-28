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
   if(%this.index>0){
	   %output = %this.strArrNames[0];
   }
   for(%i = 1; %i<%this.index;%i++){
	   if(strpos(%this.strArrNames[i],";")==-1)
	   {
   		%output = %output @ "." @ %this.strArrNames[i];
	   }
	   else
	   {
   		%temp = strreplace(%this.strArrNames[i],";","[") @ "]";
	   }
   }
   return %output;
}

function DorminAddress::ToDottedString(%this)
{
    %output = "";
    if(%this.index>0){
      %output = %this.strArrNames[0];
    }
    for(%i = 1; %i<this.index;%i++){
      %output = %output @ "." @ %this.strArrNames[i];
    }
    return %output;
}

function DorminAddress::create(%this,%objNames)
{
   //code for string split function. %objNames is always assumed to be a string
   //use the count attribute to form the array later.
   %index = 0;
   %splitString = ".";
   %prev = 0;

   for(%a =0;%a <strlen(%objNames);%a++)
   {
      if(getSubStr(%objNames, %a, 1) $= %splitString)
      {
         %this.strArrNames[%index] = getSubStr(%objNames,%prev,(%a-%prev+1));
         %index = %index +1;
         %prev = %a + 1;
      }
   %this.strArrNames[%index] = getSubStr(%objNames,%prev,1000);
   }
   %this.count = %index;
   %this.ConvertToBrackets = DorminAddress::ConvertToBrackets();
   %this.ToDottedString = DorminAddress::ToDottedString();
   
   return %this;
}

function DorminMessage::create(%this,%strAddress, %strVerb, %objValue)
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
	%this.strVerb = %strVerb;
	%this.numMessageNum = $numNextMessageNum;
	
	%this.DorminAddr = new ScriptObject(DorminAddress).create(%strAddress);
	//here typechecking goes for various types of objvalues.
	//For hint functionality we just implement string type for now.
	%this.arrParameters = new ScriptObject(DorminParameter).create("VALUE",new ScriptObject(TypeHolder).create('S', escapeString(%objValue)));
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
		if(%objValue.value == 0)
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
	//ToDo: not implemented
}
function AppendRange(%range){
	//ToDo: not implemented
}
function AppendMapping(%map){
	//ToDo: not implemented
}

function AppendString(%str){
	return "S:" @ strlen(%str) @ ":" @ %str;
}

function AppendInteger(%number){
	return "I:" @ strlen(%number) @ ":" @ %number;
}

function AppendAddress(%DorminAddr){
	%rtn = "OBJECT=O:" @ %DorminAddr.index @ ":";
	for(%i = 0; %i<%DorminAddr.index;%i++){
		%rtn = %rtn @ AppendString("OBJECT") @ "," @ AppendString("NAME") @ "," @ AppendString(%DorminAddr.strArrNames[%i]);
		if(%i<%DorminAddr.index-1){
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
   if(isObject(%this.arrParameters)){
      %output = %output @ AppendParameter(%this.arrParameters.strValueName);
      %output = %output @ AppendParameter(%this.arrParameters.strName);
      %output = %output @ AppendParameter(%this.arrParameters.objValue);
   }
   return output;
}

function DorminStringReader::create(%this,%strDorminString){
	%this.numLocation = 0;
	%this.strDorminString = %strDorminString;
}

function DorminMsgFromString(%strDorminString){
	%message = new ScriptObject(DorminMessage).create("","","");
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
	return new ScriptObject(DorminAddress).create(numReadIdentifier(%reader));
}

function numReadIdentifier(%reader){
	%reader.numLocation = %reader.numLocation + 20; 
	%tmpStr = strReadString(%reader);
	%reader.numLocation++;
	return %tmpStr;
}

function arrReadParams(%reader){
	if(%reader.numLocation<strlen(%reader.strDorminString)-1 && charAt(%reader.strDorminString,%reader.numLocation)!$="\n"){
		%params = DPReadParameter(%reader);
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
      return -1;
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
	%reader.numLocation = %place+1;
	%list = objReadPrimitive(%reader);
	%reader.numLocation++;
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
	//even elements are keys and odd elements are values
	//var dict = [];
	//for (i = 0; i < num; i++){
		//reader.numLocation++;
		//var key = strReadString(reader);
		//reader.numLocation++;
		//var val = objReadPrimitive(reader);
		//dict[dict.length] = key;
		//dict[dict.length] = val;
		//reader.numLocation++;
		//if (i < num - 1)reader.numLocation++;
	//}
		//reader.numLocation++;
	//return dict;
	
	//This function is incompletely implemented.
	//You need to devise a common way of how to pass arrays to functions.
	//Eeither pass them as encoded strings and do necessary processing 
	//at the function which uses them.
}