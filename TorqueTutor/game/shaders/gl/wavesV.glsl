uniform mat4 modelview;
uniform mat3 cubeTrans;
uniform vec3 cubeEyePos;
uniform vec3 eyePos;
uniform vec3 inLightVec;

varying vec2 TEX0;
varying vec4 outLightVec;
varying vec3 outPos;
varying vec3 outEyePos;

void main()
{
   gl_Position = modelview * gl_Vertex;
   TEX0 = gl_MultiTexCoord0.xy;

   vec3 normal = normalize( gl_Normal );
   
   mat3 objToTangentSpace;
   objToTangentSpace[0] = vec3(gl_MultiTexCoord2.x, gl_MultiTexCoord3.x, normal.x);
   objToTangentSpace[1] = vec3(gl_MultiTexCoord2.y, gl_MultiTexCoord3.y, normal.y);
   objToTangentSpace[2] = vec3(gl_MultiTexCoord2.z, gl_MultiTexCoord3.z, normal.z);
   
   vec3 pos = cubeTrans * gl_Vertex.xyz;
   vec3 eye = cubeEyePos - pos;
   eye = normalize(eye);
   
   outLightVec.xyz = -inLightVec;
   outLightVec.xyz = objToTangentSpace * outLightVec.xyz;
   outPos = objToTangentSpace * (gl_Vertex.xyz / 100.0);
   outEyePos.xyz = objToTangentSpace * (eyePos.xyz / 100.0);
   outLightVec.w = step(0.0, dot(-inLightVec, normal));
}

   