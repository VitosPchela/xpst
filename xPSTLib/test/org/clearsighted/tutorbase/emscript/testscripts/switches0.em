sequence
{
	ImageResize then ExitResize;
}

mappings
{
	switch
	{
		Image._2="Image:Resize",
		Image._3="Image:CanvasSize"
	} => ImageResize;
	ResizeDialog.OK => ExitResize;
}

feedback
{
	ImageResize
	{
		answer: 0;
	}
	ExitResize
	{
		answer: 0;
	}
	
}
