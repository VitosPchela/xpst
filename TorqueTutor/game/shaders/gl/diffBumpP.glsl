//-----------------------------------------------------------------------------
// Data
//-----------------------------------------------------------------------------
uniform sampler2D diffuseMap, bumpMap;
uniform vec4 ambient;

varying vec3 outLightVec;
varying vec2 outTexCoord, bumpCoord;

//-----------------------------------------------------------------------------
// Main                                                                        
//-----------------------------------------------------------------------------
void main()
{
   gl_FragColor = texture2D(diffuseMap, outTexCoord);
   vec4 bumpNormal = texture2D(bumpMap, bumpCoord);

   vec3 lightVec = outLightVec * 2.0 - 1.0;
   vec4 bumpDot = vec4(clamp( dot(bumpNormal.xyz * 2.0 - 1.0, lightVec), 0.0, 1.0 ));
   gl_FragColor *= bumpDot + ambient;
}
