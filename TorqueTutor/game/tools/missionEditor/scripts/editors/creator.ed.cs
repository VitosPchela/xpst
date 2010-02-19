//-----------------------------------------------------------------------------
// Torque Game Engine
// Copyright (C) GarageGames.com, Inc.
//-----------------------------------------------------------------------------

function Creator::onWake(%this)
{
   Creator.init();
}

function Creator::init( %this )
{
   //%this.clear();
   $InstantGroup = "MissionGroup";

   // ---------- INTERIORS
   %base = %this.insertItem( 0, "Interiors" );

   // walk all the interiors and add them to the correct group
   %interiorId = "";
   %file = findFirstFile( "*.dif" );

   while( %file !$= "" )
   {
      %file = makeRelativePath(%file, getMainDotCSDir());
      
      // Determine which group to put the file in
      // and build the group hierarchy as we go
      %split    = strreplace(%file, "/", " ");
      %dirCount = getWordCount(%split)-1;
      %parentId = %base;

      for(%i=0; %i<%dirCount; %i++)
      {
         %parent = getWords(%split, 0, %i);
         if( %parent $= "" )
            continue;

         // if the group doesn't exist create it
         if ( !%interiorId[%parent] )
            %interiorId[%parent] = %this.insertItem( %parentId, getWord(%split, %i));
         %parentId = %interiorId[%parent];
      }

      // Add the file to the group
      %create = "createInterior(" @ "\"" @ %file @ "\"" @ ");";
      %this.insertItem( %parentId, fileBase( %file ), %create,"Interior" );

      %file = findNextFile( "*.dif" );
   }


   // ---------- SHAPES - add in all the shapes now...
   %base = %this.insertItem(0, "Shapes");
   %dataGroup = "DataBlockGroup";

   for(%i = 0; %i < %dataGroup.getCount(); %i++)
   {
      %obj = %dataGroup.getObject(%i);
      // echo ("Obj: " @ %obj.getName() @ " - " @ %obj.category );
      if(%obj.category !$= "" || %obj.category != 0)
      {
         %id = %this.findItemByName(%obj.category);
           if (%id  == 0)
           {
            %grp = %this.insertItem(%base,%obj.category);
            %this.insertItem(%grp, %obj.getName(), %obj.getClassName() @ "::create(" @ %obj.getName() @ ");","Item");
         }
         else
         {
            %this.insertItem(%id, %obj.getName(), %obj.getClassName() @ "::create(" @ %obj.getName() @ ");","Item");
         }
      }
   }


   // ---------- Static Shapes
   %base = %this.insertItem( 0, "Static Shapes" );

   // walk all the statics and add them to the correct group
   %staticId = "";
   %file = findFirstFile( "*.dts" );
   while( %file !$= "" )
   {
      %file = makeRelativePath(%file, getMainDotCSDir());
      
      // Determine which group to put the file in
      // and build the group hierarchy as we go
      %split    = strreplace(%file, "/", " ");
      %dirCount = getWordCount(%split)-1;
      %parentId = %base;

      for(%i=0; %i<%dirCount; %i++)
      {
         %parent = getWords(%split, 0, %i);
         if( %parent $= "" )
            continue;
         
         // if the group doesn't exist create it
         if ( !%staticId[%parent] )
            %staticId[%parent] = %this.insertItem( %parentId, getWord(%split, %i));
         %parentId = %staticId[%parent];
      }
      // Add the file to the group
      %create = "TSStatic::create(\"" @ %file @ "\");";
      %this.insertItem( %parentId, fileBase( %file ), %create,"TSStatic" );

      %file = findNextFile( "*.dts" );
   }


   // *** OBJECTS - do the objects now...
   %objGroup[0] = "Environment";
   %objGroup[1] = "Mission";
   %objGroup[2] = "System";
   //%objGroup[3] = "AI";

   %Environment_Item[0]  = "Sky";
   %Environment_Item[1]  = "Sun";
   %Environment_Item[2]  = "Lightning";
   %Environment_Item[3]  = "Water";
   %Environment_Item[4]  = "SFXEmitter";
   %Environment_Item[5]  = "Precipitation";
   %Environment_Item[6]  = "ParticleEmitter";
   %Environment_Item[7]  = "fxSunLight";
   %Environment_Item[8]  = "fxShapeReplicator";
   %Environment_Item[9] = "fxFoliageReplicator";
   %Environment_Item[10] = "fxLight";

   %Environment_Item[11] = "sgLightObject";
   %Environment_Item[12] = "VolumeLight";
   %Environment_Item[13] = "sgMissionLightingFilter";
   %Environment_Item[14] = "sgDecalProjector";

   %Environment_Item[15] = "GroundCover";
   
   %Environment_Item[16]  = "TerrainBlock";
   %Environment_Item[17] = "MegaTerrain";
   %Environment_Item[18] = "Atlas";

   %Mission_Item[0] = "MissionArea";
   %Mission_Item[1] = "Path";
   %Mission_Item[2] = "PathMarker";
   %Mission_Item[3] = "Trigger";
   %Mission_Item[4] = "PhysicalZone";
   %Mission_Item[5] = "Camera";
   //%Mission_Item[5] = "GameType";
   //%Mission_Item[6] = "Forcefield";

   %System_Item[0] = "SimGroup";

   //%AI_Item[0] = "Objective";
   //%AI_Item[1] = "NavigationGraph";

   // objects group
   %base = %this.insertItem(0, "Mission Objects");

   // create 'em
   for(%i = 0; %objGroup[%i] !$= ""; %i++)
   {
      %grp = %this.insertItem(%base, %objGroup[%i]);

      %groupTag = "%" @ %objGroup[%i] @ "_Item";

      %done = false;
      for(%j = 0; !%done; %j++)
      {
         eval("%itemTag = " @ %groupTag @ %j @ ";");

         if(%itemTag $= "")
            %done = true;
         else
         {
            //echo("itemTag:" @ %itemTag @ " j:" @ %j);
            %this.insertItem(%grp, %itemTag, "ObjectBuilderGui.build" @ %itemTag @ "();",%itemTag);
         }
      }
   }
}

