//-----------------------------------------------
// Synapse Gaming - Lighting System
// Copyright © Synapse Gaming 2003
// Written by John Kabus
//-----------------------------------------------

//-----------------------------------------------------------------------------
// Data 
//-----------------------------------------------------------------------------
uniform sampler2D intensitymap, bloommap1, frame, tonemap;
uniform vec4 drlinfo;

varying vec2 TEX0;

//-----------------------------------------------------------------------------
// Main
//-----------------------------------------------------------------------------
void main()
{
	vec2 halfway = vec2(0.5);
	
	vec4 framecol = texture2D(frame, TEX0);
	gl_FragColor = framecol;
	
	float intensity = texture2D(intensitymap, halfway).x;
	float fullrange = 1.0 / (intensity + 0.0001);
	
	intensity = drlinfo.z * fullrange;
	intensity = sqrt(intensity);
	intensity = min(intensity, drlinfo.x);
	intensity = max(intensity, drlinfo.y);

	gl_FragColor.x *= intensity;
	gl_FragColor.y *= intensity;
	gl_FragColor.z *= intensity;
	
	framecol.w = max(max(framecol.x, framecol.y), framecol.z);
	
	vec2 uv;
	uv.x = framecol.w * fullrange * 0.5;
	uv.y = 0.5;
	
	gl_FragColor.xyz *= drlinfo.w;
	gl_FragColor.xyz += texture2D(bloommap1, TEX0).xyz * 2.0 * intensity;
	gl_FragColor.xyz *= texture2D(tonemap, uv).xyz * 2.0;
}
