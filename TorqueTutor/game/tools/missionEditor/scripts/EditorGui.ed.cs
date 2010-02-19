//-----------------------------------------------------------------------------
// Torque Game Engine
// Copyright (C) GarageGames.com, Inc.
//-----------------------------------------------------------------------------

function EditorGui::init(%this)
{
   %this.getPrefs();

   if(!isObject(ETerraformer))
      new Terraformer(ETerraformer);

   $SelectedOperation = -1;
   $NextOperationId   = 1;
   $HeightfieldDirtyRow = -1;

   %this.buildMenus();

   // Build Creator tree
   if( !isObject( %this-->InspectorWindow ) )
   {
      // Load Creator/Inspector GUI
      exec("~/missionEditor/gui/guiWorldEditorMissionInspector.ed.gui");
      if( isObject( %guiContent ) )
         %this.add( %guiContent->InspectorWindow );
   }
   if( !isObject( %this-->TerrainPainter ) )
   {
      // Load Terrain Painter GUI
      exec("~/missionEditor/gui/guiTerrainPainterContainer.ed.gui");
      if( isObject( %guiContent ) )
         %this.add( %guiContent->TerrainPainter );
   }

   if( !isObject( %this-->TextureEditor ) )
   {
      // Load Terrain Texture Editor GUI
      exec("~/missionEditor/gui/guiTerrainTexEdContainer.ed.gui");
      if( isObject( %guiContent ) )
         %this.add( %guiContent->TextureEditor );
         %this.add( %guiContent->TexPrevWindow );
   }

   if( !isObject( %this-->MissionAreaEditor ) )
   {
      // Load Mission Area Editor GUI
      exec("~/missionEditor/gui/guiMissionAreaEditorContainer.ed.gui");
      if( isObject( %guiContent ) )
         %this.add( %guiContent->MissionAreaEditor );      
   }

   if(!isObject(%this-->TerraformEditor))
   {
      exec("~/missionEditor/gui/guiTerrainEditorContainer.ed.gui");
      if(isObject (%guiContent))
      {   
         %this.add(%guiContent->TerraformEditor);
         %this.add(%guiContent->HeightfieldWindow);
      }
   }

   EWorldEditor.init();
   ETerrainEditor.attachTerrain();
   TerraformerInit();
   TextureInit();



   //
   //Creator.init();
   EditorTree.init();
   ObjectBuilderGui.init();

   %this.setMenuDefaultState();

   EWorldEditor.isDirty = false;
   ETerrainEditor.isDirty = false;
   ETerrainEditor.isMissionDirty = false;
   EditorGui.saveAs = false;

   EWorldEditorDisplayPopup.clear();
   EWorldEditorDisplayPopup.add("Top" TAB "(XY)", 0);
   EWorldEditorDisplayPopup.add("Bottom" TAB "(XY)", 1);
   EWorldEditorDisplayPopup.add("Front" TAB "(XZ)", 2);
   EWorldEditorDisplayPopup.add("Back" TAB "(XZ)", 3);
   EWorldEditorDisplayPopup.add("Left" TAB "(YZ)", 4);
   EWorldEditorDisplayPopup.add("Right" TAB "(YZ)", 5);
   EWorldEditorDisplayPopup.add("Perspective", 6);
   EWorldEditorDisplayPopup.add("Isometric", 7);
   EWorldEditorDisplayPopup.setSelected(EWorldEditor.getDisplayType());

   EWorldEditorInteriorPopup.clear();
   EWorldEditorInteriorPopup.add("Interior Outline", 1);
   EWorldEditorInteriorPopup.add("Interior Zones", 2);
   EWorldEditorInteriorPopup.add("Interior Normal", 0);
   EWorldEditorInteriorPopup.setSelected($MFDebugRenderMode);
}

//-----------------------------------------------------------------------------

function EditorGui::setWorldEditorVisible(%this)
{
   EWorldEditor.setVisible(true);
   ETerrainEditor.setVisible(false);
   %this.menuBar.insert(%this.worldMenu, %this.menuBar.dynamicItemInsertPos);
   %this.menuBar.remove(%this.actionMenu);
   %this.menuBar.remove(%this.brushMenu);
   EWorldEditor.makeFirstResponder(true);
   EditorTree.open(MissionGroup,true);

   WorldEditorMap.push();
}

