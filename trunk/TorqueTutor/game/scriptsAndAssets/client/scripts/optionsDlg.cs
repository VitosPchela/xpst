function optionsDlg::setPane(%this, %pane)
{
   OptAudioPane.setVisible(false);
   OptGraphicsPane.setVisible(false);
   OptNetworkPane.setVisible(false);
   OptControlsPane.setVisible(false);
   OptLightingPane.setVisible(false);
   ("Opt" @ %pane @ "Pane").setVisible(true);

   // Lame hack
   if( %pane $= "Graphics" )
      OptLightingPane.setVisible( true );

   OptRemapList.fillList();
}

function OptionsDlg::onWake(%this)
{
   OptGraphicsButton.performClick();
   
   OptGraphicsFullscreenToggle.setStateOn(Canvas.isFullScreen());
   OptScreenshotMenu.setValue($pref::Video::screenShotFormat);
   
   OptGraphicsDriverMenu.clear();
   OptScreenshotMenu.init();

   %buffer = getDisplayDeviceList();
   %count = getFieldCount( %buffer );   
   for(%i = 0; %i < %count; %i++)
      OptGraphicsDriverMenu.add(getField(%buffer, %i), %i);

   %selId = OptGraphicsDriverMenu.findText( getDisplayDeviceInformation() );
	if ( %selId == -1 )
		OptGraphicsDriverMenu.setFirstSelected();
   else
	   OptGraphicsDriverMenu.setSelected( %selId );

   // Set up the texture quality menu
   OptTextureQualityMenu.clear();
   OptTextureQualityMenu.add("Auto", 0);
   OptTextureQualityMenu.add("Force Low", 1);
   OptTextureQualityMenu.add("Force High",  2);

   $OptTextureQualityMenu::origValue = $pref::TextureManager::qualityMode;
   OptTextureQualityMenu.setSelected( $pref::TextureManager::qualityMode );

   // set up the Refresh Rate menu
   OptRefreshSelectMenu.clear();
   // OptRefreshSelectMenu.add("Auto", 60);
   OptRefreshSelectMenu.add("60", 60);
   OptRefreshSelectMenu.add("75", 75);
   
   $OptRefreshSelectMenu::origValue = $pref::Video::RefreshRate;
   OptRefreshSelectMenu.setSelected( $pref::Video::RefreshRate );
   
   // set up the FSAA menu
   
   OptFSAASelectMenu.clear();
   OptFSAASelectMenu.add("Off", 0);
   OptFSAASelectMenu.add("2", 2);
   OptFSAASelectMenu.add("4", 4);
   OptFSAASelectMenu.add("8", 8);
   // OptFSAASelectMenu.add("16", 16);
   
   $OptFSAASelectMenu::origValue = $pref::Video::FSAALevel;
   OptFSAASelectMenu.setSelected($pref::Video::FSAALevel);
   
   // Audio
   //OptAudioHardwareToggle.setStateOn($pref::SFX::useHardware);
   //OptAudioHardwareToggle.setActive( true );
   
   OptAudioVolumeMaster.setValue($pref::SFX::masterVolume);
   OptAudioVolumeShell.setValue($pref::SFX::channelVolume[$GuiAudioType]);
   OptAudioVolumeSim.setValue($pref::SFX::channelVolume[$SimAudioType]);
   
   OptAudioProviderList.clear();
   %buffer = sfxGetAvailableDevices();
   %count = getRecordCount( %buffer );   
   for(%i = 0; %i < %count; %i++)
   {
      %record = getRecord(%buffer, %i);
      %provider = getField(%record, 0);
      
      if ( OptAudioProviderList.findText( %provider ) == -1 )
            OptAudioProviderList.add( %provider, %i );
   }

   %selId = OptAudioProviderList.findText($pref::SFX::provider);
	if ( %selId == -1 )
		OptAudioProviderList.setFirstSelected();
   else
	   OptAudioProviderList.setSelected( %selId );

   // Synapse Lighting options
	OptShadowQualityMenu.clear();
	OptShadowQualityMenu.add("High", 0);
	OptShadowQualityMenu.add("Medium", 1);
	OptShadowQualityMenu.add("Low", 2);
	OptShadowQualityMenu.setSelected($pref::LightManager::DynamicShadowQuality);

	OptShadowDetailMenu.clear();
	OptShadowDetailMenu.add("High", 0);
	OptShadowDetailMenu.add("Medium", 1);
	OptShadowDetailMenu.add("Low", 2);
	OptShadowDetailMenu.add("Lowest", 3);
	OptShadowDetailMenu.setSelected($pref::LightManager::DynamicShadowDetailSize);
}

