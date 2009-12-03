
function onEnterBuilding(%trigger,%obj)
{
   if(!%trigger.doneOnce)
   {
      %entname = %trigger.getName() @ ":Enter";
      %msg1 = new ScriptObject(DorminMessage).create(%entname, "NOTEVALUESET", "1","","String","String");
      sendMessage(%msg1.MakeString());
      %temp = getSubStr(%trigger.getName(),0,strpos(%trigger.getName(),"Trigger"));
      MessageBoxOK("Message","You have entered " @ %temp);
      %trigger.doneOnce = true;
   }
}