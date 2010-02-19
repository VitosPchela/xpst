//-----------------------------------------------------------------------------
// Torque Builder
// Copyright (C) GarageGames.com, Inc.
//-----------------------------------------------------------------------------

$InGuiEditor = false;

function GuiEdit(%val)
{

   if (Canvas.isFullscreen())
   {
      MessageBoxOKOld("Windowed Mode Required", "Please switch to windowed mode to access the GUI Editor.");
      return;
   }

   if(%val != 0)
      return;

   if (!$InGuiEditor)
   {
      
      if( !isObject( GuiEditCanvas ) )
         new GuiControl( GuiEditCanvas );

      GuiEditorOpen(Canvas.getContent());
   
      $InGuiEditor = true;
   }
   else
   {
      GuiEditCanvas.quit();
   }

}

function GuiEditCanvas::onAdd( %this )
{
   // %this.setWindowTitle("Torque Gui Editor");

   %this.onCreateMenu();
         
}

function GuiEditCanvas::onRemove( %this )
{
   if( isObject( GuiEditorGui.menuGroup() ) )
      GuiEditorGui.delete();

   // cleanup
   %this.onDestroyMenu();
}

function GuiEditCanvas::onCreateMenu(%this)
{
   
   if(isObject(%this.menuBar))
      return;
   
   //set up %cmdctrl variable so that it matches OS standards
   %cmdCtrl = $platform $= "macos" ? "Cmd" : "Ctrl";
   
   // Menu bar
   %this.menuBar = new MenuBar()
   {
      dynamicItemInsertPos = 3;
      
      new PopupMenu()
      {
         superClass = "MenuBuilder";
         barTitle = "File";
         internalName = "FileMenu";
         
         item[0] = "New Gui..." TAB %cmdCtrl SPC "N" TAB %this @ ".create();";
         item[1] = "Open From File..." TAB %cmdCtrl SPC "O" TAB %this @ ".open();";
         item[2] = "-";
         item[3] = "Revert To File" TAB %cmdCtrl SPC "R" TAB  %this @ ".revert();";
         item[4] = "Save To File..." TAB %cmdCtrl SPC "S" TAB %this @ ".save();";
         item[5] = "-";
         item[5] = "Close Gui" TAB %cmdCtrl SPC "W" TAB %this @ ".close();";
         item[5] = "-";
         item[6] = "Quit" TAB "F10" TAB %this @ ".quit();";
      };

      new PopupMenu()
      {
         superClass = "MenuBuilder";
         barTitle = "Edit";
         internalName = "EditMenu";
         
         item[0] = "Undo" TAB %cmdCtrl SPC "Z" TAB "GuiEditor.undo();";
         item[1] = "Redo" TAB %cmdCtrl @ "-shift Z" TAB "GuiEditor.redo();";
         item[2] = "-";
         item[3] = "Cut" TAB %cmdCtrl SPC "X" TAB "GuiEditor.saveSelection($Gui::clipboardFile); GuiEditor.deleteSelection();";
         item[4] = "Copy" TAB %cmdCtrl SPC "C" TAB "GuiEditor.saveSelection($Gui::clipboardFile);";
         item[5] = "Paste" TAB %cmdCtrl SPC "V" TAB "GuiEditor.loadSelection($Gui::clipboardFile);";
         item[6] = "Select All" TAB %cmdCtrl SPC "A" TAB "GuiEditor.selectAll();";
         item[7] = "-";
         item[8] = "Preferences" TAB %cmdCtrl SPC "," TAB "GuiEditor.showPrefsDialog();";
      };

      new PopupMenu()
      {
         superClass = "MenuBuilder";
         barTitle = "Layout";
         internalName = "LayoutMenu";
         
         item[0] = "Align Left" TAB %cmdCtrl SPC "L" TAB "GuiEditor.Justify(0);";
         item[1] = "Align Right" TAB %cmdCtrl SPC "R" TAB "GuiEditor.Justify(2);";
         item[2] = "Align Top" TAB %cmdCtrl SPC "T" TAB "GuiEditor.Justify(3);";
         item[3] = "Align Bottom" TAB %cmdCtrl SPC "B" TAB "GuiEditor.Justify(4);";
         item[4] = "-";
         item[5] = "Center Horizontally" TAB "" TAB "GuiEditor.Justify(1);";
         item[6] = "Space Vertically" TAB "" TAB "GuiEditor.Justify(5);";
         item[7] = "Space Horizontally" TAB "" TAB "GuiEditor.Justify(6);";
         item[8] = "-";
         item[9] = "Bring to Front" TAB "" TAB "GuiEditor.BringToFront();";
         item[10] = "Send to Back" TAB "" TAB "GuiEditor.PushToBack();";
         item[11] = "Lock Selection" TAB "" TAB "GuiEditorTreeView.lockSelection(true);";
         item[12] = "Unlock Selection" TAB "" TAB "GuiEditorTreeView.lockSelection(false);";
      };
      
      new PopupMenu()
      {
         superClass = "MenuBuilder";
         barTitle = "Move";
         internalName = "MoveMenu";
            
         item[0] = "Nudge Left" TAB "Left" TAB "GuiEditor.moveSelection(-1,0);";
         item[1] = "Nudge Right" TAB "Right" TAB "GuiEditor.moveSelection(1,0);";
         item[2] = "Nudge Up" TAB "Up" TAB "GuiEditor.moveSelection(0,-1);";
         item[3] = "Nudge Down" TAB "Down" TAB "GuiEditor.moveSelection(0,1);";
         item[4] = "-";
         item[5] = "Big Nudge Left" TAB "Shift Left" TAB "GuiEditor.moveSelection(-$pref::guiEditor::snap2gridsize * 2,0);";
         item[6] = "Big Nudge Right" TAB "Shift Right" TAB "GuiEditor.moveSelection($pref::guiEditor::snap2gridsize * 2,0);";
         item[7] = "Big Nudge Up" TAB "Shift Up" TAB "GuiEditor.moveSelection(0,-$pref::guiEditor::snap2gridsize * 2);";
         item[8] = "Big Nudge Down" TAB "Shift Down" TAB "GuiEditor.moveSelection(0,$pref::guiEditor::snap2gridsize * 2);";
      };
   };
   %this.menuBar.attachToCanvas( Canvas, 0 );
}

