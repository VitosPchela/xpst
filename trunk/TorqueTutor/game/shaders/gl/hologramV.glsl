//-----------------------------------------------------------------------------
// Data
//-----------------------------------------------------------------------------
uniform mat4 modelview;
uniform mat3 cubeTrans;
uniform vec3 cubeEyePos, inLightVec, eyePos;

varying vec4 TEX0, outLightVec;
varying vec3 outPos, outEyePos;

//-----------------------------------------------------------------------------
// Main
//-----------------------------------------------------------------------------
void main()
{
   gl_Position = modelview * gl_Vertex;
   TEX0 = gl_MultiTexCoord0;

   mat3 objToTangentSpace;
	objToTangentSpace[0] = gl_MultiTexCoord2.xyz;
	objToTangentSpace[1] = gl_MultiTexCoord3.xyz;
	objToTangentSpace[2] = gl_MultiTexCoord4.xyz;
   
   vec3 pos = cubeTrans * gl_Vertex.xyz;
   vec3 eye = cubeEyePos - pos;
   eye = normalize( eye );

   outLightVec.xyz = -inLightVec;
   outLightVec.xyz = objToTangentSpace * outLightVec.xyz;
   outPos = objToTangentSpace * gl_Vertex.xyz;
   outEyePos.xyz = objToTangentSpace * eyePos;
   outLightVec.w = step( 0.0, dot( -inLightVec, gl_Normal ) );
}


