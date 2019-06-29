#include "Common.fxh"

sampler BaseSampler : register(s0);


float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
  float4 color = tex2D(BaseSampler, input.TexCoords.xy);
  float s = (color.r + color.g + color.b) / 3.0;
  return float4(input.TexCoords.x, input.TexCoords.y, s, 1) * color.a;
}


TECHNIQUE(Technique1, PassThroughVertexFunction, PixelShaderFunction);

