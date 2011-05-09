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

var hintMsg = '';
var hintNumber = 0;
var hintLocation = 0;

function getHintHTMLInner(msg, num, maxnum)
{
	var serverurl = window.opener.g_webTREURL;
	var i = serverurl.lastIndexOf('/', serverurl.length - 2);
	var webtreassets = serverurl.substring(0, i) + '/WebxPST/assets/';
	var thisurl = window.location.href;
	var skinbase = thisurl.substring(0, thisurl.lastIndexOf('/')) + '/skin';
	var wrappedmsg = '<message><ishint/><page index="' + num + '" total="' + maxnum + '"/><base href="' + webtreassets + '"/><skinbase href="' + skinbase + '"/>' + msg + "</message>";
	var xslt = xslTransform.load(skinbase + '/message.xml');
	var xmsg = xslTransform.load(wrappedmsg);
	if (typeof(xmsg) != 'object' || xmsg.documentElement.nodeName == 'parsererror')
		return null;
	return xslTransform.transform(xslt, xmsg,	{}).string;
}

function getHintHTML(msg, num, maxnum)
{
	var ret = getHintHTMLInner(msg, num, maxnum);
	if (ret == null)
		ret = getHintHTMLInner('Sorry, the message XML didn\'t parse', num, maxnum);
	return ret;
}

function onLoad()
{
	hintMsg = window.arguments[0];
	hintLocation = window.arguments[1];
	var box = window.document.getElementById("frameid");
	var numhints = hintMsg.arrParameters[hintLocation].objValue.value.length;
	if (hintMsg == false || hintNumber + 1 == numhints)
	{
		if (hintMsg != false)
		{
			var msg = hintMsg.arrParameters[hintLocation].objValue.value[hintNumber].value;
			box.setAttribute('src', "data:text/html;base64," + btoa(getHintHTML(msg, hintNumber, numhints)));
		}
	}
	else
	{
		var msg = hintMsg.arrParameters[hintLocation].objValue.value[hintNumber].value;
		box.setAttribute('src', "data:text/html;base64," + btoa(getHintHTML(msg, hintNumber, numhints)));
	}
	moveTo(opener.screenX + opener.outerWidth / 2 - window.outerWidth / 2, opener.screenY + opener.outerHeight / 2 - window.outerHeight / 2);
	
	window.addEventListener('keypress', keypress, false);
}

function keypress(evt)
{
	if (evt.keyCode == 123)  // Ctrl-F12
	{
		opener.completeCurrentGoalnode();
		close();
	}
}

function ok()
{
	close();
}

function prevHint()
{
	opener.logToServer('PREV HINT');
	var box = window.document.getElementById("frameid");
	hintNumber--;
	var numhints = hintMsg.arrParameters[hintLocation].objValue.value.length;
	var msg = hintMsg.arrParameters[hintLocation].objValue.value[hintNumber].value
	box.setAttribute('src', "data:text/html;base64," + btoa(getHintHTML(msg, hintNumber, numhints)));
}

function nextHint()
{
	opener.logToServer('NEXT HINT');
	var box = window.document.getElementById("frameid");
	hintNumber++;
	var numhints = hintMsg.arrParameters[hintLocation].objValue.value.length;
	var msg = hintMsg.arrParameters[hintLocation].objValue.value[hintNumber].value
	box.setAttribute('src', "data:text/html;base64," + btoa(getHintHTML(msg, hintNumber, numhints)));
}
