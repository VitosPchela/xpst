#define IN_HLSL
#include "../shdrConsts.h"
#include "atlas.h"

//-----------------------------------------------------------------------------
// Main
//-----------------------------------------------------------------------------
VertDetailConnectData main( VertData IN,
                  uniform float4x4 modelView : register(C0),
                  uniform float3   eyePos    : register(VC_EYE_POS),
                  uniform float3   detData   : register(C50)
                  )
{
   VertDetailConnectData OUT;

   // Transform and return.
   OUT.hpos = mul(modelView, IN.position);
 
   // Do the detail map calculations.
   float eyeDist = distance( IN.position, eyePos );
   OUT.detCoord = IN.texCoord * detData.z;                 // Amplify texcoords.
   OUT.fade.x     = 1.0 - (eyeDist - detData.x) * detData.y; // And fade with distance.
   OUT.fade.yz    = 0.f;
   
   // Our fade should never go below zero
   OUT.fade.x = clamp(OUT.fade.x, 0, 1);
   
   return OUT;
}
