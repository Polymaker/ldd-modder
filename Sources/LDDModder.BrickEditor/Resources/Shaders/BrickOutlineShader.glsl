-- Vertex
#version 150
in vec3 Position;
in vec2 vRECoord0;
in vec2 vRECoord1;
in vec2 vRECoord2;
in vec2 vRECoord3;
in vec2 vRECoord4;
in vec2 vRECoord5;
//in vec2 OutlineDataIndex;

uniform mat4 MVPMatrix;
//uniform sampler2D PackedRoundEdgeDataTexture;

smooth out vec2 vRECoords[6];


void main()
{
	// transform vertex position
	gl_Position = MVPMatrix * vec4(Position, 1.0);
	vRECoords[0] = vRECoord0;
	vRECoords[1] = vRECoord1;
	vRECoords[2] = vRECoord2;
	vRECoords[3] = vRECoord3;
	vRECoords[4] = vRECoord4;
	vRECoords[5] = vRECoord5;
	/*vec4 texCoordPair;
	float texOffset = 0.015625;
	
	for (int j = 0; j < 3; j++)
	{
		texCoordPair = texture2D(PackedRoundEdgeDataTexture, vec2(OutlineDataIndex.x + (j * texOffset), OutlineDataIndex.y));
		
		vRECoords[(j*2)] = texCoordPair.xy;
		vRECoords[(j*2) + 1] = texCoordPair.zw;
	}*/
}


-- Fragment
#version 150

uniform sampler2D Texture0;
uniform sampler2D Texture1;
uniform sampler2D Texture2;
uniform vec4 MaterialColor;
uniform int PairToDisplay;

smooth in vec2 vRECoords[6];
out vec4 FragColor;

const float maxWidthDepth = 0.15;
const float cutOffDepth = 0.7;
const float minWidth = 1.0;
const float maxWidth = 0.2;
const float inverseMaxZoom = 1.0 / 1400.0;
const float edgeColorModifier = 0.15;

vec4 GetTexel(sampler2D tex1, sampler2D tex2, vec2 texcoord)
{
	vec4 texel1 = texture2D( tex1, texcoord);
	vec4 texel2 = texture2D( tex2, texcoord);
	
	float select = step(0.0, texcoord.x);
	
	return (select * texel2) + ((1.0 - select) * texel1);
}

void main()
{
	vec4 finalColor = MaterialColor;
	
	float distanceToCamera = clamp((gl_FragCoord.z / gl_FragCoord.w) * inverseMaxZoom, 0.0, 1.0);
	float distanceToCameraWithMaxWidthDepth = clamp( distanceToCamera / maxWidthDepth, 0.0, 1.0);
	float scaleFactor = maxWidth + (1.0 - distanceToCameraWithMaxWidthDepth) * ( minWidth - maxWidth );

	float mf = 0;
	
	if (PairToDisplay > 0)
	{
		mf = texture2D( Texture0, vRECoords[PairToDisplay - 1] * scaleFactor ).b;
	}
	else
	{
		vec3 v1;

		v1.x = texture2D( Texture0, vRECoords[0] * scaleFactor ).b;
		v1.y = texture2D( Texture0, vRECoords[2] * scaleFactor ).b;
		v1.z = texture2D( Texture0, vRECoords[4] * scaleFactor ).b;
		
		vec2 texColorUnit1 = GetTexel( Texture2, Texture1, vRECoords[1] * scaleFactor).rb;
		vec2 texColorUnit3 = GetTexel( Texture2, Texture1, vRECoords[3] * scaleFactor).rb;
		vec2 texColorUnit5 = GetTexel( Texture2, Texture1, vRECoords[5] * scaleFactor).rb;
		
		vec3 v2 = vec3(texColorUnit1.y, texColorUnit3.y, texColorUnit5.y);
		vec3 v3 = vec3(texColorUnit1.x, texColorUnit3.x, texColorUnit5.x);
		
		vec3 mixFactors = min(max(v1, v2), v3);
		mf = max(max(mixFactors.x, mixFactors.y), mixFactors.z);
	}
	
	// On a Mac OS X machine the color is sometimes not completely black (0.0), but close (0.03125), so we change anything below 0.04 to black.
	mf = mf * step(0.04, mf);
	
	// Disable outlines when distance to pixel from camera is greater or equal to cutOffDepth
	mf = mf * step(distanceToCamera, cutOffDepth);
	
	// Compute greyscale intensity to figure out if we should lighten or darken the edge
	float intensity = finalColor.r * 0.3 + finalColor.g * 0.59 + finalColor.b * 0.11;
	float edgeColor = ((step(0.5, intensity) * 2.0) - 1.0) * -edgeColorModifier;
	//finalColor.rgb += (1.0 - step(mf, 0.0)) * edgeColor;
	finalColor = vec4(0);
	finalColor.b = mf;
	finalColor.a = 1;
	FragColor = finalColor;
}