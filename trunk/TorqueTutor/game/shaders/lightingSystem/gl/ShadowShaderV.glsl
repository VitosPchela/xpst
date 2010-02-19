//-----------------------------------------------
// Synapse Gaming - Lighting System
// Copyright Synapse Gaming 2003
// Written by John Kabus
//-----------------------------------------------

//-----------------------------------------------------------------------------
// Data
//-----------------------------------------------------------------------------
uniform mat4 toscreen, tolightspace;
uniform vec4 projectioninfo, bias, lightvector;

varying vec4 TEX0;

//-----------------------------------------------------------------------------
// Main
//-----------------------------------------------------------------------------
void main()
{
	vec4 pos = tolightspace * gl_Vertex;
	
	float depth = pos.y;
	TEX0.x = (pos.x * projectioninfo.z * 0.5) + 0.5;
	TEX0.y = (-pos.z * projectioninfo.z * 0.5) + 0.5;
	TEX0.z = ((depth + projectioninfo.y) / projectioninfo.x) - bias.x;
	TEX0.w = 1.0 - (((depth + projectioninfo.y) / projectioninfo.x) - 0.001);
	
	vec3 TEX1 = vec3(tolightspace * vec4(gl_Normal.xyz, 0.0));
	TEX1.x = clamp(dot(lightvector, vec4(TEX1.xyz, 1.0)) * 2.0, 0.0, 1.0);
	
	TEX0.w *= TEX1.x;
	
	gl_Position = toscreen * gl_Vertex;
	gl_Position.z -= bias.y;
}

