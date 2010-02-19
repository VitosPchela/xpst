//-----------------------------------------------------------------------------
// Data
//-----------------------------------------------------------------------------
uniform sampler2D refractMap, colorMap;
uniform vec4 specularColor;
uniform float specularPower;

//varying vec4 texCoord;
varying vec4 hpos2;
varying vec3 outEyePos, normal, pos, screenNorm, outLightVec;

//-----------------------------------------------------------------------------
// Main                                                                        
//-----------------------------------------------------------------------------
void main()
{
//   gl_FragColor = texture2D( colorMap, texCoord.xy );
   gl_FragColor = vec4( 0.05, 0.05, 0.1, 1.0 );

   
   vec3 eyeVec = normalize( outEyePos - pos );
   
   // Refraction   
   vec3 refractVec = refract( vec3( 0.0, 0.0, 1.0 ), screenNorm, 0.7 );
   
   vec2 tc;
   tc = vec2( (  hpos2.x + refractVec.x ) / (hpos2.w), 
              ( -hpos2.y + refractVec.y ) / (hpos2.w) );
   tc = clamp( (tc + 1.0) * 0.5, 0.0, 1.0 );
   gl_FragColor += texture2D( refractMap, tc );

  
   // Specular 
   vec3 halfAng = normalize(eyeVec + outLightVec);
   float specular = clamp( dot(normal, halfAng), 0.0, 1.0 );
   specular = pow(specular, specularPower);
   gl_FragColor += specularColor * specular;
}
