//-----------------------------------------------------------------------------
// Data
//-----------------------------------------------------------------------------
uniform sampler1D fresnelMap;
uniform sampler2D fogMap;
uniform samplerCube cubeMap;

varying vec2 TEX0, fogCoord;
varying vec3 reflectVec;

//-----------------------------------------------------------------------------
// Main
//-----------------------------------------------------------------------------
void main()
{
   vec4 fresCol = texture1D( fresnelMap, TEX0.x );
   vec4 skyCol = textureCube( cubeMap, reflectVec );
   skyCol.a = 1.0 - fresCol.a;
   
   
   gl_FragColor = mix( skyCol, fresCol, fresCol.a );
   gl_FragColor.rgb *= 0.5;

   vec4 fogColor = texture2D( fogMap, fogCoord );
   gl_FragColor = mix( gl_FragColor, fogColor, fogColor.a );
}

