//-----------------------------------------------------------------------------
// Data
//-----------------------------------------------------------------------------
uniform sampler3D dlightMap;
uniform samplerCube dlightMask;
uniform vec4 lightColor;

varying vec3 dlightCoord, dlightMaskCoord;

//-----------------------------------------------------------------------------
// Main                                                                        
//-----------------------------------------------------------------------------
void main()
{
   gl_FragColor = texture3D(dlightMap, dlightCoord) * lightColor;
   gl_FragColor *= textureCube(dlightMask, dlightMaskCoord);
   // alpha is sum(r,g,b) so we can filter unlit pixels with alpha test.
   gl_FragColor.w = gl_FragColor.x + gl_FragColor.y + gl_FragColor.z;
}
