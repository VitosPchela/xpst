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

function DorminParameter(name, value){
	this.strValueName = "VALUE";
	this.strName = name;
	this.objValue = value;
}

// Describes a message target for a Dormin message. Basically a string of names
// running down a heirarchy.

// objNames can either be a string array or a string containing a dotted array
// if bArray is true then objNames is an array otherwise its a string
function DorminAddress(objNames, bArray){
	// a string array
	this.strArrNames = objNames;
	if(!bArray){
		this.strArrNames = objNames.split('.');
	}

    this.ConvertToBrackets = 
    function()
    {
        var output = "";
        var temp = "";
        if(this.strArrNames.length>0){
            output = this.strArrNames[0];
        }
        for(var i = 1; i<this.strArrNames.length;i++){
            if(this.strArrNames[i].indexOf(';')==-1)
            {
                output+="."+this.strArrNames[i];
            }
            else
            {
                //path = path.replace(/_/g,"\[");
                temp = this.strArrNames[i].replace(/;/g,"[")+"]";
            }
        }
        return output;
    }
	// Convert the address to a nice string representation, like foo.bar.baz
	// returns a Dotted string representation of address
  this.ToDottedString = 
  function (){
    var output = "";
    if(this.strArrNames.length>0){
      output = this.strArrNames[0];
    }
    for(var i = 1; i<this.strArrNames.length;i++){
      output+="."+this.strArrNames[i];
    }
    return output;
	}
}

// A list of items for a Dormin message.
function DorminList(objArrItems){
	this.objArrItems = objArrItems;
}

// Map, mainly used for Structured Student Input
function DorminMap(mapping){
	// this needs to be set in maps for Structured Student Input
	this.strDisplayString = "display-string";

	//replaces "Dictionary" in C#
	//use like return mapItems["key"];
	this.mapItems = mapping;
}

// TODO
function DorminRange(){
	// TODO
}

function escapeString(str)
{
	// need to escape newlines, which means we need to escape backslashes. This'll need to be unescaped on the Event Mapper end
	str = str.replace(/\\/g, '\\\\');
	str = str.replace(/\n/g, '\\n');
	return str;
}

