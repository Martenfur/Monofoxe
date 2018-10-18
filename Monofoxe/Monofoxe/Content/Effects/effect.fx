sampler s0;

struct VertexShaderOutput
{
  float4 Position : POSITION0;
  float2 TexCoords : TEXCOORD0;
  float4 Color : COLOR0;
};


float Solarize(float input)
{
  return 3.0f * input * (1.0 - input);
}

float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
  float4 color = tex2D(s0, input.TexCoords);
  float alpha = color.a;
  color = color / alpha; // Taking in account, that alpha is premultiplied.

 // return float4(1.0 - color.r, 1.0 - color.g, 1.0 - color.b, 1) * alpha;
  return float4(Solarize(color.r), Solarize(color.g), Solarize(color.b), 1) * alpha;

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