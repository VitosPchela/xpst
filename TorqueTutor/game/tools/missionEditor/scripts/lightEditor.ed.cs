//-----------------------------------------------
// Copyright © Synapse Gaming 2004
// Written by John Kabus
//-----------------------------------------------

$lightEditor::profilePath = "common/ui/";

$lightEditor::profileScrollImage = $lightEditor::profilePath @ (($platform $= "macos") ? "osxScroll" : "darkScroll");
$lightEditor::profileCheckImage = $lightEditor::profilePath @ (($platform $= "macos") ? "osxCheck" : "torqueCheck");
$lightEditor::profileMenuImage = $lightEditor::profilePath @ (($platform $= "macos") ? "osxMenu" : "torqueMenu");

$lightEditor::profileSliderImage = $lightEditor::profilePath @ (($platform $= "macos") ? "darkSlider" : "darkSlider");


if(!isObject(LightEdTextProfile)) new GuiControlProfile (LightEdTextProfile)
{
   fontColor = "0 0 0";
   fontColorLink = "255 96 96";
   fontColorLinkHL = "0 0 255";
   fontSize = 12;
   autoSizeWidth = true;
   autoSizeHeight = true;
};

if(!isObject(lightEdGuiPopUpMenuProfile)) new GuiControlProfile (lightEdGuiPopUpMenuProfile)
{
   opaque = true;
   mouseOverSelected = true;

   border = 2;
   borderThickness = 2;
   borderColor = "0 0 0";
   fontSize = 12;
   fontColor = "0 0 0";
   fontColorHL = "32 100 100";
   fontColorSEL = "32 100 100";
   fixedExtent = true;
   justify = "center";
   bitmap = $lightEditor::profileScrollImage;
   hasBitmapArray = true;
};

if(!isObject(lightEdSliderProfile)) new GuiControlProfile (lightEdSliderProfile)
{
   opaque = true;
   mouseOverSelected = true;

   border = 4;
   borderThickness = 2;
   borderColor = "0 0 0";
   fontSize = 12;
   fontColor = "0 0 0";
   fontColorHL = "32 100 100";
   fontColorSEL = "32 100 100";
   fixedExtent = true;
   justify = "center";
   bitmap = $lightEditor::profileSliderImage;
   hasBitmapArray = true;
};

if(!isObject(lightEdCheckBoxProfile)) new GuiControlProfile (lightEdCheckBoxProfile)
{
   opaque = false;
   fillColor = "232 232 232";
   border = false;
   borderColor = "0 0 0";
   fontSize = 12;
   fontColor = "0 0 0";
   fontColorHL = "32 100 100";
   fixedExtent = true;
   justify = "left";
   bitmap = $lightEditor::profileCheckImage;
   hasBitmapArray = true;

};

if(!isObject(lightEdTextEditProfile)) new GuiControlProfile (lightEdTextEditProfile)
{
   opaque = true;
   fillColor = "255 255 255";
   fillColorHL = "128 128 128";
   border = 3;
   borderThickness = 2;
   borderColor = "0 0 0";
   fontColor = "0 0 0";
   fontColorHL = "255 255 255";
   fontColorNA = "128 128 128";
   fontSize = 12;
   textOffset = "0 2";
   autoSizeWidth = false;
   autoSizeHeight = true;
   tab = true;
   canKeyFocus = true;
};

if(!isObject(lightEdButtonProfile)) new GuiControlProfile (lightEdButtonProfile)
{
   opaque = true;
   border = true;
   fontColor = "0 0 0";
   fontColorHL = "32 100 100";
   fontSize = 12;
   fixedExtent = true;
   justify = "center";
	canKeyFocus = false;
};

if(!isObject(lightEdMenuBarProfile)) new GuiControlProfile (lightEdMenuBarProfile)
{
   opaque = true;
   //fillColor = ($platform $= "macos") ? "211 211 211" : "192 192 192";
   fillColorHL = "0 0 96";
   border = 1;
   fontColor = "0 0 0";
   fontColorHL = "255 255 255";
   fontColorNA = "128 128 128";
   fontSize = 12;
   fixedExtent = true;
   justify = "center";
   canKeyFocus = false;
   mouseOverSelected = true;
   bitmap = $lightEditor::profileMenuImage;
   hasBitmapArray = true;
};

lightEditor.activePanel = "";
lightEditorLightEditorPanel.currentDBName = "";
lightEditorCinematicFilterEditorPanel.currentDBName = "";

function lightEditorMenuBar::onMenuItemSelect(%this, %menuId, %menu, %itemId, %item)
{
   if(%this.scriptCommand[%menu, %itemId] !$= "")
      eval(%this.scriptCommand[%menu, %itemId]);
   else
      error("No script command defined for menu " @ %menu  @ " item " @ %item);
}

