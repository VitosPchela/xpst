uniform mat4 modelview;
uniform mat4 objTrans;
uniform vec3 eyePos, fogData;
uniform float morphT;

varying vec2 fogCoord;

void main()
{
   vec4 realPos = gl_Vertex + (gl_MultiTexCoord2 * morphT);
   realPos.w = 1.0;
   
   gl_Position = modelview * realPos;
   
   float eyeDist = distance(gl_Vertex.xyz, eyePos);
   vec3 transPos = vec3(objTrans * gl_Vertex);
   
   fogCoord.x = 1.0 - (eyeDist / fogData.z);
   fogCoord.y = (transPos.z - fogData.x) * fogData.y;
}