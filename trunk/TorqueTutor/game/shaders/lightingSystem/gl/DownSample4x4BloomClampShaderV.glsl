//-----------------------------------------------
// Synapse Gaming - Lighting System
// Copyright © Synapse Gaming 2003
// Written by John Kabus
//-----------------------------------------------

//-----------------------------------------------------------------------------
// Data
//-----------------------------------------------------------------------------
uniform vec4 samplestride;

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
	gl_Position = gl_Vertex;

	
	vec4 uv;
	uv.xy = gl_MultiTexCoord0.xy;
	uv.zw = gl_MultiTexCoord0.xy;
   
   uv.y = 1.0 - uv.y;
   uv.w = 1.0 - uv.w;
	
	vec4 stridey;
	stridey.x = 0.0;
	stridey.y = samplestride.y;
	stridey.z = 0.0;
	stridey.w = samplestride.y;
	
	TEX0 = uv;
	TEX0.z += samplestride.x;
	TEX1 = uv;
	TEX1.x += samplestride.x * 2.0;
	TEX1.z += samplestride.x * 3.0;
	
	uv += stridey;
	TEX2 = uv;
	TEX2.z += samplestride.x;
	TEX3 = uv;
	TEX3.x += samplestride.x * 2.0;
	TEX3.z += samplestride.x * 3.0;
	
	uv += stridey;
	TEX4 = uv;
	TEX4.z += samplestride.x;
	TEX5 = uv;
	TEX5.x += samplestride.x * 2.0;
	TEX5.z += samplestride.x * 3.0;
	
	uv += stridey;
	TEX6 = uv;
	TEX6.z += samplestride.x;
	TEX7 = uv;
	TEX7.x += samplestride.x * 2.0;
	TEX7.z += samplestride.x * 3.0;
}

