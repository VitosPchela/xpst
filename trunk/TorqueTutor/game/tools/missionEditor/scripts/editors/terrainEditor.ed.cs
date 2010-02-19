//-----------------------------------------------------------------------------
// Torque Game Engine
// Copyright (C) GarageGames.com, Inc.
//-----------------------------------------------------------------------------
$Pref::TerrainEditor::TextureFileSpec = "Image Files (*.png, *.jpg)|*.png;*.jpg|All Files (*.*)|*.*|";

function ETerrainEditor::setPaintMaterial(%this, %matIndex)
{
   ETerrainEditor.paintMaterial = EPainter.mat[%matIndex];
   ETerrainEditor.paintIndex    = %matIndex;
}

function ETerrainEditor::changeMaterial(%this, %matIndex)
{
   EPainter.matIndex = %matIndex;

   %defaultFileName = EPainter.mat[%matIndex];
   if( $Pref::TerrainEditor::LastPath $= "" )
      $Pref::TerrainEditor::LastPath = expandFilename("~/data/terrains");


   %dlg = new OpenFileDialog()
   {
      Filters        = $Pref::TerrainEditor::TextureFileSpec;
      DefaultPath    = $Pref::TerrainEditor::LastPath;
      DefaultFile    = %defaultFileName;
      ChangePath     = false;
      MustExist      = true;
   };
         
   %ret = %dlg.Execute();
   if(%ret)
   {
      $Pref::TerrainEditor::LastPath = filePath( %dlg.FileName );
      %file = %dlg.FileName;
   }
   
   %dlg.delete();
   if( !%ret )
      return;
      
   // Make the absolute filename returned by the dialog (absolute) relative
   // to the game directory.  Note that while material filenames are stored
   // relative to the terrain file *within* the terrain file, they are kept
   // relative to the game directory while in-game.
   
   %file = makeRelativePath( %file, getMainDotCsDir() );

   // Make sure the material isn't already in the terrain.
   
   for(%i = 0; %i < 6; %i++)
      if(EPainter.mat[%i] $= %file)
         return;

   // Add it to our matrix and update the terrain.

   EPainter.mat[EPainter.matIndex] = %file;
  
   %mats = "";
   for(%i = 0; %i < 6; %i++)
      %mats = %mats @ EPainter.mat[%i] @ "\n";
      
   ETerrainEditor.setTerrainMaterials(%mats);
   EPainter.setup();
   ("ETerrainMaterialPaint" @ EPainter.matIndex).performClick();
}

function ETerrainEditor::setup(%this)
{
   if (ETerrainEditor.savedAction $= "")
      ETerrainEditor.currentAction = brushAdjustHeight;
   else
      ETerrainEditor.currentAction = ETerrainEditor.savedAction;
      
   ETerrainEditor.setAction(ETerrainEditor.currentAction);
}

function EPainter::setup(%this, %selectedIndex)
{
   //EditorMenuBar.onActionMenuItemSelect(0, "Paint Material");
   %mats = ETerrainEditor.getTerrainMaterials();
   %valid = true;
   for(%i = 0; %i < getRecordCount(%mats); %i++)
   {
      if (!isObject("ETerrainMaterialText" @ %i))
         continue;

      %mat = getRecord(%mats, %i);
      %this.mat[%i] = %mat;
      ("ETerrainMaterialText" @ %i).setText(fileBase(%mat));
      ("ETerrainMaterialBitmap" @ %i).setBitmap(%mat);
      ("ETerrainMaterialChange" @ %i).setActive(true);
      ("ETerrainMaterialPaint" @ %i).setActive(%mat !$= "");
      if(%mat $= "")
      {
         ("ETerrainMaterialChange" @ %i).setText("Add...");
         if(%valid)
            %valid = false;
         else
            ("ETerrainMaterialChange" @ %i).setActive(false);
      }
      else
         ("ETerrainMaterialChange" @ %i).setText("Change...");
   }
   
   if (%selectedIndex $= "")
      %selectedIndex = 0;
   
   if (ETerrainEditor.savedAction != ETerrainEditor.currentAction)
      ETerrainEditor.savedAction = ETerrainEditor.currentAction;
   
   ("ETerrainMaterialPaint" @ %selectedIndex).performClick();
   
   // Automagically put us into material paint mode.
   ETerrainEditor.currentMode = "paint";
   ETerrainEditor.selectionHidden = true;
   ETerrainEditor.currentAction = paintMaterial;
   ETerrainEditor.setAction(ETerrainEditor.currentAction);
   ETerrainEditor.renderVertexSelection = true;
}