// A Dormin message. These are used to communicate to/from the ITS.
//the parameters to this method are optional and will default to undefined
function DorminMessage(strAddress, strVerb, objValue){
	//public static String Verb = "";
	// TODO: or use an enum.
	// TODO: or use public const strings, so they can be used in a switch
	
	// static String to use for a "NOTEVALUESET" verb
	this.strNoteValueSetVerb = "NOTEVALUESET";
	// static String to use for a "FLAG" verb
	this.strFlagVerb = "FLAG";
	// static String to use for a "APPROVE" verb
	this.strApproveVerb = "APPROVE";
	// static String to use for a "HINTMESSAGE" verb
	this.strHintMessageVerb = "HINTMESSAGE";
	// static String to use for a "JITMESSAGE" verb
	this.strJITMessageVerb = "JITMESSAGE";
	// static String to use for a "GETHINT" verb
	this.strGetHintVerb = "GETHINT";
	// static String to use for a "ENUMAVAILABLENODES" verb (Clearsighted-specific)
	this.strEnumAvailableNodesVerb = "ENUMAVAILABLENODES";
	// static String to use for a "NOTEAVAILABLENODES" verb (Clearsighted-specific)
	this.strNoteAvailableNodesVerb = "NOTEAVAILABLENODES";
	// static String to use for a "STOPTRE" verb (Clearsighted-specific)
	this.strStopTRE = "STOPTRE";
	// static String to use for a "NOTEINITIALVALUE" verb (Clearsighted-specific)
	this.strNoteInitialValue = "NOTEINITIALVALUE";

	this.strSkillVerb = "SkillVerb";
	
	// The Dormin verb. These are defined semantically by the CLI TRE (potentially with
	// supplementary ones by Clearsighted.
	if(typeof strVerb !== "undefined"){
		this.strVerb = strVerb;
	}
	else{
		this.strVerb = "";
	}
	
	// A message sequence number that should usually be incremented for each message.
	// The constructor does this automatically, but it can be overwritten if necessary.
	DorminMessage.prototype.numNextMessageNum++;
	this.numMessageNum = DorminMessage.prototype.numNextMessageNum;
	
	// The address of the target of the message.
	//alert(typeof(strAddress));
	if(typeof(strAddress) !== "undefined"){//typeof strAddress !== "undefined"){
		//alert(strAddress);
		this.DorminAddr = new DorminAddress(strAddress, false);
	}
	else{
		this.DorminAddr = null;
	}
	// Parameters for the message, as defined semantically by the CLI TRE.
	this.arrParameters = [];
	if(typeof objValue !== "undefined"){
		if(objValue !== null){
			switch(typeof(objValue)){
				case 'string':
						 this.arrParameters = [new DorminParameter("VALUE", new TypeHolder('S', escapeString(objValue)))];
						 //alert("string");
						 break;
				case 'object':
					if(objValue.constructor == Array){
						//alert("array");
						this.arrParameters = [new DorminParameter("VALUE", new TypeHolder('L', objValue))];
					}else if(objValue.constructor == DorminAddress){
						//alert("address");
						this.arrParameters = [new DorminParameter("VALUE", new TypeHolder('O', objValue))];
					}else{//mapping
						//alert("mapping");
						this.arrParameters = [new DorminParameter("VALUE", new TypeHolder('X', objValue))];
					}
					break;
				case 'boolean':
					//alert("bool");
					this.arrParameters = [new DorminParameter("VALUE", new TypeHolder('B', objValue))];
					break;
				case 'number':
					if(isInteger(objValue)){
						//alert("int");
						 this.arrParameters = [new DorminParameter("VALUE", new TypeHolder('I', objValue))];
					}else{
						//alert("real");
						 this.arrParameters = [new DorminParameter("VALUE", new TypeHolder('D', objValue))];
					}
					break;
				case 'R':
					alert("not implemented");
					//not implemented
					break;
				case 'null':
					this.arrParameters = [new DorminParameter("VALUE", new TypeHolder('N', objValue))];
					//alert("null");
					break;
				default:
					this.arrParameters = [new DorminParameter("VALUE", new TypeHolder('N', objValue))];
					//alert("null");
			}
		}
	}
}

function isInteger(s){
    return Math.ceil(s) == Math.floor(s);
}

function typeOf(value) {
    var s = typeof value;
    if (s === 'object') {
        if (value) {
            if (typeof value.length === 'number' &&
                    !(value.propertyIsEnumerable('length')) &&
                    typeof value.splice === 'function') {
                s = 'array';
            }
        } else {
            s = 'null';
        }
    }
    return s;
}

DorminMessage.prototype = {
    numNextMessageNum: 0,
    
    MakeString : function ()
    {
        var i;
        var output = "";
        output+="SE/1.2&VERB=";
        output+=AppendString(this.strVerb);
        output+="&MESSAGENUMBER=";
        output+=AppendInteger(this.numMessageNum);
        output+="&";
        output+=AppendAddress(this.DorminAddr);
        output+="&";
        if(this.arrParameters!==null){
            for(i = 0; i<this.arrParameters.length; i++){
                output+=AppendParameter(this.arrParameters[i]);
            }
        }
        return output;
    },
    MakeParameterString : function ()
    {
        var i = 0;
        var output = "";
        if(this.arrParameters!==null){
            for(i = 0; i<this.arrParameters.length; i++)
            {
                if(this.arrParameters[i].strName == "MESSAGE")
                {
                    //output+=i+this.arrParameters[i].strName+" - "+this.arrParameters[i].objValue.typeChar+" - "+AppendDorminPrimitive(this.arrParameters[i].objValue)+"-\n";
                    output+=this.arrParameters[i].objValue.value[0].value+"-\n";
                }
            }
            return output;
        }
        else
        {
            return "No parameters";
        }
    }
}

