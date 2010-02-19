//---------------------------------------------------------------------------------------------
// Torque Game Builder
// Copyright (C) GarageGames.com, Inc.
//---------------------------------------------------------------------------------------------

$Gui::fontCacheDirectory = getPrefsPath("fonts");

// If we got back no prefs path modification
if( $Gui::fontCacheDirectory $= "fonts" )
{
   $Gui::fontCacheDirectory = expandFilename( "~/data/fonts" );
}

//---------------------------------------------------------------------------------------------
// GuiDefaultProfile is a special profile that all other profiles inherit defaults from. It
// must exist.
//---------------------------------------------------------------------------------------------
if(!isObject(GuiDefaultProfile)) new GuiControlProfile (GuiDefaultProfile)
{
   tab = false;
   canKeyFocus = false;
   hasBitmapArray = false;
   mouseOverSelected = false;

   // fill color
   opaque = false;
   fillColor = "127 136 153";
   fillColorHL = "197 202 211";
   fillColorNA = "144 154 171";

   // border color
   border = 0;
   borderColor   = "0 0 0"; 
   borderColorHL = "197 202 211";
   borderColorNA = "91 101 119";
   bevelColorHL = "255 255 255";
   bevelColorLL = "0 0 0";

   // font
   fontType = "Arial";
   fontSize = 14;
   fontCharset = ANSI;

   fontColor = "0 0 0";
   fontColorHL = "23 32 47";
   fontColorNA = "0 0 0";
   fontColorSEL= "126 137 155";

   // bitmap information
   bitmap = "";
   bitmapBase = "";
   textOffset = "0 0";

   // used by guiTextControl
   modal = true;
   justify = "left";
   autoSizeWidth = false;
   autoSizeHeight = false;
   returnTab = false;
   numbersOnly = false;
   cursorColor = "0 0 0 255";

   // sounds
   soundButtonDown = "";
   soundButtonOver = "";
};

if(!isObject(GuiSolidDefaultProfile)) new GuiControlProfile (GuiSolidDefaultProfile)
{
   opaque = true;
   border = true;
};

if(!isObject(GuiTransparentProfile)) new GuiControlProfile (GuiTransparentProfile)
{
   opaque = false;
   border = false;
};


if(!isObject(GuiToolTipProfile)) new GuiControlProfile (GuiToolTipProfile)
{
   // fill color
   fillColor = "239 237 222";

   // border color
   borderColor   = "138 134 122";

   // font
   fontType = "Arial";
   fontSize = 14;
   fontColor = "0 0 0";

};

if(!isObject(GuiModelessDialogProfile)) new GuiControlProfile("GuiModelessDialogProfile")
{
   modal = false;
};

if(!isObject(GuiFrameSetProfile)) new GuiControlProfile (GuiFrameSetProfile)
{
   fillColor = GuiDefaultProfile.fillColorNA;
   borderColor   = GuiDefaultProfile.borderColorNA;
   opaque = true;
   border = true;
};


if(!isObject(GuiWindowProfile)) new GuiControlProfile (GuiWindowProfile)
{
   opaque = true;
   border = 2;
   fillColor = "145 154 171";
   fillColorHL = "221 202 173";
   fillColorNA = "221 202 173";
   fontColor = "255 255 255";
   fontColorHL = "255 255 255";
   text = "untitled";
   bitmap = "./images/window";
   textOffset = "6 6";
   hasBitmapArray = true;
   justify = "center";
};

if(!isObject(GuiContentProfile)) new GuiControlProfile (GuiContentProfile)
{
   opaque = true;
   fillColor = "255 255 255";
};

if(!isObject(GuiBlackContentProfile)) new GuiControlProfile (GuiBlackContentProfile)
{
   opaque = true;
   fillColor = "0 0 0";
};

if(!isObject(GuiInputCtrlProfile)) new GuiControlProfile( GuiInputCtrlProfile )
{
   tab = true;
   canKeyFocus = true;
};

if(!isObject(GuiTextProfile)) new GuiControlProfile (GuiTextProfile)
{
   fontColor = "0 0 0";
};

