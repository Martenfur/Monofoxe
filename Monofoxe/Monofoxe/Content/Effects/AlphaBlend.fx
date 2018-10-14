/*
Base shader for drawing everything, when there are no other shaders applied.
Main reason for it is proper alpha blending for pretty much everything.

NOTE: This shader is vital part of the engine. 
DO NOT modify it, unless you know, what you're doing and have good reasons!
*/

float4x4 World;
float4x4 View;
float4x4 Projection;
float4 AmbientColor = float4(1, 1, 1, 1);

uniform const texture BasicTexture;

uniform const sampler BaseSampler : register(s0) = sampler_state
{
  Texture = (BasicTexture);
};

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


VertexShaderOutput VS_Texture(VertexShaderInput input)
{
  VertexShaderOutput output;

  float4 worldPosition = mul(input.Position, World);
  float4 viewPosition = mul(worldPosition, View);
  output.Position = mul(viewPosition, Projection);
  output.TexCoords = input.TexCoords;
  output.Color = input.Color;

  return output;
}


/*
Premultiplying shader. Used for sufraces, regular sprites, textured primitives and texture fonts.
*/
float4 PS_TexturePremultiplied(VertexShaderOutput input) : COLOR0
{
  float4 color = tex2D(BaseSampler, input.TexCoords.xy);
  return float4(color.r * input.Color.r, color.g * input.Color.g, color.b * input.Color.b, input.Color.a) * color.a;
}

/*
Non-premultiplying shader. Used for Monogame's spritefonts.
*/
float4 PS_TextureNonPremultiplied(VertexShaderOutput input) : COLOR0
{
  float4 color = tex2D(BaseSampler, input.TexCoords.xy);
  return color * input.Color;
}

/*
Basic shader for primitives without texture.
*/
float4 PS_Basic(VertexShaderOutput input) : COLOR0
{
  return input.Color;
}

/*
Techniques are chosen by the engine automatically.
*/

technique Basic
{
  pass Pass1
  {
    VertexShader = compile vs_2_0 VS_Texture();
    PixelShader = compile ps_2_0 PS_Basic();
  }
}

technique TexturePremultiplied
{
  pass Pass1
  {
    VertexShader = compile vs_2_0 VS_Texture();
    PixelShader = compile ps_2_0 PS_TexturePremultiplied();
  }
}

technique TextureNonPremultiplied
{
  pass Pass1
  {
    VertexShader = compile vs_2_0 VS_Texture();
    PixelShader = compile ps_2_0 PS_TextureNonPremultiplied();
  }
}