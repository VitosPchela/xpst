//-----------------------------------------------------------------------------
// Data
//-----------------------------------------------------------------------------
uniform mat4 modelview;

//-----------------------------------------------------------------------------
// Main                                                                        
//-----------------------------------------------------------------------------
void main()
{
   gl_Position = modelview * gl_Vertex;
}