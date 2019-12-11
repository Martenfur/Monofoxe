struct VertexShaderOutput
{
	float4 Position : POSITION0;
	float2 TexCoords : TEXCOORD0;
	float4 Color : COLOR0;
};

#ifdef SM4
// DirectX compilation.

#define TECHNIQUE(name, vsName, psName) \
	technique name \
	{ \
		pass Pass1 \
		{ \
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
			PixelShader = compile ps_3_0 psName(); \
		} \
	}

#endif