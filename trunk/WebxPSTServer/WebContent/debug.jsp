<!--
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
-->

<?xml version="1.0" encoding="ISO-8859-1" ?>
<%@ page language="java" contentType="text/html; charset=ISO-8859-1" pageEncoding="ISO-8859-1"%>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
	<meta http-equiv="Content-Type" content="text/html; charset=ISO-8859-1" />
	<title>WebTRE debugging</title>
	<link rel="stylesheet" href="jquery/themes/flora/flora.all.css" type="text/css" media="screen" title="Flora (Default)"/>
	<script type="text/javascript" src="jquery/jquery.js"></script>
	<script type="text/javascript" src="jquery/ui.tablesorter.js"></script>
	<script type="text/javascript" src="jquery/jquery.dimensions.js"></script>
	<script type="text/javascript" src="jquery/ui.mouse.js"></script>
	<script type="text/javascript" src="jquery/ui.resizable.js"></script>
	<script type="text/javascript" src="dormin.js"></script>
<script type="text/javascript">
var trename = window.location.search.substring(4);
var lastlogpos = 0;
var tutorName = null;
var goalNodeToAppNode = {};

function gotlog(data, status)
{
	var logdoc = $('#logframe')[0].contentWindow.document;
	var log = $('#log > tbody', logdoc);
//	$(log).empty();
	$(data).find('record').each(
		function()
		{
			var tr = $('<tr></tr>');
			$(tr).append($('<td></td>').text($(this).attr('time')));
			$(tr).append($('<td></td>').text($(this).attr('friendlymessage')));
			$(tr).append($('<td></td>').text($(this).attr('fullmessage')));
			$(log).append(tr);
			lastlogpos++;
		}
	);
}

function gotSubmitResponse(data)
{
	getAppNodes();
}

function submit(gn, data)
{
	var msg;
	var addr = goalNodeToAppNode[gn];
	var dmsg = new DorminMessage(addr, 'NOTEVALUESET', data);
	msg = dmsg.MakeString();
	
	$.ajax(
		{
			url: 'WebTRE/' + trename,
			type: 'POST',
			data: msg,
			success: gotSubmitResponse
		}
	);
}

function clickedgn()
{
	var goalnode = $(this).text();
	$.post('WebTRE', {'trename': trename, 'command': 'getrepresentativevalue', 'goalnode': goalnode},
		function(data)
		{
			submit(goalnode, data);
		}
	);
}

function gotemdebug(data, status)
{
	var emdoc = $('#emdebugframe')[0].contentWindow.document;
	var em = $('#em', emdoc);
	$(em).empty();
	$(em).append(data.documentElement);
	var gns = $(em).find('.MappedEnabledGoalNode,.MappedDisabledGoalNode,.UnmappedEnabledGoalNode,.UnmappedDisabledGoalNode');
	gns.click(clickedgn);
	gns.css("cursor", "pointer");
}

function dorefresh()
{
	$.get('WebTRE/' + trename + '/log?pos=' + lastlogpos, gotlog);
	$.get('WebTRE/' + trename + '/emdebug', gotemdebug);
	setTimeout(dorefresh, 500);
}

function gotTutorName(data)
{
	tutorName = data;
}

function gotAppNodes(data)
{
	$(data).find('appnode').each(
		function()
		{
			var appn = $(this).attr('name');
			var gn = $(this).attr('goalnodename');
			goalNodeToAppNode[gn] = appn;
		}
	);
}

function getAppNodes()
{
	$.get('WebTRE/' + trename + '/appnodes', gotAppNodes);
}

$(document).ready(
	function()
	{
		$.get('WebTRE/' + trename + '/tutorname', gotTutorName);
		getAppNodes();
		setTimeout(dorefresh, 500);   // I tried actually using .ready on the iframes and it wasn't working out... hope this does.
	}
);
</script>
</head>
<body style="padding:0;margin:0">
	<iframe id="logframe" src="logtemplate.html" style="padding:0;margin:0" width="100%" height="300px"></iframe>
	<iframe id="emdebugframe" src="emdebugtemplate.html" style="padding:0;margin:0" width="100%" height="300px"></iframe>
</body>
</html>
