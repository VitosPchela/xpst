#define IN_HLSL
#include "shdrConsts.h"
#include "hlslStructs.h"

//-----------------------------------------------------------------------------
// Structures                                                                  
//-----------------------------------------------------------------------------

struct ConnectData
{
   float4 hpos            : POSITION;
   float2 texCoord        : TEXCOORD0;
   float2 lmCoord         : TEXCOORD1;
};

//-----------------------------------------------------------------------------
// Main                                                                        
//-----------------------------------------------------------------------------
ConnectData main( VertexIn_PNTTTB IN,
                  uniform float4x4 modelviewProj   : register(VC_WORLD_PROJ)
)
{
   ConnectData OUT;
   OUT.hpos = mul(modelviewProj, IN.pos);

   OUT.texCoord   = IN.uv0;
   OUT.lmCoord    = IN.uv1;
   
   return OUT;
}
