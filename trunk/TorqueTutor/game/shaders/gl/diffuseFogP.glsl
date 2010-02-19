//-----------------------------------------------------------------------------
// Data
//-----------------------------------------------------------------------------
uniform sampler2D diffuseMap, fogMap;
uniform vec4 ambient;

varying vec4 shading;
varying vec2 outTexCoord, fogCoord;

//-----------------------------------------------------------------------------
// Main                                                                        
//-----------------------------------------------------------------------------
void main()
{
   gl_FragColor = shading + ambient;
   gl_FragColor *= texture2D(diffuseMap, outTexCoord);
   vec4 fogColor = texture2D(fogMap, fogCoord);
   gl_FragColor = mix( gl_FragColor, fogColor, fogColor.a );
}
