//-----------------------------------------------------------------------------
// Data
//-----------------------------------------------------------------------------
uniform sampler2D texmap, reliefmap, lightmap;
uniform vec4 ambient, specular;
uniform float shine;

varying vec3 eye, light;
varying vec2 texcoord;

// use linear and binary search
vec3 ray_intersect_rm(sampler2D reliefmap, vec3 s, vec3 ds)
{
   const int linear_search_steps = 10;
   const int binary_search_steps = 5;
  
   ds /= float(linear_search_steps);

   for (int i = 0; i < linear_search_steps - 1; i++)
   {
		vec4 t = texture2D(reliefmap, s.xy);

		if (s.z < t.w)
			s += ds;
   }

   for (int i = 0; i < binary_search_steps; i++)
   {
		ds *= 0.5;
		vec4 t = texture2D(reliefmap, s.xy);
		
		if (s.z < t.w)
			s += 2.0 * ds;
			
		s -= ds;
   }

   return s;
}

// only linear search for shadows
vec3 ray_intersect_rm_lin(sampler2D reliefmap, vec3 s, vec3 ds)
{
   const int linear_search_steps = 10;
   
   ds /= float(linear_search_steps);

   for (int i = 0; i < linear_search_steps - 1; i++ )
   {
		vec4 t = texture2D(reliefmap, s.xy);
		if (s.z < t.w)
			s += ds;
   }
   
   return s;
}

void main()
{
	const float  depth    = 0.05;
	const vec4   diffuse  = vec4(1.0, 1.0, 1.0, 1.0);
	
	// view direction
	vec3 v = normalize(eye);
	
	// serach start point and serach vector with depth bias
	vec3 s = vec3(texcoord, 0.0);
	v.xy *= depth * (2.0 * v.z - v.z * v.z);
	v /= v.z;

	// ray intersect depth map
	vec3 tx = ray_intersect_rm(reliefmap, s, v);

	// displace start position to intersetcion point
	s.xy += v.xy * tx.z;

	// get normal and color at intersection point
	vec4 t = texture2D(reliefmap, tx.xy);
	vec4 c = texture2D(texmap, tx.xy);

	// expand normal
	t.xyz = t.xyz * 2.0 - 1.0;
	t.y = -t.y;

	// light direction
	vec3 l = normalize(light);
	
	// view direction
	v = normalize(eye);
	v.z = -v.z;

	// compute diffuse and specular terms
	float diff = clamp(dot(l, t.xyz), 0.0, 1.0);
	float spec = clamp(dot(normalize(l - v), t.xyz), 0.0, 1.0);

	// attenuation factor
	float att = 1.0 - max(0.0, l.z);
	att = 1.0 - att * att;

	// light serch vector with depth bias
	l.xy *= depth * (2.0 * l.z - l.z * l.z);
	l.z = -l.z;
	l /= l.z;
	
	// displace start position to light entry point
	s.xy -= l.xy * tx.z;
	
	// ray intersect from light
	vec3 tx2 = ray_intersect_rm_lin(reliefmap, s, l);
	
	// if pixel in shadow zero attenuation
	if (tx2.z < tx.z - 0.05) 
		att = 0.0;

	// compute final color
	gl_FragColor = ambient * c + 
	   att * (c * diffuse * diff + specular * pow(spec, shine));
	gl_FragColor.w = 1.0;
}

