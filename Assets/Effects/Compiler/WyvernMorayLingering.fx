sampler textureSampler0 : register(s0);

texture Texture1 : register(s1);
sampler textureSampler1 = sampler_state
{
    texture = <Texture1>;
};

float Time;

float4 WyvernMorayLingering(float2 coords : TEXCOORD0, float4 sampleColor : COLOR0) : COLOR0
{
    float4 color = tex2D(textureSampler0, coords.xy);
    float4 perlin = tex2D(textureSampler1, coords.xy * 0.5f + float2(Time, 0));
    
    perlin.a = (perlin.r + perlin.g + perlin.b) / 3.0;
    
    return color * perlin * sampleColor;
}

technique Technique1
{
    pass WyvernMorayLingering
    {
        PixelShader = compile ps_2_0 WyvernMorayLingering();
    }
}