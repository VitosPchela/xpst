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

var g_webTREURL = null;
var g_taskName = 'se';
// some global state
var g_isTutorRunning = false;
var g_oldSelectedIndex = null;
var g_blockJITs = false;
var g_observeIDs = false;
var mytre = '';
var g_vanthUsername = null;
var XULNS = "http://www.mozilla.org/keymaster/gatekeeper/there.is.only.xul";

var objScriptLoader1 = Components.classes["@mozilla.org/moz/jssubscript-loader;1"].getService(Components.interfaces.mozIJSSubScriptLoader);
objScriptLoader1.loadSubScript("chrome://webxpst/content/BubbleTooltips.js");
var objScriptLoader2 = Components.classes["@mozilla.org/moz/jssubscript-loader;1"].getService(Components.interfaces.mozIJSSubScriptLoader);
objScriptLoader2.loadSubScript("chrome://webxpst/content/catcher.js");

var catcher = null;

// window.addEventListener("load", EventCatcher_initialize, false);
window.addEventListener('unload', onUnload, false);
window.addEventListener('keypress', onKeypress, false);
var mainWindow = window.QueryInterface(Components.interfaces.nsIInterfaceRequestor)
	.getInterface(Components.interfaces.nsIWebNavigation)
	.QueryInterface(Components.interfaces.nsIDocShellTreeItem)
	.rootTreeItem
	.QueryInterface(Components.interfaces.nsIInterfaceRequestor)
	.getInterface(Components.interfaces.nsIDOMWindow);
mainWindow.document.addEventListener('webxpst-open-tasklist', onOpenTasklist, false, true);
mainWindow.document.addEventListener('webxpst-start-task', onStartTask, false, true);
mainWindow.document.addEventListener('webxpst-disable-tutor-ui', onDisableTutorUI, false, true);

var g_prefs = Components.classes["@mozilla.org/preferences-service;1"].getService(Components.interfaces.nsIPrefService).getBranch('webxpst.');

function onLoad()
{
	if (g_prefs.getBoolPref('authoringmode'))
	{
		g_observeIDs = true;
		$('#evtbox').attr('collapsed', false);
		startEventCatcher();
	}

	var doc = content.document;
	var evt = doc.createEvent('Events');
	evt.initEvent('webxpst-sidebar-opened', true, false);
	doc.dispatchEvent(evt);
}

function onUnload()
{
	if (catcher)
		catcher.removeClickListeners();
}

function showToolbar()
{
	document.getElementById('webxpst-toolbar').collapsed = false;
	document.persist('webxpst-toolbar', 'collapsed');
}

function onDisableTutorUI()
{
	setButtonsDisabled(true);
}

function onHelp()
{
	window.openDialog('chrome://webxpst/content/help.html', 'Help', 'chrome,modal,width=500,height=300,centerscreen=yes');
}

function setButtonsDisabled(val)
{
	document.getElementById('webxpst-hint-button').disabled = val;
	document.getElementById('webxpst-done-button').disabled = val;
}

function onOpenTasklist(evt)
{
	if (g_isTutorRunning)
		stop();

	var tasklist = evt.originalTarget;
	g_webTREURL = tasklist.getAttribute('webtreurl');
	var tasks = tasklist.getElementsByTagName('task');
	var taskel = document.getElementById('task');
	var numtaskelchildren = taskel.childNodes.length;
	for (var j = 0; j < numtaskelchildren; j++)
		taskel.removeChild(taskel.firstChild);
	for (var i in tasks)
	{
		var task = tasks[i];
		var listitem = document.createElementNS(XULNS, 'listitem');
		listitem.setAttribute('label', task.getAttribute('title'));
		listitem.setAttribute('value', task.getAttribute('name'));
		taskel.appendChild(listitem);
	}
	// taskel.firstChild.setAttribute('selected', true);
	if(g_observeIDs)
	{
		var urlparts = new Array();
		urlparts=g_webTREURL.split('/');
		var hostmach=urlparts[2]; 
		document.getElementById('serverdetails').setAttribute('collapsed', false);
		document.getElementById('servername').value=hostmach;
	}
	else
	{
		document.getElementById('serverdetails').setAttribute('collapsed', true);
	}
	
	document.getElementById('waitingtext').setAttribute('collapsed', true);
	document.getElementById('taskselect').setAttribute('collapsed', false);
}

