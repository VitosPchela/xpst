//-----------------------------------------------------------------------------
// Torque Game Engine
// Copyright (C) GarageGames.com, Inc.
//-----------------------------------------------------------------------------

function TerraformerGui::init(%this)
{
   TerraformerHeightfieldGui.init();
   TerraformerTextureGui.init();
}

function TerraformerGui::onWake(%this)
{
   // Only the canvas level gui's get wakes, so udpate manually.
   TerraformerTextureGui.update();
}

function TerraformerGui::onSleep(%this)
{
   %this.setPrefs();
}

//--------------------------------------
function ETerraformer::onActiveTerrainChange( %this, %newTerrain )
{
   if( EHeightField.isVisible() )
   {
      Heightfield::refresh();
      if( HeightfieldPreview.isVisible() )
         Heightfield::preview( %selectedOperation );
   }
}

$nextTextureId = 1;
$nextTextureRegister = 1000;
$selectedMaterial = -1;
$selectedTextureOperation = -1;
$TerraformerTextureDir = "common/editor/textureScripts";

//--------------------------------------

function TextureInit()
{
   // Assumes the terrain object is called terrain

   Texture_operation_menu.clear();
   Texture_operation_menu.setText("Placement Operations");
   Texture_operation_menu.add("Place by Fractal", 1);
   Texture_operation_menu.add("Place by Height", 2);
   Texture_operation_menu.add("Place by Slope", 3);
   Texture_operation_menu.add("Place by Water Level", 4);

   $HeightfieldSrcRegister = Heightfield_operation.rowCount()-1;

   // sync up the preview windows
   TexturePreview.setValue(HeightfieldPreview.getValue());
   %script = ETerrainEditor.getActiveTerrain().getTextureScript();
   if(%script !$= "")
      Texture::loadFromScript(%script);

   if (Texture_material.rowCount() == 0)
   {
      Texture_operation.clear();
      $nextTextureRegister = 1000;
   }
   else
   {
      // it's difficult to tell if the heightfield was modified so
      // just in case flag all dependent operations as dirty.
      %rowCount = Texture_material.rowCount();
      for (%row = 0; %row < %rowCount; %row++)
      {
         %data = Texture_material.getRowText(%row);
         %entry= getRecord(%data,0);
         %reg  = getField(%entry,1);
         $dirtyTexture[ %reg ] = true;

         %opCount = getRecordCount(%data);
         for (%op = 2; %op < %opCount; %op++)
         {
            %entry= getRecord(%data,%op);
            %label= getField(%entry,0);
            if (%label !$= "Place by Fractal" && %label !$= "Fractal Distortion")
            {
               %reg  = getField(%entry,2);
               $dirtyTexture[ %reg ] = true;
            }
         }
      }
      Texture::previewMaterial();
   }
}

function TerraformerTextureGui::refresh(%this)
{
}


//--------------------------------------
function Texture_material_menu::onSelect(%this, %id, %text)
{
   %this.setText("Materials");

   // FORMAT
   //   material name
   //   register
   //     operation
   //       name
   //       tab name
   //       register
   //       distortion register
   //       {field,value}, ...
   //     operation
   //       ...
   Texture::saveMaterial();
   Texture::hideTab();
   %id = Texture::addMaterial(%text @ "\t" @ $nextTextureRegister++);

   if (%id != -1)
   {
      Texture_material.setSelectedById(%id);
      Texture::addOperation("Fractal Distortion\ttab_DistortMask\t" @ $nextTextureRegister++ @ "\t0\tdmask_interval\t20\tdmask_rough\t0\tdmask_seed\t" @ ETerraformer.generateSeed() @ "\tdmask_filter\t0.00000 0.00000 0.13750 0.487500 0.86250 1.00000 1.00000");
   }
}


function Texture::addMaterialTexture()
{
   %defaultFilePath = filePath(ETerrainEditor.getActiveTerrain().terrainFile);
   if( %defaultFilePath $= "" )
      %defaultFilePath = $defaultGame @ "/data/terrains/";

   %dlg = new OpenFileDialog()
   {
      Filters        = $Pref::TerrainEditor::TextureFileSpec;
      DefaultPath    = %defaultFilePath;
      ChangePath     = false;
      MustExist      = true;
   };
         
   %ret = %dlg.Execute();
   if(%ret)
      %file = %dlg.FileName;
   
   %dlg.delete();
   
   if(! %ret)
      return;
   
   Texture::saveMaterial();
   Texture::hideTab();
   %text = filePath(%file) @ "/" @ fileBase(%file);
   %text = makeRelativePath(%text, getMainDotCSDir());
   
   %id = Texture::addMaterial(%text @ "\t" @ $nextTextureRegister++);
   if (%id != -1)
   {
      Texture_material.setSelectedById(%id);
      Texture::addOperation("Fractal Distortion\ttab_DistortMask\t" @ $nextTextureRegister++ @ "\t0\tdmask_interval\t20\tdmask_rough\t0\tdmask_seed\t" @ ETerraformer.generateSeed() @ "\tdmask_filter\t0.00000 0.00000 0.13750 0.487500 0.86250 1.00000 1.00000");
   }
   Texture::save();
}

