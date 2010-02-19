//*****************************************************************************
// TGEA -- HLSL procedural shader                                              
//*****************************************************************************
//-----------------------------------------------------------------------------
// Structures                                                                  
//-----------------------------------------------------------------------------
// Features:
// Vert Position
// Base Texture
// Lightmap
// Light Normal Map
// Generic ShaderFeature
// Visibility
// Fog
struct ConnectData
{
   float2 texCoord        : TEXCOORD0;
   float2 lmCoord         : TEXCOORD1;
   float2 fogCoord        : TEXCOORD2;
};


struct Fragout
{
   float4 col : COLOR0;
};


//-----------------------------------------------------------------------------
// Main                                                                        
//-----------------------------------------------------------------------------
Fragout main( ConnectData IN,
              uniform sampler2D diffuseMap      : register(S0),
              uniform sampler2D lightMap        : register(S1),
              uniform sampler2D lightNormMap    : register(S2),
              uniform float     visibility      : register(C0),
              uniform sampler2D fogMap          : register(S3)
)
{
   Fragout OUT;

   OUT.col = tex2D(diffuseMap, IN.texCoord);
   OUT.col *= tex2D(lightMap, IN.lmCoord);
   float4 lightmapNormal = tex2D(lightNormMap, IN.lmCoord);
   lightmapNormal.xyz = lightmapNormal.xyz * 2.0 - 1.0;
   OUT.col.w *= visibility;
   float4 fogColor = tex2D(fogMap, IN.fogCoord);
   OUT.col.rgb = lerp(OUT.col.rgb, fogColor.rgb, fogColor.a);

   return OUT;
}
