//-----------------------------------------------------------------------------
// Data
//-----------------------------------------------------------------------------
uniform mat4 modelview;
uniform vec4 sourceTexScales;

varying vec2 texCoord0, texCoord1, texCoord2, lmTexCoord;

void main()
{
   gl_Position = modelview * gl_Vertex;
   texCoord0  = gl_MultiTexCoord0.st;
   texCoord1  = gl_MultiTexCoord2.st * sourceTexScales.x;
   texCoord2  = gl_MultiTexCoord2.st * sourceTexScales.y;
   lmTexCoord = gl_MultiTexCoord1.st;
}

