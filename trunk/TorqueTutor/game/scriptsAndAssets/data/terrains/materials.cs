//*****************************************************************************
// Custom Materials
//*****************************************************************************

// for writing to z buffer
new CustomMaterial( Blank )
{
   shader = BlankShader;
   version = 1.1;
};

new GFXSamplerStateData(SamplerBlendTextureAlpha)
{
   textureColorOp = GFXTOPBlendTextureAlpha;
   addressModeU = GFXAddressClamp;
   addressModeV = GFXAddressClamp;
   addressModeW = GFXAddressClamp;
   magFilter = GFXTextureFilterLinear;
   minFilter = GFXTextureFilterLinear;
   mipFilter = GFXTextureFilterLinear;
};

//*****************************************************************************
// Environmental Materials
//*****************************************************************************

new GFXStateBlockData(AtlasDynamicLightingStateBlock)
{
   blendDefined = true;
   blendEnable = true;
   blendSrc = GFXBlendSrcAlpha;
   blendDest = GFXBlendOne;
   
   alphaDefined = true;
   alphaTestEnable = true;
   alphaTestFunc = GFXCmpGreater;
   alphaTestRef = 0;
 
   samplersDefined = true;
   samplerStates[0] = SamplerClampLinear;
};

new GFXStateBlockData(AtlasDynamicLightingMaskStateBlock : AtlasDynamicLightingStateBlock)
{
   samplersStates[1] = SamplerClampLinear;
};

new CustomMaterial(AtlasDynamicLightingMaskMaterial)
{
   texture[0] = "$dynamiclight";
   texture[1] = "$dynamiclightmask";
   shader = AtlasDynamicLightingMaskShader;
   version = 1.1;
   preload = true;
   stateBlock = AtlasDynamicLightingMaskStateBlock;
};

new CustomMaterial(AtlasDynamicLightingMaterial)
{
   texture[0] = "$dynamiclight";
   shader = AtlasDynamicLightingShader;
   version = 1.1;
   preload = true;
   stateBlock = AtlasDynamicLightingStateBlock;
};

new CustomMaterial(AtlasMaterial)
{
   shader = AtlasShader;
   version = 1.1;

   dynamicLightingMaterial = AtlasDynamicLightingMaterial;
   dynamicLightingMaskMaterial = AtlasDynamicLightingMaskMaterial;
   preload = true;   
};

new GFXStateBlockData(TerrainLightingState)
{   
   blendDefined = true;
   blendEnable = true;
   blendSrc = GFXBlendOne;
   blendDest = GFXBlendOne;
 
   samplersDefined = true;
   samplerStates[0] = SamplerWrapLinear;
   samplerStates[1] = SamplerBlendTextureAlpha;   
   samplerStates[2] = SamplerClampLinear;   
};

new GFXStateBlockData(TerrainLightingStateFF)
{   
   blendDefined = true;
   blendEnable = true;
   blendSrc = GFXBlendOne;
   blendDest = GFXBlendOne;
 
   samplersDefined = true;
   samplerStates[0] = SamplerWrapLinear;
   samplerStates[1] = SamplerClampLinear;   
   vertexColorEnable = true;
};

new CustomMaterial(TerrainMaterialDynamicLightingFF)
{
   version = 0;
   preload = true;
   stateBlock = TerrainLightingStateFF;
};

new CustomMaterial(TerrainMaterialDynamicLightingMask)
{
   texture[1] = "$blackfog";
   texture[2] = "$dynamiclight";
   texture[3] = "$dynamiclightmask";
   
   shader = TerrDynamicLightingMaskShader;
   version = 1.1;
   preload = true;
   stateBlock = TerrainLightingState;
   fallback = TerrainMaterialDynamicLightingFF;
};

new CustomMaterial(TerrainMaterialDynamicLighting)
{
   texture[1] = "$blackfog";
   texture[2] = "$dynamiclight";

   shader = TerrDynamicLightingShader;
   version = 1.1;
   preload = true;
   stateBlock = TerrainLightingState;
   fallback = TerrainMaterialDynamicLightingFF;
};

