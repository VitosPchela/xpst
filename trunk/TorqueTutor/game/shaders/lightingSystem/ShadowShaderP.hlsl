//-----------------------------------------------
// Synapse Gaming - Lighting System
// Copyright © Synapse Gaming 2003
// Written by John Kabus
//-----------------------------------------------

//-----------------------------------------------------------------------------
// Data 
//-----------------------------------------------------------------------------
struct v2f
{
	float4 HPOS : POSITION;
	float4 TEX0 : TEXCOORD0;
	float3 TEX1 : TEXCOORD1;
};

struct Fragout
{
	float4 col : COLOR0;
};

float4 sgSampleShadow(sampler2D shadowmap, float2 coordx, float2 coordy, float2 coordz, float2 coordw, float4 world)
{
	float4 shadow;
	shadow = tex2D(shadowmap, coordx).x;
	shadow.y = tex2D(shadowmap, coordy).x;
	shadow.z = tex2D(shadowmap, coordz).x;
	shadow.w = tex2D(shadowmap, coordw).x;
	return saturate((world - shadow) * 10000);
};

//-----------------------------------------------------------------------------
// Main
//-----------------------------------------------------------------------------
Fragout main(v2f IN,
			 uniform sampler2D diffuseMap : register(S0),
			 uniform float4    stride     : register(C0),
			 uniform float4    color      : register(C3))
{
	Fragout OUT;
	
	float4 coord = IN.TEX0;
	float1 str = stride.w;
	
	
	// clamp to lexel...
	float4 intuv = (coord * stride.z);
	intuv = floor(intuv);
	intuv *= str.x;
	
	
	// sample...
	float4 uv01;
	uv01.xy = intuv.xy;
	uv01.zw = intuv.xy;
	uv01.z += str.x;
	
	float4 uv23 = uv01;
	uv23.yw += str.x;
	
	float4 sample0 = sgSampleShadow(diffuseMap, uv01.xy, uv01.zw, uv23.xy, uv23.zw, IN.TEX0.z);
	
	float1 total = (sample0.x + sample0.y + sample0.z + sample0.w);
	

	// final setup...
	float4 shadow = color * total.x * 0.25;
	shadow *= IN.TEX0.w;
	OUT.col = 1.0 - shadow;
	
	OUT.col = saturate(OUT.col);
	OUT.col.w = shadow.x;
	
	return OUT;
}
