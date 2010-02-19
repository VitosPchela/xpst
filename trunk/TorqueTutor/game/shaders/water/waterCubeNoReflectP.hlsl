//*****************************************************************************
// TSE -- water shader
//*****************************************************************************
//-----------------------------------------------------------------------------
// Structures                                                                  
//-----------------------------------------------------------------------------
#define CLARITY 0
#define FRESNEL_BIAS 1
#define FRESNEL_POWER 2

struct ConnectData
{
   float4 texCoord   : TEXCOORD0;
   float3 lightVec   : TEXCOORD1;
   float2 texCoord2  : TEXCOORD2;
   float4 texCoord3  : TEXCOORD3;
   float2 fogCoord   : TEXCOORD4;
   float3 pos        : TEXCOORD6;
   float3 eyePos     : TEXCOORD7;
   float4 depth      : COLOR;
};

struct Fragout
{
   float4 col : COLOR0;
};

//-----------------------------------------------------------------------------
// approximate Fresnel function
//-----------------------------------------------------------------------------
float fresnel(float NdotV, float bias, float power)
{
   return bias + (1.0-bias)*pow(abs(1.0 - max(NdotV, 0)), power);
}

//-----------------------------------------------------------------------------
// Main                                                                        
//-----------------------------------------------------------------------------
Fragout main( ConnectData IN,
              uniform sampler    bumpMap           : register(S0),
              uniform sampler2D  reflectMap        : register(S1),
              uniform sampler    refractBuff       : register(S2),
              uniform sampler    fogMap            : register(S3),
              uniform samplerCUBE skyMap           : register(S4),
              uniform float4     specularColor     : register(C0),
              uniform float      specularPower     : register(C1),
              uniform float4     baseColor         : register(C2),
              uniform float3     miscParams        : register(C3)
)
{
   Fragout OUT;

                       

   float3 bumpNorm = tex2D(bumpMap, IN.texCoord.xy) * 2.0 - 1.0;
   bumpNorm += tex2D(bumpMap, IN.texCoord.zw) * 2.0 - 1.0;
   
   // This large scale texture has 1/3 the influence as the other two.
   // Its purpose is to break up the repetitive patterns of the other two textures.
   bumpNorm += (tex2D(bumpMap, IN.texCoord2) * 2.0 - 1.0) * 0.3;
   
   // calc distortion, place in projected tex coords
   float distortion = (length( IN.eyePos - IN.pos ) / 20.0) + 0.15;
   IN.texCoord3.xy += bumpNorm.xy * distortion;

   // use cubemap colors instead of reflection colors for waves that are angled towards camera
   bumpNorm.xy *= 0.75;
   bumpNorm = normalize( bumpNorm );
   float3 eyeVec = IN.pos - IN.eyePos;
   float3 reflect = eyeVec - bumpNorm * (dot(eyeVec, bumpNorm) * 2.0 );
   float4 skyColor = texCUBE( skyMap, reflect );
   eyeVec = -eyeVec;
   eyeVec = normalize( eyeVec );

   float4 reflectColor = skyColor;
   float4 refractColor = tex2Dproj( refractBuff, IN.texCoord3 );

   // calc "diffuse" color
   float4 diffuseColor = lerp( baseColor, refractColor, miscParams[CLARITY] * IN.depth.x);
   
   // fresnel calculation   
   const float3 planeNorm = float3( 0.0, 0.0, 1.0 );
   float fresnelTerm = fresnel( dot( eyeVec, planeNorm ), miscParams[FRESNEL_BIAS], miscParams[FRESNEL_POWER] );

   // output combined color
   OUT.col = lerp( diffuseColor, reflectColor, fresnelTerm );



   // specular experiments

// 1:
/*
   float fDot = dot( bumpNorm, IN.lightVec );
   float3 reflect = normalize( 2.0 * bumpNorm * fDot - eyeVec );
//   float specular = saturate(dot( reflect, IN.lightVec ) );
   float specular = pow( reflect, specularPower );
   OUT.col += specularColor * specular;
*/


// 2:  This almost looks good
   
   bumpNorm.xy *= 2.0;
   bumpNorm = normalize( bumpNorm );

   float3 halfAng = normalize( eyeVec + IN.lightVec );
   float specular = saturate( dot( bumpNorm, halfAng) );
   specular = pow(specular, specularPower);
   OUT.col += specularColor * specular * IN.depth.y;

   // fog it
   float4 fogColor = tex2D(fogMap, IN.fogCoord);
   OUT.col = lerp( OUT.col, fogColor, fogColor.a );

   return OUT;
}

