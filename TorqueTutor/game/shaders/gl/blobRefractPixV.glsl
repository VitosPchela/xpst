uniform mat4 modelview;
uniform vec3 eyePos;
uniform vec3 inLightVec;

varying vec2 texCoord;
varying vec3 outEyePos, pos;
varying vec4 hpos2;
varying vec3 lightVec;

void main()
{
   gl_Position = modelview * gl_Vertex;
   texCoord = gl_MultiTexCoord0.st * 6.0;
   hpos2 = gl_Position;
   
   mat3 objToTangentSpace;
   objToTangentSpace[0] = vec3(gl_MultiTexCoord2.x, gl_MultiTexCoord3.x, gl_Normal.x);
   objToTangentSpace[1] = vec3(gl_MultiTexCoord2.y, gl_MultiTexCoord3.y, gl_Normal.y);
   objToTangentSpace[2] = vec3(gl_MultiTexCoord2.z, gl_MultiTexCoord3.z, gl_Normal.z);
   
   pos = objToTangentSpace * (gl_Vertex.xyz / 100.0);
   outEyePos = objToTangentSpace * (eyePos / 100.0);
   lightVec = objToTangentSpace * -inLightVec;
}