sampler uImage0 : register(s0);
sampler uImage1 : register(s1);

float2 uImageSize0;
float2 uImageSize1;

float3 uColor;
float3 uSecondaryColor;

float uTime;
float uRotation;
float uOpacity;
float uSaturation;
float uDirection;

float4 uSourceRect;
float2 uWorldPosition;
float2 uTargetPosition;
float3 uLightSource;

float4 uLegacyArmorSourceRect;
float2 uLegacyArmorSheetSize;

float4 TransparentDust(float2 coords : TEXCOORD0, float4 sampleColor : COLOR0) : COLOR0
{
	float4 color = tex2D(uImage0, coords);
	return float4(color.rgb * color.a * sampleColor, color.a);
}

technique Technique1
{
	pass TransparentDust
	{
		PixelShader = compile ps_2_0 TransparentDust();
	}
}