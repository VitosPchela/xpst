#define fadeConstant 3.0
#define mapCount 2

uniform mat4 modelview;
uniform float morphT;
uniform vec4 mapInfo[mapCount];

varying vec2 texCoord[mapCount];
varying vec4 fade;

vec4 atlasVertex(in mat4 modelview, in float morph)
{
   // Apply morph calculations.
   vec4 realPos = gl_Vertex + (gl_MultiTexCoord2 * morph);
   realPos.w = 1.0;

   // Transform and return.
   return modelview * realPos;
}

vec2 atlasTexCoord(in float morph)
{
   return gl_MultiTexCoord0.st + (morph * gl_MultiTexCoord1.st);
}

void main()
{
   gl_Position = atlasVertex(modelview, morphT);
   
   vec2 tc = atlasTexCoord(morphT);
   
   for(int i = 0; i < mapCount; i++)
      texCoord[i] = tc * mapInfo[i].z;
      
   vec4 distAcc = vec4(0.0);
   for(int i = 0; i < mapCount; i++)
      distAcc[i] = distance(mapInfo[i].xy, texCoord[i].xy);
   
   for(int i = 0; i < mapCount; i++)
      texCoord[i].y = 1.0 - texCoord[i].y;
   
   for(int i = 0; i < 4; i++)
      fade[i] = (distAcc[i] * (2.0 * fadeConstant) - (fadeConstant - 1.0)) / 2.0;
}
      