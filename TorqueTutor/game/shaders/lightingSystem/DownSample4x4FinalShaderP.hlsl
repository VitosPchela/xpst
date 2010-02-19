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
	float4 TEX1 : TEXCOORD1;
	float4 TEX2 : TEXCOORD2;
	float4 TEX3 : TEXCOORD3;
	float4 TEX4 : TEXCOORD4;
	float4 TEX5 : TEXCOORD5;
	float4 TEX6 : TEXCOORD6;
	float4 TEX7 : TEXCOORD7;
};

struct Fragout
{
	float4 col : COLOR0;
};

//-----------------------------------------------------------------------------
// Main
//-----------------------------------------------------------------------------
Fragout main(v2f IN,
			 uniform sampler2D diffuseMap : register(S0),
			 uniform float4 samplestride  : register(C0),
			 uniform float4 blend         : register(C3))
{
	Fragout OUT;
	
	float4 multiplier = 0.0625;
	
	OUT.col = tex2D(diffuseMap, IN.TEX0.xy);
	OUT.col += tex2D(diffuseMap, IN.TEX0.zw);
	OUT.col += tex2D(diffuseMap, IN.TEX1.xy);
	OUT.col += tex2D(diffuseMap, IN.TEX1.zw);
	
	OUT.col += tex2D(diffuseMap, IN.TEX2.xy);
	OUT.col += tex2D(diffuseMap, IN.TEX2.zw);
	OUT.col += tex2D(diffuseMap, IN.TEX3.xy);
	OUT.col += tex2D(diffuseMap, IN.TEX3.zw);
	
	OUT.col += tex2D(diffuseMap, IN.TEX4.xy);
	OUT.col += tex2D(diffuseMap, IN.TEX4.zw);
	OUT.col += tex2D(diffuseMap, IN.TEX5.xy);
	OUT.col += tex2D(diffuseMap, IN.TEX5.zw);
	
	OUT.col += tex2D(diffuseMap, IN.TEX6.xy);
	OUT.col += tex2D(diffuseMap, IN.TEX6.zw);
	OUT.col += tex2D(diffuseMap, IN.TEX7.xy);
	OUT.col += tex2D(diffuseMap, IN.TEX7.zw);
	
	OUT.col *= multiplier;
	
	OUT.col.x = OUT.col.w;
	OUT.col.w = blend.x;
	
	return OUT;
}
