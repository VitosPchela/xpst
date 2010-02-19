//-----------------------------------------------------------------------------
// Data
//-----------------------------------------------------------------------------
uniform sampler2D baseTex, bumpMap;
uniform samplerCube cubeMap;

varying vec4 shading;
varying vec3 reflectVec, outLightVec;
varying vec2 outTexCoord, outBumpTexCoord;

//-----------------------------------------------------------------------------
// Main                                                                        
//-----------------------------------------------------------------------------
void main()
{
   vec4 bumpNormal = texture2D(bumpMap, outBumpTexCoord);
   vec4 bumpDot = vec4(clamp( dot(bumpNormal.xyz * 2.0 - 1.0, outLightVec * 2.0 - 1.0), 0.0, 1.0 ));
   gl_FragColor = bumpDot * 0.75;
   gl_FragColor += shading;
   gl_FragColor *= texture2D( baseTex, outTexCoord );
   gl_FragColor += textureCube(cubeMap, reflectVec) * bumpNormal.a;
}
