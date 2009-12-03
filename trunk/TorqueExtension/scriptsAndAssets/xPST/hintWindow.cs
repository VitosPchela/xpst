function showHint(%msg)
{
   $hintMsg = %msg;
   if($hintMsg !$= "")
   {
      $hintCount = $hintMsg.arrParameters.getValue($hintLocation).objValue.value.count();
      if($hintCount > 0)
      {
         %hmsg = $hintMsg.arrParameters.getValue($hintLocation).objValue.value.getValue($hintNumber).value;
         MessageBoxHint(%hmsg,"closeHandle();","prevHandle();","nextHandle();");
      }
   }
}

function closeHandle()
{
   $hintNumber = 0;
   $hintMsg = "";
   $hintLocation = "";
   $hintCount = 0;
}

function prevHandle()
{
   $hintNumber = $hintNumber - 1;
   %hmsg = $hintMsg.arrParameters.getValue($hintLocation).objValue.value.getValue($hintNumber).value;
   MessageBoxHint(%hmsg,"closeHandle();","prevHandle();","nextHandle();");
}

function nextHandle()
{
   $hintNumber = $hintNumber + 1;
   %hmsg = $hintMsg.arrParameters.getValue($hintLocation).objValue.value.getValue($hintNumber).value;
   MessageBoxHint(%hmsg,"closeHandle();","prevHandle();","nextHandle();");
}