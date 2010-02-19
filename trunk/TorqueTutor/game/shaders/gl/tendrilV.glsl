//-----------------------------------------------------------------------------
// Data
//-----------------------------------------------------------------------------
uniform mat4 modelview, texMat;
uniform vec3 eyePos, inLightVec;

//varying vec4 texCoord;
varying vec4 hpos2;
varying vec3 outEyePos, normal, pos, screenNorm, outLightVec;

//-----------------------------------------------------------------------------
// Main                                                                        
//-----------------------------------------------------------------------------
void main()
{
   gl_Position = modelview * gl_Vertex;

   //vec4 texCoordExtend = vec4( gl_MultiTexCoord0.st, 0.0, 1.0 );
   //texCoord = texMat * texCoordExtend * 0.5;
   
   hpos2       = gl_Position;
   screenNorm  = (modelview * vec4(gl_Normal, 1.0)).xyz;
   outEyePos   = eyePos / 100.0;
   normal      = gl_Normal;
   pos         = gl_Vertex.xyz / 100.0;
   outLightVec = -inLightVec;   
}
