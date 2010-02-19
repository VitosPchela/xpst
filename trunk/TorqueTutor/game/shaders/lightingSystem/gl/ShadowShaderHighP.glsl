//-----------------------------------------------
// Synapse Gaming - Lighting System
// Copyright Synapse Gaming 2003
// Written by John Kabus
//-----------------------------------------------

//-----------------------------------------------------------------------------
// Data 
//-----------------------------------------------------------------------------
uniform sampler2D diffuseMap;
uniform vec4 stride, color;

varying vec4 TEX0;

vec4 sgSampleShadow(sampler2D shadowmap, vec2 coordx, vec2 coordy, vec2 coordz, vec4 world)
{
	vec4 shadow;
	shadow = vec4(texture2D(shadowmap, coordx).x);
	shadow.y = texture2D(shadowmap, coordy).x;
	shadow.z = texture2D(shadowmap, coordz).x;
	return clamp((world - shadow) * 10000.0, 0.0, 1.0);
}

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
	vec2 uv0 = intuv.xy;
	
	vec4 uv12;
	uv12.xy = intuv.xy;
	uv12.x += str;
	
	uv12.zw = uv12.xy;
	uv12.z += str;
	
	vec4 sample0 = sgSampleShadow(diffuseMap, uv0, uv12.xy, uv12.zw, vec4(TEX0.z));
	
	uv0.y += str;
	uv12.yw += str;
	
	vec4 sample1 = sgSampleShadow(diffuseMap, uv0, uv12.xy, uv12.zw, vec4(TEX0.z));
	
	uv0.y += str;
	uv12.yw += str;
	
	vec4 sample2 = sgSampleShadow(diffuseMap, uv0, uv12.xy, uv12.zw, vec4(TEX0.z));
	
	vec4 rows01 = sample0 + sample1;
	vec4 rows12 = sample1 + sample2;
	
	vec2 col0;
	vec2 col1;
	
	col0.x = rows01.x + rows01.y;
	col0.y = rows12.x + rows12.y;
	
	col1.x = rows01.y + rows01.z;
	col1.y = rows12.y + rows12.z;
	
	vec2 lerpuv;
	lerpuv = (coord.xy - intuv.xy) * stride.z;
	
	vec2 val;
	val.xy = mix(col0.xy, col1.xy, lerpuv.x);
	float total = mix(val.x, val.y, lerpuv.y);
	

	// final setup...
	vec4 shadow = color * total * 0.25;
	shadow *= TEX0.w;
	gl_FragColor = 1.0 - shadow;
	
	gl_FragColor = clamp(gl_FragColor, 0.0, 1.0);
	gl_FragColor.w = shadow.x;
}
