//-----------------------------------------------------------------------------
// Data
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
   TEX0 = gl_Vertex.xy * 0.14;

   mat3 objToTangentSpace  = mat3(1.0, 0.0, 0.0,
                                  0.0, 1.0, 0.0,
                                  0.0, 0.0, 1.0);
   
   tangentToCube0.xyz = objToTangentSpace * cubeTrans[0];
   tangentToCube1.xyz = objToTangentSpace * cubeTrans[1];
   tangentToCube2.xyz = objToTangentSpace * cubeTrans[2];
   
   vec3 pos = cubeTrans * gl_Vertex.xyz;
   vec3 eye = cubeEyePos.xyz - pos;
   eye = normalize( eye );
   
   tangentToCube0.w = eye.x;
   tangentToCube1.w = eye.y;
   tangentToCube2.w = eye.z;
}


