//------------------------------------------------------------------------------
// Torque        Engine
// Copyright (C) GarageGames.com, Inc.
//------------------------------------------------------------------------------

//------------------------------------------------------------------------------
// ListMenu methods
//------------------------------------------------------------------------------

/// Callback when this gui is added to the sim.
function ListMenu::onAdd(%this)
{
   %this.addRow("Play Game", "onSinglePlayer", 0);
   %this.addRow("Select Model", "onSelectModel", 4, -15);
   %this.addRow("Setup", "onOptions", 6, -15);
   %this.addRow("Exit Game", "onQuit", 8, -15);
}

//------------------------------------------------------------------------------
// MainMenuButtonHolder methods
//------------------------------------------------------------------------------

function MainMenuButtonHolder::onWake(%this)
{
   %this.add(GamepadButtonsGui);

   GamepadButtonsGui.setButton($BUTTON_A, "Go", ListMenu.CallbackOnA);
}

//------------------------------------------------------------------------------
// global methods
//------------------------------------------------------------------------------

/// Callback from the shell button for triggering single player.
function onSinglePlayer()
{
   echo("Default implementation. Override onSinglePlayer() to add game specific functionality");
}

/// Callback from the shell button to bring up the object picker.
function onSelectModel()
{
   Canvas.setContent(ObjectPickerGui);
}

/// Callback from the shell button to bring up the options gui.
function onOptions()
{
   Canvas.setContent(OptionsGui);
}

/// Callback from the shell "quit" button.
function onQuit()
{
   echo("Default implementation. Override onQuit() to add game specific functionality");
   quit();
}
