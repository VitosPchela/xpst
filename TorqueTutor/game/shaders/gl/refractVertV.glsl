//-----------------------------------------------------------------------------
// Data
//-----------------------------------------------------------------------------
uniform mat4 modelview;
uniform vec3 eyePos;

varying vec2 texCoord;

//-----------------------------------------------------------------------------
// Main                                                                        
//-----------------------------------------------------------------------------
void main()
{
   gl_Position = modelview * gl_Vertex;
   
   vec3 screenNorm = (modelview * vec4(gl_Normal, 1.0)).xyz;
   vec3 refractVec = refract( vec3( 0.0, 0.0, 1.0 ), screenNorm, 0.7 );
   
   texCoord = vec2( (  gl_Position.x + refractVec.x ) / (gl_Position.w), 
                    ( -gl_Position.y + refractVec.y ) / (gl_Position.w) );
   
   texCoord = clamp( (texCoord + 1.0) * 0.5, 0.0, 1.0 );
}
