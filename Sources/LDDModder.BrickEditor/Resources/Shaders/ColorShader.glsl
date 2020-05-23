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

-- Fragment
#version 150
out vec4 FragColor;
uniform vec4 Color;

void main()
{
	FragColor = Color;
}