
#define LM_BLEND_FACTOR 2.0

uniform sampler2D opacity, lightMap, tex1, tex2, tex3, tex4;

varying vec2 tex, lmTex, texCoord1, texCoord2, texCoord3, texCoord4;

void main()
{
   vec4 o = texture2D(opacity, tex);
   vec4 t1 = texture2D(tex1, texCoord1);
   vec4 t2 = texture2D(tex2, texCoord2);
   vec4 t3 = texture2D(tex3, texCoord3);
   vec4 t4 = texture2D(tex4, texCoord4);
   vec4 lm = texture2D(lightMap, lmTex);
   
   gl_FragColor = LM_BLEND_FACTOR * lm * (o.r * t1 + o.g * t2 + o.b * t3 + o.a * t4);
   gl_FragColor.a = 1.0;
}
