//-----------------------------------------------------------------------------
// Torque Game Engine Advanced
// Copyright (C) GarageGames.com, Inc.
//-----------------------------------------------------------------------------

/// This is the max number of ground cover types 
/// supported by the GroundCover object and the shader.
#define MAX_COVERTYPES    6

#ifdef IN_HLSL

// This is the shader input vertex structure.
struct Vert
{
   // .xyz  = point
   float3 position : POSITION;

   // .x    = size x
   // .y    = size y
   // .z    = type index
   // .w    = wind attenuation
   float4 params : TEXCOORD0;

   // .rgb
   // .a    = corner index
   float4 ambient : COLOR;	
};

// Shader Connect Structures
struct GroundCoverVOut
{
   float4 position : POSITION;
   float4 ambient  : COLOR;
   float2 texCoord : TEXCOORD0;
   float4 osPos    : TEXCOORD1;
};

struct GroundCoverPIn
{
   float4 ambient  : COLOR;
   float2 texCoord : TEXCOORD0; 
   float4 osPos    : TEXCOORD1;
};

//static float sPi     = 3.14159265f;
//static float sTwoPi  = 6.28318530f;

static float sCornerRight[4] = { -0.5, 0.5, 0.5, -0.5 };
static float sCornerUp[4] = { 0, 0, 1, 1 };

static float sMovableCorner[4] = { 0, 0, 1, 1 };

static float2 sUVCornerExtent[4] =
{ 
   float2( 0, 1 ),
   float2( 1, 1 ), 
   float2( 1, 0 ), 
   float2( 0, 0 )
};

///////////////////////////////////////////////////////////////////////////////
// The following wind effect was derived from the GPU Gems 3 chapter...
//
// "Vegetation Procedural Animation and Shading in Crysis"
// by Tiago Sousa, Crytek
//

float2 smoothCurve( float2 x )
{
   return x * x * ( 3.0 - 2.0 * x );
}

float2 triangleWave( float2 x )
{
   return abs( frac( x + 0.5 ) * 2.0 - 1.0 );
}

float2 smoothTriangleWave( float2 x )
{
   return smoothCurve( triangleWave( x ) );
}

float windTurbulence( float bbPhase, float frequency, float strength )
{
   // We create the input value for wave generation from the frequency and phase.
   float2 waveIn = bbPhase.xx + frequency.xx;

   // We use two square waves to generate the effect which
   // is then scaled by the overall strength.
   float2 waves = ( frac( waveIn.xy * float2( 1.975, 0.793 ) ) * 2.0 - 1.0 );
   waves = smoothTriangleWave( waves );

   // Sum up the two waves into a single wave.
   return ( waves.x + waves.y ) * strength;
}

float2 windEffect(   float bbPhase, 
                     float2 windDirection,
                     float gustLength,
                     float gustFrequency,
                     float gustStrength,
                     float turbFrequency,
                     float turbStrength )
{
   // Calculate the ambient wind turbulence.
   float turbulence = windTurbulence( bbPhase, turbFrequency, turbStrength );

   // We simulate the overall gust via a sine wave.
   float gustPhase = clamp( sin( ( bbPhase - gustFrequency ) / gustLength ) , 0, 1 );
   float gustOffset = ( gustPhase * gustStrength ) + ( ( 0.2 + gustPhase ) * turbulence );

   // Return the final directional wind effect.
   return gustOffset.xx * windDirection.xy;
}

#endif // IN_HLSL
