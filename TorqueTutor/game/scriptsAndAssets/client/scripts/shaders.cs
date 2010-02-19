//*****************************************************************************
// Shaders  ( For Custom Materials )
//*****************************************************************************

//-----------------------------------------------------------------------------
// Planar Reflection
//-----------------------------------------------------------------------------
new ShaderData( ReflectBump )
{
   DXVertexShaderFile 	= "shaders/planarReflectBumpV.hlsl";
   DXPixelShaderFile 	= "shaders/planarReflectBumpP.hlsl";
   
   OGLVertexShaderFile 	= "shaders/gl/planarReflectBumpV.glsl";
   OGLPixelShaderFile 	= "shaders/gl/planarReflectBumpP.glsl";
              
   samplerNames[0] = "$diffuseMap";
   samplerNames[1] = "$refractMap";
   samplerNames[2] = "$bumpMap";
   
   pixVersion = 2.0;
};

new ShaderData( Reflect )
{
   DXVertexShaderFile 	= "shaders/planarReflectV.hlsl";
   DXPixelShaderFile 	= "shaders/planarReflectP.hlsl";
   
   OGLVertexShaderFile 	= "shaders/gl/planarReflectV.glsl";
   OGLPixelShaderFile 	= "shaders/gl/planarReflectP.glsl";
   
   samplerNames[0] = "$diffuseMap";
   samplerNames[1] = "$refractMap";
   
   pixVersion = 1.4;
};

//-----------------------------------------------------------------------------
// Water
//-----------------------------------------------------------------------------
new ShaderData( WaterFres1_1 )
{
   DXVertexShaderFile 	= "shaders/water/waterFresV1_1.hlsl";
   DXPixelShaderFile 	= "shaders/water/waterFresP1_1.hlsl";
   
   OGLVertexShaderFile 	= "shaders/water/gl/waterFresV1_1.glsl";
   OGLPixelShaderFile 	= "shaders/water/gl/waterFresP1_1.glsl";
               
   samplerNames[1] = "$fresnelMap";
   samplerNames[2] = "$cubeMap";
   samplerNames[3] = "$fogMap";
   
   pixVersion = 1.1;
};

new ShaderData( WaterBlend )
{
   DXVertexShaderFile 	= "shaders/water/WaterBlendV.hlsl";
   DXPixelShaderFile 	= "shaders/water/WaterBlendP.hlsl";
   
   OGLVertexShaderFile 	= "shaders/water/gl/WaterBlendV.glsl";
   OGLPixelShaderFile 	= "shaders/water/gl/WaterBlendP.glsl";
				
	samplerNames[0] = "$texMap";
   samplerNames[1] = "$texMap2";
   
   pixVersion = 1.1;
};

new ShaderData( Water1_1 )
{
   DXVertexShaderFile 	= "shaders/water/waterV1_1.hlsl";
   DXPixelShaderFile 	= "shaders/water/waterP1_1.hlsl";
   
   OGLVertexShaderFile 	= "shaders/water/gl/waterV1_1.glsl";
   OGLPixelShaderFile 	= "shaders/water/gl/waterP1_1.glsl";
            
   samplerNames[0] = "$bumpMap";
   samplerNames[1] = "$cubeMap";
   
   pixVersion = 1.1;
};

new ShaderData( WaterCubeReflectRefract )
{
   DXVertexShaderFile 	= "shaders/water/waterCubeReflectRefractV.hlsl";
   DXPixelShaderFile 	= "shaders/water/waterCubeReflectRefractP.hlsl";
   
   OGLVertexShaderFile  = "shaders/water/gl/waterCubeReflectRefractV.glsl";
   OGLPixelShaderFile   = "shaders/water/gl/waterCubeReflectRefractP.glsl";
   
   samplerNames[0] = "$bumpMap";
   samplerNames[1] = "$reflectMap";
   samplerNames[2] = "$refractBuff";
   samplerNames[3] = "$fogMap";
   samplerNames[4] = "$skyMap";
   
   pixVersion = 2.0;
};

