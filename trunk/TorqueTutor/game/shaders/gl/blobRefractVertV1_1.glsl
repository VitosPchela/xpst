//-----------------------------------------------------------------------------
// Data
//-----------------------------------------------------------------------------
uniform mat4 modelview;
uniform vec3 eyePos, inLightVec;

varying vec4 outSpecular;
varying vec2 texCoord, gradiantCoord;

//-----------------------------------------------------------------------------
// Main                                                                        
//-----------------------------------------------------------------------------
void main()
{
   gl_Position = modelview * gl_Vertex;
   
   vec3 screenNorm = ( modelview * vec4(gl_Normal, 1.0) ).xyz;
   vec3 refractVec = refract( vec3( 0.0, 0.0, 1.0 ), screenNorm, 0.7 );
   
   texCoord = vec2( (  gl_Position.x + refractVec.x ) / (gl_Position.w), 
                    ( -gl_Position.y + refractVec.y ) / (gl_Position.w) );
   
   texCoord = clamp( (texCoord + 1.0) * 0.5, 0.0, 1.0 );
   
   vec3 eyeToVert = normalize( eyePos - gl_Vertex.xyz );
   gradiantCoord.x = clamp( dot( gl_Normal, eyeToVert ), 0.0, 1.0 ) - (0.5 / 128.0);
   gradiantCoord.y = 0.0;

   // Specular 
   vec3 eyeVec = normalize( eyePos - gl_Vertex.xyz );
   vec3 halfAng = normalize(eyeVec + (-inLightVec) );
   float specular = clamp( dot(gl_Normal, halfAng), 0.0, 1.0 );
   specular = pow( specular, 16.0 );
   outSpecular = vec4(specular) * 0.85;
}
