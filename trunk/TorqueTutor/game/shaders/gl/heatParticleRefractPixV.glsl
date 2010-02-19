//-----------------------------------------------------------------------------
// Data
//-----------------------------------------------------------------------------
uniform mat4 modelview;
uniform vec3 eyePos, inLightVec;

varying vec4 hpos2, vertColor;
varying vec2 texCoord;

//-----------------------------------------------------------------------------
// Main                                                                        
//-----------------------------------------------------------------------------
void main()
{
   gl_Position = modelview * gl_Vertex;
   vertColor = gl_Color;
   texCoord = gl_MultiTexCoord0.st;
   hpos2 = gl_Position;
}