if(!isObject(GuiAutoSizeTextProfile)) new GuiControlProfile (GuiAutoSizeTextProfile)
{
   fontColor = "0 0 0";
   autoSizeWidth = true;
   autoSizeHeight = true;   
};

if(!isObject(GuiTextRightProfile)) new GuiControlProfile (GuiTextRightProfile : GuiTextProfile)
{
   justify = "right";
};

if(!isObject(GuiMediumTextProfile)) new GuiControlProfile (GuiMediumTextProfile : GuiTextProfile)
{
   fontSize = 24;
};

if(!isObject(GuiBigTextProfile)) new GuiControlProfile (GuiBigTextProfile : GuiTextProfile)
{
   fontSize = 36;
};

if(!isObject(GuiMLTextProfile)) new GuiControlProfile ("GuiMLTextProfile")
{
   fontColorLink = "255 96 96";
   fontColorLinkHL = "0 0 255";
   autoSizeWidth = true;
   autoSizeHeight = true;  
   border = false;
};

if(!isObject(GuiTextArrayProfile)) new GuiControlProfile (GuiTextArrayProfile : GuiTextProfile)
{
   fontColorHL = "32 100 100";
   fillColorHL = "200 200 200";
   border = false;
};

if(!isObject(GuiTextListProfile)) new GuiControlProfile (GuiTextListProfile : GuiTextProfile) 
{
   tab = true;
   canKeyFocus = true;
};

if(!isObject(GuiTextEditProfile)) new GuiControlProfile (GuiTextEditProfile)
{
   opaque = true;
   fillColor = "255 255 255";
   fillColorHL = "128 128 128";
   border = -2;
   bitmap = "./images/textEdit";
   borderColor = "40 40 40 100";
   fontColor = "0 0 0";
   fontColorHL = "255 255 255";
   fontColorNA = "128 128 128";
   textOffset = "4 2";
   autoSizeWidth = false;
   autoSizeHeight = true;
   tab = true;
   canKeyFocus = true;
   
};

if(!isObject(GuiTextEditNumericProfile)) new GuiControlProfile (GuiTextEditNumericProfile : GuiTextEditProfile)
{
   numbersOnly = true;
};

if(!isObject(GuiProgressProfile)) new GuiControlProfile ("GuiProgressProfile")
{
   opaque = false;
   fillColor = "44 152 162 100";
   border = true;
   borderColor   = "78 88 120";
};

if(!isObject(GuiProgressTextProfile)) new GuiControlProfile ("GuiProgressTextProfile")
{
   fontColor = "0 0 0";
   justify = "center";
};

if(!isObject(GuiButtonProfile)) new GuiControlProfile (GuiButtonProfile)
{
   opaque = true;
   border = true;
   fontColor = "0 0 0";
   fontColorHL = "32 100 100";
   fixedExtent = true;
   justify = "center";
   canKeyFocus = false;
   hasBitmapArray = false;
};

if(!isObject(GuiCheckBoxProfile)) new GuiControlProfile (GuiCheckBoxProfile)
{
   opaque = false;
   fillColor = "232 232 232";
   border = false;
   borderColor = "0 0 0";
   fontSize = 14;
   fontColor = "0 0 0";
   fontColorHL = "32 100 100";
   fixedExtent = true;
   justify = "left";
   bitmap = "./images/checkBox";
   hasBitmapArray = true;
};

if(!isObject(GuiRadioProfile)) new GuiControlProfile (GuiRadioProfile)
{
   fontSize = 14;
   fillColor = "232 232 232";
   fontColorHL = "32 100 100";
   fixedExtent = true;
   bitmap = "./images/radioButton";
   hasBitmapArray = true;
};

if(!isObject(GuiScrollProfile)) new GuiControlProfile (GuiScrollProfile)
{
   opaque = true;
   fillcolor = "255 255 255";
   borderColor = GuiDefaultProfile.borderColor;
   border = true;
   bitmap = "./images/scrollBar";
   hasBitmapArray = true;
};