function onStartTask(evt)
{
	showToolbar();
	setButtonsDisabled(false);

	var task = evt.originalTarget;
	g_webTREURL = task.getAttribute('webtreurl');
	g_taskName = task.getAttribute('task');
//	document.getElementById('waitingtext').setAttribute('collapsed', true);
	if(g_observeIDs)
	{
		var urlparts = new Array();
		urlparts=g_webTREURL.split('/');
		var hostmach=urlparts[2]; 
//		document.getElementById('serverdetails').setAttribute('collapsed', false);
//		document.getElementById('servername').value=hostmach;
	}
	else
	;//	document.getElementById('serverdetails').setAttribute('collapsed', true);
	onStart();
}

function getBase()
{
	var serverurl = g_webTREURL;
	var i = serverurl.lastIndexOf('/', serverurl.length - 2);
	var base = serverurl.substring(0, i) + '/WebxPST/assets/';
	return base;
}

function getSkinBase()
{
	return 'chrome://webxpst/content/skin/';
}

function getBaseTags()
{
	return '<base href="' + getBase() + '"/><skinbase href="' + getSkinBase() + '"/>';
}

function getXMLMessageInner(msg)
{
	var wrappedmsg = '<message><isjit/>' + getBaseTags() + msg + "</message>";
	return xslTransform.load(wrappedmsg);
}

function getXMLMessage(msg)
{
	var ret = getXMLMessageInner(msg);
	if (typeof(ret) != 'object' || ret.documentElement.nodeName == 'parsererror') ret = getXMLMessageInner('Sorry, the message XML didn\'t parse');
		return ret;
}

function getMessageHTML(xmsg)
{
	var xslt = xslTransform.load(getSkinBase() + 'message.xml');
	return xslTransform.transform(xslt, xmsg,	{}).string;
}

function isBlock(xmsg)
{
	return xmsg.getElementsByTagName('block').length > 0;
}

function block(event)
{
	event.preventDefault();
	event.stopPropagation();
	if (event.originalTarget.nodeName == 'SELECT') // restore old list box index
		event.originalTarget.selectedIndex = g_oldSelectedIndex;
	if (event.originalTarget.type == 'checkbox')
	{
		// TODO: is there a better way here?
		// the preventDefault doesn't immediately reset the checkbox, so we're going to do it so that
		// the sendValueMessage below sends the right value.
		event.originalTarget.checked = !event.originalTarget.checked;
	}
	// resend the new value
	catcher.sendValueMessage(false, event.originalTarget, event);
}

function openPopup(event, jitData)
{
	g_blockJITs = true; // we're about to steal focus
	var xmsg = getXMLMessage(jitData);
	var htmlPage = getMessageHTML(xmsg);
	var page = "";
	var width = 300, height = 200;
	page += '<?xml version="1.0"?>';
	page += '<?xml-stylesheet href="chrome://global/skin/" type"text/css" ?>';
	page += '<?xml-stylesheet href="chrome://browser/skin/browser.css" type="text/css" ?>';
	page += '<window id="JIT" title="JIT" xmlns="http://www.mozilla.org/keymaster/gatekeeper/there.is.only.xul" hidechrome="true">';
	page += '<iframe id="frameid" src="data:text/html;base64,' + btoa(htmlPage) + '" flex="1"/>';
	page += '</window>';
	var el = event.originalTarget;
	/*var pos;
	pos = findPos(el);
	if (pos)
	{
		var r = getVisiblePosition(pos[0], pos[0] + pos[2], pos[1], pos[1] + pos[3], width, height);
		window.openDialog("data:text/xml," + page, 'JIT', 'chrome,modal,width=' + width + ',height=' + height + ',left=' + r[0] + ',top=' + r[1]);
	}
	else
	*/	window.openDialog("data:text/xml," + page, 'JIT', 'chrome,modal,width=' + width + ',height=' + height + ',centerscreen=yes');

	logToServer('CLOSE JIT');
	if (isBlock(xmsg))
		block(event);
	g_blockJITs = false;
}

function logToServer(msg)
{
	$.post(g_webTREURL + "/WebxPST", {'trename': mytre, 'command': 'log', 'msg': msg});
}

function Listener()
{
	this.catcher = new EventCatcher();
	this.register();
	this.dummy = "hello";
}

