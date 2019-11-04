-- Vertex
#version 150

in vec3 Position;
out vec4 vertex;

uniform mat4 MVMatrix;
uniform mat4 PMatrix;

void main()
{
	// vertex to world-space
	vec4 worldPos = MVMatrix * vec4(Position, 1.0);
	gl_Position = PMatrix * worldPos; //project to screen-space
	vertex = vec4(Position.xyz, -worldPos.z); //xyz = world-space position, w = camera distance
}

-- Fragment
#version 150

struct GridLineInfo
{
	float Spacing;
	vec4 Color;
	float Thickness;
	bool OffCenter;
};

in vec4 vertex;
out vec4 FragColor;

uniform GridLineInfo MajorGridLine;
uniform GridLineInfo MinorGridLine;

const float minCameraDistance = 15.0;
const float mxCameraDistance = 200.0;

float gridDistInverted(vec2 coord, float spacing, float thickness, bool offCenter)
{
	vec2 adjCoord = offCenter ? coord + vec2(spacing / 2.0) : coord;
	adjCoord = adjCoord / spacing;
	vec2 grid = abs(fract(adjCoord - 0.5) - 0.5) / fwidth(adjCoord);
	float lineWeight = clamp(min(grid.x, grid.y) + 0.5 - (thickness/2.0), 0.0, 1.0);
	return 1.0 - lineWeight;
}

void main()
{
	vec2 coord = vertex.xz;
	float majorDistInv = 0.0;
	float minorDistInv = 0.0;

	if (MajorGridLine.Spacing > 0.0)
		majorDistInv = gridDistInverted(coord, MajorGridLine.Spacing, MajorGridLine.Thickness, MajorGridLine.OffCenter);
	
	if (MinorGridLine.Spacing > 0.0)
		minorDistInv = gridDistInverted(coord, MinorGridLine.Spacing, MinorGridLine.Thickness, MinorGridLine.OffCenter);

	float cameraDist = clamp((vertex.w / 20.0) - 0.5, 0.0, 1.0);
	cameraDist = mix(1.0, 0.1, cameraDist);
	
	if (majorDistInv > 0)
		FragColor = vec4(MajorGridLine.Color.xyz, majorDistInv * MajorGridLine.Color.w * cameraDist);
	else if (minorDistInv > 0)
		FragColor = vec4(MinorGridLine.Color.xyz, minorDistInv * MinorGridLine.Color.w * cameraDist);
	else
		discard;
}