//-----------------------------------------------
// Synapse Gaming - Lighting System
// Copyright © Synapse Gaming 2003
// Written by John Kabus
//-----------------------------------------------

//-----------------------------------------------------------------------------
// Data
//-----------------------------------------------------------------------------
uniform vec4 stride;

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
	gl_Position = gl_Vertex;
	
	
	vec4 tex = gl_MultiTexCoord0;
   
   tex.y = 1.0 - tex.y;
   
	vec4 start = tex - (stride * 1.5);
	
	tex.y = start.y;
	TEX0.xy = tex.xy;
	

	tex.x = start.x;
	tex.y += stride.y;
	
	tex.x += stride.x;
	TEX0.zw = tex.xy;
	tex.x += stride.x;
	TEX1.xy = tex.xy;
	tex.x += stride.x;
	TEX1.zw = tex.xy;

	
	tex.x = start.x;
	tex.y += stride.y;
	
	TEX2.xy = tex.xy;
	tex.x += stride.x;
	TEX2.zw = tex.xy;
	tex.x += stride.x;
	TEX3.xy = tex.xy;
	tex.x += stride.x;
	TEX3.zw = tex.xy;
	tex.x += stride.x;
	TEX4.xy = tex.xy;
	
	
	tex.x = start.x;
	tex.y += stride.y;
	
	tex.x += stride.x;
	TEX4.zw = tex.xy;
	tex.x += stride.x;
	TEX5.xy = tex.xy;
	tex.x += stride.x;
	TEX5.zw = tex.xy;

	
	tex.x = gl_MultiTexCoord0.x;
	tex.y += stride.y;
	
	TEX6 = tex;
}

