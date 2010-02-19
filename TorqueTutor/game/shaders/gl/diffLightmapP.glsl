//-----------------------------------------------------------------------------
// Data
//-----------------------------------------------------------------------------
uniform sampler2D texMap, lightMap;

varying vec2 texCoord, lmCoord;

//-----------------------------------------------------------------------------
// Main                                                                        
//-----------------------------------------------------------------------------
void main()
{
   vec4 diffuseColor = texture2D( texMap, texCoord );
   vec4 lightmapColor = texture2D( lightMap, lmCoord );
   
   gl_FragColor = diffuseColor * lightmapColor;
}
