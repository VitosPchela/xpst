//-----------------------------------------------------------------------------
// Torque Game Engine
// Copyright (C) GarageGames.com, Inc.
//-----------------------------------------------------------------------------

function EditorGui::getPrefs()
{
   // same defaults as WorldEditor ctor
   EWorldEditor.axisGizmoActive = getPrefSetting($pref::WorldEditor::axisGizmoActive, true);
   EWorldEditor.axisGizmoMaxScreenLen = getPrefSetting($pref::WorldEditor::axisGizmoMaxScreenLen, 200);
   EWorldEditor.boundingBoxCollision = getPrefSetting($pref::WorldEditor::boundingBoxCollision, true);
   EWorldEditor.defaultHandle = getPrefSetting($pref::WorldEditor::defaultHandle, "gui/Editor_DefaultHandle.png");
   EWorldEditor.dragRectColor = getPrefSetting($pref::WorldEditor::dragRectColor, "255 255 0");
   EWorldEditor.dropType = getPrefSetting($pref::WorldEditor::dropType, "screenCenter");
   EWorldEditor.faceSelectColor = getPrefSetting($pref::WorldEditor::faceSelectColor, "0 0 100 100");
   EWorldEditor.gridColor = getPrefSetting($pref::WorldEditor::gridColor, "255 255 255 20");
   EWorldEditor.gridSize = getPrefSetting($pref::WorldEditor::gridSize, "10 10 10");
   EWorldEditor.lockedHandle = getPrefSetting($pref::WorldEditor::lockedHandle, "gui/Editor_LockedHandle.png");
   EWorldEditor.maxScaleFactor = getPrefSetting($pref::WorldEditor::maxScaleFactor, 4000);
   EWorldEditor.minScaleFactor = getPrefSetting($pref::WorldEditor::minScaleFactor, 0.1);
   EWorldEditor.mouseMoveScale = getPrefSetting($pref::WorldEditor::mouseMoveScale, 1);
   EWorldEditor.mouseRotateScale = getPrefSetting($pref::WorldEditor::mouseRotateScale, 0.01);
   EWorldEditor.mouseScaleScale = getPrefSetting($pref::WorldEditor::mouseScaleScale, 0.01);
   EWorldEditor.objectsUseBoxCenter = getPrefSetting($pref::WorldEditor::objectsUseBoxCenter, true);
   EWorldEditor.objectTextColor = getPrefSetting($pref::WorldEditor::objectTextColor, "255 255 255");
   EWorldEditor.objMouseOverColor = getPrefSetting($pref::WorldEditor::objMouseOverColor, "0 255 0");
   EWorldEditor.objMouseOverSelectColor = getPrefSetting($pref::WorldEditor::objMouseOverSelectColor, "0 0 255");
   EWorldEditor.objSelectColor = getPrefSetting($pref::WorldEditor::objSelectColor, "255 0 0");
   EWorldEditor.planarMovement = getPrefSetting($pref::WorldEditor::planarMovement, true);
   EWorldEditor.planeDim = getPrefSetting($pref::WorldEditor::planeDim, 500);
   EWorldEditor.popupBackgroundColor = getPrefSetting($pref::WorldEditor::popupBackgroundColor, "100 100 100");
   EWorldEditor.popupTextColor = getPrefSetting($pref::WorldEditor::popupTextColor, "255 255 0");
   EWorldEditor.projectDistance = getPrefSetting($pref::WorldEditor::projectDistance, 2000);
   EWorldEditor.renderObjHandle = getPrefSetting($pref::WorldEditor::renderObjHandle, true);
   EWorldEditor.renderObjText = getPrefSetting($pref::WorldEditor::renderObjText, true);
   EWorldEditor.renderPlane = getPrefSetting($pref::WorldEditor::renderPlane, true);
   EWorldEditor.renderPlaneHashes = getPrefSetting($pref::WorldEditor::renderPlaneHashes, true);
   EWorldEditor.renderPopupBackground = getPrefSetting($pref::WorldEditor::renderPopupBackground, true);
   EWorldEditor.renderSelectionBox = getPrefSetting($pref::WorldEditor::renderSelectionBox, true);
   EWorldEditor.rotationSnap = getPrefSetting($pref::WorldEditor::rotationSnap, "15");
   EWorldEditor.selectHandle = getPrefSetting($pref::WorldEditor::selectHandle, "gui/Editor_SelectHandle.png");
   EWorldEditor.selectionBoxColor = getPrefSetting($pref::WorldEditor::selectionBoxColor, "255 255 0");
   EWorldEditor.showMousePopupInfo = getPrefSetting($pref::WorldEditor::showMousePopupInfo, true);
   EWorldEditor.snapRotations = getPrefSetting($pref::WorldEditor::snapRotations, false);
   EWorldEditor.snapToGrid = getPrefSetting($pref::WorldEditor::snapToGrid, false);
   EWorldEditor.undoLimit = getPrefSetting($pref::WorldEditor::undoLimit, 40);

   ETerrainEditor.softSelecting = 1;
   ETerrainEditor.currentAction = "raiseHeight";
   ETerrainEditor.currentMode = "select";
}

