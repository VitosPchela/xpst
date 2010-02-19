//-----------------------------------------------------------------------------
// Data
//-----------------------------------------------------------------------------
uniform sampler2D refractMap, gradiantMap;

varying vec4 outSpecular;
varying vec2 texCoord, gradiantCoord;

//-----------------------------------------------------------------------------
// Main                                                                        
//-----------------------------------------------------------------------------
void main()
{
   gl_FragColor = texture2D( refractMap, texCoord );
   gl_FragColor += texture2D( gradiantMap, gradiantCoord );
   gl_FragColor += outSpecular;
}
