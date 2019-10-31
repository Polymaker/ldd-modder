-- Vertex
#version 150
in vec3 Position;
in vec3 Normal;
in vec2 TexCoord;

#define MAX_NUM_TOTAL_LIGHTS 20

struct LightInfo
{
	vec3 Position;
	vec3 Color;
	float Power;
};

struct LightData
{
	vec3 EyeDir;
	vec3 LightDir;
};

uniform LightInfo Lights[MAX_NUM_TOTAL_LIGHTS];
uniform int LightCount;
uniform mat4 ModelMatrix;
uniform mat4 ViewMatrix;
uniform mat4 Projection;

out LightData vLights[MAX_NUM_TOTAL_LIGHTS];
out vec3 VertPos;
out vec3 VertNorm;
smooth out vec2 vTexCoord;

void ComputeLight(int i)
{
	vec4 lPos = vec4(Lights[i].Position, 1.0);
	vec3 eyeDir = vec3(0,0,0) - (ViewMatrix * ModelMatrix * lPos).xyz;
	vLights[i].EyeDir = eyeDir;
	vLights[i].LightDir = (ViewMatrix * lPos).xyz + eyeDir;
}

void main()
{
	// transform vertex position
	mat4 mvp = Projection * ViewMatrix * ModelMatrix;
	gl_Position = mvp * vec4(Position, 1.0);
	
	vTexCoord = TexCoord;
	
	VertPos = (ModelMatrix * vec4(Position,1.0)).xyz;
	VertNorm = (ModelMatrix * ViewMatrix * vec4(Normal,0)).xyz;
	
	if ( LightCount > 0)
	{
		vec4 lPos = vec4(Lights[0].Position, 1.0);
		vec3 eyeDir = vec3(0,0,0) - (ViewMatrix * ModelMatrix * vec4(Position,1.0)).xyz;
		vLights[0].EyeDir = eyeDir;
		vLights[0].LightDir = (ViewMatrix * lPos).xyz + eyeDir;
	}

	/*
	for(int i = 0; i < MAX_NUM_TOTAL_LIGHTS; i++)
	{
		if (i >= LightCount)
			break;
		LightInfo curLight = Lights[i];
		vec4 lPos = vec4(curLight.Position, 1.0);
		vec3 eyeDir = vec3(0,0,0) - (ViewMatrix * ModelMatrix * lPos).xyz;
		vLights[i].EyeDir = eyeDir;
		vLights[i].LightDir = (ViewMatrix * lPos).xyz + eyeDir;
	}*/
}

-- Fragment
#version 150
#define MAX_NUM_TOTAL_LIGHTS 20

struct LightInfo
{
	vec3 Position;
	vec3 Color;
	float Power;
};

struct LightData
{
	vec3 EyeDir;
	vec3 LightDir;
};

struct MaterialInfo
{
	vec4 Diffuse;
	vec4 Specular;
	float Shininess;
};

in LightData vLights[MAX_NUM_TOTAL_LIGHTS];
in vec3 VertPos;
in vec3 VertNorm;
smooth in vec2 vTexCoord;

uniform LightInfo Lights[MAX_NUM_TOTAL_LIGHTS];
uniform int LightCount;
uniform bool UseTexture;
uniform sampler2D Texture;
uniform MaterialInfo Material;

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
	vec4 finalColor = Material.Diffuse;
	
	if (UseTexture)
	{
		vec4 texColor = texture2D(Texture, vTexCoord);
		finalColor = blendColors(finalColor, texColor);
	}
	
	if ( LightCount > 0)
	{
		float lightDist = length(Lights[0].Position - VertPos);
		float distSquared = lightDist * lightDist;
		vec3 N = normalize(VertNorm);
		vec3 L = normalize(vLights[0].LightDir);
		float cosTheta = clamp(dot(N, L ), 0, 1);

		vec3 E = normalize(vLights[0].EyeDir);
		vec3 R = reflect(-L,N);
		float cosAlpha = clamp(dot( E,R ), 0,1 );

		vec3 ambient = vec3(0.5) * finalColor.rgb;
		vec3 diffuse = finalColor.rgb * Lights[0].Color * Lights[0].Power * cosTheta / distSquared;
		vec3 specular = vec3(0.3) * Lights[0].Color * Lights[0].Power * pow(cosAlpha,5) / distSquared;
		finalColor = vec4(ambient + diffuse + specular, finalColor.a);
	}
		
	FragColor = finalColor;
}