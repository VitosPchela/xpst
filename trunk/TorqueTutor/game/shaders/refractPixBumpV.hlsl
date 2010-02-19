#define IN_HLSL
#include "shdrConsts.h"
#include "hlslStructs.h"

//-----------------------------------------------------------------------------
// Structures                                                                  
//-----------------------------------------------------------------------------

struct ConnectData
{
   float4 hpos            : POSITION;
   float4 pos             : TEXCOORD0;
   float2 texCoord        : TEXCOORD1;
};


//-----------------------------------------------------------------------------
// Save this! - my refraction
//-----------------------------------------------------------------------------
float3 myRefract( float3 inRay, float3 normal, float ri )
{
   return -(ri-1) * dot( inRay, normal ) * normal - inRay;
}


//-----------------------------------------------------------------------------
// Main                                                                        
//-----------------------------------------------------------------------------
ConnectData main( VertexIn_PNTTTB IN,
                  uniform float4x4 modelview       : register(VC_WORLD_PROJ),
                  uniform float3   eyePos          : register(VC_EYE_POS),
                  uniform float4x4 invTrans        : register(VC_TEX_TRANS2)
)
{
   ConnectData OUT;

   OUT.hpos = mul( modelview, IN.pos );
   OUT.pos = OUT.hpos;
   OUT.texCoord = IN.uv0;
   
   return OUT;
}
