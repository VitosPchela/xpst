//-----------------------------------------------------------------------------
// Data
//-----------------------------------------------------------------------------
uniform sampler2D refractMap, bumpMap;
uniform vec4 specularColor;
uniform float specularPower;

varying vec4 hpos2, vertColor;
varying vec2 texCoord;

//-----------------------------------------------------------------------------
// Main                                                                        
//-----------------------------------------------------------------------------
void main()
{
   vec4 bumpNorm = texture2D( bumpMap, texCoord );
   bumpNorm.xyz = bumpNorm.xyz * 2.0 - 1.0;
   
   vec3 refractVec = refract( vec3( 0.0, 0.0, 1.0), normalize( bumpNorm.xyz ), 1.0 );
  
   vec2 tc;
   tc = vec2( (  hpos2.x + (refractVec.x * vertColor.a * bumpNorm.a) ) / (hpos2.w), 
              ( -hpos2.y + (refractVec.y * vertColor.a * bumpNorm.a) ) / (hpos2.w) );

   tc = clamp( (tc + 1.0) * 0.5, 0.0, 1.0 );
   gl_FragColor = texture2D( refractMap, tc );
}