Listener.prototype =
{
	observe: function(subject, topic, data)
	{
		if (topic == "domwindowopened")
		{
			if (subject.location != null)
			{
				var objScriptLoader = subject.Components.classes["@mozilla.org/moz/jssubscript-loader;1"].getService(Components.interfaces.mozIJSSubScriptLoader);
				objScriptLoader.loadSubScript("chrome://webxpst/content/dormin.js");
				subject.addEventListener("load", this.catcher.makeDelayedSelect(subject), false);
			}
		}
		else
		{
			// dump("error");
		}
	},
	register: function()
	{
		var observerService = Components.classes["@mozilla.org/observer-service;1"].getService(Components.interfaces.nsIObserverService);
		observerService.addObserver(this, "domwindowopened", false);
	},
	unregister: function()
	{
		var observerService = Components.classes["@mozilla.org/observer-service;1"].getService(Components.interfaces.nsIObserverService);
		observerService.removeObserver(this, "domwindowopened", false);
	},
	QueryInterface: function(aIID)
	{
		if (aIID.equals(Components.interfaces.nsISupports) || aIID.equals(Components.interfaces.nsIObserver)) return this;
		throw Components.results.NS_NOINTERFACE;
	}
}
function startEventCatcher()
{
	if (catcher == null)
	{
		catcher = new EventCatcher();
		catcher.startSelectByClick(window, "initialize");
	
		// TODO: need to respond to DOM mutation events, etc., but for now just do it
		// on a timer
		window.setInterval('catcher.sendPendingQIVs()', 300);
	}
}

function stopEventCatcher()
{
	document.getElementById('serverdetails').setAttribute('collapsed', true);
	catcher.removeClickListeners();
	windowListener.unregister();
}

function displayScenario(xml)
{
	var scenxml = null;
	try
	{
		scenxml = xslTransform.load(getBase() + xml);
	}
	catch (e)
	{
	}
	if (scenxml == null || typeof(scenxml) != 'object' || scenxml.documentElement.nodeName == 'parsererror')
		scenxml = xslTransform.load('<message><p>Scenario XML couldn\'t be parsed.</p></message>');

	var basetag = scenxml.createElement('base');
	var skinbasetag = scenxml.createElement('skinbase');
	var scenariotag = scenxml.createElement('isscenario');
	basetag.setAttribute('href', getBase());
	skinbasetag.setAttribute('href', getSkinBase());
	scenxml.documentElement.appendChild(basetag);
	scenxml.documentElement.appendChild(skinbasetag);
	scenxml.documentElement.appendChild(scenariotag);
	$('#scenario').attr('src', 'data:text/html;base64,' + btoa(getMessageHTML(scenxml)));
}

function onStart()
{
	startEventCatcher();
	var params = "command=starttre&task=" + g_taskName;
	var xh = new XMLHttpRequest();
	xh.open("POST", g_webTREURL + "/WebxPST", false);
	xh.setRequestHeader("Content-type", "application/x-www-form-urlencoded");
	xh.setRequestHeader("Content-length", params.length);
	xh.setRequestHeader("Connection", "close");
	xh.send(params);
	var resp = xh.responseText;
	var brk = resp.indexOf(' ');
	if (brk == -1)
		mytre = resp;
	else
	{
		mytre = resp.substring(0, brk);
		var errmsg = resp.substring(brk + 1);
		if (errmsg.length > 0)
			alert(errmsg);
	}
	g_isTutorRunning = true;
	sendTutorMessage('', null); // Get initial messages from tutor

	displayScenario(g_taskName + '-scenario.xml');
	document.getElementById('taskselect').collapsed = true;
	document.getElementById('tutorstuff').collapsed = false;
	//document.getElementById('serverdetails').collapsed = false;
	
}

var hintNumber = 0;
var hintMsg;
var hintLocation;

function onHint(event)
{
	var msg1 = new DorminMessage("", "GETHINT", "answer");
	var hintWindow = openDialog('chrome://webxpst/content/hintWindow.xul', 'Hint', 'chrome,modal,width=310, height=220',
		sendTutorMessage(msg1.MakeString(), null), hintLocation);
	logToServer('CLOSE HINT');
	g_blockJITs = false;
}

function onDone(event)
{
	var msg1 = new DorminMessage("TutorLink.Done", "NOTEVALUESET", "1");
	sendTutorMessage(msg1.MakeString(), event);
	if(g_isTutorRunning)
		alert("You are not done with this problem yet. Ask for a hint if you would like help.")
}

