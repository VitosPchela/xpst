//---------------------------------------------------------------------------------------------
// Torque Game Builder
// Copyright (C) GarageGames.com, Inc.
//---------------------------------------------------------------------------------------------

//---------------------------------------------------------------------------------------------
// onWake
// Called by the engine when the options gui is first set.
//---------------------------------------------------------------------------------------------
function OptionsDlg::onWake(%this)
{
   // Get the fullscreen value.
   FullscreenToggle.setValue(Canvas.isFullScreen());
   
   // Fill the graphics drop down menu.
   %buffer = getDisplayDeviceList();
   %count = getFieldCount(%buffer);
   GraphicsDriverMenu.clear();
   
   for(%i = 0; %i < %count; %i++)
      GraphicsDriverMenu.add(getField(%buffer, %i), %i);
      
   // Grab the current graphics driver selection.
   %selId = GraphicsDriverMenu.findText($pref::Video::displayDevice);
   if (%selId == -1)
      %selId = 0;
      
   GraphicsDriverMenu.setSelected(%selId);
   GraphicsDriverMenu.onSelect(%selId, "");
   
   // Set audio.
   MusicAudioVolume.setValue($pref::Audio::channelVolume[$musicAudioType]);
   EffectsAudioVolume.setValue($pref::Audio::channelVolume[$effectsAudioType]);
   
   // Fill the screenshot drop down menu.
   ScreenshotMenu.clear();
   ScreenshotMenu.add("PNG", 0);
   ScreenshotMenu.add("JPEG", 1);
   ScreenshotMenu.setValue($pref::Video::screenShotFormat);
   
   // Set up the keybind options.
   initializeKeybindOptions();
}

//---------------------------------------------------------------------------------------------
// GraphicsDriverMenu.onSelect
// Called when a graphics device is selected.
//---------------------------------------------------------------------------------------------
function GraphicsDriverMenu::onSelect(%this, %id, %text)
{
   // Attempt to keep the same res and bpp settings.
   %prevRes = getWords(Canvas.getVideoMode(), $WORD::RES_X, $WORD::RES_Y);
   %prevBPP = getWord(Canvas.getVideoMode(), $WORD::BITDEPTH);
   
   debugv("%prevBPP");
   
   //FullscreenToggle.setActive(true);
   
   // Fill the resolution and bit depth lists.
   ResolutionMenu.init(%this.getText(), FullscreenToggle.getValue());
   BPPMenu.init(%this.getText());
   
   // Try to select the previous settings, otherwise set the first in the list.
   %selId = ResolutionMenu.findText(%prevRes);
   if (%selId == -1)
      %selId = ResolutionMenu.size() - 1;
      
   ResolutionMenu.setSelected(%selId);
   
   // Bitdepth
   %selId = BPPMenu.findText(%prevBPP);
   if (%selId == -1)
      %selId = 0;
      
   BPPMenu.setSelected(%selId);
}

//---------------------------------------------------------------------------------------------
// ResolutionMenu
// Initialize the resolution menu based on the device and fullscreen settings.
//---------------------------------------------------------------------------------------------
function ResolutionMenu::init(%this, %device, %fullScreen)
{
   // Clear out previous values.
   %this.clear();
   
   if( %fullScreen $= "" )
      %fullScreen = FullscreenToggle.getValue();

   // Loop through all and add all valid resolutions
   %count = 0;
   %resCount = Canvas.getModeCount();
   for (%i = 0; %i < %resCount; %i++)
   {
      %testResString = Canvas.getMode(%i);
      %testRes  = getWords(%testResString, $WORD::RES_X, $WORD::RES_Y);

      // Only add to list if it isn't there already.
      if (%this.findText(%testRes) == -1)
      {
         %this.add(%testRes, %count);
         %count++;
      }
   }
}

//---------------------------------------------------------------------------------------------
// BPPMenu.init
// Initialize the bits per pixel menu.
//---------------------------------------------------------------------------------------------
function BPPMenu::init(%this, %device)
{
   // Clear previous values.
   %this.clear();
   
   %resCount = Canvas.getModeCount();
   %count = 0;
   for (%i = 0; %i < %resCount; %i++)
   {
      %curResString = Canvas.getMode(%i);
      %bpp = getWord(%curResString, 2);

      // Only add to list if it isn't there already.
      if (%this.findText(%bpp) == -1)
      {
         %this.add(%bpp, %count);
         %count++;
      }
   }
}

