uniform sampler2D refractMap, colorMap;
uniform vec4 specularColor;
uniform float specularPower;

varying vec3 outEyePos, normal, pos, screenNorm, lightVec;
varying vec4 hpos2;

void main()
{
   vec3 eyeVec = normalize(outEyePos - pos);
   vec2 texCoord2;
   texCoord2.x = clamp(dot(normal, eyeVec), 0.0, 1.0) - (0.5/128.0);
   texCoord2.y = 0.0;
   
   vec4 grad = texture2D(colorMap, texCoord2);
   
   vec3 refractVec = refract(vec3(0.0, 0.0, 1.0), screenNorm, 0.7);
   
   vec2 tc;
   tc = (hpos2.xy + refractVec.xy)/hpos2.w;
   tc = clamp((tc + 1.0) * 0.5, 0.0, 1.0);
   gl_FragColor = texture2D(refractMap, tc) + grad;
   
   vec3 halfAng = normalize(eyeVec + lightVec);
   float specular = clamp(dot(normal, halfAng), 0.0, 1.0);
   specular = pow(specular, specularPower);
   
   gl_FragColor += specularColor * specular;
}