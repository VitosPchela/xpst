//-----------------------------------------------------------------------------
// Data
//-----------------------------------------------------------------------------
uniform mat4 modelview;
uniform mat3 cubeTrans;
uniform vec3 cubeEyePos, eyePos;

varying vec3 cubeNormal, outCubeEyePos, pos;

//-----------------------------------------------------------------------------
// Main                                                                        
//-----------------------------------------------------------------------------
void main()
{
   gl_Position = modelview * gl_Vertex;
   vec3 normal = normalize( gl_Vertex.xyz );
   cubeNormal = normalize( cubeTrans * normal );
   outCubeEyePos = cubeEyePos / 100.0;
   pos = cubeTrans * gl_Vertex.xyz / 100.0;

/*   
   gl_Position = modelview * gl_Vertex;
   vec3 cubeNormal = normalize( cubeTrans * gl_Normal );
   vec3 cubeVertPos = cubeTrans * gl_Vertex.xyz;
   vec3 eyeToVert = cubeVertPos - cubeEyePos;
   reflectVec = reflect(eyeToVert, cubeNormal);
*/
}
