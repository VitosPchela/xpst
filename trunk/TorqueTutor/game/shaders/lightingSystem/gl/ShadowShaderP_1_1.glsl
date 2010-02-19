//-----------------------------------------------
// Synapse Gaming - Lighting System
// Copyright © Synapse Gaming 2003
// Written by John Kabus
//-----------------------------------------------

//-----------------------------------------------------------------------------
// Data 
//-----------------------------------------------------------------------------
uniform sampler2D diffuseMap0, diffuseMap1, diffuseMap2, diffuseMap3;
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
	vec4 s;
	s = texture2D(diffuseMap0, TEX0);
	s += texture2D(diffuseMap1, TEX1);
	s += texture2D(diffuseMap2, TEX2);
	s += texture2D(diffuseMap3, TEX3);
	
	vec4 shadow = color * s * 0.25;
	shadow *= COL;
	shadow = 1.0 - shadow;
	
	gl_FragColor = shadow;
}
