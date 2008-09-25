<?xml version="1.0"?>
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

<RDF:RDF xmlns:em="http://www.mozilla.org/2004/em-rdf#"
         xmlns:NC="http://home.netscape.com/NC-rdf#"
         xmlns:RDF="http://www.w3.org/1999/02/22-rdf-syntax-ns#">
  <RDF:Description RDF:about="urn:mozilla:install-manifest"
                   em:id="{7d8fe5ef-4859-42e7-abfd-3f2c5a30e46d}"
                   em:name="WebxPST"
                   em:version="@VERSION@"
                   em:description="WebxPST Tutoring System"
                   em:creator="Clearsighted"
                   em:homepageURL="http://www.clearsighted.org/"
                   em:updateURL="https://its.clearsighted.org/webtutor/updates.rdf">
    <em:targetApplication RDF:resource="rdf:#$ZxpaP3"/>
    <em:optionsURL>chrome://webxpst/content/TutorOptions.xul</em:optionsURL>
  </RDF:Description>
  <RDF:Description RDF:about="rdf:#$ZxpaP3"
                   em:id="{ec8030f7-c20a-464f-9b0e-13a3a9e97384}"
                   em:minVersion="2.0.0.0"
                   em:maxVersion="3.0.*.*" />
</RDF:RDF>
