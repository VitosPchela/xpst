$g_gameTREURL = "";
$g_taskName = "";
$g_isTutorRunning = false;
$mytre = "";
$g_blockJITs = false;
$hintNumber = 0;
$hintMsg = "";
$hintLocation = "";
$hintCount = 0;

function startTutor(%id,%mission)
{
   
   doKeyMaps();
      
   $g_gameTREURL = "http://xpst.vrac.iastate.edu/WebxPST/WebxPST";
   //$g_gameTREURL = "http://localhost:8080/WebxPSTServer/WebxPST";
   $g_taskName = getMissionDisplayName(%mission);
   
   //executing task cs file
   exec("~/tasks/" @ $g_taskName @ ".cs");
   
   // sending message to xPST engine at the start
    %data = "command=starttre&task=" @ $g_taskName;
	
   new TCPObject(httpPage) { };
   httpPage.post($g_gameTREURL,%data,0);
}

function doKeyMaps()
{
   moveMap.bindCmd( keyboard, "a", "keyPress("@"a"@");", "" );
   moveMap.bindCmd( keyboard, "b", "keyPress("@"b"@");", "" );
   moveMap.bindCmd( keyboard, "c", "keyPress("@"c"@");", "" );
   moveMap.bindCmd( keyboard, "d", "keyPress("@"d"@");", "" );
   moveMap.bindCmd( keyboard, "e", "keyPress("@"e"@");", "" );
   moveMap.bindCmd( keyboard, "f", "keyPress("@"f"@");", "" );
   moveMap.bindCmd( keyboard, "g", "keyPress("@"g"@");", "" );
   moveMap.bindCmd( keyboard, "h", "keyPress("@"h"@");", "" );
   moveMap.bindCmd( keyboard, "i", "keyPress("@"i"@");", "" );
   moveMap.bindCmd( keyboard, "j", "keyPress("@"j"@");", "" );
   moveMap.bindCmd( keyboard, "k", "keyPress("@"k"@");", "" );
   moveMap.bindCmd( keyboard, "l", "keyPress("@"l"@");", "" );
   moveMap.bindCmd( keyboard, "m", "keyPress("@"m"@");", "" );
   moveMap.bindCmd( keyboard, "n", "keyPress("@"n"@");", "" );
   moveMap.bindCmd( keyboard, "o", "keyPress("@"o"@");", "" );
   moveMap.bindCmd( keyboard, "p", "keyPress("@"p"@");", "" );
   moveMap.bindCmd( keyboard, "q", "keyPress("@"q"@");", "" );
   moveMap.bindCmd( keyboard, "r", "keyPress("@"r"@");", "" );
   moveMap.bindCmd( keyboard, "s", "keyPress("@"s"@");", "" );
   moveMap.bindCmd( keyboard, "t", "keyPress("@"t"@");", "" );
   moveMap.bindCmd( keyboard, "u", "keyPress("@"u"@");", "" );
   moveMap.bindCmd( keyboard, "v", "keyPress("@"v"@");", "" );
   moveMap.bindCmd( keyboard, "w", "keyPress("@"w"@");", "" );
   moveMap.bindCmd( keyboard, "x", "keyPress("@"x"@");", "" );
   moveMap.bindCmd( keyboard, "y", "keyPress("@"y"@");", "" );
   moveMap.bindCmd( keyboard, "z", "keyPress("@"z"@");", "" );
}

function sendMessage(%msg)
{
   if ($g_isTutorRunning == false)
		return false;
   new TCPObject(httpPage) { };
   httpPage.post($g_gameTREURL @ "/" @ $mytre,%msg,1);
}

function onHint()
{
   %msg1 = new ScriptObject(DorminMessage).create("", "GETHINT", "answer","Undefined","String","String");
   sendMessage(%msg1.MakeString());
   $g_blockJITs = false;
}

function onDone()
{
   %msg1 = new ScriptObject(DorminMessage).create("TutorLink.Done", "NOTEVALUESET", "1","","String","String");
	sendMessage(strreplace(%msg1.MakeString(),"done","Done"));
}

//edit the keyPress function for altering key mappings as required.

