//-----------------------------------------------------------------------------
// Torque Game Engine Advanced
// Copyright (C) GarageGames.com, Inc.
//-----------------------------------------------------------------------------

#define IN_HLSL
#include "groundCover.h"


float4 main(   GroundCoverPIn In,
               uniform sampler2D diffuseMap : register(S0) ) : COLOR
{
   // Why the 2x?  In the brightest areas the lightmap value is 2/3s of the
   // sunlight color and in the darkest areas its 1/2 of the ambient value.
   // So the 2x ensures that the ambient doesn't get darker than it should
   // at the expense of a little over brightening in the sunlight.
   float3 diffuse = saturate( In.ambient.rgb * 2 );

   // Return the final color.
   return tex2D( diffuseMap, In.texCoord ) * float4( diffuse, In.ambient.a );
}
