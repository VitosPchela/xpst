$MAX_HTTP_QUERY_STRING = 255;
$what_ondisconnect = 0;

function httpPage::init(%this, %url) {
   %host = "";
   %page = "";

   if(strpos(%url, "http://") == 0)
   {
      %host = getSubStr(%url, 7, strpos(%url, "/", 8) - 7);
      %page = getSubStr(%url, strpos(%url, "/", 8), $MAX_HTTP_QUERY_STRING);
   }
   else
   {
      %host = getSubStr(%url, 0, strpos(%url, "/", 8));
      %page = getSubStr(%url, strpos(%url, "/"));
   }

   if(strpos(%host, ":") < 0) %host = %host @ ":" @ "80";
   %this.Address = %host;
   %this.Page = %page;
}

function httpPage::get(%this, %url)
{
   %this.Buffer = "";
   %this.doBuffer = false;


   %this.init(%url);
   warn("Connecting to: " @ %this.Address @ %this.Page);
   %this.Method = "GET";
   %this.connect(%this.Address);
}

function httpPage::post(%this, %url, %data, %type)
{
  %this.Data = "";
  if(isObject(%data)) {
    warn("Data is Object: true");
    for(%x = 0; %x < %data.getCount(); %x++) {
      %datum = %data.getObject(%x);
      if(strlen(%postData) > 0) %postData = %postData @ "&";
      %this.Data = %datum.key @ "=" @ %datum.value;
    }
  } else {
    warn("Data is Object: false");
    %this.Data = %data;
  }


  error("Data: " @ %this.Data);
  error("%data: " @ %data);

  %this.init(%url);
  warn("Connecting to: " @ %this.Address @ %this.Page);
  %this.Method = "POST";
  $what_ondisconnect = %type;
  %this.connect(%this.Address);
}

function httpPage::onConnected(%this)
{
   warn("Connected ...");
   %query = %this.Method @ " " @ %this.page @ " HTTP/1.0\nHost: " @ %this.Address;
   if(%this.Method $= "POST") {
     %query = %query @ "\n" @ "Content-Type: application/x-www-form-urlencoded\n";
     %query = %query @ "Content-Length: " @ strlen(%this.Data) @ "\n\n";
     %query = %query @ %this.Data @ "\n";
   } else {
     %query = %query @ "\n\n";
   }
   warn("QUERY: " @ %query);
   %this.send(%query);
}

function httpPage::onLine(%this, %line)
{
   warn("LINE: " @ %line);
   if(!%this.doBuffer && %line $= "") { %this.doBuffer = true; return; }
   if(%this.doBuffer)
   {
      error("BUFFER: " @ %line);
      if(%this.Buffer !$= "") %this.Buffer = %this.Buffer @ "\n";
      %this.Buffer = %this.Buffer @ %line;
   }
}

function httpPage::getResult(%this)
{  
   return %this.Buffer;
}

function httpPage::onDisconnect(%this)
{
  warn("Disconnected: " @ %this.Address);
  %resp = %this.Buffer;
  if($what_ondisconnect == 0)
  {
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
      sendMessage("");  
  }
  else if($what_ondisconnect == 1)
  {
      parseReceivedMessage(%resp);
  }
  
}

function httpPage::onConnectFailed(%this)
{
   error("Connection Failed: " @ %this.Address);
}


function parseReceivedMessage(%message)
{
   %loop = true;
   %msg = DorminMsgFromString(%message);
   %rtnVal = "";

	while (%loop == true)
	{
		%loop = false;
		if (%msg.strVerb $= "APPROVE")
		{
			%addrarr = %msg.DorminAddr.strArrNames;
			if (%addrarr.getValue(0) $= 'TutorLink' && %addrarr.getValue(1) $= 'Done')
			{
			   MessageBoxOK("DONE!!!","Congratulations.You have succesfully completed the task");
				//implement messagebox to show this
            //stopTutor();
			}
		}
		else if (%msg.strVerb $= "JITMESSAGE")
		{
			if ($g_blockJITs == false)
				//show messagebox with JIT message
				MessageBoxOK("JIT Message",%msg.arrParameters.getValue(0).objValue.value.getValue(0).value);
		}
		else if (%msg.strVerb $= "HINTMESSAGE")
		{
			for(%tempHint = 0;%tempHint<%msg.arrParameters.count();%tempHint++)
			{
				if (%msg.arrParameters.getValue(%tempHint).strName $= "MESSAGE")
				{
					$hintLocation = %tempHint;
					break;
				}
			}
			%rtnVal = %msg;
			
		}
		else if (%msg.strVerb $= "FLAG")
		{
		}
		else if (%msg.strVerb $= "GETHINT")
		{
		}
		else if (%msg.strVerb $= "SETPROPERTY")
		{
		}
		else if (%msg.strVerb $= "QUERYINITIALVALUE")
		{
		}

		%nextMsgLocation = strpos(%message,"\n");// check if there is another message
		if (%nextMsgLocation != -1)
		{
			%loop = true;
			%nextMsgLocation++;
			%message = getSubStr(%message,%nextMsgLocation,strlen(%message));
			%msg = DorminMsgFromString(%message);
		}
	}
	showHint(%rtnVal);
}