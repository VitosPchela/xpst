uniform sampler2D baseTex, bumpMap;
uniform samplerCube cubeMap;
uniform vec4 specularColor;
uniform float specularPower;

varying vec2 outTexCoord;
varying vec3 outLightVec, reflectVec, pos, outEyePos;
varying vec4 shading;

void main()
{
   vec4 bumpNormal = texture2D(bumpMap, outTexCoord);
   vec4 bumpDot = vec4(clamp(dot(bumpNormal.xyz * 2.0 - 1.0, outLightVec), 0.0, 1.0));
   gl_FragColor = bumpDot * 0.75;
   gl_FragColor += shading;
   gl_FragColor *= texture2D(baseTex, outTexCoord);
   gl_FragColor += textureCube(cubeMap, reflectVec) * bumpNormal.a;
   
   vec3 eyeVec = normalize(outEyePos - pos);
   vec3 halfAng = normalize(eyeVec + outLightVec);
   float specular = clamp(dot(bumpNormal.xyz * 2.0 - 1.0, halfAng), 0.0, 1.0);
   specular = pow(specular, specularPower);
   gl_FragColor += specularColor * specular * bumpNormal.a * 8.0;
}