function lightEditor::setPanel(%this, %panel)
{
   for(%i=0; %i<%this.constants_panelCount; %i++)
   {
      %this.constants_panel[%i].visible = false;
   }

   %panel.visible = true;
   %this.activePanel = %panel;
   //%panel.setGui();
   
   //echo("trying to set the list item (should kick off the DB lookup)");
   echo(%this.activePanel.datablockType_list);
   echo(%this.activePanel.currentDBName);
   echo(%this.activePanel.datablockType_list.findText(%this.activePanel.currentDBName));
   %this.activePanel.datablockType_list.setSelected(%this.activePanel.datablockType_list.findText(%this.activePanel.currentDBName));
   
   //echo("what panel is this???");
   //echo(%panel);
   
   lightEditorWindow.text = "Torque Lighting Kit " @ %this.activePanel.datablockType_text;
}

function lightEditor::setLightEditorPanel(%this, %panel)
{
   for(%i=0; %i<%this.constants_lightEditor_panelCount; %i++)
   {
      %this.constants_lightEditor_panel[%i].visible = false;
   }

   %panel.visible = true;
}

function lightEditor::setFilterEditorPanel(%this, %panel)
{
   for(%i=0; %i<%this.constants_filterEditor_panelCount; %i++)
   {
      %this.constants_filterEditor_panel[%i].visible = false;
   }

   %panel.visible = true;
}

function lightEditor::findDataBlocks(%this, %path, %list, %panel)
{
   %list.clear();

   %file = findFirstFile(%path @ "*.cs");
   %i = 0;
  
   while(%file !$= "")
   {
      %file = fileBase(%file);
      %list.add(%file, %i);
      %file = findNextFile(%path @ "*.cs");
      %i++;
   }

   %list.sort();
   
   //if(%panel.currentDBName $= "")
   //   %list.setSelected(0);
   //else
   //   %list.setSelected(%list.findText(%panel.currentDBName));
}

function lightEditor::findLightingModels(%this)
{
   LightingModelList.clear();
   
   %count = lightingModelCount();
   //echo(%count);
   for(%i=0; %i<%count; %i++)
   {
      LightingModelList.add(lightingModelName(%i), %i);
   }
}

function lightEditor::resetLightAnimations(%this)
{
   %count = LightSet.getCount();
   //echo(%count);
   for(%i=0; %i<%count; %i++)
   {
      %light = LightSet.getObject(%i);
      if(isObject(%light))
         %light.reset();
   }
}

