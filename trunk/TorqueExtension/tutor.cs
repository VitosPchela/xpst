$g_gameTREURL = "";
$g_taskName = "";
$g_isTutorRunning = false;
$mytre = "";

function startTutor(%id,%mission)
{
   // you can start event catching here
   
   $g_gameTREURL = "http://localhost:8081/GamexPSTServer/GamexPST";
   $g_taskName = getMissionDisplayName(%mission);
   
   // sending message to xPST engine at the start
   
   %data = "command=starttre&task=" @ $g_taskName;
   %resp = sendpost(%data);
   echo("********************" @ %resp);
   %brk = strpos(%resp," ");
	if (%brk == -1)
		$mytre = %resp;
	else
	{
		$mytre = getSubStr(%resp,0,%brk);
		%errmsg = getSubStr(%resp,%brk+1,5000);
		if (strlen(%errmsg) > 0)
			error(%errmsg);
	}
	$g_isTutorRunning = true;
}

//send the message to server and returns the response
function sendTutorMessage(%msg)
{
   new TCPObject(httpPage) { };
   httpPage.post($g_gameTREURL @ "/" @ $mytre,%msg);
   return httpPage.getResult();
}

function sendpost(%data)
{
   new TCPObject(httpPage) { };
   httpPage.post($g_gameTREURL,%data);
   return httpPage.getResult();
}