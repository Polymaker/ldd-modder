-- Vertex
#version 150
in vec3 Position;
in vec3 Values;
out vec3 vPosition;
out vec3 vValues;

void main()
{
	vPosition = Position;
	vValues = Values;
	gl_Position = vec4(0);
}

-- Geometry
#version 150

layout(points) in;
layout(triangle_strip, max_vertices = 8) out;

in vec3 vPosition[1];
in vec3 vValues[1];
smooth out vec2 gTexCoord;

uniform vec2 CellSize;
uniform mat4 ModelMatrix;
uniform mat4 ViewMatrix;
uniform mat4 Projection;
uniform bool IsMale;


void drawQuad(mat4 mvp, vec2 pt1, vec2 pt2, vec2 texCoord, float yOffset)
{
	gl_Position = mvp * vec4(pt2.x, yOffset, pt1.y, 1.0);
	gTexCoord = texCoord + vec2(0.5,0.5);
	EmitVertex();
	
	gl_Position = mvp * vec4(pt2.x, yOffset, pt2.y, 1.0);
	gTexCoord = texCoord + vec2(0.5,0);
	EmitVertex();
	
	gl_Position = mvp * vec4(pt1.x, yOffset, pt1.y, 1.0);
	gTexCoord = texCoord + vec2(0,0.5);
	EmitVertex();
	
	gl_Position = mvp * vec4(pt1.x, yOffset, pt2.y, 1.0);
	gTexCoord = texCoord + vec2(0,0);
	EmitVertex();
	
	EndPrimitive();
}

void main()
{
	float yOffset = IsMale ? 0.0001 : -0.0001;
	
	mat4 mvp = Projection * ViewMatrix * ModelMatrix;
	vec2 pt1 = vPosition[0].xy * CellSize;
	vec2 pt2 = (vPosition[0].xy + vec2(1)) * CellSize;
	
	drawQuad(mvp, pt1, pt2, vec2(0), yOffset);
	
	if (vValues[0].x <= 2)
	{
		vec2 studCenter = (floor(pt1 / vec2(0.8)) * vec2(0.8)) + vec2(0.4);
		drawQuad(mvp, studCenter - vec2(0.4), studCenter + vec2(0.4), vec2(0,0.5), yOffset * 2);
	}
	
	/*
	gl_Position = mvp * vec4(pt2.x, yOffset, pt1.y, 1.0);
	gTexCoord = vec2(1,1);
	EmitVertex();
	
	gl_Position = mvp * vec4(pt2.x, yOffset, pt2.y, 1.0);
	gTexCoord = vec2(1,0);
	EmitVertex();
	
	gl_Position = mvp * vec4(pt1.x, yOffset, pt1.y, 1.0);
	gTexCoord = vec2(0,1);
	EmitVertex();
	
	gl_Position = mvp * vec4(pt1.x, yOffset, pt2.y, 1.0);
	gTexCoord = vec2(0,0);
	EmitVertex();
	
	EndPrimitive();*/
}

-- Fragment
#version 150

smooth in vec2 gTexCoord;

out vec4 FragColor;
uniform sampler2D Texture;

/*
vec4 blendColors(vec4 color1, vec4 color2)
{
	float alpha = color1.a + color2.a * (1.0 - color1.a);
	
	vec3 rgb = vec3(0);
	if (alpha > 0)
		rgb = ((color1.rgb * color1.a) + (color2.rgb * color2.a * (1.0 - color1.a))) / alpha;
		
	return vec4(rgb, alpha);
}
*/

void main()
{
	FragColor = texture2D(Texture, gTexCoord);
	//if (FragColor.a == 0)
	//	discard;
	/*float lineThickness = 1.0;
	vec2 grid = abs(fract(gTexCoord - 0.5) - 0.5) / fwidth(gTexCoord);
	float lineWeight = clamp(min(grid.x, grid.y) + 0.5 - (lineThickness/2.0), 0.0, 1.0);
	float edgeFactor = 1.0 - lineWeight;
	
	if (edgeFactor < 0.004)
		discard;
	FragColor = vec4(vec3(0), edgeFactor);*/
}