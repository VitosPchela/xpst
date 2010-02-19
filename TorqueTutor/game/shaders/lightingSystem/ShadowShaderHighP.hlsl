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

float4 sgSampleShadow(sampler2D shadowmap, float2 coordx, float2 coordy, float2 coordz/*, float4 coordw*/, float4 world)
{
	float4 shadow;
	shadow = tex2D(shadowmap, coordx).x;
	shadow.y = tex2D(shadowmap, coordy).x;
	shadow.z = tex2D(shadowmap, coordz).x;
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
	float2 uv0 = intuv.xy;
	
	float4 uv12;
	uv12.xy = intuv;
	uv12.x += str.x;
	
	uv12.zw = uv12.xy;
	uv12.z += str.x;
	
	float4 sample0 = sgSampleShadow(diffuseMap, uv0, uv12.xy, uv12.zw, IN.TEX0.z);
	
	uv0.y += str.x;
	uv12.yw += str.x;
	
	float4 sample1 = sgSampleShadow(diffuseMap, uv0, uv12.xy, uv12.zw, IN.TEX0.z);
	
	uv0.y += str.x;
	uv12.yw += str.x;
	
	float4 sample2 = sgSampleShadow(diffuseMap, uv0, uv12.xy, uv12.zw, IN.TEX0.z);
	
	float4 rows01 = sample0 + sample1;
	float4 rows12 = sample1 + sample2;
	
	float2 col0;
	float2 col1;
	
	col0.x = rows01.x + rows01.y;
	col0.y = rows12.x + rows12.y;
	
	col1.x = rows01.y + rows01.z;
	col1.y = rows12.y + rows12.z;
	
	float2 lerpuv;
	lerpuv = (coord.xy - intuv.xy) * stride.z;
	
	float2 val;
	val.xy = lerp(col0.xy, col1.xy, lerpuv.x);
	float1 total = lerp(val.x, val.y, lerpuv.y);
	

	// final setup...
	float4 shadow = color * total.x * 0.25;
	shadow *= IN.TEX0.w;
	OUT.col = 1.0 - shadow;
	
	OUT.col = saturate(OUT.col);
	OUT.col.w = shadow.x;
	
	return OUT;
}
