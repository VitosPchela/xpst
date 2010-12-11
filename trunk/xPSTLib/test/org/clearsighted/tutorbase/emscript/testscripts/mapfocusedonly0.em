sequence
{
	Check-Maintain-Aspect then (Set-Print-Height and Set-Print-Width and Set-Pixel-Width and Set-Pixel-Height);
}

mappings
{
	Resize.constrainCheckBox => Check-Maintain-Aspect;
	
	[focusedonly] Resize.printWidthUpDown => Set-Print-Width;
	[focusedonly] Resize.printHeightUpDown => Set-Print-Height;
	[focusedonly] Resize.pixelWidthUpDown => Set-Pixel-Width;
	[focusedonly] Resize.pixelHeightUpDown => Set-Pixel-Height;
}

feedback
{
	Check-Maintain-Aspect { answer: 0; }
	Set-Print-Width { answer: 0; }
	Set-Print-Height { answer: 0; }
	Set-Pixel-Width { answer: 0; }
	Set-Pixel-Height { answer: 0; }
}
