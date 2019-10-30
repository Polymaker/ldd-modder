-- Vertex
#version 150
#define MAX_NUM_TOTAL_LIGHTS 100

layout (location = 0) in vec3 Position;
layout (location = 1) in vec3 Normal;
layout (location = 2) in vec3 TexCoord;

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

void main()
{
	// transform vertex position
	gl_Position = (ModelMatrix * ViewMatrix * Projection) * vec4(Position, 1.0);
	vTexCoord = TexCoord;
	
	VertPos = (ModelMatrix * vec4(Position,1.0)).xyz;
	VertNorm = (ViewMatrix * ModelMatrix * vec4(Normal,0)).xyz;
	
	for(int i = 0; i < LightCount; i++)
	{
		LightInfo curLight = Lights[i];
		vLights[i].EyeDir = vec3(0,0,0) - (ViewMatrix * ModelMatrix * vec4(curLight.Position, 1.0)).xyz;
		vLights[i].LightDir = (ViewMatrix * vec4(curLight.Position,1)).xyz + vLights[i].EyeDir;
	}
}

-- Fragment
#version 150
#define MAX_NUM_TOTAL_LIGHTS 100

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

	FragColor = finalColor;
}