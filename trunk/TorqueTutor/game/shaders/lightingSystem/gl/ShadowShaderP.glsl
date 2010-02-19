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

vec4 sgSampleShadow(sampler2D shadowmap, vec2 coordx, vec2 coordy, vec2 coordz, vec2 coordw, vec4 world)
{
	vec4 shadow;
	shadow.x = texture2D(shadowmap, coordx).x;
	shadow.y = texture2D(shadowmap, coordy).x;
	shadow.z = texture2D(shadowmap, coordz).x;
	shadow.w = texture2D(shadowmap, coordw).x;
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
	vec4 uv01;
	uv01.xy = intuv.xy;
	uv01.zw = intuv.xy;
	uv01.z += str;
	
	vec4 uv23 = uv01;
	uv23.yw += str;
	
	vec4 sample0 = sgSampleShadow(diffuseMap, uv01.xy, uv01.zw, uv23.xy, uv23.zw, vec4(TEX0.z));
	
	float total = (sample0.x + sample0.y + sample0.z + sample0.w);
	

	// final setup...
	vec4 shadow = color * total * 0.25;
	shadow *= TEX0.w;
	gl_FragColor = 1.0 - shadow;
	
	gl_FragColor = clamp(gl_FragColor, 0.0, 1.0);
	gl_FragColor.w = shadow.x;
}
