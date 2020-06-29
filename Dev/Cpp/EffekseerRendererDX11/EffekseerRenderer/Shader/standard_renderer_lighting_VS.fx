
struct VS_Input
{
	float3 Pos : POSITION0;
	float4 Color : NORMAL0;
	float4 Normal : NORMAL1;
	float4 Tangent : NORMAL2;
	float2 UV1 : TEXCOORD0;
	float2 UV2 : TEXCOORD1;
#ifdef __EFFEKSEER_BUILD_VERSION16__
    float2 AlphaUV          : TEXCOORD2;
    float2 UVDistortionUV   : TEXCOORD3;
    float2 BlendUV          : TEXCOORD4;
    float2 BlendAlphaUV     : TEXCOORD5;
    float2 BlendUVDistortionUV : TEXCOORD6;
    float FlipbookIndex     : TEXCOORD7;
    float AlphaThreshold    : TEXCOORD8;
#endif
};

struct VS_Output
{
	float4 Position : SV_POSITION;
	float4 VColor : COLOR;
	float2 UV1 : TEXCOORD0;
	float2 UV2 : TEXCOORD1;
	float3 WorldP : TEXCOORD2;
	float3 WorldN : TEXCOORD3;
	float3 WorldT : TEXCOORD4;
	float3 WorldB : TEXCOORD5;
	float2 ScreenUV : TEXCOORD6;
#ifdef __EFFEKSEER_BUILD_VERSION16__
    float2 AlphaUV              : TEXCOORD7;
    float2 UVDistortionUV       : TEXCOORD8;
    float2 BlendUV              : TEXCOORD9;
    float2 BlendAlphaUV         : TEXCOORD10;
    float2 BlendUVDistortionUV  : TEXCOORD11;
    float FlipbookRate          : TEXCOORD12;
    float2 FlipbookNextIndexUV  : TEXCOORD13;
    float AlphaThreshold        : TEXCOORD14;
#endif
};

#ifdef __EFFEKSEER_BUILD_VERSION16__
cbuffer VS_ConstantBuffer : register(b0)
{
    float4x4 mCamera;
    float4x4 mProj;
    float4 mUVInversed;

    float4 mflipbookParameter; // x:enable, y:loopType, z:divideX, w:divideY
};

#include "FlipbookInterpolationUtils.fx"
#else
float4x4 mCamera : register(c0);
float4x4 mProj : register(c4);
float4 mUVInversed : register(c8);
#endif

VS_Output VS( const VS_Input Input )
{
	VS_Output Output = (VS_Output)0;
	float3 worldPos = Input.Pos;
	float3 worldNormal = (Input.Normal.xyz - float3(0.5, 0.5, 0.5)) * 2.0;
	float3 worldTangent = (Input.Tangent.xyz - float3(0.5, 0.5, 0.5)) * 2.0;
	float3 worldBinormal = cross(worldNormal, worldTangent);

	// UV
	float2 uv1 = Input.UV1;
	float2 uv2 = Input.UV1;
	uv1.y = mUVInversed.x + mUVInversed.y * uv1.y;
	uv2.y = mUVInversed.x + mUVInversed.y * uv2.y;
#ifdef __EFFEKSEER_BUILD_VERSION16__
    // alpha texture
    float2 alphaUV = Input.AlphaUV;
    alphaUV.y = mUVInversed.x + mUVInversed.y * alphaUV.y;
    
    // uv distortion texture
    float2 uvDistorionUV = Input.UVDistortionUV;
    uvDistorionUV.y = mUVInversed.x + mUVInversed.y * uvDistorionUV.y;
    
    // blend texture
    float2 blendUV = Input.BlendUV;
    blendUV.y = mUVInversed.x + mUVInversed.y * blendUV.y;
    
    // blend alpha texture
    float2 blendAlphaUV = Input.BlendAlphaUV;
    blendAlphaUV.y = mUVInversed.x + mUVInversed.y * blendAlphaUV.y;
    
    // blend uv distortion texture
    float2 blendUVDistortionUV = Input.BlendUVDistortionUV;
    blendUVDistortionUV.y = mUVInversed.x + mUVInversed.y * blendUVDistortionUV.y;
    
    // flipbook interpolation
    ApplyFlipbookVS(Output.FlipbookRate, Output.FlipbookNextIndexUV, mflipbookParameter, Input.FlipbookIndex, Output.UV1);

    // alpha threshold
    Output.AlphaThreshold = Input.AlphaThreshold;
#endif

	// NBT
	Output.WorldN = worldNormal;
	Output.WorldB = worldBinormal;
	Output.WorldT = worldTangent;

	float3 pixelNormalDir = float3(0.5, 0.5, 1.0);

	float4 cameraPos = mul(mCamera, float4(worldPos, 1.0));
	cameraPos = cameraPos / cameraPos.w;
	Output.Position = mul(mProj, cameraPos);

	Output.WorldP = worldPos;
	Output.VColor = Input.Color;
	Output.UV1 = uv1;
	Output.UV2 = uv2;
#ifdef __EFFEKSEER_BUILD_VERSION16__
    Output.AlphaUV = alphaUV;
    Output.UVDistortionUV = uvDistorionUV;
    Output.BlendUV = blendUV;
    Output.BlendAlphaUV = blendAlphaUV;
    Output.BlendUVDistortionUV = blendUVDistortionUV;
#endif
	Output.ScreenUV = Output.Position.xy / Output.Position.w;
	Output.ScreenUV.xy = float2(Output.ScreenUV.x + 1.0, 1.0 - Output.ScreenUV.y) * 0.5;

	return Output;
}

