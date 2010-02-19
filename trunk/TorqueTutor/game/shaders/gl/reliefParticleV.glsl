//-----------------------------------------------------------------------------
// Data
//-----------------------------------------------------------------------------
uniform mat4 modelview, texMat, objTrans;
uniform vec4 lightpos;
uniform vec3 eyePos;

varying vec4 color;
varying vec3 eye, light;
varying vec2 texcoord;

//-----------------------------------------------------------------------------
// Main                                                                        
//-----------------------------------------------------------------------------
void main()
{
	// vertex position in object space
	vec4 pos = vec4(gl_Vertex.xyz, 1.0);

	// vertex position in clip space
	gl_Position = modelview * pos;

	// copy color and texture coordinates
	texcoord = (texMat * gl_MultiTexCoord0).xy;
	
	// tangent vectors in view space
	mat3 tangentspace = mat3(gl_MultiTexCoord2.xyz, gl_MultiTexCoord3.xyz, gl_Normal);

	// vertex position in view space (with model transformations)
	vec3 vpos = (modelview * pos).xyz;

	// view in tangent space
	vec3 objectspace_view_vector = (texMat * vec4(eyePos, 1.0) - gl_Vertex).xyz;
   
   eye = tangentspace * objectspace_view_vector;
	eye = -eye;
	
	light = -lightpos.xyz;
	light = tangentspace * light;
	light = light / 2.0 + 0.5;

	color = gl_Color;
}