// This material doesn't have a shader, it's here to get the dynamic lighting
// materials
new CustomMaterial(TerrainMaterial)
{
   version = 0;
   
   dynamicLightingMaterial = TerrainMaterialDynamicLighting;
   dynamicLightingMaskMaterial = TerrainMaterialDynamicLightingMask;
   preload = true;
};

new GFXStateBlockData(TerrainDetailStateBlock)
{   
   blendDefined = true;
   blendEnable = true;
   blendSrc = GFXBlendDestColor;
   blendDest = GFXBlendSrcColor;
 
   samplersDefined = true;
   samplerStates[0] = SamplerWrapLinear;
};

new CustomMaterial(TerrainDetailMaterial)
{
   shader = AtlasShaderDetail;
   pixVersion = 1.1;
   preload = true;
   
   stateBlock = TerrainDetailStateBlock;
};

new GFXSamplerStateData(TerrainDetailFF_S0)
{
   textureColorOp = GFXTOPModulate;
   colorArg1 = GFXTADiffuse;
   colorArg2 = GFXTATexture;
   
   addressModeU = GFXTextureAddressWrap;
   addressModeV = GFXTextureAddressWrap;
   addressModeW = GFXTextureAddressWrap;
   
   magFilter = GFXTextureFilterLinear;
   minFilter = GFXTextureFilterLinear;
   mipFilter = GFXTextureFilterLinear;
   
   textureTransform = GFXTTFFCoord2D;
};

new GFXSamplerStateData(TerrainDetailFF_LERPOP_S1)
{
   textureColorOp = GFXTOPLERP;
   colorArg1 = GFXTACurrent;
   colorArg2 = GFXTATFactor;
   colorArg3 = "AlphaReplicate GFXTADiffuse";
};

new GFXStateBlockData(TerrainDetailFFLerpOpStateBlock)
{
   vertexColorEnable = true;
   
   blendDefined = true;
   blendEnable = true;
   blendSrc = GFXBlendDestColor;
   blendDest = GFXBlendSrcColor;
   
   samplersDefined = true;
   samplerStates[0] = TerrainDetailFF_S0;
   samplerStates[1] = TerrainDetailFF_LERPOP_S1;
   
   // Set TFactor to ( 128, 128, 128, 255 ). These aren't magic numbers, detail
   // textures are created such that values > 0.5/128/0x80 will brighten the
   // area, and values less than 'middle' will darken the area.
   textureFactor = "128 128 128 255";   
};

new CustomMaterial(TerrainDetailFFLerpOpMaterial)
{
   version = 0;
   preload = true;
   stateBlock = TerrainDetailFFLerpOpStateBlock;      
};

new GFXSamplerStateData(TerrainDetailFF_4STAGE_S1)
{
   textureColorOp = GFXTOPModulate;
   colorArg1 = GFXTACurrent;
   colorArg2 = "AlphaReplicate GFXTADiffuse";
   resultArg = GFXTATemp;
};

// TerrainDetailFF_4STAGE_S2.colorArg1
new GFXSamplerStateData(TerrainDetailFF_4STAGE_S2)
{
   textureColorOp = GFXTOPModulate;
   colorArg1 = "OneMinus AlphaReplicate GFXTADiffuse";
   colorArg2 = GFXTATFactor;
};

new GFXSamplerStateData(TerrainDetailFF_4STAGE_S3)
{
   textureColorOp = GFXTOPAdd;
   colorArg1 = GFXTACurrent;
   colorArg2 = GFXTATemp;
};

