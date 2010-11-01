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

var g_currentTasklist = null;
var g_currentTask = null;

function openTasklist(tasklist)
{
	var myurl = window.location.href;
	var webtreurl = myurl.substring(0, myurl.lastIndexOf('/') + 1);
	var evt = document.createEvent('Event');
	evt.initEvent('webxpst-open-tasklist', true, false);
	var tasklistel = document.createElement('tasklist');
	tasklistel.setAttribute('webtreurl', webtreurl);
	for (var i in tasklist)
	{
		var task = tasklist[i];
		var taskel = document.createElement('task');
		taskel.setAttribute('title', task[0]);
		taskel.setAttribute('name', task[1]);
		tasklistel.appendChild(taskel);
	}
	document.documentElement.appendChild(tasklistel);
	tasklistel.dispatchEvent(evt);
	document.documentElement.removeChild(tasklistel);
	
	g_currentTasklist = tasklist;
	window.addEventListener('webxpst-sidebar-opened', onWebxPSTSidebarOpened, false);
}

function onWebxPSTSidebarOpened(evt)
{
	if (g_currentTask != null)
		startTask(g_currentTask);
	if (g_currentTasklist != null)
		openTasklist(g_currentTasklist);
}

function disableTutorUI()
{
	var evt = document.createEvent('Event');
	evt.initEvent('webxpst-disable-tutor-ui', true, false);
	document.dispatchEvent(evt);
}

function startTask(task)
{
	var myurl = window.location.href;
	var webtreurl = myurl.substring(0, myurl.lastIndexOf('/') + 1);
	var evt = document.createEvent('Event');
	evt.initEvent('webxpst-start-task', true, false);
	var taskel = document.createElement('task');
	taskel.setAttribute('webtreurl', webtreurl);
	taskel.setAttribute('task', task);
	document.documentElement.appendChild(taskel);
	taskel.dispatchEvent(evt);
	document.documentElement.removeChild(taskel);

	g_currentTask = task;
	window.addEventListener('webxpst-sidebar-opened', onWebxPSTSidebarOpened, false);
}
