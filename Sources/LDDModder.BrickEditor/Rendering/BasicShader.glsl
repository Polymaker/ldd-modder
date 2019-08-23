-- Vertex
#version 140
in vec3 InPosition;
uniform mat4 ModelViewProjectionMatrix;

void main()
{
	// transform vertex position
	gl_Position = ModelViewProjectionMatrix * vec4(InPosition,1);
}

-- Fragment
#version 140
out vec4 FragColor;
uniform vec4 MaterialColor;

void main()
{
	FragColor = MaterialColor;
}