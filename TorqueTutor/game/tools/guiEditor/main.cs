//-----------------------------------------------------------------------------
// Torque Game Builder
// Copyright (C) GarageGames.com, Inc.
//-----------------------------------------------------------------------------

function initializeGuiEditor()
{
   echo(" % - Initializing Gui Builder");
   
   // Load Client Scripts.
   exec("./gui/guiEditor.ed.gui");
   exec("./gui/guiEditor.ed.cs");
   exec("./gui/newGuiDialog.ed.gui");
   exec("./gui/fileDialogs.ed.cs");
   exec("./gui/guiEditorPrefsDlg.ed.gui");
   exec("./gui/guiEditorPrefsDlg.ed.cs");
   exec("./gui/guiEditorPalette.ed.gui");
   exec("./gui/guiEditorUndo.ed.cs");
   exec("./scripts/guiEditorCanvas.ed.cs");
}

function destroyGuiEditor()
{
}
