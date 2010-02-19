//---------------------------------------------------------------------------------------------
// Torque Game Builder
// Copyright (C) GarageGames.com, Inc.
//---------------------------------------------------------------------------------------------

function GuiEditorPaletteGui::onWake(%this)
{
   GuiEditorStealPalette(%this);
}

function GuiEditorStealPalette(%thief)
{
   %pal = %thief-->palette;
   if(!isObject(%pal))
      %thief.add(GuiEditorPalette);
}

//----------------------------------------
function GuiEditorStartCreate()
{
   NewGuiDialogClass.setText("GuiControl");
   NewGuiDialogClass.sort();
   NewGuiDialogName.setValue("NewGui");
   Canvas.pushDialog(NewGuiDialog);
}

//----------------------------------------
function GuiEditorCreate()
{
   %name = NewGuiDialogName.getValue();
   %class = NewGuiDialogClass.getText();
   Canvas.popDialog(NewGuiDialog);
   %obj = eval("return new " @ %class @ "(" @ %name @ ");");
   GuiEditorOpen(%obj);
}

package GuiEditor_BlockDialogs
{

function GuiCanvas::pushDialog()
{

}

function GuiCanvas::popDialog()
{

}

};

function GuiEditor::enableMenuItems(%this, %val)
{
   %menu = GuiEditCanvas.menuBar->EditMenu.getID();
   
   %menu.enableItem(0, %val); // undo
   %menu.enableItem(1, %val); // redo
   %menu.enableItem(3, %val); // cut
   %menu.enableItem(4, %val); // copy
   %menu.enableItem(5, %val); // paste
   %menu.enableItem(6, %val); // selectall
   
   GuiEditCanvas.menuBar->LayoutMenu.enableAllItems(%val);
   GuiEditCanvas.menuBar->MoveMenu.enableAllItems(%val);
}

function GuiEditorScanGroupForGuis(%group)
{
   while((%obj = %group.getObject(%i)) != -1)
   {
      if(%obj.getClassName() $= "GuiCanvas")
      {
         GuiEditorScanGroupForGuis(%obj);
      }
      else 
      {
         if(%obj.getName() $= "")
            %name = "(unnamed) - " @ %obj;
         else
            %name = %obj.getName() @ " - " @ %obj;

         GuiEditorContentList.add(%name, %obj);
      }
      %i++;
   }
}

//----------------------------------------
function GuiEditorOpen(%content)
{   
   Canvas.setContent(GuiEditorGui);
   %obj = GuiEditorContent.getObject(0);
   while(isObject(%obj))
   {
      GuiGroup.add(%obj); // get rid of anything being edited
      %obj = GuiEditorContent.getObject(0);
   }
   
   // enumerate all Guis, and put them in a popup. Skip canvases, and the blank gui. 
   %i = 0;
   GuiEditorContentList.clear();
   GuiEditorScanGroupForGuis(GuiGroup);
   
   activatePackage(GuiEditor_BlockDialogs);
   GuiEditorContent.add(%content);
   deactivatePackage(GuiEditor_BlockDialogs);
   GuiEditorContentList.sort();

   GuiEditorResList.clear();
   GuiEditorResList.add("640 x 480", 640);
   GuiEditorResList.add("800 x 600", 800);
   GuiEditorResList.add("1024 x 768", 1024);
   %ext = $Pref::GuiEditor::PreviewResolution;
   if( %ext $= "" )
   {
   %ext = GuiEditorRegion.getExtent();
   echo("extent is " @ %ext );
   switch(getWord(%ext, 0))
   {
      case 640:
         GuiEditorResList.setText("640 x 480");
      case 800:
         GuiEditorResList.setText("800 x 600");
      case 1024:
         GuiEditorResList.setText("1024 x 768");
   }
   }
   else
   {
      GuiEditorResList.setText( getWord(%ext,0) @ " x " @ getWord(%ext, 1) );
   }
   if(%content.getName() $= "")
      %name = "(unnamed) - " @ %content;
   else
      %name = %content.getName() @ " - " @ %content;
   
   GuiEditorContentList.setText(%name);

   GuiEditor.setRoot(%content);
   GuiEditorRegion.resize(0,0,getWord(%ext,0), getWord(%ext, 1));
   GuiEditorContent.getObject(0).resize(0,0,getWord(%ext,0), getWord(%ext, 1));

   //%content.resize(0,0,getWord(%ext,0), getWord(%ext, 1));

   GuiEditorTreeView.open(%content);
   
   // clear the undo manager if we're switching controls.
   if(GuiEditor.lastContent != %content)
      GuiEditor.getUndoManager().clearAll();
      
   GuiEditor.setFirstResponder();
   
   GuiEditor.updateUndoMenu();
   GuiEditor.lastContent = %content;
   $pref::GuiEditor::lastContent = %content;
}

