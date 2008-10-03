#!/bin/bash

RELEASE=../release

./getandbuild.sh
rm -rf $RELEASE
mkdir -p $RELEASE/src
cp -r * $RELEASE/src
find $RELEASE/src -type d -path '*/.svn' -prune -print0 | xargs -0 rm -rf
cp builds/WebxPST/updates.rdf builds/WebxPST/WebxPST.war builds/WebxPST/WebxPST.xpi $RELEASE/
(cd $RELEASE; zip -9rm release *)
