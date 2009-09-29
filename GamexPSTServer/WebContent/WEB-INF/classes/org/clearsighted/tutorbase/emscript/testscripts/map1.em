sequence
{
	ResizeImage and Error and Disabled;
}

mappings
{
	Special.Disabled => Disabled;
	MainMenu.Image._1 => ResizeImage;
	switch
	{
		MainMenu.Image._2="Bliz"
	} => Error;
}