function OptionsDlg::onSleep(%this)
{
   // write out the control config into the rw/config.cs file
   moveMap.save( "~/client/config.cs" );

}

function OptRefreshSelectMenu::onSelect(%this, %id, %text)
{
   %this.setText(%text);
   $pref::Video::RefreshRate = %id;
}

function OptFSAASelectMenu::onSelect(%this, %id, %text)
{
   %this.setText(%text);
   $pref::Video::FSAALevel = %id;
}

function OptShadowQualityMenu::onSelect(%this, %id, %text)
{
	%this.setText(%text);
	$pref::LightManager::sgDynamicShadowQuality = %id;
}

function OptShadowDetailMenu::onSelect(%this, %id, %text)
{
	%this.setText(%text);
	$pref::LightManager::sgDynamicShadowDetailSize = %id;
}

function OptTextureQualityMenu::onSelect( %this, %id, %text )
{
   if(%id != $OptTextureQualityMenu::origValue)
      MessageBoxOK( "Changes Require Restart", "This change will not take effect until you restart this application." );

   if(%text $= "Force High")
      MessageBoxOK( "WARNING", "By forcing textures to high, you may exceed your available video memory. Doing so may " @
                               "result in SLOW FRAMERATES, VISUAL ARTIFACTS, AND/OR DAMAGE TO YOUR SYSTEM. If you aren't " @
                               "SURE you want to force high-resolution textures, please set this option back to auto.");

   $pref::TextureManager::qualityMode = %id;
}

function OptGraphicsDriverMenu::onSelect( %this, %id, %text )
{
	// Attempt to keep the same res and bpp settings:
   %currRes = getWords(Canvas.getVideoMode(), $WORD::RES_X, $WORD::RES_Y);
   %currBPP = getWord(Canvas.getVideoMode(), $WORD::BITDEPTH);

	// Fill the resolution and bit depth lists:
	OptGraphicsResolutionMenu.init( %this.getText(), OptGraphicsFullscreenToggle.getValue() );
	OptGraphicsBPPMenu.init( %this.getText() );

	// Try to select the previous settings:
	%selId = OptGraphicsResolutionMenu.findText( %currRes );
	if ( %selId == -1 )
		%selId = 0;
	OptGraphicsResolutionMenu.setSelected( %selId );

	%selId = OptGraphicsBPPMenu.findText( %currBPP );
	if ( %selId == -1 )
		%selId = 0;
	OptGraphicsBPPMenu.setSelected( %selId );
	OptGraphicsBPPMenu.setText( OptGraphicsBPPMenu.getTextById( %selId ) );
}