//--------------------------------------
function Texture_material::onSelect(%this, %id, %text)
{
   Texture::saveMaterial();
   if (%id != $selectedMaterial)
   {
      $selectedTextureOperation = -1;
      Texture_operation.clear();

      Texture::hideTab();
      Texture::restoreMaterial(%id);
   }

   %matName = getField(%text, 0);
   ETerrainEditor.paintMaterial = %matName;

   Texture::previewMaterial(%id);
   $selectedMaterial = %id;
   $selectedTextureOperation = -1;
   Texture_operation.clearSelection();
}


//--------------------------------------
function Texture_operation_menu::onSelect(%this, %id, %text)
{
   %this.setText("Placement Operations");
   %id = -1;

   if ($selectedMaterial == -1)
      return;

   %dreg = getField(Texture_operation.getRowText(0),2);

   switch$ (%text)
   {
      case "Place by Fractal":
         %id = Texture::addOperation("Place by Fractal\ttab_FractalMask\t" @ $nextTextureRegister++ @ "\t" @ %dreg @ "\tfbmmask_interval\t16\tfbmmask_rough\t0.000\tfbmmask_seed\t" @ ETerraformer.generateSeed() @ "\tfbmmask_filter\t0.000000 0.166667 0.333333 0.500000 0.666667 0.833333 1.000000\tfBmDistort\ttrue");

      case "Place by Height":
         %id = Texture::addOperation("Place by Height\ttab_HeightMask\t" @ $nextTextureRegister++ @ "\t" @ %dreg @ "\ttextureHeightFilter\t0 0.2 0.4 0.6 0.8 1.0\theightDistort\ttrue");

      case "Place by Slope":
         %id = Texture::addOperation("Place by Slope\ttab_SlopeMask\t" @ $nextTextureRegister++ @ "\t" @ %dreg @ "\ttextureSlopeFilter\t0 0.2 0.4 0.6 0.8 1.0\tslopeDistort\ttrue");

      case "Place by Water Level":
         %id = Texture::addOperation("Place by Water Level\ttab_WaterMask\t" @ $nextTextureRegister++ @ "\t" @ %dreg @ "\twaterDistort\ttrue");
   }

   // select it
   Texture::hideTab();
   if (%id != -1)
      Texture_operation.setSelectedById(%id);
}


//--------------------------------------
function Texture_operation::onSelect(%this, %id, %text)
{
   Texture::saveOperation();
   if (%id !$= $selectedTextureOperation)
   {
      Texture::hideTab();
      Texture::restoreOperation(%id);
      Texture::showTab(%id);
   }

   Texture::previewOperation(%id);
   $selectedTextureOperation = %id;
}


//--------------------------------------
function Texture::deleteMaterial(%id)
{
   if (%id $= "")
      %id = $selectedMaterial;
   if (%id == -1)
      return;

   %row = Texture_material.getRowNumById(%id);

   Texture_material.removeRow(%row);

   // find the next row to select
   %rowCount = Texture_material.rowCount()-1;
   if (%row > %rowCount)
      %row = %rowCount;

   if (%id == $selectedMaterial)
      $selectedMaterial = -1;

   Texture_operation.clear();
   %id = Texture_material.getRowId(%row);
   Texture_material.setSelectedById(%id);
   Texture::save();
}


//--------------------------------------
function Texture::deleteOperation(%id)
{
   if (%id $= "")
      %id = $selectedTextureOperation;
   if (%id == -1)
      return;

   %row = Texture_operation.getRowNumById(%id);

   // don't delete the first entry
   if (%row == 0)
      return;

   Texture_operation.removeRow(%row);

   // find the next row to select
   %rowCount = Texture_operation.rowCount()-1;
   if (%row > %rowCount)
      %row = %rowCount;

   if (%id == $selectedTextureOperation)
      $selectedTextureOperation = -1;

   %id = Texture_operation.getRowId(%row);
   Texture_operation.setSelectedById(%id);
   Texture::save();
}


//--------------------------------------
function Texture::applyMaterials()
{
   Texture::saveMaterial();
   %count = Texture_material.rowCount();
   if (%count > 0)
   {
      %data = getRecord(Texture_material.getRowText(0),0);
      %mat_list = getField( %data, 0);
      %reg_list = getField( %data, 1);
      Texture::evalMaterial(Texture_material.getRowId(0));

      for (%i=1; %i<%count; %i++)
      {
         Texture::evalMaterial(Texture_material.getRowId(%i));
         %data = getRecord(Texture_material.getRowText(%i),0);
         %mat_list = %mat_list @ " " @ getField( %data, 0);
         %reg_list = %reg_list @ " " @ getField( %data, 1);
      }
      ETerraformer.setMaterials(%reg_list, %mat_list);
   }
}


//--------------------------------------
function Texture::previewMaterial(%id)
{
   if (%id $= "")
      %id = $selectedMaterial;
   if (%id == -1)
      return;

   %data = Texture_material.getRowTextById(%id);
   %row  = Texture_material.getRowNumById(%id);
   %reg  = getField(getRecord(%data,0),1);

   Texture::evalMaterial(%id);

   ETerraformer.preview(TexturePreview, %reg);
}


