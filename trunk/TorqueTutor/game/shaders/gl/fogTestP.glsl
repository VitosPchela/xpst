//-----------------------------------------------------------------------------
// Data
//-----------------------------------------------------------------------------
uniform sampler2D diffuseMap, lightMap, fogTex;

varying vec2 texCoord, lmCoord, fogCoord;

//-----------------------------------------------------------------------------
// Main                                                                        
//-----------------------------------------------------------------------------
void main()
{
   vec4 diffuseColor = texture2D(diffuseMap, texCoord);
   vec4 lmColor = texture2D(lightMap, lmCoord);
   vec4 fogColor = texture2D(fogTex, fogCoord);
   
   gl_FragColor = mix( diffuseColor * lmColor, fogColor, fogColor.a );
}
