sequence
{
	(ResizeImage then OtherThing) and Error and Disabled;
}

mappings
{
	Special.Disabled => Disabled;
	MainMenu.Image._1 => ResizeImage;

	[priority=0]
	switch
	{
		MainMenu.Image._2="Bliz"
	} => Error;
	
	switch
	{
		MainMenu.Image._2="That"
	} => OtherThing;
}

feedback
{
	Disabled { answer: 0; }
	ResizeImage { answer: 0; }
	Error { answer: 0; }
	OtherThing { answer: 0; }
}
