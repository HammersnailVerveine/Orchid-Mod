matrix WorldViewProj;

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

struct VertexShaderInput
{
    float2 coord : TEXCOORD0;
    float4 color : COLOR0;
    float4 position : POSITION0;
};

struct VertexShaderOutput
{
    float2 coord : TEXCOORD0;
    float4 color : COLOR0;
    float4 position : SV_POSITION;
};

VertexShaderOutput MainVertexShader(in VertexShaderInput input)
{
    VertexShaderOutput output = (VertexShaderOutput)0;
    output.coord = input.coord;
    output.color = input.color;
    output.position = mul(input.position, WorldViewProj);
    return output;
}

float Time;

float4 WyvernMoray(VertexShaderOutput input) : COLOR
{
    float4 color = tex2D(textureSampler0, input.coord);
    float4 color2 = tex2D(textureSampler1, input.coord + float2(Time, 0));
    color.a = (color.r + color.g + color.b) / 3.0;
    color2.a = (color2.r + color2.g + color2.b) / 3.0;
    return color * color2 * input.color;
}

technique Technique1
{
    pass WyvernMorayTrail
    {
        VertexShader = compile vs_2_0 MainVertexShader();
        PixelShader = compile ps_2_0 WyvernMoray();
    }
}