function EditorGui::setTerrainEditorVisible(%this)
{
   EWorldEditor.setVisible(false);
   ETerrainEditor.setVisible(true);
   ETerrainEditor.attachTerrain();
   EHeightField.setVisible(false);
   %this.menuBar.remove(%this.worldMenu);
   %this.menuBar.insert(%this.actionMenu, %this.menuBar.dynamicItemInsertPos);
   %this.menuBar.insert(%this.brushMenu, %this.menuBar.dynamicItemInsertPos + 1);
   ETerrainEditor.makeFirstResponder(true);

   WorldEditorMap.pop();
}

function EditorGui::setEditor(%this, %editor)
{
   %this.currentEditor = %editor;

   switch$(%editor)
   {
      case "World Editor":
         EWMissionArea.setVisible(false);
         %this.setWorldEditorVisible();
         %this-->InspectorWindow.setVisible(false);
         %this-->TerrainPainter.setVisible(false);
         %this-->MissionAreaEditor.setVisible(false);
         %this-->TextureEditor.setVisible(false);
         %this-->TerraformEditor.setVisible(false);
         %this-->HeightfieldWindow.setVisible(false);
         %this-->TexPrevWindow.setVisible(false);          
      case "World Editor Inspector":
         EWMissionArea.setVisible(false);
         EWCreatorPane.setVisible(false);
         EWInspectorPane.setVisible(true);
         %this.setWorldEditorVisible();
         %this-->InspectorWindow.setVisible(true);
         %this-->TerrainPainter.setVisible(false);
         %this-->MissionAreaEditor.setVisible(false);
         %this-->TextureEditor.setVisible(false);  
         %this-->TerraformEditor.setVisible(false);
         %this-->HeightfieldWindow.setVisible(false);
         %this-->TexPrevWindow.setVisible(false);  
      case "World Editor Creator":
         EWMissionArea.setVisible(false);
         EWCreatorPane.setVisible(true);
         EWInspectorPane.setVisible(false);
         %this.setWorldEditorVisible();
         %this-->InspectorWindow.setVisible(true);
         %this-->TerrainPainter.setVisible(false);
         %this-->MissionAreaEditor.setVisible(false);
         %this-->TextureEditor.setVisible(false);
         %this-->TerraformEditor.setVisible(false);  
         %this-->HeightfieldWindow.setVisible(false);
         %this-->TexPrevWindow.setVisible(false); 
      case "Mission Area Editor":
         EWMissionArea.setVisible(true);
         %this.setWorldEditorVisible();
         %this-->InspectorWindow.setVisible(false);
         %this-->TerrainPainter.setVisible(false);
         %this-->MissionAreaEditor.setVisible(true);
         %this-->TextureEditor.setVisible(false);    
         %this-->TerraformEditor.setVisible(false);
         %this-->HeightfieldWindow.setVisible(false);      
         %this-->TexPrevWindow.setVisible(false); 
      case "Terrain Editor":
         %this.setTerrainEditorVisible();
         %this-->InspectorWindow.setVisible(false);
         %this-->TerrainPainter.setVisible(false);
         %this-->MissionAreaEditor.setVisible(false);
         %this-->TextureEditor.setVisible(false); 
         %this-->TerraformEditor.setVisible(false);
         %this-->HeightfieldWindow.setVisible(false);  
         ETerrainEditor.setup();
         %this-->TexPrevWindow.setVisible(false);     
         
      case "Terrain Terraform Editor":
         %this.setTerrainEditorVisible();
         EHeightField.setVisible(true);
         %this-->InspectorWindow.setVisible(false);
         %this-->TerrainPainter.setVisible(false);
         %this-->MissionAreaEditor.setVisible(false);
         %this-->TextureEditor.setVisible(false);
         %this-->TerraformEditor.setVisible(true);
         %this-->HeightfieldWindow.setVisible(true);
         %this-->TexPrevWindow.setVisible(false);   
      
      case "Terrain Texture Editor":
         %this.setTerrainEditorVisible();
         %this-->InspectorWindow.setVisible(false);
         %this-->TerrainPainter.setVisible(false);
         %this-->MissionAreaEditor.setVisible(false);
         %this-->TextureEditor.setVisible(true);   
         %this-->TerraformEditor.setVisible(false);
         %this-->HeightfieldWindow.setVisible(false);       
         %this-->TexPrevWindow.setVisible(true); 
         
      case "Terrain Painter":
         %this.setTerrainEditorVisible();
         %this-->InspectorWindow.setVisible(false);
         %this-->TerrainPainter.setVisible(true);
         %this-->MissionAreaEditor.setVisible(false);
         %this-->TextureEditor.setVisible(false);  
         %this-->TerraformEditor.setVisible(false); 
         %this-->HeightfieldWindow.setVisible(false);                
         EPainter.setup();
         %this-->TexPrevWindow.setVisible(false); 

   }
}

