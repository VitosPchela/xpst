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
struct VertData
{
   float2 texCoord        : TEXCOORD0;
   float2 lmCoord         : TEXCOORD1;
   float3 T               : TEXCOORD2;
   float3 B               : TEXCOORD3;
   float3 normal          : NORMAL;
   float4 position        : POSITION;
};


struct ConnectData
{
   float4 hpos            : POSITION;
   float2 outTexCoord     : TEXCOORD0;
   float4 dlightshading   : COLOR;
   float4 dlightCoord     : TEXCOORD1;
};


//-----------------------------------------------------------------------------
// Main                                                                        
//-----------------------------------------------------------------------------
ConnectData main( VertData IN,
                  uniform float4x4 modelview       : register(C0),
                  uniform float4   lightPos        : register(C4),
                  uniform float4x4 objTrans        : register(C5),
                  uniform float4x4 lightingMatrix  : register(C9)
)
{
   ConnectData OUT;

   OUT.hpos = mul(modelview, IN.position);
   OUT.outTexCoord = IN.texCoord;

   float3 lightdir;
   float3 worldpos = mul(objTrans, IN.position.xyz);
   lightdir = worldpos - mul(objTrans, lightPos.xyz);
   OUT.dlightCoord.xyz = lightdir * lightPos.w;
   OUT.dlightCoord.xyz = mul(lightingMatrix, OUT.dlightCoord.xyz) + 0.5;
   OUT.dlightCoord.w = 1.0;

   float3 worldnorm = normalize(mul((float3x3)objTrans, IN.normal));
   OUT.dlightshading.xyz = saturate(dot(normalize(-lightdir), worldnorm));
   OUT.dlightshading.w = 1.0;
   return OUT;
}
