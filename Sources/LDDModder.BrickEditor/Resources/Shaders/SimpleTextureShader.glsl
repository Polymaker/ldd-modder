-- Vertex
#version 150
in vec3 Position;
in vec2 TexCoord;
smooth out vec2 vTexCoord;

uniform mat4 ModelMatrix;
uniform mat4 ViewMatrix;
uniform mat4 Projection;

void main()
{
	mat4 mvp = Projection * ViewMatrix * ModelMatrix;
	gl_Position = mvp * vec4(Position, 1.0);
	vTexCoord = TexCoord;
}

-- Fragment
#version 150

smooth in vec2 vTexCoord;
out vec4 FragColor;
uniform sampler2D Texture;


void main()
{
	FragColor = texture2D(Texture, vTexCoord);
	if (FragColor.a == 0)
		discard;
}