//-----------------------------------------------------------------------------
// Torque Game Engine 
// Copyright (C) GarageGames.com, Inc.
//-----------------------------------------------------------------------------

//------------------------------------------------------------------------------
// Mission Editor Manager
new ActionMap(EditorMap);

//EditorMap.bindCmd(keyboard, "f2", "editor.setEditor(WorldEditor);", "");
//EditorMap.bindCmd(keyboard, "f3", "editor.setEditor(TerrainEditor);", "");
//EditorMap.bindCmd(keyboard, "f4", "editor.setEditor(Terraformer);", "");   
//EditorMap.bindCmd(keyboard, "f5", "editor.setEditor(AIEditor);", "");   

//EditorMap.bindCmd(keyboard, "alt s", "Canvas.pushDialog(EditorSaveMissionDlg);", "");
//EditorMap.bindCmd(keyboard, "alt r", "lightScene(\"\", forceAlways);", "");
EditorMap.bindCmd(keyboard, "escape", "editor.close();", "");

// These shortcuts conflict with menu shortcuts

// alt-#: set bookmark
//for(%i = 0; %i < 9; %i++)
   //EditorMap.bindCmd(keyboard, "alt " @ %i, "editor.setBookmark(" @ %i @ ");", "");

// ctrl-#: goto bookmark
//for(%i = 0; %i < 9; %i++)
   //EditorMap.bindCmd(keyboard, "ctrl " @ %i, "editor.gotoBookmark(" @ %i @ ");", "");


//------------------------------------------------------------------------------
// World Editor
new ActionMap(WorldEditorMap);
WorldEditorMap.bindCmd(keyboard, "space", "wEditor.nextMode();", "");

//WorldEditorMap.bindCmd(keyboard, "delete", "wEditor.copySelection();wEditor.deleteSelection();Inspector.uninspect();", "");
//WorldEditorMap.bindCmd(keyboard, "ctrl c", "wEditor.copySelection();", "");
//WorldEditorMap.bindCmd(keyboard, "ctrl x", "wEditor.copySelection();wEditor.deleteSelection();Inspector.uninspect();", "");
//WorldEditorMap.bindCmd(keyboard, "ctrl v", "wEditor.pasteSelection();", "");

//WorldEditorMap.bindCmd(keyboard, "ctrl z", "wEditor.undo();", "");
//WorldEditorMap.bindCmd(keyboard, "ctrl y", "wEditor.redo();", "");

//WorldEditorMap.bindCmd(keyboard, "ctrl h", "wEditor.hideSelection(true);", "");
//WorldEditorMap.bindCmd(keyboard, "alt h", "wEditor.hideSelection(false);", "");
//WorldEditorMap.bindCmd(keyboard, "ctrl d", "wEditor.dropSelection();", "");
//WorldEditorMap.bindCmd(keyboard, "ctrl q", "wEditor.dropCameraToSelection();", "");
//WorldEditorMap.bindCmd(keyboard, "ctrl m", "wEditor.moveSelectionInPlace();", "");
//WorldEditorMap.bindCmd(keyboard, "ctrl r", "wEditor.resetTransforms();", "");

WorldEditorMap.bindCmd(keyboard, "z", "EWorldEditor.toggleSnapToGrid();", "");

WorldEditorMap.bindCmd(keyboard, "i", "Canvas.pushDialog(interiorDebugDialog);", "");
WorldEditorMap.bindCmd(keyboard, "o", "Canvas.pushDialog(WorldEditorSettingsDlg);", "");

WorldEditorMap.bindCmd(keyboard, "numpad1", "EWorldEditorDisplayPopup.setSelected(0);", "");
WorldEditorMap.bindCmd(keyboard, "numpad2", "EWorldEditorDisplayPopup.setSelected(1);", "");
WorldEditorMap.bindCmd(keyboard, "numpad3", "EWorldEditorDisplayPopup.setSelected(2);", "");
WorldEditorMap.bindCmd(keyboard, "numpad4", "EWorldEditorDisplayPopup.setSelected(3);", "");
WorldEditorMap.bindCmd(keyboard, "numpad5", "EWorldEditorDisplayPopup.setSelected(4);", "");
WorldEditorMap.bindCmd(keyboard, "numpad6", "EWorldEditorDisplayPopup.setSelected(5);", "");
WorldEditorMap.bindCmd(keyboard, "numpad7", "EWorldEditorDisplayPopup.setSelected(6);", "");
WorldEditorMap.bindCmd(keyboard, "numpad8", "EWorldEditorDisplayPopup.setSelected(7);", "");


//------------------------------------------------------------------------------
// Terrain Editor
new ActionMap(TerrainEditorMap);

