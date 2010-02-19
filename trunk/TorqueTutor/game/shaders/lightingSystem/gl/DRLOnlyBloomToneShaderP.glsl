//-----------------------------------------------
// Synapse Gaming - Lighting System
// Copyright © Synapse Gaming 2003
// Written by John Kabus
//-----------------------------------------------

//-----------------------------------------------------------------------------
// Data 
//-----------------------------------------------------------------------------
uniform sampler2D bloommap1, frame, tonemap;
uniform vec4 drlinfo;

varying vec2 TEX0;

//-----------------------------------------------------------------------------
// Main
//-----------------------------------------------------------------------------
void main()
{
	vec4 framecol = texture2D(frame, TEX0);
	gl_FragColor = framecol;
	
	framecol.w = max(max(framecol.x, framecol.y), framecol.z);
	
	vec2 uv;
	uv.x = framecol.w * drlinfo.w * 0.5;
	uv.y = 0.5;
	
	gl_FragColor.xyz += texture2D(bloommap1, TEX0).xyz * 2.0;
	gl_FragColor.xyz *= texture2D(tonemap, uv).xyz * 2.0;
}
