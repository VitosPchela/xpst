function onServerMessage(%a, %b, %c, %d, %e, %f, %g, %h, %i)
{
   echo("onServerMessage: ");
   if(%a !$= "") echo("  +- a: " @ %a);
   if(%b !$= "") echo("  +- b: " @ %b);
   if(%c !$= "") echo("  +- c: " @ %c);
   if(%d !$= "") echo("  +- d: " @ %d);
   if(%e !$= "") echo("  +- e: " @ %e);
   if(%f !$= "") echo("  +- f: " @ %f);
   if(%g !$= "") echo("  +- g: " @ %g);
   if(%h !$= "") echo("  +- h: " @ %h);
   if(%i !$= "") echo("  +- i: " @ %i);
}