function lightEditor::onWake(%this)
{
   lightEditorLightEditorPanel.datablockType_name = "sgLightObjectData";
   lightEditorLightEditorPanel.datablockType_path = $lightEditor::lightDBPath;
   lightEditorLightEditorPanel.datablockType_list = LightDBList;
   lightEditorLightEditorPanel.datablockType_text = "Light Editor";
   
   lightEditorCinematicFilterEditorPanel.datablockType_name = "sgMissionLightingFilterData";
   lightEditorCinematicFilterEditorPanel.datablockType_path = $lightEditor::filterDBPath;
   lightEditorCinematicFilterEditorPanel.datablockType_list = FilterDBList;
   lightEditorCinematicFilterEditorPanel.datablockType_text = "Filter Editor";
   
   %this.constants_panel[0] = lightEditorLightEditorPanel;
   %this.constants_panel[1] = lightEditorCinematicFilterEditorPanel;
   %this.constants_panelCount = 2;
   
   %this.constants_lightEditor_panel[0] = lightEditorTypePanel;
   %this.constants_lightEditor_panel[1] = lightEditorColorPanel;
   %this.constants_lightEditor_panel[2] = lightEditorFlarePanel;
   %this.constants_lightEditor_panel[3] = lightEditorAnimationPanel;
   %this.constants_lightEditor_panelCount = 4;
   
   %this.constants_filterEditor_panel[0] = lightEditorCinematicFilterEditorColorPanel;
   %this.constants_filterEditor_panel[1] = lightEditorCinematicFilterEditorFilterPanel;
   %this.constants_filterEditor_panelCount = 2;
   
   %this.constants_menuName[0] = "File";
   %this.constants_menuName[1] = "Relight";
   %this.constants_menuName[2] = "Editor";
   %this.constants_menuName[3] = "Quality";
   
   %this.constants_menuId[%this.constants_menuName[0]] = 0;
   %this.constants_menuId[%this.constants_menuName[1]] = 1;
   %this.constants_menuId[%this.constants_menuName[2]] = 2;
   %this.constants_menuId[%this.constants_menuName[3]] = 3;
   
   %this.constants_quality_menuItemId[0] = 100;
   %this.constants_quality_menuItemId[1] = 101;
   %this.constants_quality_menuItemId[2] = 102;
   %this.constants_quality_menuItemId["shadows"] = 1000;
   %this.constants_quality_menuItemId["drl"] = 1001;
   %this.constants_quality_menuItemId["bloom"] = 1002;
   %this.constants_quality_menuItemId["tonemapping"] = 1003;

   lightClearFieldMapping(lightEditorLightEditorPanel);
   lightAddFieldMapping(lightEditorLightEditorPanel, "lightEditorEffectsDTSObjects", "EffectsDTSObjects");
   lightAddFieldMapping(lightEditorLightEditorPanel, "lightEditorSpotAngle", "SpotAngle");
   lightAddFieldMapping(lightEditorLightEditorPanel, "lightEditorSpotLight", "SpotLight");
   lightAddFieldMapping(lightEditorLightEditorPanel, "lightEditorIntensity", "Brightness");
   lightAddFieldMapping(lightEditorLightEditorPanel, "lightEditorAmbient", "LocalAmbientAmount");
   lightAddFieldMapping(lightEditorLightEditorPanel, "lightEditorStaticLight", "StaticLight");
   lightAddFieldMapping(lightEditorLightEditorPanel, "lightEditorLightOn", "LightOn");
   lightAddFieldMapping(lightEditorLightEditorPanel, "lightEditorShadows", "CastsShadows");
   lightAddFieldMapping(lightEditorLightEditorPanel, "lightEditorRestrictZoneDiffuse", "DiffuseRestrictZone");
   lightAddFieldMapping(lightEditorLightEditorPanel, "lightEditorRestrictZoneAmbient", "AmbientRestrictZone");
   lightAddFieldMapping(lightEditorLightEditorPanel, "lightEditorSmoothing", "SmoothSpotLight");
   lightAddFieldMapping(lightEditorLightEditorPanel, "lightEditorTwoSidedAmbient", "DoubleSidedAmbient");
   lightAddFieldMapping(lightEditorLightEditorPanel, "lightEditorRadius", "Radius");

   lightAddFieldMapping(lightEditorLightEditorPanel, "lightEditorFlareConstantSize", "ConstantSizeOn");
   lightAddFieldMapping(lightEditorLightEditorPanel, "lightEditorFlareOn", "FlareOn");
   lightAddFieldMapping(lightEditorLightEditorPanel, "lightEditorFlareBlendMode", "BlendMode");
   lightAddFieldMapping(lightEditorLightEditorPanel, "lightEditorFlareFileName", "FlareBitmap");
   lightAddFieldMapping(lightEditorLightEditorPanel, "lightEditorFlareSizeNear", "NearSize");
   lightAddFieldMapping(lightEditorLightEditorPanel, "lightEditorFlareSizeFar", "FarSize");
   lightAddFieldMapping(lightEditorLightEditorPanel, "lightEditorFlareDistanceNear", "NearDistance");
   lightAddFieldMapping(lightEditorLightEditorPanel, "lightEditorFlareDistanceFar", "FarDistance");
   lightAddFieldMapping(lightEditorLightEditorPanel, "lightEditorFlareFadeTime", "FadeTime");
   lightAddFieldMapping(lightEditorLightEditorPanel, "lightEditorAnimationRadiusOn", "AnimRadius");
   lightAddFieldMapping(lightEditorLightEditorPanel, "lightEditorAnimationColorOn", "AnimColour");
   lightAddFieldMapping(lightEditorLightEditorPanel, "lightEditorAnimationIntensityOn", "AnimBrightness");
   lightAddFieldMapping(lightEditorLightEditorPanel, "lightEditorAnimationIntensity2Time", "BrightnessTime");
   lightAddFieldMapping(lightEditorLightEditorPanel, "lightEditorAnimationRadius2Time", "RadiusTime");
   lightAddFieldMapping(lightEditorLightEditorPanel, "lightEditorAnimationRadius2", "MinRadius");
   lightAddFieldMapping(lightEditorLightEditorPanel, "lightEditorAnimationIntensity2", "MinBrightness");
   lightAddFieldMapping(lightEditorLightEditorPanel, "lightEditorAnimationColor2Time", "ColourTime");

   lightEditorStaticLight.fieldDependency = lightEditorLightOn;
   lightEditorRestrictZoneDiffuse.fieldDependency = lightEditorLightOn;
   lightEditorIntensity.fieldDependency = lightEditorLightOn;
   lightEditorRadius.fieldDependency = lightEditorLightOn;
   lightEditorFlareOn.fieldDependency = lightEditorLightOn;
   lightEditorAnimationRadiusOn.fieldDependency = lightEditorLightOn;
   lightEditorAnimationColorOn.fieldDependency = lightEditorLightOn;
   lightEditorAnimationIntensityOn.fieldDependency = lightEditorLightOn;
   lightEditorTwoSidedAmbient.fieldDependency = lightEditorLightOn;
   lightEditorShadows.fieldDependency = lightEditorLightOn;
   lightEditorSpotLight.fieldDependency = lightEditorLightOn;

   lightEditorRestrictZoneAmbient.fieldDependency = lightEditorStaticLight;
   lightEditorEffectsDTSObjects.fieldDependency = lightEditorStaticLight;
   lightEditorAmbient.fieldDependency = lightEditorStaticLight;

   lightEditorSmoothing.fieldDependency = lightEditorSpotLight;
   lightEditorSpotAngle.fieldDependency = lightEditorSpotLight;

   lightEditorFlareConstantSize.fieldDependency = lightEditorFlareOn;
   lightEditorFlareBlendMode.fieldDependency = lightEditorFlareOn;
   lightEditorFlareFileName.fieldDependency = lightEditorFlareOn;
   lightEditorFlareSizeNear.fieldDependency = lightEditorFlareOn;
   lightEditorFlareSizeFar.fieldDependency = lightEditorFlareOn;
   lightEditorFlareDistanceNear.fieldDependency = lightEditorFlareOn;
   lightEditorFlareDistanceFar.fieldDependency = lightEditorFlareOn;
   lightEditorFlareFadeTime.fieldDependency = lightEditorFlareOn;

   lightEditorAnimationRadius2.fieldDependency = lightEditorAnimationRadiusOn;
   lightEditorAnimationRadius2Time.fieldDependency = lightEditorAnimationRadiusOn;

   lightEditorAnimationIntensity2.fieldDependency = lightEditorAnimationIntensityOn;
   lightEditorAnimationIntensity2Time.fieldDependency = lightEditorAnimationIntensityOn;

   lightEditorAnimationColor2Time.fieldDependency = lightEditorAnimationColorOn;
   
   lightClearFieldMapping(lightEditorCinematicFilterEditorPanel);
   lightAddFieldMapping(lightEditorCinematicFilterEditorPanel, "lightEditorCinematicFilterEditorIntensity", "LightingIntensity");
   lightAddFieldMapping(lightEditorCinematicFilterEditorPanel, "lightEditorCinematicFilterEditorFilterOn", "CinematicFilter");
   lightAddFieldMapping(lightEditorCinematicFilterEditorPanel, "lightEditorCinematicFilterEditorFilterAmount", "CinematicFilterAmount");
   lightAddFieldMapping(lightEditorCinematicFilterEditorPanel, "lightEditorCinematicFilterEditorFilterIntensity", "CinematicFilterReferenceIntensity");
   
   lightEditorCinematicFilterEditorFilterAmount.fieldDependency = lightEditorCinematicFilterEditorFilterOn;
   lightEditorCinematicFilterEditorFilterIntensity.fieldDependency = lightEditorCinematicFilterEditorFilterOn;

   lightEditorMenuBar.clearMenus();
   
   %menuname = %this.constants_menuName[0];
   %menuid = %this.constants_menuId[%menuname];

   lightEditorMenuBar.addMenu(%menuname, %menuid);
   %i = 1;
   lightEditorMenuBar.addMenuItem(%menuname, "New...", %i);
   lightEditorMenuBar.scriptCommand[%menuname, %i] = "lightEditor.newDB(false);";
   %i = %i + 1;
   lightEditorMenuBar.addMenuItem(%menuname, "Clone...", %i);
   lightEditorMenuBar.scriptCommand[%menuname, %i] = "lightEditor.newDB(true);";
   lightEditorMenuBar.addMenuItem(%menuname, "-", 0);
   %i = %i + 1;
   lightEditorMenuBar.addMenuItem(%menuname, "Save", %i);
   lightEditorMenuBar.scriptCommand[%menuname, %i] = "lightEditor.saveDB();";
   %i = %i + 1;
   lightEditorMenuBar.addMenuItem(%menuname, "Restore", %i);
   lightEditorMenuBar.scriptCommand[%menuname, %i] = "lightEditor.restoreDB();";
   lightEditorMenuBar.addMenuItem(%menuname, "-", 0);
   %i = %i + 1;
   lightEditorMenuBar.addMenuItem(%menuname, "Close", %i, "F12");
   lightEditorMenuBar.scriptCommand[%menuname, %i] = "lightEditor.toggle();";
   
   %menuname = %this.constants_menuName[2];
   %menuid = %this.constants_menuId[%menuname];

   lightEditorMenuBar.addMenu(%menuname, %menuid);
   %i = 1;
   lightEditorMenuBar.addMenuItem(%menuname, "Light Editor", %i);
   lightEditorMenuBar.scriptCommand[%menuname, %i] = "lightEditor.setPanel(lightEditorLightEditorPanel);";
   %i = %i + 1;
   lightEditorMenuBar.addMenuItem(%menuname, "Cinematic Filter Editor", %i);
   lightEditorMenuBar.scriptCommand[%menuname, %i] = "lightEditor.setPanel(lightEditorCinematicFilterEditorPanel);";

   %menuname = %this.constants_menuName[3];
   %menuid = %this.constants_menuId[%menuname];

   lightEditorMenuBar.addMenu(%menuname, %menuid);
   %i = %this.constants_quality_menuItemId[0];
   lightEditorMenuBar.addMenuItem(%menuname, "Production", %i, "", 1);
   lightEditorMenuBar.scriptCommand[%menuname, %i] = "lightEditor.setQuality(0);";
   %i = %this.constants_quality_menuItemId[1];
   lightEditorMenuBar.addMenuItem(%menuname, "Design", %i, "", 1);
   lightEditorMenuBar.scriptCommand[%menuname, %i] = "lightEditor.setQuality(1);";
   %i = %this.constants_quality_menuItemId[2];
   lightEditorMenuBar.addMenuItem(%menuname, "Draft", %i, "", 1);
   lightEditorMenuBar.scriptCommand[%menuname, %i] = "lightEditor.setQuality(2);";
   lightEditorMenuBar.addMenuItem(%menuname, "-", 0);
   %i = %this.constants_quality_menuItemId["shadows"];
   lightEditorMenuBar.addMenuItem(%menuname, "Shadows", %i, "", 2);
   lightEditorMenuBar.scriptCommand[%menuname, %i] = "lightEditor.setShadows(!$pref::LightManager::LightingProfileAllowShadows);";
   
   //lightEditorMenuBar.addMenuItem(%menuname, "-", 0);
   //%i = %this.constants_quality_menuItemId["drl"];
   //lightEditorMenuBar.addMenuItem(%menuname, "Dynamic Range Lighting", %i, "", 3);
   //lightEditorMenuBar.scriptCommand[%menuname, %i] = "lightEditor.setDRL(!$pref::LightManager::UseDynamicRangeLighting);";
   //%i = %this.constants_quality_menuItemId["bloom"];
   //lightEditorMenuBar.addMenuItem(%menuname, "Bloom", %i, "", 4);
   //lightEditorMenuBar.scriptCommand[%menuname, %i] = "lightEditor.setBloom(!$pref::LightManager::UseBloom);";
   //%i = %this.constants_quality_menuItemId["tonemapping"];
   //lightEditorMenuBar.addMenuItem(%menuname, "Full Screen Cinematic Filtering", %i, "", 5);
   //lightEditorMenuBar.scriptCommand[%menuname, %i] = "lightEditor.setToneMapping(!$pref::LightManager::UseToneMapping);";

   %menuname = %this.constants_menuName[1];
   %menuid = %this.constants_menuId[%menuname];

   lightEditorMenuBar.addMenu(%menuname, %menuid);
   %i = 1;
   lightEditorMenuBar.addMenuItem(%menuname, "Filtered Relight", %i);
   lightEditorMenuBar.scriptCommand[%menuname, %i] = "lightEditor.filteredRelight();";
   //lightEditorMenuBar.addMenuItem(%menuname, "-", 0);
   %i = %i + 1;
   lightEditorMenuBar.addMenuItem(%menuname, "Full Relight", %i);
   lightEditorMenuBar.scriptCommand[%menuname, %i] = "Editor.lightScene(\"filteredRelightComplete();\", forceAlways);";
   lightEditorMenuBar.addMenuItem(%menuname, "-", 0);
   %i = %i + 1;
   lightEditorMenuBar.addMenuItem(%menuname, "Reset All Light Animation", %i);
   lightEditorMenuBar.scriptCommand[%menuname, %i] = "lightEditor.resetLightAnimations();";

   // load first!
   %this.findLightingModels();
   %this.findDataBlocks($lightEditor::lightDBPath, LightDBList, lightEditorLightEditorPanel);
   %this.findDataBlocks($lightEditor::filterDBPath, FilterDBList, lightEditorCinematicFilterEditorPanel);

   // set the active panel next...
   echo("onwake state");
   echo(%this.activePanel);
   if(!isObject(%this.activePanel))
      %this.setPanel(lightEditorLightEditorPanel);
   else
      %this.setPanel(%this.activePanel);
   
   //echo("calling activePanel.setGUI();");
   //%this.activePanel.setGui();
   //echo("done");
   
   lightEditorTypeButton.performClick();
   lightEditorCinematicFilterEditorColorButton.performClick();
   
   $SceneLighting::FilterRelightVisible = true;
   
   %this.setQuality($pref::LightManager::LightingProfileQuality);
   %this.setShadows($pref::LightManager::LightingProfileAllowShadows);
   %this.setDRL($pref::LightManager::UseDynamicRangeLighting);
   %this.setBloom($pref::LightManager::UseBloom);
   %this.setToneMapping($pref::LightManager::UseToneMapping);
}

