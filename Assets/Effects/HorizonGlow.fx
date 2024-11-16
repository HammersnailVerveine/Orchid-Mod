sampler uImage0 : register(s0);
sampler uImage1 : register(s1);
float3 uColor;
float3 uSecondaryColor;
float uOpacity;
float uSaturation;
float uRotation;
float uTime;
float4 uSourceRect;
float2 uWorldPosition;
float uDirection;
float3 uLightSource;
float2 uImageSize0;
float2 uImageSize1;
float2 uTargetPosition;
float4 uLegacyArmorSourceRect;
float2 uLegacyArmorSheetSize;

float4 PixelShaderFunction(float4 sampleColor : COLOR0, float2 coords : TEXCOORD0) : COLOR0
{
    float fade = (sampleColor.r + sampleColor.b + sampleColor.g) / 3 + sampleColor.a;
    if (fade == 0) return sampleColor;
	float4 color = tex2D(uImage0, coords);
    float lumi = 0.3 * color.r * color.r + 0.4 * color.g * color.g + 0.2 * color.b * color.b;
    color.rgb = (color.rgb / 1.5 + lumi) * (0.6 + sampleColor.a * 0.4);
    float frameY = (coords.y * uImageSize0.y - uSourceRect.y) / uSourceRect.w;
    float sinTime = sin(uTime + frameY * 8);
    float cosTime = cos((uTime + frameY * 8) * 2) + 1;
    if (sinTime > 0)
    {
        color.r *= 3.4 * sinTime + cosTime;
        color.b *= (sqrt(color.b) * 0.67 + 0.04) * sinTime + cosTime;
    }
    else
    {
        color.r *= (0.745 + 0.077) * -sinTime + cosTime;
        color.b *= 3.15 * -sinTime + cosTime;
    }
    color.g = 1.5 - 0.77 / (color.g + 0.375) + color.g * cosTime;
    color.a *= + sampleColor.a;
    if (fade < 0.4f) color *= fade * 3;
	return color;
}

technique Technique1
{
	pass HorizonShaderPass
	{
		PixelShader = compile ps_2_0 PixelShaderFunction();
	}
}