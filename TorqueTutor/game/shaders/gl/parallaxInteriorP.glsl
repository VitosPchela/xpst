//-----------------------------------------------------------------------------
// Data
//-----------------------------------------------------------------------------
uniform sampler2D texmap, reliefmap, lightmap;
uniform vec4 ambient, specular;
uniform float shine;

varying vec3 eye, light;
varying vec2 texcoord, lmcoord;

//-----------------------------------------------------------------------------
// Main                                                                        
//-----------------------------------------------------------------------------
void main()
{
	const vec4 diffuse  = vec4(1.0, 1.0, 1.0, 1.0);
	
	// view and light directions
	vec3 v = normalize(eye);
	vec3 l = normalize(light);

	vec2 uv = texcoord;

	// parallax code
	float height = texture2D(reliefmap, uv).w * 0.06 - 0.03;
	uv += height * v.xy;

	// normal map
	vec4 normal = texture2D(reliefmap, uv);
	normal.xyz = normalize(normal.xyz - 0.5);
	normal.y = -normal.y;
	
	// color map
	vec4 color = texture2D(texmap, uv);
	
	// light map
	vec4 lm = texture2D(lightmap, lmcoord);
	
	// compute diffuse and specular terms
	float diff = clamp(dot(l, normal.xyz), 0.0, 1.0);
	float spec = clamp(dot(normalize(l-v), normal.xyz), 0.0, 1.0);
	
	// attenuation factor
	float att = 1.0 - max(0.0, l.z);
	att = 1.0 - att * att;

	// compute final color
	gl_FragColor = ambient * color +
		att * (color * diffuse * diff + specular * pow(spec, shine)) * lm;
		
	gl_FragColor.a = 1.0;
}
