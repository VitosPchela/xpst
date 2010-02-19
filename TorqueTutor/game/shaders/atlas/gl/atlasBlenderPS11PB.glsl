//-----------------------------------------------------------------------------
// Data
//-----------------------------------------------------------------------------
uniform sampler2D opacity, lightMap, tex3, tex4;

varying vec2 texCoord0, texCoord3, texCoord4, lmTexCoord;

#ifndef LM_BLEND_FACTOR
#   define LM_BLEND_FACTOR   2.0
#endif

//-----------------------------------------------------------------------------
// Main
//-----------------------------------------------------------------------------
void main()
{
   // Get colors
   vec4 o  = texture2D(opacity,  texCoord0);
   vec4 t3 = texture2D(tex3,     texCoord3);
   vec4 t4 = texture2D(tex4,     texCoord4);
   vec4 lm = texture2D(lightMap, lmTexCoord);

   // Blend
   gl_FragColor = LM_BLEND_FACTOR * lm * (o.b * t3 + o.a * t4);
}
