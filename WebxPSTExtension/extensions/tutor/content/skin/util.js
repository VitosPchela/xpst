/*
Copyright (c) Clearsighted 2006-08 stephen@clearsighted.net

This library is free software; you can redistribute it and/or
modify it under the terms of the GNU Lesser General Public
License as published by the Free Software Foundation; either
version 2.1 of the License, or (at your option) any later version.

This library is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
Lesser General Public License for more details.

You should have received a copy of the GNU Lesser General Public
License along with this library; if not, write to the Free Software
Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301  USA
*/

function findPos(obj)
{
	var doc = obj.ownerDocument;
	var bo = doc.getBoxObjectFor(obj);
	var r = [bo.screenX, bo.screenY, bo.width, bo.height];
	return r;
}
	
function getVisiblePosition(excll, exclr, exclt, exclb, boxw, boxh)
{
	var padding = 20;
	var clearright = exclr + padding + boxw <= window.screen.width;
	var clearleft = excll - padding - boxw >= 0;
	var cleartop = exclt - padding - boxh >= 0;
	var clearbottom = exclb + padding + boxh <= window.screen.height;
	var px, py;

	if (clearright)
		px = exclr + padding;
	else if (clearleft)
		px = excll - padding - boxw;
	else
		px = window.screen.width - boxw;

	if (cleartop)
		py = exclt - padding - boxh;
	else if (clearbottom)
		py = exclb + padding;
	else
		py = 0;

	return [px, py];
}
