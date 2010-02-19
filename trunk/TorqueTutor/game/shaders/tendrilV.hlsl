#define IN_HLSL
#include "shdrConsts.h"
#include "hlslStructs.h"

//-----------------------------------------------------------------------------
// Structures                                                                  
//-----------------------------------------------------------------------------

struct ConnectData
{
   float4 hpos            : POSITION;
   float4 texCoord        : TEXCOORD0;
   float3 eyePos          : TEXCOORD1;
   float3 normal          : TEXCOORD2;
   float3 pos             : TEXCOORD3;
   float4 hpos2           : TEXCOORD4;
   float3 screenNorm      : TEXCOORD5;
   float3 lightVec        : TEXCOORD6;
   
};

//-----------------------------------------------------------------------------
// Main                                                                        
//-----------------------------------------------------------------------------
ConnectData main( VertexIn_PNTTTB IN,
                  uniform float4x4 modelview       : register(VC_WORLD_PROJ),
                  uniform float3   eyePos          : register(VC_EYE_POS),
                  uniform float3   inLightVec      : register(VC_LIGHT_DIR1),
                  uniform float4x4 texMat          : register(VC_TEX_TRANS1)
                  
)
{
   ConnectData OUT;

   OUT.hpos = mul(modelview, IN.pos);

   float4 texCoordExtend = float4( IN.uv0, 0.0, 1.0 );
   OUT.texCoord = mul(texMat, texCoordExtend) * 0.5;
   
   OUT.hpos2 = OUT.hpos;
   OUT.screenNorm = mul( modelview, IN.normal );
   OUT.eyePos = eyePos / 100.0;
   OUT.normal = IN.normal;
   OUT.pos    = IN.pos / 100.0;
   OUT.lightVec = -inLightVec;   
   
   return OUT;
}
