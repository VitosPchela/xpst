//-----------------------------------------------------------------------------
// Torque Game Engine
// Copyright (C) GarageGames.com, Inc.
//-----------------------------------------------------------------------------

function AreaEditor::onUpdate(%this, %area)
{
   AreaEditingText.setValue( "X: " @ getWord(%area,0) @ " Y: " @ getWord(%area,1) @ " W: " @ getWord(%area,2) @ " H: " @ getWord(%area,3));
}

function AreaEditor::onWorldOffset(%this, %offset)
{
}
