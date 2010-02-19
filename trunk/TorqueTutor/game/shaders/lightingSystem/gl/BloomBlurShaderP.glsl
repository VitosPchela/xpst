//-----------------------------------------------
// Synapse Gaming - Lighting System
// Copyright © Synapse Gaming 2003
// Written by John Kabus
//-----------------------------------------------

//-----------------------------------------------------------------------------
// Data 
//-----------------------------------------------------------------------------
uniform sampler2D diffuseMap;
uniform vec4 seedamount;

varying vec4 TEX0;
varying vec4 TEX1;
varying vec4 TEX2;
varying vec4 TEX3;
varying vec4 TEX4;
varying vec4 TEX5;
varying vec4 TEX6;

//-----------------------------------------------------------------------------
// Main
//-----------------------------------------------------------------------------
void main()
{
	gl_FragColor = texture2D(diffuseMap, TEX0.xy) * 0.04;

	gl_FragColor += texture2D(diffuseMap, TEX0.zw) * 0.07;
	gl_FragColor += texture2D(diffuseMap, TEX1.xy) * 0.1;
	gl_FragColor += texture2D(diffuseMap, TEX1.zw) * 0.07;
	
	gl_FragColor += texture2D(diffuseMap, TEX2.xy) * 0.04;
	gl_FragColor += texture2D(diffuseMap, TEX2.zw) * 0.1;
	gl_FragColor += texture2D(diffuseMap, TEX3.xy) * seedamount;
	gl_FragColor += texture2D(diffuseMap, TEX3.zw) * 0.1;
	gl_FragColor += texture2D(diffuseMap, TEX4.xy) * 0.04;
	
	gl_FragColor += texture2D(diffuseMap, TEX4.zw) * 0.07;
	gl_FragColor += texture2D(diffuseMap, TEX5.xy) * 0.1;
	gl_FragColor += texture2D(diffuseMap, TEX5.zw) * 0.07;
	
	gl_FragColor += texture2D(diffuseMap, TEX6.xy) * 0.04;
}