if(!isObject(GuiTransparentScrollProfile)) new GuiControlProfile (GuiTransparentScrollProfile)
{
   opaque = false;
   fillColor = "255 255 255";
   border = false;
   borderThickness = 2;
   borderColor = "0 0 0";
   bitmap = "./images/scrollBar";
   hasBitmapArray = true;
};
 
if(!isObject(GuiSliderProfile)) new GuiControlProfile (GuiSliderProfile)
{
   bitmap = "./images/slider";
};

if(!isObject(GuiPaneProfile)) new GuiControlProfile(GuiPaneProfile)
{
   bitmap = "./images/popupMenu";
   hasBitmapArray = true;
};



if(!isObject(GuiPopupMenuItemBorder)) new GuiControlProfile ( GuiPopupMenuItemBorder : GuiButtonProfile )
{
   borderThickness = 1;
   borderColor = "51 51 53 200";
   borderColorHL = "51 51 53 200";

};
if(!isObject(GuiPopUpMenuDefault)) new GuiControlProfile (GuiPopUpMenuDefault : GuiDefaultProfile )
{
   opaque = true;
   mouseOverSelected = true;
   textOffset = "3 3";
   border = 1;
   borderThickness = 0;
   fixedExtent = true;
   bitmap = "./images/scrollbar";
   hasBitmapArray = true;
   profileForChildren = GuiPopupMenuItemBorder;
   fillColor = "255 255 255 100";
   fillColorHL = "100 100 100";
   fontColorHL = "220 220 220";
   borderColor = "100 100 108";
};

if(!isObject(GuiPopupBackgroundProfile)) new GuiControlProfile (GuiPopupBackgroundProfile)
{
   modal = true;
};


if(!isObject(GuiPopUpMenuProfile)) new GuiControlProfile (GuiPopUpMenuProfile : GuiPopUpMenuDefault)
{
   textOffset         = "6 3";
   bitmap             = "./images/dropDown";
   hasBitmapArray     = true;
   border             = -3;
   profileForChildren = GuiPopUpMenuDefault;
};

if(!isObject(GuiPopUpMenuEditProfile)) new GuiControlProfile (GuiPopUpMenuEditProfile : GuiPopUpMenuDefault)
{
   textOffset         = "6 3";
   canKeyFocus        = true;
   bitmap             = "./images/dropDown";
   hasBitmapArray     = true;
   border             = -3;
   profileForChildren = GuiPopUpMenuDefault;
};

if(!isObject(GuiListBoxProfile)) new GuiControlProfile (GuiListBoxProfile)
{
   tab = true;
   canKeyFocus = true;
};

if(!isObject(GuiTabBookProfile)) new GuiControlProfile (GuiTabBookProfile)
{
   fillColorHL = "64 150 150";
   fillColorNA = "150 150 150";
   fontColor = "30 30 30";
   fontColorHL = "32 100 100";
   fontColorNA = "0 0 0";
   fontType = "Arial Bold";
   fontSize = 14;
   justify = "center";
   bitmap = "./images/tab";
   tabWidth = 64;
   tabHeight = 24;
   tabPosition = "Top";
   tabRotation = "Horizontal";
   textOffset = "0 -3";
   tab = true;
   cankeyfocus = true;
   border = false;
   opaque = false;
};
if(!isObject(GuiTabPageProfile)) new GuiControlProfile (GuiTabPageProfile : GuiDefaultProfile )
{
   opaque = false;
};

if(!isObject(GuiMenuBarProfile)) new GuiControlProfile (GuiMenuBarProfile)
{
   fontType = "Arial";
   fontSize = 15;
   opaque = true;
   fillColor = "239 237 222";
   fillColorHL = "102 153 204";
   borderColor = "138 134 122";
   borderColorHL = "0 51 153";
   border = 5;
   fontColor = "0 0 0";
   fontColorHL = "255 255 255";
   fontColorNA = "128 128 128";
   fixedExtent = true;
   justify = "center";
   canKeyFocus = false;
   mouseOverSelected = true;
   bitmap = "./images/menu";
   hasBitmapArray = true;
};

