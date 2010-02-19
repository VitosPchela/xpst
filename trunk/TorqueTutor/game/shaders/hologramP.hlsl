#define IN_HLSL
#include "shdrConsts.h"

//-----------------------------------------------------------------------------
// Data
//-----------------------------------------------------------------------------
struct v2f
{
   float4 HPOS             : POSITION;
	float4 TEX0             : TEXCOORD0;
	float4 tangentToCube0   : TEXCOORD1;
	float4 tangentToCube1   : TEXCOORD2;
	float4 tangentToCube2   : TEXCOORD3;
   float4 lightVec         : TEXCOORD4;
   float3 pixPos           : TEXCOORD5;
   float3 eyePos           : TEXCOORD6;
};



struct Fragout
{
   float4 col : COLOR0;
};

//-----------------------------------------------------------------------------
// Main
//-----------------------------------------------------------------------------
Fragout main(v2f IN,
		uniform sampler2D    diffMap  : register(S0),
		uniform samplerCUBE  cubeMap  : register(S1),
		uniform sampler2D    bumpMap  : register(S2),
		uniform sampler2D    diffMap2 : register(S3),
		uniform float4    specularColor   : register(PC_MAT_SPECCOLOR),
		uniform float     specularPower   : register(PC_MAT_SPECPOWER),
		uniform float4    ambient         : register(PC_AMBIENT_COLOR),
		uniform float accumTime   : register(PC_ACCUM_TIME)
)
{
	Fragout OUT;

	float2 texOffset, texOffset2;
	float sinOffset1 = sin( accumTime * 5.2 + IN.TEX0.y * 1.5);
	float sinOffset2 = sin( accumTime * 3.2 + IN.TEX0.y * 0.55);

	texOffset.x = IN.TEX0.x + (sinOffset1 + sinOffset2) * 0.005;
	texOffset.y = IN.TEX0.y;

	texOffset2.x = IN.TEX0.x + (sinOffset1 - sinOffset2) * 0.05;
	texOffset2.y = IN.TEX0.y + accumTime;

	float4 bumpNorm = tex2D( bumpMap, texOffset ) * 2.0 - 1.0;
	
	float4 diffA = tex2D( diffMap, texOffset );
	float4 diffB = tex2D(diffMap2, texOffset2);
	diffB.a = 0;
	float4 diffuse = diffA * 0.6 + diffB * 0.4;

	OUT.col = diffuse * (saturate( dot( IN.lightVec, bumpNorm.xyz ) ) + ambient);

	float3 eyeVec = normalize(IN.eyePos - IN.pixPos);
	float3 halfAng = normalize(eyeVec + IN.lightVec.xyz);
	float specular = saturate( dot(bumpNorm, halfAng) ) * IN.lightVec.w;
	specular = pow(specular, specularPower);
	OUT.col += specularColor * specular;

	return OUT;
}