function OptGraphicsResolutionMenu::init( %this, %device, %fullScreen )
{
   // Clear out previous values
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

function OptGraphicsFullscreenToggle::onAction(%this)
{
   Parent::onAction();
   %prevRes = OptGraphicsResolutionMenu.getText();

   // Update the resolution menu with the new options
   OptGraphicsResolutionMenu.init( OptGraphicsDriverMenu.getText(), %this.getValue() );

   // Set it back to the previous resolution if the new mode supports it.
   %selId = OptGraphicsResolutionMenu.findText( %prevRes );
   if ( %selId == -1 )
   	%selId = 0;
 	OptGraphicsResolutionMenu.setSelected( %selId );
}


function OptGraphicsBPPMenu::init( %this, %device )
{
	// Clear previous values.
   %this.clear();
   
   %resCount = Canvas.getModeCount();
   %count = 0;
   for (%i = 0; %i < %resCount; %i++)
   {
      %curResString = Canvas.getMode(%i);
      %bpp = getWord(%curResString, $WORD::BITDEPTH);

      // Only add to list if it isn't there already.
      if (%this.findText(%bpp) == -1)
      {
         %this.add(%bpp, %count);
         %count++;
      }
   }
}

function OptScreenshotMenu::init( %this )
{
   if( %this.findText("PNG") == -1 )
      %this.add("PNG", 0);
   if( %this.findText("JPEG") == - 1 )
      %this.add("JPEG", 1);
}

function optionsDlg::applyGraphics( %this )
{
	%newAdapter    = OptGraphicsDriverMenu.getText();
	%newRes        = OptGraphicsResolutionMenu.getText();
	%newBpp        = OptGraphicsBPPMenu.getText();
	%newFullScreen = OptGraphicsFullscreenToggle.getValue();
	%newRefresh    = OptRefreshSelectMenu.getSelected();
	%newFSAA       = OptFSAASelectMenu.getSelected();
	
	%numAdapters   = GFXInit::getAdapterCount();
	%newDevice     = $pref::Video::displayDevice;
	
	for( %i = 0; %i < %numAdapters; %i ++ )
	   if( GFXInit::getAdapterName( %i ) $= %newAdapter )
	   {
	      %newDevice = GFXInit::getAdapterType( %i );
	      break;
	   }
	      
   $pref::Video::displayDevice = %newDevice;
	if( %newAdapter !$= getDisplayDeviceInformation() )
	   MessageBoxOK( "Change requires restart", "Please restart the application for a display device change to take effect." );
	
	$pref::Video::screenShotFormat = OptScreenshotMenu.getText();
	
	// do some validation
	if ( (%newBpp $= "") || (%newBpp $= "Default") )
      %newBpp = getWord(getDesktopResolution(), 2);
   
   %selId = OptRefreshSelectMenu.findText( %newRefresh );
   if ( %selId == -1 )
   {
   	%selId = 0;
 	   OptRefreshSelectMenu.setSelected( %selId );
 	   $newRefresh = OptRefreshSelectMenu.getText();
   }

   %newMode = %newRes SPC %newFullScreen SPC %newBpp SPC %newRefresh SPC %newFSAA;
   if( %newMode !$= $pref::Video::mode )
   {
      $pref::Video::mode = %newMode;
      configureCanvas();
   }
}



$RemapCount = 0;
$RemapName[$RemapCount] = "Forward";
$RemapCmd[$RemapCount] = "moveforward";
$RemapCount++;
$RemapName[$RemapCount] = "Backward";
$RemapCmd[$RemapCount] = "movebackward";
$RemapCount++;
$RemapName[$RemapCount] = "Strafe Left";
$RemapCmd[$RemapCount] = "moveleft";
$RemapCount++;
$RemapName[$RemapCount] = "Strafe Right";
$RemapCmd[$RemapCount] = "moveright";
$RemapCount++;
$RemapName[$RemapCount] = "Turn Left";
$RemapCmd[$RemapCount] = "turnLeft";
$RemapCount++;
$RemapName[$RemapCount] = "Turn Right";
$RemapCmd[$RemapCount] = "turnRight";
$RemapCount++;
$RemapName[$RemapCount] = "Look Up";
$RemapCmd[$RemapCount] = "panUp";
$RemapCount++;
$RemapName[$RemapCount] = "Look Down";
$RemapCmd[$RemapCount] = "panDown";
$RemapCount++;
$RemapName[$RemapCount] = "Jump";
$RemapCmd[$RemapCount] = "jump";
$RemapCount++;
$RemapName[$RemapCount] = "Fire Weapon";
$RemapCmd[$RemapCount] = "mouseFire";
$RemapCount++;
$RemapName[$RemapCount] = "Adjust Zoom";
$RemapCmd[$RemapCount] = "setZoomFov";
$RemapCount++;
$RemapName[$RemapCount] = "Toggle Zoom";
$RemapCmd[$RemapCount] = "toggleZoom";
$RemapCount++;
$RemapName[$RemapCount] = "Free Look";
$RemapCmd[$RemapCount] = "toggleFreeLook";
$RemapCount++;
$RemapName[$RemapCount] = "Switch 1st/3rd";
$RemapCmd[$RemapCount] = "toggleFirstPerson";
$RemapCount++;
$RemapName[$RemapCount] = "Chat to Everyone";
$RemapCmd[$RemapCount] = "toggleMessageHud";
$RemapCount++;
$RemapName[$RemapCount] = "Message Hud PageUp";
$RemapCmd[$RemapCount] = "pageMessageHudUp";
$RemapCount++;
$RemapName[$RemapCount] = "Message Hud PageDown";
$RemapCmd[$RemapCount] = "pageMessageHudDown";
$RemapCount++;
$RemapName[$RemapCount] = "Resize Message Hud";
$RemapCmd[$RemapCount] = "resizeMessageHud";
$RemapCount++;
$RemapName[$RemapCount] = "Show Scores";
$RemapCmd[$RemapCount] = "showPlayerList";
$RemapCount++;
$RemapName[$RemapCount] = "Animation - Wave";
$RemapCmd[$RemapCount] = "celebrationWave";
$RemapCount++;
$RemapName[$RemapCount] = "Animation - Salute";
$RemapCmd[$RemapCount] = "celebrationSalute";
$RemapCount++;
$RemapName[$RemapCount] = "Suicide";
$RemapCmd[$RemapCount] = "suicide";
$RemapCount++;
$RemapName[$RemapCount] = "Toggle Camera";
$RemapCmd[$RemapCount] = "toggleCamera";
$RemapCount++;
$RemapName[$RemapCount] = "Drop Camera at Player";
$RemapCmd[$RemapCount] = "dropCameraAtPlayer";
$RemapCount++;
$RemapName[$RemapCount] = "Drop Player at Camera";
$RemapCmd[$RemapCount] = "dropPlayerAtCamera";
$RemapCount++;
$RemapName[$RemapCount] = "Bring up Options Dialog";
$RemapCmd[$RemapCount] = "bringUpOptions";
$RemapCount++;


function restoreDefaultMappings()
{
   moveMap.delete();
   exec( "~/scripts/default.bind.cs" );
   OptRemapList.fillList();
}

function getMapDisplayName( %device, %action )
{
	if ( %device $= "keyboard" )
		return( %action );		
	else if ( strstr( %device, "mouse" ) != -1 )
	{
		// Substitute "mouse" for "button" in the action string:
		%pos = strstr( %action, "button" );
		if ( %pos != -1 )
		{
			%mods = getSubStr( %action, 0, %pos );
			%object = getSubStr( %action, %pos, 1000 );
			%instance = getSubStr( %object, strlen( "button" ), 1000 );
			return( %mods @ "mouse" @ ( %instance + 1 ) );
		}
		else
			error( "Mouse input object other than button passed to getDisplayMapName!" );
	}
	else if ( strstr( %device, "joystick" ) != -1 )
	{
		// Substitute "joystick" for "button" in the action string:
		%pos = strstr( %action, "button" );
		if ( %pos != -1 )
		{
			%mods = getSubStr( %action, 0, %pos );
			%object = getSubStr( %action, %pos, 1000 );
			%instance = getSubStr( %object, strlen( "button" ), 1000 );
			return( %mods @ "joystick" @ ( %instance + 1 ) );
		}
		else
	   { 
	      %pos = strstr( %action, "pov" );
         if ( %pos != -1 )
         {
            %wordCount = getWordCount( %action );
            %mods = %wordCount > 1 ? getWords( %action, 0, %wordCount - 2 ) @ " " : "";
            %object = getWord( %action, %wordCount - 1 );
            switch$ ( %object )
            {
               case "upov":   %object = "POV1 up";
               case "dpov":   %object = "POV1 down";
               case "lpov":   %object = "POV1 left";
               case "rpov":   %object = "POV1 right";
               case "upov2":  %object = "POV2 up";
               case "dpov2":  %object = "POV2 down";
               case "lpov2":  %object = "POV2 left";
               case "rpov2":  %object = "POV2 right";
               default:       %object = "??";
            }
            return( %mods @ %object );
         }
         else
            error( "Unsupported Joystick input object passed to getDisplayMapName!" );
      }
	}
		
	return( "??" );		
}

function buildFullMapString( %index )
{
   %name       = $RemapName[%index];
   %cmd        = $RemapCmd[%index];

   %temp = moveMap.getBinding( %cmd );
   if ( %temp $= "" )
      return %name TAB "";

   %mapString = "";

   %count = getFieldCount( %temp );
   for ( %i = 0; %i < %count; %i += 2 )
   {
      if ( %mapString !$= "" )
         %mapString = %mapString @ ", ";

      %device = getField( %temp, %i + 0 );
      %object = getField( %temp, %i + 1 );
      %mapString = %mapString @ getMapDisplayName( %device, %object );
   }

   return %name TAB %mapString; 
}

function OptRemapList::fillList( %this )
{
	%this.clear();
   for ( %i = 0; %i < $RemapCount; %i++ )
      %this.addRow( %i, buildFullMapString( %i ) );
}

//------------------------------------------------------------------------------
function OptRemapList::doRemap( %this )
{
	%selId = %this.getSelectedId();
   %name = $RemapName[%selId];

	OptRemapText.setValue( "REMAP \"" @ %name @ "\"" );
	OptRemapInputCtrl.index = %selId;
	Canvas.pushDialog( RemapDlg );
}

//------------------------------------------------------------------------------
function redoMapping( %device, %action, %cmd, %oldIndex, %newIndex )
{
	//%actionMap.bind( %device, %action, $RemapCmd[%newIndex] );
	moveMap.bind( %device, %action, %cmd );
	OptRemapList.setRowById( %oldIndex, buildFullMapString( %oldIndex ) );
	OptRemapList.setRowById( %newIndex, buildFullMapString( %newIndex ) );
}

//------------------------------------------------------------------------------
function findRemapCmdIndex( %command )
{
	for ( %i = 0; %i < $RemapCount; %i++ )
	{
		if ( %command $= $RemapCmd[%i] )
			return( %i );			
	}
	return( -1 );	
}

/// This unbinds actions beyond %count associated to the
/// particular moveMap %commmand.
function unbindExtraActions( %command, %count )
{
   %temp = moveMap.getBinding( %command );
   if ( %temp $= "" )
      return;

   %count = getFieldCount( %temp ) - ( %count * 2 );
   for ( %i = 0; %i < %count; %i += 2 )
   {
      %device = getField( %temp, %i + 0 );
      %action = getField( %temp, %i + 1 );
      
      moveMap.unbind( %device, %action );
   }
}

function OptRemapInputCtrl::onInputEvent( %this, %device, %action )
{
   //error( "** onInputEvent called - device = " @ %device @ ", action = " @ %action @ " **" );
   Canvas.popDialog( RemapDlg );

   // Test for the reserved keystrokes:
   if ( %device $= "keyboard" )
   {
      // Cancel...
      if ( %action $= "escape" )
      {
         // Do nothing...
         return;
      }
   }

   %cmd  = $RemapCmd[%this.index];
   %name = $RemapName[%this.index];

   // Grab the friendly display name for this action
   // which we'll use when prompting the user below.
   %mapName = getMapDisplayName( %device, %action );
   
   // Get the current command this action is mapped to.
   %prevMap = moveMap.getCommand( %device, %action );

   // If nothing was mapped to the previous command 
   // mapping then it's easy... just bind it.
   if ( %prevMap $= "" )
   {
      unbindExtraActions( %cmd, 1 );
      moveMap.bind( %device, %action, %cmd );
      OptRemapList.setRowById( %this.index, buildFullMapString( %this.index ) );
      return;
   }

   // If the previous command is the same as the 
   // current then they hit the same input as what
   // was already assigned.
   if ( %prevMap $= %cmd )
   {
      unbindExtraActions( %cmd, 0 );
      moveMap.bind( %device, %action, %cmd );
      OptRemapList.setRowById( %this.index, buildFullMapString( %this.index ) );
      return;   
   }

   // Look for the index of the previous mapping.
   %prevMapIndex = findRemapCmdIndex( %prevMap );
   
   // If we get a negative index then the previous 
   // mapping was to an item that isn't included in
   // the mapping list... so we cannot unmap it.
   if ( %prevMapIndex == -1 )
   {
      MessageBoxOK( "Remap Failed", "\"" @ %mapName @ "\" is already bound to a non-remappable command!" );
      return;
   }

   // Setup the forced remapping callback command.
   %callback = "redoMapping(" @ %device @ ", \"" @ %action @ "\", \"" @
                              %cmd @ "\", " @ %prevMapIndex @ ", " @ %this.index @ ");";
   
   // Warn that we're about to remove the old mapping and
   // replace it with another.
   %prevCmdName = $RemapName[%prevMapIndex];
   MessageBoxYesNo( "Warning",
      "\"" @ %mapName @ "\" is already bound to \""
      @ %prevCmdName @ "\"!\nDo you wish to replace this mapping?",
       %callback, "" );
}

new SFXDescription(ZeroChannel)
{
   volume   = 1.0;
   isLooping= false;
   is3D     = false;
   channel  = 0;
};

$AudioTestHandle = 0;

function OptAudioUpdateMasterVolume(%volume)
{
   if (%volume == $pref::SFX::masterVolume)
      return;
      
   sfxSetMasterVolume(%volume);
   $pref::SFX::masterVolume = %volume;
   
   if (!isObject($AudioTestHandle))
      $AudioTestHandle = sfxPlayOnce( ZeroChannel, expandFilename( "~/data/sound/testing.wav" ) );
}

function OptAudioUpdateChannelVolume(%description, %volume)
{
   %channel = %description.channel;
   
   if (%channel < 1 || %channel > 8)
      return;

   if (%volume == $pref::SFX::channelVolume[%channel])
      return;

   sfxSetChannelVolume(%channel, %volume);
   $pref::SFX::channelVolume[%channel] = %volume;
   
   if (!isObject($AudioTestHandle))
      $AudioTestHandle = sfxPlayOnce( %description, expandFilename( "~/data/sound/testing.wav" ) );
}

function OptAudioProviderList::onSelect( %this, %id, %text )
{
   // Skip empty provider selections.   
   if ( %text $= "" )
      return;
      
   $pref::SFX::provider = %text;
   OptAudioDeviceList.clear();
   
   %buffer = sfxGetAvailableDevices();
   %count = getRecordCount( %buffer );   
   for(%i = 0; %i < %count; %i++)
   {
      %record = getRecord(%buffer, %i);
      %provider = getField(%record, 0);
      %device = getField(%record, 1);
      
      if (%provider !$= %text)
         continue;
            
       if ( OptAudioDeviceList.findText( %device ) == -1 )
            OptAudioDeviceList.add( %device, %i );
   }

   // Find the previous selected device.
   %selId = OptAudioDeviceList.findText($pref::SFX::device);
   if ( %selId == -1 )
      OptAudioDeviceList.setFirstSelected();
   else
   OptAudioDeviceList.setSelected( %selId );
}

function OptAudioDeviceList::onSelect( %this, %id, %text )
{
   // Skip empty selections.
   if ( %text $= "" )
      return;
      
   $pref::SFX::device = %text;
   
   if ( !sfxCreateDevice(  $pref::SFX::provider, 
                           $pref::SFX::device, 
                           $pref::SFX::useHardware,
                           -1 ) )                              
      error( "Unable to create SFX device: " @ $pref::SFX::provider 
                                             SPC $pref::SFX::device 
                                             SPC $pref::SFX::useHardware );                                             
}

/*
function OptAudioHardwareToggle::onClick(%this)
{
   if (!sfxCreateDevice($pref::SFX::provider, $pref::SFX::device, $pref::SFX::useHardware, -1))
      error("Unable to create SFX device: " @ $pref::SFX::provider SPC $pref::SFX::device SPC $pref::SFX::useHardware);
}
*/