//TerrainEditorMap.bindCmd(keyboard, "ctrl z", "tEditor.undo();", "");
//TerrainEditorMap.bindCmd(keyboard, "ctrl y", "tEditor.redo();", "");

TerrainEditorMap.bindCmd(keyboard, "left", "tEditor.offsetBrush(-1, 0);", "");
TerrainEditorMap.bindCmd(keyboard, "right", "tEditor.offsetBrush(1, 0);", "");
TerrainEditorMap.bindCmd(keyboard, "up", "tEditor.offsetBrush(0, 1);", "");
TerrainEditorMap.bindCmd(keyboard, "down", "tEditor.offsetBrush(0, -1);", "");

// Revisit these
//TerrainEditorMap.bindCmd(keyboard, "1", "TERaiseHeightActionRadio.setValue(1);", "");
//TerrainEditorMap.bindCmd(keyboard, "2", "TELowerHeightActionRadio.setValue(1);", "");
//TerrainEditorMap.bindCmd(keyboard, "3", "TESetHeightActionRadio.setValue(1);", "");
//TerrainEditorMap.bindCmd(keyboard, "4", "TESetEmptyActionRadio.setValue(1);", "");
//TerrainEditorMap.bindCmd(keyboard, "5", "TEClearEmptyActionRadio.setValue(1);", "");
//TerrainEditorMap.bindCmd(keyboard, "6", "TEFlattenHeightActionRadio.setValue(1);", "");
//TerrainEditorMap.bindCmd(keyboard, "7", "TESmoothHeightActionRadio.setValue(1);", "");
//TerrainEditorMap.bindCmd(keyboard, "8", "TESetMaterialActionRadio.setValue(1);", "");
//TerrainEditorMap.bindCmd(keyboard, "9", "TEAdjustHeightActionRadio.setValue(1);", "");

// Conflict with camera speed
//TerrainEditorMap.bindCmd(keyboard, "shift 1", "tEditor.processUsesBrush = true;TERaiseHeightActionRadio.setValue(1);tEditor.processUsesBrush = false;", "");
//TerrainEditorMap.bindCmd(keyboard, "shift 2", "tEditor.processUsesBrush = true;TELowerHeightActionRadio.setValue(1);tEditor.processUsesBrush = false;", "");
//TerrainEditorMap.bindCmd(keyboard, "shift 3", "tEditor.processUsesBrush = true;TESetHeightActionRadio.setValue(1);tEditor.processUsesBrush = false;", "");
//TerrainEditorMap.bindCmd(keyboard, "shift 4", "tEditor.processUsesBrush = true;TESetEmptyActionRadio.setValue(1);tEditor.processUsesBrush = false;", "");
//TerrainEditorMap.bindCmd(keyboard, "shift 5", "tEditor.processUsesBrush = true;TEClearEmptyActionRadio.setValue(1);tEditor.processUsesBrush = false;", "");
//TerrainEditorMap.bindCmd(keyboard, "shift 6", "tEditor.processUsesBrush = true;TEFlattenHeightActionRadio.setValue(1);tEditor.processUsesBrush = false;", "");
//TerrainEditorMap.bindCmd(keyboard, "shift 7", "tEditor.processUsesBrush = true;TESmoothHeightActionRadio.setValue(1);tEditor.processUsesBrush = false;", "");
//TerrainEditorMap.bindCmd(keyboard, "shift 8", "tEditor.processUsesBrush = true;TESetMaterialActionRadio.setValue(1);tEditor.processUsesBrush = false;", "");
//TerrainEditorMap.bindCmd(keyboard, "shift 9", "tEditor.processUsesBrush = true;TEAdjustHeightActionRadio.setValue(1);tEditor.processUsesBrush = false;", "");

//TerrainEditorMap.bindCmd(keyboard, "h", "TESelectModeRadio.setValue(1);", "");
//TerrainEditorMap.bindCmd(keyboard, "j", "TEPaintModeRadio.setValue(1);", "");
//TerrainEditorMap.bindCmd(keyboard, "k", "TEAdjustModeRadio.setValue(1);", "");

TerrainEditorMap.bindCmd(keyboard, "i", "Canvas.pushDialog(interiorDebugDialog);", "");
TerrainEditorMap.bindCmd(keyboard, "o", "Canvas.pushDialog(TerrainEditorValuesSettingsGui, 99);", "");
TerrainEditorMap.bindCmd(keyboard, "m", "Canvas.pushDialog(TerrainEditorTextureSelectGui, 99);", "");

TerrainEditorMap.bindCmd(keyboard, "backspace", "tEditor.clearSelection();", "");

