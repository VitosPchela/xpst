uniform sampler2D fogMap;

varying vec2 fogCoord;

void main()
{
   gl_FragColor = texture2D(fogMap, fogCoord);
}