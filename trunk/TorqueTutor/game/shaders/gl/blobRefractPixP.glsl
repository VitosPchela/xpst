uniform sampler2D refractMap, colorMap, bumpMap;
uniform vec4 specularColor;
uniform float specularPower;

varying vec2 texCoord;
varying vec3 outEyePos, pos;
varying vec4 hpos2;
varying vec3 lightVec;

void main()
{
   vec3 bumpNorm = texture2D(bumpMap, texCoord).rgb * 2.0 - 1.0;
   vec3 eyeVec = normalize(outEyePos - pos);
   vec2 texCoord2;
   texCoord2.x = clamp(dot(bumpNorm, eyeVec), 0.0, 1.0) - (0.5/128.0);
   texCoord2.y = 0.0;
   
   vec4 grad = texture2D(colorMap, texCoord2);
   
   vec3 refractVec = refract(vec3(0.0, 0.0, 1.0), normalize(bumpNorm), 1.0);
   
   vec2 tc;
   tc = vec2((hpos2.x + refractVec.x) / (hpos2.w),
             (hpos2.y + refractVec.y) / (hpos2.w));
   
   tc = clamp((tc + 1.0) * 0.5, 0.0, 1.0);
   
   gl_FragColor = texture2D(refractMap, tc) + grad;
   
   vec3 halfAng = normalize(eyeVec + lightVec);
   float specular = clamp(dot(bumpNorm, halfAng), 0.0, 1.0);
   specular = pow(specular, specularPower);
   gl_FragColor += specularColor * specular;
}