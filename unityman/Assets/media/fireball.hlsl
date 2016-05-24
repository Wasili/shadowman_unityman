// part of the Irrlicht Engine Shader example.
// These simple Direct3D9 pixel and vertex shaders will be loaded by the shaders
// example. Please note that these example shaders don't do anything really useful.
// They only demonstrate that shaders can be used in Irrlicht.

//-----------------------------------------------------------------------------
// Global variables
//-----------------------------------------------------------------------------
static float iGlobalTime;

float4x4 mWorldViewProj; // World * View * Projection transformation
float4x4 mInvWorld;      // Inverted world matrix
float4x4 mTransWorld;    // Transposed world matrix
float3 mLightPos;        // Light position
float4 mLightColor;      // Light color


// Vertex shader output structure
struct VS_OUTPUT
{
	float4 Position : POSITION;  // vertex position
	float4 Diffuse  : COLOR0;    // vertex diffuse color
	float2 TexCoord : TEXCOORD0; // tex coords
};


VS_OUTPUT vertexMain(in float4 vPosition : POSITION,
					in float3 vNormal    : NORMAL,
					float2 texCoord      : TEXCOORD0 )
{
	VS_OUTPUT Output;
	
	
	// transform position to clip space
	Output.Position = mul(vPosition, mWorldViewProj);

	// transform normal
	float3 normal = mul(float4(vNormal,0.0), mInvWorld);

	// renormalize normal
	normal = normalize(normal);

	// position in world coodinates
	float3 worldpos = mul(mTransWorld, vPosition);

	// calculate light vector, vtxpos - lightpos
	float3 lightVector = worldpos - mLightPos;

	// normalize light vector
	lightVector = normalize(lightVector);

	// calculate light color
	float3 tmp = dot(-lightVector, normal);
	tmp = lit(tmp.x, tmp.y, 1.0);

	tmp = mLightColor * tmp.y;
	Output.Diffuse = float4(tmp.x, tmp.y, tmp.z, 0);
	Output.TexCoord = texCoord;
	
	return Output;
}


// Pixel shader output structure
struct PS_OUTPUT
{
	float4 RGBColor : COLOR0; // Pixel color
};


float noise(float3 p) //Thx to Las^Mercury
{
	float3 i = floor(p);
		float4 a = dot(i, float3(1., 57., 21.)) + float4(0., 57., 21., 78.);
		float3 f = cos((p - i)*acos(-1.))*(-.5) + .5;
		a = lerp(sin(cos(a)*a), sin(cos(1. + a)*(1. + a)), f.x);
	a.xy = lerp(a.xz, a.yw, f.y);
	return lerp(a.x, a.y, f.z);
}

float sphere(float3 p, float4 spr)
{
	return length(spr.xyz - p) - spr.w;
}

float flame(float3 p)
{
	float d = sphere(p*float3(1., .5, 1.), float4(.0, -1., .0, 1.));
	return d + (noise(p + float3(.0, iGlobalTime*2., .0)) + noise(p*3.)*.5)*.25*(p.y);
}

float scene(float3 p)
{
	return min(100. - length(p), abs(flame(p)));
}

float4 raymarch(float3 org, float3 dir)
{
	float d = 0.0, glow = 0.0, eps = 0.02;
	float3  p = org;
		bool glowed = false;

	for (int i = 0; i<64; i++)
	{
		d = scene(p) + eps;
		p += d * dir;
		if (d>eps)
		{
			if (flame(p) < .0)
				glowed = true;
			if (glowed)
				glow = float(i) / 64.;
		}
	}
	return float4(p, glow);
}
//sampler2D myTexture;
	
PS_OUTPUT pixelMain(float2 TexCoord : TEXCOORD0,
					float4 Position : POSITION,
					float4 Diffuse  : COLOR0 ) 
{

	PS_OUTPUT Output;

		iGlobalTime = 1.;

	float2 TexCoord = float2(200, 150);

		float2 v = -1.0 + 2.0 * Output.PosH / TexCoord.xy;
		v.x *= TexCoord.x / TexCoord.y;

	float3 org = float3(0., -2., 4.);
		float3 dir = normalize(float3(v.x*1.6, -v.y, -1.5));

		float4 p = raymarch(org, dir);
		float glow = p.w;

	float4 col = lerp(float4(1., .5, .1, 1.), float4(0.1, .5, 1., 1.), p.y*.02 + .4);

		float4 fragColor = lerp(float4(0., 0., 0., 0.), col, pow(glow*2., 4.));

		return Output;
	//float4 col = tex2D( myTexture, TexCoord ); // sample color map

	// multiply with diffuse and do other senseless operations
	//Output.RGBColor = Diffuse * col;
	//Output.RGBColor *= 4.0;

	//return Output;
}

