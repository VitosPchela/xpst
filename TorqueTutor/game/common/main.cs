//---------------------------------------------------------------------------------------------
// Torque Game Builder
// Copyright (C) GarageGames.com, Inc.
//---------------------------------------------------------------------------------------------

// Constants for referencing video resolution preferences
$WORD::RES_X = 0;
$WORD::RES_Y = 1;
$WORD::FULLSCREEN = 2;
$WORD::BITDEPTH = 3;
$WORD::REFRESH = 4;
$WORD::AA = 5;

//---------------------------------------------------------------------------------------------
// CommonPackage
// Adds functionality for this mod to some standard functions.
//---------------------------------------------------------------------------------------------
package CommonPackage
{
//---------------------------------------------------------------------------------------------
// onStart
// Called when the engine is starting up. Initializes this mod.
//---------------------------------------------------------------------------------------------
function onStart()
{
   Parent::onStart();

   // Here is where we will do the video device stuff, so it overwrites the defaults
   // First set the PCI device variables (yes AGP/PCI-E works too)
   initDisplayDeviceInfo();

   // Uncomment to disable ShaderGen, useful when debugging
   //$ShaderGen::GenNewShaders = false;

   // Uncomment useNVPerfHud to allow you to start up correctly
   // when you drop your executable onto NVPerfHud
   //$Video::useNVPerfHud = true;
   
   // Uncomment these to allow you to force your app into using
   // a specific pixel shader version (0 is for fixed function)
   //$pref::Video::forcePixVersion = true;
   //$pref::Video::forcedPixVersion = 0;
   
   if ($platform $= "macos")
      $pref::Video::displayDevice = "OpenGL";
   else
      $pref::Video::displayDevice = "D3D9";
   
   // Initialise stuff.
   exec("./gameScripts/common.cs");
   initializeCommon();

   exec("./unifiedShell/main.cs");

   exec("./clientScripts/client.cs");
   exec("./serverScripts/server.cs");
   
   exec("./gui/guiBreadcrumbsMenu.cs");

   echo(" % - Initialized Common");
}

//---------------------------------------------------------------------------------------------
// onExit
// Called when the engine is shutting down. Shutdowns this mod.
//---------------------------------------------------------------------------------------------
function onExit()
{   
   // Shutdown stuff.
   shutdownCommon();

   Parent::onExit();
}

function loadKeybindings()
{
   $keybindCount = 0;
   // Load up the active projects keybinds.
   if(isFunction("setupKeybinds"))
      setupKeybinds();
}

//---------------------------------------------------------------------------------------------
// displayHelp
// Prints the command line options available for this mod.
//---------------------------------------------------------------------------------------------
function displayHelp() {
   // Let the parent do its stuff.
   Parent::displayHelp();

   error("Common Mod options:\n" @
         "  -fullscreen            Starts game in full screen mode\n" @
         "  -windowed              Starts game in windowed mode\n" @
         "  -autoVideo             Auto detect video, but prefers OpenGL\n" @
         "  -openGL                Force OpenGL acceleration\n" @
         "  -directX               Force DirectX acceleration\n" @
         "  -voodoo2               Force Voodoo2 acceleration\n" @
         "  -prefs <configFile>    Exec the config file\n");
}

//---------------------------------------------------------------------------------------------
// parseArgs
// Parses the command line arguments and processes those valid for this mod.
//---------------------------------------------------------------------------------------------
function parseArgs()
{
   // Let the parent grab the arguments it wants first.
   Parent::parseArgs();

   // Loop through the arguments.
   for (%i = 1; %i < $Game::argc; %i++)
   {
      %arg = $Game::argv[%i];
      %nextArg = $Game::argv[%i+1];
      %hasNextArg = $Game::argc - %i > 1;
   
      switch$ (%arg)
      {
         case "-fullscreen":
            setFullScreen(true);
            $argUsed[%i]++;

         case "-windowed":
            setFullScreen(false);
            $argUsed[%i]++;

         case "-openGL":
            $pref::Video::displayDevice = "OpenGL";
            $argUsed[%i]++;

         case "-directX":
            $pref::Video::displayDevice = "D3D";
            $argUsed[%i]++;

         case "-voodoo2":
            $pref::Video::displayDevice = "Voodoo2";
            $argUsed[%i]++;

         case "-autoVideo":
            $pref::Video::displayDevice = "";
            $argUsed[%i]++;

         case "-prefs":
            $argUsed[%i]++;
            if (%hasNextArg) {
               exec(%nextArg, true, true);
               $argUsed[%i+1]++;
               %i++;
            }
            else
               error("Error: Missing Command Line argument. Usage: -prefs <path/script.cs>");
      }
   }
}

};

activatePackage(CommonPackage);

