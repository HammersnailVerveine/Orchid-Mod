matrix TransformMatrix;

texture Texture0 : register(s0);
sampler textureSampler0 = sampler_state
{
    texture = <Texture0>;
};

texture Texture1 : register(s1);
sampler textureSampler1 = sampler_state
{
    texture = <Texture1>;
};

float Time;

float4 WyvernMoray(float2 coords : TEXCOORD0, float4 sampleColor : COLOR0) : COLOR0
{
    float4 color = tex2D(textureSampler0, coords.xy);
    float4 color2 = tex2D(textureSampler1, coords.xy + float2(Time, 0));
    color.a = (color.r + color.g + color.b) / 3.0;
    color2.a = (color2.r + color2.g + color2.b) / 3.0;
    return color * color2 * sampleColor;
}

technique Technique1
{
    pass WyvernMorayTrail
    {
        PixelShader = compile ps_2_0 WyvernMoray();
    }
}