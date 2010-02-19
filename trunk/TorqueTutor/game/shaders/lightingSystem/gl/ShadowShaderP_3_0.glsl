//-----------------------------------------------
// Synapse Gaming - Lighting System
// Copyright © Synapse Gaming 2003
// Written by John Kabus
//-----------------------------------------------

//-----------------------------------------------------------------------------
// Data 
//-----------------------------------------------------------------------------
uniform sampler2D diffuseMap;
uniform vec4 stride, color;

varying vec4 TEX0;
varying vec3 TEX1;

vec4 sgSampleShadow(sampler2D shadowmap, vec4 coord, vec4 world)
{
	vec4 shadow = texture2D(shadowmap, coord);
	return clamp((world - shadow.x) * 10000, 0.0, 1.0);
};

//-----------------------------------------------------------------------------
// Main
//-----------------------------------------------------------------------------
void main()
{
	vec4 coord = TEX0;
	float str = stride.w;
	
	
	// clamp to lexel...
	vec4 intuv = (coord * stride.z);
	intuv = floor(intuv);
	intuv *= str;
	
	
	// sample...
	vec4 uv = intuv;
	vec4 s00 = sgSampleShadow(diffuseMap, uv, TEX0.z);
	
	uv.x += str;
	vec4 s10 = sgSampleShadow(diffuseMap, uv, TEX0.z);
	
	uv.x += str;
	vec4 s20 = sgSampleShadow(diffuseMap, uv, TEX0.z);
	
	uv = intuv;
	uv.y += str;
	vec4 s01 = sgSampleShadow(diffuseMap, uv, TEX0.z);
	
	uv.x += str;
	vec4 s11 = sgSampleShadow(diffuseMap, uv, TEX0.z);
	
	uv.x += str;
	vec4 s21 = sgSampleShadow(diffuseMap, uv, TEX0.z);
	
	uv.x = intuv.x;
	uv.y += str;
	vec4 s02 = sgSampleShadow(diffuseMap, uv, TEX0.z);
	
	uv.x += str;
	vec4 s12 = sgSampleShadow(diffuseMap, uv, TEX0.z);
	
	uv.x += str;
	vec4 s22 = sgSampleShadow(diffuseMap, uv, TEX0.z);
	

	// filter...	
	float1 s10_11 = s10.x + s11.x;
	float1 s12_11 = s12.x + s11.x;
	
	vec4 ss;
	ss.x = s00.x + s01.x + s10_11.x;
	ss.y = s20.x + s21.x + s10_11.x;
	ss.z = s01.x + s02.x + s12_11.x;
	ss.w = s21.x + s22.x + s12_11.x;

	vec2 lerpuv;
	lerpuv.x = (coord.x - intuv.x) * stride.z;
	lerpuv.y = (coord.y - intuv.y) * stride.z;
	
	float3 val;
	val.x = lerp(ss.x, ss.y, lerpuv.x);
	val.y = lerp(ss.z, ss.w, lerpuv.x);
	val.z = lerp(val.x, val.y, lerpuv.y);
	

	// final setup...
	vec4 shadow = color * val.z * 0.5 * 0.25;
	shadow *= TEX0.w;
	gl_FragColor = 1.0 - shadow;
	
	gl_FragColor = clamp(gl_FragColor, 0.0, 1.0);
	gl_FragColor.w = 1.0 - gl_FragColor.x;
}
