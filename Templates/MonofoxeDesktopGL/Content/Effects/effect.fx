float4x4 World;
float4x4 View;
float4x4 Projection;

float4 AmbientColor = float4(1, 0, 1, 1);
float AmbientIntensity = 1;

float4 test;

sampler s0;



struct VertexShaderInput
{
    float4 Position : POSITION0;
};

struct VertexShaderOutput
{
    float4 Position : POSITION0;
};

VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
{
    VertexShaderOutput output;

    float4 worldPosition = mul(input.Position, World);
    float4 viewPosition = mul(worldPosition, View);
    output.Position = mul(viewPosition, Projection);

    return output;
}

float4 PixelShaderFunction(float2 coords: TEXCOORD0) : COLOR0
{
	float4 color = tex2D(s0, coords);
  float s = (color.r + color.g + color.b) / 3.0;
  return float4(s, s, s, 1.0f) * test;
}

technique Ambient
{
  pass Pass1
  {
    AlphaBlendEnable = true;
    DestBlend = DESTALPHA;
    SrcBlend = SRCALPHA;
    PixelShader = compile ps_2_0 PixelShaderFunction();
  }
}