function stop()
{
	g_isTutorRunning = false;
	var params = "command=stoptre&trename=" + mytre;
	var xh = new XMLHttpRequest();
	xh.open("POST", g_webTREURL + "/WebxPST", false);
	xh.setRequestHeader("Content-type", "application/x-www-form-urlencoded");
	xh.setRequestHeader("Content-length", params.length);
	xh.setRequestHeader("Connection", "close");
	xh.send(params);
	mytre = null;
	setButtonsDisabled(true);
//	document.getElementById('tutorstuff').collapsed = true;
//	document.getElementById('taskselect').collapsed = false;
//	document.getElementById('serverdetails').collapsed = true;
}

function goQIV(msg)
{
	var xh = new XMLHttpRequest();
	xh.open("POST", g_webTREURL + "/WebxPST/" + mytre, false);
	xh.send(sendMsg);
}

function sendTutorMessage(sendMsg, event)
{
	if (!g_isTutorRunning)
		return false;

	var loop = true;// while there are more message(s)
	var xh = new XMLHttpRequest();
	xh.open("POST", g_webTREURL + "/WebxPST/" + mytre, false);
	xh.send(sendMsg);
	var message = xh.responseText;
	var msg = DorminMsgFromString(message);
	var rtnValue = null;

	while (loop)
	{
		loop = false;
		if (msg.strVerb == "APPROVE")
		{
			//Visual feedback (green checks)		
			var addrarr = msg.DorminAddr.strArrNames;
			
			if(addrarr[0].charAt(0)=="1" && addrarr[1].search('_')!=-1)
			{
				//Jotform Radiobutton
				//NW 1814629371.radioGroup-q22_inEnglish
				//W  1994821566.radioGroup-q7_radioButton
				var temp1 = addrarr[1].substr(addrarr[1].indexOf("radioGroup-q") + 12);
				var temp2 = temp1.substr(0,temp1.indexOf("_")) + "_0";
				var widget_goal = "input_" + temp2;
			}
			else
			{
				var widget_goal = addrarr[0];
			}	
			
			//Widget Element		
			var wElement = content.document.getElementById(widget_goal);	
			var posX = 0;
  			var posY = 0;              
  			while(wElement != null){
    			posX += wElement.offsetLeft;
    			posY += wElement.offsetTop;
    			wElement = wElement.offsetParent;
  			}
  			
  			//Image Element		 	
  			posX = posX - 20 + "px";
  			posY = posY + "px";  
  			
 			iElement = content.document.createElement("img");
 			iElement.src = "http://xpst.vrac.iastate.edu/xPSTImages/tick2.gif";
			iElement.style.position = "absolute";
			iElement.style.left = posX;
			iElement.style.top = posY;
			iElement.style.zIndex = 100 + "px";			
 			content.document.body.appendChild(iElement);
 			var x_dummy = iElement.style.left;
 					
			//Remove Red Xs
			var removeElement = content.document.getElementById("redXimage");
			while(removeElement)
			{
				removeElement.parentNode.removeChild(removeElement);
				var removeElement = content.document.getElementById("redXimage");
			}
			
			if (addrarr[0] == 'TutorLink' && addrarr[1] == 'Done')
			{
				alert("Congratulations.You have succesfully completed the task.");
			    var taskel = document.getElementById('task');
				if (taskel != null && taskel.childNodes.length > 1)
			    {
					stop();
					onLoad();
			    }
			    else
			    {
					stop();
					//document.getElementById('tutorstuff').collapsed = true;
					//document.getElementById('taskselect').collapsed = true;
					//document.getElementById('waitingtext').setAttribute('collapsed',false);
			    }
			}
			
			if(msg.arrParameters.length = 1)
			{
				if(msg.arrParameters[0].strName == "MESSAGE")
				{
					alert(msg.arrParameters[0].objValue.value[0].value);
				}
			}
		}
		else if (msg.strVerb == "JITMESSAGE")
		{
			if (!g_blockJITs)
				openPopup(event, msg.arrParameters[0].objValue.value[0].value);
			// return false;
		}
		else if (msg.strVerb == "HINTMESSAGE")
		{
			var tempHint;
			for (tempHint in msg.arrParameters)
			{
				if (msg.arrParameters[tempHint].strName == "MESSAGE")
				{
					hintLocation = tempHint;
					break;
				}
			}
			rtnValue = msg;
			// return msg;
		}
		else if (msg.strVerb == "FLAG")
		{
			//Visual feedback (Red Xs)		
			//Remove existing Red Xs to prevent overlapping
			var removeElement = content.document.getElementById("redXimage");
			while(removeElement)
			{
				removeElement.parentNode.removeChild(removeElement);
				var removeElement = content.document.getElementById("redXimage");
			}
			
			var addrarr = msg.DorminAddr.strArrNames;
			
			if(addrarr[0].charAt(0)=="1" && addrarr[1].search('_')!=-1)
			{
				var temp1 = addrarr[1].substr(addrarr[1].indexOf("radioGroup-q") + 12);
				var temp2 = temp1.substr(0,temp1.indexOf("_")) + "_0";
				var widget_goal = "input_" + temp2;
			}
			else
			{
				var widget_goal = addrarr[0];
			}	
			
			//Widget Element
			var wElement = content.document.getElementById(widget_goal);
			var posX = 0;
  			var posY = 0;              
  			while(wElement != null){
    			posX += wElement.offsetLeft;
    			posY += wElement.offsetTop;
    			wElement = wElement.offsetParent;
  			}
  			
  			//Image Element		 	
  			posX = posX - 20 + "px";
  			posY = posY + "px";  
 			iElement = content.document.createElement("img");
 			iElement.src = "http://xpst.vrac.iastate.edu/xPSTImages/cross1.png";
 			iElement.id = "redXimage";
			iElement.style.position = "absolute";
			iElement.style.left = posX;
			iElement.style.top = posY;
			iElement.style.zIndex = 98 + "px";			
 			content.document.body.appendChild(iElement);
 			var x_dummy = iElement.style.left;
 			//alert(iElement.id);
		}
		else if (msg.strVerb == "GETHINT")
		{
		}
		else if (msg.strVerb == "SETPROPERTY")
		{
		}
		else if (msg.strVerb == "QUERYINITIALVALUE")
		{
			var namearr = msg.DorminAddr.strArrNames;
			catcher.sendQIV(namearr);
		}

		var nextMsgLocation = message.indexOf('\n');// check if there is another message
		if (nextMsgLocation !== message.length - 1)
		{
			loop = true;
			nextMsgLocation++;
			message = message.substring(nextMsgLocation);
			var msg = DorminMsgFromString(message);
		}
	}
	return rtnValue;
}

