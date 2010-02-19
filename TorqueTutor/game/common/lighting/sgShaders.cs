//-----------------------------------------------
// Synapse Gaming - Lighting System
// Copyright Synapse Gaming 2003
// Written by John Kabus
//-----------------------------------------------


new ShaderData(TerrDynamicLightingMaskShader)
{
   DXVertexShaderFile   = "shaders/legacyTerrain/terrainDynamicLightingMaskV.hlsl";
   DXPixelShaderFile    = "shaders/legacyTerrain/terrainDynamicLightingMaskP.hlsl";
   
   OGLVertexShaderFile  = "shaders/legacyTerrain/gl/terrainDynamicLightingMaskV.glsl";
   OGLPixelShaderFile   = "shaders/legacyTerrain/gl/terrainDynamicLightingMaskP.glsl";
   
   samplerNames[0] = "$diffuseMap";
   samplerNames[1] = "$blackfogMap";
   samplerNames[2] = "$dlightMap";
   samplerNames[3] = "$dlightMask";
   
   pixVersion = 1.1;
};

new ShaderData(TerrDynamicLightingShader)
{
   DXVertexShaderFile   = "shaders/legacyTerrain/terrainDynamicLightingV.hlsl";
   DXPixelShaderFile    = "shaders/legacyTerrain/terrainDynamicLightingP.hlsl";
   
   OGLVertexShaderFile  = "shaders/legacyTerrain/gl/terrainDynamicLightingV.glsl";
   OGLPixelShaderFile   = "shaders/legacyTerrain/gl/terrainDynamicLightingP.glsl";
   
   samplerNames[0] = "$diffuseMap";
   samplerNames[1] = "$blackfogMap";
   samplerNames[2] = "$dlightMap";
   
   pixVersion = 1.1;
};

new ShaderData(AtlasDynamicLightingMaskShader)
{
   DXVertexShaderFile   = "shaders/atlas/atlasSurfaceDynamicLightingMaskV.hlsl";
   DXPixelShaderFile    = "shaders/atlas/atlasSurfaceDynamicLightingMaskP.hlsl";
   
   OGLVertexShaderFile  = "shaders/atlas/gl/atlasSurfaceDynamicLightingMaskV.glsl";
   OGLPixelShaderFile   = "shaders/atlas/gl/atlasSurfaceDynamicLightingMaskP.glsl";
   
   samplerNames[0] = "$dlightMap";
   samplerNames[1] = "$dlightMask";
   
   pixVersion = 1.1;
};

new ShaderData(AtlasDynamicLightingShader)
{
   DXVertexShaderFile   = "shaders/atlas/atlasSurfaceDynamicLightingV.hlsl";
   DXPixelShaderFile    = "shaders/atlas/atlasSurfaceDynamicLightingP.hlsl";
   
   OGLVertexShaderFile  = "shaders/atlas/gl/atlasSurfaceDynamicLightingV.glsl";
   OGLPixelShaderFile   = "shaders/atlas/gl/atlasSurfaceDynamicLightingP.glsl";
   
   samplerNames[0] = "$dlightMap";
   
   pixVersion = 1.1;
};

new ShaderData(ShadowShaderFastPartition)
{
   DXVertexShaderFile 	= "shaders/lightingSystem/ShadowShaderV_1_1.hlsl";
   DXPixelShaderFile 	= "shaders/lightingSystem/ShadowShaderFastPartitionP.hlsl";

   OGLVertexShaderFile 	= "shaders/lightingSystem/gl/ShadowShaderV_1_1.glsl";
   OGLPixelShaderFile 	= "shaders/lightingSystem/gl/ShadowShaderFastPartitionP.glsl";

   samplerNames[0] = "$diffuseMap";

   pixVersion = 2.0;
};

new ShaderData(ShadowBuilderShader_2_0)
{
   DXVertexShaderFile 	= "shaders/lightingSystem/ShadowBuilderShaderV.hlsl";
   DXPixelShaderFile 	= "shaders/lightingSystem/ShadowBuilderShaderP.hlsl";

   OGLVertexShaderFile 	= "shaders/lightingSystem/gl/ShadowBuilderShaderV.glsl";
   OGLPixelShaderFile 	= "shaders/lightingSystem/gl/ShadowBuilderShaderP.glsl";

   pixVersion = 2.0;
};

new ShaderData(ShadowShaderHigh_2_0)
{
   DXVertexShaderFile 	= "shaders/lightingSystem/ShadowShaderV.hlsl";
   DXPixelShaderFile 	= "shaders/lightingSystem/ShadowShaderHighP.hlsl";

   OGLVertexShaderFile 	= "shaders/lightingSystem/gl/ShadowShaderV.glsl";
   OGLPixelShaderFile 	= "shaders/lightingSystem/gl/ShadowShaderHighP.glsl";

   samplerNames[0] = "$diffuseMap";

   pixVersion = 2.0;
   useDevicePixVersion = true;
};

new ShaderData(ShadowShader_2_0)
{
   DXVertexShaderFile 	= "shaders/lightingSystem/ShadowShaderV.hlsl";
   DXPixelShaderFile 	= "shaders/lightingSystem/ShadowShaderP.hlsl";

   OGLVertexShaderFile 	= "shaders/lightingSystem/gl/ShadowShaderV.glsl";
   OGLPixelShaderFile 	= "shaders/lightingSystem/gl/ShadowShaderP.glsl";

   samplerNames[0] = "$diffuseMap";

   pixVersion = 2.0;
   useDevicePixVersion = true;
};

