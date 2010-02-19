//-----------------------------------------------------------------------------
// Data
//-----------------------------------------------------------------------------
uniform sampler2D diffuseMap, lightMap;
uniform samplerCube cubeMap;

varying vec4 TEX0, TEX1;
varying vec3 reflectVec, fresnel;

//-----------------------------------------------------------------------------
// Main
//-----------------------------------------------------------------------------
void main()
{
   vec4 reflectColor = textureCube(cubeMap, reflectVec) * 0.1 * vec4(fresnel.xyz, 1.0);
   gl_FragColor = reflectColor + texture2D(lightMap, TEX1.xy) * texture2D(diffuseMap, TEX0.xy);
}
