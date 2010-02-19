//-----------------------------------------------------------------------------
// Torque Game Engine Advanced
// Copyright (c) 2002 GarageGames.Com
//-----------------------------------------------------------------------------

if (isObject(GuiDefaultProfile)) GuiDefaultProfile.delete();
new GuiControlProfile (GuiDefaultProfile)
{
   tab = false;
   canKeyFocus = false;
   hasBitmapArray = false;
   mouseOverSelected = false;

   // fill color
   opaque = false;
   fillColor = "201 182 153";
   fillColorHL = "221 202 173";
   fillColorNA = "221 202 173";

   // border color
   border = false;
   borderColor   = "0 0 0"; 
   borderColorHL = "179 134 94";
   borderColorNA = "126 79 37";
   
   // bevel color
   bevelColorHL = "255 255 255";
   bevelColorLL = "0 0 0";

   // font
   fontType = "Arial";
   fontSize = 14;
   fontCharset = ANSI;

   fontColor = "0 0 0";
   fontColorHL = "32 100 100";
   fontColorNA = "0 0 0";
   fontColorSEL= "200 200 200";

   // bitmap information
   bitmap = "./demoWindow";
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

if (isObject(GuiWindowProfile)) GuiWindowProfile.delete();
new GuiControlProfile (GuiWindowProfile)
{
   opaque = true;
   border = 2;
   fillColor = "201 182 153";
   fillColorHL = "221 202 173";
   fillColorNA = "221 202 173";
   fontColor = "255 255 255";
   fontColorHL = "255 255 255";
   text = "GuiWindowCtrl test";
   bitmap = "./demoWindow";
   textOffset = "6 6";
   hasBitmapArray = true;
   justify = "center";
};

if (isObject(GuiScrollProfile)) GuiScrollProfile.delete();
new GuiControlProfile (GuiScrollProfile)
{
   opaque = true;
   fillColor = "255 255 255";
   border = 3;
   borderThickness = 2;
   borderColor = "0 0 0";
   bitmap = "./demoScroll";
   hasBitmapArray = true;
};

if (isObject(GuiCheckBoxProfile)) GuiCheckBoxProfile.delete();
new GuiControlProfile (GuiCheckBoxProfile)
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
   bitmap = "./demoCheck";
   hasBitmapArray = true;
};

if (isObject(GuiRadioProfile)) GuiRadioProfile.delete();
new GuiControlProfile (GuiRadioProfile)
{
   fontSize = 14;
   fillColor = "232 232 232";
   fontColorHL = "32 100 100";
   fixedExtent = true;
   bitmap = "./demoRadio";
   hasBitmapArray = true;
};

