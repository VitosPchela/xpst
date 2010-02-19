//-----------------------------------------------------------------------------
// Data
//-----------------------------------------------------------------------------
uniform mat4 modelview, objTrans;
uniform vec3 inLightVec, eyePos, fogData;

varying vec4 shading;
varying vec2 outTexCoord, fogCoord;

//-----------------------------------------------------------------------------
// Main                                                                        
//-----------------------------------------------------------------------------
void main()
{
   gl_Position = modelview * gl_Vertex;
   shading = vec4(clamp( dot(-inLightVec, gl_Normal), 0.0, 1.0 ));
   outTexCoord = gl_MultiTexCoord0.st;

   // fog setup
   vec3 transPos = (objTrans * gl_Vertex).xyz;
   fogCoord.x = 1.0 - ( distance( gl_Vertex.xyz, eyePos ) / fogData.z );
   fogCoord.y = (transPos.z - fogData.x) * fogData.y;
}