new GFXStateBlockData(TerrainDetailFF4StageStateBlock)
{
   vertexColorEnable = true;
   
   blendDefined = true;
   blendEnable = true;
   blendSrc = GFXBlendDestColor;
   blendDest = GFXBlendSrcColor;
   
   samplersDefined = true;
   samplerStates[0] = TerrainDetailFF_S0;
   samplerStates[1] = TerrainDetailFF_4STAGE_S1;
   samplerStates[2] = TerrainDetailFF_4STAGE_S2;
   samplerStates[3] = TerrainDetailFF_4STAGE_S3;
   
   // Set TFactor to ( 128, 128, 128, 255 ). These aren't magic numbers, detail
   // textures are created such that values > 0.5/128/0x80 will brighten the
   // area, and values less than 'middle' will darken the area.
   textureFactor = "128 128 128 255";   
};

new CustomMaterial(TerrainDetailFF4StageMaterial)
{
   version = 0;
   preload = true;
   stateBlock = TerrainDetailFF4StageStateBlock;
};

new GFXStateBlockData(TerrainFogStateBlock)
{
   blendDefined = true;
   blendEnable = true;
   blendSrc = GFXBlendSrcAlpha;
   blendDest = GFXBlendInvSrcAlpha;
   
   alphaDefined = true;
   alphaTestEnable = true;
   alphaTestFunc = GFXCmpGreaterEqual;
   alphaTestRef = 2;
   
   samplersDefined = true;
   samplerStates[0] = SamplerClampLinear;
};

new CustomMaterial(TerrainFogMaterialFF)
{
   version = 0;
   preload = true;
   // Not read by FF material yet.
   texture[0] = "$fog";
   
   stateBlock = TerrainFogStateBlock;    
};

new CustomMaterial(TerrainFogMaterial)
{
   shader = AtlasShaderFog;
   pixVersion = 1.1;
   preload = true;
   texture[0] = "$fog";
   
   stateBlock = TerrainFogStateBlock;    
   fallback = TerrainFogMaterialFF;
};

new GFXStateBlockData(AtlasStateBlock2)
{
   samplersDefined = true;
   samplerStates[0] = SamplerWrapLinear;
   samplerStates[1] = SamplerWrapLinear;
};

new CustomMaterial(AtlasMaterial2)
{
   shader = AtlasShader2;
   pixVersion = 1.1;
   preload = true;
   stateBlock = AtlasStateBlock2;   
};

new GFXStateBlockData(AtlasStateBlock3)
{
   samplersDefined = true;
   samplerStates[0] = SamplerWrapLinear;
   samplerStates[1] = SamplerWrapLinear;
   samplerStates[2] = SamplerWrapLinear;
};

new CustomMaterial(AtlasMaterial3)
{
   shader = AtlasShader3;
   pixVersion = 1.1;
   preload = true;
   stateBlock = AtlasStateBlock3;   
};

new GFXStateBlockData(AtlasStateBlock4)
{
   samplersDefined = true;
   samplerStates[0] = SamplerWrapLinear;
   samplerStates[1] = SamplerWrapLinear;
   samplerStates[2] = SamplerWrapLinear;
   samplerStates[3] = SamplerWrapLinear;
};

new CustomMaterial(AtlasMaterial4)
{
   shader = AtlasShader4;
   pixVersion = 1.1;
   preload = true;
   stateBlock = AtlasStateBlock4;
};

new GFXStateBlockData(AtlasFFBasePassSB)
{
   samplersDefined = true;
   samplerStates[0] = SamplerWrapLinear;   
   vertexColorEnable = true;
};

new CustomMaterial(AtlasMaterialFFBasePass)
{   
   version = 0;
   preload = true;
   stateBlock = AtlasFFBasePassSB;   
};

new GFXStateBlockData(AtlasFFAddPassSB : AtlasFFBasePassSB)
{
   blendDefined = true;
   blendEnable = true;
   blendSrc = GFXBlendSrcAlpha;
   blendDest = GFXBlendInvSrcAlpha;      
};

new CustomMaterial(AtlasMaterialFFAddPass)
{
   version = 0;
   preload = true;
   stateBlock = AtlasFFAddPassSB;      
};