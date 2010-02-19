uniform mat4 modelview;
uniform vec4 sourceTexScales;

varying vec2 tex, lmTex, texCoord1, texCoord2, texCoord3, texCoord4;

void main()
{
   gl_Position = modelview * gl_Vertex;
   
   tex = gl_MultiTexCoord0.st;
   lmTex = gl_MultiTexCoord1.st;
   texCoord1 = gl_MultiTexCoord2.st * sourceTexScales.x;
   texCoord2 = gl_MultiTexCoord2.st * sourceTexScales.y;
   texCoord3 = gl_MultiTexCoord2.st * sourceTexScales.z;
   texCoord4 = gl_MultiTexCoord2.st * sourceTexScales.w;
}
   