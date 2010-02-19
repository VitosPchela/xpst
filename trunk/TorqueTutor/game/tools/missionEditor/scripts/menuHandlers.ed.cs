//-----------------------------------------------------------------------------
// Torque Game Engine
// Copyright (C) GarageGames.com, Inc.
//-----------------------------------------------------------------------------

$Pref::MissionEditor::FileSpec = "Torque Mission Files (*.mis)|*.mis|All Files (*.*)|*.*|";

//////////////////////////////////////////////////////////////////////////
// File Menu Handlers
//////////////////////////////////////////////////////////////////////////

function EditorFileMenu::onMenuSelect(%this)
{
   %editingHeightfield = ETerrainEditor.isVisible() && EHeightField.isVisible();
   %this.enableItem(8, %editingHeightfield);
   %this.enableItem(2, ETerrainEditor.isDirty || ETerrainEditor.isMissionDirty || EWorldEditor.isDirty);
}

//////////////////////////////////////////////////////////////////////////

function EditorQuitMission()
{  
   if(ETerrainEditor.isMissionDirty || ETerrainEditor.isDirty || EWorldEditor.isDirty)
   {
      MessageBoxYesNo("Mission Modified", "Would you like to save your changes before quiting?", "EditorDoQuitMission(true);", "EditorDoQuitMission(false);");
   }
   else
      EditorDoQuitMission(false);
}

function EditorDoQuitMission(%saveFirst)
{
   if(%saveFirst)
   {
      EditorSaveMissionMenu();
      if (isObject( MainMenuGui ))
         Editor.close("MainMenuGui");
      else if (isObject( UnifiedMainMenuGui ))
         Editor.close("UnifiedMainMenuGui");
      disconnect();
   }
   else
   {
      if (isObject( MainMenuGui ))
         Editor.close("MainMenuGui");
      else if (isObject( UnifiedMainMenuGui ))
         Editor.close("UnifiedMainMenuGui");
      disconnect();
   }
}

function EditorNewMission()
{
   %saveFirst = false;
   if(ETerrainEditor.isMissionDirty || ETerrainEditor.isDirty || EWorldEditor.isDirty)
   {
      error(knob);
      %saveFirst = MessageBox("Mission Modified", "Would you like to save changes to the current mission \"" @
         $Server::MissionFile @ "\" before creating a new mission?", "SaveDontSave", "Question") == $MROk;
   }
      
   if(%saveFirst)
      EditorSaveMission();

   // Clear dirty flags first to avoid duplicate dialog box from EditorOpenMission()
   EWorldEditor.isDirty = false;
   ETerrainEditor.isDirty = false;
   ETerrainEditor.isMissionDirty = false;
   
   EditorOpenMission("scriptsAndAssets/data/missions/newMission.mis");
   EWorldEditor.isDirty = true;
   ETerrainEditor.isDirty = true;
   EditorGui.saveAs = true;
}

function EditorSaveMissionMenu()
{
   if(EditorGui.saveAs)
      EditorSaveMissionAs();
   else
      EditorSaveMission();
}

function EditorSaveMission()
{
   // just save the mission without renaming it

   // first check for dirty and read-only files:
   if((EWorldEditor.isDirty || ETerrainEditor.isMissionDirty) && !isWriteableFileName($Server::MissionFile))
   {
      MessageBox("Error", "Mission file \""@ $Server::MissionFile @ "\" is read-only.", "Ok", "Stop");
      return false;
   }
   if(ETerrainEditor.isDirty)
   {
      // Find all of the terrain files
      initContainerTypeSearch($TypeMasks::TerrainObjectType);

      while ((%terrainObject = containerSearchNext()) != 0)
      {
         if (!isWriteableFileName(%terrainObject.terrainFile))
         {
            if (MessageBox("Error", "Terrain file \""@ %terrainObject.terrainFile @ "\" is read-only.", "Ok", "Stop") == $MROk)
               continue;
            else
               return false;
         }
      }
   }
  
   // now write the terrain and mission files out:

   if(EWorldEditor.isDirty || ETerrainEditor.isMissionDirty)
      MissionGroup.save($Server::MissionFile);
   if(ETerrainEditor.isDirty)
   {
      // Find all of the terrain files
      initContainerTypeSearch($TypeMasks::TerrainObjectType);

      while ((%terrainObject = containerSearchNext()) != 0)
         %terrainObject.save(%terrainObject.terrainFile);
   }
   EWorldEditor.isDirty = false;
   ETerrainEditor.isDirty = false;
   ETerrainEditor.isMissionDirty = false;
   EditorGui.saveAs = false;

   return true;
}

