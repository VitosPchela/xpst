#define IN_HLSL
#include "../shdrConsts.h"
#include "atlas.h"
#include "../lightingSystem/shdrLib.h"

struct VertLightConnectData
{
   float4 hpos : POSITION;
   float3 dlightCoord   : TEXCOORD0;
   float3 dlightMaskCoord : TEXCOORD1;
};

//-----------------------------------------------------------------------------
// Main
//-----------------------------------------------------------------------------
VertLightConnectData main( VertData IN,
                  uniform float4x4 modelView : register(C0),
                  uniform float3   eyePos    : register(VC_EYE_POS),
                  uniform float4x4 objTrans        : register(VC_OBJ_TRANS),              
                  uniform float4   lightPos        : register(VC_LIGHT_POS1),
                  uniform float4x4 lightingMatrix  : register(VC_LIGHT_TRANS)
                  )
{
   VertLightConnectData OUT;

   // Transform and return.
   OUT.hpos = mul(modelView, IN.position);

   // Let's get some lighting calcs in here.
   OUT.dlightCoord = getDynamicLightingCoord(IN.position, lightPos, objTrans, lightingMatrix);
   OUT.dlightMaskCoord = normalize(OUT.dlightCoord.xzy - 0.5);

   return OUT;
}