//--------------------------------------
function Texture::evalMaterial(%id)
{
   if (%id $= "")
      %id = $selectedMaterial;
   if (%id == -1)
      return;

   %data = Texture_material.getRowTextbyId(%id);
   %reg  = getField(getRecord(%data,0), 1);

   // make sure all operation on this material are up to date
   // and accumulate register data for each
   %opCount = getRecordCount(%data);
   if (%opCount >= 2)    // record0=material record1=fractal
   {
      %entry = getRecord(%data, 1);
      Texture::evalOperationData(%entry, 1);
      for (%op=2; %op<%opCount; %op++)
      {
         %entry = getRecord(%data, %op);
         %reg_list = %reg_list @ getField(%entry, 2) @ " ";
         Texture::evalOperationData(%entry, %op);
      }
      // merge the masks in to the dst reg
      ETerraformer.mergeMasks(%reg_list, %reg);
   }
   Texture::save();
}


//--------------------------------------
function Texture::evalOperation(%id)
{
   if (%id $= "")
      %id = $selectedTextureOperation;
   if (%id == -1)
      return;

   %data   = Texture_operation.getRowTextById(%id);
   %row    = Texture_operation.getRowNumById(%id);

   if (%row != 0)
      Texture::evalOperation( Texture_operation.getRowId(0) );

   Texture::evalOperationData(%data, %row);
   Texture::save();
}


//--------------------------------------
function Texture::evalOperationData(%data, %row)
{
   %label  = getField(%data, 0);
   %reg    = getField(%data, 2);
   %dreg   = getField(%data, 3);
   %id     = Texture_material.getRowId(%row);

   if ( $dirtyTexture[%reg] == false )
   {
      return;
   }

   switch$ (%label)
   {
      case "Fractal Distortion":
         ETerraformer.maskFBm( %reg, getField(%data,5), getField(%data,7), getField(%data,9), getField(%data,11), false, 0 );

      case "Place by Fractal":
         ETerraformer.maskFBm( %reg, getField(%data,5), getField(%data,7), getField(%data,9), getField(%data,11), getField(%data,13), %dreg );

      case "Place by Height":
         ETerraformer.maskHeight( $HeightfieldSrcRegister, %reg, getField(%data,5), getField(%data,7), %dreg );

      case "Place by Slope":
         ETerraformer.maskSlope( $HeightfieldSrcRegister, %reg, getField(%data,5), getField(%data,7), %dreg );

      case "Place by Water Level":
         ETerraformer.maskWater( $HeightfieldSrcRegister, %reg, getField(%data,5), %dreg );
   }


   $dirtyTexture[%reg] = false;
}



//--------------------------------------
function Texture::previewOperation(%id)
{
   if (%id $= "")
      %id = $selectedTextureOperation;
   if (%id == -1)
      return;

   %row  = Texture_operation.getRowNumById(%id);
   %data = Texture_operation.getRowText(%row);
   %reg  = getField(%data,2);

   Texture::evalOperation(%id);
   ETerraformer.previewScaledGreyscale(TexturePreview, %reg);
}

//--------------------------------------
function Texture::restoreMaterial(%id)
{
   if (%id == -1)
      return;

   %data = Texture_material.getRowTextById(%id);

   Texture_operation.clear();
   %recordCount = getRecordCount(%data);
   for (%record=1; %record<%recordCount; %record++)
   {
      %entry = getRecord(%data, %record);
      Texture_operation.addRow($nextTextureId++, %entry);
   }
}


//--------------------------------------
function Texture::saveMaterial()
{
   %id = $selectedMaterial;
   if (%id == -1)
      return;

   Texture::SaveOperation();
   %data = Texture_Material.getRowTextById(%id);
   %newData = getRecord(%data,0);

   %rowCount = Texture_Operation.rowCount();
   for (%row=0; %row<%rowCount; %row++)
      %newdata = %newdata @ "\n" @ Texture_Operation.getRowText(%row);

   Texture_Material.setRowById(%id, %newdata);
   Texture::save();
}


//--------------------------------------
function Texture::restoreOperation(%id)
{
   if (%id == -1)
      return;

   %data = Texture_operation.getRowTextById(%id);

   %fieldCount = getFieldCount(%data);
   for (%field=4; %field<%fieldCount; %field += 2)
   {
      %obj = getField(%data, %field);
      %obj.setValue( getField(%data, %field+1) );
   }
   Texture::save();
}


//--------------------------------------
function Texture::saveOperation()
{
   %id = $selectedTextureOperation;
   if (%id == -1)
      return;

   %data = Texture_operation.getRowTextById(%id);
   %newData = getField(%data,0) @ "\t" @ getField(%data,1) @ "\t" @ getField(%data,2) @ "\t" @ getField(%data,3);

   // go through each object and update its value
   %fieldCount = getFieldCount(%data);
   for (%field=4; %field<%fieldCount; %field += 2)
   {
      %obj = getField(%data, %field);
      %newdata = %newdata @ "\t" @ %obj @ "\t" @ %obj.getValue();
   }

   %dirty = (%data !$= %newdata);
   %reg   = getField(%data, 2);
   $dirtyTexture[%reg] = %dirty;

   Texture_operation.setRowById(%id, %newdata);

   // mark the material register as dirty too
   if (%dirty == true)
   {
      %data = Texture_Material.getRowTextById($selectedMaterial);
      %reg  = getField(getRecord(%data,0), 1);
      $dirtyTexture[ %reg ] = true;
   }

   // if row is zero the fractal mask was modified
   // mark everything else in the list as dirty
    %row = Texture_material.getRowNumById(%id);
    if (%row == 0)
    {
       %rowCount = Texture_operation.rowCount();
       for (%r=1; %r<%rowCount; %r++)
       {
          %data = Texture_operation.getRowText(%r);
          $dirtyTexture[ getField(%data,2) ] = true;
       }
   }
   Texture::save();
}


