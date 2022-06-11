sampler uImage0 : register(s0);

texture PerlinTexture : register(s1);
sampler2D perlinSampler = sampler_state { texture = <PerlinTexture>; };

float Time;
float Radius;
float Thickness;

float4 Color;
float4 Color2;

struct VertexShaderInput
{
    float2 coord : TEXCOORD0;
    float4 Position : POSITION0;
};

struct VertexShaderOutput
{
    float2 coord : TEXCOORD0;
    float4 Position : SV_POSITION;
};

VertexShaderOutput MainVS(in VertexShaderInput input)
{
    VertexShaderOutput output = (VertexShaderOutput) 0;
    output.coord = input.coord;
    output.Position = input.Position;
    return output;
}

float4 ShroomiteScepter(VertexShaderOutput input) : COLOR
{
    float2 uv = floor(input.coord * Radius) / Radius;
    
    float2 center = float2(0.5f, 0.5f);
    float4 ret = float4(0, 0, 0, 0);
    
    //float2 pixel = (uv * imageSize) - sourceRect.xy + uWorldPosition * parallax;
    
    float4 noise = tex2D(perlinSampler, uv + float2(0, Time * 0.05f));
    
    float dist = distance(uv, center);  
    float offset = 0.01f - noise.r * 0.02f;
    float fff = 0.5f / Radius * Thickness;
    
    float ddd = dist + fff + offset;
    if (ddd <= 0.5f) ret = Color2 * (1 - noise.r * 0.5f) * lerp(0, 1, (ddd - 0.435f) * 10);
    
    if ((dist - offset) <= 0.5f && dist > (0.5f - fff - offset)) ret = lerp(Color, Color2, noise.g);
    
    return ret;
}

technique Technique1
{
    pass ShroomiteScepter
    {
        PixelShader = compile ps_2_0 ShroomiteScepter();
    }
}