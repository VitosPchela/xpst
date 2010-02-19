//-----------------------------------------------
// Synapse Gaming - Lighting System
// Copyright © Synapse Gaming 2003
// Written by John Kabus
//-----------------------------------------------

//-----------------------------------------------------------------------------
// Data 
//-----------------------------------------------------------------------------
uniform sampler2D diffuseMap;

varying vec4 TEX0;
varying vec4 TEX1;
varying vec4 TEX2;
varying vec4 TEX3;
varying vec4 TEX4;
varying vec4 TEX5;
varying vec4 TEX6;
varying vec4 TEX7;

//-----------------------------------------------------------------------------
// Main
//-----------------------------------------------------------------------------
void main()
{
	vec4 multiplier = vec4(0.0625);
	
	gl_FragColor = texture2D(diffuseMap, TEX0.xy);
	gl_FragColor += texture2D(diffuseMap, TEX0.zw);
	gl_FragColor += texture2D(diffuseMap, TEX1.xy);
	gl_FragColor += texture2D(diffuseMap, TEX1.zw);
	
	gl_FragColor += texture2D(diffuseMap, TEX2.xy);
	gl_FragColor += texture2D(diffuseMap, TEX2.zw);
	gl_FragColor += texture2D(diffuseMap, TEX3.xy);
	gl_FragColor += texture2D(diffuseMap, TEX3.zw);
	
	gl_FragColor += texture2D(diffuseMap, TEX4.xy);
	gl_FragColor += texture2D(diffuseMap, TEX4.zw);
	gl_FragColor += texture2D(diffuseMap, TEX5.xy);
	gl_FragColor += texture2D(diffuseMap, TEX5.zw);
	
	gl_FragColor += texture2D(diffuseMap, TEX6.xy);
	gl_FragColor += texture2D(diffuseMap, TEX6.zw);
	gl_FragColor += texture2D(diffuseMap, TEX7.xy);
	gl_FragColor += texture2D(diffuseMap, TEX7.zw);
	
	gl_FragColor *= multiplier;
}