//function GuiEditorMenuBar::onMenuItemSelect(%this, %menuId, %menu, %itemId, %item)
//{
//   if(%this.scriptCommand[%menu, %itemId] !$= "")
//      eval(%this.scriptCommand[%menu, %itemId]);
//   else
//      error("No script command defined for menu " @ %menu  @ " item " @ %item);
//}

//----------------------------------------
function GuiEditorContentList::onSelect(%this, %id)
{
   GuiEditorOpen(%id);
}

//----------------------------------------
function GuiEditorResList::onSelect(%this, %id)
{
   switch(%id)
   {
      case 640:
         GuiEditorRegion.resize(0,0,640,480);
         GuiEditorContent.getObject(0).resize(0,0,640,480);
         $Pref::GuiEditor::PreviewResolution = "640 480";
      case 800:
         GuiEditorRegion.resize(0,0,800,600);
         GuiEditorContent.getObject(0).resize(0,0,800,600);
         $Pref::GuiEditor::PreviewResolution = "800 600";
      case 1024:
         GuiEditorRegion.resize(0,0,1024,768);
         GuiEditorContent.getObject(0).resize(0,0,1024,768);
         $Pref::GuiEditor::PreviewResolution = "1024 768";
   }
}

//----------------------------------------
// defines the icons to be used in the tree view control
// provide the paths to each icon minus the file extension
// seperate them with : 
// the order of the icons must correspond to the bit array defined
// in the GuiTreeViewCtrl.h
function GuiEditorTreeView::onDefineIcons(%this)
{
   //%icons = "common/ui/shll_icon_passworded_hi:common/ui/shll_icon_passworded:common/ui/shll_icon_notqueried_hi:common/ui/shll_icon_notqueried:common/ui/shll_icon_favorite_hi:common/ui/shll_icon_default:";
   //GuiEditorTreeView.buildIconTable(%icons);
}

function GuiEditorTreeView::update(%this)
{
   %obj = GuiEditorContent.getObject(0);
   
   if(!isObject(%obj))
      GuiEditorTreeView.clear();
   else
      GuiEditorTreeView.open(GuiEditorContent.getObject(0));
}

function GuiEditorTreeView::onRightMouseDown(%this, %item, %pts, %obj)
{
   if(%obj)
   {
      GuiEditor.setCurrentAddSet(%obj);
   }
}
function GuiEditorTreeView::onAddSelection(%this,%ctrl)
{
   // rdbnote: commented this out, because it causes the control to get added
   // to the selected controls vector, in the gui editor, multiple times. This
   // is because addSelection does NOT clear the currently selected, like using
   // the .select would. Ironically enough, .select will get called from the
   // code, thus already selecting the object for us.
   //GuiEditor.addSelection(%ctrl);
   
   GuiEditor.setFirstResponder();
}
function GuiEditorTreeView::onRemoveSelection(%this,%ctrl)
{
   GuiEditor.removeSelection(%ctrl);
}
function GuiEditor::onClearSelected(%this)
{ 
   GuiEditorTreeView.clearSelection();
}
function GuiEditor::onAddSelected(%this,%ctrl)
{
   GuiEditorTreeView.addSelection(%ctrl);
   GuiEditorTreeView.scrollVisibleByObjectId(%ctrl);
}

function GuiEditor::onRemoveSelected(%this,%ctrl)
{
   GuiEditorTreeView.removeSelection(%ctrl); 
}
function GuiEditor::onDelete(%this)
{
   GuiEditorTreeView.update();
   // clear out the gui inspector.
   GuiEditorInspectFields.update(0);
}

