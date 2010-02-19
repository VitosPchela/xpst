//-----------------------------------------------------------------------------
// Torque Builder 
// Copyright (C) GarageGames.com, Inc.
//-----------------------------------------------------------------------------

function initializeBase()
{
   echo(" % - Initializing Base Editor");
   
   // Load Custom Editors
   loadDirectory(expandFilename("./canvas"));
   loadDirectory(expandFilename("./menuBar"));
   
   // testBaseEditor();
   
}

function destroyMissionEditor()
{
}
