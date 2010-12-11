defines
{
	ImageMenu = switch
	{
		Image._2="Image:Resize",
		Image._3="Image:CanvasSize"
	};
}

sequence
{
	ImageResize;
}

mappings
{
	switch
	{
		Edit._1="Copy",
		+ImageMenu,
		Edit._2="Paste"
	} => ImageResize;
}

feedback
{
	ImageResize
	{
		answer: 0;
	}
}
