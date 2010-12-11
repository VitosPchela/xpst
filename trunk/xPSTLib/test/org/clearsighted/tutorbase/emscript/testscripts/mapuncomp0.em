#include "OffPath.em";

options
{
	tutorname="PDN";
}

sequence
{
	(Set-Resolution then Click-OK) and Error-Disabled-Undoable and Error-Disabled-NoUndo and 
  	Error-Off-Path-Undoable and Error-Off-Path-NoUndo and Click-Cancel and Click-OK
  	;
}

mappings
{

	###
	[priority=2] Application.FileOpen => Select-File-To-Open;

	#------------------------------------------------------
	# Case 1a&1b: Rotate 90cw (270ccw) then flip horizontal
	#------------------------------------------------------
	
	### Rotate 90cw before any flipping has occurred
	[priority=2] switch
	{
		# answer
		mainMenu._3._5._0:click="Image:Rotate:90 CW", 
		# for JITs
		mainMenu._3._5._1:click="Image:Rotate:180 CW", 
		mainMenu._3._5._5:click="Image:Rotate:180 CCW", 
	} => Rotate-First-90CW;

	
	### Rotate 270 CCW before any flipping has occurred
	[priority=2] switch
	{
		# answer
		mainMenu._3._5._6:click="Image:Rotate:270 CCW", 
		# for JITs
		mainMenu._3._5._1:click="Image:Rotate:180 CW", 
		mainMenu._3._5._5:click="Image:Rotate:180 CCW", 
	} => Rotate-First-270CCW;
	
	### when the image has already been rotated and needs a horizontal flip
	[priority=2] switch
	{
		# answer
		mainMenu._3._4._0:click="Image:Flip:Horizontal",
		# for JITs
		mainMenu._3._4._1:click="Image:Flip:Vertical",
		mainMenu._3._5._0:click="Image:Rotate:90 CW", 
		mainMenu._3._5._1:click="Image:Rotate:180 CW", 
		mainMenu._3._5._2:click="Image:Rotate:270 CW", 
		mainMenu._3._5._4:click="Image:Rotate:90 CCW", 
		mainMenu._3._5._5:click="Image:Rotate:180 CCW", 
		mainMenu._3._5._6:click="Image:Rotate:270 CCW", 
	} => Flip-Horizontal-Second;
	
	#------------------------------------------------------
	# Case 2a&2b: Rotate 90ccw (270cw) then flip vertical
	#------------------------------------------------------
	
	### rotate 270cw before any flipping has occurred
	[priority=2] switch
	{
		#answer
		mainMenu._3._5._2:click="Image:Rotate:270 CW", 
		mainMenu._3._5._1:click="Image:Rotate:180 CW", 
		mainMenu._3._5._5:click="Image:Rotate:180 CCW",
	} => Rotate-First-270CW;
	
	### rotate 90ccw before any flipping has occurred
	[priority=2] switch
	{
		#answer
		mainMenu._3._5._4:click="Image:Rotate:90 CCW", 		
		mainMenu._3._5._1:click="Image:Rotate:180 CW", 
		mainMenu._3._5._5:click="Image:Rotate:180 CCW",
	} => Rotate-First-90CCW;
		
	### when the image has already been rotated and needs a vertical flip
	[priority=2] switch
	{
		# answer
		mainMenu._3._4._1:click="Image:Flip:Vertical",
		# for JITs
		mainMenu._3._4._0:click="Image:Flip:Horizontal",
		mainMenu._3._5._0:click="Image:Rotate:90 CW", 
		mainMenu._3._5._1:click="Image:Rotate:180 CW", 
		mainMenu._3._5._2:click="Image:Rotate:270 CW", 
		mainMenu._3._5._4:click="Image:Rotate:90 CCW", 
		mainMenu._3._5._5:click="Image:Rotate:180 CCW", 
		mainMenu._3._5._6:click="Image:Rotate:270 CCW", 
	} => Flip-Vertical-Second;
	
	
	#------------------------------------------------------
	# Case 3a&3b: Flip horizontal then rotate 90ccw (270cw)
	#------------------------------------------------------
	
	### when the image is flipped horizontally before it is rotated
	[priority=2] switch
	{
		# answer
		mainMenu._3._4._0:click="Image:Flip:Horizontal",
		# for JITs
		mainMenu._3._5._1:click="Image:Rotate:180 CW", 
		mainMenu._3._5._5:click="Image:Rotate:180 CCW",  
	} => Flip-Horizontal-First;
	
	
	#rotate after flip, 270CW
	[priority=2] switch
	{
		#answer
		mainMenu._3._5._2:click="Image:Rotate:270 CW", 
		# for JITs
		mainMenu._3._4._1:click="Image:Flip:Vertical",
		mainMenu._3._4._0:click="Image:Flip:Horizontal",
		mainMenu._3._5._0:click="Image:Rotate:90 CW", 
		mainMenu._3._5._1:click="Image:Rotate:180 CW", 
		mainMenu._3._5._5:click="Image:Rotate:180 CCW", 
		mainMenu._3._5._6:click="Image:Rotate:270 CCW", 
	} => Rotate-Second-270CW;
	
	### rotate after flip, 90ccw
	[priority=2] switch
	{
		#answer
		mainMenu._3._5._4:click="Image:Rotate:90 CCW", 
		# for JITs
		mainMenu._3._4._1:click="Image:Flip:Vertical",
		mainMenu._3._4._0:click="Image:Flip:Horizontal",
		mainMenu._3._5._0:click="Image:Rotate:90 CW", 
		mainMenu._3._5._1:click="Image:Rotate:180 CW", 
		mainMenu._3._5._5:click="Image:Rotate:180 CCW", 
		mainMenu._3._5._6:click="Image:Rotate:270 CCW", 
	} => Rotate-Second-90CCW;
	
	#------------------------------------------------------
	# Case 4a&4b: Flip vertical then rotate 90cw (270ccw)
	#------------------------------------------------------
	
	### when the image is flipped vertically before it is rotated
	[priority=2] switch
	{
		# answer
		mainMenu._3._4._1:click="Image:Flip:Vertical",
		# for JITs
		mainMenu._3._5._1:click="Image:Rotate:180 CW", 
		mainMenu._3._5._5:click="Image:Rotate:180 CCW", 
	} => Flip-Vertical-First;
	
	### Rotate 90 after flipping has occurred
	[priority=2] switch
	{
		# answer
		mainMenu._3._5._0:click="Image:Rotate:90 CW", 
		# for JITs
		mainMenu._3._4._0:click="Image:Flip:Horizontal",
		mainMenu._3._4._1:click="Image:Flip:Vertical",
		mainMenu._3._5._1:click="Image:Rotate:180 CW", 
		mainMenu._3._5._2:click="Image:Rotate:270 CW", 
		mainMenu._3._5._4:click="Image:Rotate:90 CCW", 
		mainMenu._3._5._5:click="Image:Rotate:180 CCW", 
	} => Rotate-Second-90CW;
	
	### Rotate 270 CCW after flipping has occurred
	[priority=2] switch
	{
		# answer
		mainMenu._3._5._6:click="Image:Rotate:270 CCW", 
		# for JITs
		mainMenu._3._4._0:click="Image:Flip:Horizontal",
		mainMenu._3._4._1:click="Image:Flip:Vertical",
		mainMenu._3._5._1:click="Image:Rotate:180 CW", 
		mainMenu._3._5._2:click="Image:Rotate:270 CW", 
		mainMenu._3._5._4:click="Image:Rotate:90 CCW", 
		mainMenu._3._5._5:click="Image:Rotate:180 CCW", 
	} => Rotate-Second-270CCW;
		
	###
	[priority=2] switch
	{
		# answer
		mainMenu._3._1:click="Image:Resize...",
		# JIT
		mainMenu._3._2:click="Image:Canvas Size..."
	} => Open-Resize;


	###
	[priority=2] switch
	{
		# answer
		Resize.absoluteRB="By absolute size",
		# JIT
		Resize.percentRB="By percentage"
	} => Choose-Absolute;

	###
	# Don't include ":click" b/c we want to query the checkbox, 
	# not capture the boolean click
	[priority=2] Resize.constrainCheckBox => Check-Maintain-Aspect;

	###
	[priority=2] Resize.resolutionUpDown => Set-Resolution;
	[priority=2,focusedonly] Resize.pixelWidthUpDown => Set-Pixel-Width;
	[priority=2,focusedonly] Resize.pixelHeightUpDown => Set-Pixel-Height;

	[priority=2] Resize.okButton:click => Click-OK;
	[priority=2] Resize.cancelButton:click => Click-Cancel;
	
	[priority=2] switch 
	{
		# answer
		mainMenu._0._9:click="File:Save As...",
		# JIT
		mainMenu._0._8:click="File:Save"
	} => Save-File;

	###
	[priority=2] Application.FileSaveAs => Enter-Save-Name;

  #################################
  ### Mappings for disabled GNs ###
  #################################

  #### UNDOABLE 

	[priority=1] switch
	{
		mainMenu._3._4._0:click="flipped the image horizontally",
		mainMenu._3._4._1:click="flipped the image vertically",
		mainMenu._3._5._0:click="rotated the image 90 degrees clockwise", 
		mainMenu._3._5._1:click="rotated the image 180 degrees clockwise", 
		mainMenu._3._5._2:click="rotated the image 270 degrees clockwise", 
		mainMenu._3._5._4:click="rotated the image 90 degrees counterclockwise", 
		mainMenu._3._5._5:click="rotated the image 180 degrees counterclockwise", 
		mainMenu._3._5._6:click="rotated the image 270 degrees counterclockwise", 
		Resize.okButton:click="resized the image",
  } => Error-Disabled-Undoable;
	
	###

	#### NO UNDO

  #[priority=1] Application.FileOpen => Error-Disabled-NoUndo; # will want to uncomplete all gns and display message?


###
	[priority=1] switch
	{
		# answer
		mainMenu._3._1:click="Image:Resize...",
	} => Error-Disabled-NoUndo;

  #### Choose UNDO Itself
  switch 
	{
		mainMenu._1._0:click="Edit:Undo",
	} => Error-Chose-Undo;

    
#	[priority=0] switch
#	{
#		Workspace.toolStripContainer._0.documentView.panel.surfaceBox:click="clicked on the image" 
#	} => Error-Disabled-Undoable;



}

feedback
{
	Error-Disabled-NoUndo { answer: 0; }
	Click-OK { answer: 0; }
	Error-Off-Path-NoUndo { answer: 0; }
	Error-Off-Path-Undoable { answer: 0; }
	Set-Resolution { answer: 0; }
	Error-Disabled-Undoable { answer: 0; }
	Click-Cancel { answer: 0; }
}
