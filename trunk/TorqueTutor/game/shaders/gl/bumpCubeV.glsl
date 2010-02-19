//-----------------------------------------------------------------------------
// Constants
//-----------------------------------------------------------------------------
uniform mat4 modelview;
uniform mat3 cubeTrans;
uniform vec3 cubeEyePos;

varying vec4 tangentToCube0, tangentToCube1, tangentToCube2;
varying vec2 TEX0;

//-----------------------------------------------------------------------------
// Main
//-----------------------------------------------------------------------------
void main()
{
   gl_Position = modelview * gl_Vertex;
   TEX0 = gl_MultiTexCoord0.st;

   
	mat3 objToTangentSpace;
	objToTangentSpace[0] = gl_MultiTexCoord2.xyz;
	objToTangentSpace[1] = gl_MultiTexCoord3.xyz;
	objToTangentSpace[2] = gl_Normal;
   
   
   tangentToCube0.xyz = objToTangentSpace * cubeTrans[0];
   tangentToCube1.xyz = objToTangentSpace * cubeTrans[1];
   tangentToCube2.xyz = objToTangentSpace * cubeTrans[2];
   
   vec3 pos = cubeTrans * gl_Vertex.xyz;
   vec3 eye = cubeEyePos - pos;
   eye = normalize( eye );

   tangentToCube0.w = eye.x;
   tangentToCube1.w = eye.y;
   tangentToCube2.w = eye.z;
}