function TypeHolder(typeChar, value){
	this.typeChar = typeChar;
	this.value = value;
}

function DorminStringReader(strDorminString){
	//this.numLocation = 0;
	this.numLocation = 0;
	this.strDorminString = strDorminString;
}

//returns a DorminMessage object from a string representation of the message
function DorminMsgFromString(strDorminString){
	var message = new DorminMessage();
	var DSR = new DorminStringReader(strDorminString);
	//preamble-should be discarded
	//voidReadPreamble(DSR);
	//dump("Before preamble: "+DSR.numLocation+"\n");
  DSR.numLocation = 7;
	//dormin-verb
		//dump("Before verb: "+DSR.numLocation+"\n");
	message.strVerb =  strReadVerb(DSR);
  //dump("verb: "+message.strVerb+"\n\n");
	/*if(message.strVerb == "HINTMESSAGE")
	{
		dump("===================================================================\n");
	}*/
	//dormin-message-number
	//dump("Before MessageNum: "+DSR.numLocation+"\n");
	message.numMessageNum = numReadMsgNum(DSR);
	//dormin-address
	//dump("Before Addr: "+DSR.numLocation+"\n");
	message.DorminAddr = strReadAddr(DSR);
	//dormin-parameter *
	//dump("Before ArrParameters: "+DSR.numLocation+"\n");
	message.arrParameters = arrReadParams(DSR);
	/*if(message.strVerb == "HINTMESSAGE")
	{
		dump("+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\n");
	}*/

	return message;
}

function voidReadPreamble(reader){
	reader.numLocation = 7;
}

function strReadVerb(reader){
	reader.numLocation+=5;
	var verb = strReadString(reader);
	reader.numLocation++; // Move an extra position for skipping &
	return verb;
}

function numReadMsgNum(reader){
	reader.numLocation += 14;
	var msgnum = numReadInteger(reader);
	reader.numLocation++;
	return msgnum;
}

function strReadAddr(reader){
	var i = 0;
	reader.numLocation += 9; // Skip "OBJECT=O:"
	var numargs = parseInt(reader.strDorminString.substring(reader.numLocation, reader.strDorminString.indexOf(':', reader.numLocation)));
	//dump("numArgs "+numargs+"\n");
	reader.numLocation = reader.strDorminString.indexOf(':', reader.numLocation)+1;
	var arrIdents = [];
	//dump("string length "+reader.strDorminString.length+"\n");
	//dump("numLocation: "+reader.numLocation+"\n");
	for(i = 0; i<numargs;i++){
		arrIdents[arrIdents.length] = numReadIdentifier(reader);
		//dump("Num args: "+numargs+" Rest of string after args "+i+1 +" "+reader.strDorminString.substr(reader.numLocation)+" numLocation: "+reader.numLocation+"\n");
    //dump("address arguments: "+arrIdents[arrIdents.length-1]+"\n\n");
	}
	return new DorminAddress(arrIdents, true);
}

function arrReadParams(reader){
  //dump("read parameters \n\n");
	var params = [];
	//dump("Array params read\n");
	//dump(reader.numLocation+"\n");
	//dump(reader.strDorminString.length+"\n");
	while(reader.numLocation<reader.strDorminString.length-1 && reader.strDorminString[reader.numLocation]!=='\n'){
    //dump(reader.strDorminString[reader.numLocation-1]+reader.strDorminString[reader.numLocation]+reader.strDorminString[reader.numLocation+1]);
    //dump("rest of string: "+reader.strDorminString.substr(reader.numLocation)+" numLocation: "+reader.numLocation+"\n\n");
		params[params.length] = DPReadParameter(reader);
	}
	return params;
}