function EditorSaveMissionAs()
{
   %defaultFileName = $Server::MissionFile;
   if( %defaultFileName $= "" )
      %defaultFileName = expandFilename("~/data/missions/untitled.mis");

   %dlg = new SaveFileDialog()
   {
      Filters        = $Pref::MissionEditor::FileSpec;
      DefaultPath    = $Pref::MissionEditor::LastPath;
      DefaultFile    = %defaultFileName;
      ChangePath     = false;
      OverwritePrompt   = true;
   };
         
   %ret = %dlg.Execute();
   if(%ret)
   {
      $Pref::MissionEditor::LastPath = filePath( %dlg.FileName );
      %missionName = %dlg.FileName;
   }
   
   %dlg.delete();
   
   if(! %ret)
      return;
      
   if( fileExt( %missionName ) $= "" )
      %missionName = %missionName @ ".mis";
      
   ETerrainEditor.isDirty = true;
   EWorldEditor.isDirty = true;
   %saveMissionFile = $Server::MissionFile;

   $Server::MissionFile = %missionName;
   
   // Rename all the terrain files.  Save all previous names so we can
   // reset them if saving fails.
   
   %terrainFileBase = filePath( %missionName ) @ "/" @ fileBase( %missionName );
   %terrainFileBase = makeRelativePath( %terrainFileBase, getMainDotCsDir() );

   initContainerTypeSearch( $TypeMasks::TerrainObjectType );
   %savedTerrNames = new ScriptObject();
   for( %i = 0;; %i ++ )
   {
      %terrainObject = containerSearchNext();
      if( !%terrainObject )
         break;
         
      %savedTerrNames.array[ %i ] = %terrainObject.terrainFile;
      %terrainObject.terrainFile = %terrainFileBase @ "_" @ %i @ ".ter";
   }
   
   // Save the mission.

   if(!EditorSaveMission())
   {
      // It failed, so restore the mission and terrain filenames.
      
      $Server::MissionFile = %saveMissionFile;

      initContainerTypeSearch( $TypeMasks::TerrainObjectType );
      for( %i = 0;; %i ++ )
      {
         %terrainObject = containerSearchNext();
         if( !%terrainObject )
            break;
            
         %terrainObject.terrainFile = %savedTerrNames.array[ %i ];
      }
   }
   
   %savedTerrNames.delete();
}

function EditorOpenMission(%filename)
{
   if(ETerrainEditor.isMissionDirty || ETerrainEditor.isDirty || EWorldEditor.isDirty)
   {
      // "EditorSaveBeforeLoad();", "getLoadFilename(\"*.mis\", \"EditorDoLoadMission\");"
      if(MessageBox("Mission Modified", "Would you like to save changes to the current mission \"" @
         $Server::MissionFile @ "\" before opening a new mission?", SaveDontSave, Question) == $MROk)
      {
         if(! EditorSaveMission())
            return;
      }
   }

   if(%filename $= "")
   {
      %defaultFileName = $Server::MissionFile;
      if( %defaultFileName $= "" )
         %defaultFileName = expandFilename("~/data/missions/untitled.mis");

      %dlg = new OpenFileDialog()
      {
         Filters        = $Pref::MissionEditor::FileSpec;
         DefaultPath    = $Pref::MissionEditor::LastPath;
         DefaultFile    = %defaultFileName;
         ChangePath     = false;
         MustExist      = true;
      };
            
      %ret = %dlg.Execute();
      if(%ret)
      {
         $Pref::MissionEditor::LastPath = filePath( %dlg.FileName );
         %filename = %dlg.FileName;
      }
      
      %dlg.delete();
      
      if(! %ret)
         return;
   }
   
   // close the current editor, it will get cleaned up by MissionCleanup
   Editor.close();

   loadMission( %filename, true ) ;

   // recreate and open the editor
   Editor::create();
   MissionCleanup.add(Editor);
   EditorGui.loadingMission = true;
   Editor.open();
}

