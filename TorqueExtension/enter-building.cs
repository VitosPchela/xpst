$b1 = false;
$b2 = false;
$b3 = false;

function onEnterBuilding(%trigger,%obj)
{
   if(%trigger.getName() $= "building1trigger" && $b1 == false)
   {
      echo("entered building1");
      $b1 = true;
   }
   else if(%trigger.getName() $= "building2trigger" && $b2 == false)
   {
      echo("entered building2");
      $b2 = true;
   }
   else if(%trigger.getName() $= "building3trigger" && $b3 == false)
   {
      echo("entered building3");
      $b3 = true;
   }
}