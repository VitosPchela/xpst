//-----------------------------------------------------------------------------
// Structures                                                                  
//-----------------------------------------------------------------------------
#define IN_HLSL
#include "../shdrConsts.h"
#include "../lightingSystem/shdrLib.h"

struct ConnectData
{
   float3 lightingCoord   : TEXCOORD0;
};

struct Fragout
{
   float4 col : COLOR0;
};


//-----------------------------------------------------------------------------
// Main                                                                        
//-----------------------------------------------------------------------------
Fragout main( ConnectData IN,			  
              uniform sampler3D dlightMap       : register(S0),
              uniform float4    lightColor      : register(PC_DIFF_COLOR)
              )
{
   Fragout OUT;
   
   OUT.col = getDynamicLighting(dlightMap, IN.lightingCoord, lightColor);
   // alpha is sum(r,g,b) so we can filter unlit pixels with alpha test.
   OUT.col.w = OUT.col.x + OUT.col.y + OUT.col.z;
   
   return OUT;
}
