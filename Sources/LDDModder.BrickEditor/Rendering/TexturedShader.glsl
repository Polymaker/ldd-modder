-- Vertex
#version 150
in vec3 InPosition;
in vec3 InNormal;
in vec2 InTexCoord;

//uniform mat4 ModelViewMatrix;
uniform mat4 ModelViewProjectionMatrix;

void main()
{
	// transform vertex position
	gl_Position = ModelViewProjectionMatrix * vec4(InPosition, 1.0);
}

-- Geometry
#version 150

layout(triangles) in;
layout(triangle_strip, max_vertices =3) out;
noperspective out vec3 WireCoord;

void main()
{
	for(int i = 0; i < 3; i++)
	{
		gl_Position = gl_in[i].gl_Position;
		WireCoord = vec3(0.0);
		WireCoord[i] = 1.0;
		EmitVertex();
	}
	EndPrimitive();
}

-- Fragment
#version 150
out vec4 FragColor;
in vec3 WireCoord;
smooth in vec2 TexCoord;

uniform vec4 MaterialColor;
uniform sampler2D Texture;
uniform bool DisplayWireframe = false;

void main()
{
	vec4 finalColor = texture(Texture, TexCoord);

	if (finalColor.a < 1)
	{
		finalColor = finalColor + (MaterialColor * (1.0 - finalColor.a));
	}

	if (DisplayWireframe)
	{
		vec3 d = fwidth(WireCoord);
		vec3 a3 = smoothstep(vec3(0.0), d * 1, WireCoord);
		float edgeFactor = min(min(a3.x, a3.y), a3.z);
		finalColor = vec4(mix(vec3(0.0), finalColor.rgb, edgeFactor), finalColor.a);
	}

	FragColor = finalColor;
}