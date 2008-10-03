#!/bin/bash

RELEASE=../release

./getandbuild.sh
rm -rf $RELEASE
mkdir -p $RELEASE/src
cp -r * $RELEASE/src
find $RELEASE/src -type d -path '*/.svn' -prune -print0 | xargs -0 rm -rf
rm -rf $RELEASE/src/builds
cp builds/WebxPST/updates.rdf builds/WebxPST/WebxPST.war builds/WebxPST/WebxPST.xpi $RELEASE/
GCVer=`svn info | grep "Last Changed Rev" | cut -d' ' -f4`
(cd $RELEASE; zip -9rm xPST-release-0.3.$GCVer.zip *)