//////////////////////////////////////////////////////////////////////////
// Edit Menu Handlers
//////////////////////////////////////////////////////////////////////////

function EditorEditMenu::onMenuSelect(%this)
{
   if(EWorldEditor.isVisible())
   {
      //%this.enableItem(7, true); // Select All
      %this.enableItem(5, EWorldEditor.canPasteSelection()); // Paste
      
      %canCutCopy = EWorldEditor.getSelectionSize() > 0;

      %this.enableItem(3, %canCutCopy); // Cut
      %this.enableItem(4, %canCutCopy); // Copy

   }
   else if(ETerrainEditor.isVisible())
   {
      %this.enableItem(3, false); // Cut
      %this.enableItem(4, false); // Copy
      %this.enableItem(5, false); // Paste
      //%this.enableItem(7, false); // Select All
      //%this.enableItem(8, false); // Select Nonde
   }
}

//////////////////////////////////////////////////////////////////////////

function EditorMenuEditUndo()
{
   if(EWorldEditor.isVisible())
      EWorldEditor.undo();
   else if(ETerrainEditor.isVisible())
      ETerrainEditor.undo();
}

function EditorMenuEditRedo()
{
   if(EWorldEditor.isVisible())
      EWorldEditor.redo();
   else if(ETerrainEditor.isVisible())
      ETerrainEditor.redo();
}

function EditorMenuEditCopy()
{
   if(! EWorldEditor.isVisible())
      return;
      
   EWorldEditor.copySelection();
}

function EditorMenuEditCut()
{
   if(! EWorldEditor.isVisible())
      return;
      
   EWorldEditor.copySelection();
   EWorldEditor.deleteSelection();
   //Inspector.uninspect();
}

function EditorMenuEditPaste()
{
   if(! EWorldEditor.isVisible())
      return;
   
   EWorldEditor.pasteSelection();
}

// Note: The original editor didn't implement these either, but leaving stubs since
// we will want to implement them at some point in the future.
function EditorMenuEditSelectAll()
{
}

function EditorMenuEditSelectNone()
{
}

//////////////////////////////////////////////////////////////////////////
// Window Menu Handler
//////////////////////////////////////////////////////////////////////////

function EditorWindowMenu::onSelectItem(%this, %id, %text)
{
   // The text passed in may have the accellerator in it, so this is safer
   EditorGui.setEditor(getField(%this.item[%id], 0));
   
   %this.checkRadioItem(0, 8, %id);
}

function EditorWindowMenu::setupDefaultState(%this)
{
   %this.onSelectItem(1, getField(%this.item[1], 0));
   Parent::setupDefaultState(%this);
}

//////////////////////////////////////////////////////////////////////////
// Camera Menu Handler
//////////////////////////////////////////////////////////////////////////

function EditorCameraSpeedMenu::onSelectItem(%this, %id, %text)
{
   // CodeReview - Seriously, comment your magic numbers... -JDD
   $Camera::movementSpeed = (%id / 6.0) * 195 + 5;
   
   %this.checkRadioItem(0, 6, %id);
}

function EditorCameraSpeedMenu::setupDefaultState(%this)
{
   %this.onSelectItem(3, getField(%this.item[3], 0));
   Parent::setupDefaultState(%this);
}

//////////////////////////////////////////////////////////////////////////
// World Menu Handler
//////////////////////////////////////////////////////////////////////////

