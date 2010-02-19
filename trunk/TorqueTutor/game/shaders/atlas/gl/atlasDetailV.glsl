uniform mat4 modelView;
uniform vec3 eyePos, detData;
uniform float morphT;

varying vec2 detCoord;
varying vec3 fade;

//-----------------------------------------------------------------------------
// Main
//-----------------------------------------------------------------------------
void main()
{
   // Apply morph calculations.
   vec4 realPos = gl_Vertex + (gl_MultiTexCoord2 * morphT);
   realPos.w = 1.0;

   // Transform and return.
   gl_Position = modelView * realPos;
 
   // Do the detail map calculations.
   float eyeDist = distance( gl_Vertex.xyz, eyePos );
   detCoord = gl_MultiTexCoord0.st * detData.z;                 // Amplify texcoords.
   fade.x     = clamp(1.0 - (eyeDist - detData.x) * detData.y, 0.0, 1.0); // And fade with distance.
   fade.yz    = vec2(0.0);
}
