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
uniform float FadeDistance;

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

vec2 gridDistInverted2(vec2 coord, float spacing, float thickness, bool offCenter)
{
	vec2 adjCoord = offCenter ? coord + vec2(spacing / 2.0) : coord;
	adjCoord = adjCoord / spacing;
	vec2 grid = abs(fract(adjCoord - 0.5) - 0.5) / fwidth(adjCoord);
	grid.x = 1.0 - clamp(grid.x + 0.5 - (thickness/2.0), 0.0, 1.0);
	grid.y = 1.0 - clamp(grid.y + 0.5 - (thickness/2.0), 0.0, 1.0);
	return grid;
}

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
	vec2 coord = vertex.xz;
	float majorDistInv = 0.0;
	float minorDistInv = 0.0;
	vec2 axisDist = vec2(0);
	
	if (MajorGridLine.Spacing > 0.0)
		majorDistInv = gridDistInverted(coord, MajorGridLine.Spacing, MajorGridLine.Thickness, MajorGridLine.OffCenter);
	
	if (MinorGridLine.Spacing > 0.0)
		minorDistInv = gridDistInverted(coord, MinorGridLine.Spacing, MinorGridLine.Thickness, MinorGridLine.OffCenter);
		
	if (abs(coord.x) <= 0.1 || abs(coord.y) <= 0.1)
		axisDist = gridDistInverted2(coord, 1000.0, 1.0, false);
	
	if (length(axisDist) > 0 || majorDistInv > 0 || minorDistInv > 0) {
	
		float cameraDist = 1.0;
		if (FadeDistance > 0)
		{
			cameraDist = clamp((vertex.w / FadeDistance) - 0.5, 0.0, 1.0);
			cameraDist = mix(1.0, 0.1, cameraDist);
		}
		
		FragColor = vec4(0.0);
		if (axisDist.y > 0) {
			vec4 gridColor = vec4(1,0.09,0.26, axisDist.y * cameraDist);
			FragColor = gridColor;//blendColors(gridColor, FragColor);
		}
		if (axisDist.x > 0) {
			vec4 gridColor = vec4(0.156,0.564,1, axisDist.x * cameraDist);
			FragColor = blendColors(FragColor, gridColor);
		}
		if (majorDistInv > 0) {
			vec4 gridColor = vec4(MajorGridLine.Color.xyz, majorDistInv * MajorGridLine.Color.w * cameraDist);
			FragColor = blendColors(FragColor, gridColor);
		}
		
		if (minorDistInv > 0) {
			vec4 gridColor = vec4(MinorGridLine.Color.xyz, minorDistInv * MinorGridLine.Color.w * cameraDist);
			FragColor = blendColors(FragColor, gridColor);
		}	
		
		
	}
	else
		discard;
		
	
}