function createInterior(%name)
{
   %obj = new InteriorInstance()
   {
      position = "0 0 0";
      rotation = "0 0 0";
      interiorFile = %name;
   };

   return(%obj);
}

//function Creator::onAction(%this)
//{
//   %this.currentSel = -1;
//   %this.currentRoot = -1;
//   %this.currentObj = -1;

  // %sel = %this.getSelected();
  // if(%sel == -1 || %this.isGroup(%sel) || !$missionRunning)
  //    return;

   // the value is the callback function..
  // if(%this.getValue(%sel) $= "")
  //    return;

//   %this.currentSel = %sel;
//   %this.currentRoot = %this.getRootGroup(%sel);

  // %this.create(%sel);
//}

function Creator::onSelect(%this)
{
   Creator.clearSelection();
}

function Creator::onInspect(%this,%obj)
{
   if(!$missionRunning)
      return;

   %objId = eval(%this.getItemValue(%obj));

   // drop it from the editor - only SceneObjects can be selected...
   Creator.removeSelection(%obj);

   EditorTree.clearSelection();
   EWorldEditor.clearSelection();
   EWorldEditor.selectObject(%objId);
   EWorldEditor.dropSelection();
}

function Creator::create(%this, %sel)
{
   // create the obj and add to the instant group
   %obj = eval(%this.getItemValue(%sel));

   if(%obj == -1)
      return;

//   %this.currentObj = %obj;

   $InstantGroup.add(%obj);

   // drop it from the editor - only SceneObjects can be selected...
   EWorldEditor.clearSelection();
   EWorldEditor.selectObject(%obj);
   EWorldEditor.dropSelection();
}


function TSStatic::create(%shapeName)
{
   %obj = new TSStatic()
   {
      shapeName = %shapeName;
   };
   return(%obj);
}

function TSStatic::damage(%this)
{
   // prevent console error spam
}


//function Creator::getRootGroup(%sel)
//{
//   if(%sel == -1 || %sel == 0)
//      return(-1);
//
//   %parent = %this.getParent(%sel);
//   while(%parent != 0 || %parent != -1)
//   {
//      %sel = %parent;
//      %parent = %this.getParent(%sel);
//   }
//
//   return(%sel);
//}
//
//function Creator::getLastItem(%rootGroup)
//{
//   %traverse = %rootGroup + 1;
//   while(%this.getRootGroup(%traverse) == %rootGroup)
//      %traverse++;
//   return(%traverse - 1);
//}
//
//function Creator::createNext(%this)
//{
//   if(%this.currentSel == -1 || %this.currentRoot == -1 || %this.currentObj == -1)
//      return;
//
//   %sel = %this.currentSel;
//   %this.currentSel++;
//
//   while(%this.currentSel != %sel)
//   {
//      if(%this.getRootGroup(%this.currentSel) != %this.currentRoot)
//         %this.currentSel = %this.currentRoot + 1;
//
//      if(%this.isGroup(%this.currentSel))
//         %this.currentSel++;
//      else
//         %sel = %this.currentSel;
//   }
//
//   //
//   %this.currentObj.delete();
//   %this.create(%sel);
//}
//
//function Creator::createPrevious(%this)
//{
//   if(%this.currentSel == -1 || %this.currentGroup == -1 || %this.currentObj == -1)
//      return;
//
//   %sel = %this.currentSel;
//   %this.currentSel--;
//
//   while(%this.currentSel != %sel)
//   {
//      if(%this.getRootGroup(%this.currentSel) != %this.currentRoot)
//         %this.currentSel = getLastItem(%this.currentRoot);
//
//      if(%this.isGroup(%this.currentSel))
//         %this.currentSel--;
//      else
//         %sel = %this.currentSel;
//   }
//
//   //
//   %this.currentObj.delete();
//   %this.create(%sel);
//}
