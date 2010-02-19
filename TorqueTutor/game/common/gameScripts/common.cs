//---------------------------------------------------------------------------------------------
// Torque Game Builder
// Copyright (C) GarageGames.com, Inc.
//---------------------------------------------------------------------------------------------

//---------------------------------------------------------------------------------------------
// initializeCommon
// Initializes common game functionality.
//---------------------------------------------------------------------------------------------
function initializeCommon()
{
   // Not Reentrant
   if( $commonInitialized == true )
      return;
      
   // Common keybindings.
   GlobalActionMap.bind(keyboard, tilde, toggleConsole);
   GlobalActionMap.bind(keyboard, "ctrl p", doScreenShot);
   GlobalActionMap.bindcmd(keyboard, "alt enter", "Canvas.toggleFullScreen();","");
   GlobalActionMap.bindcmd(keyboard, "alt k", "cls();",  "");
   GlobalActionMap.bindCmd(keyboard, "escape", "", "handleEscape();");
   
   
   
   // Very basic functions used by everyone.
   exec("./audio.cs");
   exec("./canvas.cs");
   exec("./cursor.cs");

   // Content.
   exec("~/gui/profiles.cs");
   exec("~/gui/cursors.cs");

   // Seed the random number generator.
   setRandomSeed();
   // Set up networking.
   setNetPort(0);
   // Initialize the canvas.
   initializeCanvas("Torque Game Engine Advanced");
   
   // Common Guis.
   exec("~/gui/remap.gui");
   exec("~/gui/console.gui");
   
   // Gui Helper Scripts.
   exec("~/gui/help.cs");

   // Random Scripts.
   exec("./screenshot.cs");
   exec("./scriptDoc.cs");
   exec("./keybindings.cs");
   exec("./helperfuncs.cs");
   
   // Client scripts
   exec("~/clientScripts/metrics.cs");
   exec("~/clientScripts/recordings.cs");
   
   exec("~/clientScripts/shaders.cs");
   exec("~/clientScripts/materials.cs");
   
   // Set a default cursor.
   Canvas.setCursor(DefaultCursor);
   
   loadKeybindings();

   $commonInitialized = true;

}

//---------------------------------------------------------------------------------------------
// shutdownCommon
// Shuts down common game functionality.
//---------------------------------------------------------------------------------------------
function shutdownCommon()
{      
   sfxShutdown();
}

//---------------------------------------------------------------------------------------------
// dumpKeybindings
// Saves of all keybindings.
//---------------------------------------------------------------------------------------------
function dumpKeybindings()
{
   // Loop through all the binds.
   for (%i = 0; %i < $keybindCount; %i++)
   {
      // If we haven't dealt with this map yet...
      if (isObject($keybindMap[%i]))
      {
         // Save and delete.
         $keybindMap[%i].save(getPrefsPath("bind.cs"), %i == 0 ? false : true);
         $keybindMap[%i].delete();
      }
   }
}

function handleEscape()
{

   if (isObject(EditorGui))
   {
      if (Canvas.getContent() == EditorGui.getId())
      {
         ToggleEditor(1);
         return;    
      }
   }

   if (isObject(GuiEditor))
   {
      if (GuiEditor.isAwake())
      {
         GuiEditCanvas.quit();
         return;    
      }
   }

   if (PlayGui.isAwake())	
      escapeFromGame();	
}