function onNeedRelight()
{
   if( RelightMessage.visible == false )
      RelightMessage.visible = true;
}

function TerrainEditor::onGuiUpdate(%this, %text)
{
   %mouseBrushInfo = " (Mouse) #: " @ getWord(%text, 0) @ "  avg: " @ getWord(%text, 1) @ " " @ ETerrainEditor.currentAction;
   %selectionInfo = "     (Selected) #: " @ getWord(%text, 2) @ "  avg: " @ getWord(%text, 3);

   TEMouseBrushInfo.setValue(%mouseBrushInfo);
   TEMouseBrushInfo1.setValue(%mouseBrushInfo);
   TESelectionInfo.setValue(%selectionInfo);
   TESelectionInfo1.setValue(%selectionInfo);
}

function TerrainEditor::offsetBrush(%this, %x, %y)
{
   %curPos = %this.getBrushPos();
   %this.setBrushPos(getWord(%curPos, 0) + %x, getWord(%curPos, 1) + %y);
}

function TerrainEditor::swapInLoneMaterial(%this, %name)
{
   // swapped?
   if(%this.baseMaterialsSwapped $= "true")
   {
      %this.baseMaterialsSwapped = "false";
      tEditor.popBaseMaterialInfo();
   }
   else
   {
      %this.baseMaterialsSwapped = "true";
      %this.pushBaseMaterialInfo();
      %this.setLoneBaseMaterial(%name);
   }

   //
   flushTextureCache();
}

function TerrainEditor::onActiveTerrainChange(%this, %newTerrain)
{
   // Need to refresh the Terrain Painter
   if (EditorGui.currentEditor $= "Terrain Painter")
      EPainter.setup(ETerrainEditor.paintIndex);
      
   ETerraformer.onActiveTerrainChange( %newTerrain );
}

//------------------------------------------------------------------------------
// Functions
//------------------------------------------------------------------------------

//------------------------------------------------------------------------------


function TELoadTerrainButton::onAction(%this)
{
   %defaultFileName = EPainter.mat[%matIndex];
   if( $Pref::TerrainEditor::LastPath $= "" )
      $Pref::TerrainEditor::LastPath = expandFilename("~/data/");

   %dlg = new OpenFileDialog()
   {
      Filters        = $Pref::TerrainEditor::TextureFileSpec;
      DefaultPath    = $Pref::TerrainEditor::LastPath;
      DefaultFile    = %defaultFileName;
      ChangePath     = false;
      MustExist      = true;
   };
         
   %ret = %dlg.Execute();
   if(%ret)
   {
      $Pref::TerrainEditor::LastPath = filePath( %dlg.FileName );
      %file = %dlg.FileName;
   }
   
   %dlg.delete();
   
   if(! %ret)
      return;
   
   getLoadFilename("terrains/*.ter", %this @ ".gotFileName");
}

function TELoadTerrainButton::gotFileName(%this, %name)
{
   //
   %pos = "0 0 0";
   %squareSize = "8";
   %visibleDistance = "4000";

   // delete current
   if(isObject(ETerrainEditor.getActiveTerrain()))
   {
      %pos = ETerrainEditor.getActiveTerrain().position;
      %squareSize = ETerrainEditor.getActiveTerrain().squareSize;
      %visibleDistance = ETerrainEditor.getActiveTerrain().visibleDistance;

      ETerrainEditor.getActiveTerrain().delete();
   }

   // create new
   new TerrainBlock(ETerrainEditor.getActiveTerrain())
   {
      position = %pos;
      terrainFile = %name;
      squareSize = %squareSize;
      visibleDistance = %visibleDistance;
   };

   ETerrainEditor.attachTerrain();
}

function TerrainEditorSettingsGui::onWake(%this)
{
   TESoftSelectFilter.setValue(ETerrainEditor.softSelectFilter);
}

function TerrainEditorSettingsGui::onSleep(%this)
{
   ETerrainEditor.softSelectFilter = TESoftSelectFilter.getValue();
}

function TESettingsApplyButton::onAction(%this)
{
   ETerrainEditor.softSelectFilter = TESoftSelectFilter.getValue();
   ETerrainEditor.resetSelWeights(true);
   ETerrainEditor.processAction("softSelect");
}

function getPrefSetting(%pref, %default)
{
   //
   if(%pref $= "")
      return(%default);
   else
      return(%pref);
}
