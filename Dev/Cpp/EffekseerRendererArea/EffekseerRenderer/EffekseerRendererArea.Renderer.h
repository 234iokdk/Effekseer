﻿
#ifndef __EFFEKSEERRENDERER_AREA_RENDERER_H__
#define __EFFEKSEERRENDERER_AREA_RENDERER_H__

#include "../../EffekseerRendererCommon/EffekseerRenderer.Renderer.h"
#include "EffekseerRendererArea.Base.h"
#include "EffekseerRendererArea.Renderer.h"

namespace EffekseerRendererArea
{

struct Vertex
{
	::Effekseer::Vector3D Pos;
	::Effekseer::Color Col;
	float UV[2];

	void SetColor(const ::Effekseer::Color& color) { Col = color; }
};

struct RenderedMaterial
{
	std::u16string TexturePath;
	::Effekseer::AlphaBlendType AlphaBlend;
	bool DepthTest;
};

/**
	@brief
	\~english Render class to estimate an area for showing thumbnail
	\~japanese サムネイルを表示するための領域を推定するための描画クラス

*/
class Renderer : public EffekseerRenderer::Renderer, public ::Effekseer::ReferenceObject
{
protected:
	Renderer() {}
	virtual ~Renderer() {}

public:
	/**
		@brief	Create an instance
		@return	instance
	*/
	static Renderer* Create();

	virtual std::vector<Vertex>& GetCurrentVertexes() = 0;
	virtual std::vector<std::array<int32_t, 3>>& GetCurrentIndexes() = 0;
	virtual std::vector<RenderedMaterial>& GetCurrentMaterials() = 0;
	virtual std::vector<int32_t>& GetCurrentMaterialFaceCounts() = 0;
};

} // namespace EffekseerRendererArea

#endif // __EFFEKSEERRENDERER_GL_RENDERER_H__