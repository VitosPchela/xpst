uniform sampler2D detMap;
uniform float brightnessScale;

varying vec2 detCoord;
varying vec3 fade;

void main()
{
   vec4 diff = texture2D(detMap, detCoord) * brightnessScale;
   gl_FragColor = mix(vec4(0.5, 0.5, 0.5, 1.0), diff, fade.x);
}