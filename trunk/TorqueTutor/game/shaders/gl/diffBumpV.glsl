//-----------------------------------------------------------------------------
// Data                                                                  
//-----------------------------------------------------------------------------
uniform mat4 modelview, texMat, objTrans;
uniform vec3 inLightVec, eyePos;

varying vec3 outLightVec;
varying vec2 outTexCoord, bumpCoord;

//-----------------------------------------------------------------------------
// Main                                                                        
//-----------------------------------------------------------------------------
void main()
{
   gl_Position = modelview * gl_Vertex;
   
   vec4 texCoordExtend = vec4( gl_MultiTexCoord0.st, 0.0, 1.0 );
   outTexCoord = (texMat * texCoordExtend).xy;
   bumpCoord = outTexCoord;

   mat3 objToTangentSpace;
   objToTangentSpace[0] = gl_MultiTexCoord2.xyz;
   objToTangentSpace[1] = gl_MultiTexCoord3.xyz;
   objToTangentSpace[2] = gl_Normal;

   outLightVec = -inLightVec;
   outLightVec = objToTangentSpace * outLightVec;
   outLightVec = outLightVec / 2.0 + 0.5;
}
