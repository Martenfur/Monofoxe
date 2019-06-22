sampler s0;

float4 PixelShaderFunction(float2 coords : TEXCOORD0) : COLOR0
{
  float4 color = tex2D(s0, coords);
  float s = (color.r + color.g + color.b) / 3.0 ;
  return float4(s, s, s, 1) * color.a;
}


#ifdef SM4
// DirectX compilation.

#define TECHNIQUE(name, psName) \
	technique name \
	{ \
		pass Pass1 \
		{ \
			PixelShader = compile ps_4_0_level_9_1 psName(); \
		} \
	}

#else

// OpenGL compilation.
#define TECHNIQUE(name, psName) \
	technique name \
	{ \
		pass Pass1 \
		{ \
			PixelShader = compile ps_2_0 psName(); \
		} \
	}

#endif


TECHNIQUE(Technique1, PixelShaderFunction);