function GuiEditorTreeView::onDeleteSelection(%this)
{ 
   GuiEditor.clearSelection();
}

function GuiEditorTreeView::onSelect(%this, %obj)
{

   if(isObject(%obj))
   {
      GuiEditorInspectFields.update(%obj);
      GuiEditor.select(%obj);
   }
}

//----------------------------------------
function GuiEditorInspectFields::update(%this, %inspectTarget)
{
   GuiEditorInspectFields.inspect(%inspectTarget);
   if(isObject(%inspectTarget))
      GuiEditorInspectName.setValue(%inspectTarget.getName());
}

//----------------------------------------
function GuiEditorInspectApply()
{
   GuiEditorInspectFields.setName(GuiEditorInspectName.getValue());
}

//----------------------------------------
function GuiEditor::onSelect(%this, %ctrl)
{
   
   GuiEditorInspectFields.update(%ctrl);
   GuiEditor.clearSelection();
   GuiEditor.select(%ctrl);
   GuiEditorTreeView.addSelection(%ctrl); 
}


function GuiEditorSnapCheckBox::onWake(%this)
{
   %snap = $pref::guiEditor::snap2grid * $pref::guiEditor::snap2gridsize;
   %this.setValue(%snap);
   GuiEditor.setSnapToGrid(%snap);
   //error("snap = " @ %snap);   
}
function GuiEditorSnapCheckBox::onAction(%this)
{
   %snap = $pref::guiEditor::snap2gridsize * %this.getValue();
   $pref::guiEditor::snap2grid = %this.getValue();
   //error("snap = " @ %snap);
   GuiEditor.setSnapToGrid(%snap);
}

function GuiEditor::showPrefsDialog(%this)
{
   Canvas.pushDialog(GuiEditorPrefsDlg);
}

function GuiEditor::togglePalette(%this, %show)
{
   %vis = GuiEditorGui-->togglePaletteBtn.getValue();
   if(%show !$= "")
      %vis = %show;
   
   if(%vis)
      GuiEditorGui.add(GuiEditorPalette);
   else  
      GuiEditorPaletteGui.add(GuiEditorPalette);
      
   GuiEditorGui-->togglePaletteBtn.setValue(%vis);
}

function GuiEditorPalette::onWake(%this)
{
   %controls = enumerateConsoleClasses("GuiControl");
   %this-->listboxAll.clearItems();
   //tlc.clear();
   for (%i = 0; %i < getFieldCount(%controls); %i++)
   {
      %field = getField(%controls, %i);
      if(%field $= "GuiCanvas")
         continue;
         
      %this-->listboxAll.addItem(%field);
      //tlc.addrow(%i, %field);
   }
   
   // add some common controls to the common page.
   %list = %this-->listboxCommon;
   %list.clearItems();
   %list.addItem("GuiControl");
   %list.addItem("GuiBitmapButtonCtrl");
   %list.addItem("GuiBitmapButtonTextCtrl");
   %list.addItem("GuiButtonCtrl");
   %list.addItem("GuiCheckBoxCtrl");
   %list.addItem("GuiConsole");
   %list.addItem("GuiFadeInBitmapCtrl");
   %list.addItem("GuiFrameSetCtrl");
   %list.addItem("GuiListBoxCtrl");
   %list.addItem("GuiPopUpMenuCtrl");
   %list.addItem("GuiRadioCtrl");
   %list.addItem("GuiRolloutCtrl");
   %list.addItem("GuiScrollCtrl");
   %list.addItem("GuiSeparatorCtrl");
   %list.addItem("GuiSliderCtrl");
   %list.addItem("GuiTabBookCtrl");
   %list.addItem("GuiTabPageCtrl");
   %list.addItem("GuiTextCtrl");
   %list.addItem("GuiTextEditCtrl");
   %list.addItem("GuiWindowCtrl");
   
   %this-->paletteBook.selectPage(0);
}

