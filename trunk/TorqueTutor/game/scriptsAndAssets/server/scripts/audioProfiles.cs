//-----------------------------------------------------------------------------
// Torque Game Engine 
// Copyright (C) GarageGames.com, Inc.
//-----------------------------------------------------------------------------

//-----------------------------------------------------------------------------
// 3D Sounds
//-----------------------------------------------------------------------------

//-----------------------------------------------------------------------------
// Single shot sounds

datablock SFXDescription(AudioDefault3d)
{
   volume   = 1.0;
   isLooping= false;

   is3D     = true;
   ReferenceDistance= 20.0;
   MaxDistance= 100.0;
   channel = $SimAudioType;
};

datablock SFXDescription(AudioClose3d)
{
   volume   = 1.0;
   isLooping= false;

   is3D     = true;
   ReferenceDistance= 10.0;
   MaxDistance= 60.0;
   channel = $SimAudioType;
};

datablock SFXDescription(AudioClosest3d)
{
   volume   = 1.0;
   isLooping= false;

   is3D     = true;
   ReferenceDistance= 5.0;
   MaxDistance= 30.0;
   channel = $SimAudioType;
};


//-----------------------------------------------------------------------------
// Looping sounds

datablock SFXDescription(AudioDefaultLooping3d)
{
   volume   = 1.0;
   isLooping= true;

   is3D     = true;
   ReferenceDistance= 20.0;
   MaxDistance= 100.0;
   channel = $SimAudioType;
};

datablock SFXDescription(AudioCloseLooping3d)
{
   volume   = 1.0;
   isLooping= true;

   is3D     = true;
   ReferenceDistance= 10.0;
   MaxDistance= 50.0;
   channel = $SimAudioType;
};

datablock SFXDescription(AudioClosestLooping3d)
{
   volume   = 1.0;
   isLooping= true;

   is3D     = true;
   ReferenceDistance= 5.0;
   MaxDistance= 30.0;
   channel = $SimAudioType;
};


//-----------------------------------------------------------------------------
// 2d sounds
//-----------------------------------------------------------------------------

// Used for non-looping environmental sounds (like power on, power off)
datablock SFXDescription(Audio2D)
{
   volume = 1.0;
   isLooping = false;
   is3D = false;
   channel = $SimAudioType;
};

// Used for Looping Environmental Sounds
datablock SFXDescription(AudioLooping2D)
{
   volume = 1.0;
   isLooping = true;
   is3D = false;
   channel = $SimAudioType;
};


//-----------------------------------------------------------------------------
datablock SFXProfile(takeme)
{
   filename = "~/data/sound/takeme";
   description = "AudioDefaultLooping3d";
	preload = false;
};
