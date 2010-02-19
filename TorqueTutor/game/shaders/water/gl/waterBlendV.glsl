uniform mat4 modelview;
uniform vec4 timeData;

varying vec2 tex0, tex1;

void main()
{
   gl_Position = modelview * gl_Vertex;
   tex0 = gl_MultiTexCoord0.st;
   tex1 = gl_MultiTexCoord0.st + vec2(timeData.x, 0.3);
}