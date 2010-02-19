#define IN_HLSL
#include "shdrConsts.h"
#include "hlslStructs.h"

//-----------------------------------------------------------------------------
// Structures                                                                  
//-----------------------------------------------------------------------------
struct VertData
{
   float2 texCoord        : TEXCOORD0;
   float2 lmCoord         : TEXCOORD1;
   float3 T               : TEXCOORD2;
   float3 B               : TEXCOORD3;
   float3 normal          : NORMAL;
   float4 position        : POSITION;
};


struct ConnectData
{
   float4 hpos            : POSITION;
   float2 texCoord        : TEXCOORD0;
   float2 gradiant        : TEXCOORD1;
   float4 specular        : COLOR0;
};

//-----------------------------------------------------------------------------
// Main                                                                        
//-----------------------------------------------------------------------------
ConnectData main( VertexIn_PNTTTB IN,
                  uniform float4x4 modelview       : register(VC_WORLD_PROJ),
                  uniform float3   eyePos          : register(VC_EYE_POS),
                  uniform float3   inLightVec      : register(VC_LIGHT_DIR1)
)
{
   ConnectData OUT;

   OUT.hpos = mul(modelview, IN.pos);
   
   float3 screenNorm = mul( modelview, IN.normal );
   float3 refractVec = refract( float3( 0.0, 0.0, 1.0 ), screenNorm, 0.7 );
   
   OUT.texCoord = float2( ( OUT.hpos.x + refractVec.x ) / (OUT.hpos.w), 
                          ( -OUT.hpos.y + refractVec.y ) / (OUT.hpos.w) );
   
   OUT.texCoord = saturate( (OUT.texCoord + 1.0) * 0.5 );
   
   float3 eyeToVert = normalize( eyePos - IN.pos );
   OUT.gradiant.x = saturate( dot( IN.normal, eyeToVert ) ) - (0.5/128.0);
   OUT.gradiant.y = 0.0;

   // Specular 
   float3 eyeVec = normalize( eyePos - IN.pos );
   float3 halfAng = normalize(eyeVec + (-inLightVec) );
   float specular = saturate( dot(IN.normal.xyz, halfAng) );
   specular = pow( specular, 16.0 );
   OUT.specular = specular.xxxx * 0.85;
   
   return OUT;
}
