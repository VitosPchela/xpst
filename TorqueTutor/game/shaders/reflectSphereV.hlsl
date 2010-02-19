//*****************************************************************************
//*****************************************************************************
#include "hlslStructs.h"

//-----------------------------------------------------------------------------
// Structures                                                                  
//-----------------------------------------------------------------------------

struct ConnectData
{
   float4 hpos            : POSITION;
   float3 cubeNormal      : TEXCOORD0;
   float3 cubeEyePos      : TEXCOORD1;
   float3 pos             : TEXCOORD2;
};

/*
struct ConnectData
{
   float4 hpos            : POSITION;
   float3 reflectVec      : TEXCOORD0;
};
*/

//-----------------------------------------------------------------------------
// Main                                                                        
//-----------------------------------------------------------------------------
ConnectData main( VertexIn_PNTTTB IN,
                  uniform float4x4 modelview       : register(C0),
                  uniform float3x3 cubeTrans       : register(C16),
                  uniform float3   cubeEyePos      : register(C19),
                  uniform float3   eyePos          : register(C20)
)
{
   ConnectData OUT;


   OUT.hpos = mul(modelview, IN.pos);
   float3 normal = normalize( IN.pos );
   OUT.cubeNormal = normalize( mul(cubeTrans, normal).xyz );
   OUT.cubeEyePos = cubeEyePos / 100.0;
   OUT.pos = mul(cubeTrans, IN.pos / 100.0).xyz;


/*   
   OUT.hpos = mul(modelview, IN.position);
   float3 cubeNormal = normalize( mul(cubeTrans, IN.normal).xyz );
   float3 cubeVertPos = mul(cubeTrans, IN.position).xyz;
   float3 eyeToVert = cubeVertPos - cubeEyePos;
   OUT.reflectVec = reflect(eyeToVert, cubeNormal);
*/

   return OUT;
}
