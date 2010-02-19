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
// Visibility
// Fog
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
   float4 shading         : COLOR;
   float2 fogCoord        : TEXCOORD1;
};


//-----------------------------------------------------------------------------
// Main                                                                        
//-----------------------------------------------------------------------------
ConnectData main( VertData IN,
                  uniform float4x4 modelview       : register(C0),
                  uniform float4   inLightColor    : register(C8),
                  uniform float3   inLightVec      : register(C9),
                  uniform float3   eyePosWorld     : register(C10),
                  uniform float3   fogData         : register(C11),
                  uniform float4x4 objTrans        : register(C4)
)
{
   ConnectData OUT;

   OUT.hpos = mul(modelview, IN.position);
   OUT.outTexCoord = IN.texCoord;
   OUT.shading = saturate( dot(-inLightVec, normalize(IN.normal)) );
   OUT.shading *= inLightColor;
   OUT.shading.w = 1.0;

   // fog setup
   float3 transPos = mul( objTrans, IN.position );
   OUT.fogCoord.x = 1.0 - ( distance( transPos, eyePosWorld ) / fogData.z );
   OUT.fogCoord.y = (transPos.z - fogData.x) * fogData.y;
   return OUT;
}