new ShaderData( WaterCubeNoReflect )
{
   DXVertexShaderFile 	= "shaders/water/waterCubeNoReflectV.hlsl";
   DXPixelShaderFile 	= "shaders/water/waterCubeNoReflectP.hlsl";
   
   OGLVertexShaderFile  = "shaders/water/gl/waterCubeNoReflectV.glsl";
   OGLPixelShaderFile   = "shaders/water/gl/waterCubeNoReflectP.glsl";
   
   samplerNames[0] = "$bumpMap";
   samplerNames[2] = "$refractBuff";
   samplerNames[3] = "$fogMap";
   samplerNames[4] = "$skyMap";
   
   pixVersion = 2.0;
};

new ShaderData( WaterReflectRefract )
{
   DXVertexShaderFile 	= "shaders/water/waterReflectRefractV.hlsl";
   DXPixelShaderFile 	= "shaders/water/waterReflectRefractP.hlsl";
   
   OGLVertexShaderFile  = "shaders/water/gl/waterReflectRefractV.glsl";
   OGLPixelShaderFile   = "shaders/water/gl/waterReflectRefractP.glsl";
   
   samplerNames[0] = "$bumpMap";
   samplerNames[1] = "$reflectMap";
   samplerNames[2] = "$refractBuff";
   samplerNames[3] = "$fogMap";
   
   pixVersion = 2.0;
};

new ShaderData( WaterUnder2_0 )
{
   DXVertexShaderFile 	= "shaders/water/waterUnderV2_0.hlsl";
   DXPixelShaderFile 	= "shaders/water/waterUnderP2_0.hlsl";
   
   OGLVertexShaderFile  = "shaders/water/gl/waterUnderV.glsl";
   OGLPixelShaderFile   = "shaders/water/gl/waterUnderP.glsl";
   
   samplerNames[0] = "$bumpMap";
   samplerNames[1] = "$reflectMap";
   samplerNames[2] = "$refractBuff";
   samplerNames[3] = "$fogMap";
   
   pixVersion = 2.0;
};




//-----------------------------------------------------------------------------
// Lightmap with light-normal and bump maps
//-----------------------------------------------------------------------------
new ShaderData( LmapBump )
{
   DXVertexShaderFile 	= "shaders/baseInteriorV.hlsl";
   DXPixelShaderFile 	= "shaders/baseInteriorP.hlsl";
   
   OGLVertexShaderFile 	= "shaders/gl/baseInteriorV.glsl";
   OGLPixelShaderFile 	= "shaders/gl/baseInteriorP.glsl";
            
   samplerNames[0] = "$diffuseMap";
   samplerNames[1] = "$lightMap";
   samplerNames[2] = "$bumpMap";
   
   pixVersion = 1.4;
};

//-----------------------------------------------------------------------------
// Reflect cubemap
//-----------------------------------------------------------------------------
new ShaderData( Cubemap )
{
   DXVertexShaderFile 	= "shaders/lmapCubeV.hlsl";
   DXPixelShaderFile 	= "shaders/lmapCubeP.hlsl";
   
   OGLVertexShaderFile 	= "shaders/gl/lmapCubeV.glsl";
   OGLPixelShaderFile 	= "shaders/gl/lmapCubeP.glsl";
            
   samplerNames[0] = "$diffuseMap";
   samplerNames[1] = "$lightMap";
   samplerNames[2] = "$cubeMap";
   
   pixVersion = 1.4;
};

//-----------------------------------------------------------------------------
// Bump reflect cubemap
//-----------------------------------------------------------------------------
new ShaderData( BumpCubemap )
{
   DXVertexShaderFile 	= "shaders/bumpCubeV.hlsl";
   DXPixelShaderFile 	= "shaders/bumpCubeP.hlsl";
   
   OGLVertexShaderFile 	= "shaders/gl/bumpCubeV.glsl";
   OGLPixelShaderFile 	= "shaders/gl/bumpCubeP.glsl";
            
   samplerNames[0] = "$bumpMap";
   samplerNames[3] = "$cubeMap";
   
   pixVersion = 1.1;
};

//-----------------------------------------------------------------------------
// Diffuse + Bump
//-----------------------------------------------------------------------------
new ShaderData( DiffuseBump )
{
   DXVertexShaderFile 	= "shaders/diffBumpV.hlsl";
   DXPixelShaderFile 	= "shaders/diffBumpP.hlsl";
   
   OGLVertexShaderFile 	= "shaders/gl/diffBumpV.glsl";
   OGLPixelShaderFile 	= "shaders/gl/diffBumpP.glsl";
          
   samplerNames[0] = "$diffuseMap";
   samplerNames[1] = "$bumpMap";
   
   pixVersion = 1.4;
};

