uniform mat4 modelview, objTrans, lightingMatrix;
uniform vec4 lightPos;
uniform vec3 eyePos, fogData;

varying vec2 texCoord, fogCoord;
varying vec3 dlightCoord;

void main()
{
   texCoord = gl_MultiTexCoord0.st;
   //dlightCoord = ((vec3(objTrans * gl_Vertex) - vec3(objTrans * lightPos)) * lightPos.w);
   dlightCoord = (gl_Vertex.xyz - lightPos.xyz) * lightPos.w;
   dlightCoord = vec3(lightingMatrix * vec4(dlightCoord, 1.0)) + 0.5;
   gl_Position = modelview * gl_Vertex;
   fogCoord.x = 1.0 - (distance(gl_Vertex.xyz, eyePos)/fogData.z);
   fogCoord.y = (gl_Vertex.z - fogData.x) * fogData.y;
   texCoord.y = 1.0 - texCoord.y;
}
   