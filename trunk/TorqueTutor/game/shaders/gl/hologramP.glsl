//-----------------------------------------------------------------------------
// Data
//-----------------------------------------------------------------------------
uniform sampler2D diffMap, bumpMap, diffMap2;
uniform samplerCube cubeMap;
uniform vec4 specularColor, ambient;
uniform float specularPower, accumTime;

varying vec4 TEX0, outLightVec;
varying vec3 outPos, outEyePos;

//-----------------------------------------------------------------------------
// Main
//-----------------------------------------------------------------------------
void main()
{
	vec2 texOffset, texOffset2;
	float sinOffset1 = sin( accumTime * 5.2 + TEX0.y * 1.5);
	float sinOffset2 = sin( accumTime * 3.2 + TEX0.y * 0.55);

	texOffset.x = TEX0.x + (sinOffset1 + sinOffset2) * 0.005;
	texOffset.y = TEX0.y;

	texOffset2.x = TEX0.x + (sinOffset1 - sinOffset2) * 0.05;
	texOffset2.y = TEX0.y + accumTime;

	vec4 bumpNorm = texture2D( bumpMap, texOffset ) * 2.0 - 1.0;
	
	vec4 diffA = texture2D( diffMap, texOffset );
	vec4 diffB = texture2D(diffMap2, texOffset2);
	diffB.a = 0.0;
	vec4 diffuse = diffA * 0.6 + diffB * 0.4;

	gl_FragColor = diffuse * (clamp( dot( outLightVec, vec4(bumpNorm.xyz, 1.0) ), 0.0, 1.0 ) + ambient);

	vec3 eyeVec = normalize(outEyePos - outPos);
	vec3 halfAng = normalize(eyeVec + outLightVec.xyz);
	float specular = clamp( dot(bumpNorm.xyz, halfAng), 0.0, 1.0 ) * outLightVec.w;
	specular = pow(specular, specularPower);
	gl_FragColor += specularColor * specular;
}