//---------------------------------------------------------------------------------------------
// FullscreenToggle.onAction
// Called when the fullscreen checkbox is toggled.
//---------------------------------------------------------------------------------------------
function FullscreenToggle::onAction(%this)
{
   %prevRes = ResolutionMenu.getText();

   // Update the resolution menu with the new options
   ResolutionMenu.init(GraphicsDriverMenu.getText(), %this.getValue());

   // Set it back to the previous resolution if the new mode supports it.
   %selId = ResolutionMenu.findText(%prevRes);

   if (%selId != -1)
      ResolutionMenu.setSelected(%selId);
   else
      ResolutionMenu.setSelected(ResolutionMenu.size() - 1);
}

//---------------------------------------------------------------------------------------------
// updateChannelVolume
// Update an audio channels volume.
//---------------------------------------------------------------------------------------------
$AudioTestHandle = 0;
function updateChannelVolume(%channel, %volume)
{
   // Only valid channels are 1-8
   if (%channel < 1 || %channel > 8)
      return;
      
   sfxSetChannelVolume(%channel, %volume);
   $pref::Audio::channelVolume[%channel] = %volume;
   
   // Play a test sound for volume feedback.
   if ( !isObject( $AudioTestSource ) )
      $AudioTestSource = sfxPlayOnce( "AudioChannel" @ %channel, expandFilename( "~/data/audio/volumeTest.wav" ) );
}

//---------------------------------------------------------------------------------------------
// applyAVOptions
// Apply the AV changes.
//---------------------------------------------------------------------------------------------
function applyAVOptions()
{
   %newDriver = GraphicsDriverMenu.getText();
   %newRes = ResolutionMenu.getText();
   %newBpp = BPPMenu.getText();
   %newFullScreen = FullscreenToggle.getValue();
   $pref::Video::screenShotFormat = ScreenshotMenu.getText();
   
   if (%newFullScreen  && !Canvas.isFullScreen()) 
      $Video::WindowedDesktopSize = getDesktopResolution();
      
   if (%newDriver !$= $pref::Video::displayDevice)
   {
      MessageBoxYesNo( "Change Video Device Now?",
         "Do you want to RESTART Torque and switch to the selected video device now? If you hit yes, Torque will restart using the new video device. If you select no, your change will take effect at next run.",
         "restartInstance();", "");
         
         $pref::Video::displayDevice = %newDriver;
   }

   Canvas.setVideoMode(firstWord(%newRes), getWord(%newRes, 1), %newBpp, %newFullScreen);

   Canvas.popDialog(OptionsDlg);
   Canvas.pushDialog(OptionsDlg);

   FullscreenToggle.setValue(%newFullScreen);
}

//---------------------------------------------------------------------------------------------
// revertAVOptions
// Revert the AV options to the defaults. Does not apply the changes. Only resets the
// selections.
//---------------------------------------------------------------------------------------------
function revertAVOptions()
{
   // Default resolution: 800x600;
   %selId = ResolutionMenu.findText("800 600");
   if (%selId == -1)
      %selId = 0;
      
   ResolutionMenu.setSelected(%selId);
   
   // Default fullscreen: false;
   FullscreenToggle.setValue(false);
   BPPMenu.setText("32");
   
   // Default graphics driver: D3D9;
   %selId = GraphicsDriverMenu.findText("D3D9");
   if (%selId == -1)
      %selId = 0;
      
   GraphicsDriverMenu.setSelected(%selId);
   GraphicsDriverMenu.onSelect(%selId, "");
   
   // Default volume: 0.8;
   EffectsAudioVolume.setValue(0.8);
   MusicAudioVolume.setValue(0.8);
   updateChannelVolume($effectsAudioType, 0.8);
   updateChannelVolume($musicAudioType, 0.8);
   ScreenshotMenu.clear();
   
   // Default screenshot format: PNG;
   ScreenshotMenu.setValue("PNG");
}

//---------------------------------------------------------------------------------------------
// revertAVOptionChanges
// Revert the AV changes made since the options menu was opened - which happen to be the
// values of the related $prefs.
//---------------------------------------------------------------------------------------------
function revertAVOptionChanges()
{
   FullscreenToggle.setValue(isFullScreen());

   %selId = ResolutionMenu.findText(getWords($pref::Video::mode, $WORD::RES_X, $WORD::RES_Y));

   if (%selId == -1)
      %selId = 0;
      
   ResolutionMenu.setSelected(%selId);
   
   // Bit depth only applies to full screen mode.
   %selId = BPPMenu.findText(getWord($pref::Video::mode, $WORD::BITDEPTH));
   if (%selId == -1)
      %selId = 0;
      
   BPPMenu.setSelected(%selId);
   BPPMenu.setText(BPPMenu.getTextById(%selId));
   
   %selId = GraphicsDriverMenu.findText($pref::Video::displayDevice);
   if (%selId == -1)
      %selId = 0;
      
   GraphicsDriverMenu.setSelected(%selId);
   GraphicsDriverMenu.onSelect(%selId, "");
   
   ScreenshotMenu.setValue($pref::video::screenshotFormat);
}
