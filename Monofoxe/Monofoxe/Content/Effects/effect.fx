sampler s0;

struct VertexShaderOutput
{
  float4 Position : POSITION0;
  float2 TexCoords : TEXCOORD0;
  float4 Color : COLOR0;
};


float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
  float4 color = tex2D(s0, input.TexCoords);
  
  return float4(1.0 - color.r, 1.0 - color.g, 1.0 - color.b, 1) * color.a; //float4(1 - color.r, 1 - color.g, 1 - color.b, 1) * color.a;
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