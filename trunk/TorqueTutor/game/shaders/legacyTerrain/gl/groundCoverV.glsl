//-----------------------------------------------------------------------------
// Torque Game Engine Advanced
// Copyright (C) GarageGames.com, Inc.
//-----------------------------------------------------------------------------

#define MAX_COVERTYPES    6

uniform mat4 modelview;
uniform vec4 typeRects[MAX_COVERTYPES];
uniform vec3 camRight, camUp, camPos, terrainInfo, gustInfo;
uniform vec2 fadeParams, windDir, turbInfo;

varying vec4 ambient;
varying vec2 outTexCoord;

///////////////////////////////////////////////////////////////////////////////
// The following wind effect was derived from the GPU Gems 3 chapter...
//
// "Vegetation Procedural Animation and Shading in Crysis"
// by Tiago Sousa, Crytek
//

vec2 smoothCurve( vec2 x )
{
   return x * x * ( 3.0 - 2.0 * x );
}

vec2 triangleWave( vec2 x )
{
   return abs( fract( x + 0.5 ) * 2.0 - 1.0 );
}

vec2 smoothTriangleWave( vec2 x )
{
   return smoothCurve( triangleWave( x ) );
}

float windTurbulence( float bbPhase, float frequency, float strength )
{
   // We create the input value for wave generation from the frequency and phase.
   vec2 waveIn = vec2(bbPhase + frequency, bbPhase + frequency);

   // We use two square waves to generate the effect which
   // is then scaled by the overall strength.
   vec2 waves = ( fract( waveIn.xy * vec2( 1.975, 0.793 ) ) * 2.0 - 1.0 );
   waves = smoothTriangleWave( waves );

   // Sum up the two waves into a single wave.
   return ( waves.x + waves.y ) * strength;
}

vec2 windEffect(   float bbPhase, 
                   vec2  windDirection,
                   float gustLength,
                   float gustFrequency,
                   float gustStrength,
                   float turbFrequency,
                   float turbStrength )
{
   // Calculate the ambient wind turbulence.
   float turbulence = windTurbulence( bbPhase, turbFrequency, turbStrength );

   // We simulate the overall gust via a sine wave.
   float gustPhase = clamp( sin( ( bbPhase - gustFrequency ) / gustLength ) , 0.0, 1.0 );
   float gustOffset = ( gustPhase * gustStrength ) + ( ( 0.2 + gustPhase ) * turbulence );

   // Return the final directional wind effect.
   return vec2(gustOffset * windDirection.x, gustOffset * windDirection.y);
}

void main()         
{
   // Some useful variables
   float sCornerRight[4];
   sCornerRight[0] = -0.5;
   sCornerRight[1] =  0.5;
   sCornerRight[2] =  0.5;
   sCornerRight[3] = -0.5;

   float sCornerUp[4];
   sCornerUp[0] = 0.0;
   sCornerUp[1] = 0.0;
   sCornerUp[2] = 1.0;
   sCornerUp[3] = 1.0;

   vec2 sUVCornerExtent[4];
   sUVCornerExtent[0] = vec2( 0.0, 1.0 );
   sUVCornerExtent[1] = vec2( 1.0, 1.0 );
   sUVCornerExtent[2] = vec2( 1.0, 0.0 );
   sUVCornerExtent[3] = vec2( 0.0, 0.0 );

   // Pull some of the parameters for clarity.     
   float    fadeStart      = fadeParams.x;
   float    fadeEnd        = fadeParams.y;
   float    fadeRange      = fadeEnd - fadeStart;

   //float    maxFadeJitter  = ( fadeEnd - fadeStart ) / 2.0;    
   int      corner      = int(( gl_Color.a * 255.0 ) + 0.5);
   vec2     size        = gl_MultiTexCoord0.xy;  
   int      typeIndex   = int(gl_MultiTexCoord0.z);
           
   // The billboarding is based on the camera direction.
   vec3 rightVec   = camRight * sCornerRight[corner];
   vec3 upVec      = camUp * sCornerUp[corner];               

   // Figure out the corner position.
   vec4 outPoint;
   outPoint.xyz = ( upVec * size.y ) + ( rightVec * size.x );
   float len = length( outPoint.xyz );
   
   // We derive the billboard phase used for wind calculations from its position.
   float bbPhase = dot( gl_Vertex.xyz, vec3(1.0) );

   // Get the overall wind gust and turbulence effects.
   // gustInfo.x = gust length
   // gustInfo.y = premultiplied simulation time and gust frequency
   // gustInfo.z = gust strength
   // turbInfo.x = premultiplied simulation time and turbulance frequency
   // turbInfo.y = turbulance strength
   vec3 wind;
   wind.xy = windEffect(   bbPhase,
                           windDir,
                           gustInfo.x, gustInfo.y, gustInfo.z,
                           turbInfo.x, turbInfo.y );
   wind.z = 0.0;

   // Add the summed wind effect into the point.
   outPoint.xyz += wind.xyz * gl_MultiTexCoord0.w;

   // Do a simple spherical clamp to keep the foliage
   // from stretching too much by wind effect.
   outPoint.xyz = normalize( outPoint.xyz ) * len;

   // Move the point into world space.
   outPoint.xyz += gl_Vertex.xyz;
   outPoint.w = 1.0;

   // Grab the uv set and setup the texture coord.
   vec4 uvSet = typeRects[typeIndex]; 
   vec2 texCoord;
   texCoord.x = uvSet.x + ( uvSet.z * sUVCornerExtent[corner].x );
   texCoord.y = uvSet.y + ( uvSet.w * sUVCornerExtent[corner].y );

   // Get the alpha fade value.
   float dist = distance( camPos, outPoint.xyz ) - fadeStart;
   float alpha = 1.0 - clamp( dist / fadeRange, 0.0, 1.0 );

   // Setup the shader output data.
   gl_Position = modelview * outPoint;
   outTexCoord = texCoord;
   ambient = vec4( gl_Color.rgb, alpha );
}

