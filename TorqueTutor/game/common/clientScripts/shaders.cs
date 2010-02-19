//-----------------------------------------------------------------------------
// Torque Shader Engine
// Copyright (C) GarageGames.com, Inc.
//-----------------------------------------------------------------------------

//-----------------------------------------------------------------------------
//  This file contains shader data necessary for various engine utility functions
//-----------------------------------------------------------------------------

// Build our Synapse Lighting Kit Shaders
exec("common/lighting/sgShaders.cs");


new ShaderData( _DebugInterior_ )
{
   DXVertexShaderFile   = "shaders/debugInteriorsV.hlsl";
   DXPixelShaderFile    = "shaders/debugInteriorsP.hlsl";
   
   OGLVertexShaderFile   = "shaders/gl/debugInteriorsV.glsl";
   OGLPixelShaderFile    = "shaders/gl/debugInteriorsP.glsl";
   
   samplerNames[0] = "$diffuseMap";
   
   pixVersion = 1.1;
};

new ShaderData( GroundCoverShaderData )
{
   DXVertexShaderFile     = "shaders/legacyTerrain/groundCoverV.hlsl";
   DXPixelShaderFile      = "shaders/legacyTerrain/groundCoverP.hlsl";
   
   OGLVertexShaderFile     = "shaders/legacyTerrain/gl/groundCoverV.glsl";
   OGLPixelShaderFile      = "shaders/legacyTerrain/gl/groundCoverP.glsl";
   
   samplerNames[0] = "$diffuseMap";
   
   pixVersion = 2.0;
};
