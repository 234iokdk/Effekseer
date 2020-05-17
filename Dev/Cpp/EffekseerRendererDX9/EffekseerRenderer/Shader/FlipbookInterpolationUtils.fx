
float2 GetFlipbookOneSizeUV(float DivideX, float DivideY)
{
    return (float2(1.0, 1.0) / float2(DivideX, DivideY));
}

float2 GetFlipbookOriginUV(float2 FlipbookUV, float FlipbookIndex, float DivideX, float DivideY)
{
    float2 DivideIndex;
    DivideIndex.x = int(FlipbookIndex) % int(DivideX);
    DivideIndex.y = int(FlipbookIndex) / int(DivideY);

    float2 FlipbookOneSize = GetFlipbookOneSizeUV(DivideX, DivideY);
    float2 UVOffset = DivideIndex * FlipbookOneSize;
    
    float2 OriginUV = FlipbookUV - UVOffset;
    OriginUV *= float2(DivideX, DivideY);
    
    return OriginUV;
}

float2 GetFlipbookUVForIndex(float2 OriginUV, float Index, float DivideX, float DivideY)
{
    float2 DivideIndex;
    DivideIndex.x = int(Index) % int(DivideX);
    DivideIndex.y = int(Index) / int(DivideY);

    float2 FlipbookOneSize = GetFlipbookOneSizeUV(DivideX, DivideY);
    
    return (OriginUV * FlipbookOneSize) + (DivideIndex * FlipbookOneSize);
}

void ApplyFlipbookVS(inout float flipbookRate, inout float2 flipbookUV, float4 flipbookParameter, float flipbookIndex, float2 uv)
{
	if (flipbookParameter.x > 0)
	{
		flipbookRate = frac(flipbookIndex);

		float Index = floor(flipbookIndex);
		float IndexOffset = 1.0;

		float NextIndex = flipbookIndex + IndexOffset;

		// loop none
		if (flipbookParameter.y == 0)
		{
			if (NextIndex >= flipbookParameter.z * flipbookParameter.w)
			{
				NextIndex = (flipbookParameter.z * flipbookParameter.w) - 1;
				Index = (flipbookParameter.z * flipbookParameter.w) - 1;
			}
		}
		// loop
		else if (flipbookParameter.y == 1)
		{
			NextIndex %= (flipbookParameter.z * flipbookParameter.w);
		}
		// loop reverse
		else if (flipbookParameter.y == 2)
		{
			bool Reverse = (floor(NextIndex) / (flipbookParameter.z * flipbookParameter.w)) % 2 == 1;
			NextIndex = int(NextIndex) % (flipbookParameter.z * flipbookParameter.w);
			if (Reverse)
			{
				NextIndex = flipbookParameter.z * flipbookParameter.w - 1 - NextIndex;
			}
		}

		float2 OriginUV = GetFlipbookOriginUV(uv, Index, flipbookParameter.z, flipbookParameter.w);
		flipbookUV = GetFlipbookUVForIndex(OriginUV, NextIndex, flipbookParameter.z, flipbookParameter.w);
	}
}
