//*****************************************************************************
// TGEA -- HLSL procedural shader                                              
//*****************************************************************************
//-----------------------------------------------------------------------------
// Structures                                                                  
//-----------------------------------------------------------------------------
// Features:
// Vert Position
// Base Texture
// Self Illumination
// Visibility
// Fog
struct ConnectData
{
   float2 texCoord        : TEXCOORD0;
   float2 fogCoord        : TEXCOORD1;
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
              uniform float4    lightColor      : register(C0),
              uniform float     visibility      : register(C1),
              uniform sampler2D fogMap          : register(S1)
)
{
   Fragout OUT;

   OUT.col = tex2D(diffuseMap, IN.texCoord);
   OUT.col *= lightColor;
   OUT.col.w *= visibility;
   float4 fogColor = tex2D(fogMap, IN.fogCoord);
   OUT.col.rgb = lerp(OUT.col.rgb, fogColor.rgb, fogColor.a);

   return OUT;
}