//--------------------------------------
function Texture::addMaterial(%entry)
{
   %id = $nextTextureId++;
   Texture_material.addRow(%id, %entry);

   %reg = getField(%entry, 1);
   $dirtyTexture[%reg] = true;

   Texture::save();
   return %id;
}

//--------------------------------------
function Texture::addOperation(%entry)
{
   // Assumes: operation is being added to selected material

   %id = $nextTextureId++;
   Texture_operation.addRow(%id, %entry);

   %reg = getField(%entry, 2);
   $dirtyTexture[%reg] = true;

   Texture::save();
   return %id;
}


//--------------------------------------
function Texture::save()
{
   %script = "";

   // loop through each operation and save it to disk
   %rowCount = Texture_material.rowCount();
   for(%row = 0; %row < %rowCount; %row++)
   {
      if(%row != 0)
         %script = %script @ "\n";
      %data = expandEscape(Texture_material.getRowText(%row));
      %script = %script @ %data;
   }
   ETerrainEditor.getActiveTerrain().setTextureScript(%script);
   ETerrainEditor.isDirty = true;
}

//--------------------------------------
function Texture::import()
{
   getLoadFilename("*.ter", "Texture::doLoadTexture");
}

function Texture::loadFromScript(%script)
{
   Texture_material.clear();
   Texture_operation.clear();
   $selectedMaterial = -1;
   $selectedTextureOperation = -1;

   %i = 0;
   for(%rec = getRecord(%script, %i); %rec !$= ""; %rec = getRecord(%script, %i++))
      Texture::addMaterial(collapseEscape(%rec));
   // initialize dirty register array
   // patch up register usage
   // ...and deterime what the next register should be.
   $nextTextureRegister = 1000;
   %rowCount = Texture_material.rowCount();
   for (%row = 0; %row < %rowCount; %row++)
   {
      $dirtyTexture[ $nextTextureRegister ] = true;
      %data    = Texture_material.getRowText(%row);
      %rec     = getRecord(%data, 0);
      %rec     = setField(%rec, 1, $nextTextureRegister);
      %data    = setRecord(%data, 0, %rec);
      $nextTextureRegister++;

      %opCount = getRecordCount(%data);
      for (%op = 1; %op < %opCount; %op++)
      {
         if (%op == 1)
            %frac_reg = $nextTextureRegister;
         $dirtyTexture[ $nextTextureRegister ] = true;
         %rec  = getRecord(%data,%op);
         %rec  = setField(%rec, 2, $nextTextureRegister);
         %rec  = setField(%rec, 3, %frac_reg);
         %data = setRecord(%data, %op, %rec);
         $nextTextureRegister++;
      }
      %id = Texture_material.getRowId(%row);
      Texture_material.setRowById(%id, %data);
   }

   $selectedMaterial = -1;
   Texture_material.setSelectedById(Texture_material.getRowId(0));
}

//--------------------------------------
function Texture::doLoadTexture(%name)
{
   // ok, we're getting a terrain file...
   %newTerr = new TerrainBlock() // unnamed - since we'll be deleting it shortly:
   {
      position = "0 0 0";
      terrainFile = %name;
      squareSize = 8;
      visibleDistance = 4000;
   };
   if(isObject(%newTerr))
   {
      %script = %newTerr.getTextureScript();
      if(%script !$= "")
         Texture::loadFromScript(%script);
      %newTerr.delete();
   }
}



//--------------------------------------
function Texture::hideTab()
{
   tab_DistortMask.setVisible(false);
   tab_FractalMask.setVisible(false);
   tab_HeightMask.setVisible(false);
   tab_SlopeMask.setVisible(false);
   tab_waterMask.setVisible(false);
}


//--------------------------------------
function Texture::showTab(%id)
{
   Texture::hideTab();
   %data = Texture_operation.getRowTextById(%id);
   %tab  = getField(%data,1);
   %tab.setVisible(true);
}



$TerraformerHeightfieldDir = "common/editor/heightScripts";

function tab_Blend::reset(%this)
{
   blend_option.clear();
   blend_option.add("Add", 0);
   blend_option.add("Subtract", 1);
   blend_option.add("Max", 2);
   blend_option.add("Min", 3);
   blend_option.add("Multiply", 4);
}

function tab_fBm::reset(%this)
{
   fBm_detail.clear();
   fBm_detail.add("Very Low", 0);
   fBm_detail.add("Low", 1);
   fBm_detail.add("Normal", 2);
   fBm_detail.add("High", 3);
   fBm_detail.add("Very High", 4);
}