function strReadString(reader){
	reader.numLocation+=2;
  //dump("reader.numLocation :"+reader.numLocation+"\n\n");
  //dump(reader.strDorminString.substring(reader.numLocation, reader.strDorminString.indexOf(':', reader.numLocation))+"\n\n");
	var num = parseInt(reader.strDorminString.substr(reader.numLocation, reader.strDorminString.indexOf(':', reader.numLocation)));
  //dump("string length :"+num+"\n\n");
	reader.numLocation = reader.strDorminString.indexOf(':', reader.numLocation)+1;
	var str = reader.strDorminString.substr(reader.numLocation, num);
	reader.numLocation+=num; 
	return str;
}

function numReadInteger(reader){
	reader.numLocation+=2;
	var num = parseInt(reader.strDorminString.substring(reader.numLocation, reader.strDorminString.indexOf(':', reader.numLocation)));
	reader.numLocation = reader.strDorminString.indexOf(':', reader.numLocation)+1;
	var integer = parseInt(reader.strDorminString.substr(reader.numLocation, num));
	reader.numLocation+=num;
	return integer;
}

function numReadIdentifier(reader){
	reader.numLocation+=20; // Read through "S:6:OBJECT,S:4:NAME,"
	var tmpStr = strReadString(reader);
  //dump("identifier string :" +tmpStr+"\n\n");
	reader.numLocation++;// Move an extra position for skipping &
	return tmpStr;
}

function DPReadParameter(reader){
	//dump("Read parameters\n");
	var num = reader.strDorminString.indexOf('=', reader.numLocation);
	//dump("Num" +num+"\n");
	var name = reader.strDorminString.substring(reader.numLocation, num);
  //dump("read parameter name : " + name +"  numLocation : "+reader.numLocation+"\n\n");
	//dump("Name " +name+"\n");
	reader.numLocation=num+1;	//skips '=' after name
	var value = objReadPrimitive(reader);
  //dump("location: "+reader.numLocation+" string length: "+reader.strDorminString.length+"\n\n");
  reader.numLocation++;  //reads the '&' after the param
	return new DorminParameter(name, value);
}

function objReadPrimitive(reader){
	switch (reader.strDorminString[reader.numLocation])
	{
		case 'S':
			return new TypeHolder('S', strReadString(reader));
		case 'I':
			return new TypeHolder('I', numReadInteger(reader));
		case 'F':
            dump("unused");
		case 'D':
			return new TypeHolder('D', numReadReal(reader));
		case 'B':
			return new TypeHolder('B', bReadBool(reader));
		case 'N':
			return new TypeHolder('N', nullReadNull(reader));
		case 'L':
			return new TypeHolder('L', listReadList(reader));
		case 'O':
			return new TypeHolder('O', strReadAddr(reader));
		case 'R':
			return new TypeHolder('R', rangeReadRange(reader));
		case 'X':
			return new TypeHolder('X', mapReadMapping(reader));
		default:
			return null;
	}
}

function numReadReal(reader){
	reader.numLocation+=2;
	var num = parseInt(reader.strDorminString.substring(reader.numLocation, reader.strDorminString.indexOf(':', reader.numLocation)));
	reader.numLocation = reader.strDorminString.indexOf(':', reader.numLocation)+1;
	var real = parseInt(reader.strDorminString.substr(reader.numLocation, num));
	reader.numLocation+=num;
	return real;
}

function bReadBool(reader){
	reader.numLocation+=2;
	var place = reader.strDorminString.indexOf(':', reader.numLocation);
	var num = parseInt(reader.strDorminString.substring(reader.numLocation, place));
	reader.numLocation = place+1;
	var number = parseInt(reader.strDorminString.substr(reader.numLocation, num));
	reader.numLocation+=num;
	return number===1;
}

function nullReadNull(reader){
	reader.numLocation+=4;
	return null;
}

