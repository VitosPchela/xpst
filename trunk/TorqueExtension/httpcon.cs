$MAX_HTTP_QUERY_STRING = 255;
$message_received = 0;

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

function httpPage::post(%this, %url, %data)
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
   //while ($message_received <= 0) { }
   //$message_received = 0;
   return %this.Buffer;
}

function httpPage::onDisconnect(%this)
{
  warn("Disconnected: " @ %this.Address);
  //$message_received = 1;
}

function httpPage::onConnectFailed(%this)
{
   error("Connection Failed: " @ %this.Address);
}