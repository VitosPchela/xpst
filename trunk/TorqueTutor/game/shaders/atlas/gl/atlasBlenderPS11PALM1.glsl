//-----------------------------------------------------------------------------
// Data
//-----------------------------------------------------------------------------
uniform sampler2D opacity, lightMap, tex1, tex2;

varying vec2 texCoord0, texCoord1, texCoord2, lmTexCoord;

#ifndef LM_BLEND_FACTOR
#   define LM_BLEND_FACTOR   1.0
#endif

//-----------------------------------------------------------------------------
// Main
//-----------------------------------------------------------------------------
void main()
{
   // Get colors
   vec4 o  = texture2D(opacity,  texCoord0);
   vec4 t1 = texture2D(tex1,     texCoord1);
   vec4 t2 = texture2D(tex2,     texCoord2);
   vec4 lm = texture2D(lightMap, lmTexCoord);

   // Blend
   gl_FragColor = LM_BLEND_FACTOR * lm * (o.r * t1 + o.g * t2);
}
