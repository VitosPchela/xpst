//-----------------------------------------------------------------------------
// Structures                                                                  
//-----------------------------------------------------------------------------
struct ConnectData
{
   float2 texCoord   : TEXCOORD0;
   float2 lmCoord    : TEXCOORD1;
};


struct Fragout
{
   float4 col : COLOR0;
};




//-----------------------------------------------------------------------------
// Main                                                                        
//-----------------------------------------------------------------------------
Fragout main( ConnectData IN,
              uniform sampler2D texMap          : register(S0),
              uniform sampler2D lightMap        : register(S1)
)
{
   Fragout OUT;

   float4 diffuseColor = tex2D( texMap, IN.texCoord );
   float4 lightmapColor = tex2D( lightMap, IN.lmCoord );
   OUT.col = diffuseColor * lightmapColor;

   return OUT;
}
