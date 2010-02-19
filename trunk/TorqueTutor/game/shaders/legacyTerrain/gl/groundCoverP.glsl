//-----------------------------------------------------------------------------
// Torque Game Engine Advanced
// Copyright (C) GarageGames.com, Inc.
//-----------------------------------------------------------------------------

uniform sampler2D diffuseMap;

varying vec4 ambient;
varying vec2 outTexCoord;

void main()
{
   // Why the 2x?  In the brightest areas the lightmap value is 2/3s of the
   // sunlight color and in the darkest areas its 1/2 of the ambient value.
   // So the 2x ensures that the ambient doesn't get darker than it should
   // at the expense of a little over brightening in the sunlight.
   vec3 diffuse = clamp( ambient.rgb * 2.0, 0.0, 1.0 );

   // Return the final color.
   gl_FragColor = texture2D( diffuseMap, outTexCoord ) * vec4( diffuse, ambient.a );
}
