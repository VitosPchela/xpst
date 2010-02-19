//-----------------------------------------------------------------------------
// Data
//-----------------------------------------------------------------------------
uniform mat4 modelview;
uniform mat3 cubeTrans;
uniform vec3 cubeEyePos;

varying vec4 TEX0, TEX1;
varying vec3 reflectVec, fresnel;

//-----------------------------------------------------------------------------
// fresnel approximation
//-----------------------------------------------------------------------------
float fast_fresnel(vec3 I, vec3 N, vec3 fresnelValues)
{
    float power = fresnelValues.x;
    float scale = fresnelValues.y;
    float bias = fresnelValues.z;

    return bias + pow(1.0 - dot(I, N), power) * scale;
}

//-----------------------------------------------------------------------------
// Main
//-----------------------------------------------------------------------------
void main()
{
   gl_Position = modelview * gl_Vertex;
   TEX0 = gl_MultiTexCoord0;
   TEX1 = gl_MultiTexCoord1;

   // rotate position and normal by modelview orientation
   vec3 pos = cubeTrans * gl_Vertex.xyz;
   vec3 newNorm = cubeTrans * gl_Normal;
   newNorm = normalize( newNorm );

   // calc reflection vector and pass to pixel shader
   vec3 eyeToVert = pos - cubeEyePos;
   eyeToVert = normalize(eyeToVert);

   vec3 vertToEye = cubeEyePos - gl_Vertex.xyz;
   vertToEye = normalize( vertToEye );
   
//   fresnel = fast_fresnel( vertToEye, gl_Normal, vec3( 1.0, 1.0, 0.5) ).xxx;
   float surfAng = max( 1.0 - dot( vertToEye, gl_Normal ), 0.0 );
   
   surfAng = surfAng * 0.25 + 0.5;
   fresnel = vec3(surfAng);
   
   reflectVec = reflect(eyeToVert, newNorm.xyz);
}

