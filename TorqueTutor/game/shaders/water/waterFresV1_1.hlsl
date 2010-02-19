#define IN_HLSL

//-----------------------------------------------------------------------------
// Constants
//-----------------------------------------------------------------------------
struct Appdata
{
	float4 position   : POSITION;
};


struct Conn
{
   float4 HPOS             : POSITION;
	float2 TEX0             : TEXCOORD1;
	float3 reflectVec       : TEXCOORD2;
	float2 fogCoord         : TEXCOORD3;
};



//-----------------------------------------------------------------------------
// Main
//-----------------------------------------------------------------------------
Conn main(  Appdata In, 
            uniform float4x4 modelview    : register(C0),
            uniform float3x3 cubeTrans    : register(C4),
            uniform float4x4 objTrans     : register(C7),
            uniform float3   cubeEyePos   : register(C11),
            uniform float3   eyePos       : register(C12),
            uniform float3   fogData      : register(C13)
)
{
   Conn Out;

   Out.HPOS = mul(modelview, In.position);
   
   float3 eyeToVert = normalize( eyePos - In.position );
   Out.TEX0.x = dot( eyeToVert, float3( 0.0, 0.0, 1.0 ) );
   Out.TEX0.y = 0.0;
   
   float3 cubeVertPos = mul(cubeTrans, In.position).xyz;
   float3 cubeNormal = normalize( mul(cubeTrans, float3( 0.0, 0.0, 1.0 )).xyz );
   float3 cubeEyeToVert = cubeVertPos - cubeEyePos;
   Out.reflectVec = reflect(cubeEyeToVert, cubeNormal);

   float3 transPos = mul( objTrans, In.position );
   Out.fogCoord.x = 1.0 - ( distance( In.position, eyePos ) / fogData.z );
   Out.fogCoord.y = (transPos.z - fogData.x) * fogData.y;

   return Out;
}


