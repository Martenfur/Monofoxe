#include "Common.fxh"

sampler s0;

float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
  float4 color = tex2D(s0, input.TexCoords.xy);
  float s = (color.r + color.g + color.b) / 3.0 ;
  return float4(s, s, s, 1) * color.a;
}

TECHNIQUE(Technique1, PassThroughVertexFunction, PixelShaderFunction);

