#define IN_HLSL
#include "../shdrConsts.h"
#include "../lightingSystem/shdrLib.h"

//-----------------------------------------------------------------------------
// Structures                                                                  
//-----------------------------------------------------------------------------
#define IN_HLSL
#include "../shdrConsts.h"

struct ConnectData
{
   float2 texCoord        : TEXCOORD0;
   float2 fogCoord        : TEXCOORD1;
   float3 dlightCoord     : TEXCOORD2;
   float3 dlightMaskCoord : TEXCOORD3;
};


struct Fragout
{
   float4 col : COLOR0;
};


//-----------------------------------------------------------------------------
// Main                                                                        
//-----------------------------------------------------------------------------
Fragout main( ConnectData IN,
              uniform sampler2D diffuseMap      : register(S0),
              uniform sampler2D blackfogMap     : register(S1),
              uniform sampler3D dlightMap       : register(S2),
              uniform samplerCUBE dlightMask    : register(S3),
              uniform float4    lightColor      : register(PC_DIFF_COLOR)
              )
{
   Fragout OUT;
   
   float4 diffuseColor = tex2D(diffuseMap, IN.texCoord);
   diffuseColor *= getDynamicLighting(dlightMap, IN.dlightCoord, lightColor) * 2.0;
   diffuseColor *= texCUBE(dlightMask, IN.dlightMaskCoord);
   float4 fogColor = tex2D(blackfogMap, IN.fogCoord);
   OUT.col = lerp( diffuseColor, fogColor, fogColor.a );
   
   return OUT;
}
