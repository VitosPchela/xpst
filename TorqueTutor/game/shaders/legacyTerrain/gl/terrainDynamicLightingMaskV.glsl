//-----------------------------------------------------------------------------
// Data
//-----------------------------------------------------------------------------
uniform mat4 modelview, objTrans, lightingMatrix;
uniform vec4 lightPos;
uniform vec3 eyePos, fogData;

varying vec3 dlightCoord, dlightMaskCoord;
varying vec2 texCoord, fogCoord;

vec3 getDynamicLightingCoord(vec4 vertpos,
							        vec4 lightpos,
							        mat4 objtrans,
							        mat4 lighttrans)
{
	vec3 coord = (objtrans * vec4(vertpos.xyz, 1.0) - objtrans * vec4(lightpos.xyz, 1.0)).xyz * lightpos.w;
	return (lighttrans * vec4(coord.xyz, 1.0) + 0.5).xyz;
}

//-----------------------------------------------------------------------------
// Main                                                                        
//-----------------------------------------------------------------------------
void main()
{
   texCoord = gl_MultiTexCoord0.st;
   dlightCoord = getDynamicLightingCoord(gl_Vertex, lightPos, objTrans, lightingMatrix);
   dlightMaskCoord = normalize(dlightCoord.xzy - 0.5);
   gl_Position = gl_Position = modelview * gl_Vertex;
   fogCoord.x = 1.0 - ( distance( gl_Vertex.xyz, eyePos ) / fogData.z );
   fogCoord.y = (gl_Vertex.z - fogData.x) * fogData.y;
}
