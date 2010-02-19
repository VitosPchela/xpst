uniform mat4 modelview, texMat;
uniform mat3 cubeTrans;
uniform vec3 cubeEyePos, eyePos;

varying vec2 outTexCoord;
varying vec3 outLightVec, reflectVec, pos, outEyePos;
varying vec4 shading;

void main()
{
   vec3 inLightVec = normalize(vec3(0.0, -0.7, 0.3));
   
   gl_Position = modelview * gl_Vertex;
   
   vec4 texCoordExtend = vec4(gl_MultiTexCoord0.st, 0.0, 1.0);
   outTexCoord = vec2(texMat * texCoordExtend);
   
   vec3 normal = normalize( gl_Normal );
   
   mat3 objToTangentSpace;
   objToTangentSpace[0] = vec3(gl_MultiTexCoord2.x, gl_MultiTexCoord3.x, normal.x);
   objToTangentSpace[1] = vec3(gl_MultiTexCoord2.y, gl_MultiTexCoord3.y, normal.y);
   objToTangentSpace[2] = vec3(gl_MultiTexCoord2.z, gl_MultiTexCoord3.z, normal.z);
   
   outLightVec = -inLightVec;
   outLightVec = objToTangentSpace * outLightVec;
   
   pos = objToTangentSpace * (gl_Vertex.xyz / 100.0);
   outEyePos = objToTangentSpace * (eyePos.xyz / 100.0);
   
   vec3 cubeNormal = normalize(cubeTrans * gl_Normal);
   vec3 cubeVertPos = cubeTrans * gl_Vertex.xyz;
   vec3 eyeToVert = cubeVertPos - cubeEyePos;
   reflectVec = reflect(eyeToVert, cubeNormal);
   
   vec3 eyeVec = normalize(eyePos - gl_Vertex.xyz);
   float falloff = 1.0 - clamp(dot(eyeVec, normal), 0.0, 1.0);
   vec4 falloffColor = falloff * vec4(0.5, 0.5, 0.6, 1.0);
   
   shading = falloffColor;
}