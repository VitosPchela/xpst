//-----------------------------------------------------------------------------
// Parallax Pixel Shader - For Garagegames TSE EA
// By: Jacob Dankovchik and Jevin Johnson
//-----------------------------------------------------------------------------
struct ConnectData
{
   float2 texCoord        : TEXCOORD0;
   float2 bumpCoord       : TEXCOORD1;
   float3 lightVec        : TEXCOORD2;
   float3 tangenteye:   TEXCOORD3;
   float2 TEX1:    TEXCOORD4;
};

struct Fragout
{
   float4 col : COLOR0;
};

   float2 ParallaxTexCoord(float2 oldcoord, sampler2D heightmap, float3 eye_vect, float parallax_amount)
{
   return (tex2D(heightmap, oldcoord) 
                     * parallax_amount - parallax_amount * 0.5)
                     * eye_vect + oldcoord;
}

//-----------------------------------------------------------------------------
// Main                                                                        
//-----------------------------------------------------------------------------
Fragout main( ConnectData IN,
              uniform sampler2D diffuseMap      : register(S0),
              uniform sampler2D bumpMap         : register(S1),
	      uniform sampler2D height_map      : register(S2),
              uniform float4    ambient         : register(C2),
			  uniform sampler2D lightMap       : register(S3)
)			  
{
   Fragout OUT;
   float3 eyevect = normalize(IN.tangenteye);

   float2 modified_texcoord = ParallaxTexCoord(IN.texCoord, height_map, eyevect, 0.1);
   OUT.col = tex2D(diffuseMap, modified_texcoord)* tex2D(lightMap, IN.TEX1);
   float4 bumpNormal = tex2D(bumpMap, modified_texcoord);

   IN.lightVec = IN.lightVec * 2.0 - 1.0;
   float4 bumpDot = saturate( dot(bumpNormal.xyz * 2.0 - 1.0, IN.lightVec.xyz) );
   OUT.col *= bumpDot + ambient;

   return OUT;
}