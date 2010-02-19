//-----------------------------------------------------------------------------
// Anim flag settings - must match material.h
// These cannot be enumed through script becuase it cannot
// handle the "|" operation for combining them together
// ie. Scroll | Wave does not work.
//-----------------------------------------------------------------------------
$scroll = 1;
$rotate = 2;
$wave   = 4;
$scale  = 8;
$sequence = 16;


// Common stateblock definitions
new GFXSamplerStateData(SamplerClampLinear)
{
   textureColorOp = GFXTOPModulate;
   addressModeU = GFXAddressClamp;
   addressModeV = GFXAddressClamp;
   addressModeW = GFXAddressClamp;
   magFilter = GFXTextureFilterLinear;
   minFilter = GFXTextureFilterLinear;
   mipFilter = GFXTextureFilterLinear;
};

new GFXSamplerStateData(SamplerWrapLinear)
{
   textureColorOp = GFXTOPModulate;
   addressModeU = GFXTextureAddressWrap;
   addressModeV = GFXTextureAddressWrap;
   addressModeW = GFXTextureAddressWrap;
   magFilter = GFXTextureFilterLinear;
   minFilter = GFXTextureFilterLinear;
   mipFilter = GFXTextureFilterLinear;
};

//*****************************************************************************
// Data
//*****************************************************************************

new CubemapData( Sky_Day_Blur02 )
{
   cubeFace[0] = "~/data/water/sky_x_pos3";
   cubeFace[1] = "~/data/water/sky_x_neg3";
   cubeFace[2] = "~/data/water/sky_y_pos3";
   cubeFace[3] = "~/data/water/sky_y_neg3";
   cubeFace[4] = "~/data/water/sky_z_pos3";
   cubeFace[5] = "~/data/water/sky_z_neg";
};
