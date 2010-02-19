//-----------------------------------------------------------------------------
// Data
//-----------------------------------------------------------------------------
uniform mat4 modelview;
uniform mat3 cubeTrans;
uniform vec3 cubeEyePos, eyePos;

varying vec3 reflectVec;

//-----------------------------------------------------------------------------
// Main                                                                        
//-----------------------------------------------------------------------------
void main()
{
   gl_Position = modelview * gl_Vertex;
   vec3 cubeNormal = normalize( cubeTrans * gl_Normal );
   vec3 cubeVertPos = cubeTrans * gl_Vertex.xyz;
   vec3 eyeToVert = cubeVertPos - cubeEyePos;
   reflectVec = reflect(eyeToVert, cubeNormal);
}
