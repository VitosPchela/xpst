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
	float2 TEX0 : TEXCOORD0;
	float2 TEX1 : TEXCOORD1;
	float2 TEX2 : TEXCOORD2;
	float2 TEX3 : TEXCOORD3;
	// keep this float3, DX9 complains about anything less...
	float3 COL  : COLOR0;
};

struct Fragout
{
	float4 col : COLOR0;
};

//-----------------------------------------------------------------------------
// Main
//-----------------------------------------------------------------------------
Fragout main(v2f IN,
			 uniform sampler2D diffuseMap: register(S0),
			 uniform float4    color      : register(C3))
{
	Fragout OUT;
	
	float1 s;
	s.x = tex2D(diffuseMap, IN.TEX0).y;
	s.x += tex2D(diffuseMap, IN.TEX1).y;
	s.x += tex2D(diffuseMap, IN.TEX2).y;
	s.x += tex2D(diffuseMap, IN.TEX3).y;
	
	float4 shadow = color * s.x * 0.25;
	shadow *= IN.COL.x;
	shadow = 1.0 - shadow;
	
	OUT.col = shadow;
	OUT.col.w = 1.0 - shadow.x;
	
	return OUT;
}
