#define mapCount 3

uniform sampler2D diffuseMap0;
uniform sampler2D diffuseMap1;
uniform sampler2D diffuseMap2;

varying vec2 texCoord[mapCount];
varying vec4 fade;

void main()
{
   gl_FragColor = texture2D(diffuseMap0, texCoord[0].xy);
   
   float scaleFactor = clamp(fade[1] * 2.0, 0.0, 1.0);
   vec4 layer = texture2D(diffuseMap1, texCoord[1].xy);
   gl_FragColor = mix(layer, gl_FragColor, scaleFactor);
   
   scaleFactor = clamp(fade[2] * 2.0, 0.0, 1.0);
   layer = texture2D(diffuseMap2, texCoord[2].xy);
   gl_FragColor = mix(layer, gl_FragColor, scaleFactor);
}
