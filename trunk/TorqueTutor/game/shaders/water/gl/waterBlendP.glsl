uniform sampler2D texMap, texMap2;

varying vec2 tex0, tex1;

void main()
{
   vec4 texCol0 = texture2D(texMap, tex0);
   vec4 texCol1 = texture2D(texMap2, tex1);

   gl_FragColor = (texCol0 + texCol1) / 2.0;
}