function tab_RMF::reset(%this)
{
   rmf_detail.clear();
   rmf_detail.add("Very Low", 0);
   rmf_detail.add("Low", 1);
   rmf_detail.add("Normal", 2);
   rmf_detail.add("High", 3);
   rmf_detail.add("Very High", 4);
}

function tab_terrainFile::reset(%this)
{
   // update tab controls..
   terrainFile_textList.clear();

   %filespec = $TerraformerHeightfieldDir @ "/*.ter";

   for(%file = findFirstFile(%filespec); %file !$= ""; %file = findNextFile(%filespec))
   {
      terrainFile_textList.addRow(%i++, fileBase(%file) @ fileExt(%file));
   }
}

function tab_canyon::reset()
{
}

function tab_smooth::reset()
{
}

function tab_smoothWater::reset()
{
}

function tab_smoothRidge::reset()
{
}

function tab_filter::reset()
{
}

function tab_turbulence::reset()
{
}

function tab_thermal::reset()
{
}

function tab_hydraulic::reset()
{
}

function tab_general::reset()
{
}

function tab_bitmap::reset()
{
}

function tab_sinus::reset()
{
}


//--------------------------------------

function Heightfield::resetTabs()
{
   tab_terrainFile.reset();
   tab_fbm.reset();
   tab_rmf.reset();
   tab_canyon.reset();
   tab_smooth.reset();
   tab_smoothWater.reset();
   tab_smoothRidge.reset();
   tab_filter.reset();
   tab_turbulence.reset();
   tab_thermal.reset();
   tab_hydraulic.reset();
   tab_general.reset();
   tab_bitmap.reset();
   tab_blend.reset();
   tab_sinus.reset();
}

//--------------------------------------
function TerraformerInit()
{
   Heightfield_options.clear();
   Heightfield_options.setText("Operation");
   Heightfield_options.add("fBm Fractal",0);
   Heightfield_options.add("Rigid MultiFractal",1);
   Heightfield_options.add("Canyon Fractal",2);
   Heightfield_options.add("Sinus",3);
   Heightfield_options.add("Bitmap",4);
   Heightfield_options.add("Turbulence",5);
   Heightfield_options.add("Smoothing",6);
   Heightfield_options.add("Smooth Water",7);
   Heightfield_options.add("Smooth Ridges/Valleys", 8);
   Heightfield_options.add("Filter",9);
   Heightfield_options.add("Thermal Erosion",10);
   Heightfield_options.add("Hydraulic Erosion",11);
   Heightfield_options.add("Blend",12);
   Heightfield_options.add("Terrain File",13);

   Heightfield::resetTabs();

   %script = ETerrainEditor.getActiveTerrain().getHeightfieldScript();
   if(%script !$= "")
      Heightfield::loadFromScript(%script,true);

   if (Heightfield_operation.rowCount() == 0)
   {
      Heightfield_operation.clear();
      %id1 = Heightfield::add("General\tTab_general\tgeneral_min_height\t50\tgeneral_scale\t300\tgeneral_water\t0.000\tgeneral_centerx\t0\tgeneral_centery\t0");
      Heightfield_operation.setSelectedById(%id1);
   }

   Heightfield::resetTabs();
   Heightfield::preview();
}

//--------------------------------------
function Heightfield_options::onSelect(%this, %_id, %text)
{
   Heightfield_options.setText("Operation");
   %id = -1;

   %rowCount = Heightfield_operation.rowCount();

   // FORMAT
   //  item name
   //  tab name
   //    control name
   //    control value
   switch$(%text)
   {
      case "Terrain File":
         %id = HeightField::add("Terrain File\ttab_terrainFile\tterrainFile_terrFileText\tterrains/terr1.ter\tterrainFile_textList\tterr1.ter");

      case "fBm Fractal":
         %id = Heightfield::add("fBm Fractal\ttab_fBm\tfbm_interval\t9\tfbm_rough\t0.000\tfBm_detail\tNormal\tfBm_seed\t" @ ETerraformer.generateSeed());

      case "Rigid MultiFractal":
         %id = Heightfield::add("Rigid MultiFractal\ttab_RMF\trmf_interval\t4\trmf_rough\t0.000\trmf_detail\tNormal\trmf_seed\t" @ ETerraformer.generateSeed());

      case "Canyon Fractal":
         %id = Heightfield::add("Canyon Fractal\ttab_Canyon\tcanyon_freq\t5\tcanyon_factor\t0.500\tcanyon_seed\t" @ ETerraformer.generateSeed());

      case "Sinus":
         %id = Heightfield::add("Sinus\ttab_Sinus\tsinus_filter\t1 0.83333 0.6666 0.5 0.33333 0.16666 0\tsinus_seed\t" @ ETerraformer.generateSeed());

      case "Bitmap":
         %id = Heightfield::add("Bitmap\ttab_Bitmap\tbitmap_name\t");
   }


   if (Heightfield_operation.rowCount() >= 1)
   {
      switch$(%text)
      {
         case "Smoothing":
            %id = Heightfield::add("Smoothing\ttab_Smooth\tsmooth_factor\t0.500\tsmooth_iter\t0");

         case "Smooth Water":
            %id = Heightfield::add("Smooth Water\ttab_SmoothWater\twatersmooth_factor\t0.500\twatersmooth_iter\t0");

         case "Smooth Ridges/Valleys":
            %id = Heightfield::add("Smooth Ridges/Valleys\ttab_SmoothRidge\tridgesmooth_factor\t0.8500\tridgesmooth_iter\t1");

         case "Filter":
            %id = Heightfield::add("Filter\ttab_Filter\tfilter\t0 0.16666667 0.3333333 0.5 0.6666667 0.8333333 1");

         case "Turbulence":
            %id = Heightfield::add("Turbulence\ttab_Turbulence\tturbulence_factor\t0.250\tturbulence_radius\t10");

         case "Thermal Erosion":
            %id = Heightfield::add("Thermal Erosion\ttab_Thermal\tthermal_slope\t30\tthermal_cons\t80.0\tthermal_iter\t0");

         case "Hydraulic Erosion":
            %id = Heightfield::add("Hydraulic Erosion\ttab_Hydraulic\thydraulic_iter\t0\thydraulic_filter\t0 0.16666667 0.3333333 0.5 0.6666667 0.8333333 1");
      }
   }

   if (Heightfield_operation.rowCount() >= 2)
   {
      if("Blend" $= %text)
         %id = Heightfield::add("Blend\ttab_Blend\tblend_factor\t0.500\tblend_srcB\t" @ %rowCount-2 @"\tblend_option\tadd");
   }


   // select it
   if (%id != -1)
      Heightfield_operation.setSelectedById(%id);
}


