// Test code

function TextEditor()
{
   %canvas = new GuiCanvas(TextEditorCanvas);
   %canvas.cursorOn();
}

function TextEditorCanvas::onAdd( %this )
{
   
   %this.windowPrefix = "TextDocument - ";
   %this.dirtyFlag = "*";
   
   if(isObject(%this.textScroll))
      %this.textScroll.delete();
         
   %this.textScroll = new GuiScrollCtrl() {
      canSaveDynamicFields = "0";
      isContainer = "1";
      Profile = "GuiScrollProfile";
      HorizSizing = "width";
      VertSizing = "height";
      position = "0 0";
      Extent = "640 480";
      MinExtent = "8 2";
      canSave = "1";
      Visible = "1";
      hovertime = "1000";
      willFirstRespond = "0";
      hScrollBar = "dynamic";
      vScrollBar = "dynamic";
      constantThumbHeight = "0";
      childMargin = "1 1";
   };
   
   %this.textPad = new GuiTextPadCtrl() {
      canSaveDynamicFields = "0";
      internalName = "textpad";
      isContainer = "0";
      Profile = "GuiTextPadProfile";
      HorizSizing = "width";
      VertSizing = "height";
      position = "2 2";
      Extent = "640 480";
      MinExtent = "8 2";
      canSave = "1";
      Visible = "1";
      hovertime = "1000";
   };
   
   %this.textScroll.add( %this.textPad );
   %this.testDocument = new TextDocument();
   %this.testDocument.openFromFile("demo/client/scripts/client.cs");
   %this.textPad.setDocument( %this.testDocument );
   
   %this.schedule(1,createMenus);
   
   %this.setContent( %this.textScroll );
   
   %this.updateTitleBar();
}

function TextEditorCanvas::updateTitleBar( %this )
{
   %isDirty = false;
   %fileName = %this.testDocument.getFileName();
   if( %fileName $= "" )
      %filename = %this.untitleDocument;
   %filename = makeRelativePath(%filename, getMainDotCSDir());
   %this.setWindowTitle( %this.windowPrefix SPC %fileName SPC (%isDirty? %this.dirtyFlag : ""));   
}

function TextEditorCanvas::createMenus( %this ) 
{
   // create & clear the menu group
   if( isObject( %this.menuGroup ) )
      %this.menuGroup.delete();
   %this.menuGroup = new SimGroup();
      
   //set up %cmdctrl variable so that it matches OS standards
   %cmdCtrl = $platform $= "macos" ? "Cmd" : "Ctrl";

   %filemenu = new PopupMenu()
   {
      superClass = "MenuBuilder";
      barPosition = 0;
      barName = "File";
      Canvas = %this;
      
      item[0] = "New Document" TAB %cmdCtrl SPC "N" TAB %this @ ".create();";
      item[1] = "Open Document" TAB %cmdCtrl SPC "O" TAB %this @ ".open();";
      item[2] = "-";
      item[3] = "Revert Document" TAB %cmdCtrl SPC "R" TAB  %this @ ".revert();";
      item[4] = "Save Document" TAB %cmdCtrl SPC "S" TAB %this @ ".save();";
      item[5] = "-";
      item[6] = "Close Script Editor" TAB %cmdCtrl SPC "Q" TAB %this @ ".quit();";
   };

   // add menus to a group
   %this.menuGroup.add(%fileMenu);

   %filemenu.attachToMenuBar( %this );   
}

function TextEditorCanvas::create( %this )
{
   if( !isObject( %this.testDocument ) )
      return;
      
   %this.testDocument.createDocument();
   
   %this.setWindowTitle( %this.windowPrefix SPC %this.untitledDocument );
}
function TextEditorCanvas::open( %this )
{
   if( !isObject( %this.testDocument ) )
      return;
   
   %openFileName = TextDocumentController::getOpenName();
   if( %openFileName $= "" )
      return;   
         
   if(%this.testDocument.openFromFile( %openFileName ))
      %this.updateTitleBar();
}

function TextEditorCanvas::save( %this )
{
   if( !isObject( %this.testDocument ) )
      return;
      
   // get the filename
   if( %this.testDocument.getFileName() $= "" )
      %filename = TextDocumentController::getSaveName();
   else
      %filename = %this.testDocument.getFileName();
      
   if(%filename $= "")
      return;
      
   // Save the file
   if( isWriteableFileName( %filename ) )
      %this.testDocument.saveToFile( %filename );

   %this.updateTitleBar();    
}

function TextEditorCanvas::revert( %this )
{
   if( !isObject( %this.testDocument ) )
      return;
      
   // Revert to copy on disk
   %this.testDocument.revertToFile();
   %this.updateTitleBar(); 
      
}

function TextEditorCanvas::onWindowClose( %this )
{
   %this.quit();
}

function TextEditorCanvas::quit( %this )
{
   if( !isObject( %this.testDocument ) )
      return;
   
   // we must not delete a window while in its event handler, or we foul the event dispatch mechanism.
   %this.schedule(10, delete);
}