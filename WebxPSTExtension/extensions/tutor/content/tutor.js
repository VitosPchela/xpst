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

function onQuit()
{
  var back = document.getElementById("Browser:Back");      
  back.removeAttribute("disabled");
  window.close();
}
var mytre = "";
function onEnter(event)
{
  var box = window.document.getElementById("box-one");
  go("SE/1.2&VERB=S:12:NOTEVALUESET&MESSAGENUMBER=I:2:82&OBJECT=O:2:S:6:OBJECT,S:4:NAME,S:6:Resize,S:6:OBJECT,S:4:NAME,S:16:pixelWidthUpDown&VALUE=D:3:"+box.value+"&\n");
  //go("SE/1.2&VERB=S:12:NOTEVALUESET&MESSAGENUMBER=I:2:82&OBJECT=O:2:S:6:OBJECT,S:4:NAME,S:6:Resize,S:6:OBJECT,S:4:NAME,S:16:pixelWidthUpDown&VALUE=D:5:800.0&\n");
}
function onStart()
{
    var params = "command=starttre&task=foo";
    var xh = new XMLHttpRequest();
    xh.open("POST", "http://localhost:8080/WebTRE/WebTRE", false);
    xh.setRequestHeader("Content-type", "application/x-www-form-urlencoded");
    xh.setRequestHeader("Content-length", params.length);
    xh.setRequestHeader("Connection", "close");
    xh.send(params);
    alert(xh.responseText);
    mytre = xh.responseText;
    go("");  // Get initial messages from tutor
}
function onHint()
{
  dump("hint\n");
}
function stop()
{
  var params = "command=stoptre&trename=" + mytre;
  var xh = new XMLHttpRequest();
  xh.open("POST", "http://localhost:8080/WebTRE/WebTRE", false);
  xh.setRequestHeader("Content-type", "application/x-www-form-urlencoded");
  xh.setRequestHeader("Content-length", params.length);
  xh.setRequestHeader("Connection", "close");
  xh.send(params);
  alert(xh.responseText);
  mytre = null;
}

function gogood()
{
  go("SE/1.2&VERB=S:12:NOTEVALUESET&MESSAGENUMBER=I:2:82&OBJECT=O:2:S:6:OBJECT,S:4:NAME,S:6:Resize,S:6:OBJECT,S:4:NAME,S:16:pixelWidthUpDown&VALUE=D:5:800.0&\n");
}

function gobad()
{
  go("SE/1.2&VERB=S:12:NOTEVALUESET&MESSAGENUMBER=I:2:82&OBJECT=O:2:S:6:OBJECT,S:4:NAME,S:6:Resize,S:6:OBJECT,S:4:NAME,S:16:pixelWidthUpDown&VALUE=D:5:500.0&\n");
}

function go(msg)
{
  //alert(mytre);
  var xh = new XMLHttpRequest();
  xh.open("POST", "http://localhost:8080/WebTRE/WebTRE/" + mytre, false);
  xh.send(msg);
  alert(xh.status + " " + xh.responseText);
}
