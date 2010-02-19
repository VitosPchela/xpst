//-----------------------------------------------------------------------------
// Parallax Vertex Shader - For TSE EA
// By: Jacob Dankovchik and Jevin Johnson
//-----------------------------------------------------------------------------
struct VertData
{
   float4 texCoord        : TEXCOORD0;
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
   float2 bumpCoord       : TEXCOORD1;
   float3 outLightVec     : TEXCOORD2;
   float3 tangenteye:   TEXCOORD3;
   float2 lightmap:     TEXCOORD4;
};

//-----------------------------------------------------------------------------
// Main                                                                        
//-----------------------------------------------------------------------------
ConnectData main( VertData IN,
                  uniform float4x4 modelview       : register(C0),
                  uniform float4x4 texMat          : register(C4),
                  uniform float3   inLightVec      : register(C24),
                  uniform float3   eyePos          : register(C20),
                  uniform float4x4 objTrans        : register(C12)
)
{
   ConnectData OUT;

   OUT.hpos = mul(modelview, IN.position);
   OUT.outTexCoord = mul(texMat, IN.texCoord);
   OUT.bumpCoord = OUT.outTexCoord;

   float3x3 objToTangentSpace;
   objToTangentSpace[0] = IN.T;
   objToTangentSpace[1] = IN.B;
   objToTangentSpace[2] = IN.normal;

   OUT.outLightVec.xyz = -inLightVec;
   OUT.outLightVec.xyz = mul(objToTangentSpace, OUT.outLightVec);
   OUT.outLightVec = OUT.outLightVec / 2.0 + 0.5;
   float3 objectspace_view_vector = mul( texMat, eyePos ) - IN.position;
   OUT.tangenteye =  mul( objToTangentSpace, objectspace_view_vector );
   OUT.lightmap = IN.lmCoord;

   return OUT;
}