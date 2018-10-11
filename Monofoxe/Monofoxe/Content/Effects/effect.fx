sampler s0;
float param1;

float4 PixelShaderFunction(float2 coords : TEXCOORD0) : COLOR0
{

  float4 color = tex2D(s0, coords);

  if (coords.y > 0.5)
    color = float4(0, 0, 0, 0);

  return color;
}

technique Technique1
{
  pass Pass1
  {
    PixelShader = compile ps_2_0 PixelShaderFunction();
  }
}