//--------------------------------------
function Heightfield::eval(%id)
{
   if (%id == -1)
      return;

   %data  = restWords(Heightfield_operation.getRowTextById(%id));
   %label = getField(%data,0);
   %row   = Heightfield_operation.getRowNumById(%id);

   echo("Heightfield::eval:" @ %row @ "  " @ %label );

   switch$(%label)
   {
      case "General":
         if (ETerrainEditor.getActiveTerrain().squareSize>0) %size = ETerrainEditor.getActiveTerrain().squareSize;
         else %size = 8;
         ETerraformer.setTerrainInfo( 256, %size, getField(%data,3), getField(%data,5), getField(%data,7) );
         %x = getField( %data, 9 );
         %y = getField( %data, 11 );
         ETerraformer.setShift( %x, %y );
         HeightfieldPreview.setOrigin( %x, %y );
         ETerraformer.terrainData(%row);

      case "Terrain File":
         ETerraformer.terrainFile(%row, getField(%data,3));

      case "fBm Fractal":
         ETerraformer.fBm( %row, getField(%data,3), getField(%data,5), getField(%data,7), getField(%data,9) );

      case "Sinus":
         ETerraformer.sinus( %row, getField(%data,3), getField(%data,5) );

      case "Rigid MultiFractal":
         ETerraformer.rigidMultiFractal( %row, getField(%data,3), getField(%data,5), getField(%data,7), getField(%data,9) );

      case "Canyon Fractal":
         ETerraformer.canyon( %row, getField(%data,3), getField(%data,5), getField(%data,7) );

      case "Smoothing":
         ETerraformer.smooth( %row-1, %row, getField(%data,3), getField(%data,5) );

      case "Smooth Water":
         ETerraformer.smoothWater( %row-1, %row, getField(%data,3), getField(%data,5) );

      case "Smooth Ridges/Valleys":
         ETerraformer.smoothRidges( %row-1, %row, getField(%data,3), getField(%data,5) );

      case "Filter":
         ETerraformer.filter( %row-1, %row, getField(%data,3) );

      case "Turbulence":
         ETerraformer.turbulence( %row-1, %row, getField(%data,3), getField(%data,5) );

      case "Thermal Erosion":
         ETerraformer.erodeThermal( %row-1, %row, getField(%data,3), getField(%data,5),getField(%data,7) );

      case "Hydraulic Erosion":
         ETerraformer.erodeHydraulic( %row-1, %row, getField(%data,3), getField(%data,5) );

      case "Bitmap":
         ETerraformer.loadGreyscale(%row, getField(%data,3));

      case "Blend":
         %rowCount = Heightfield_operation.rowCount();
         if(%rowCount > 2)
         {
            %a = Heightfield_operation.getRowNumById(%id)-1;
            %b = getField(%data, 5);
            echo("Blend: " @ %data);
            echo("Blend: " @ getField(%data,3) @ "  " @ getField(%data,7));
            if(%a < %rowCount || %a > 0 || %b < %rowCount || %b > 0 )
               ETerraformer.blend(%a, %b, %row, getField(%data,3), getField(%data,7) );
            else
               echo("Heightfield Editor: Blend parameters out of range.");
         }
   }

}

//--------------------------------------
function Heightfield::add(%entry)
{
   Heightfield::saveTab();
   Heightfield::hideTab();

   %id = $NextOperationId++;
   if ($selectedOperation != -1)
   {
      %row = Heightfield_operation.getRowNumById($selectedOperation) + 1;
      %entry = %row @ " " @ %entry;
      Heightfield_operation.addRow(%id, %entry, %row); // insert

      // adjust row numbers
      for(%i = %row+1; %i < Heightfield_operation.rowCount(); %i++)
      {
         %id = Heightfield_operation.getRowId(%i);
         %text = Heightfield_operation.getRowTextById(%id);
         %text = setWord(%text, 0, %i);
         Heightfield_operation.setRowById(%id, %text);
      }
   }
   else
   {
      %entry = Heightfield_operation.rowCount() @ " " @ %entry;
      Heightfield_operation.addRow(%id, %entry);   // add to end
   }

   %row = Heightfield_operation.getRowNumById(%id);
   if (%row <= $HeightfieldDirtyRow)
      $HeightfieldDirtyRow = %row;
   Heightfield::save();
   return %id;
}


