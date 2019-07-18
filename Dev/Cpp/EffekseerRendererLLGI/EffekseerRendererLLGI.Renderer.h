﻿
#ifndef __EFFEKSEERRENDERER_LLGI_RENDERER_H__
#define __EFFEKSEERRENDERER_LLGI_RENDERER_H__

#include "../EffekseerRendererCommon/EffekseerRenderer.Renderer.h"
#include "EffekseerRendererLLGI.Base.h"

#include <LLGI.Constantbuffer.h>
#include <LLGI.Graphics.h>
#include <LLGI.IndexBuffer.h>
#include <LLGI.VertexBuffer.h>

namespace EffekseerRendererLLGI
{

/**
@brief	テクスチャ読込クラスを生成する。
*/
::Effekseer::TextureLoader* CreateTextureLoader(LLGI::Graphics* graphics, ::Effekseer::FileInterface* fileInterface = NULL);

/**
@brief	モデル読込クラスを生成する。
*/
::Effekseer::ModelLoader* CreateModelLoader(LLGI::Graphics* graphics, ::Effekseer::FileInterface* fileInterface = NULL);

struct FixedShader
{
	std::vector<LLGI::DataStructure> StandardTexture_VS;
	std::vector<LLGI::DataStructure> Standard_VS;
	std::vector<LLGI::DataStructure> StandardDistortedTexture_VS;
	std::vector<LLGI::DataStructure> StandardDistorted_VS;

	std::vector<LLGI::DataStructure> ModelShaderLightingTextureNormal_VS;
	std::vector<LLGI::DataStructure> ModelShaderLightingNormal_VS;
	std::vector<LLGI::DataStructure> ModelShaderLightingTexture_VS;
	std::vector<LLGI::DataStructure> ModelShaderLighting_VS;
	std::vector<LLGI::DataStructure> ModelShaderTexture_VS;
	std::vector<LLGI::DataStructure> ModelShader_VS;
	std::vector<LLGI::DataStructure> ModelShaderDistortionTexture_VS;
	std::vector<LLGI::DataStructure> ModelShaderDistortion_VS;

	std::vector<LLGI::DataStructure> StandardTexture_PS;
	std::vector<LLGI::DataStructure> Standard_PS;
	std::vector<LLGI::DataStructure> StandardDistortedTexture_PS;
	std::vector<LLGI::DataStructure> StandardDistorted_PS;

	std::vector<LLGI::DataStructure> ModelShaderLightingTextureNormal_PS;
	std::vector<LLGI::DataStructure> ModelShaderLightingNormal_PS;
	std::vector<LLGI::DataStructure> ModelShaderLightingTexture_PS;
	std::vector<LLGI::DataStructure> ModelShaderLighting_PS;
	std::vector<LLGI::DataStructure> ModelShaderTexture_PS;
	std::vector<LLGI::DataStructure> ModelShader_PS;
	std::vector<LLGI::DataStructure> ModelShaderDistortionTexture_PS;
	std::vector<LLGI::DataStructure> ModelShaderDistortion_PS;
};

/**
	@brief	Renderer class
*/
class Renderer : public ::EffekseerRenderer::Renderer
{
protected:
	Renderer() {}
	virtual ~Renderer() {}

public:
	/**
		@brief Create renderer
		@param	fixedShader	don't delete before deleting renderer
	*/
	static Renderer* Create(LLGI::Graphics* graphics, FixedShader* fixedShader, bool isReversedDepth, int32_t squareMaxCount);

	virtual LLGI::Graphics* GetGraphics() = 0;

	/**
		@brief	\~English	Get background
				\~Japanese	背景を取得する
	*/
	virtual Effekseer::TextureData* GetBackground() = 0;

	/**
		@brief	\~English	Set background
				\~Japanese	背景を設定する
	*/
	virtual void SetBackground(LLGI::Texture* background) = 0;
};

/**
@brief	\~English	Model
		\~Japanese	モデル
*/
class Model : public Effekseer::Model
{
private:
public:
	struct InternalModel
	{
		LLGI::VertexBuffer* VertexBuffer;
		LLGI::IndexBuffer* IndexBuffer;
		int32_t VertexCount;
		int32_t IndexCount;
		int32_t FaceCount;

		InternalModel()
		{
			VertexBuffer = nullptr;
			IndexBuffer = nullptr;
			VertexCount = 0;
			IndexCount = 0;
			FaceCount = 0;
		}

		virtual ~InternalModel()
		{
			ES_SAFE_RELEASE(VertexBuffer);
			ES_SAFE_RELEASE(IndexBuffer);
		}
	};

	InternalModel* InternalModels = nullptr;
	int32_t ModelCount;

	Model(uint8_t* data, int32_t size) : Effekseer::Model(data, size), InternalModels(nullptr), ModelCount(0)
	{
		this->m_vertexSize = sizeof(VertexWithIndex);
	}

	virtual ~Model() { ES_SAFE_DELETE_ARRAY(InternalModels); }
};

} // namespace EffekseerRendererLLGI

#endif // __EFFEKSEERRENDERER_LLGI_RENDERER_H__
