defines
{
	ImageMenu = switch
	{
		Image._2="Image:Resize",
		Image._3="Image:CanvasSize"
	};
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