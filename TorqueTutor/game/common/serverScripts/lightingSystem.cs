//-----------------------------------------------
// Copyright © Synapse Gaming 2004
// Written by John Kabus
//-----------------------------------------------

$lightEditor::lightDBPath = "common/lighting/lights/";
$lightEditor::filterDBPath = "common/lighting/filters/";

function lightLoadDataBlocks(%basepath)
{
   //load precompiled...
   %path = %basepath @ "*.dso";
   echo("");
   echo("//-----------------------------------------------");
   echo("Loading light datablocks from: " @ %basepath);
   %file = findFirstFile(%path);
  
  while(%file !$= "")
  {
     %file = filePath(%file) @ "/" @ fileBase(%file);
     exec(%file);
      %file = findNextFile(%path);
  }

   //load uncompiled...
   %path = %basepath @ "*.cs";
   %file = findFirstFile(%path);

   while(%file !$= "")
   {
      exec(%file);
      %file = findNextFile(%path);
   }
}

lightLoadDataBlocks($lightEditor::lightDBPath);
lightLoadDataBlocks($lightEditor::filterDBPath);

function serverCmdGetLightDBId(%client, %db)
{
   %id = nameToId(%db);
   commandToClient(%client, 'GetLightDBIdCallback', %id);
}



