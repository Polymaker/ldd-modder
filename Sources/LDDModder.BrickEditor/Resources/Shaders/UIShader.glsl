-- Vertex
#version 150

// Inputs
in vec3 Position;
in vec2 TexCoord;

// Ouputs
smooth out vec2 vTexCoord;

// Uniforms
uniform mat4 Projection;

void main()
{
	// transform vertex position
	gl_Position = Projection * vec4(Position, 1.0);
	vTexCoord = TexCoord;
}

-- Fragment
#version 150

// Inputs
smooth in vec2 vTexCoord;

// Ouputs
out vec4 FragColor;

// Uniforms
uniform sampler2D Texture;

void main()
{
	FragColor = texture2D(Texture, vTexCoord);
	/*if (FragColor.a == 0)
		FragColor = vec4(1);*/
}