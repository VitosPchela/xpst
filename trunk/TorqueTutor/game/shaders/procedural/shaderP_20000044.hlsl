//*****************************************************************************
// TGEA -- HLSL procedural shader                                              
//*****************************************************************************
//-----------------------------------------------------------------------------
// Structures                                                                  
//-----------------------------------------------------------------------------
// Features:
// Vert Position
// Base Texture
// Dynamic Lighting
// Generic ShaderFeature
struct ConnectData
{
   float2 texCoord        : TEXCOORD0;
   float4 dlightshading   : COLOR;
   float4 dlightCoord     : TEXCOORD1;
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
              uniform sampler3D dlightMap       : register(S1),
              uniform float4    lightColor      : register(C0)
)
{
   Fragout OUT;

   OUT.col = tex2D(diffuseMap, IN.texCoord);
   OUT.col.a = 1.0;
   float4 attn = tex3D(dlightMap, IN.dlightCoord) * lightColor * IN.dlightshading.x;
   OUT.col *= attn;

   return OUT;
}
