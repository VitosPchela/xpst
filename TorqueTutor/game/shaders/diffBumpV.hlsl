#include "hlslStructs.h"

//-----------------------------------------------------------------------------
// Structures                                                                  
//-----------------------------------------------------------------------------

struct ConnectData
{
   float4 hpos            : POSITION;
   float2 outTexCoord     : TEXCOORD0;
   float2 bumpCoord       : TEXCOORD1;
   float3 outLightVec     : TEXCOORD2;
};


//-----------------------------------------------------------------------------
// Main                                                                        
//-----------------------------------------------------------------------------
ConnectData main( VertexIn_PNTTTB IN,
                  uniform float4x4 modelview       : register(C0),
                  uniform float4x4 texMat          : register(C4),
                  uniform float3   inLightVec      : register(C24),
                  uniform float3   eyePos          : register(C20),
                  uniform float4x4 objTrans        : register(C12)
)
{
   ConnectData OUT;

   OUT.hpos = mul(modelview, IN.pos);
   
   float4 texCoordExtend = float4( IN.uv0, 0.0, 1.0 );
   OUT.outTexCoord = mul(texMat, texCoordExtend);
   OUT.bumpCoord = OUT.outTexCoord;

   float3x3 objToTangentSpace;
   objToTangentSpace[0] = IN.T;
   objToTangentSpace[1] = IN.B;
   objToTangentSpace[2] = IN.normal;

   OUT.outLightVec.xyz = -inLightVec;
   OUT.outLightVec.xyz = mul(objToTangentSpace, OUT.outLightVec);
   OUT.outLightVec = OUT.outLightVec / 2.0 + 0.5;

   return OUT;
}
