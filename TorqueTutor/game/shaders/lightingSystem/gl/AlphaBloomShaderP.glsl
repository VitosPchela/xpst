//-----------------------------------------------
// Synapse Gaming - Lighting System
// Copyright © Synapse Gaming 2003
// Written by John Kabus
//-----------------------------------------------

//-----------------------------------------------------------------------------
// Data 
//-----------------------------------------------------------------------------
uniform sampler2D diffuseMap;

varying vec2 TEX0;

//-----------------------------------------------------------------------------
// Main
//-----------------------------------------------------------------------------
void main()
{
	gl_FragColor = texture2D(diffuseMap, TEX0);
	gl_FragColor.w = (gl_FragColor.x + gl_FragColor.y + gl_FragColor.z) * 0.333;
	
	gl_FragColor.x = (gl_FragColor.x - 0.75) * 2.0;
	gl_FragColor.y = (gl_FragColor.y - 0.75) * 2.0;
	gl_FragColor.z = (gl_FragColor.z - 0.75) * 2.0;
}