function lightEditor::onSleep(%this)
{
   %this.state = 0;

   $SceneLighting::FilterRelightVisible = false;
}

function lightEditor::toggle(%this)
{
   //echo("lightEditor::toggle()");
   if(%this.state == 0)
   {
      canvas.pushdialog(lightEditor);
      %this.state = 1;
   }
   else
   {
      canvas.popdialog(lightEditor);
      %this.state = 0;
   }
}


//-----------------------------------------------
// datablock streaming
//-----------------------------------------------

function lightEditor::newDB(%this, %clone)
{
   lightEditorNewDB.clone = %clone;
   Canvas.pushDialog(lightEditorNewDB);
}

function lightEditor::createDB(%this, %name)
{
   %path = %this.activePanel.datablockType_path @ %name @ ".cs";
   
   if(!isFile(%path))
   {
      %file = new FileObject();
      %file.openForWrite(%path);
      %file.writeLine("//--- OBJECT WRITE BEGIN ---");
      %file.writeLine("datablock " @ %this.activePanel.datablockType_name @ "(" @ %name @ ") {");
      %file.writeLine("className = \"" @ %this.activePanel.datablockType_name @ "\";");
      %file.writeLine("};");
      %file.writeLine("//--- OBJECT WRITE END ---");
      %file.delete();
   
      exec(%path);
      
      %this.onWake();
   }
   
   echo("Creating new db...");
   echo(%this.activePanel);
   echo(%this.activePanel.datablockType_list);
   echo(%name);
   echo("Done.");
   
   %this.activePanel.datablockType_list.setSelected(%this.activePanel.datablockType_list.findText(%name));
}

