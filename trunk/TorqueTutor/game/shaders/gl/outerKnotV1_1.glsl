//-----------------------------------------------------------------------------
// Data
//-----------------------------------------------------------------------------
uniform mat4 modelview, texMat;
uniform mat3 cubeTrans;
uniform vec3 cubeEyePos, eyePos;

varying vec4 shading;
varying vec3 reflectVec, outLightVec;
varying vec2 outTexCoord, outBumpTexCoord;

//-----------------------------------------------------------------------------
// Main                                                                        
//-----------------------------------------------------------------------------
void main()
{
   vec3 inLightVec = normalize( vec3( 0.0, -0.7, 0.3 ) );

   gl_Position = modelview * gl_Vertex;
   
   vec4 texCoordExtend = vec4( gl_MultiTexCoord0.st, 0.0, 1.0 );
   outTexCoord = (texMat * texCoordExtend).xy;
   outBumpTexCoord = outTexCoord;

   mat3 objToTangentSpace = mat3( gl_MultiTexCoord2.xyz, gl_MultiTexCoord3.xyz, gl_Normal );
   
   outLightVec = -inLightVec;
   outLightVec = objToTangentSpace * outLightVec;
   outLightVec = outLightVec / 2.0 + 0.5;

   vec3 cubeNormal = normalize( cubeTrans * gl_Normal );
   vec3 cubeVertPos = cubeTrans * gl_Vertex.xyz;
   vec3 eyeToVert = cubeVertPos - cubeEyePos;
   reflectVec = reflect(eyeToVert, cubeNormal);

   vec3 eyeVec = normalize( eyePos - gl_Vertex.xyz );
   float falloff = 1.0 - clamp( dot( eyeVec, gl_Normal ), 0.0, 1.0 );
   vec4 falloffColor = falloff * vec4( 0.50, 0.50, 0.6, 1.0 );
   
   shading = falloffColor;
}
