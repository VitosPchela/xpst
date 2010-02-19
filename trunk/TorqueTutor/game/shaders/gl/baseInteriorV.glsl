//-----------------------------------------------------------------------------
// Data
//-----------------------------------------------------------------------------
uniform mat4 modelview;
uniform vec3 lightDir;

varying vec4 TEX0, TEX1, TEX2;
varying vec3 outLightDir;

//-----------------------------------------------------------------------------
// Main
//-----------------------------------------------------------------------------
void main()
{
   // compute the 3x3 tranform from tangent space to object space
	mat3 objToTangentSpace;
	objToTangentSpace[0] = gl_MultiTexCoord2.xyz;
	objToTangentSpace[1] = gl_MultiTexCoord3.xyz;
	objToTangentSpace[2] = gl_Normal;

   // rotate light dir into tangent space
   outLightDir = objToTangentSpace * -lightDir;
   
   // ps 1.1 clamps from 0.0 to 1.0 in tex coords
   outLightDir = (outLightDir + 1.0) * 0.5;
   
   // send tex coords to pix shader
   gl_Position = modelview * gl_Vertex;
   TEX0 = gl_MultiTexCoord0;
   TEX1 = gl_MultiTexCoord1;
   TEX2 = gl_MultiTexCoord0;
}

