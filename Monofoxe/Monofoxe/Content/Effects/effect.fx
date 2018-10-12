sampler s0;
float param1;

float4 PixelShaderFunction(float2 coords : TEXCOORD0) : COLOR0
{
  float4 color = tex2D(s0, coords);

  return float4(1 - color.r, 1 - color.g, 1 - color.b, 1) * color.a;
}

technique Technique1
{
  pass Pass1
  {
    //AlphaBlendEnable = true;
    //DestBlend = DESTALPHA;
    //SrcBlend = SRCALPHA;
    
    PixelShader = compile ps_2_0 PixelShaderFunction();
  }
}