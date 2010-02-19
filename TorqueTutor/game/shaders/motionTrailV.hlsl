#define IN_HLSL
#include "shdrConsts.h"

//-----------------------------------------------------------------------------
// Structures                                                                  
//-----------------------------------------------------------------------------
struct VertData
{
   float4 position        : POSITION;
   float4 shading         : COLOR;
};


struct ConnectData
{
   float4 hpos              : POSITION;
   float4 shading           : COLOR;
};
    

//-----------------------------------------------------------------------------
// Main                                                                        
//-----------------------------------------------------------------------------
ConnectData main( VertData IN,
                  uniform float4x4 modelview : register(VC_WORLD_PROJ),
                  uniform float3   eyePos    : register(VC_EYE_POS),
                  uniform float4x4 objTrans  : register(VC_OBJ_TRANS) )
{
   ConnectData OUT;
   
   OUT.hpos = mul(modelview, IN.position);
   OUT.shading = IN.shading;
   
   return OUT;
}
