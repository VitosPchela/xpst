uniform mat4 modelview;
uniform vec3 eyePos, inLightVec;

varying vec3 outEyePos, normal, pos, screenNorm, lightVec;
varying vec4 hpos2;

void main()
{
   gl_Position = modelview * gl_Vertex;
   
   hpos2 = gl_Position;
   normal = normalize( gl_Normal );
   screenNorm = vec3(modelview * vec4(normal, 0.0));
   outEyePos = eyePos / 100.0;
   pos = gl_Vertex.xyz / 100.0;
   lightVec = -inLightVec;
}