function EditorWorldMenu::onMenuSelect(%this)
{
   %selSize = EWorldEditor.getSelectionSize();
   %lockCount = EWorldEditor.getSelectionLockCount();
   %hideCount = EWorldEditor.getSelectionHiddenCount();
   
   %this.enableItem(0, %lockCount < %selSize);  // Lock Selection
   %this.enableItem(1, %lockCount > 0);  // Unlock Selection
   
   %this.enableItem(3, %hideCount < %selSize);  // Hide Selection
   %this.enableItem(4, %hideCount > 0);  // Show Selection
   
   %this.enableItem(6, %selSize > 1);  // Align bounds
   %this.enableItem(7, %selSize > 1);  // Align center
   
   %this.enableItem(9, %selSize > 0);  // Reset Selected Rotation
   %this.enableItem(10, %selSize > 0);  // Reset Selected Scale

   %this.enableItem(15, %selSize > 0);  // Add to instant group
   %this.enableItem(13, %selSize > 0);  // Camera To Selection
   %this.enableItem(14, %selSize > 0 && %lockCount == 0);  // Reset Transforms
   %this.enableItem(16, %selSize > 0 && %lockCount == 0);  // Drop Selection
   %this.enableItem(12, %lockCount == 0);  // Delete Selection
   
}

//////////////////////////////////////////////////////////////////////////

function EditorDropTypeMenu::onSelectItem(%this, %id, %text)
{
   // Read out dropType setting.
   EWorldEditor.dropType = getField(%this.item[%id], 2);   
   
   %this.checkRadioItem(0, 6, %id);
}

function EditorDropTypeMenu::setupDefaultState(%this)
{
   // Check the radio item for the currently set drop type.
   
   %dropTypeIndex = 0;
   for( ; %dropTypeIndex < 7; %dropTypeIndex ++ )
      if( getField( %this.item[ %dropTypeIndex ], 2 ) $= EWorldEditor.dropType )
         break;
 
   // Default to screenCenter if we didn't match anything.        
   if( %dropTypeIndex > 6 )
      %dropTypeIndex = 4;
   
   %this.checkRadioItem( 0, 6, %dropTypeIndex );
      
   Parent::setupDefaultState(%this);
}

//////////////////////////////////////////////////////////////////////////

function EditorAlignBoundsMenu::onSelectItem(%this, %id, %text)
{
   // Have the editor align all selected objects by the selected bounds.
   EWorldEditor.alignByBounds(getField(%this.item[%id], 2));
}

function EditorAlignBoundsMenu::setupDefaultState(%this)
{
   // Allow the parent to set the menu's default state
   Parent::setupDefaultState(%this);
}

//////////////////////////////////////////////////////////////////////////

function EditorAlignCenterMenu::onSelectItem(%this, %id, %text)
{
   // Have the editor align all selected objects by the selected axis.
   EWorldEditor.alignByAxis(getField(%this.item[%id], 2));
}

function EditorAlignCenterMenu::setupDefaultState(%this)
{
   // Allow the parent to set the menu's default state
   Parent::setupDefaultState(%this);
}

//////////////////////////////////////////////////////////////////////////
// Brush Menu Handler
//////////////////////////////////////////////////////////////////////////

function EditorBrushMenu::onSelectItem(%this, %id, %text)
{
   if(%id >= 0 && %id <= 1)
      %this.checkRadioItem(0, 1, %id);
      
   if(%id >= 3 && %id <= 4)
      %this.checkRadioItem(3, 4, %id);
   
   // Pass off to parent for handling of actual action
   Parent::onSelectItem(%this, %id, %text);
}

function EditorBrushMenu::setupDefaultState(%this)
{
   %this.onSelectItem(1, getField(%this.item[1], 0));
   %this.onSelectItem(3, getField(%this.item[3], 0));
   Parent::setupDefaultState(%this);
}

function EditorBrushSizeMenu::onSelectItem(%this, %id, %text)
{
   %size = getField(%this.item[%id], 2);
   
   ETerrainEditor.brushSize = %size;
   ETerrainEditor.setBrushSize(%size, %size);
   
   %this.checkRadioItem(0, 5, %id);
}

