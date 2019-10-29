-- Vertex
#version 150
in vec3 Position;
in vec3 Normal;

struct LightInfo
{
	vec3 Position;
	vec4 Ambient;
	vec4 Diffuse;
};
struct LightData
{
	vec3 Normal;
	vec3 Pos;
	vec3 EyeDir;
	vec3 LightDir;
};

const int maxLights = 8;

uniform int LightCount;
uniform LightInfo Lights[maxLights];

out LightData vLights[maxLights];

uniform mat4 ModelMatrix;
uniform mat4 ViewMatrix;
uniform mat4 ProjectionMatrix;

void main()
{
	mat4 mvp = ModelMatrix * ViewMatrix * ProjectionMatrix;
	
	// transform vertex position
	gl_Position = mvp * vec4(Position, 1.0);
	
	for (int i =0; i < LightCount; i++)
	{
		vLight[i].Pos = (ModelMatrix * vec4(Position,1.0)).xyz;
		vLight[i].EyeDir = vec3(0,0,0) - (ViewMatrix * ModelMatrix * vec4(Position, 1.0)).xyz;
		vLight[i].LightDir = (ViewMatrix * vec4(Lights[i].Position,1)).xyz + vLight[i].EyeDir;
		vLight[i].Normal = (ViewMatrix * ModelMatrix * vec4(Normal,0)).xyz;
	}
}

-- Fragment
#version 150
out vec4 FragColor;
in vec3 WireCoord;

struct LightInfo
{
	vec3 Position;
	vec4 Ambient;
	vec4 Diffuse;
};

struct LightData
{
	vec3 Normal;
	vec3 Pos;
	vec3 EyeDir;
	vec3 LightDir;
};

const int maxLights = 8;

uniform int LightCount;
uniform LightInfo Lights[maxLights];
in LightInfo vLights[maxLights];

uniform vec4 MaterialColor;

void main()
{
	vec4 finalColor = MaterialColor;
	vec3 LightColor = vec3(1,1,1);
	float LightPower = 80.0f;
	
	float distance = length(LightPosition - gLight.Pos);
	
	vec3 N = normalize(gLight.Normal);
	vec3 L = normalize(gLight.LightDir);
	float cosTheta = clamp(dot(N, L ), 0, 1);

	vec3 E = normalize(gLight.EyeDir);
	vec3 R = reflect(-L,N);
	float cosAlpha = clamp(dot( E,R ), 0,1 );

	vec3 ambiant = vec3(0.5) * MaterialColor.rgb;
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
		vec3 a3 = smoothstep(vec3(0.0), d * 0.75, WireCoord);
		float edgeFactor = min(min(a3.x, a3.y), a3.z);
		finalColor = vec4(mix(vec3(0.0), finalColor.rgb, edgeFactor), finalColor.a);
	}

	FragColor = finalColor;
}