//-----------------------------------------------------------------------------

function EditorGui::getHelpPage(%this)
{
   switch$(%this.currentEditor)
   {
      case "World Editor" or "World Editor Inspector" or "World Editor Creator":
         return "5. World Editor";
      case "Mission Area Editor":
         return "6. Mission Area Editor";
      case "Terrain Editor":
         return "7. Terrain Editor";
      case "Terrain Terraform Editor":
         return "8. Terrain Terraform Editor";
      case "Terrain Texture Editor":
         return "9. Terrain Texture Editor";
      case "Terrain Painter":
         return "10. Terrain Painter";
   }
}

//-----------------------------------------------------------------------------

function EditorGui::onWake(%this)
{
   MoveMap.push();
   EditorMap.push();
   %this.setEditor(%this.currentEditor);

   if (isObject(DemoEditorAlert) && DemoEditorAlert.helpTag<2)
      Canvas.pushDialog(DemoEditorAlert);

}

function EditorGui::onSleep(%this)
{
   %this.setPrefs();

   EditorMap.pop();
   MoveMap.pop();
   if(isObject($Server::CurrentScene))
      $Server::CurrentScene.open();
}

//-----------------------------------------------------------------------------

// Called when we have been set as the content and onWake has been called
function EditorGui::onSetContent(%this, %oldContent)
{
   %this.attachMenus();
}

// Called before onSleep when the canvas content is changed
function EditorGui::onUnsetContent(%this, %newContent)
{
   %this.detachMenus();
}

//------------------------------------------------------------------------------

function EditorGui::addCameraBookmark(%this, %name)
{
   %obj = new CameraBookmark() {
      datablock = CameraBookmarkMarker;
      name = %name;
   };

   // Place into correct group
   if(!isObject(CameraBookmarks))
   {
      %grp = new SimGroup(CameraBookmarks);
      $InstantGroup.add(%grp);
   }
   CameraBookmarks.add(%obj);

   %cam = LocalClientConnection.camera.getTransform();
   %obj.setTransform(%cam);
}

function EditorGui::removeCameraBookmark(%this, %name)
{
   if(!isObject(CameraBookmarks))
      return;

   %count = CameraBookmarks.getCount();
   for(%i=0; %i<%count; %i++)
   {
      %obj = CameraBookmarks.getObject(%i);
      if(%obj.name $= %name)
      {
         %obj.delete();
         return;
      }
   }
}

function EditorGui::removeCameraBookmarkIndex(%this, %index)
{
   if(!isObject(CameraBookmarks))
      return;

   if(%index < 0 || %index >= CameraBookmarks.getCount())
      return;

   %obj = CameraBookmarks.getObject(%index);
   %obj.delete();
}

function EditorGui::jumpToBookmark(%this, %name)
{
   if(!isObject(CameraBookmarks))
      return;

   %count = CameraBookmarks.getCount();
   for(%i=0; %i<%count; %i++)
   {
      %obj = CameraBookmarks.getObject(%i);
      if(%obj.name $= %name)
      {
         LocalClientConnection.camera.setTransform(%obj.getTransform());
         return;
      }
   }
}

