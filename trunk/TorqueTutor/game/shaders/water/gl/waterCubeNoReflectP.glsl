//-----------------------------------------------------------------------------
// TGEA -- water shader
//-----------------------------------------------------------------------------
#define CLARITY 0
#define FRESNEL_BIAS 1
#define FRESNEL_POWER 2

//-----------------------------------------------------------------------------
// Data                                                                  
//-----------------------------------------------------------------------------
uniform sampler2D bumpMap, reflectMap, refractBuff, fogMap;
uniform samplerCube skyMap;
uniform vec4 specularColor, baseColor;
uniform vec3 miscParams;
uniform float specularPower;

varying vec4 texCoord, texCoord3;
varying vec3 outLightVec, outPos, outEyePos;
varying vec2 texCoord2, fogCoord, depth;

//-----------------------------------------------------------------------------
// approximate Fresnel function
//-----------------------------------------------------------------------------
float fresnel(float NdotV, float bias, float power)
{
   return bias + (1.0-bias)*pow(1.0 - max(NdotV, 0.0), power);
}

//-----------------------------------------------------------------------------
// Main                                                                        
//-----------------------------------------------------------------------------
void main()
{
   vec3 bumpNorm = texture2D(bumpMap, texCoord.xy).rgb * 2.0 - 1.0;
   bumpNorm += texture2D(bumpMap, texCoord.zw).rgb * 2.0 - 1.0;
   
   // This large scale texture has 1/3 the influence as the other two.
   // Its purpose is to break up the repetitive patterns of the other two textures.
   bumpNorm += (texture2D(bumpMap, texCoord2).rgb * 2.0 - 1.0) * 0.3;
   
   // calc distortion, place in projected tex coords
   float distortion = (length( outEyePos - outPos ) / 20.0) + 0.15;
   vec4 distortCoord = texCoord3;
   distortCoord.xy += bumpNorm.xy * distortion;

   // use cubemap colors instead of reflection colors for waves that are angled towards camera
   bumpNorm.xy *= 0.75;
   bumpNorm = normalize( bumpNorm );
   vec3 eyeVec = outPos - outEyePos;
   vec3 reflect = eyeVec - bumpNorm * (dot(eyeVec, bumpNorm) * 2.0 );
   vec4 skyColor = textureCube( skyMap, reflect );
   eyeVec = -eyeVec;
   eyeVec = normalize( eyeVec );
   vec4 reflectColor = skyColor;
   vec4 refractColor = texture2DProj( refractBuff, distortCoord );

   // calc "diffuse" color
   vec4 diffuseColor = mix( baseColor, refractColor, miscParams[CLARITY] * depth.x);
   
   // fresnel calculation   
   const vec3 planeNorm = vec3( 0.0, 0.0, 1.0 );
   float fresnelTerm = fresnel( dot( eyeVec, planeNorm ), miscParams[FRESNEL_BIAS], miscParams[FRESNEL_POWER] );

   // output combined color
   gl_FragColor = mix( diffuseColor, reflectColor, fresnelTerm );



   // specular experiments

// 1:
/*
   float fDot = dot( bumpNorm, outLightVec );
   vec3 reflect = normalize( 2.0 * bumpNorm * fDot - eyeVec );
//   float specular = clamp(dot( reflect, outLightVec ), 0.0, 1.0 );
   float specular = pow( reflect, specularPower );
   gl_FragColor += specularColor * specular;
*/


// 2:  This almost looks good
   
   bumpNorm.xy *= 2.0;
   bumpNorm = normalize( bumpNorm );

   vec3 halfAng = normalize( eyeVec + outLightVec );
   float specular = clamp( dot( bumpNorm, halfAng), 0.0, 1.0 );
   specular = pow(specular, specularPower);
   gl_FragColor += specularColor * specular * depth.y;

   // fog it
   vec4 fogColor = texture2D(fogMap, fogCoord);
   gl_FragColor = mix( gl_FragColor, fogColor, fogColor.a );
}