//-----------------------------------------------------------------------------
// Torque Game Engine Advanced
// Copyright (C) GarageGames.com, Inc.
//-----------------------------------------------------------------------------

// The master server is declared with the server defaults, which is
// loaded on both clients & dedicated servers.  If the server mod
// is not loaded on a client, then the master must be defined. 
// $pref::Master[0] = "2:master.garagegames.com:28002";

$pref::Player::Name = "Visitor";
$pref::Player::defaultFov = 90;
$pref::Player::zoomSpeed = 0;

$pref::Net::LagThreshold = 400;
$pref::Net::Port = 28000;

$pref::shadows = "2";
$pref::HudMessageLogSize = 40;
$pref::ChatHudLength = 1;

$pref::Input::LinkMouseSensitivity = 1;
// DInput keyboard, mouse, and joystick prefs
$pref::Input::KeyboardEnabled = 1;
$pref::Input::MouseEnabled = 1;
$pref::Input::JoystickEnabled = 0;
$pref::Input::KeyboardTurnSpeed = 0.1;

$pref::sceneLighting::cacheSize = 20000;
$pref::sceneLighting::purgeMethod = "lastCreated";
$pref::sceneLighting::cacheLighting = 1;
$pref::sceneLighting::terrainGenerateLevel = 1;

$pref::ts::detailAdjust = 0.45;

$pref::Terrain::DynamicLights = 1;
$pref::Interior::TexturedFog = 0;

$pref::Video::displayDevice = "D3D9";
$pref::Video::allowOpenGL = 0;
$pref::Video::allowD3D = 1;
$pref::Video::preferOpenGL = 0;
$pref::Video::appliedPref = 0;
$pref::Video::disableVerticalSync = 1;
$pref::Video::monitorNum = 0;
$pref::Video::windowedRes = "800 600";
$pref::Video::Resolution = "800 600";
$pref::Video::fullScreen = 0;
$pref::Video::BitsPerPixel = "32";
$pref::Video::RefreshRate = "60";
$pref::Video::FSAALevel = "0";
$pref::Video::MinBitsPerPixel = "32";
$pref::Video::defaultFenceCount = 0;
$pref::Video::screenShotSession = 0;
$pref::Video::screenShotFormat = "PNG";
$pref::Video::shaderPath = "shaders/procedural";
$pref::video::sfxBackBufferSize = "512";

$pref::VisibleDistanceMod = 1.0;


/// Causes the system to do a one time autodetect
/// of an SFX provider and device at startup if the
/// provider is unset.
$pref::SFX::autoDetect = true;

/// The sound provider to select at startup.  Typically
/// this is DirectSound, OpenAL, or XACT.  There is also 
/// a special Null provider which acts normally, but 
/// plays no sound.
$pref::SFX::provider = "";

/// The sound device to select from the provider.  Each
/// provider may have several different devices.
$pref::SFX::device = "";

/// If true the device will try to use hardware buffers
/// and sound mixing.  If not it will use software.
$pref::SFX::useHardware = false;

/// If you have a software device you have a 
/// choice of how many software buffers to
/// allow at any one time.  More buffers cost
/// more CPU time to process and mix.
$pref::SFX::maxSoftwareBuffers = 16;

/// This is the playback frequency for the primary 
/// sound buffer used for mixing.  Although most
/// providers will reformat on the fly, for best 
/// quality and performance match your sound files
/// to this setting.
$pref::SFX::frequency = 44100;

/// This is the playback bitrate for the primary 
/// sound buffer used for mixing.  Although most
/// providers will reformat on the fly, for best 
/// quality and performance match your sound files
/// to this setting.
$pref::SFX::bitrate = 32;

/// The overall system volume at startup.  Note that 
/// you can only scale volume down, volume does not
/// get louder than 1.
$pref::SFX::masterVolume = 0.8;

/// The startup sound channel volumes.  These are 
/// used to control the overall volume of different 
/// classes of sounds.
$pref::SFX::channelVolume1 = 1;
$pref::SFX::channelVolume2 = 1;
$pref::SFX::channelVolume3 = 1;
$pref::SFX::channelVolume4 = 1;
$pref::SFX::channelVolume5 = 1;
$pref::SFX::channelVolume6 = 1;
$pref::SFX::channelVolume7 = 1;
$pref::SFX::channelVolume8 = 1;


$pref::Editor::visibleDistance = "1000";

$pref::sgLightManager::AtlasMaxDynamicLights = "16";
$pref::sgLightManager::DynamicShadowDetailSize = "0";
$pref::sgLightManager::DynamicShadowQuality = "0";
$pref::sgLightManager::LightingProfileAllowShadows = "1";
$pref::sgLightManager::LightingProfileQuality = "0";
$pref::sgLightManager::MaxBestLights = "10";
$pref::sgLightManager::MultipleDynamicShadows = "1";
$pref::sgLightManager::ShowCacheStats = "0";
$pref::sgLightManager::UseBloom = "";
$pref::sgLightManager::UseDRLHighDynamicRange = "0";
$pref::sgLightManager::UseDynamicRangeLighting = "0";
$pref::sgLightManager::UseDynamicShadows = "1";
$pref::sgLightManager::UseToneMapping = "";