function listReadList(reader){
  var i;
	reader.numLocation+=2;//go past the "L:"
	//location of the next delineator
	var place = reader.strDorminString.indexOf(':', reader.numLocation);
	//retrieve and convert the string to an int
	var num = parseInt(reader.strDorminString.substring(reader.numLocation, place));
	reader.numLocation = place+1+1;//go past the number and the ":["
	var list = [];
	for (i = 0; i < num; i++)
	{
    //dump(i);
		list[list.length] = objReadPrimitive(reader);
		if (i < num - 1)reader.numLocation++;//go past the ","
	}
  //dump("\n\n");
	reader.numLocation++;//go past the "]"
	return list;
}

function rangeReadRange(reader){
	//don't use this
}

function mapReadMapping(reader){
	reader.numLocation+=2;
	//location of the next delineator
	var place = reader.strDorminString.indexOf(':', reader.numLocation);
	//retrieve and convert the string to an int
	var num = parseInt(reader.strDorminString.substr(reader.numLocation, place));
	reader.numLocation++;
	//even elements are keys and odd elements are values
	var dict = [];
	for (i = 0; i < num; i++){
		reader.numLocation++;
		var key = strReadString(reader);
		reader.numLocation++;
		var val = objReadPrimitive(reader);
		dict[dict.length] = key;
		dict[dict.length] = val;
		reader.numLocation++;
		if (i < num - 1)reader.numLocation++;
	}
		reader.numLocation++;
	return dict;
}
function AppendString(str){
	return "S:"+str.length+":"+str;
}
function AppendInteger(number){
	return "I:"+number.toString().length+":"+number.toString();
}
function AppendAddress(DorminAddr){
  var i;
	var rtn = "OBJECT=O:"+DorminAddr.strArrNames.length+":";
	for(i = 0; i<DorminAddr.strArrNames.length;i++){
		rtn+=AppendString("OBJECT")+","+AppendString("NAME")+","+AppendString(DorminAddr.strArrNames[i]);
		if(i<DorminAddr.strArrNames.length-1){
			rtn+=",";
		}
	}
	return rtn;
}
function AppendParameter(parameter){
	//alert(parameter.objValue);
	//alert("appendparameter");
	return parameter.strName+'='+AppendDorminPrimitive(parameter.objValue)+"&";
}
function AppendDorminPrimitive(objValue){
	//alert(objValue.constructor);
	switch(objValue.typeChar){
		case 'S':
			return AppendString(objValue.value);
		case 'I':
			return AppendInteger(Math.round(objValue.value));
		case 'D':
			return AppendDecimal(objValue.value);
		case 'B':
			return AppendBoolean(new Boolean(objValue.value));
		case 'N':
			return AppendNull();
		case 'L':
			return AppendList(objValue.value);
		case 'O':
			return AppendAddress(objValue.value);
		case 'R':
			return AppendRange(objValue.value);
		case 'X':
			return AppendMapping(objValue.value);
		default:
			return "";
	}
}
function AppendDecimal(number){
	return "D:"+number.toString().length+":"+number.toString();
}
function AppendBoolean(bool){
	return "B:"+bool.toString().length+":"+bool.toString();
}
function AppendNull(){
	return "N:0:";
}
function AppendList(list){
	var str = "L:"+list.length+":[";
	for(i = 0; i<list.length;i++){
		//alert("appendlist");
		str+=AppendDorminPrimitive(list[i]);
		if(i<list.length-1){
			str+=",";
		}
	}
	str+="]";
	return str;
}
function AppendRange(range){
	//ToDo: not implemented
}
function AppendMapping(map){
	var str = "X:"+map.length+":[";
	for(i in map){
		//alert("appendprimitive");
		str+="["+AppendDorminPrimitive(i)+","+AppendDorminPrimitive(map[i])+"],";
	}
	str = str.slice(0, str.length-1);//remove extra trailing comma
	str+="]";
}
