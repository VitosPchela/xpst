//-----------------------------------------------------------------------------
// Torque Shader Engine 
// Copyright (C) GarageGames.com, Inc.
//-----------------------------------------------------------------------------

// load gui used to display various metric outputs
exec("~/gui/FrameOverlayGui.gui");

// Note:  To implement your own metrics overlay 
// just add a function with a name in the form 
// XXXXMetricsCallback which can be enabled via
// metrics( XXXX )

function fpsMetricsCallback()
{
   return "  FPS: " @ $fps::real @ 
          "  max: " @ $fps::realMax @
          "  min: " @ $fps::realMin @
          "  mspf: " @ 1000 / $fps::real;
}

function gfxMetricsCallback()
{
   return "  | GFX |" @
          "  PolyCount: " @ $GFXDeviceStatistics::polyCount @
          "  DrawCalls: " @ $GFXDeviceStatistics::drawCalls @
          "  RTChanges: " @ $GFXDeviceStatistics::renderTargetChanges;
          
}

function terrainMetricsCallback()
{
   return "  | Terrain |" @
          "  MaxIdxBatch: " @ $TRender::mBiggestIndexBatch @
          "  MaxVertBatch: " @ $TRender::mBiggestVertexBatch;
}

function netMetricsCallback()
{
   return "  | Net |" @
          "  BitsSent: " @ $Stats::netBitsSent @
          "  BitsRcvd: " @ $Stats::netBitsReceived @
          "  GhostUpd: " @ $Stats::netGhostUpdates;
}

function groundCoverMetricsCallback()
{
   return "  | GroundCover |" @
          "  Cells: " @ $GroundCover::renderedCells @
          "  Billboards: " @ $GroundCover::renderedBillboards @
          "  Batches: " @ $GroundCover::renderedBatches @
          "  Shapes: " @ $GroundCover::renderedShapes;
}

function sfxMetricsCallback() 
{
   return "  | SFX |"@
          "  Sources: " @ $SFX::numSources @
          "  Playing: " @ $SFX::numPlaying @
          "  Culled: " @ $SFX::numCulled @
          "  Voices: " @ $SFX::numVoices;             
}

function timeMetricsCallback()
{
   return "  | Time |" @ 
          "  Sim Time: " @ getSimTime() @ 
          "  Mod: " @ getSimTime() % 32;
}

function renderMetricsCallback()
{
   return "  | Render |" @
          "  Int: " @ $RenderMetrics::RIT_Interior @
          "  IntDL: " @ $RenderMetrics::RIT_InteriorDynamicLighting @
          "  Mesh: " @ $RenderMetrics::RIT_Mesh @
          "  MeshDL: " @ $RenderMetrics::RIT_MeshDynamicLighting @
          "  Shadow: " @ $RenderMetrics::RIT_Shadow @
          "  Sky: " @ $RenderMetrics::RIT_Sky @
          "  Obj: " @ $RenderMetrics::RIT_Object @
          "  ObjT: " @ $RenderMetrics::RIT_ObjectTranslucent @
          "  Decal: " @ $RenderMetrics::RIT_Decal @
          "  Water: " @ $RenderMetrics::RIT_Water @
          "  Foliage: " @ $RenderMetrics::RIT_Foliage @
          "  Trans: " @ $RenderMetris::RIT_Translucent @
          "  Custom: " @ $RenderMetrics::RIT_Custom;
}

// alias
function audioMetricsCallback()
{
   return sfxMetricsCallback(); 
}

// alias
function videoMetricsCallback()
{
   return gfxMetricsCallback();
}

// Add a metrics HUD.  %expr can be a vector of names where each element
// must have a corresponding '<name>MetricsCallback()' function defined
// that will be called on each update of the GUI control.  The results
// of each function are stringed together.
//
// Example: metrics( "fps gfx" );

function metrics( %expr )
{
   %metricsExpr = "";
   if( %expr !$= "" )
   {
      for( %i = 0;; %i ++ )
      {
         %name = getWord( %expr, %i );
         if( %name $= "" )
            break;
         else
         {
            %cb = %name @ "MetricsCallback";
            if( !isFunction( %cb ) )
               error( "metrics - undefined callback: " @ %cb );
            else
            {
               %cb = %cb @ "()";
               if( %i > 0 )
                  %metricsExpr = %metricsExpr @ " @ ";
               %metricsExpr = %metricsExpr @ %cb;
            }
         }
      }
      
      if( %metricsExpr !$= "" )
         %metricsExpr = %metricsExpr @ " @ \" \"";
   }
   
   if( %metricsExpr !$= "" )
   {
      Canvas.pushDialog( FrameOverlayGui, 1000 );
      TextOverlayControl.setValue( %metricsExpr );
   }
   else
      Canvas.popDialog(FrameOverlayGui);
}