if(!isObject(GuiConsoleProfile)) new GuiControlProfile (GuiConsoleProfile)
{
   fontType = ($platform $= "macos") ? "Monaco" : "Lucida Console";
   fontSize = ($platform $= "macos") ? 13 : 12;
    fontColor = "255 255 255";
    fontColorHL = "155 155 155";
    fontColorNA = "255 0 0";
    fontColors[6] = "100 100 100";
    fontColors[7] = "100 100 0";
    fontColors[8] = "0 0 100";
    fontColors[9] = "0 100 0";
};

if(!isObject(GuiConsoleTextEditProfile)) new GuiControlProfile (GuiConsoleTextEditProfile : GuiTextEditProfile)
{
   fontType = ($platform $= "macos") ? "Monaco" : "Lucida Console";
   fontSize = ($platform $= "macos") ? 13 : 12;
};

if (!isObject(GuiTreeViewProfile)) new GuiControlProfile (GuiTreeViewProfile)
{  
   bitmap            = "./images/treeView";
   autoSizeHeight    = true;
   canKeyFocus       = true;
   
   fillColor = GuiDefaultProfile.fillColor;
   fillColorHL = GuiDefaultProfile.fillColorHL;
   fillColorNA = GuiDefaultProfile.fillColorNA;

   fontColor = GuiDefaultProfile.fontColor;
   fontColorHL = GuiDefaultProfile.fontColorHL;
   fontColorNA = GuiDefaultProfile.fontColorNA;
   fontColorSEL= GuiDefaultProfile.fontColorSEL;

   fontSize = 14;
   
   opaque = false;
   border = false;
};

if(!isObject(GuiSimpleTreeProfile)) new GuiControlProfile (GuiSimpleTreeProfile : GuiTreeViewProfile)
{
   opaque = true;
   fillColor = "255 255 255 255";
   border = true;
};

//*** DAW:
if(!isObject(GuiText24Profile)) new GuiControlProfile (GuiText24Profile : GuiTextProfile)
{
   fontSize = 24;
};

if(!isObject(GuiRSSFeedMLTextProfile)) new GuiControlProfile ("GuiRSSFeedMLTextProfile")
{
   fontColorLink = "55 55 255";
   fontColorLinkHL = "255 55 55";
};

if(!isObject(ConsoleScrollProfile)) new GuiControlProfile( ConsoleScrollProfile : GuiScrollProfile )
{
	opaque = true;
	fillColor = "0 0 0 120";
	border = 3;
	borderThickness = 0;
	borderColor = "0 0 0";
};

if(!isObject(GuiTextPadProfile)) new GuiControlProfile( GuiTextPadProfile )
{
   fontType = ($platform $= "macos") ? "Monaco" : "Lucida Console";
   fontSize = ($platform $= "macos") ? 13 : 12;
   tab = true;
   canKeyFocus = true;
   
   // Deviate from the Default
   opaque=true;  
   fillColor = "255 255 255";
   
   border = 0;
};

if(!isObject(GuiTransparentProfileModeless)) new GuiControlProfile (GuiTransparentProfileModeless : GuiTransparentProfile) 
{
   modal = false;
};

if(!isObject(GuiParticleListBoxProfile)) new GuiControlProfile (GuiParitcleListBoxProfile : GuiListBoxProfile)
{
   tab = true;
   canKeyFocus = true;
   fontColor = "0 0 0";
   fontColorHL = "25 25 25 220";
   fontColorNA = "128 128 128";
   fontColor = "0 0 0 150";
};

if(!isObject(GuiFormProfile)) new GuiControlProfile(GuiFormProfile : GuiTextProfile )
{
   opaque = false;
   border = 5;
   
   bitmap = "./images/form";
   hasBitmapArray = true;

   justify = "center";
   
   profileForChildren = GuiButtonProfile;
   
   // border color
   borderColor   = "153 153 153"; 
   borderColorHL = "230 230 230";
   borderColorNA = "126 79 37";
};

if (!isObject(GuiBreadcrumbsMenuProfile)) new GuiControlProfile(GuiBreadcrumbsMenuProfile)
{
   fontColor = "0 0 0";
   fontType = "Arial";
   fontSize = 14;
   
   bitmap = "./images/breadcrumbs";
   hasBitmapArray = true;
};

