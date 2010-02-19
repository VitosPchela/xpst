#define IN_HLSL
#include "../shdrConsts.h"
#include "atlas.h"

//-----------------------------------------------------------------------------
// Main
//-----------------------------------------------------------------------------
VertFogConnectData main( VertData IN,
                  uniform float4x4 modelView : register(C0),
                  uniform float4x4 objTrans  : register(VC_OBJ_TRANS),
                  uniform float3   eyePos    : register(VC_EYE_POS),
                  uniform float3   fogData   : register(VC_FOGDATA)
                  )
{
   VertFogConnectData OUT;

   // Transform and return.
   OUT.hpos = mul(modelView, IN.position);
 
   // For fog, do the fog calculation.
   float eyeDist = distance( IN.position, eyePos );
   float3 transPos = mul( objTrans, IN.position );
   
   OUT.fogCoord.x = 1.0 - ( eyeDist  / fogData.z );
   OUT.fogCoord.y = (transPos.z - fogData.x) * fogData.y;

   return OUT;
}
