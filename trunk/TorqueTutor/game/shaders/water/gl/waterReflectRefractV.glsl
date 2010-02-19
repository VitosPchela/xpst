uniform mat4 modelview, objTrans;
uniform mat3 cubeTrans;
uniform vec4 timeData;
uniform vec3 cubeEyePos, eyePos, inLightVec, fogData;
uniform vec2 waveTexScale[3], waveData[3];
uniform float reflectTexSize;

varying vec4 texCoord, texCoord3;
varying vec3 outPos, outEyePos;
varying vec2 texCoord2, fogCoord;

void main()
{
   gl_Position = modelview * gl_Vertex;
 
   texCoord.xy = gl_Vertex.xy * waveTexScale[0];
   texCoord.xy += waveData[0].xy * timeData[0];

   texCoord.zw = gl_Vertex.xy * waveTexScale[1];
   texCoord.zw += waveData[1].xy * timeData[1];

   texCoord2.xy = gl_Vertex.xy * waveTexScale[2];
   texCoord2.xy += waveData[2].xy * timeData[2];

   outPos = gl_Vertex.xyz;
   outEyePos = eyePos;

   // use projection matrix for reflection / refraction texture coords
   mat4 texGen = mat4( 0.5,  0.0,  0.0,  0.0,
                       0.0, -0.5,  0.0,  0.0,
                       0.0,  0.0,  1.0,  0.0,
                       0.5 + 0.5 / reflectTexSize,  0.5 + 1.0 / reflectTexSize,  0.0,  1.0 );

   texCoord3 = texGen * gl_Position;
   texCoord3.y = -texCoord3.y;

   vec3 transPos = vec3(objTrans * gl_Vertex);
   fogCoord.x = 1.0 - (distance(gl_Vertex.xyz, eyePos)/fogData.z);
   fogCoord.y = (transPos.z - fogData.x) * fogData.y;
}

   