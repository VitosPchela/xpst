//-----------------------------------------------------------------------------
// Data
//-----------------------------------------------------------------------------
uniform mat4 modelview;
uniform vec3 eyePos, fogData;

varying vec2 texCoord, lmCoord, fogCoord;

//-----------------------------------------------------------------------------
// Main                                                                        
//-----------------------------------------------------------------------------
void main()
{
   gl_Position = modelview * gl_Vertex;
   texCoord = gl_MultiTexCoord0.st;
   lmCoord = gl_MultiTexCoord1.st;

   fogCoord.x = 1.0 - ( distance( gl_Vertex.xyz, eyePos ) / fogData.z );
   fogCoord.y = (gl_Vertex.z - fogData.x) * fogData.y;
}