//--------------------------------------
function Heightfield::onDelete(%id)
{
   if (%id $= "")
      %id = $selectedOperation;

   %row = Heightfield_operation.getRowNumById(%id);

   // don't delete the first entry
   if (%row == 0)
      return;

   Heightfield_operation.removeRow(%row);

   // adjust row numbers
   for(%i = %row; %i < Heightfield_operation.rowCount(); %i++)
   {
      %id2 = Heightfield_operation.getRowId(%i);
      %text = Heightfield_operation.getRowTextById(%id2);
      %text = setWord(%text, 0, %i);
      Heightfield_operation.setRowById(%id2, %text);
   }

   // adjust the Dirty Row position
   if ($HeightfieldDirtyRow >= %row)
      $HeightfieldDirtyRow = %row;

   // find the next row to select
   %rowCount = Heightfield_operation.rowCount()-1;
   if (%row > %rowCount)
      %row = %rowCount;

   if (%id == $selectedOperation)
      $selectedOperation = -1;

   %id = Heightfield_operation.getRowId(%row);
   Heightfield_operation.setSelectedById(%id);
   Heightfield::save();
}


//--------------------------------------
function Heightfield_operation::onSelect(%this, %id, %text)
{
   Heightfield::saveTab();
   Heightfield::hideTab();

   $selectedOperation = %id;
   Heightfield::restoreTab($selectedOperation);
   Heightfield::showTab($selectedOperation);
   Heightfield::preview($selectedOperation);
}

//--------------------------------------
function Heightfield::restoreTab(%id)
{
   if (%id == -1)
      return;

   Heightfield::hideTab();

   %data = restWords(Heightfield_operation.getRowTextById(%id));

   %fieldCount = getFieldCount(%data);
   for (%field=2; %field<%fieldCount; %field += 2)
   {
      %obj = getField(%data, %field);
      %obj.setValue( getField(%data, %field+1) );
   }
   Heightfield::save();
}


//--------------------------------------
function Heightfield::saveTab()
{
   if ($selectedOperation == -1)
      return;

   %data = Heightfield_operation.getRowTextById($selectedOperation);

   %rowNum = getWord(%data, 0);
   %data = restWords(%data);
   %newdata = getField(%data,0) @ "\t" @ getField(%data,1);

   %fieldCount = getFieldCount(%data);
   for (%field=2; %field < %fieldCount; %field += 2)
   {
      %obj = getField(%data, %field);
      %newdata = %newdata @ "\t" @ %obj @ "\t" @ %obj.getValue();
   }
   // keep track of the top-most dirty operation
   // so we know who to evaluate later
   if (%data !$= %newdata)
   {
      %row = Heightfield_operation.getRowNumById($selectedOperation);
      if (%row <= $HeightfieldDirtyRow && %row > 0)
         $HeightfieldDirtyRow = %row;
   }

   Heightfield_operation.setRowById($selectedOperation, %rowNum @ " " @ %newdata);
   Heightfield::save();
}


//--------------------------------------
function Heightfield::preview(%id)
{
   %rowCount = Heightfield_operation.rowCount();
   if (%id $= "")
      %id = Heightfield_operation.getRowId(%rowCount-1);

   %row = Heightfield_operation.getRowNumById(%id);

   Heightfield::refresh(%row);
   ETerraformer.previewScaled(HeightfieldPreview, %row);
}


//--------------------------------------
function Heightfield::refresh(%last)
{
   if (%last $= "")
      %last = Heightfield_operation.rowCount()-1;

   // always update the general info
   Heightfield::eval(Heightfield_operation.getRowId(0));

   for( 0; $HeightfieldDirtyRow<=%last; $HeightfieldDirtyRow++)
   {
      %id = Heightfield_operation.getRowId($HeightfieldDirtyRow);
      Heightfield::eval(%id);
   }
   Heightfield::save();
}


//--------------------------------------
function Heightfield::apply(%id)
{
   %rowCount = Heightfield_operation.rowCount();
   if (%rowCount < 1)
      return;
   if (%id $= "")
      %id = Heightfield_operation.getRowId(%rowCount-1);

   %row = Heightfield_operation.getRowNumById(%id);

   HeightfieldPreview.setRoot();
   Heightfield::refresh(%row);
   ETerraformer.setTerrain(%row);

   //ETerraformer.setCameraPosition(0,0,0);
   ETerrainEditor.isDirty = true;
}

//--------------------------------------
$TerraformerSaveRegister = 0;
function Heightfield::saveBitmap(%name)
{
   if(%name $= "")
      getSaveFilename("*.png", "Heightfield::doSaveBitmap",
         $TerraformerHeightfieldDir @ "/" @ fileBase($Client::MissionFile) @ ".png");
   else
      Heightfield::doSaveBitmap(%name);
}

