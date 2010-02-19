//-----------------------------------------------------------------------------
// Data
//-----------------------------------------------------------------------------
uniform sampler2D bumpMap;
uniform samplerCube cubeMap;

varying vec4 tangentToCube0, tangentToCube1, tangentToCube2;
varying vec2 TEX0;

//-----------------------------------------------------------------------------
// Main
//-----------------------------------------------------------------------------
void main()
{
   vec3 bumpNorm = texture2D( bumpMap, TEX0 ).rgb * 2.0 - 1.0;
   
   vec3 eye = vec3( tangentToCube0.w, 
                    tangentToCube1.w, 
                    tangentToCube2.w );

   mat3 cubeMat = mat3(  tangentToCube0.xyz,
                         tangentToCube1.xyz,
                         tangentToCube2.xyz );

   vec3 norm = cubeMat * bumpNorm;

   vec3 reflectVec = 2.0 * dot( eye, norm ) * norm - eye * dot( norm, norm );
                       
   gl_FragColor = textureCube(cubeMap, reflectVec);
}
