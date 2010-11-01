#!/bin/bash
ant=ant
export JAVA_HOME=/usr/lib/jvm/java-6-sun

svn up

NewxPSTLibVer=`svn info xPSTLib | grep "Last Changed Rev" | cut -d' ' -f4`
NewWebxPSTVer=`svn info WebxPSTServer | grep "Last Changed Rev" | cut -d' ' -f4`
NewPluginVer=`svn info WebxPSTExtension | grep "Last Changed Rev" | cut -d' ' -f4`
NewCogVer=`svn info CogModelling | grep "Last Changed Rev" | cut -d' ' -f4`
if [ $NewxPSTLibVer -gt $NewWebxPSTVer ]; then NewWebxPSTVer=$NewxPSTLibVer; fi
if [ $NewCogVer -gt $NewWebxPSTVer ]; then NewWebxPSTVer=$NewCogVer; fi

if [ -f ./builtversions.txt ]; then source ./builtversions.txt; else BuiltWebxPSTVer=0; BuiltPluginVer=0; BuiltCogVer=0; fi

cd WebxPSTExtension/extensions
if [ $NewPluginVer -gt $BuiltPluginVer ]; then if $ant -Dversion=$NewPluginVer; then BuiltPluginVer=$NewPluginVer; else echo "bad build"; fi fi
cd ../..

BuiltCogVer=$NewCogVer

cd WebxPSTServer
if [ $NewWebxPSTVer -gt $BuiltWebxPSTVer ]; then if $ant; then BuiltWebxPSTVer=$NewWebxPSTVer; else echo "bad build"; fi fi
cd ..

if [ -f ./builtversions.txt ]; then rm builtversions.txt; fi
echo "BuiltWebxPSTVer=$BuiltWebxPSTVer" >> builtversions.txt
echo "BuiltPluginVer=$BuiltPluginVer" >> builtversions.txt
echo "BuiltCogVer=$BuiltCogVer" >> builtversions.txt

#svn commit -m "automatic build" builds
