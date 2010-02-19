//-----------------------------------------------------------------------------
// Data
//-----------------------------------------------------------------------------
uniform samplerCube cubeMap;

varying vec3 cubeNormal, outCubeEyePos, pos;

//-----------------------------------------------------------------------------
// Main                                                                        
//-----------------------------------------------------------------------------
void main()
{
   vec3 eyeToPix = pos - outCubeEyePos;
   vec3 reflectVec = reflect(eyeToPix, cubeNormal);
   gl_FragColor = textureCube(cubeMap, reflectVec);
   gl_FragColor = gl_FragColor + vec4( 0.0, 0.0, 0.1, 0.0 );

/*   
   gl_FragColor = textureCube(cubeMap, reflectVec);
   gl_FragColor = gl_FragColor + vec4( 0.0, 0.0, 0.2, 0.0 );
*/
}
