-- Vertex
#version 150
in vec3 InPosition;
in vec3 InNormal;

struct LightInfo
{
	vec3 Normal;
	vec3 Pos;
	vec3 EyeDir;
	vec3 LightDir;
};
out LightInfo vLight;

uniform vec3 LightPosition;
uniform mat4 ModelMatrix;
uniform mat4 ViewMatrix;
uniform mat4 ModelViewProjectionMatrix;

void main()
{
	// transform vertex position
	gl_Position = ModelViewProjectionMatrix * vec4(InPosition, 1.0);

	vLight.Pos = (ModelMatrix * vec4(InPosition,1.0)).xyz;
	vLight.EyeDir = vec3(0,0,0) - (ViewMatrix * ModelMatrix * vec4(InPosition, 1.0)).xyz;
	vLight.LightDir = (ViewMatrix * vec4(LightPosition,1)).xyz + vLight.EyeDir;
	vLight.Normal = (ViewMatrix * ModelMatrix * vec4(InNormal,0)).xyz;
}

-- Geometry
#version 150

layout(triangles) in;
layout(triangle_strip, max_vertices =3) out;

struct LightInfo
{
	vec3 Normal;
	vec3 Pos;
	vec3 EyeDir;
	vec3 LightDir;
};
in LightInfo vLight[];
out LightInfo gLight;

noperspective out vec3 WireCoord;

void main()
{
	for(int i = 0; i < 3; i++)
	{
		gl_Position = gl_in[i].gl_Position;
		gLight = vLight[i];
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

struct LightInfo
{
	vec3 Normal;
	vec3 Pos;
	vec3 EyeDir;
	vec3 LightDir;
};
in LightInfo gLight;

uniform vec4 MaterialColor;
uniform bool DisplayWireframe = false;
uniform vec3 LightPosition;

void main()
{
	vec4 finalColor = MaterialColor;
	vec3 LightColor = vec3(1,1,1);
	float LightPower = 60.0f;

	float distance = length(LightPosition - gLight.Pos);

	vec3 N = normalize(gLight.Normal);
	vec3 L = normalize(gLight.LightDir);
	float cosTheta = clamp(dot(N, L ), 0, 1);

	vec3 E = normalize(gLight.EyeDir);
	vec3 R = reflect(-L,N);
	float cosAlpha = clamp(dot( E,R ), 0,1 );

	vec3 ambiant = vec3(0.3) * MaterialColor.rgb;
	vec3 diffuse = MaterialColor.rgb * LightColor * LightPower * cosTheta / (distance*distance);
	vec3 specular = vec3(0.3) * LightColor * LightPower * pow(cosAlpha,5) / (distance*distance);
	
	finalColor = vec4(ambiant + diffuse + specular, finalColor.a);

	/*
	float xDist = (floor((gl_FragCoord.x / 10.0) + 0.5) * 10.0) + 0.5;
	xDist = abs(gl_FragCoord.x - xDist);
	float yDist = (floor((gl_FragCoord.y / 10.0) + 0.5) * 10.0) + 0.5;
	yDist = abs(gl_FragCoord.y - yDist);
	if (xDist <= 1 && yDist <= 1)
	{
		finalColor = vec4(mix(vec3(1.0), finalColor.rgb, max(xDist,yDist)), finalColor.a);
	}
	*/

	if (DisplayWireframe)
	{
		vec3 d = fwidth(WireCoord);
		vec3 a3 = smoothstep(vec3(0.0), d * 1, WireCoord);
		float edgeFactor = min(min(a3.x, a3.y), a3.z);
		finalColor = vec4(mix(vec3(0.0), finalColor.rgb, edgeFactor), finalColor.a);
	}

	FragColor = finalColor;
}