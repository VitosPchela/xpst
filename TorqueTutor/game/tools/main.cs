//---------------------------------------------------------------------------------------------
// Torque Game Builder
// Copyright (C) GarageGames.com, Inc.
//---------------------------------------------------------------------------------------------

//---------------------------------------------------------------------------------------------
// Path to the folder that contains the editors we will load.
//---------------------------------------------------------------------------------------------
$Tools::resourcePath = "tools/";

// These must be loaded first, in this order, before anything else is loaded
$Tools::loadFirst = "editorClasses base";

//---------------------------------------------------------------------------------------------
// Tools Package.
//---------------------------------------------------------------------------------------------
package Tools
{
   function loadKeybindings()
   {
      Parent::loadKeybindings();
      

   }
   
   // Start-up.
   function onStart()
   {
      Parent::onStart();
      
      echo(" % - Initializing common GUIs");
      
      // Common GUI stuff.
      exec("./gui/cursors.ed.cs");
      exec("./gui/profiles.ed.cs");
      
      // Make sure we get editor profiles before any GUI's
      // BUG: these dialogs are needed earlier in the init sequence, and should be moved to
      // common, along with the guiProfiles they depend on.
      exec("./gui/guiDialogs.ed.cs");
      
      //%toggle = $Scripts::ignoreDSOs;
      //$Scripts::ignoreDSOs = true;
       
      echo(" % - Initializing Tools");
            
      $ignoredDatablockSet = new SimSet();
   
      // fill the list of editors
      $editors[count] = getWordCount($Tools::loadFirst);
      for(%i = 0;%i < $editors[count];%i++)
      {
         $editors[%i] = getWord($Tools::loadFirst, %i);
      }
      
      %pattern = $Tools::resourcePath @ "/*/main.cs";
      %folder = findFirstFile(%pattern);
      if( %folder $= "")
      {
         // if we have absolutely no matches for main.cs, we look for main.cs.dso
         %pattern = $Tools::resourcePath @ "/*/main.cs.dso";
         %folder = findFirstFile(%pattern);
      }
      while(%folder !$= "")
      {
         %folder = filePath(%folder);
         %editor = fileName(%folder);
         if(IsDirectory(%folder))
         {
            // Yes, this sucks and should be done better
            if(strstr($Tools::loadFirst, %editor) == -1)
            {
               $editors[$editors[count]] = %editor;
               $editors[count]++;
            }
         }
         %folder = findNextFile(%pattern);
      }

      // initialize every editor
      %count = $editors[count];
      for (%i = 0; %i < %count; %i++)
      {
         exec("./" @ $editors[%i] @ "/main.cs");
         call("initialize" @ $editors[%i]);
      }



      // Load up the tools resources. All the editors are initialized at this point, so
      // resources can override, redefine, or add functionality.
      Tools::LoadResources( $Tools::resourcePath );
      
      //$Scripts::ignoreDSOs = %toggle;
   }
   
   // Shutdown.
   function onExit()
   {
      // Save any Layouts we might be using
      //GuiFormManager::SaveLayout(LevelBuilder, Default, User);
      
      %count = $editors[%i];
      for (%i = 0; %i < %count; %i++)
      {
         call("destroy" @ $editors[%i]);
      }
      
      // Export Preferences.
      echo("Exporting Gui preferences.");
      export("$Pref::FileDialogs::*", "fileDialogPrefs.cs", false);
	
      // Call Parent.
      Parent::onExit();
   }
};

function Tools::LoadResources( %path )
{
   %resourcesPath = %path @ "resources/";
   %resourcesList = getDirectoryList( %resourcesPath );
   
   %wordCount = getFieldCount( %resourcesList );
   for( %i = 0; %i < %wordCount; %i++ )
   {
      %resource = GetField( %resourcesList, %i );
      if( isFile( %resourcesPath @ %resource @ "/resourceDatabase.cs") )
         ResourceObject::load( %path, %resource );
   }
}


//-----------------------------------------------------------------------------
// Activate Package.
//-----------------------------------------------------------------------------
activatePackage(Tools);