//-----------------------------------------------------------------------------
// Diffuse + Fog
//-----------------------------------------------------------------------------
new ShaderData( DiffuseFog )
{
   DXVertexShaderFile 	= "shaders/diffuseFogV.hlsl";
   DXPixelShaderFile 	= "shaders/diffuseFogP.hlsl";
   
   OGLVertexShaderFile 	= "shaders/gl/diffuseFogV.glsl";
   OGLPixelShaderFile 	= "shaders/gl/diffuseFogP.glsl";
       
   samplerNames[0] = "$diffuseMap";
   samplerNames[1] = "$fogMap";
   
   pixVersion = 1.1;
};

//-----------------------------------------------------------------------------
// Diffuse + Lightmap
//-----------------------------------------------------------------------------
new ShaderData( DiffLmap )
{
   DXVertexShaderFile   = "shaders/diffLightmapV.hlsl";
   DXPixelShaderFile    = "shaders/diffLightmapP.hlsl";
   
   OGLVertexShaderFile  = "shaders/gl/diffLightmapV.glsl";
   OGLPixelShaderFile   = "shaders/gl/diffLightmapP.glsl";

   samplerNames[0] = "$texMap";
   samplerNames[1] = "$lightMap";
   
   pixVersion = 1.1;
};

//-----------------------------------------------------------------------------
// Bump reflect cubemap with diffuse texture
//-----------------------------------------------------------------------------
new ShaderData( BumpCubeDiff )
{
   DXVertexShaderFile 	= "shaders/bumpCubeDiffuseV.hlsl";
   DXPixelShaderFile 	= "shaders/bumpCubeDiffuseP.hlsl";
   
   OGLVertexShaderFile  = "shaders/gl/bumpCubeDiffuseV.glsl";
   OGLPixelShaderFile   = "shaders/gl/bumpCubeDiffuseP.glsl";
   
   samplerNames[0] = "$bumpMap";
   samplerNames[1] = "$diffMap";
   samplerNames[3] = "$cubeMap";
   
   pixVersion = 2.0;
};


//-----------------------------------------------------------------------------
// Fog Test
//-----------------------------------------------------------------------------
new ShaderData( FogTest )
{
   DXVertexShaderFile 	= "shaders/fogTestV.hlsl";
   DXPixelShaderFile 	= "shaders/fogTestP.hlsl";
   
   OGLVertexShaderFile 	= "shaders/gl/fogTestV.glsl";
   OGLPixelShaderFile 	= "shaders/gl/fogTestP.glsl";
        
   samplerNames[0] = "$diffuseMap";
   samplerNames[1] = "$lightMap";
   samplerNames[2] = "$fogTex";
   
   pixVersion = 1.1;
};

//-----------------------------------------------------------------------------
// Vertex refraction
//-----------------------------------------------------------------------------
new ShaderData( RefractVert )
{
   DXVertexShaderFile 	= "shaders/refractVertV.hlsl";
   DXPixelShaderFile 	= "shaders/refractVertP.hlsl";
   
   OGLVertexShaderFile 	= "shaders/gl/refractVertV.glsl";
   OGLPixelShaderFile 	= "shaders/gl/refractVertP.glsl";
   
   samplerNames[0] = "$refractMap";
   
   pixVersion = 2.0;
};

//-----------------------------------------------------------------------------
// Custom shaders for blob
//-----------------------------------------------------------------------------
new ShaderData( BlobRefractVert )
{
   DXVertexShaderFile 	= "shaders/blobRefractVertV.hlsl";
   DXPixelShaderFile 	= "shaders/blobRefractVertP.hlsl";
   
   OGLVertexShaderFile  = "shaders/gl/blobRefractVertV.glsl";
   OGLPixelShaderFile   = "shaders/gl/blobRefractVertP.glsl";
   
   samplerNames[0] = "$refractMap";
   samplerNames[1] = "$colorMap";
   
   pixVersion = 2.0;
};

