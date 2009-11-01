$g_gameTREURL = "";
$g_taskName = "";
$g_isTutorRunning = false;
$mytre = "";
$g_blockJITs = false;
$hintNumber = 0;
$hintMsg = "";
$hintLocation = "";

function startTutor(%id,%mission)
{
   // you can start event catching here
   
   $g_gameTREURL = "http://localhost:8080/WebxPSTServer/WebxPST";
   $g_taskName = getMissionDisplayName(%mission);
   
   // sending message to xPST engine at the start
   
   %data = "command=starttre&task=" @ $g_taskName;
   new TCPObject(httpPage) { };
   httpPage.post($g_gameTREURL,%data,0);
}

function sendMessage(%msg)
{
   if ($g_isTutorRunning == false)
		return false;
   echo("###Sending message:" @ %msg);
   new TCPObject(httpPage) { };
   httpPage.post($g_gameTREURL @ "/" @ $mytre,%msg,1);
}

function onHint()
{
   %msg1 = new ScriptObject(DorminMessage).create("", "GETHINT", "answer","Undefined","String","String");
   sendMessage(%msg1.MakeString());
   $g_blockJITs = true;
}