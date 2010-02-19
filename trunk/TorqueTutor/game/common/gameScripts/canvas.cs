//---------------------------------------------------------------------------------------------
// Torque Game Builder
// Copyright (C) GarageGames.com, Inc.
//---------------------------------------------------------------------------------------------

//---------------------------------------------------------------------------------------------
// initializeCanvas
// Constructs and initializes the default canvas window.
//---------------------------------------------------------------------------------------------
$canvasCreated = false;

function configureCanvas()
{
   // Build our $pref::Video::mode if needed
   if ($pref::Video::mode $= "")
   {
      // First some defaults
      %res = "800 600";
      %fs = false;
      %bpp = 32;
      %rate = 60;
      %fsaa = 0;
   
      if ($pref::Video::Resolution !$= "")
         %res = $pref::Video::Resolution;
      if ($pref::Video::fullScreen !$= "")
         %fs = $pref::Video::fullScreen;
      if ($pref::Video::BitsPerPixel !$= "")
         %bpp = $pref::Video::BitsPerPixel;
      if ($pref::Video::RefreshRate !$= "")
         %rate = $pref::Video::RefreshRate;
      if ($pref::Video::FSAALevel !$= "")
         %fsaa = $pref::Video::FSAALevel;
      
      $pref::Video::mode = %res SPC %fs SPC %bpp SPC %rate SPC %fsaa;
   }

   %resX = getWord($pref::Video::mode, $WORD::RES_X);
   %resY = getWord($pref::Video::mode, $WORD::RES_Y);
   %fs = getWord($pref::Video::mode,   $WORD::FULLSCREEN);
   %bpp = getWord($pref::Video::mode,  $WORD::BITDEPTH);
   %rate = getWord($pref::Video::mode, $WORD::REFRESH);
   %fsaa = getWord($pref::Video::mode, $WORD::AA);
   
   echo("--------------");
   echo("Attempting to set resolution to \"" @ $pref::Video::mode @ "\"");
   
   %deskRes    = getDesktopResolution();      
   %deskResX   = getWord(%deskRes, $WORD::RES_X);
   %deskResY   = getWord(%deskRes, $WORD::RES_Y);
   %deskResBPP = getWord(%deskRes, 2);
   
   // We shouldn't be getting this any more but just in case...
   if (%bpp $= "Default")
      %bpp = %deskResBPP;
      
   // Make sure we are running at a valid resolution
   if (%fs $= "0" || %fs $= "false")
   {
      // Windowed mode has to use the same bit depth as the desktop
      %bpp = %deskResBPP;
      
      // Windowed mode also has to run at a smaller resolution than the desktop
      if ((%resX >= %deskResX) || (%resY >= %deskResY))
      {
         warn("Warning: The requested windowed resolution is equal to or larger than the current desktop resolution. Attempting to find a better resolution");
      
         %resCount = Canvas.getModeCount();
         for (%i = (%resCount - 1); %i >= 0; %i--)
         {
            %testRes = Canvas.getMode(%i);
            %testResX = getWord(%testRes, $WORD::RES_X);
            %testResY = getWord(%testRes, $WORD::RES_Y);
            %testBPP  = getWord(%testRes, $WORD::BITDEPTH);

            if (%testBPP != %bpp)
               continue;
            
            if ((%testResX < %deskResX) && (%testResY < %deskResY))
            {
               // This will work as our new resolution
               %resX = %testResX;
               %resY = %testResY;
               
               warn("Warning: Switching to \"" @ %resX SPC %resY SPC %bpp @ "\"");
               
               break;
            }
         }
      }
   }
      
   $pref::Video::mode = %resX SPC %resY SPC %fs SPC %bpp SPC %rate SPC %fsaa;
   
   if (%fs == 1 || %fs $= "true")
      %fsLabel = "Yes";
   else
      %fsLabel = "No";

   echo("Accepted Mode: " NL
      "--Resolution : " @  %resX SPC %resY NL 
      "--Full Screen : " @ %fsLabel NL
      "--Bits Per Pixel : " @ %bpp NL
      "--Refresh Rate : " @ %rate NL
      "--FSAA Level " @ %fsaa NL
      "--------------");

   // Actually set the new video mode
   Canvas.setVideoMode(%resX, %resY, %fs, %bpp, %rate, %fsaa);
}

function initializeCanvas(%windowName)
{
   // Don't duplicate the canvas.
   if($canvasCreated)
   {
      error("Cannot instantiate more than one canvas!");
      return;
   }

   if (!createCanvas(%windowName))
   {
      error("Canvas creation failed. Shutting down.");
      quit();
   }

   $canvasCreated = true;
}

//---------------------------------------------------------------------------------------------
// resetCanvas
// Forces the canvas to redraw itself.
//---------------------------------------------------------------------------------------------
function resetCanvas()
{
   if (isObject(Canvas))
      Canvas.repaint();
}

//---------------------------------------------------------------------------------------------
// Callbacks for window events.
//---------------------------------------------------------------------------------------------

function GuiCanvas::onLoseFocus(%this)
{
}