new ShaderData(ShadowBuilderShader_1_1)
{
   DXVertexShaderFile 	= "shaders/lightingSystem/ShadowBuilderShaderV_1_1.hlsl";
   DXPixelShaderFile 	= "shaders/lightingSystem/ShadowBuilderShaderP_1_1.hlsl";

   OGLVertexShaderFile 	= "shaders/lightingSystem/gl/ShadowBuilderShaderV_1_1.glsl";
   OGLPixelShaderFile 	= "shaders/lightingSystem/gl/ShadowBuilderShaderP_1_1.glsl";

   pixVersion = 1.1;
};

new ShaderData(ShadowShader_1_1)
{
   DXVertexShaderFile 	= "shaders/lightingSystem/ShadowShaderV_1_1.hlsl";
   DXPixelShaderFile 	= "shaders/lightingSystem/ShadowShaderP_1_1.hlsl";

   OGLVertexShaderFile 	= "shaders/lightingSystem/gl/ShadowShaderV_1_1.glsl";
   OGLPixelShaderFile 	= "shaders/lightingSystem/gl/ShadowShaderP_1_1.glsl";

   samplerNames[0] = "$diffuseMap0";
   samplerNames[1] = "$diffuseMap1";
   samplerNames[2] = "$diffuseMap2";
   samplerNames[3] = "$diffuseMap3";

   pixVersion = 1.1;
};

new ShaderData(AlphaBloomShader)
{
   DXVertexShaderFile 	= "shaders/lightingSystem/AlphaBloomShaderV.hlsl";
   DXPixelShaderFile 	= "shaders/lightingSystem/AlphaBloomShaderP.hlsl";

   OGLVertexShaderFile 	= "shaders/lightingSystem/gl/AlphaBloomShaderV.glsl";
   OGLPixelShaderFile 	= "shaders/lightingSystem/gl/AlphaBloomShaderP.glsl";

   samplerNames[0] = "$diffuseMap";

   pixVersion = 2.0;
};

new ShaderData(DownSample4x4Shader)
{
   DXVertexShaderFile 	= "shaders/lightingSystem/DownSample8x4ShaderV.hlsl";
   DXPixelShaderFile 	= "shaders/lightingSystem/DownSample8x4ShaderP.hlsl";

   OGLVertexShaderFile 	= "shaders/lightingSystem/gl/DownSample8x4ShaderV.glsl";
   OGLPixelShaderFile 	= "shaders/lightingSystem/gl/DownSample8x4ShaderP.glsl";

   samplerNames[0] = "$diffuseMap";

   pixVersion = 2.0;
};

new ShaderData(DownSample4x4FinalShader)
{
   DXVertexShaderFile 	= "shaders/lightingSystem/DownSample8x4ShaderV.hlsl";
   DXPixelShaderFile 	= "shaders/lightingSystem/DownSample4x4FinalShaderP.hlsl";

   OGLVertexShaderFile 	= "shaders/lightingSystem/gl/DownSample8x4ShaderV.glsl";
   OGLPixelShaderFile 	= "shaders/lightingSystem/gl/DownSample4x4FinalShaderP.glsl";

   samplerNames[0] = "$diffuseMap";

   pixVersion = 2.0;
};

new ShaderData(DownSample4x4BloomClampShader)
{
   DXVertexShaderFile 	= "shaders/lightingSystem/DownSample4x4BloomClampShaderV.hlsl";
   DXPixelShaderFile 	= "shaders/lightingSystem/DownSample4x4BloomClampShaderP.hlsl";

   OGLVertexShaderFile 	= "shaders/lightingSystem/gl/DownSample4x4BloomClampShaderV.glsl";
   OGLPixelShaderFile 	= "shaders/lightingSystem/gl/DownSample4x4BloomClampShaderP.glsl";

   samplerNames[0] = "$diffuseMap";
   samplerNames[1] = "$viewMap";

   pixVersion = 2.0;
};

new ShaderData(BloomBlurShader)
{
   DXVertexShaderFile 	= "shaders/lightingSystem/BloomBlurShaderV.hlsl";
   DXPixelShaderFile 	= "shaders/lightingSystem/BloomBlurShaderP.hlsl";

   OGLVertexShaderFile 	= "shaders/lightingSystem/gl/BloomBlurShaderV.glsl";
   OGLPixelShaderFile 	= "shaders/lightingSystem/gl/BloomBlurShaderP.glsl";

   samplerNames[0] = "$diffuseMap";

   pixVersion = 2.0;
};

new ShaderData(DRLFullShader)
{
   DXVertexShaderFile 	= "shaders/lightingSystem/DRLShaderV.hlsl";
   DXPixelShaderFile 	= "shaders/lightingSystem/DRLShaderP.hlsl";

   OGLVertexShaderFile 	= "shaders/lightingSystem/gl/DRLShaderV.glsl";
   OGLPixelShaderFile 	= "shaders/lightingSystem/gl/DRLShaderP.glsl";

   samplerNames[0] = "$intensitymap";
   samplerNames[1] = "$bloommap1";
   samplerNames[3] = "$frame";
   samplerNames[5] = "$tonemap";

   pixVersion = 2.0;
};

new ShaderData(DRLOnlyBloomToneShader)
{
   DXVertexShaderFile 	= "shaders/lightingSystem/DRLShaderV.hlsl";
   DXPixelShaderFile 	= "shaders/lightingSystem/DRLOnlyBloomToneShaderP.hlsl";

   OGLVertexShaderFile 	= "shaders/lightingSystem/gl/DRLShaderV.glsl";
   OGLPixelShaderFile 	= "shaders/lightingSystem/gl/DRLOnlyBloomToneShaderP.glsl";

   samplerNames[1] = "$bloommap1";
   samplerNames[3] = "$frame";
   samplerNames[5] = "$tonemap";

   pixVersion = 2.0;
};



