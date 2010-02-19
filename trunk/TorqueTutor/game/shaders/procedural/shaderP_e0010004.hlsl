//*****************************************************************************
// TGEA -- HLSL procedural shader                                              
//*****************************************************************************
//-----------------------------------------------------------------------------
// Structures                                                                  
//-----------------------------------------------------------------------------
// Features:
// Vert Position
// Base Texture
// Vert Light Color
// Generic ShaderFeature
// Visibility
// Fog
struct ConnectData
{
   float2 texCoord        : TEXCOORD0;
   float4 shading         : COLOR;
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
              uniform float4    ambient         : register(C0),
              uniform float     visibility      : register(C1),
              uniform sampler2D fogMap          : register(S1)
)
{
   Fragout OUT;

   OUT.col = tex2D(diffuseMap, IN.texCoord);
   float shadowed = 1.0;
   OUT.col *= float4( IN.shading.rgb * shadowed + ambient.rgb, 1 );
   OUT.col.w *= visibility;
   float4 fogColor = tex2D(fogMap, IN.fogCoord);
   OUT.col.rgb = lerp(OUT.col.rgb, fogColor.rgb, fogColor.a);

   return OUT;
}