new ShaderData( BlobRefractVert1_1 )
{
   DXVertexShaderFile 	= "shaders/blobRefractVertV1_1.hlsl";
   DXPixelShaderFile 	= "shaders/blobRefractVertP1_1.hlsl";
   
   OGLVertexShaderFile 	= "shaders/gl/blobRefractVertV1_1.glsl";
   OGLPixelShaderFile 	= "shaders/gl/blobRefractVertP1_1.glsl";

   samplerNames[0] = "$refractMap";
   samplerNames[1] = "$gradiantMap";
   
   pixVersion = 1.1;
};

new ShaderData( BlobRefractPix )
{
   DXVertexShaderFile 	= "shaders/blobRefractPixV.hlsl";
   DXPixelShaderFile 	= "shaders/blobRefractPixP.hlsl";
   
   OGLVertexShaderFile  = "shaders/gl/blobRefractPixV.glsl";
   OGLPixelShaderFile   = "shaders/gl/blobRefractPixP.glsl";
   
   samplerNames[0] = "$refractMap";
   samplerNames[1] = "$colorMap";
   samplerNames[2] = "$bumpMap";
   
   pixVersion = 2.0;
};

//-----------------------------------------------------------------------------
// Custom shader for reflective sphere
//-----------------------------------------------------------------------------
new ShaderData( ReflectSphere )
{
   DXVertexShaderFile 	= "shaders/reflectSphereV.hlsl";
   DXPixelShaderFile 	= "shaders/reflectSphereP.hlsl";
   
   OGLVertexShaderFile 	= "shaders/gl/reflectSphereV.glsl";
   OGLPixelShaderFile 	= "shaders/gl/reflectSphereP.glsl";
   
   samplerNames[0] = "$cubeMap";
   
   pixVersion = 2.0;
};

new ShaderData( ReflectSphere1_1 )
{
   DXVertexShaderFile 	= "shaders/reflectSphereV1_1.hlsl";
   DXPixelShaderFile 	= "shaders/reflectSphereP1_1.hlsl";
   
   OGLVertexShaderFile 	= "shaders/gl/reflectSphereV1_1.glsl";
   OGLPixelShaderFile 	= "shaders/gl/reflectSphereP1_1.glsl";
   
   samplerNames[0] = "$cubeMap";
   
   pixVersion = 1.1;
};

new ShaderData( TendrilShader )
{
   DXVertexShaderFile 	= "shaders/tendrilV.hlsl";
   DXPixelShaderFile 	= "shaders/tendrilP.hlsl";
   
   OGLVertexShaderFile 	= "shaders/gl/tendrilV.glsl";
   OGLPixelShaderFile 	= "shaders/gl/tendrilP.glsl";

   samplerNames[0] = "$refractMap";
   samplerNames[1] = "$colorMap";
   
   pixVersion = 2.0;
};

new ShaderData( TendrilShader1_1 )
{
   DXVertexShaderFile 	= "shaders/tendrilV1_1.hlsl";
   DXPixelShaderFile 	= "shaders/tendrilP1_1.hlsl";
   
   OGLVertexShaderFile 	= "shaders/gl/tendrilV1_1.glsl";
   OGLPixelShaderFile 	= "shaders/gl/tendrilP1_1.glsl";
   
   samplerNames[0] = "$refractMap";
   
   pixVersion = 1.1;
};

//-----------------------------------------------------------------------------
// Blank shader - to draw to z buffer before rendering rest of scene
//-----------------------------------------------------------------------------
new ShaderData( BlankShader )
{
   DXVertexShaderFile 	= "shaders/blankV.hlsl";
   
   OGLVertexShaderFile 	= "shaders/gl/blankV.glsl";
   
   pixVersion = 1.1;
};

//-----------------------------------------------------------------------------
// Outer Knot
//-----------------------------------------------------------------------------
new ShaderData( OuterKnotShader )
{
   DXVertexShaderFile 	= "shaders/outerKnotV.hlsl";
   DXPixelShaderFile 	= "shaders/outerKnotP.hlsl";
   
   OGLVertexShaderFile  = "shaders/gl/outerKnotV.glsl";
   OGLPixelShaderFile   = "shaders/gl/outerKnotP.glsl";
   
   samplerNames[0] = "$baseTex";
   samplerNames[1] = "$bumpMap";
   samplerNames[2] = "$cubeMap";
   
   pixVersion = 2.0;
};