function GuiEditorPaletteDragList::onMouseDragged(%this)
{
   %position = %this.getGlobalPosition();
   %cursorpos = Canvas.getCursorPos();
   
   %class = %this.getItemText(%this.getSelectedItem());
   %payload = eval("return new " @ %class @ "();");
   if(!isObject(%payload))
      return;
      
   // this offset puts the cursor in the middle of the dragged object.
   %xOffset = getWord(%payload.extent, 0) / 2;
   %yOffset = getWord(%payload.extent, 1) / 2;  
   
   // position where the drag will start, to prevent visible jumping.
   %xPos = getWord(%cursorpos, 0) - %xOffset;
   %yPos = getWord(%cursorpos, 1) - %yOffset;
   
   %dragCtrl = new GuiDragAndDropControl() {
      canSaveDynamicFields = "0";
      Profile = "GuiDefaultProfile";
      HorizSizing = "right";
      VertSizing = "bottom";
      Position = %xPos SPC %yPos;
      extent = %payload.extent;
      MinExtent = "32 32";
      canSave = "1";
      Visible = "1";
      hovertime = "1000";
      deleteOnMouseUp = true;
   };

   %dragCtrl.add(%payload);
   Canvas.getContent().add(%dragCtrl);
   
   %dragCtrl.startDragging(%xOffset, %yOffset);
}

function GuiEditor::onControlDragged(%this, %payload, %position)
{
   // use the position under the mouse cursor, not the payload position.
   %position = VectorSub(%position, GuiEditorContent.getGlobalPosition());
   %x = getWord(%position, 0);
   %y = getWord(%position, 1);
   %target = GuiEditorContent.findHitControl(%x, %y);
   
   while(! %target.isContainer )
      %target = %target.getParent();
   //echo(%target SPC %target.getName());
   if( %target != %this.getCurrentAddSet())
   %this.setCurrentAddSet(%target);
}

function GuiEditor::onControlDropped(%this, %payload, %position)
{  
   %pos = %payload.getGlobalPosition();
   %x = getWord(%pos, 0);
   %y = getWord(%pos, 1);

   %this.addNewCtrl(%payload);
   
   %payload.setPositionGlobal(%x, %y);
   %this.setFirstResponder();
}

function GuiEditor::onGainFirstResponder(%this)
{
   %this.enableMenuItems(true);
}

function GuiEditor::onLoseFirstResponder(%this)
{
   %this.enableMenuItems(false);
}

function GuiEditor::undo(%this)
{
   %this.getUndoManager().undo();
   %this.updateUndoMenu();
   %this.clearSelection();
}

function GuiEditor::redo(%this)
{
   %this.getUndoManager().redo();
   %this.updateUndoMenu();
   %this.clearSelection();
}

function GuiEditor::updateUndoMenu(%this)
{
   %man = %this.getUndoManager();
   %nextUndo = %man.getNextUndoName();
   %nextRedo = %man.getNextRedoName();

   %cmdCtrl = $platform $= "macos" ? "Cmd" : "Ctrl";
   %undoitem = "Undo" SPC %nextUndo TAB %cmdCtrl SPC "Z" TAB "GuiEditor.undo();";
   %redoitem = "Redo" SPC %nextRedo TAB %cmdCtrl @ "-shift Z" TAB "GuiEditor.redo();";
   
   GuiEditCanvas.menuBar->editMenu.removeItem(0);
   GuiEditCanvas.menuBar->editMenu.removeItem(0);
   GuiEditCanvas.menuBar->editMenu.addItem(0, %undoitem);
   GuiEditCanvas.menuBar->editMenu.addItem(1, %redoitem);
   
   GuiEditCanvas.menuBar->editMenu.enableItem(0, %nextUndo !$= "");
   GuiEditCanvas.menuBar->editMenu.enableItem(1, %nextRedo !$= "");
}

//------------------------------------------------------------------------------
// Gui Editor Menu activation
function GuiEditorGui::onWake(%this)
{
   if( !isObject( %this.menuGroup))
      return;

   for( %i = 0; %i < %this.menuGroup.getCount(); %i++)
     %this.menuGroup.getObject(%i).attachToMenuBar();
}

function GuiEditorGui::onSleep( %this)
{
   if( !isObject( %this.menuGroup))
      return;
      
   for( %i = 0; %i < %this.menuGroup.getCount(); %i++)
      %this.menuGroup.getObject(%i).removeFromMenuBar();
}

GlobalActionMap.bind(keyboard, "f10", GuiEdit);
