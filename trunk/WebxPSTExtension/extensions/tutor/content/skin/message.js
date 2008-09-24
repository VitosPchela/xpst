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

image01 = new Image();
image01.src = g_skinbase + "/images/close.jpg";
image02 = new Image();
image02.src = g_skinbase + "/images/close2.jpg";

image03 = new Image();
image03.src = g_skinbase + "/images/previoushint.jpg";
image04 = new Image();
image04.src = g_skinbase + "/images/previoushint2.jpg";

image05 = new Image();
image05.src = g_skinbase + "/images/nexthint.jpg";
image06 = new Image();
image06.src = g_skinbase + "/images/nexthint2.jpg";

image07 = new Image();
image07.src = g_skinbase + "/images/ok.png";
image08 = new Image();
image08.src = g_skinbase + "/images/ok2.png";

function rollover(imagename, newsrc)
{
	document.images[imagename].src = newsrc.src;
}

var MouseDownAtX = null, MouseDownAtY = null;
var MouseDownWinPosX = null, MouseDownWinPosY = null;
function mousedown(evt)
{
//							document.getElementById("header").setCapture();
	MouseDownAtX = evt.screenX;
	MouseDownAtY = evt.screenY;
	MouseDownWinPosX = window.parent.screenX;
	MouseDownWinPosY = window.parent.screenY;
}

function mouseup(evt)
{
//							document.getElementById("header").releaseCapture();
	MouseDownAtX = null;
	MouseDownAtY = null;
	MouseDownWinPosX = null;
	MouseDownWinPosY = null;
}

function mousemove(evt)
{
	if (MouseDownWinPosX)
	{
		var dx = evt.screenX - MouseDownAtX;
		var dy = evt.screenY - MouseDownAtY;
		window.parent.screenX = MouseDownWinPosX + dx;
		window.parent.screenY = MouseDownWinPosY + dy;
	}
}

function makeHTML(msgcore, type)
{
	var wrappedmsg = '<message><is' + type + '/><page index="0" total="0"/><base href="' + g_base + '"/><skinbase href="' + g_skinbase + '"/>' + msgcore + '</message>';
	var xslt = xslTransform.load(g_skinbase + 'message.xml');
	var xmsg = xslTransform.load(wrappedmsg);
	var ret = xslTransform.transform(xslt, xmsg, {}).string;
	return ret;
}

function makeHTMLWithXML(xmsg, type)
{
	var basetag = xmsg.createElement('base');
	var skinbasetag = xmsg.createElement('skinbase');
	var scenariotag = xmsg.createElement('isscenario');
	basetag.setAttribute('href', g_base);
	skinbasetag.setAttribute('href', g_skinbase);
	xmsg.documentElement.appendChild(basetag);
	xmsg.documentElement.appendChild(skinbasetag);
	xmsg.documentElement.appendChild(scenariotag);
	var xslt = xslTransform.load(g_skinbase + 'message.xml');
	var ret = xslTransform.transform(xslt, xmsg, {}).string;
	return ret;
}

var g_imagepopup = null;
function popupimage(url, thumb)
{
	var si = new Image();
	si.src = url;
	var pos = findPos(thumb);
	var r = getVisiblePosition(pos[0], pos[0] + thumb.width, pos[1], pos[1] + thumb.height, si.width, si.height);
	var imagehtml = makeHTML('<url>' + url + '</url>', 'image');
	g_imagepopup = window.open('data:text/html;base64,' + btoa(imagehtml), 'Image Detail', 'chrome,width=' + si.width + ',height=' + si.height + ',left=' + r[0] + ',top=' + r[1]);
}

function popup(evt, type, href, el)
{
	var width = 400, height = 400;
	evt.preventDefault();
	evt.stopPropagation();
	var pos = findPos(el);
	var r = getVisiblePosition(pos[0], pos[0] + el.offsetWidth, pos[1], pos[1] + el.offsetHeight, width, height);
	var xmsg = xslTransform.load(href);
	var html = makeHTMLWithXML(xmsg, type);
	g_imagepopup = window.open('data:text/html;base64,' + btoa(html), 'More information', 'chrome,width=' + width + ',height=' + height + ',left=' + r[0] + ',top=' + r[1]);
}

function popdownimage()
{
	if (g_imagepopup)
	{
		g_imagepopup.close();
		g_imagepopup = null;
	}
}