function lightEditor::cloneDB(%this, %name)
{
   %path = %this.activePanel.datablockType_path @ %name @ ".cs";
   %this.activePanel.currentDB.save(%path);
   
   //update file to define datablock...
   %file = new FileObject();
   %file.openForRead(%path);
   %line = "";
   while(!%file.isEOF())
      %line = %line @ %file.readLine() @ "\n";
   %file.delete();
   
   %line = strreplace(%line, "//--- OBJECT WRITE BEGIN ---\nnew", "//--- OBJECT WRITE BEGIN ---\ndatablock");
   %line = strreplace(%line, %this.activePanel.currentDBName, %name);
   
   %file = new FileObject();
   %file.openForWrite(%path);
   %file.writeLine(%line);
   %file.delete();
   
   exec(%path);
   
   %this.onWake();
   %this.activePanel.datablockType_list.setSelected(%this.activePanel.datablockType_list.findText(%name));
}

function lightEditor::saveDB(%this)
{
   %path = %this.activePanel.datablockType_path @ %this.activePanel.currentDBName @ ".cs";
   %this.activePanel.currentDB.save(%path);
   
   //update file to define datablock...
   %file = new FileObject();
   %file.openForRead(%path);
   %line = "";
   while(!%file.isEOF())
      %line = %line @ %file.readLine() @ "\n";
   %file.delete();
   
   %line = strreplace(%line, "//--- OBJECT WRITE BEGIN ---\nnew", "//--- OBJECT WRITE BEGIN ---\ndatablock");
   
   //%line = strreplace(%line, "sgUniversalStaticLightData", "sgLightObjectData");
   
   %file = new FileObject();
   %file.openForWrite(%path);
   %file.writeLine(%line);
   %file.delete();
}