new ShaderData( OuterKnotShader1_1 )
{
   DXVertexShaderFile 	= "shaders/outerKnotV1_1.hlsl";
   DXPixelShaderFile 	= "shaders/outerKnotP1_1.hlsl";
   
   OGLVertexShaderFile 	= "shaders/gl/outerKnotV1_1.glsl";
   OGLPixelShaderFile 	= "shaders/gl/outerKnotP1_1.glsl";

   samplerNames[0] = "$baseTex";
   samplerNames[1] = "$bumpMap";
   samplerNames[2] = "$cubeMap";
   
   pixVersion = 1.4;
};

//-----------------------------------------------------------------------------
// Waves
//-----------------------------------------------------------------------------
new ShaderData( Waves )
{
   DXVertexShaderFile 	= "shaders/wavesV.hlsl";
   DXPixelShaderFile 	= "shaders/wavesP.hlsl";
   
   OGLVertexShaderFile  = "shaders/gl/wavesV.glsl";
   OGLPixelShaderFile   = "shaders/gl/wavesP.glsl";
   
   samplerNames[0] = "$diffMap";
   samplerNames[1] = "$cubeMap";
   samplerNames[2] = "$bumpMap";
   
   pixVersion = 2.0;
};

//-----------------------------------------------------------------------------
// fxFoliageReplicator
//-----------------------------------------------------------------------------
new ShaderData( fxFoliageReplicatorShader )
{
   DXVertexShaderFile 	= "shaders/fxFoliageReplicatorV.hlsl";
   DXPixelShaderFile 	= "shaders/fxFoliageReplicatorP.hlsl";
   
   OGLVertexShaderFile  = "shaders/gl/fxFoliageReplicatorV.glsl";
   OGLPixelShaderFile   = "shaders/gl/fxFoliageReplicatorP.glsl";

   samplerNames[0] = "$diffuseMap";
   samplerNames[1] = "$alphaMap";
   
   pixVersion = 1.4;
};

//-----------------------------------------------------------------------------
// Terrain Shaders
//-----------------------------------------------------------------------------

new ShaderData( AtlasShader2 )
{
   DXVertexShaderFile   = "shaders/atlas/atlasSurfaceV2.hlsl";
   DXPixelShaderFile    = "shaders/atlas/atlasSurfaceP2.hlsl";
   
   OGLVertexShaderFile  = "shaders/atlas/gl/atlasSurfaceV2.glsl";
   OGLPixelShaderFile   = "shaders/atlas/gl/atlasSurfaceP2.glsl";
   
   samplerNames[0] = "$diffuseMap0";
   samplerNames[1] = "$diffuseMap1";
   
   pixVersion = 1.1;
};

new ShaderData( AtlasShader3 )
{
   DXVertexShaderFile   = "shaders/atlas/atlasSurfaceV3.hlsl";
   DXPixelShaderFile    = "shaders/atlas/atlasSurfaceP3.hlsl";
      
   OGLVertexShaderFile  = "shaders/atlas/gl/atlasSurfaceV3.glsl";
   OGLPixelShaderFile   = "shaders/atlas/gl/atlasSurfaceP3.glsl";
   
   samplerNames[0] = "$diffuseMap0";
   samplerNames[1] = "$diffuseMap1";
   samplerNames[2] = "$diffuseMap2";
   
   pixVersion = 1.1;
};

new ShaderData( AtlasShader4 )
{
   DXVertexShaderFile   = "shaders/atlas/atlasSurfaceV4.hlsl";
   DXPixelShaderFile    = "shaders/atlas/atlasSurfaceP4.hlsl";
      
   OGLVertexShaderFile  = "shaders/atlas/gl/atlasSurfaceV4.glsl";
   OGLPixelShaderFile   = "shaders/atlas/gl/atlasSurfaceP4.glsl";
   
   samplerNames[0] = "$diffuseMap0";
   samplerNames[1] = "$diffuseMap1";
   samplerNames[2] = "$diffuseMap2";
   samplerNames[3] = "$diffuseMap3";
   
   pixVersion = 1.1;
};