function start()
{
	if(document.getElementById('task').selectedIndex==-1)
	alert("Please select a task in the Tasklist and then click on Start.");
	else
	{
		g_taskName = document.getElementById('task').selectedItem.value;
		onStart();
	}
}

function onOptions()
{
	window.open('chrome://webxpst/content/taskselect.xul', 'options', 'chrome,modal');
	onStart();
}

function onHintMouseDown()
{
	g_blockJITs = true; // the click on the hint button will steal focus, but we
	// don't want to JIT. We'll unblock when the hint box is
	// dismissed.
}

function completeCurrentGoalnode()
{
	logToServer('FORCING COMPLETION');
	var appns = $.ajax({type: 'GET', url: g_webTREURL + '/WebxPST/' + mytre + '/appnodes', async: false, dataType: 'xml'});
	var nextnode = $('appnode[isnext=true]', appns.responseXML);
	
	
	
	
	if (nextnode.length == 0)
		alert('couldn\'t find next node');
	else
	{
		var gn = nextnode.attr('goalnodename');
		var appn = nextnode.attr('name');
		var repval = $.ajax({type: 'POST', data: {'trename': mytre, 'command': 'getrepresentativevalue', 'goalnode': gn}, url: g_webTREURL + '/WebxPST', async: false, dataType: 'text'}).responseText;

		var dm = new DorminMessage(appn, 'NOTEVALUESET', repval);
		sendTutorMessage(dm.MakeString(), null);
	}	
	
}

function onKeypress(evt)
{
	if (evt.keyCode == 121) // Ctrl-F10
	{
		g_isTutorRunning = !g_isTutorRunning;
		logToServer(g_isTutorRunning ? 'TUTOR UNSUSPENDED' : 'TUTOR SUSPENDED');
		$('#suspended').attr('collapsed', g_isTutorRunning);
		$('#ctrlbuttons').attr('collapsed', !g_isTutorRunning);
		$('#serverdetails').attr('collapsed',true);
	}
}
