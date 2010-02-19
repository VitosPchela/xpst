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
   float2 lmCoord         : TEXCOORD1;
   float2 fogCoord        : TEXCOORD2;
};


//-----------------------------------------------------------------------------
// Main                                                                        
//-----------------------------------------------------------------------------
ConnectData main( VertData IN,
                  uniform float4x4 modelview       : register(C0),
                  uniform float3   eyePosWorld     : register(C8),
                  uniform float3   fogData         : register(C9),
                  uniform float4x4 objTrans        : register(C4)
)
{
   ConnectData OUT;

   OUT.hpos = mul(modelview, IN.position);
   OUT.outTexCoord = IN.texCoord;
   OUT.lmCoord = IN.lmCoord;

   // fog setup
   float3 transPos = mul( objTrans, IN.position );
   OUT.fogCoord.x = 1.0 - ( distance( transPos, eyePosWorld ) / fogData.z );
   OUT.fogCoord.y = (transPos.z - fogData.x) * fogData.y;
   return OUT;
}
