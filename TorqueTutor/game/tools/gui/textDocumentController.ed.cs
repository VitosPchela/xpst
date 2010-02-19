//---------------------------------------------------------------------------------------------
// Torque Game Builder
// Copyright (C) GarageGames.com, Inc.
//---------------------------------------------------------------------------------------------

$Pref::TextEditor::FileSpec = "Torque Script Files (*.cs)|*.cs|Torque Gui Files (*.gui)|*.gui|All Files (*.*)|*.*|";

/// TextDocumentController::getSaveName - Open a Native File dialog and retrieve the
///  location to save the current document.
/// @arg defaultFileName   The FileName to default in the field and to be selected when a path is opened
function TextDocumentController::getSaveName( %defaultFileName )
{
   // if we're editing a game, we want to default to the games dir.
   // if we're not, then we default to the tools directory or the base.
   if(isScriptPathExpando("^game"))
      %prefix = "^game";
   else if( isScriptPathExpando("^tools"))
      %prefix = "^tools";
   else
      %prefix = "";
      
   if( %defaultFileName $= "" )
      %defaultFileName = expandFilename(%prefix @ "/gui/untitled.gui");
      
   %dlg = new SaveFileDialog()
   {
      Filters           = $Pref::TextEditor::FileSpec;
      DefaultPath       = $Pref::TextEditor::LastPath;
      DefaultFile       = %defaultFileName;
      ChangePath        = false;
      OverwritePrompt   = true;
   };
         
   if(%dlg.Execute())
   {
      $Pref::TextEditor::LastPath = filePath( %dlg.FileName );
      %filename = %dlg.FileName;
   }
   else
      %filename = "";
      
   %dlg.delete();
   
   return %filename;
}

function TextDocumentController::getOpenName( %defaultFileName )
{
   if( %defaultFileName $= "" )
      %defaultFileName = expandFilename("^game/gui/untitled.gui");
   
   %dlg = new OpenFileDialog()
   {
      Filters        = $Pref::TextEditor::FileSpec;
      DefaultPath    = $Pref::TextEditor::LastPath;
      DefaultFile    = %defaultFileName;
      ChangePath     = false;
      MustExist      = true;
   };
         
   if(%dlg.Execute())
   {
      $Pref::TextEditor::LastPath = filePath( %dlg.FileName );
      %filename = %dlg.FileName;      
      %dlg.delete();
      return %filename;
   }
   
   %dlg.delete();
   return "";   
}
