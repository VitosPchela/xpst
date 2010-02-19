//-----------------------------------------------------------------------------
// Data 
//-----------------------------------------------------------------------------
uniform sampler2D diffuseMap, lightMap, bumpMap;

varying vec4 TEX0, TEX1, TEX2;
varying vec3 outLightDir;

//-----------------------------------------------------------------------------
// Main
//-----------------------------------------------------------------------------
void main()
{
   vec4 bumpNormal = texture2D( bumpMap, TEX2.xy ) * 2.0 - 1.0;
   vec4 base = texture2D(diffuseMap, TEX0.xy) * texture2D(lightMap, TEX1.xy);

   // expand light dir
   vec3 lightDir = outLightDir * 2.0 - 1.0;
   
   float bump = dot( bumpNormal.xyz, lightDir );
   
   gl_FragColor = base * bump;
}