function EditorGui::jumpToBookmarkIndex(%this, %index)
{
   if(!isObject(CameraBookmarks))
      return;

   if(%index < 0 || %index >= CameraBookmarks.getCount())
      return;

   %trans = CameraBookmarks.getObject(%index).getTransform();
   LocalClientConnection.camera.setTransform(%trans);
}

//-----------------------------------------------------------------------------

function WorldEditor::toggleSnapToGrid(%this)
{
   %this.snapToGrid = !(%this.snapToGrid);
}

//-----------------------------------------------------------------------------

function EWorldEditorDisplayPopup::onSelect(%this, %id, %text)
{
   EWorldEditor.setDisplayType(%id);
}

//-----------------------------------------------------------------------------

function EWorldEditorInteriorPopup::onSelect(%this, %id, %text)
{
   $MFDebugRenderMode = %id;

   switch(%id)
   {
      case 0:
         // Back to normal
         setInteriorRenderMode(0);

      case 1:
         // Outline mode, including fonts so no stats
         setInteriorRenderMode(1);

      case 2:
         // Interior debug mode
         setInteriorRenderMode(7);
   }
}
//-----------------------------------------------------------------------------

function EditorTree::onObjectDeleteCompleted(%this)
{
   EWorldEditor.copySelection();
   EWorldEditor.deleteSelection();
}

function EditorTree::onClearSelected(%this)
{
   WorldEditor.clearSelection();
}

function EditorTree::init(%this)
{
   //%this.open(MissionGroup);

   // context menu
   new GuiControl(ETContextPopupDlg)
   {
    profile = "GuiModelessDialogProfile";
      horizSizing = "width";
      vertSizing = "height";
      position = "0 0";
      extent = "640 480";
      minExtent = "8 8";
      visible = "1";
      setFirstResponder = "0";
      modal = "1";

      new GuiPopUpMenuCtrl(ETContextPopup)
      {
         profile = "GuiScrollProfile";
         position = "0 0";
         extent = "0 0";
         minExtent = "0 0";
         maxPopupHeight = "200";
         command = "canvas.popDialog(ETContextPopupDlg);";
      };
   };
   ETContextPopup.setVisible(false);
}

function EditorTree::onInspect(%this, %obj)
{
   Inspector.inspect(%obj);
   InspectorNameEdit.setValue(%obj.getName());
}

function EditorTree::onAddSelection(%this, %obj)
{
   EWorldEditor.selectObject(%obj);
}
function EditorTree::onRemoveSelection(%this, %obj)
{
   EWorldEditor.unselectObject(%obj);
}
function EditorTree::onSelect(%this, %obj)
{
   EWorldEditor.selectObject(%obj);
}

function EditorTree::onUnselect(%this, %obj)
{
   EWorldEditor.unselectObject(%obj);
}

function ETContextPopup::onSelect(%this, %index, %value)
{
   switch(%index)
   {
      case 0:
         EditorTree.contextObj.delete();
   }
}

//------------------------------------------------------------------------------

function Editor::open(%this)
{
   // prevent the mission editor from opening while the GuiEditor is open.
   if(Canvas.getContent() == GuiEditorGui.getId())
      return;

   Canvas.setContent(EditorGui);
}

function Editor::close(%this, %gui)
{
   Canvas.setContent(%gui);
   if(isObject(MessageHud))
      MessageHud.close();
}

$RelightCallback = "";

function EditorLightingComplete()
{
   $lightingMission = false;
   RelightStatus.visible = false;
   
   if ($RelightCallback !$= "")
   {
      eval($RelightCallback);
   }
   
   $RelightCallback = "";
}

function updateEditorLightingProgress()
{
   RelightProgress.setValue(($SceneLighting::lightingProgress));
   if ($lightingMission)
      $lightingProgressThread = schedule(1, 0, "updateEditorLightingProgress");
}

function Editor::lightScene(%this, %callback, %forceAlways)
{
   if ($lightingMission)
      return;
      
   $lightingMission = true;
   $RelightCallback = %callback;
   RelightStatus.visible = true;
   RelightProgress.setValue(0);
   Canvas.repaint();  
   lightScene("EditorLightingComplete", %forceAlways);
   updateEditorLightingProgress();
} 
