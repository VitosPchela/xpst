//-----------------------------------------------------------------------------
// Torque Game Engine 
// Copyright (C) GarageGames.com, Inc.
// Environment Audio Profiles
//-----------------------------------------------------------------------------

//-----------------------------------------------------------------------------
// 3D Sounds
//-----------------------------------------------------------------------------

datablock SFXDescription(Shore01Looping3d)
{
  volume   = 1.0;
  isLooping= true;

  is3D     = true;
  ReferenceDistance = 20;
  maxDistance = 65;
  channel = $SimAudioType;
};

datablock SFXDescription(TreeGrove01Looping3d)
{
  volume   = 1.0;
  isLooping= true;

  is3D     = true;
  ReferenceDistance = 20;
  maxDistance = 80;
  channel = $SimAudioType;
};

datablock SFXDescription(Tree01Looping3d)
{
  volume   = 1.0;
  isLooping= true;

  is3D     = true;
  ReferenceDistance = 20;
  maxDistance = 60;
  channel = $SimAudioType;
};

datablock SFXDescription(Fire01Looping3d)
{
  volume   = 1.0;
  isLooping= true;

  is3D     = true;
  ReferenceDistance = 3;
  maxDistance = 10;
  channel = $SimAudioType;
};

//-----------------------------------------------------------------------------


datablock SFXProfile(Shore01Snd)
{
   fileName = "~/data/sound/Lakeshore_mono_01";
   description = "Shore01Looping3d";
	 preload = true;
};

datablock SFXProfile(TreeGrove01Snd)
{
   fileName = "~/data/sound/treegrove_mono_01";
   description = "TreeGrove01Looping3d";
	 preload = true;
};

datablock SFXProfile(Tree01Snd)
{
   fileName = "~/data/sound/tree_mono_01";
   description = "Tree01Looping3d";
	 preload = true;
};

datablock SFXProfile(Fire01Snd)
{
   fileName = "~/data/sound/Fire_Mono_01";
   description = "Fire01Looping3d";
	 preload = true;
};