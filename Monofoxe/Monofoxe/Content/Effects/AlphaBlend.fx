float4x4 World;
float4x4 View;
float4x4 Projection;
float4 AmbientColor = float4(1, 1, 1, 1);

uniform const texture BasicTexture;

uniform const sampler s : register(s0) = sampler_state
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

float4 PS_Texture(VertexShaderOutput input) : COLOR0
{
  float4 color = tex2D(s, input.TexCoords.xy);

  return float4(color.r * input.Color.r, color.g * input.Color.g, color.b * input.Color.b, input.Color.a) * color.a;
}

float4 PS_Basic(VertexShaderOutput input) : COLOR0
{
  return input.Color;
}


technique Basic
{
  pass Pass1
  {
    VertexShader = compile vs_2_0 VS_Texture();
    PixelShader = compile ps_2_0 PS_Basic();
  }
}

technique Textured
{
  pass Pass1
  {
    VertexShader = compile vs_2_0 VS_Texture();
    PixelShader = compile ps_2_0 PS_Texture();
  }
}