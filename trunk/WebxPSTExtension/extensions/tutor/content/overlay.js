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

var Tutor = {
    //////////////////////
    
    /*
    add event handler to window overlay onload maybe for sidebar?
    */
    //////////////////////
    //sideBarVisible: false,
    closePopup: function() {
        var popup = document.getElementById("jitPopup");
        popup.hidePopup();
    },
    onLoad: function() {
        //dump("load");
        // initialization code
        this.initialized = true;
        //dump("||||"+sidebarWindow+"||||\n");
        //var element = document.getElementById("sidebar");
        //dump(element+"||||||\n");
        //dump(element.contentWindow+"||||||\n");
        var sidebarWindow = document.getElementById("sidebar").contentWindow;
        // Verify that our sidebar is open at this moment:
        //dump("||||"+sidebarWindow+"||||\n");
        //dump("||||"+sidebarWindow.document.location.href+"||||\n");
        if (sidebarWindow.document.location.href == "chrome://webxpst/content/TutorSideBar.xul") {
            // call "yourNotificationFunction" in the sidebar's context:
            toggleSidebar('viewEmptySidebar');
        } 
    },
    
    onMenuItemCommand: function() {
        //dump(this.sideBarVisible);
        //if(!this.sideBarVisible){
        //    this.sideBarVisible = true;
            toggleSidebar('viewEmptySidebar');
        //}
        //window.open("chrome://webxpst/content/tutor.xul", "", "chrome");
        //var back = document.getElementById("Browser:Back");
        //back.setAttribute("disabled", true);
    },
    
    onclick: function(event) {
        if(event.cancelable){
            event.stopPropagation();
        }
        alert("here");
    }
};

window.addEventListener("load", function(e) { Tutor.onLoad(e); }, false); 
//window.document.addEventListener("onclick", function(e) { Tutor.onclick(e);}, true);