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
   GlobalActionMap.bind(keyboard, "h", onHint);
   
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
   new TCPObject(httpPage) { };
   httpPage.post($g_gameTREURL @ "/" @ $mytre,%msg,1);
}

function onHint()
{
   %msg1 = new ScriptObject(DorminMessage).create("", "GETHINT", "answer","Undefined","String","String");
   sendMessage(%msg1.MakeString());
   $g_blockJITs = true;
}