-- Vertex
#version 150

in vec3 Position;
out vec3 vertex;

uniform mat4 MVPMatrix;

void main()
{
	// transform vertex position
	gl_Position = MVPMatrix * vec4(Position, 1.0);
	vertex = Position.xyz;
}

-- Fragment
#version 150

in vec3 vertex;
out vec4 FragColor;

uniform vec4 MajorGridColor;
uniform vec4 MinorGridColor;
uniform vec3 MajorSettings;
uniform vec3 MinorSettings;

float gridDistInverted(vec2 coord, float spacing, float thickness, bool offCenter)
{
	vec2 adjCoord = offCenter ? coord + vec2(spacing / 2.0) : coord;
	adjCoord = adjCoord / spacing;
	vec2 grid = abs(fract(adjCoord - 0.5) - 0.5) / fwidth(adjCoord);
	float minDist = min(grid.x, grid.y);
	return 1.0 -  min(minDist, 1.0);
}

void main()
{
	vec2 coord = vertex.xz;
	float majorDistInv = 0.0;
	float minorDistInv = 0.0;
	
	if (MajorSettings.x > 0.0)
	{
		majorDistInv = gridDistInverted(coord, MajorSettings.x, MajorSettings.y, MajorSettings.z > 0);
	}
	
	if (MinorSettings.x > 0.0)
	{
		minorDistInv = gridDistInverted(coord, MinorSettings.x, MinorSettings.y, MinorSettings.z > 0);
	}

	vec4 c1 = vec4(MajorGridColor.xyz, majorDistInv * MajorGridColor.w);
	vec4 c2 = vec4(MinorGridColor.xyz, minorDistInv * MinorGridColor.w);
	
	vec4 result = mix(c2, c1, majorDistInv);

	FragColor = result;
}