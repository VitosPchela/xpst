$g_gameTREURL = "";
$g_taskName = "";
$g_isTutorRunning = false;
$mytre = "";

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

function sendblankmessage()
{
   new TCPObject(httpPage) { };
   httpPage.post($g_gameTREURL @ "/" @ $mytre,"",1);
}