//-----------------------------------------------------------------------------
// Torque Game Engine 
// Copyright (C) GarageGames.com, Inc.
//-----------------------------------------------------------------------------

function initializeMissionEditor()
{
   echo(" % - Initializing Mission Editor");
   
   // Load GUI
   exec("./gui/profiles.ed.cs");
   exec("./scripts/cursors.ed.cs");
   
   exec("./gui/EditorGui.ed.gui");

   exec("./gui/guiTerrainEditorContainer.ed.gui"); 
   
   exec("./gui/objectBuilderGui.ed.gui");
   exec("./gui/TerrainEditorVSettingsGui.ed.gui");
   exec("./gui/WorldEditorSettingsDlg.ed.gui");
   
   exec("./gui/ParticleEditor.ed.gui");
   
   // Load Scripts.
   exec("./scripts/menus.ed.cs");
   exec("./scripts/menuHandlers.ed.cs");
   exec("./scripts/editor.ed.cs");
   exec("./scripts/editor.bind.ed.cs");
   exec("./scripts/EditorGui.ed.cs");
   exec("./scripts/editorPrefs.ed.cs");
   exec("./scripts/editorRender.ed.cs");

   // Load Custom Editors
   loadDirectory(expandFilename("./scripts/editors"));
   loadDirectory(expandFilename("./scripts/interfaces"));
   
   // Load Custom Interface Types
   exec("./scripts/particleEditor.ed.cs");
   
   // Load the light editor
   exec("./gui/lightEditor.ed.gui");
   exec("./scripts/lightEditor.ed.cs");
   exec("./gui/lightEditorNewDB.ed.gui");
}

function destroyMissionEditor()
{
}
