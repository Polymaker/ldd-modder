-- Vertex
#version 150
in vec3 Position;
//in vec3 Normal;

uniform mat4 ModelMatrix;
uniform mat4 ViewMatrix;
uniform mat4 Projection;

smooth out vec2 vTexCoord;

void main()
{
	// transform vertex position
	mat4 mvp = Projection * ViewMatrix * ModelMatrix;
	gl_Position = mvp * vec4(Position, 1.0);
}

-- Fragment
#version 150

uniform vec2 GridSize;
uniform vec2 SelectStart;
uniform vec2 SelectEnd;

out vec4 FragColor;

vec4 blendColors(vec4 color1, vec4 color2)
{
	float alpha = color1.a + color2.a * (1.0 - color1.a);
	
	vec3 rgb = vec3(0);
	if (alpha > 0)
		rgb = ((color1.rgb * color1.a) + (color2.rgb * color2.a * (1.0 - color1.a))) / alpha;
		
	return vec4(rgb, alpha);
}

void main()
{
	vec4 baseColor = vec4(0);
	
	FragColor = vec4(0);
}