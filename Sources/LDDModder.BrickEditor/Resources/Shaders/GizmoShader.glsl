-- Vertex
#version 150
in vec3 Position;

uniform mat4 ModelMatrix;
uniform mat4 ViewMatrix;
uniform mat4 Projection;

out vec4 xVec;
out vec4 yVec;
out vec4 zVec;

void main()
{
	// transform vertex position
	mat4 mvp = Projection * ViewMatrix * ModelMatrix;
	gl_Position = mvp * vec4(Position, 1.0);

	xVec = ((mvp * vec4(Position + vec3(1,0,0), 1.0)) - gl_Position);
	yVec = ((mvp * vec4(Position + vec3(0,1,0), 1.0)) - gl_Position);
	zVec = ((mvp * vec4(Position + vec3(0,0,1), 1.0)) - gl_Position);

}

-- Geometry
#version 150

layout(points) in;
layout(line_strip, max_vertices = 6) out;

in vec4 xVec[];
in vec4 yVec[];
in vec4 zVec[];
out vec4 axisColor;

void DrawAxis(vec4 origin, vec4 axis, vec4 color)
{
	axisColor = color;
	gl_Position = origin;
	EmitVertex();
	gl_Position = origin + axis;
	EmitVertex();
	
}

void main()
{
	vec4 gizmoOrigin = gl_in[0].gl_Position;
	
	DrawAxis(gizmoOrigin, xVec[0], vec4(1,0,0,1));
	DrawAxis(gizmoOrigin, yVec[0], vec4(0,1,0,1));
	DrawAxis(gizmoOrigin, zVec[0], vec4(0,0,1,1));
	EndPrimitive();
}

-- Fragment
#version 150
out vec4 FragColor;
in vec4 axisColor;

void main()
{
	FragColor = axisColor;
}