function lightEditor::restoreDB(%this)
{
   %path = %this.activePanel.datablockType_path @ %this.activePanel.currentDBName @ ".cs";
   exec(%path);
   %this.activePanel.setGui();
}


//-----------------------------------------------
// Synapse Gaming GUI Framework
//-----------------------------------------------

function lightClearFieldMapping(%obj)
{
   %obj.lightFieldCount = 0;
}

function lightAddFieldMapping(%obj, %windowfield, %datablockfield)
{
   %obj.WindowField[%obj.lightFieldCount] = %windowfield;
   %obj.DatablockField[%obj.lightFieldCount] = %datablockfield;
   %obj.lightFieldCount = %obj.lightFieldCount + 1;

   %windowfield.fieldDependency = "";
}

function lightProcessFields(%obj)
{
   //echo("lightProcessFields");
   //echo(%obj.lightFieldCount);

   for(%i=0; %i<%obj.lightFieldCount; %i++)
   {
      lightProcessField(%obj, %obj.WindowField[%i]);
   }
}

function lightProcessField(%obj, %field)
{
   //echo("lightEditor::processField");
   //echo(%field);

   %dep = %field.fieldDependency;
   if(!isObject(%dep))
      return;

   //echo("Found dependency...");

   lightProcessField(%obj, %dep);

   if(%dep.isActive() && (%dep.getValue() $= "1"))
   {
      %field.setActive(true);
	  //echo("Setting " @ %field @ " active.");
	}
   else
   {
      %field.setActive(false);
	  //echo("Setting " @ %field @ " inactive.");
	}
}


