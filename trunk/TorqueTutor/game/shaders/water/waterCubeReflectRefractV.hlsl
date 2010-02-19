//*****************************************************************************
// TSE -- water shader                                               
//*****************************************************************************
//-----------------------------------------------------------------------------
// Structures                                                                  
//-----------------------------------------------------------------------------
struct VertData
{
   float4 position        : POSITION;
   float2 depth           : TEXCOORD0;
};


struct ConnectData
{
   float4 hpos             : POSITION;
   float4 texCoord         : TEXCOORD0;
   float3 lightVec         : TEXCOORD1;
   float2 texCoord2        : TEXCOORD2;
   float4 texCoord3        : TEXCOORD3;
   float2 fogCoord         : TEXCOORD4;
   float3 pos              : TEXCOORD6;
   float3 eyePos           : TEXCOORD7;
   float4 depth            : COLOR;
};


//-----------------------------------------------------------------------------
// Main                                                                        
//-----------------------------------------------------------------------------
ConnectData main( VertData IN,
                  uniform float4x4 modelview       : register(C0),
                  uniform float3x3 cubeTrans       : register(C4),
                  uniform float4x4 objTrans        : register(C7),
                  uniform float3   cubeEyePos      : register(C11),
                  uniform float3   eyePos          : register(C12),
                  uniform float2   waveData[3]     : register(C13),
                  uniform float4   timeData        : register(C16),
                  uniform float2   waveTexScale[3] : register(C17),
                  uniform float    reflectTexSize  : register(C20),
                  uniform float3   inLightVec      : register(C21),
                  uniform float3   fogData         : register(C22)
)
{
   ConnectData OUT;
   OUT.hpos = mul(modelview, IN.position);

   // set up tex coordinates for the 3 interacting normal maps
   OUT.texCoord.xy = IN.position.xy * waveTexScale[0];
   OUT.texCoord.xy += waveData[0].xy * timeData[0];

   OUT.texCoord.zw = IN.position.xy * waveTexScale[1];
   OUT.texCoord.zw += waveData[1].xy * timeData[1];

   OUT.texCoord2.xy = IN.position.xy * waveTexScale[2];
   OUT.texCoord2.xy += waveData[2].xy * timeData[2];

   // send misc data to pixel shaders
   OUT.pos = IN.position;
   OUT.eyePos = eyePos;
   OUT.lightVec = -inLightVec;
   
   // use projection matrix for reflection / refraction texture coords
   float4x4 texGen = { 0.5,  0.0,  0.0,  0.5 + 0.5 / reflectTexSize,
                       0.0, -0.5,  0.0,  0.5 + 1.0 / reflectTexSize,
                       0.0,  0.0,  1.0,  0.0,
                       0.0,  0.0,  0.0,  1.0 };

   OUT.texCoord3 = mul( texGen, OUT.hpos );


   // fog setup - may want to make this per-pixel
   float3 transPos = mul( objTrans, IN.position );
   OUT.fogCoord.x = 1.0 - ( distance( IN.position, eyePos ) / fogData.z );
   OUT.fogCoord.y = (transPos.z - fogData.x) * fogData.y;

   // send depth to the pixel shader
   OUT.depth.xy = IN.depth.xy;
   OUT.depth.z = 0;
   OUT.depth.w = 0;
   
   return OUT;

}