function EditorBrushSizeMenu::setupDefaultState(%this)
{
   %this.onSelectItem(3, getField(%this.item[3], 0));
   Parent::setupDefaultState(%this);
}

//////////////////////////////////////////////////////////////////////////
// Action Menu Handler
//////////////////////////////////////////////////////////////////////////

// FIXME: Tidy this up
function EditorActionMenu::onSelectItem(%this, %id, %text)
{
   %this.checkRadioItem(0, 13, %id);
   
   %item = getField(%this.item[%id], 0);
   
   switch$(%item)
   {
      case "Select":
         ETerrainEditor.currentMode = "select";
         ETerrainEditor.selectionHidden = false;
         ETerrainEditor.renderVertexSelection = true;
         ETerrainEditor.setAction("select");
      case "Deselect":
         ETerrainEditor.currentMode = "deselect";
         ETerrainEditor.selectionHidden = false;
         ETerrainEditor.renderVertexSelection = true;
         ETerrainEditor.setAction("deselect");
      case "Adjust Selection":
         ETerrainEditor.currentMode = "adjust";
         ETerrainEditor.selectionHidden = false;
         ETerrainEditor.setAction("adjustHeight");
         ETerrainEditor.currentAction = brushAdjustHeight;
         ETerrainEditor.renderVertexSelection = true;
      case "Clear Selection":
         ETerrainEditor.currentMode = "clear";
         ETerrainEditor.selectionHidden = true;
         ETerrainEditor.renderVertexSelection = true;
         ETerrainEditor.setAction("clear");
         %this.onSelectItem(0, getField(%this.item[0], 0));
      default:
         ETerrainEditor.currentMode = "paint";
         ETerrainEditor.selectionHidden = true;
         ETerrainEditor.setAction(ETerrainEditor.currentAction);
         switch$(%item)
         {
            case "Add Dirt":
               ETerrainEditor.currentAction = raiseHeight;
               ETerrainEditor.renderVertexSelection = true;
            case "Paint Material":
               ETerrainEditor.currentAction = paintMaterial;
               ETerrainEditor.renderVertexSelection = true;
            case "Clear Materials":
               ETerrainEditor.currentAction = clearMaterials;
               ETerrainEditor.renderVertexSelection = true;
            case "Excavate":
               ETerrainEditor.currentAction = lowerHeight;
               ETerrainEditor.renderVertexSelection = true;
            case "Set Height":
               ETerrainEditor.currentAction = setHeight;
               ETerrainEditor.renderVertexSelection = true;
            case "Adjust Height":
               ETerrainEditor.currentAction = brushAdjustHeight;
               ETerrainEditor.renderVertexSelection = true;
            case "Flatten":
               ETerrainEditor.currentAction = flattenHeight;
               ETerrainEditor.renderVertexSelection = true;
            case "Smooth":
               ETerrainEditor.currentAction = smoothHeight;
               ETerrainEditor.renderVertexSelection = true;
            case "Set Empty":
               ETerrainEditor.currentAction = setEmpty;
               ETerrainEditor.renderVertexSelection = false;
            case "Clear Empty":
               ETerrainEditor.currentAction = clearEmpty;
               ETerrainEditor.renderVertexSelection = false;
         }
         if(ETerrainEditor.currentMode $= "select")
            ETerrainEditor.processAction(ETerrainEditor.currentAction);
         else if(ETerrainEditor.currentMode $= "deselect")
            ETerrainEditor.processAction(ETerrainEditor.currentAction);
         else if(ETerrainEditor.currentMode $= "paint")
            ETerrainEditor.setAction(ETerrainEditor.currentAction);
         else if(ETerrainEditor.currentMode $= "clear")
            %this.onSelectItem(0, getField(%this.item[0], 0));
   }
}

function EditorActionMenu::setupDefaultState(%this)
{
   %this.onSelectItem(5, getField(%this.item[5], 0));
   Parent::setupDefaultState(%this);
}
