//-----------------------------------------------------------------------------
// Data
//-----------------------------------------------------------------------------
uniform sampler2D diffuseMap, blackfogMap;
uniform sampler3D dlightMap;
uniform samplerCube dlightMask;
uniform vec4 lightColor;

varying vec3 dlightCoord, dlightMaskCoord;
varying vec2 texCoord, fogCoord;

//-----------------------------------------------------------------------------
// Main                                                                        
//-----------------------------------------------------------------------------
void main()
{
   vec4 diffuseColor = texture2D(diffuseMap, texCoord);
   diffuseColor *= texture3D(dlightMap, dlightCoord) * lightColor * 2.0;
   diffuseColor *= textureCube(dlightMask, dlightMaskCoord);
   vec4 fogColor = texture2D(blackfogMap, fogCoord);
   gl_FragColor = mix( diffuseColor, fogColor, fogColor.a );
}
