-- Vertex
#version 150
in vec3 Position;
in vec3 Normal;
in vec2 TexCoord;

uniform mat4 ModelMatrix;
uniform mat4 ViewMatrix;
uniform mat4 Projection;

out vec3 FragPos;
out vec3 FragNorm;
smooth out vec2 vTexCoord;

void main()
{
	// transform vertex position
	mat4 mvp = Projection * ViewMatrix * ModelMatrix;
	gl_Position = mvp * vec4(Position, 1.0);
	
	vTexCoord = TexCoord;
	
	FragPos = (ModelMatrix * vec4(Position,1.0)).xyz;
	FragNorm = mat3(transpose(inverse(ModelMatrix))) * Normal;
}

-- Fragment
#version 150
#define MAX_NUM_TOTAL_LIGHTS 10

struct LightInfo
{
	vec3 Position;
	float Constant;
	float Linear;
	float Quadratic;
	vec3 Ambient;
	vec3 Diffuse;
	vec3 Specular;
};

struct MaterialInfo
{
	vec4 Diffuse;
	vec3 Specular;
	float Shininess;
};

in vec3 FragPos;
in vec3 FragNorm;
smooth in vec2 vTexCoord;

uniform LightInfo Lights[MAX_NUM_TOTAL_LIGHTS];
uniform int LightCount;
uniform bool UseTexture;
uniform bool IsSelected;
uniform sampler2D Texture;
uniform MaterialInfo Material;
uniform vec3 ViewPosition;

out vec4 FragColor;

vec4 blendColors(vec4 color1, vec4 color2)
{
	float alpha = color1.a + color2.a * (1.0 - color1.a);
	
	vec3 rgb = vec3(0);
	if (alpha > 0)
		rgb = ((color1.rgb * color1.a) + (color2.rgb * color2.a * (1.0 - color1.a))) / alpha;
		
	return vec4(rgb, alpha);
}


vec3 CalcPointLight(LightInfo light, vec3 diffuseColor, vec3 normal, vec3 fragPos, vec3 viewDir)
{
    vec3 lightDir = normalize(light.Position - fragPos);
	
    // diffuse shading
    float diff = clamp(dot(normal, lightDir), 0, 1);
	
    // specular shading
    vec3 reflectDir = reflect(-lightDir, normal);
    float spec = pow(clamp(dot(viewDir, reflectDir), 0, 1), Material.Shininess);
	
    // attenuation
    float distance    = length(light.Position - fragPos);
    float attenuation = light.Constant + 
		(light.Linear * distance) + 
		(light.Quadratic * (distance * distance));
	attenuation =  clamp(1.0 / (attenuation * 0.7), 0, 1);
	
    // combine results
    vec3 ambient  = light.Ambient  * diffuseColor;
    vec3 diffuse  = light.Diffuse  * diff * diffuseColor;
    vec3 specular = light.Specular * spec * Material.Specular;
    ambient  *= min(0.5 + attenuation, 1.0);
    diffuse  *= attenuation;
    specular *= attenuation;
    return (ambient + diffuse + specular);
} 

float getBrightness(vec3 color)
{
	float fmin = min(min(color.r, color.g), color.b); //Min. value of RGB
 	float fmax = max(max(color.r, color.g), color.b); //Max. value of RGB
	return (fmax + fmin) / 2.0;
}

void main()
{
	
	vec3 norm = normalize(FragNorm);
    vec3 viewDir = normalize(ViewPosition - FragPos);
	
	vec4 baseColor = Material.Diffuse;
	
	if (UseTexture)
	{
		vec4 texColor = texture2D(Texture, vTexCoord);
		baseColor = blendColors(texColor, baseColor);
		baseColor.a = Material.Diffuse.a;
	}
	
	if (IsSelected)
	{
		//float baseBrightness = getBrightness(baseColor.rgb);
		baseColor.rgb = clamp(baseColor.rgb * 1.15 + (0.05), vec3(0), vec3(1));
		//baseColor.rgb = clamp(max(baseColor.rgb, baseBrightness * 0.5) * 1.15, vec3(0), vec3(1));
	}
	
	vec3 finalColor = baseColor.rgb * 0.1;
	
	if ( LightCount > 0)
		finalColor += CalcPointLight(Lights[0], baseColor.rgb, norm, FragPos, viewDir);  
	
	if ( LightCount > 1)
		finalColor += CalcPointLight(Lights[1], baseColor.rgb, norm, FragPos, viewDir);

	if ( LightCount > 2)
		finalColor += CalcPointLight(Lights[2], baseColor.rgb, norm, FragPos, viewDir);  	

	if ( LightCount > 3)
		finalColor += CalcPointLight(Lights[3], baseColor.rgb, norm, FragPos, viewDir);  
	
	FragColor = vec4(finalColor, baseColor.a);
}