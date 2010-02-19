//*****************************************************************************
// Lightmap / cubemap shader
//*****************************************************************************
#define IN_HLSL
#include "shdrConsts.h"
#include "hlslStructs.h"

//-----------------------------------------------------------------------------
// Constants
//-----------------------------------------------------------------------------

struct Conn
{
   float4 HPOS             : POSITION;
	float2 TEX0             : TEXCOORD0;
	float4 tangentToCube0   : TEXCOORD1;
	float4 tangentToCube1   : TEXCOORD2;
	float4 tangentToCube2   : TEXCOORD3;
};



//-----------------------------------------------------------------------------
// Main
//-----------------------------------------------------------------------------
Conn main( VertexIn_PNTTTB In, 
           uniform float4x4 modelview : register(VC_WORLD_PROJ),
           uniform float3x3 cubeTrans : register(VC_CUBE_TRANS),
           uniform float3   cubeEyePos : register(VC_CUBE_EYE_POS)
)
{
   Conn Out;

   Out.HPOS = mul(modelview, In.pos);
   Out.TEX0 = In.uv0;

   
	float3x3 objToTangentSpace;
	objToTangentSpace[0] = In.T;
	objToTangentSpace[1] = In.B;
	objToTangentSpace[2] = In.normal;
   
   
   Out.tangentToCube0.xyz = mul( objToTangentSpace, cubeTrans[0].xyz );
   Out.tangentToCube1.xyz = mul( objToTangentSpace, cubeTrans[1].xyz );
   Out.tangentToCube2.xyz = mul( objToTangentSpace, cubeTrans[2].xyz );
   
   float3 pos = mul( cubeTrans, In.pos ).xyz;
   float3 eye = cubeEyePos - pos;
   normalize( eye );

   Out.tangentToCube0.w = eye.x;
   Out.tangentToCube1.w = eye.y;
   Out.tangentToCube2.w = eye.z;

   return Out;
}

