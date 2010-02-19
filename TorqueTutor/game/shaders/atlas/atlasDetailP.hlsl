struct ConnectData
{
   float4 hpos      : POSITION;
   float2 detCoord  : TEXCOORD0;
   float3  fade     : TEXCOORD1;
};

struct Fragout
{
   float4 col : COLOR0;
};

Fragout main( ConnectData IN,
              uniform sampler2D detMap          : register(S0),
              uniform float brightnessScale     : register(C0)
              )
{
   Fragout OUT;
   
   float4 diff = tex2D(detMap, IN.detCoord) * brightnessScale;
   
   OUT.col = lerp(float4(0.5, 0.5, 0.5, 1), diff, IN.fade.x);
   return OUT;
}
