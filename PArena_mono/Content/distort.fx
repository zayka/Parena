//float strength;
//float2 pos;

sampler inputSampler : register(s0); // distortmap
sampler TextureSampler : register(s1); // main


struct PS_input
{
	float4 color    : COLOR0;
	float2 texCoord : TEXCOORD0;
};
const float ZeroOffset = 0.5f / 255.0f;

float4 PixelShaderFunction(PS_input input) : COLOR0
{
	float4 color1;
	float4 color2;
	float2 newcoord=0;


	color1 = tex2D(inputSampler, input.texCoord);
	color1 -= 0.5 - 0.5/255;

	newcoord.x += input.texCoord.x+color1.r*0.05; // 0...1 * 0.1 // (color1.r-0.5)*0.1
	newcoord.y += input.texCoord.y+color1.g*0.05; // 0...1 * 0.1

	color2 = tex2D(TextureSampler, newcoord);
	return color2;	
}

technique Technique1
{
    pass Pass1
    {
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}