//-----------------------------------------------
// panel specific code
//-----------------------------------------------

function lightEditorLightEditorPanel::setDB(%this)
{
   %color = lightEditorColorRed.value @ " " @
            lightEditorColorGreen.value @ " " @
            lightEditorColorBlue.value;
   %this.currentDB.Colour = %color;
   %this.currentDB.MaxColour = %color;

   %color = lightEditorAnimationColor2Red.value @ " " @
            lightEditorAnimationColor2Green.value @ " " @
            lightEditorAnimationColor2Blue.value;
   %this.currentDB.MinColour = %color;

   //echo(%this.lightFieldCount);

   for(%i=0; %i<%this.lightFieldCount; %i++)
   {
      %cmd = "%this.currentDB." @ %this.DatablockField[%i] @
         " = " @ %this.WindowField[%i] @ ".getValue();";
      //echo(%cmd);
      eval(%cmd);
   }

   %this.currentDB.ConstantSize = %this.currentDB.NearSize;
   %this.currentDB.MaxBrightness = %this.currentDB.Brightness;
   %this.currentDB.MaxRadius = %this.currentDB.Radius;

   %this.currentDB.LightingModelName = LightingModelList.getText();

   lightProcessFields(%this);
}

function lightEditorLightEditorPanel::setGui(%this)
{
   //echo("what db is this???");
   //echo(%this.currentDB);
   //echo(%this.currentDBName);
   
   %r = getWord(%this.currentDB.Colour, 0);
   %g = getWord(%this.currentDB.Colour, 1);
   %b = getWord(%this.currentDB.Colour, 2);
   lightEditorColorRed.setValue(%r);
   lightEditorColorGreen.setValue(%g);
   lightEditorColorBlue.setValue(%b);

   %r = getWord(%this.currentDB.MinColour, 0);
   %g = getWord(%this.currentDB.MinColour, 1);
   %b = getWord(%this.currentDB.MinColour, 2);
   lightEditorAnimationColor2Red.setValue(%r);
   lightEditorAnimationColor2Green.setValue(%g);
   lightEditorAnimationColor2Blue.setValue(%b);

   //echo(%this.lightFieldCount);

   for(%i=0; %i<%this.lightFieldCount; %i++)
   {
      %cmd = %this.WindowField[%i] @ ".setValue(%this.currentDB." @
         %this.DatablockField[%i] @ ");";
      //echo(%cmd);
      eval(%cmd);
   }
   
   LightingModelList.setSelected(LightingModelList.findText(%this.currentDB.LightingModelName));

   lightProcessFields(%this);
}

function lightEditorCinematicFilterEditorPanel::setDB(%this)
{
   %color = lightEditorCinematicFilterEditorColorRed.value @ " " @
            lightEditorCinematicFilterEditorColorGreen.value @ " " @
            lightEditorCinematicFilterEditorColorBlue.value;
   %this.currentDB.LightingFilter = %color;
   
   %color = lightEditorCinematicFilterEditorFilterColorRed.value @ " " @
            lightEditorCinematicFilterEditorFilterColorGreen.value @ " " @
            lightEditorCinematicFilterEditorFilterColorBlue.value;
   %this.currentDB.CinematicFilterReferenceColor = %color;

   //echo(%this.lightFieldCount);

   for(%i=0; %i<%this.lightFieldCount; %i++)
   {
      %cmd = "%this.currentDB." @ %this.DatablockField[%i] @
         " = " @ %this.WindowField[%i] @ ".getValue();";
      //echo(%cmd);
      eval(%cmd);
   }

   lightProcessFields(%this);
}

