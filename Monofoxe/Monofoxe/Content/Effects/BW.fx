/*float4x4 World;
float4x4 View;
float4x4 Projection;

float4 AmbientColor = float4(1, 0, 1, 1);
float AmbientIntensity = 1;

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

float4 PixelShaderFunction(float2 coords : TEXCOORD0) : COLOR0
{
  float4 color = tex2D(s0, coords);
  float s = (color.r + color.g + color.b) / 3.0;
  return float4(s, s, s, 1.0f);
}

technique BW
{
  pass Pass1
  {
    AlphaBlendEnable = true;
    DestBlend = DESTALPHA;
    SrcBlend = SRCALPHA;
    PixelShader = compile ps_2_0 PixelShaderFunction();
  }
}*/

sampler s0;
float param1;

float4 PixelShaderFunction(float2 coords : TEXCOORD0) : COLOR0
{
  float cx = coords.x - 0.5;
  float cy = coords.y - 0.5;
  
  float4 color = tex2D(s0, coords);
  float s = (color.r + color.g + color.b) / 3.0 ;
  return float4(
    coords.x,//color.r / 1.3,
    color.g,// / 1.1, 
    color.b,
    1
   ) * color.a; //float4(s, s, s, 1) * color.a;

}

technique Technique1
{
  pass Pass1
  {
   // AlphaBlendEnable = true;
   // DestBlend = DESTALPHA;
   // SrcBlend = SRCALPHA;
    
    PixelShader = compile ps_2_0 PixelShaderFunction();
  }
}