function Heightfield::doSaveBitmap(%name)
{
   ETerraformer.saveGreyscale($TerraformerSaveRegister, %name);
}

//--------------------------------------

function Heightfield::save()
{
   %script = "";
   %rowCount = Heightfield_operation.rowCount();
   for(%row = 0; %row < %rowCount; %row++)
   {
      if(%row != 0)
         %script = %script @ "\n";
      %data = restWords(Heightfield_operation.getRowText(%row));
      %script = %script @ expandEscape(%data);
   }
   ETerrainEditor.getActiveTerrain().setHeightfieldScript(%script);
   ETerrainEditor.isDirty = true;
}

//--------------------------------------
function Heightfield::import()
{
   getLoadFilename("*.ter", "Heightfield::doLoadHeightfield");
}


//--------------------------------------
function Heightfield::loadFromScript(%script,%leaveCamera)
{
   echo(%script);

   Heightfield_operation.clear();
   $selectedOperation = -1;
   $HeightfieldDirtyRow = -1;

   // zero out all shifting
   HeightfieldPreview.reset();

   for(%rec = getRecord(%script, %i); %rec !$= ""; %rec = getRecord(%script, %i++))
      Heightfield::add(collapseEscape(%rec));

   if (Heightfield_operation.rowCount() == 0)
   {
      // if there was a problem executing the script restore
      // the operations list to a known state
      Heightfield_operation.clear();
      Heightfield::add("General\tTab_general\tgeneral_min_height\t50\tgeneral_scale\t300\tgeneral_water\t0.000\tgeneral_centerx\t0\tgeneral_centery\t0");
   }
   %data = restWords(Heightfield_operation.getRowText(0));
   %x = getField(%data,7);
   %y = getField(%data,9);
   HeightfieldPreview.setOrigin(%x, %y);
   Heightfield_operation.setSelectedById(Heightfield_operation.getRowId(0));

   // Move the control object to the specified position
   if (!%leaveCamera)
      ETerraformer.setCameraPosition(%x,%y);
}

//--------------------------------------
function strip(%stripStr, %strToStrip)
{
   %len = strlen(%stripStr);
   if(strcmp(getSubStr(%strToStrip, 0, %len), %stripStr) == 0)
      return getSubStr(%strToStrip, %len, 100000);
   return %strToStrip;
}

function Heightfield::doLoadHeightfield(%name)
{
   // ok, we're getting a terrain file...

   %newTerr = new TerrainBlock() // unnamed - since we'll be deleting it shortly:
   {
      position = "0 0 -1000";
      terrainFile = strip("terrains/", %name);
      squareSize = 8;
      visibleDistance = 4000;
   };
   if(isObject(%newTerr))
   {
      %script = %newTerr.getHeightfieldScript();
      if(%script !$= "")
         Heightfield::loadFromScript(%script);
      %newTerr.delete();
   }
}


//--------------------------------------
function Heightfield::setBitmap()
{
   getLoadFilename("*.png", "Heightfield::doSetBitmap", bitmap_name.getValue());
}

//--------------------------------------
function Heightfield::doSetBitmap(%name)
{
   %fileName = makeRelativePath( %name, getMainDotCsDir() );
   bitmap_name.setValue( %fileName );
   Heightfield::saveTab();
   Heightfield::preview($selectedOperation);
}


//--------------------------------------
function Heightfield::hideTab()
{
   tab_terrainFile.setVisible(false);
   tab_fbm.setvisible(false);
   tab_rmf.setvisible(false);
   tab_canyon.setvisible(false);
   tab_smooth.setvisible(false);
   tab_smoothWater.setvisible(false);
   tab_smoothRidge.setvisible(false);
   tab_filter.setvisible(false);
   tab_turbulence.setvisible(false);
   tab_thermal.setvisible(false);
   tab_hydraulic.setvisible(false);
   tab_general.setvisible(false);
   tab_bitmap.setvisible(false);
   tab_blend.setvisible(false);
   tab_sinus.setvisible(false);
}


//--------------------------------------
function Heightfield::showTab(%id)
{
   Heightfield::hideTab();
   %data = restWords(Heightfield_operation.getRowTextById(%id));
   %tab  = getField(%data,1);
   echo("Tab data: " @ %data @ " tab: " @ %tab);
   %tab.setVisible(true);
}


//--------------------------------------
function Heightfield::center()
{
   %camera = ETerraformer.getCameraPosition();
   %x = getWord(%camera, 0);
   %y = getWord(%camera, 1);

   HeightfieldPreview.setOrigin(%x, %y);

   %origin = HeightfieldPreview.getOrigin();
   %x = getWord(%origin, 0);
   %y = getWord(%origin, 1);

   %root = HeightfieldPreview.getRoot();
   %x += getWord(%root, 0);
   %y += getWord(%root, 1);

   general_centerx.setValue(%x);
   general_centery.setValue(%y);
   Heightfield::saveTab();
}

function ExportHeightfield::onAction()
{
   error("Time to export the heightfield...");
   if (Heightfield_operation.getSelectedId() != -1) {
      $TerraformerSaveRegister = getWord(Heightfield_operation.getValue(), 0);
      Heightfield::saveBitmap("");
   }
}
