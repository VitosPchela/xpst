//-----------------------------------------------------------------------------
// Data
//-----------------------------------------------------------------------------
uniform mat4 modelview, objTrans;
uniform mat3 cubeTrans;
uniform vec3 cubeEyePos, eyePos, fogData;

varying vec2 TEX0, fogCoord;
varying vec3 reflectVec;

//-----------------------------------------------------------------------------
// Main
//-----------------------------------------------------------------------------
void main()
{
   gl_Position = modelview * gl_Vertex;
   
   vec3 eyeToVert = normalize( eyePos - gl_Vertex.xyz );
   TEX0.x = dot( eyeToVert, vec3( 0.0, 0.0, 1.0 ) );
   TEX0.y = 0.0;
   
   vec3 cubeVertPos = cubeTrans * gl_Vertex.xyz;
   vec3 cubeNormal = normalize( cubeTrans * vec3( 0.0, 0.0, 1.0 ) );
   vec3 cubeEyeToVert = cubeVertPos - cubeEyePos;
   reflectVec = reflect(cubeEyeToVert, cubeNormal);

   vec3 transPos = (objTrans * gl_Vertex).xyz;
   fogCoord.x = 1.0 - ( distance( gl_Vertex.xyz, eyePos ) / fogData.z );
   fogCoord.y = (transPos.z - fogData.x) * fogData.y;
}


