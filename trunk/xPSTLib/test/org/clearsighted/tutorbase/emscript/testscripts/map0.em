sequence
{
	ResizeImage and Error;
}

mappings
{
	MainMenu.Image._1 => ResizeImage;
	switch
	{
		MainMenu.Image._2="Bliz"
	} => Error;
}

feedback
{
	ResizeImage
	{
		answer: 0;
	}
	Error
	{
		answer: 0;
	}
}
