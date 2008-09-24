/*javascript for Bubble Tooltips by Alessandro Fulciniti
- http://pro.html.it - http://web-graphics.com */

function enableTooltips(id, topElem){
  var links,i,h;
  if(!document.getElementById || !document.getElementsByTagName){
    return;
  }
  AddCss(topElem);
  h=document.createElement("span");
  h.id="";
  h.setAttribute("id","btc");
  h.style.position="absolute";
  topElem.getElementsByTagName("body")[0].appendChild(h);
  //document.getElementsByTagName("body")[0].appendChild(h);
  if(id==null){
    links=topElem.getElementsByTagName("a");
    //links=document.getElementsByTagName("a");
  }
  else{
    links=topElem.getElementById(id);//.getElementsByTagName("a");
    //links=document.getElementById(id).getElementsByTagName("a");
  }
  Prepare(links, topElem, h);
  /*for(i=0;i<links.length;i++){
      Prepare(links[i]);
  }*/
}

function Prepare(el, topElem, jit){
  var tooltip,t,b,s,l;
  t=el.getAttribute("title");
  if(t==null || t.length==0){
    t="You are ";
  }
  el.removeAttribute("title");
  tooltip=CreateEl("span","tooltip");
  s=CreateEl("span","top");
  s.appendChild(topElem.createTextNode(t));
  //s.appendChild(document.createTextNode(t));
  tooltip.appendChild(s);
  b=CreateEl("b","bottom");
  /*l=el.getAttribute("href");
  if(l.length>30){
    l=l.substr(0,27)+"...";
  }*/
  l = "here.";
  b.appendChild(topElem.createTextNode(l));
  //b.appendChild(document.createTextNode(l));
  tooltip.appendChild(b);
  setOpacity(tooltip);
  el.tooltip=tooltip;
  //dump(el);
  //dump(el.onmouseov);
  el.addEventListener("mouseover", 
    function (e){
      //dump("in function");
      //jit.appendChild(this.tooltip);
      jit.appendChild(tooltip);
      //document.getElementById("btc").appendChild(this.tooltip);
      var posx=0,posy=0;
      if(e==null){
        dump("null event\n");
        e=window.event;
      }
      if(e.pageX || e.pageY){
          posx=e.pageX; posy=e.pageY;
      }
      else if(e.clientX || e.clientY){
          if(document.documentElement.scrollTop){
              posx=e.clientX+document.documentElement.scrollLeft;
              posy=e.clientY+document.documentElement.scrollTop;
              }
          else{
              posx=e.clientX+document.body.scrollLeft;
              posy=e.clientY+document.body.scrollTop;
          }
      }
      jit.style.top=(posy+10)+"px";
      jit.style.left=(posx-20)+"px";
      //document.getElementById("btc").style.top=(posy+10)+"px";
      //document.getElementById("btc").style.left=(posx-20)+"px";
      //Locate(e);
    },
    true );

  el.addEventListener("mouseout",
    function (e)
    {
      //hideTooltip.apply(el, [e]);
      //var d=this.document.getElementById("btc");
      //if(d.childNodes.length>0){
      //  d.removeChild(d.firstChild);
      //}
      jit.removeChild(jit.firstChild);
    }, 
    true );
  el.addEventListener("mousemove",
    function (e){
      var posx=0,posy=0;
      if(e==null){
        e=window.event;
      }
      if(e.pageX || e.pageY){
          posx=e.pageX; posy=e.pageY;
      }
      else if(e.clientX || e.clientY){
          if(document.documentElement.scrollTop){
              posx=e.clientX+document.documentElement.scrollLeft;
              posy=e.clientY+document.documentElement.scrollTop;
              }
          else{
              posx=e.clientX+document.body.scrollLeft;
              posy=e.clientY+document.body.scrollTop;
          }
      }
      jit.style.top = (posy+10)+"px";
      jit.style.left =(posx-20)+"px"
      //this.document.getElementById("btc").style.top=(posy+10)+"px";
      //this.document.getElementById("btc").style.left=(posx-20)+"px";
    }, 
    true );
  /*el.onmouseover=showTooltip;
  el.onmouseout=hideTooltip;
  el.onmousemove=Locate;*/
}

function showTooltip(e){
  /*if(this.document == document)
  {
    dump("true");
  }
  else 
  {
    dump("false");
  }*/
  //this.document.getElementById("btc").appendChild(this.tooltip);
  jit.appendChild(this.tooltip);
  Locate(e. jit);
}

function hideTooltip(e){
  var d=document.getElementById("btc");
  if(d.childNodes.length>0){
    d.removeChild(d.firstChild);
  }
  jit.removeChild(jit.firstChild);
}

function setOpacity(el){
  el.style.filter="alpha(opacity:95)";
  el.style.KHTMLOpacity="0.95";
  el.style.MozOpacity="0.95";
  el.style.opacity="0.95";
}

function CreateEl(t,c){
  var x=document.createElement(t);
  x.className=c;
  x.style.display="block";
  return(x);
}

//todo: change method so that calling it twice won't add another "link" element
function AddCss(topElem){
  var l=CreateEl("link");
  l.setAttribute("type","text/css");
  l.setAttribute("rel","stylesheet");
  l.setAttribute("href","bt.css");
  l.setAttribute("media","screen");
  topElem.getElementsByTagName("head")[0].appendChild(l);
  //document.getElementsByTagName("head")[0].appendChild(l);
}

function Locate(e, jit){
  var posx=0,posy=0;
  if(e==null){
    e=window.event;
  }
  if(e.pageX || e.pageY){
      posx=e.pageX; posy=e.pageY;
  }
  else if(e.clientX || e.clientY){
      if(document.documentElement.scrollTop){
          posx=e.clientX+document.documentElement.scrollLeft;
          posy=e.clientY+document.documentElement.scrollTop;
          }
      else{
          posx=e.clientX+document.body.scrollLeft;
          posy=e.clientY+document.body.scrollTop;
      }
  }
  jit.style.top=(posy+10)+"px";
  jit.style.left=(posx-20)+"px";
  //document.getElementById("btc").style.top=(posy+10)+"px";
  //document.getElementById("btc").style.left=(posx-20)+"px";
}
