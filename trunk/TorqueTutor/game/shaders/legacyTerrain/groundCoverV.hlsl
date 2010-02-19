//-----------------------------------------------------------------------------
// Torque Game Engine Advanced
// Copyright (C) GarageGames.com, Inc.
//-----------------------------------------------------------------------------

#define IN_HLSL
#include "groundCover.h"

GroundCoverVOut main( Vert In, 

		      uniform float4x4 modelview : register(C0),

            uniform float3 camRight : register(C6),	         
	         uniform float3 camUp    : register(C8),
	         uniform float3 camPos   : register(C9),

	         uniform float2 fadeParams : register(C10),
	         
	         uniform float3 terrainInfo : register(C11),

	         uniform float2 windDir : register(C12),
            
            // .x = gust length
            // .y = premultiplied simulation time and gust frequency
            // .z = gust strength
	         uniform float3 gustInfo : register(C13),

            // .x = premultiplied simulation time and turbulance frequency
            // .y = turbulance strength
	         uniform float2 turbInfo : register(C14),

	         uniform float4 typeRects[MAX_COVERTYPES] : register(C60) )	         
{

   // Pull some of the parameters for clarity.     
   float    fadeStart      = fadeParams.x;
   float    fadeEnd        = fadeParams.y;
   const float fadeRange   = fadeEnd - fadeStart;

   //float    maxFadeJitter  = ( fadeEnd - fadeStart ) / 2.0f;    
   int      corner      = ( In.ambient.a * 255.0f ) + 0.5f;
   float2   size        = In.params.xy;  
   int      typeIndex   = In.params.z;
           
   // The billboarding is based on the camera direction.
   float3 rightVec   = camRight * sCornerRight[corner];
   float3 upVec      = camUp * sCornerUp[corner];               

   // Figure out the corner position.
   float4 outPoint;
   outPoint.xyz = ( upVec * size.y ) + ( rightVec * size.x );
   float len = length( outPoint.xyz );
   
   // We derive the billboard phase used for wind calculations from its position.
   float bbPhase = dot( In.position.xyz, 1 );

   // Get the overall wind gust and turbulence effects.
   float3 wind;
   wind.xy = windEffect(   bbPhase,
                           windDir,
                           gustInfo.x, gustInfo.y, gustInfo.z,
                           turbInfo.x, turbInfo.y );
   wind.z = 0;

   // Add the summed wind effect into the point.
   outPoint.xyz += wind.xyz * In.params.w;

   // Do a simple spherical clamp to keep the foliage
   // from stretching too much by wind effect.
   outPoint.xyz = normalize( outPoint.xyz ) * len;

   // Move the point into world space.
   outPoint.xyz += In.position.xyz;
   outPoint.w = 1;

   // Grab the uv set and setup the texture coord.
   float4 uvSet = typeRects[typeIndex]; 
   float2 texCoord;
   texCoord.x = uvSet.x + ( uvSet.z * sUVCornerExtent[corner].x );
   texCoord.y = uvSet.y + ( uvSet.w * sUVCornerExtent[corner].y );

   // Get the alpha fade value.
   float dist = distance( camPos, outPoint.xyz ) - fadeStart;
   float alpha = 1 - clamp( dist / fadeRange, 0, 1 );

   // Setup the shader output data.
   GroundCoverVOut Out;              
   Out.position = mul( modelview, outPoint );
   Out.texCoord = texCoord;
   Out.ambient = float4( In.ambient.rgb, alpha );
   Out.osPos = float4( outPoint.xyz, 1.0 );

   return Out;
}

