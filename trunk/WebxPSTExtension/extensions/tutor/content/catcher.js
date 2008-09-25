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

var objScriptLoader = Components.classes["@mozilla.org/moz/jssubscript-loader;1"].getService(Components.interfaces.mozIJSSubScriptLoader);
objScriptLoader.loadSubScript("chrome://webxpst/content/dormin.js");

var objScriptLoader2 = Components.classes["@mozilla.org/moz/jssubscript-loader;1"].getService(Components.interfaces.mozIJSSubScriptLoader);
objScriptLoader2.loadSubScript("chrome://webxpst/content/BubbleTooltips.js");
const nsISupports = Components.interfaces.nsISupports;
const nsIObserver = Components.interfaces.nsIObserver;
const nsIHttpChannel = Components.interfaces.nsIHttpChannel;
const observerService = Components.classes["@mozilla.org/observer-service;1"].getService(Components.interfaces.nsIObserverService);
const OB_HOMR = 'http-on-modify-request';
const OB_HOER = 'http-on-examine-response';
const OB_DWC = 'domwindowclosed';
const OB_DWO = 'domwindowopened';
const VANTH_HTML_EDITOR = /.*?vanth.org\/.*?\/HtmlEditor.html/;
var g_vanthEditorPrevValue = null;
var g_vanthEditorAssociatedNode = null;
var g_vanthEditorAssociations =
{
		'editPreface': ['ProblemStatement'],
		'EditBefore': ['QuestionText'],
		'EditAfter': ['TextAfter'],
		'EditFeedback': ['AttemptFeedback'],
		'EditText': ['QuestionText'],
		'RichEditChoice': ['ChoiceText']
};

var lastFocus = null;
var restartClick = false;
var clickTarget = null;

var g_inEvent = false;
var g_newQIVs = [];
var g_requestQIVs = null;

function CSObserver()
{
	this.wrappedJSObject = this;
}

CSObserver.prototype = 
{
	QueryInterface : function(aIID)
	{
		if (aIID.equals(Components.interfaces.nsIObserver) ||
			aIID.equals(Components.interfaces.nsISupportsWeakReference) ||
			aIID.equals(Components.interfaces.nsISupports))
					return this;
		throw Components.results.NS_NOINTERFACE;
	},

	observe: function(subject, topic, data)
	{
		if (topic == OB_DWC)
		{
			var brs = subject.document.getElementsByTagNameNS(XULNS, 'tabbrowser');
			if (brs.length > 0 && g_vanthEditorAssociatedNode)
			{
				if (brs[0].contentWindow.location.href.match(VANTH_HTML_EDITOR))
				{
					if (g_vanthEditorAssociatedNode != null && g_vanthEditorAssociatedNode.value != g_vanthEditorPrevValue)
					{
						var evt = {'type': 'change', 'originalTarget': g_vanthEditorAssociatedNode};
						catcher.sendValueMessage(false, g_vanthEditorAssociatedNode, evt);
					}
				}
			}
		}
		else if (topic == OB_HOMR || topic == OB_HOER)
		{
			var request = subject.QueryInterface(nsIHttpChannel);
			var host = request.getRequestHeader('host');
	
			// TODO: hack!
			if (host.indexOf('vanth') != -1)
			{
				if (topic == OB_HOMR)
				{
					if (g_inEvent)
					{
						if (g_requestQIVs)
							g_requestQIVs = g_requestQIVs.concat(g_newQIVs);
						else
							g_requestQIVs = g_newQIVs;
						g_newQIVs = [];
					}
				}
				else if (topic == OB_HOER)
				{
					// TODO: want to associate the QIVs with specific requests, but the request objects are not hashably-unique.
					if (g_requestQIVs)
					{
						for (var i in g_requestQIVs)
							setTimeout(hackSendQIVNow, 300, [g_requestQIVs[i]]);
						g_requestQIVs = null;
					}
				}
			}
		}
	}
};

function hackSendQIVNow(namearr)  // TODO: sendQIVNow and others should be functions instead of methods
{
	catcher.sendQIVNow(namearr);
}

function EventCatcher()
{
	var obs = new CSObserver();
	observerService.addObserver(obs, OB_HOMR, false);
	observerService.addObserver(obs, OB_HOER, false);
	observerService.addObserver(obs, OB_DWC, false);
	observerService.addObserver(obs, OB_DWO, false);
}

