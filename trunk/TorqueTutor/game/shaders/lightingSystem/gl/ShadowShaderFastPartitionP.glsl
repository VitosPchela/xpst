//-----------------------------------------------
// Synapse Gaming - Lighting System
// Copyright © Synapse Gaming 2003
// Written by John Kabus
//-----------------------------------------------

//-----------------------------------------------------------------------------
// Data 
//-----------------------------------------------------------------------------
uniform sampler2D diffuseMap;
uniform vec4 color;

varying vec2 TEX0;
varying vec2 TEX1;
varying vec2 TEX2;
varying vec2 TEX3;
varying float COL;

//-----------------------------------------------------------------------------
// Main
//-----------------------------------------------------------------------------
void main()
{
	float s;
	s = texture2D(diffuseMap, TEX0).y;
	s += texture2D(diffuseMap, TEX1).y;
	s += texture2D(diffuseMap, TEX2).y;
	s += texture2D(diffuseMap, TEX3).y;
	
	vec4 shadow = color * s * 0.25;
	shadow *= COL;
	shadow = 1.0 - shadow;
	
	gl_FragColor = shadow;
	gl_FragColor.w = 1.0 - shadow.x;
}
