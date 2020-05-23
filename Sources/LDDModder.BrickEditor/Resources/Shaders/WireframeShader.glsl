-- Vertex
#version 150
in vec3 Position;
in vec3 Normal;

uniform mat4 ModelMatrix;
uniform mat4 ViewMatrix;
uniform mat4 Projection;

void main()
{
	// transform vertex position
	mat4 mvp = Projection * ViewMatrix * ModelMatrix;
	vec3 offsetPos = Position + (Normal * 0.0001);
	gl_Position = mvp * vec4(offsetPos, 1.0);
}

-- Geometry
#version 150

layout(triangles) in;
layout(line_strip, max_vertices = 4) out;

void main()
{
	for(int i = 0; i < 4; i++)
	{
		gl_Position = gl_in[i%3].gl_Position;
		EmitVertex();
	}
	EndPrimitive();
}

-- Fragment
#version 150
out vec4 FragColor;

uniform vec4 Color;

void main()
{
	FragColor = Color;
}