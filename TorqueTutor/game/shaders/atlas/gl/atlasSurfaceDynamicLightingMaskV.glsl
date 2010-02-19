//-----------------------------------------------------------------------------
// Data
//-----------------------------------------------------------------------------
uniform mat4 modelview, objTrans, lightingMatrix;
uniform vec4 lightPos;
uniform vec3 eyePos;

varying vec3 dlightCoord, dlightMaskCoord;

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
   // Transform and return.
   gl_Position = gl_Position = modelview * gl_Vertex;

   // Let's get some lighting calcs in here.
   dlightCoord = getDynamicLightingCoord(gl_Position, lightPos, objTrans, lightingMatrix);
   dlightMaskCoord = normalize(dlightCoord.xzy - 0.5);
}