function keyPress(%char)
{
   switch$(%char)
   {
      case "a":
         %msg1 = new ScriptObject(DorminMessage).create("startcommunicate", "NOTEVALUESET", %char,"","String","String");
	      sendMessage(%msg1.MakeString());
      case "b":
         %msg1 = new ScriptObject(DorminMessage).create("startcommunicate", "NOTEVALUESET", %char,"","String","String");
	      sendMessage(%msg1.MakeString());
      case "c":
         onCommunicate();
      case "d":
         onDone();
      case "e":
         %msg1 = new ScriptObject(DorminMessage).create("startcommunicate", "NOTEVALUESET", %char,"","String","String");
	      sendMessage(%msg1.MakeString());
      case "f":
         %msg1 = new ScriptObject(DorminMessage).create("startcommunicate", "NOTEVALUESET", %char,"","String","String");
	      sendMessage(%msg1.MakeString());
      case "g":
         %msg1 = new ScriptObject(DorminMessage).create("startcommunicate", "NOTEVALUESET", %char,"","String","String");
	      sendMessage(%msg1.MakeString());
      case "h":
         onHint();
      case "i":
         %msg1 = new ScriptObject(DorminMessage).create("startcommunicate", "NOTEVALUESET", %char,"","String","String");
	      sendMessage(%msg1.MakeString());
      case "j":
         %msg1 = new ScriptObject(DorminMessage).create("startcommunicate", "NOTEVALUESET", %char,"","String","String");
	      sendMessage(%msg1.MakeString());
      case "k":
         %msg1 = new ScriptObject(DorminMessage).create("startcommunicate", "NOTEVALUESET", %char,"","String","String");
	      sendMessage(%msg1.MakeString());
      case "l":
         %msg1 = new ScriptObject(DorminMessage).create("startcommunicate", "NOTEVALUESET", %char,"","String","String");
	      sendMessage(%msg1.MakeString());
      case "m":
         %msg1 = new ScriptObject(DorminMessage).create("startcommunicate", "NOTEVALUESET", %char,"","String","String");
	      sendMessage(%msg1.MakeString());
      case "n":
         %msg1 = new ScriptObject(DorminMessage).create("startcommunicate", "NOTEVALUESET", %char,"","String","String");
	      sendMessage(%msg1.MakeString());
      case "o":
         %msg1 = new ScriptObject(DorminMessage).create("startcommunicate", "NOTEVALUESET", %char,"","String","String");
	      sendMessage(%msg1.MakeString());
      case "p":
         %msg1 = new ScriptObject(DorminMessage).create("startcommunicate", "NOTEVALUESET", %char,"","String","String");
	      sendMessage(%msg1.MakeString());
      case "q":
         %msg1 = new ScriptObject(DorminMessage).create("startcommunicate", "NOTEVALUESET", %char,"","String","String");
	      sendMessage(%msg1.MakeString());
      case "r":
         %msg1 = new ScriptObject(DorminMessage).create("startcommunicate", "NOTEVALUESET", %char,"","String","String");
	      sendMessage(%msg1.MakeString());
      case "s":
         %msg1 = new ScriptObject(DorminMessage).create("startcommunicate", "NOTEVALUESET", %char,"","String","String");
	      sendMessage(%msg1.MakeString());
      case "t":
         %msg1 = new ScriptObject(DorminMessage).create("startcommunicate", "NOTEVALUESET", %char,"","String","String");
	      sendMessage(%msg1.MakeString());
      case "u":
         %msg1 = new ScriptObject(DorminMessage).create("startcommunicate", "NOTEVALUESET", %char,"","String","String");
	      sendMessage(%msg1.MakeString());
      case "v":
         %msg1 = new ScriptObject(DorminMessage).create("startcommunicate", "NOTEVALUESET", %char,"","String","String");
	      sendMessage(%msg1.MakeString());
      case "w":
         %msg1 = new ScriptObject(DorminMessage).create("startcommunicate", "NOTEVALUESET", %char,"","String","String");
	      sendMessage(%msg1.MakeString());
      case "x":
         %msg1 = new ScriptObject(DorminMessage).create("startcommunicate", "NOTEVALUESET", %char,"","String","String");
	      sendMessage(%msg1.MakeString());      
      case "y":
         %msg1 = new ScriptObject(DorminMessage).create("startcommunicate", "NOTEVALUESET", %char,"","String","String");
	      sendMessage(%msg1.MakeString());
      case "z":
         %msg1 = new ScriptObject(DorminMessage).create("startcommunicate", "NOTEVALUESET", %char,"","String","String");
	      sendMessage(%msg1.MakeString());
   }
   
}


function onEnterBuilding(%trigger,%obj)
{
   if(!%trigger.doneOnce)
   {
      if($g_taskName $= "Evacuate")
      {
         %entname = %trigger.getName() @ ":Enter";
         %msg1 = new ScriptObject(DorminMessage).create(%entname, "NOTEVALUESET", "1","","String","String");
         sendMessage(%msg1.MakeString());
         %temp = getSubStr(%trigger.getName(),0,strpos(%trigger.getName(),"Trigger"));
         //MessageBoxOK("Message","You have entered " @ %temp);
         %trigger.doneOnce = true;
      }
      else if($g_taskName $= "TargetAcquisition")
      {
         %entname = %trigger.getName() @ ":EnterProximity";
         %msg1 = new ScriptObject(DorminMessage).create(%entname, "NOTEVALUESET", "1","","String","String");
         sendMessage(%msg1.MakeString());
         MessageBoxOK("Message","You have entered Target's proximity region");
         %trigger.doneOnce = true;
      }
   }
}

function onCommunicate()
{
   Canvas.pushDialog("communicateMenu");
   %msg1 = new ScriptObject(DorminMessage).create("startcommunicate", "NOTEVALUESET", "c","","String","String");
	sendMessage(%msg1.MakeString());
}
