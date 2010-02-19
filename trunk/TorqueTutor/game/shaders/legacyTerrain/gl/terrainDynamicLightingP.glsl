uniform sampler2D diffuseMap, blackfogMap;
uniform sampler3D dlightMap;
uniform vec4 lightColor;

varying vec2 texCoord, fogCoord;
varying vec3 dlightCoord;

void main()
{
   vec4 diffuseColor= texture2D(diffuseMap, texCoord);
   diffuseColor *= texture3D(dlightMap, dlightCoord).r * lightColor * 2.0;
   vec4 fogColor = texture2D(blackfogMap, fogCoord);
   gl_FragColor = mix(diffuseColor, fogColor, fogColor.a);
}