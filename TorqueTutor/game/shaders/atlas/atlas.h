// Grab our shader constants.
#define IN_HLSL
#include "../shdrConsts.h"

#ifndef mapCount
#define mapCount 1
#endif

//-----------------------------------------------------------------------------
// Structures
//-----------------------------------------------------------------------------
struct VertData
{
   float4 position        : POSITION;  // Our base position.
   float2 texCoord        : TEXCOORD0; // Our texture co-ordinates.
};


struct VertClipMapConnectData
{
   float4 hpos               : POSITION;
   float4 fade               : COLOR0;
   float2 texCoord[mapCount] : TEXCOORD0;
};

struct PixClipMapConnectData
{
   float4 fade : COLOR;
   float2 texCoord[mapCount] : TEXCOORD0;
};

struct VertFogConnectData
{
   float4 hpos               : POSITION;
   float2 fogCoord           : TEXCOORD0;
};

struct PixFogConnectData
{
	float2 fogCoord          : TEXCOORD0;
};

struct VertDetailConnectData
{
   float4 hpos               : POSITION;
   float2 detCoord           : TEXCOORD0;
   float3  fade              : TEXCOORD1;
};

struct PixDetailConnectData
{
   float2 detCoord           : TEXCOORD0;
   float3  fade              : TEXCOORD1;
};

struct FragOut
{
   float4 col : COLOR;
};

//-----------------------------------------------------------------------------

#define fadeConstant 3.0

VertClipMapConnectData vertClipmap( VertData IN,
                  uniform float4x4 modelView         : register(C0),
                  uniform float4   mapInfo[mapCount] : register(C50)
                  )
{
   VertClipMapConnectData OUT;

   // Do vertex transform...
   OUT.hpos = mul( modelView, IN.position );

   // And get our morphed texcoord...
   float2 tc = IN.texCoord;

   // Scale texcoords.
   for(int i=0; i<mapCount; i++)
      OUT.texCoord[i] = tc * mapInfo[i].z;

   // Caculate fades. We never fade base level, so skip it.
   float4 distAcc = 0;
   for(int i=1; i<mapCount; i++)
      distAcc[i] = distance(mapInfo[i].xy, OUT.texCoord[i].xy);

   // Do all the fade biasing in one go.
   for(int i=0; i<4; i++)
      OUT.fade[i] = (distAcc[i] * (2.0 * fadeConstant) - (fadeConstant - 1.0)) / 2.0;

   return OUT;
}

FragOut pixClipmap( PixClipMapConnectData IN, uniform sampler2D diffuseMap[mapCount] : register(S0))
{

   FragOut OUT;

   // Do a layered blend into accumulator, so most detail when we have it will show through...
   OUT.col = tex2D(diffuseMap[0], IN.texCoord[0].xy);

   for(int i=1; i<mapCount; i++)
   {
      float  scaleFactor = saturate(IN.fade[i] * 2.0);
      float4 layer       = tex2D(diffuseMap[i], IN.texCoord[i].xy);
      OUT.col = lerp(layer, OUT.col , scaleFactor);
   }

   return OUT;
}

