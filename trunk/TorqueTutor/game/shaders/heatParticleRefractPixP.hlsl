#define IN_HLSL
#include "shdrConsts.h"

//-----------------------------------------------------------------------------
// Structures                                                                  
//-----------------------------------------------------------------------------
struct ConnectData
{
   float2 texCoord        : TEXCOORD0;
   float3 eyePos          : TEXCOORD1;
   float3 normal          : TEXCOORD2;
   float3 pos             : TEXCOORD3;
   float4 hpos2           : TEXCOORD4;
   float3 screenNorm      : TEXCOORD5;
   float3 lightVec        : TEXCOORD6;
   float4 vertColor	  : COLOR;
};


struct Fragout
{
   float4 col : COLOR0;
};


//-----------------------------------------------------------------------------
// Main                                                                        
//-----------------------------------------------------------------------------
Fragout main( ConnectData IN,
              uniform sampler2D refractMap   : register(S0),
              uniform sampler2D bumpMap      : register(S1),
              uniform float4 specularColor   : register(PC_MAT_SPECCOLOR),
              uniform float  specularPower   : register(PC_MAT_SPECPOWER)
)
{
   Fragout OUT;
   
   float4 bumpNorm = tex2D( bumpMap, IN.texCoord );
   bumpNorm.xyz = bumpNorm.xyz * 2.0 - 1.0;
   
   float3 refractVec = refract( float3( 0.0, 0.0, 1.0), normalize( bumpNorm.xyz ), 1.0 );
  
   float2 tc;
   tc = float2( (  IN.hpos2.x + (refractVec.x * IN.vertColor.a * bumpNorm.a) ) / (IN.hpos2.w), 
                ( -IN.hpos2.y + (refractVec.y * IN.vertColor.a * bumpNorm.a) ) / (IN.hpos2.w) );

   tc = saturate( (tc + 1.0) * 0.5 );
   OUT.col = tex2D( refractMap, tc );
   
   return OUT;
}

