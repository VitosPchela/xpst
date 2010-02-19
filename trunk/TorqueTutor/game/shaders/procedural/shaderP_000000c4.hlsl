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
// Dynamic Lighting Dummy
struct ConnectData
{
   float2 texCoord        : TEXCOORD0;
   float4 dlightshading   : COLOR;
   float4 dlightCoord     : TEXCOORD1;
   float4 dlightCoordSec  : TEXCOORD2;
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
              uniform float4    lightColor      : register(C0),
              uniform sampler3D dlightMapSec    : register(S2),
              uniform float4    lightColorSec   : register(C1)
)
{
   Fragout OUT;

   float4 diffuseColor = tex2D(diffuseMap, IN.texCoord);
   OUT.col = diffuseColor;
   OUT.col.a = 1.0;
   float4 attn = tex3D(dlightMap, IN.dlightCoord) * lightColor * IN.dlightshading.x;
   float4 attnsec = tex3D(dlightMapSec, IN.dlightCoordSec) * lightColorSec * IN.dlightshading.w;
   OUT.col *= (attn + attnsec);

   return OUT;
}
