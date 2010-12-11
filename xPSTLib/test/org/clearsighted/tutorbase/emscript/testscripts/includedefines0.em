include "definestoinclude.em";

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