new ShaderData( AtlasShaderFog )
{
   DXVertexShaderFile   = "shaders/atlas/atlasFogV.hlsl";
   DXPixelShaderFile    = "shaders/atlas/atlasFogP.hlsl";
   
   OGLVertexShaderFile  = "shaders/atlas/gl/atlasFogV.glsl";
   OGLPixelShaderFile   = "shaders/atlas/gl/atlasFogP.glsl";
   
   samplerNames[0] = "$fogMap";
   
   pixVersion = 1.1;
};

new ShaderData( AtlasShaderDetail )
{
   DXVertexShaderFile   = "shaders/atlas/atlasDetailV.hlsl";
   DXPixelShaderFile    = "shaders/atlas/atlasDetailP.hlsl";
   
   OGLVertexShaderFile  = "shaders/atlas/gl/atlasDetailV.glsl";
   OGLPixelShaderFile   = "shaders/atlas/gl/atlasDetailP.glsl";
   
   samplerNames[0] = "$detMap";
   
   pixVersion = 1.4;
};

new ShaderData( AtlasBlender20Shader )
{
   DXVertexShaderFile   = "shaders/atlas/atlasBlenderPS20V.hlsl";
   DXPixelShaderFile    = "shaders/atlas/atlasBlenderPS20P.hlsl";
   
   OGLVertexShaderFile  = "shaders/atlas/gl/atlasBlenderV.glsl";
   OGLPixelShaderFile   = "shaders/atlas/gl/atlasBlenderP.glsl";
   
   samplerNames[0] = "$opacity";
   samplerNames[1] = "$lightMap";
   samplerNames[2] = "$tex1";
   samplerNames[3] = "$tex2";
   samplerNames[4] = "$tex3";
   samplerNames[5] = "$tex4";
   
   pixVersion = 2.0;
};

new ShaderData( AtlasBlender20ShaderLM1 : AtlasBlender20Shader )
{
   DXPixelShaderFile    = "shaders/atlas/atlasBlenderPS20PLM1.hlsl";
   OGLPixelShaderFile   = "shaders/atlas/gl/atlasBlenderPLM1.glsl";
};

new ShaderData( AtlasBlender11AShader )
{
   DXVertexShaderFile     = "shaders/atlas/atlasBlenderPS11VA.hlsl";
   DXPixelShaderFile      = "shaders/atlas/atlasBlenderPS11PA.hlsl";
   
   OGLVertexShaderFile    = "shaders/atlas/gl/atlasBlenderPS11VA.glsl";
   OGLPixelShaderFile     = "shaders/atlas/gl/atlasBlenderPS11PA.glsl";
	
	samplerNames[0] = "$opacity";
   samplerNames[1] = "$lightMap";
   samplerNames[2] = "$tex1";
   samplerNames[3] = "$tex2";

   pixVersion = 1.1;
};

new ShaderData( AtlasBlender11AShaderLM1 : AtlasBlender11AShader )
{
   DXPixelShaderFile      = "shaders/atlas/atlasBlenderPS11PALM1.hlsl";
   OGLPixelShaderFile     = "shaders/atlas/gl/atlasBlenderPS11PALM1.glsl";
};

new ShaderData( AtlasBlender11BShader )
{
   DXVertexShaderFile     = "shaders/atlas/atlasBlenderPS11VB.hlsl";
   DXPixelShaderFile      = "shaders/atlas/atlasBlenderPS11PB.hlsl";
   
   OGLVertexShaderFile    = "shaders/atlas/gl/atlasBlenderPS11VB.glsl";
   OGLPixelShaderFile     = "shaders/atlas/gl/atlasBlenderPS11PB.glsl";
	
	samplerNames[0] = "$opacity";
   samplerNames[1] = "$lightMap";
   samplerNames[2] = "$tex3";
   samplerNames[3] = "$tex4";

   pixVersion = 1.1;
};

new ShaderData( AtlasBlender11BShaderLM1 : AtlasBlender11BShader )
{
   DXPixelShaderFile      = "shaders/atlas/atlasBlenderPS11PBLM1.hlsl";
   OGLPixelShaderFile     = "shaders/atlas/gl/atlasBlenderPS11PBLM1.glsl";
};
