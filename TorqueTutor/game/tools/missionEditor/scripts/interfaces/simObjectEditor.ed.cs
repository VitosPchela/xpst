//-----------------------------------------------------------------------------
// Torque Game Engine
// Copyright (C) GarageGames.com, Inc.
//-----------------------------------------------------------------------------
// Define the field types for objects that link to the namespace MissionInfo
function SimObject::onDefineFieldTypes( %this )
{
   %this.setFieldType("Locked", "TypeBool");   
}