// Called before onSleep when the canvas content is changed
function GuiEditCanvas::onDestroyMenu(%this)
{
   if( !isObject( %this.menuBar ) )
      return;

   // Destroy menus      
   while( %this.menuBar.getCount() != 0 )
      %this.menuBar.getObject(0).delete();
   
   if (isObject(%this.menubar))
   {
      %this.menuBar.removeFromCanvas( Canvas );
      %this.menuBar.delete();
   }
}

//
// Menu Operations
//
function GuiEditCanvas::create( %this )
{
   GuiEditorStartCreate();
}

function GuiEditCanvas::open( %this )
{
   %openFileName = GuiBuilder::getOpenName();
   if( %openFileName $= "" )
      return;   

   // Make sure the file is valid.
   if ((!isFile(%openFileName)) && (!isFile(%openFileName @ ".dso")))
      return;
   
   // Load up the level.
   exec(%openFileName);
   
   // The level file should have contained a scenegraph, which should now be in the instant
   // group. And, it should be the only thing in the group.
   if (!isObject(%guiContent))
   {
      MessageBox("Torque Builder", "You have loaded a Gui file that was created before this version.  It has been loaded but you must open it manually from the content list dropdown", "Ok", "Information" );   
      return 0;
   }
  
   GuiEditorOpen(%guiContent);   
}
function GuiEditCanvas::save( %this )
{
   %currentObject = GuiEditorContent.getObject( 0 );
   
   if( %currentObject == -1 )
      return;
   
   if( %currentObject.getName() !$= "" )
      %name =  %currentObject.getName() @ ".gui";
   else
      %name = "Untitled.gui";
      
   %currentFile = %currentObject.getScriptFile();
   if( %currentFile $= "")
   {
      if( $Pref::GuiEditor::LastPath !$= "" )
         %currentFile = $Pref::GuiEditor::LastPath @ %name;
      else
         %currentFile = expandFileName( %name );
   }
   else
      %currentFile = expandFileName(%currentFile);
   
   // get the filename
   %filename = GuiBuilder::getSaveName(%currentFile);
   
   if(%filename $= "")
      return;
      
   // Save the Gui
   if( isWriteableFileName( %filename ) )
   {
      //
      // Extract any existent TorqueScript before writing out to disk
      //
      %fileObject = new FileObject();
      %fileObject.openForRead( %filename );      
      %skipLines = true;
      %beforeObject = true;
      // %var++ does not post-increment %var, in torquescript, it pre-increments it,
      // because ++%var is illegal. 
      %lines = -1;
      %beforeLines = -1;
      %skipLines = false;
      while( !%fileObject.isEOF() )
      {
         %line = %fileObject.readLine();
         if( %line $= "//--- OBJECT WRITE BEGIN ---" )
            %skipLines = true;
         else if( %line $= "//--- OBJECT WRITE END ---" )
         {
            %skipLines = false;
            %beforeObject = false;
         }
         else if( %skipLines == false )
         {
            if(%beforeObject)
               %beforeNewFileLines[ %beforeLines++ ] = %line;
            else
               %newFileLines[ %lines++ ] = %line;
         }
      }      
      %fileObject.close();
      %fileObject.delete();
     
      %fo = new FileObject();
      %fo.openForWrite(%filename);
      
      // Write out the captured TorqueScript that was before the object before the object
      for( %i = 0; %i <= %beforeLines; %i++)
         %fo.writeLine( %beforeNewFileLines[ %i ] );
         
      %fo.writeLine("//--- OBJECT WRITE BEGIN ---");
      %fo.writeObject(%currentObject, "%guiContent = ");
      %fo.writeLine("//--- OBJECT WRITE END ---");
      
      // Write out captured TorqueScript below Gui object
      for( %i = 0; %i <= %lines; %i++ )
         %fo.writeLine( %newFileLines[ %i ] );
               
      %fo.close();
      %fo.delete();
      
   }
   else
      MessageBox("Torque Game Builder", "There was an error writing to file '" @ %currentFile @ "'. The file may be read-only.", "Ok", "Error" );   
}
function GuiEditCanvas::revert( %this )
{
}
function GuiEditCanvas::close( %this )
{
}

function GuiEditCanvas::onWindowClose(%this)
{
   %this.quit();
}

function GuiEditCanvas::quit( %this )
{
   %this.close();
   GuiGroup.add(GuiEditorGui);
   // we must not delete a window while in its event handler, or we foul the event dispatch mechanism
   %this.schedule(10, delete);
   
   Canvas.setContent(GuiEditor.lastContent);
   $InGuiEditor = false;
}