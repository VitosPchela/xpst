//-----------------------------------------------------------------------------------------
// Torque Game Builder
// Copyright (C) GarageGames.com, Inc.
//-----------------------------------------------------------------------------------------

// This is a temporary test script for the breadcrumbs menu.
// We fill the menu with some default data, for testing purposes.
function GuiBreadcrumbsMenu::onWake(%this)
{
   %oldsep = %this.separator;
   %this.separator = ".";
   %this.addPath("TGBX.prefs.user.kilroy.video.resolution");
   %this.addPath(".a.be.cat.dump.ether.flatly.gloster.humanity.islington.");
   %this.setCurrentPath("TGBX.prefs.user");

   // put prefs in the menu.
   %file = new FileObject();
   %file.openForRead($defaultGame @ "/client/prefs.cs");
   %this.separator = ":";
   while(! %file.isEof() )
   {
      %line = %file.readLine();
      // first remove dots
      //%line = strreplace(%line, ".", ",");
      // then convert "::" and " = " to dots
      %line = strreplace(%line, "::", %this.separator);
      %line = strreplace(%line, " = ", %this.separator);
      %this.addPath(%line);
      if( %firstPath $= "" )
         %firstPath = %line;
   }
   //%this.setCurrentPath(%firstPath);

   %file = findFirstFile("*");
   %this.separator = "/";
   while( %file !$= "")
   {
      //%file = strreplace(%file, ".", ",");
      //%file = strreplace(%file, ":", ".");
      //%file = strreplace(%file, "/", ".");
      
      %this.addPath(%file);
      if(%firstPath $= "")
         %firstPath = %file;
         
      %file = findNextFile("*");
   }
   //%this.setCurrentPath(%firstPath);
   %this.separator = %oldsep;
}
