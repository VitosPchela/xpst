uniform sampler2D diffMap;
uniform sampler2D bumpMap;
uniform samplerCube cubeMap;
uniform vec4 specularColor;
uniform float specularPower;

varying vec2 TEX0;
varying vec4 tangentToCube0, tangentToCube1, tangentToCube2;
varying vec4 outLightVec;
varying vec3 outPos;
varying vec3 outEyePos;

void main()
{
   vec3 bumpNorm = texture2D( bumpMap, TEX0 ).xyz * 2.0 - 1.0;
   
   vec3 eye = vec3( tangentToCube0.w, 
                        tangentToCube1.w, 
                        tangentToCube2.w );

   mat3 cubeMat = mat3(  tangentToCube0.xyz,
                                 tangentToCube1.xyz,
                                 tangentToCube2.xyz );

   vec3 norm = cubeMat * bumpNorm;

   vec3 reflectVec = 2.0 * dot( eye, norm ) * norm - eye * dot( norm, norm );
                       
   
   vec4 diffuseColor = texture2D( diffMap, TEX0 );
   
   gl_FragColor = textureCube(cubeMap, reflectVec ) * diffuseColor;

   vec3 eyeVec = normalize(outEyePos - outPos);
   vec3 halfAng = normalize(eyeVec + outLightVec.xyz);
   float specular = clamp( dot(bumpNorm, halfAng), 0.0, 1.0 ) * outLightVec.w;
   specular = pow(specular, specularPower);
   gl_FragColor += specularColor * specular * diffuseColor.a;
}
