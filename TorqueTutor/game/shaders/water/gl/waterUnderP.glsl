uniform sampler2D bumpMap, reflectMap, refractBuff, fogMap;
uniform vec4 baseColor;

varying vec4 texCoord, texCoord3;
varying vec3 pos, outEyePos;
varying vec2 texCoord2, fogCoord;

void main()
{
   vec3 bumpNorm = texture2D(bumpMap, texCoord.xy).rgb * 2.0 - 1.0;
   bumpNorm += texture2D(bumpMap, texCoord.zw).rgb * 2.0 - 1.0;

   // This large scale texture has 1/3 the influence as the other two.
   // Its purpose is to break up the repetitive patterns of the other two textures.
   bumpNorm += (texture2D(bumpMap, texCoord2).rgb * 2.0 - 1.0) * 0.3;

   // calc distortion, place in projected tex coords
   float distortion = (length( outEyePos - pos ) / 20.0) + 0.15;
   vec4 distortCoord = texCoord3;
   distortCoord.xy += bumpNorm.xy * distortion;
   
   // get clean refract color
   vec4 refractColor = texture2DProj(refractBuff, distortCoord);
   gl_FragColor = refractColor;

   // fog it
   vec4 fogColor = texture2D(fogMap, fogCoord);
   gl_FragColor = mix(gl_FragColor, fogColor, fogColor.a);
}
