//-----------------------------------------------
// Synapse Gaming - Lighting System
// Copyright © Synapse Gaming 2003
// Written by John Kabus
//-----------------------------------------------

//-----------------------------------------------------------------------------
// Data
//-----------------------------------------------------------------------------
uniform mat4 lightspace;
uniform vec4 projectioninfo;

varying vec4 TEX0;

//-----------------------------------------------------------------------------
// Main
//-----------------------------------------------------------------------------
void main()
{
	gl_Position = lightspace * gl_Vertex;
	TEX0 = vec4(0.0);
	
	float depth = gl_Position.y;
	gl_Position.y = gl_Position.z;
	TEX0.z = ((depth + projectioninfo.y) / projectioninfo.x);

	gl_Position.z = TEX0.z;
}

