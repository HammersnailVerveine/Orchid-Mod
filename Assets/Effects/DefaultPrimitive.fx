matrix WorldViewProj;
bool MultiplyColorByAlpha;

texture Texture0 : register(s0);
sampler textureSampler0 = sampler_state
{
    texture = <Texture0>;
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

float4 DefaultPrimitive(VertexShaderOutput input) : COLOR
{
    float4 color = tex2D(textureSampler0, input.coord);
    if (MultiplyColorByAlpha) color.a = color.rgb / 3.0;
    return color * input.color;
}

technique Technique1
{
    pass DefaultPrimitive
    {
        VertexShader = compile vs_2_0 MainVertexShader();
        PixelShader = compile ps_2_0 DefaultPrimitive();
    }
}