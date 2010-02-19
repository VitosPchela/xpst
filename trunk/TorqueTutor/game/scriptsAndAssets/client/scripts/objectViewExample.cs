//-----------------------------------------------------------------------------
// Torque Game Engine
// 
// Copyright (c) 2001 GarageGames.Com
// Portions Copyright (c) 2001 by Sierra Online, Inc.
//-----------------------------------------------------------------------------

//------------------------------------------------------------------------------
function objectViewExample::onAdd(%this)
{
   
}

//------------------------------------------------------------------------------
function objectViewExample::onWake(%this)
{
   // Set main object
   view.setModel("demo/data/shapes/spaceorc/player.dts");

   // Load dsq's
   view.loadDSQ("player","fps/data/shapes/player/player_root.dsq");
   view.loadDSQ("player","fps/data/shapes/player/player_forward.dsq");
   view.loadDSQ("player","fps/data/shapes/player/player_back.dsq");
   view.loadDSQ("player","fps/data/shapes/player/player_jump.dsq");

   // Set animation
   view.setSequence("player", "Root", 1);
}

//------------------------------------------------------------------------------
function objectViewExample::onSleep(%this)
{
   view.setEmpty();
}