function lightEditorCinematicFilterEditorPanel::setGui(%this)
{
   %r = getWord(%this.currentDB.LightingFilter, 0);
   %g = getWord(%this.currentDB.LightingFilter, 1);
   %b = getWord(%this.currentDB.LightingFilter, 2);
   lightEditorCinematicFilterEditorColorRed.setValue(%r);
   lightEditorCinematicFilterEditorColorGreen.setValue(%g);
   lightEditorCinematicFilterEditorColorBlue.setValue(%b);
   
   %r = getWord(%this.currentDB.CinematicFilterReferenceColor, 0);
   %g = getWord(%this.currentDB.CinematicFilterReferenceColor, 1);
   %b = getWord(%this.currentDB.CinematicFilterReferenceColor, 2);
   lightEditorCinematicFilterEditorFilterColorRed.setValue(%r);
   lightEditorCinematicFilterEditorFilterColorGreen.setValue(%g);
   lightEditorCinematicFilterEditorFilterColorBlue.setValue(%b);

   //echo(%this.lightFieldCount);

   for(%i=0; %i<%this.lightFieldCount; %i++)
   {
      %cmd = %this.WindowField[%i] @ ".setValue(%this.currentDB." @
         %this.DatablockField[%i] @ ");";
      //echo(%cmd);
      eval(%cmd);
   }

   lightProcessFields(%this);
}


//-----------------------------------------------
// datablock id info
//-----------------------------------------------

function clientCmdGetLightDBIdCallback(%id)
{
   echo("clientCmdGetLightDBIdCallback");
   lightEditor.activePanel.currentDB = %id;
   lightEditor.activePanel.setGui();
}

function LightDBList::onSelect(%this, %id, %text)
{
   if(lightEditor.activePanel !$= "lightEditorLightEditorPanel")
   {
      error("LightDBList::onSelect> Error Light Editor panel not set.");
	  return;
   }
   
   lightEditor.activePanel.currentDBName = LightDBList.getText();  
   //commandToServer('GetLightDBId', lightEditor.activePanel.currentDBName);
   //this never used to work...
   lightEditor.activePanel.currentDB = nameToId(lightEditor.activePanel.currentDBName);
   //echo(lightEditor.activePanel.currentDBName);
   //echo(lightEditor.activePanel.currentDB);
   lightEditor.activePanel.setGui();
}

function FilterDBList::onSelect(%this, %id, %text)
{
   if(lightEditor.activePanel !$= "lightEditorCinematicFilterEditorPanel")
   {
      error("FilterDBList::onSelect> Error Filter Editor panel not set.");
	  return;
   }
   
   lightEditor.activePanel.currentDBName = FilterDBList.getText();  
   //commandToServer('GetLightDBId', lightEditor.activePanel.currentDBName);
   lightEditor.activePanel.currentDB = nameToId(lightEditor.activePanel.currentDBName);
   //echo(lightEditor.activePanel.currentDBName);
   //echo(lightEditor.activePanel.currentDB);
   lightEditor.activePanel.setGui();
}


//-----------------------------------------------
// filtered relight
//-----------------------------------------------

function lightEditor::filteredRelight(%this)
{
   $SceneLighting::FilterRelight = "true";
   $SceneLighting::FilterRelightByDistance = "true";
   Editor.lightScene("filteredRelightComplete();", forceAlways);
}

function filteredRelightComplete()
{
   $SceneLighting::FilterRelight = "false";
}

function lightEditor::setQuality(%this, %qual)
{
   $pref::LightManager::LightingProfileQuality = %qual;
   %menuid = %this.constants_quality_menuItemId[%qual];
   lightEditorMenuBar.setMenuItemChecked(%this.constants_menuId[%this.constants_menuName[3]], %menuid, true);
}

function lightEditor::setShadows(%this, %value)
{
   $pref::LightManager::LightingProfileAllowShadows = %value;
   %menuid = %this.constants_quality_menuItemId["shadows"];
   lightEditorMenuBar.setMenuItemChecked(%this.constants_menuId[%this.constants_menuName[3]], %menuid, $pref::LightManager::LightingProfileAllowShadows);
}

function lightEditor::setDRL(%this, %value)
{
   $pref::LightManager::UseDynamicRangeLighting = %value;
   %menuid = %this.constants_quality_menuItemId["drl"];
   lightEditorMenuBar.setMenuItemChecked(%this.constants_menuId[%this.constants_menuName[3]], %menuid, %value);
}

function lightEditor::setBloom(%this, %value)
{
   $pref::LightManager::UseBloom = %value;
   %menuid = %this.constants_quality_menuItemId["bloom"];
   lightEditorMenuBar.setMenuItemChecked(%this.constants_menuId[%this.constants_menuName[3]], %menuid, %value);
}

function lightEditor::setToneMapping(%this, %value)
{
   $pref::LightManager::UseToneMapping = %value;
   %menuid = %this.constants_quality_menuItemId["tonemapping"];
   lightEditorMenuBar.setMenuItemChecked(%this.constants_menuId[%this.constants_menuName[3]], %menuid, %value);
}
