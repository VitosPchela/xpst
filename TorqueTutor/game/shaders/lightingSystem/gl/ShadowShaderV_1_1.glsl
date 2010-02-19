//-----------------------------------------------
// Synapse Gaming - Lighting System
// Copyright � Synapse Gaming 2003
// Written by John Kabus
//-----------------------------------------------

//-----------------------------------------------------------------------------
// Data
//-----------------------------------------------------------------------------
uniform mat4 toscreen, tolightspace;
uniform vec4 projectioninfo, bias, stride;

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
	vec4 pos = tolightspace * gl_Vertex;
	
	float depth = pos.y;
	TEX0.x = (pos.x * projectioninfo.z * 0.5) + 0.5;
	TEX0.y = (-pos.z * projectioninfo.z * 0.5) + 0.5;
	
	TEX1 = TEX0;
	TEX1.x += stride.x;
	TEX2 = TEX0;
	TEX2.y += stride.y;
	TEX3 = TEX0 + stride.xy;
   
   TEX0.y = -TEX0.y;
   TEX1.y = -TEX1.y;
   TEX2.y = -TEX2.y;
   TEX3.y = -TEX3.y;
	
	gl_Position = toscreen * gl_Vertex;
	gl_Position.z -= bias.y;

	COL = 1.0 - (((depth + projectioninfo.y) / projectioninfo.x) - 0.001);
}

