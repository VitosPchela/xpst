//-----------------------------------------------------------------------------
// Torque Game Engine
// Copyright (C) GarageGames.com, Inc.
//-----------------------------------------------------------------------------

function EditorGui::buildMenus(%this)
{
   if(isObject(%this.menuBar))
      return;
   
   // Sub menus (temporary, until MenuBuilder gets updated)
   %this.cameraSpeedMenu = new PopupMenu()
   {
      superClass = "MenuBuilder";
      class = "EditorCameraSpeedMenu";

      item[0] = "Slowest" TAB "Ctrl-Shift 1";
      item[1] = "Very Slow" TAB "Ctrl-Shift 2";
      item[2] = "Slow" TAB "Ctrl-Shift 3";
      item[3] = "Medium Pace" TAB "Ctrl-Shift 4";
      item[4] = "Fast" TAB "Ctrl-Shift 5";
      item[5] = "Very Fast" TAB "Ctrl-Shift 6";
      item[6] = "Fastest" TAB "Ctrl-Shift 7";
   };
   
   // Menu bar
   %this.menuBar = new MenuBar()
   {
      dynamicItemInsertPos = 3;
      
      // File Menu
      new PopupMenu()
      {
         superClass = "MenuBuilder";
         class = "EditorFileMenu";

         barTitle = "File";
         
         item[0] = "New Mission" TAB "" TAB "EditorNewMission();";
         item[1] = "Open Mission ..." TAB "Ctrl O" TAB "EditorOpenMission();";
         item[2] = "Save Mission" TAB "Ctrl S" TAB "EditorSaveMissionMenu();";
         item[3] = "Save Mission As ..." TAB "" TAB "EditorSaveMissionAs();";
         item[4] = "-";
         item[5] = "Import Terraform Data ..." TAB "" TAB "Heightfield::import();";
         item[6] = "Import Texture Data ..." TAB "" TAB "Texture::import();";
         item[7] = "-";
         item[8] = "Export Terraform Data ..." TAB "" TAB "Heightfield::saveBitmap(\"\");";
         item[9] = "-";
         item[10] = "Toggle Map Editor" TAB "F11" TAB "Editor.close(\"PlayGui\");";
         item[11] = "Quit" TAB "" TAB "EditorQuitMission();";
      };
      
      // Edit Menu
      new PopupMenu()
      {
         superClass = "MenuBuilder";
         class = "EditorEditMenu";

         barTitle = "Edit";
         
         item[0] = "Undo" TAB "Ctrl Z" TAB "EditorMenuEditUndo();";
         item[1] = "Redo" TAB "Ctrl Y" TAB "EditorMenuEditRedo();";
         item[2] = "-";
         item[3] = "Cut" TAB "Ctrl X" TAB "EditorMenuEditCut();";
         item[4] = "Copy" TAB "Ctrl C" TAB "EditorMenuEditCopy();";
         item[5] = "Paste" TAB "Ctrl V" TAB "EditorMenuEditPaste();";
         item[6] = "-";
         //item[7] = "Select All" TAB "Ctrl A" TAB "EditorMenuEditSelectAll();";
         //item[8] = "Select None" TAB "Ctrl N" TAB "EditorMenuEditSelectNone();";
         //item[9] = "-";
         item[7] = "World Editor Settings ..." TAB "" TAB "Canvas.pushDialog(WorldEditorSettingsDlg);";
         item[8] = "Terrain Editor Settings ..." TAB "" TAB "Canvas.pushDialog(TerrainEditorValuesSettingsGui, 99);";
      };
      
      // Camera Menu
      new PopupMenu()
      {
         superClass = "MenuBuilder";
         class = "EditorCameraMenu";

         barTitle = "Camera";
         
         item[0] = "Drop Camera at Player" TAB "Alt Q" TAB "commandToServer('dropCameraAtPlayer');";
         item[1] = "Drop Player at Camera" TAB "Alt W" TAB "commandToServer('DropPlayerAtCamera');";
         item[2] = "-";
         item[3] = "Toggle Camera" TAB "Alt C" TAB "commandToServer('ToggleCamera');";
         item[4] = "-";
         item[5] = "Speed" TAB %this.cameraSpeedMenu;
      };
      
      // Window Menu
      new PopupMenu()
      {
         superClass = "MenuBuilder";
         class = "EditorWindowMenu";

         barTitle = "Window";
         
         item[0] = "World Editor" TAB "F2";
         item[1] = "World Editor Inspector" TAB "F3";
         item[2] = "World Editor Creator" TAB "F4";
         item[3] = "Mission Area Editor" TAB "F5";
         item[4] = "-";
         item[5] = "Terrain Editor" TAB "F6";
         item[6] = "Terrain Texture Editor" TAB "F7";
         item[7] = "Terrain Painter" TAB "F8";
         item[8] = "Terrain Terraform Editor" TAB "F9";
      };
      
      // Lighting Menu
      new PopupMenu()
      {
         superClass = "MenuBuilder";
         class = "EditorLightingMenu";

         barTitle = "Lighting";
         
         item[0] = "Light Editor" TAB "Alt G" TAB "lightEditor.toggle();";
         //item[1] = "DRL Editor" TAB "" TAB "";
         item[1] = "-";
         item[2] = "Filtered Relight" TAB "Alt F" TAB "lightEditor.filteredRelight();";
         item[3] = "Full Relight" TAB "Alt L" TAB "Editor.lightScene(\"\", forceAlways);";
      };
   };
   
   // Menus that are added/removed dynamically (temporary)
   
   // World Menu
   if(! isObject(%this.worldMenu))
   {
      %this.dropTypeMenu = new PopupMenu()
      {
         superClass = "MenuBuilder";
         class = "EditorDropTypeMenu";

         // The onSelectItem() callback for this menu re-purposes the command field
         // as the MenuBuilder version is not used.
         item[0] = "Drop at Origin" TAB "" TAB "atOrigin";
         item[1] = "Drop at Camera" TAB "" TAB "atCamera";
         item[2] = "Drop at Camera w/Rot" TAB "" TAB "atCameraRot";
         item[3] = "Drop below Camera" TAB "" TAB "belowCamera";
         item[4] = "Drop at Screen Center" TAB "" TAB "screenCenter";
         item[5] = "Drop at Centroid" TAB "" TAB "atCentroid";
         item[6] = "Drop to Ground" TAB "" TAB "toGround";
      };
      
      %this.alignBoundsMenu = new PopupMenu()
      {
         superClass = "MenuBuilder";
         class = "EditorAlignBoundsMenu";

         // The onSelectItem() callback for this menu re-purposes the command field
         // as the MenuBuilder version is not used.
         item[0] = "+X" TAB "" TAB "0";
         item[1] = "+Y" TAB "" TAB "1";
         item[2] = "+Z" TAB "" TAB "2";
         item[3] = "-X" TAB "" TAB "3";
         item[4] = "-Y" TAB "" TAB "4";
         item[5] = "-Z" TAB "" TAB "5";
      };
      
      %this.alignCenterMenu = new PopupMenu()
      {
         superClass = "MenuBuilder";
         class = "EditorAlignCenterMenu";

         // The onSelectItem() callback for this menu re-purposes the command field
         // as the MenuBuilder version is not used.
         item[0] = "X Axis" TAB "" TAB "0";
         item[1] = "Y Axis" TAB "" TAB "1";
         item[2] = "Z Axis" TAB "" TAB "2";
      };
      
      %this.worldMenu = new PopupMenu()
      {
         superClass = "MenuBuilder";
         class = "EditorWorldMenu";

         barTitle = "World";
         
         item[0] = "Lock Selection" TAB "Ctrl L" TAB "EWorldEditor.lockSelection(true);";
         item[1] = "Unlock Selection" TAB "Ctrl-Shift L" TAB "EWorldEditor.lockSelection(false);";
         item[2] = "-";
         item[3] = "Hide Selection" TAB "Ctrl H" TAB "EWorldEditor.hideSelection(true);";
         item[4] = "Show Selection" TAB "Ctrl-Shift H" TAB "EWorldEditor.hideSelection(false);";
         item[5] = "-";
         item[6] = "Align Bounds" TAB %this.alignBoundsMenu;
         item[7] = "Align Center" TAB %this.alignCenterMenu;
         item[8] = "-";
         item[9] = "Reset Selected Rotation" TAB "" TAB "EWorldEditor.resetSelectedRotation();";
         item[10] = "Reset Selected Scale" TAB "" TAB "EWorldEditor.resetSelectedScale();";
         item[11] = "-";
         item[12] = "Delete Selection" TAB "Delete" TAB "EditorTree.deleteSelection(); EWorldEditor.clearSelection();Inspector.uninspect();";
         item[13] = "Camera to Selection" TAB "Ctrl Q" TAB "EWorldEditor.dropCameraToSelection();";
         item[14] = "Reset Transforms" TAB "Ctrl R" TAB "EWorldEditor.resetTransforms();";
         item[15] = "Add Selection to Instant Group" TAB "" TAB "EWorldEditor.addSelectionToAddGroup();";
         item[16] = "Drop Selection" TAB "Ctrl D" TAB "EWorldEditor.dropSelection();";
         item[17] = "-";
         item[18] = "Drop Location" TAB %this.dropTypeMenu;
      };
   }

   // Action Menu
   %m++;
   if(! isObject(%this.actionMenu))
   {
      %this.actionMenu = new PopupMenu()
      {
         superClass = "MenuBuilder";
         class = "EditorActionMenu";

         barTitle = "Action";
         
         item[0] = "Select" TAB "" TAB "";
         item[1] = "Deselect" TAB "" TAB "";
         item[2] = "Adjust Selection" TAB "" TAB "";
         item[3] = "Clear Selection" TAB "" TAB "";
         item[4] = "-";
         item[5] = "Add Dirt" TAB "" TAB "";
         item[6] = "Excavate" TAB "" TAB "";
         item[7] = "Adjust Height" TAB "Ctrl 1" TAB "";
         item[8] = "Flatten" TAB "" TAB "";
         item[9] = "Smooth" TAB "" TAB "";
         item[10] = "Set Height" TAB "" TAB "";
         item[11] = "-";
         item[12] = "Set Empty" TAB "" TAB "";
         item[13] = "Clear Empty" TAB "" TAB "";
         item[14] = "-";
         item[15] = "Paint Material" TAB "Ctrl 2" TAB "";
         item[16] = "Clear Materials" TAB "" TAB "";         
      };
   }

   // Brush Menu
   %m++;
   if(! isObject(%this.brushMenu))
   {
      %this.brushSizeMenu = new PopupMenu()
      {
         superClass = "MenuBuilder";
         class = "EditorBrushSizeMenu";

         // The onSelectItem() callback for this menu re-purposes the command field
         // as the MenuBuilder version is not used.
         item[0] = "Size 1 x 1" TAB "Alt 1" TAB "1";
         item[1] = "Size 3 x 3" TAB "Alt 2" TAB "3";
         item[2] = "Size 5 x 5" TAB "Alt 3" TAB "5";
         item[3] = "Size 9 x 9" TAB "Alt 4" TAB "9";
         item[4] = "Size 15 x 15" TAB "Alt 5" TAB "15";
         item[5] = "Size 25 x 25" TAB "Alt 6" TAB "25";
      };
      
      %this.brushMenu = new PopupMenu()
      {
         superClass = "MenuBuilder";
         class = "EditorBrushMenu";

         barTitle = "Brush";
         
         item[0] = "Box Brush" TAB "" TAB "ETerrainEditor.setBrushType(box);";
         item[1] = "Circle Brush" TAB "" TAB "ETerrainEditor.setBrushType(ellipse);";
         item[2] = "-";
         item[3] = "Soft Brush" TAB "" TAB "ETerrainEditor.enableSoftBrushes = true;";
         item[4] = "Hard Brush" TAB "" TAB "ETerrainEditor.enableSoftBrushes = false;";
         item[5] = "-";
         item[6] = "Brush Size" TAB %this.brushSizeMenu;
      };
   }
}

//////////////////////////////////////////////////////////////////////////

function EditorGui::attachMenus(%this)
{
   %this.menuBar.attachToCanvas(Canvas, 0);
}

function EditorGui::detachMenus(%this)
{
   %this.menuBar.removeFromCanvas();
}

function EditorGui::setMenuDefaultState(%this)
{  
   if(! isObject(%this.menuBar))
      return 0;
      
   for(%i = 0;%i < %this.menuBar.getCount();%i++)
   {
      %menu = %this.menuBar.getObject(%i);
      %menu.setupDefaultState();
   }
   
   %this.worldMenu.setupDefaultState();
   %this.actionMenu.setupDefaultState();
   %this.brushMenu.setupDefaultState();
}

//////////////////////////////////////////////////////////////////////////

function EditorGui::findMenu(%this, %name)
{
   if(! isObject(%this.menuBar))
      return 0;
      
   for(%i = 0;%i < %this.menuBar.getCount();%i++)
   {
      %menu = %this.menuBar.getObject(%i);
      
      if(%name $= %menu.barTitle)
         return %menu;
   }
   
   return 0;
}