function EditorGui::setPrefs()
{
   $pref::WorldEditor::axisGizmoActive = EWorldEditor.axisGizmoActive;
   $pref::WorldEditor::axisGizmoMaxScreenLen = EWorldEditor.axisGizmoMaxScreenLen;
   $pref::WorldEditor::boundingBoxCollision = EWorldEditor.boundingBoxCollision;
   $pref::WorldEditor::defaultHandle = EWorldEditor.defaultHandle;
   $pref::WorldEditor::dragRectColor = EWorldEditor.dragRectColor;
   $Pref::WorldEditor::dropType = EWorldEditor.dropType;
   $pref::WorldEditor::gridColor = EWorldEditor.GridColor;
   $pref::WorldEditor::gridSize = EWorldEditor.GridSize;
   $pref::WorldEditor::lockedHandle = EWorldEditor.lockedHandle;
   $pref::WorldEditor::maxScaleFactor = EWorldEditor.maxScaleFactor;
   $pref::WorldEditor::minScaleFactor = EWorldEditor.minScaleFactor;
   $pref::WorldEditor::mouseMoveScale = EWorldEditor.mouseMoveScale;
   $pref::WorldEditor::mouseRotateScale = EWorldEditor.mouseRotateScale;
   $pref::WorldEditor::mouseScaleScale = EWorldEditor.mouseScaleScale;
   $pref::WorldEditor::objectsUseBoxCenter = EWorldEditor.objectsUseBoxCenter;
   $pref::WorldEditor::objectTextColor = EWorldEditor.ObjectTextColor;
   $pref::WorldEditor::objMouseOverColor = EWorldEditor.objMouseOverColor;
   $pref::WorldEditor::objMouseOverSelectColor = EWorldEditor.objMouseOverSelectColor;
   $pref::WorldEditor::objSelectColor = EWorldEditor.objSelectColor;
   $pref::WorldEditor::planarMovement = EWorldEditor.planarMovement;
   $pref::WorldEditor::planeDim = EWorldEditor.planeDim;
   $pref::WorldEditor::popupBackgroundColor = EWorldEditor.PopupBackgroundColor;
   $pref::WorldEditor::popupTextColor = EWorldEditor.PopupTextColor;
   $pref::WorldEditor::projectDistance = EWorldEditor.projectDistance;
   $pref::WorldEditor::raceSelectColor = EWorldEditor.faceSelectColor;
   $pref::WorldEditor::renderObjHandle = EWorldEditor.renderObjHandle;
   $pref::WorldEditor::renderObjText = EWorldEditor.renderObjText;
   $pref::WorldEditor::renderPlane = EWorldEditor.renderPlane;
   $pref::WorldEditor::renderPlaneHashes = EWorldEditor.renderPlaneHashes;
   $pref::WorldEditor::renderPopupBackground = EWorldEditor.renderPopupBackground;
   $pref::WorldEditor::renderSelectionBox = EWorldEditor.renderSelectionBox;
   $pref::WorldEditor::rotationSnap = EWorldEditor.rotationSnap;
   $pref::WorldEditor::selectHandle = EWorldEditor.selectHandle;
   $pref::WorldEditor::selectionBoxColor = EWorldEditor.selectionBoxColor;
   $pref::WorldEditor::showMousePopupInfo = EWorldEditor.showMousePopupInfo;
   $pref::WorldEditor::snapRotations = EWorldEditor.snapRotations;
   $pref::WorldEditor::snapToGrid = EWorldEditor.snapToGrid;
   $pref::WorldEditor::undoLimit = EWorldEditor.undoLimit;
}
