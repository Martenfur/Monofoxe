float4x4 World;
float4x4 View;
float4x4 Projection;


struct VertexShaderInput
{
  float4 Position : POSITION0;
  float2 TexCoords : TEXCOORD0;
  float4 Color : COLOR0;
};

struct VertexShaderOutput
{
  float4 Position : POSITION0;
  float2 TexCoords : TEXCOORD0;
  float4 Color : COLOR0;
};


VertexShaderOutput PassThroughVertexFunction(VertexShaderInput input)
{
  VertexShaderOutput output;

	float4 worldPosition = mul(input.Position, World);
  float4 viewPosition = mul(worldPosition, View);
  output.Position = mul(viewPosition, Projection);
  output.TexCoords = input.TexCoords;
  output.Color = input.Color;

  return output;
}


#ifdef SM4
// DirectX compilation.

#define TECHNIQUE(name, vsName, psName) \
	technique name \
	{ \
		pass Pass1 \
		{ \
			VertexShader = compile vs_4_0_level_9_1 vsName(); \
			PixelShader = compile ps_4_0_level_9_1 psName(); \
		} \
	}

#else

// OpenGL compilation.
#define TECHNIQUE(name, vsName, psName) \
	technique name \
	{ \
		pass Pass1 \
		{ \
			VertexShader = compile vs_3_0 vsName(); \
			PixelShader = compile ps_3_0 psName(); \
		} \
	}

#endif