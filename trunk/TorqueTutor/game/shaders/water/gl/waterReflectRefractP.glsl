#define CLARITY 0
#define FRESNEL_BIAS 1
#define FRESNEL_SCALE 2

uniform sampler2D bumpMap, reflectMap, refractBuff, fogMap;
uniform vec4 specularColor, baseColor;
uniform vec3 miscParams;
uniform float specularPower;

varying vec4 texCoord, texCoord3;
varying vec3 outPos, outEyePos;
varying vec2 texCoord2, fogCoord;

float fresnel(in float NdotV, in float bias, in float power)
{
   return bias + (1.0-bias)*pow(1.0 - max(NdotV, 0.0), power);
}

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
   
   // get clean reflect / refract colors
   vec4 reflectColor = texture2DProj(reflectMap, distortCoord);
   vec4 refractColor = texture2DProj(refractBuff, distortCoord);

   // calc "diffuse" color
   vec4 diffuseColor = mix(baseColor, refractColor, miscParams[CLARITY]);

   // fresnel calculation
   vec3 eyeVec = normalize(outEyePos - outPos);
   const vec3 planeNorm = vec3(0.0, 0.0, 1.0);
   float fresnelTerm = fresnel(dot(eyeVec, planeNorm), miscParams[FRESNEL_BIAS], miscParams[FRESNEL_SCALE]);
   
   // output combined color
   gl_FragColor = mix(diffuseColor, reflectColor, fresnelTerm);

   // fog it
   vec4 fogColor = texture2D(fogMap, fogCoord);
   gl_FragColor = mix(gl_FragColor, fogColor, fogColor.a);
}
