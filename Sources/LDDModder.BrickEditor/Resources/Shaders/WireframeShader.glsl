-- Vertex
#version 150
in vec3 Position;

uniform mat4 ModelMatrix;
uniform mat4 ViewMatrix;
uniform mat4 Projection;

void main()
{
	// transform vertex position
	mat4 mvp = Projection * ViewMatrix * ModelMatrix;
	gl_Position = mvp * vec4(Position, 1.0);
}

-- Geometry
#version 150

layout(triangles) in;
layout(line_strip, max_vertices =3) out;

noperspective out vec3 WireCoord;

void main()
{
	for(int i = 0; i < 3; i++)
	{
		gl_Position = gl_in[i].gl_Position;
		WireCoord = vec3(0.0);
		WireCoord[i] = 1.0;
		EmitVertex();
	}
	EndPrimitive();
}

-- Fragment
#version 150
out vec4 FragColor;
in vec3 WireCoord;

uniform vec4 Color;
uniform float Thickness;
/*
float edgeFactor(){
    vec3 d = fwidth(WireCoord);
    vec3 a3 = smoothstep(vec3(0.0), d*Thickness, WireCoord);
    return min(min(a3.x, a3.y), a3.z);
}
*/
void main()
{
	FragColor = Color;
    //FragColor = vec4(Color.rgb, (1.0-edgeFactor())*0.5);
}