EventCatcher.prototype =
{
	makeDelayedSelect: function(targetWindow)
	{
		var thisUp = this;
		return function(event)
		{
			thisUp.startSelectByClick(targetWindow, "");
		}
	},

	addListeners: function(el, type)
	{
		el.addEventListener(type, this.eventDispatcher, true);
		el.addEventListener(type, this.afterEventDispatcher, false);
	},
	
	removeListeners: function(el, type)
	{
		el.removeEventListener(type, this.eventDispatcher, true);
		el.removeEventListener(type, this.afterEventDispatcher, false);
	},
	
	startSelectByClick: function(targetWindow, astring)
	{
		// this.mSelectDocs = getAllDocuments(targetWindow);
		this.mSelectDocs = [targetWindow.top.document];

		for (var i = 0; i < this.mSelectDocs.length; ++i)
		{
			this.mSelectDocs[i].addEventListener('dblclick',
				function(event)
				{
					event.stopPropagation();
					event.preventDefault();
				}
			, true);
			var el = this.mSelectDocs[i];
			this.addListeners(el, 'mousedown');
			this.addListeners(el, 'click');
			this.addListeners(el, 'change');
		}
	},

	sendValueMessage: function(isinitial, node, event)
	{
		if (node.namespaceURI == XULNS)
			return false;
		if (node.ownerDocument.documentElement.namespaceURI == XULNS)
			return false;

		var path = this.getPath(node);

		var sent = false;

		if (g_vanthUsername == null && event != null)
		{
			var ref = event.originalTarget.ownerDocument.location.href;
			const getuname = /.*?quotalink_.*?_.*?_(.*?)~(.*?)~/;
			var matches = ref.match(getuname);
			if (matches)
			{
				g_vanthUsername = matches[1];
				logToServer('USERNAME:' + g_vanthUsername);
			}
		}
		
		if (event != null && event.type == 'click' && g_vanthEditorAssociations[path] != null)
		{
			g_vanthEditorAssociatedNode = this.getNode(g_vanthEditorAssociations[path]);
			if (g_vanthEditorAssociatedNode != null)
				g_vanthEditorPrevValue = g_vanthEditorAssociatedNode.value;
		}
		
		if (g_observeIDs && event.type == 'click')
		{
			var list = document.getElementById('idlist');
			list.value = list.value + path + '\n';
		}

		if (event != null && event.type == 'mousedown')
		{
			// remember old index so we can restore it in a block JIT
			if (event.originalTarget.nodeName == 'SELECT' && (event.originalTarget.size == 0 || event.originalTarget.size == 1))
				g_oldSelectedIndex = event.originalTarget.selectedIndex;
			if (event.originalTarget.nodeName == 'OPTION' && event.originalTarget.parentNode.size > 1)
				g_oldSelectedIndex = event.originalTarget.parentNode.selectedIndex;
		}

		var value = null, pathsuffix = '';
		if ((node.nodeName == "INPUT" || node.nodeName == "TEXTAREA") && (node.type == "text" || node.type == "textarea") && (isinitial || event.type == "change"))
			value = node.value;
		else if ((node.nodeName == "INPUT" && node.type == "file") && (isinitial || event.type == "change"))
			value = node.value;
		else if (node.type == "checkbox" && (isinitial || event.type == 'click'))
			value = node.checked.toString();
		else if ((node.nodeName == "INPUT" && node.type == "radio") && (isinitial || event.type == 'change'))
		{
			// Radio buttons are special. We have to name their paths differently than everything else
			var groupspace = null;
			if (node.form)
				groupspace = this.getPath(node.form);
			else
			{
				// TODO: this is a hack, but we try to do _something_ when the radio buttons are not part of a form.
				groupspace = this.getPath(node.ownerDocument.documentElement);
			}
			var group = node.name;
			path = groupspace + '.radioGroup-' + group;
			value = node.value;
		}
		else if (node.nodeName == "SELECT" && (isinitial || event.type == 'change'))
		{
			if (node.selectedIndex > -1)
			{
				var optnode = node.options[node.selectedIndex];
				var nodeText = optnode.lastChild.nodeValue;
				while (nodeText.charAt(nodeText.length - 1) == " " || nodeText.charAt(nodeText.length - 1) == "\n")
				{
					nodeText = nodeText.substring(0, nodeText.length - 1);
				}
				value = nodeText;
			}
			else
				value = nodeText;
		}
		else if (node.nodeName == "OPTION")
			; // do nothing, let select case above handle it
		else if (node.className != undefined && node.className.substr(0, 4) == "leaf" && event.type == 'click')
			value = node.lastChild.nodeValue;
		else if (!isinitial && event.type == 'click')
		{
			pathsuffix = ':click';
			value = '1';
		}

		if (value != null)
		{
			var msg = new DorminMessage(path + pathsuffix, isinitial ? 'NOTEINITIALVALUE' : 'NOTEVALUESET', value);
			sendTutorMessage(msg.MakeString(), event);
			sent = true;
		}

		return sent;
	},

	getPath: function(node)
	{
		var path = "";
		var children = 0;
		var parents = 0;
		var idFound = false;

		var parent;
		if (node.nodeName == "OPTION")
		{
			parent = node.parentNode;
		}
		else if (node.className != undefined && node.className.substr(0, 4) == "leaf")
		{
			parent = node.parentNode.parentNode;
		}
		else
		{
			parent = node;
		}

		while (parent != null)
		{
			if (!idFound && parent.id != "" && parent.id != undefined)
			{
				if (path != "")
				{
					path = parent.id + "." + path;
				}
				else
				{
					path = parent.id;
				}
				idFound = true;
			}
			children = 0;
			previousChild = parent.previousSibling;

			while (previousChild != null)
			{
				previousChild = previousChild.previousSibling;
				children++;
			}
			if (!idFound && parents != 0)
			{
				path = "." + path;
			}

			if (parent.parentNode == null)
			{
				if (!idFound && parent.defaultView != undefined && parent.defaultView.frameElement != null)
				{
					path = "contentDocument" + path;
				}
			}
			else if (parent.parentNode.tagName == "browser")
			{
				if (!idFound)
				{
					path = parent.tagName + path;
				}
			}
			else
			{
				if (!idFound)
				{
					path = "childNodes-" + children + path;
				}
			}

			if (parent.parentNode == null)
			{
				refElem = parent;
				if (parent.defaultView != null)
				{
					parent = parent.defaultView.frameElement;
				}
				else
				{
					parent = null;
				}
			}
			else
			{
				refElem = parent;
				parent = parent.parentNode;
			}
			parents++;
		}

		return path;
	},

	eventDispatcher: function(event)
	{
		g_inEvent = true;
		catcher.sendValueMessage(false, event.originalTarget, event);
	},

	processNewQIVs: function()
	{
		for (var i in g_newQIVs)
		{
			if (!this.sendQIVNow(g_newQIVs[i]))
				this.pendingQIVs.push(g_newQIVs[i]);
		}
		g_newQIVs = [];
	},

	afterEventDispatcher: function(event)
	{
		g_inEvent = false;
		catcher.processNewQIVs();
	},
	
	removeClickListeners: function()
	{
		for (var i = 0; i < this.mSelectDocs.length; ++i)
		{
			var el = this.mSelectDocs[i];
			this.removeListeners(el, 'click');
			this.removeListeners(el, 'mousedown');
			this.removeListeners(el, 'change');
		}
	},

	getNode: function(namearr)
	{
		var docs = getAllDocuments(window);
		var n = null;
		for (var j in docs)
		{
			var k;
			for (k = 0; k < namearr.length; k++)
			{
				if (k == 0)
					n = docs[j].getElementById(namearr[0]);
				else
				{
					var name = namearr[k];
					var dash = name.lastIndexOf('-');
					var cnnum = parseInt(name.substring(dash + 1));
					n = n.childNodes[cnnum];
				}
				if (n == null)
					break;
			}
			if (k == namearr.length && n != null)
				return n;
		}

		return null;
	},

	pendingQIVs: [],
	isNonValueNode: function(node)
	{
		var name = node.nodeName.toLowerCase();
		if (name == 'a')
			return true;
		if (name == 'input')
		{
			var type = node.getAttribute('type').toLowerCase();
			if (type == 'button')
				return true;
		}
		return false;
	},

	sendQIVNow: function(namearr)
	{
		var docs = getAllDocuments(window);
		for (var j in docs)
		{
			var n = null;
			for (var k in namearr)
			{
				if (k == 0)
					n = docs[j].getElementById(namearr[0]);
				else
				{
					var name = namearr[k];
					// special case for radio groups
					if (name.substring(0, 11) == 'radioGroup-')
					{
						var group = name.substring(11);
						var inps = n.getElementsByTagName('INPUT');
						for (var i in inps)
						{
							var inp = inps[i];
							if (inp.type == 'radio' && inp.name == group && inp.checked)
							{
								if (this.sendValueMessage(true, inp, null, null))
									return true;
							}
						}
					}
					else
					{
						var dash = name.lastIndexOf('-');
						var cnnum = parseInt(name.substring(dash + 1));
						n = n.childNodes[cnnum];
					}
				}
				if (n == null)
					break;
				else
				{
					if (!n.disabled)
					{
						var val = null;

						if (this.isNonValueNode(n))
							return true;
						else if (this.sendValueMessage(true, n, null, null))
							return true;
					}
				}
			}
		}

		return false;
	},

	sendPendingQIVs: function()
	{
		var todel = [];
		for (var i in this.pendingQIVs)
		{
			if (this.sendQIVNow(this.pendingQIVs[i]))
				todel.push(i);
		}

		while (todel.length > 0)
			delete this.pendingQIVs[todel.shift()];
	},

	sendQIV: function(namearr)
	{
//		if (!this.sendQIVNow(namearr))
//			this.pendingQIVs.push(namearr);
		 g_newQIVs.push(namearr);
	}
};

function getAllDocuments(targetWindow)
{
	var doc = targetWindow.top.document;
	var results = [doc];
	findDocuments(doc, results);
	return results;
}

function findDocuments(aDoc, aArray)
{
	addKidsToArray(aDoc.getElementsByTagName("frame"), aArray);
	addKidsToArray(aDoc.getElementsByTagName("iframe"), aArray);
	addKidsToArray(aDoc.getElementsByTagNameNS(XULNS, "browser"), aArray);
	addKidsToArray(aDoc.getElementsByTagNameNS(XULNS, "tabbrowser"), aArray);
	addKidsToArray(aDoc.getElementsByTagNameNS(XULNS, "editor"), aArray);
}

function addKidsToArray(aKids, aArray)
{
	for (var i = 0; i < aKids.length; ++i)
	{
		try
		{
			aArray.push(aKids[i].contentDocument);
			findDocuments(aKids[i].contentDocument, aArray);
		}
		catch (ex)
		{
		}
	}
}
