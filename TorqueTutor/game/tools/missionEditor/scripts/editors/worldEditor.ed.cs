//-----------------------------------------------------------------------------
// Torque Game Engine
// Copyright (C) GarageGames.com, Inc.
//-----------------------------------------------------------------------------

function WorldEditor::onDelete(%this)
{
   EditorTree.deleteSelection();
   //Inspector.uninspect();
}

function WorldEditor::onSelect(%this,%obj)
{
   EditorTree.addSelection(%obj);
}

function WorldEditor::onUnSelect(%this,%obj)
{
   EditorTree.removeSelection(%obj);
   //Inspector.uninspect();
}

function WorldEditor::onClearSelected(%this)
{
   EditorTree.clearSelection();
}

function WorldEditor::onClearSelection(%this)
{
   EditorTree.clearSelection();
}

//////////////////////////////////////////////////////////////////////////

function WorldEditor::init(%this)
{
   // add objclasses which we do not want to collide with
   %this.ignoreObjClass(Sky, AIObjective);

   // editing modes
   %this.numEditModes = 3;
   %this.editMode[0]    = "move";
   %this.editMode[1]    = "rotate";
   %this.editMode[2]    = "scale";

   // context menu
   new GuiControl(WEContextPopupDlg)
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

      new GuiPopUpMenuCtrl(WEContextPopup)
      {
         profile = "GuiScrollProfile";
         position = "0 0";
         extent = "0 0";
         minExtent = "0 0";
         maxPopupHeight = "200";
         command = "canvas.popDialog(WEContextPopupDlg);";
      };
   };
   WEContextPopup.setVisible(false);
}

//------------------------------------------------------------------------------

function WorldEditor::onDblClick(%this, %obj)
{
   // Commented out because making someone double click to do this is stupid
   // and has the possibility of moving hte object

   //Inspector.inspect(%obj);
   //InspectorNameEdit.setValue(%obj.getName());
}

function WorldEditor::onClick( %this, %obj )
{
   Inspector.inspect( %obj );
   InspectorNameEdit.setValue( %obj.getName() );
}

function WorldEditor::onEndDrag( %this, %obj )
{
   Inspector.inspect( %obj );
   InspectorNameEdit.setValue( %obj.getName() );
}

//------------------------------------------------------------------------------

function WorldEditor::export(%this)
{
   getSaveFilename("~/editor/*.mac", %this @ ".doExport", "selection.mac");
}

function WorldEditor::doExport(%this, %file)
{
   missionGroup.save("~/editor/" @ %file, true);
}

function WorldEditor::import(%this)
{
   getLoadFilename("~/editor/*.mac", %this @ ".doImport");
}

function WorldEditor::doImport(%this, %file)
{
   exec("~/editor/" @ %file);
}

function WorldEditor::onGuiUpdate(%this, %text)
{
}

function WorldEditor::getSelectionLockCount(%this)
{
   %ret = 0;
   for(%i = 0; %i < %this.getSelectionSize(); %i++)
   {
      %obj = %this.getSelectedObject(%i);
      if(%obj.locked $= "true")
         %ret++;
   }
   return %ret;
}

function WorldEditor::getSelectionHiddenCount(%this)
{
   %ret = 0;
   for(%i = 0; %i < %this.getSelectionSize(); %i++)
   {
      %obj = %this.getSelectedObject(%i);
      if(%obj.hidden $= "true")
         %ret++;
   }
   return %ret;
}

function WorldEditor::dropCameraToSelection(%this)
{
   if(%this.getSelectionSize() == 0)
      return;

   %pos = %this.getSelectionCentroid();
   %cam = LocalClientConnection.camera.getTransform();

   // set the pnt
   %cam = setWord(%cam, 0, getWord(%pos, 0));
   %cam = setWord(%cam, 1, getWord(%pos, 1));
   %cam = setWord(%cam, 2, getWord(%pos, 2));

   LocalClientConnection.camera.setTransform(%cam);
}

// * pastes the selection at the same place (used to move obj from a group to another)
function WorldEditor::moveSelectionInPlace(%this)
{
   %saveDropType = %this.dropType;
   %this.dropType = "atCentroid";
   %this.copySelection();
   %this.deleteSelection();
   %this.pasteSelection();
   %this.dropType = %saveDropType;
}

function WorldEditor::addSelectionToAddGroup(%this)
{
   for(%i = 0; %i < %this.getSelectionSize(); %i++)
   {
      %obj = %this.getSelectedObject(%i);
      $InstantGroup.add(%obj);
   }
}

// resets the scale and rotation on the selection set
function WorldEditor::resetTransforms(%this)
{
   %this.addUndoState();

   for(%i = 0; %i < %this.getSelectionSize(); %i++)
   {
      %obj = %this.getSelectedObject(%i);
      %transform = %obj.getTransform();

      %transform = setWord(%transform, 3, "0");
      %transform = setWord(%transform, 4, "0");
      %transform = setWord(%transform, 5, "1");
      %transform = setWord(%transform, 6, "0");

      //
      %obj.setTransform(%transform);
      %obj.setScale("1 1 1");
   }
}


function WorldEditorToolbarDlg::init(%this)
{
   WorldEditorInspectorCheckBox.setValue(WorldEditorToolFrameSet.isMember("EditorToolInspectorGui"));
   WorldEditorMissionAreaCheckBox.setValue(WorldEditorToolFrameSet.isMember("EditorToolMissionAreaGui"));
   WorldEditorTreeCheckBox.setValue(WorldEditorToolFrameSet.isMember("EditorToolTreeViewGui"));
   WorldEditorCreatorCheckBox.setValue(WorldEditorToolFrameSet.isMember("EditorToolCreatorGui"));
}

function WorldEditor::onAddSelected(%this,%obj)
{
   EditorTree.addSelection(%obj);
}
