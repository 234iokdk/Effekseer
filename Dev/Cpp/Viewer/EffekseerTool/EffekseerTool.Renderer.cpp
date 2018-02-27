﻿
#include "EffekseerTool.Renderer.h"
#include "EffekseerTool.Grid.h"
#include "EffekseerTool.Guide.h"
#include "EffekseerTool.Culling.h"
#include "EffekseerTool.Paste.h"

#include "../EffekseerRendererCommon/EffekseerRenderer.PngTextureLoader.h"

namespace EffekseerTool
{
	Renderer::DistortingCallback::DistortingCallback(efk::Graphics* renderer)
		: renderer(renderer)
	{
		IsEnabled = true;
		Blit = true;
	}

	Renderer::DistortingCallback::~DistortingCallback()
	{
	
	}

	bool Renderer::DistortingCallback::OnDistorting()
	{
		if (Blit)
		{
			renderer->CopyToBackground();

			if (renderer->GetDeviceType() == efk::DeviceType::OpenGL)
			{
				auto r = (::EffekseerRendererGL::Renderer*)renderer->GetRenderer();
				//r->SetBackground(renderer->GetBack());
			}
			else
			{
				auto r = (::EffekseerRendererDX9::Renderer*)renderer->GetRenderer();
				r->SetBackground((IDirect3DTexture9*)renderer->GetBack());
			}
		}

		return IsEnabled;
	}

	Renderer::Renderer(int32_t squareMaxCount, bool isSRGBMode, bool isOpenGLMode)
		: m_handle(NULL)
		, m_width(0)
		, m_height(0)
		, m_squareMaxCount(squareMaxCount)
		, m_projection(PROJECTION_TYPE_PERSPECTIVE)
		, m_renderer(NULL)

		, RateOfMagnification(1.0f)

		, m_grid(NULL)
		, m_guide(NULL)
		, m_culling(NULL)
		, m_background(NULL)

		, GuideWidth(100)
		, GuideHeight(100)
		, RendersGuide(false)

		, IsGridShown(true)

		, IsGridXYShown(false)
		, IsGridXZShown(true)
		, IsGridYZShown(false)

		, IsRightHand(true)
		, GridLength(2.0f)

		, IsCullingShown(false)
		, CullingRadius(0.0f)
		, CullingPosition()

		, Distortion(eDistortionType::DistortionType_Current)

		, m_isSRGBMode(isSRGBMode)

		, BackgroundColor(0, 0, 0, 255)
		, GridColor(255, 255, 255, 255)
		, IsBackgroundTranslucent(false)
	{
		if (isOpenGLMode)
		{
			graphics = new efk::GraphicsGL();
		}
		else
		{
			graphics = new efk::GraphicsDX9();
		}
	}

	Renderer::~Renderer()
	{
		assert(!m_recording);

		if (backgroundData != nullptr)
		{
			textureLoader->Unload(backgroundData);
			backgroundData = nullptr;
		}

		ES_SAFE_DELETE(textureLoader);

		ES_SAFE_DELETE(m_guide);
		ES_SAFE_DELETE(m_grid);
		ES_SAFE_DELETE(m_culling);

		ES_SAFE_DELETE(m_background);

		ES_SAFE_DELETE(graphics);
	}

bool Renderer::Initialize( HWND handle, int width, int height )
{
	if (!graphics->Initialize(handle, width, height, m_isSRGBMode, m_squareMaxCount))
	{
		return false;
	}

	HRESULT hr;

	m_handle = handle;
	m_width = width;
	m_height = height;

	m_distortionCallback = new DistortingCallback(graphics);
	m_renderer = graphics->GetRenderer();
	m_renderer->SetDistortingCallback(m_distortionCallback);

	// グリッド生成
	m_grid = ::EffekseerRenderer::Grid::Create(graphics);

	// ガイド作成
	m_guide = ::EffekseerRenderer::Guide::Create(graphics);

	m_culling = ::EffekseerRenderer::Culling::Create(graphics);

	// 背景作成
	m_background = ::EffekseerRenderer::Paste::Create(graphics);


	if( m_projection == PROJECTION_TYPE_PERSPECTIVE )
	{
		SetPerspectiveFov( width, height );
	}
	else if( m_projection == PROJECTION_TYPE_ORTHOGRAPHIC )
	{
		SetOrthographic( width, height );
	}

	textureLoader = graphics->GetRenderer()->CreateTextureLoader();

	return true;
}

bool Renderer::Present()
{
	return graphics->Present();
}

void Renderer::ResetDevice()
{
	if (backgroundData != nullptr)
	{
		textureLoader->Unload(backgroundData);
		backgroundData = nullptr;
	}

	if (LostedDevice != nullptr)
	{
		LostedDevice();
	}

	graphics->ResetDevice();

	if (ResettedDevice != nullptr)
	{
		ResettedDevice();
	}

	backgroundData = textureLoader->Load(backgroundPath.c_str(), Effekseer::TextureType::Color);
}

eProjectionType Renderer::GetProjectionType()
{
	return m_projection;
}

void Renderer::SetProjectionType( eProjectionType type )
{
	m_projection = type;

	if( m_projection == PROJECTION_TYPE_PERSPECTIVE )
	{
		SetPerspectiveFov( m_width, m_height );
	}
	else if( m_projection == PROJECTION_TYPE_ORTHOGRAPHIC )
	{
		SetOrthographic( m_width, m_height );
	}
}

void Renderer::SetPerspectiveFov( int width, int height )
{
	::Effekseer::Matrix44 proj;

	if (graphics->GetDeviceType() == efk::DeviceType::OpenGL)
	{
		if (IsRightHand)
		{
			// Right hand coordinate
			proj.PerspectiveFovRH_OpenGL(60.0f / 180.0f * 3.141592f, (float)width / (float)height, 1.0f, 300.0f);
		}
		else
		{
			// Left hand coordinate
			proj.PerspectiveFovLH_OpenGL(60.0f / 180.0f * 3.141592f, (float)width / (float)height, 1.0f, 300.0f);
		}
	}
	else
	{
		if (IsRightHand)
		{
			// Right hand coordinate
			proj.PerspectiveFovRH(60.0f / 180.0f * 3.141592f, (float)width / (float)height, 1.0f, 300.0f);
		}
		else
		{
			// Left hand coordinate
			proj.PerspectiveFovLH(60.0f / 180.0f * 3.141592f, (float)width / (float)height, 1.0f, 300.0f);
		}
	}
	

	proj.Values[0][0] *= RateOfMagnification;
	proj.Values[1][1] *= RateOfMagnification;

	m_renderer->SetProjectionMatrix( proj );
}

void Renderer::SetOrthographic( int width, int height )
{
	::Effekseer::Matrix44 proj;

	if( IsRightHand )
	{
		// Right hand coordinate
		proj.OrthographicRH( (float)width / 16.0f / RateOfMagnification, (float)height / 16.0f / RateOfMagnification, 1.0f, 300.0f );
	}
	else
	{
		// Left hand coordinate
		proj.OrthographicLH( (float)width / 16.0f / RateOfMagnification, (float)height / 16.0f / RateOfMagnification, 1.0f, 300.0f );
	}

	m_renderer->SetProjectionMatrix( proj );
}

bool Renderer::Resize( int width, int height )
{
	m_width = width;
	m_height = height;

	if( m_projection == PROJECTION_TYPE_PERSPECTIVE )
	{
		SetPerspectiveFov( width, height );
	}
	else if( m_projection == PROJECTION_TYPE_ORTHOGRAPHIC )
	{
		SetOrthographic( width, height );
	}

	graphics->Resize(width, height);

	ResetDevice();

	return true;
}

void Renderer::RecalcProjection()
{
	if( m_projection == PROJECTION_TYPE_PERSPECTIVE )
	{
		SetPerspectiveFov( m_width, m_height );
	}
	else if( m_projection == PROJECTION_TYPE_ORTHOGRAPHIC )
	{
		SetOrthographic( m_width, m_height );
	}
}

bool Renderer::BeginRendering()
{
	HRESULT hr;

	graphics->BeginScene();

	if (!m_recording)
	{
		graphics->Clear(Effekseer::Color(0, 0, 0, 0));
	}

	if( m_recording && IsBackgroundTranslucent )
	{
		graphics->Clear(Effekseer::Color(0, 0, 0, 0));
	}
	else
	{
		graphics->Clear(Effekseer::Color(BackgroundColor.R, BackgroundColor.G, BackgroundColor.B, 255));
	}

	/* 背景 */
	if( !m_recording && backgroundData != nullptr)
	{
		// 値は適当(背景は画面サイズと一致しないので問題ない)
		m_background->Rendering((IDirect3DTexture9*)backgroundData->UserPtr, 1024, 1024);
	}
	else if(!m_recording)
	{
		m_background->Rendering(nullptr, 1024, 1024);
	}

	if( !m_recording && IsGridShown )
	{
		m_grid->SetLength( GridLength );
		m_grid->IsShownXY = IsGridXYShown;
		m_grid->IsShownXZ = IsGridXZShown;
		m_grid->IsShownYZ = IsGridYZShown;
		m_grid->Rendering(GridColor, IsRightHand);
	}

	if( !m_recording )
	{
		m_culling->IsShown = IsCullingShown;
		m_culling->Radius = CullingRadius;
		m_culling->X = CullingPosition.X;
		m_culling->Y = CullingPosition.Y;
		m_culling->Z = CullingPosition.Z;
		m_culling->Rendering( IsRightHand );
	}

	// ガイド部分が描画されるように拡大
	if (m_recording)
	{
		m_cameraMatTemp = m_renderer->GetCameraMatrix();
		m_projMatTemp = m_renderer->GetProjectionMatrix();
		auto proj = m_projMatTemp;

		::Effekseer::Matrix44 mat;
		mat.Values[0][0] = (float) m_width / (float) GuideWidth;
		mat.Values[1][1] = (float) m_height / (float) GuideHeight;
		::Effekseer::Matrix44::Mul(proj, proj, mat);

		m_renderer->SetProjectionMatrix(proj);
	}

	// Distoriton
	if (Distortion == eDistortionType::DistortionType_Current)
	{
		CopyToBackground();
		
		m_distortionCallback->Blit = false;
		m_distortionCallback->IsEnabled = true;
	}
	else if (Distortion == eDistortionType::DistortionType_Effekseer120)
	{
		m_distortionCallback->Blit = true;
		m_distortionCallback->IsEnabled = true;
	}
	else
	{
		if (graphics->GetDeviceType() == efk::DeviceType::OpenGL)
		{
			auto r = (::EffekseerRendererGL::Renderer*)graphics->GetRenderer();
		}
		else
		{
			auto r = (EffekseerRendererDX9::RendererImplemented*)m_renderer;
			r->SetBackground(nullptr);
		}

		m_distortionCallback->Blit = false;
		m_distortionCallback->IsEnabled = false;
	}

	m_renderer->BeginRendering();
	
	return true;
}

bool Renderer::EndRendering()
{
	m_renderer->EndRendering();

	if( RendersGuide && !m_recording )
	{
		m_guide->Rendering( m_width, m_height, GuideWidth, GuideHeight );
	}

	if (!m_recording)
	{
		if (graphics->GetDeviceType() == efk::DeviceType::OpenGL)
		{
			auto r = (::EffekseerRendererGL::Renderer*)graphics->GetRenderer();
		}
		else
		{
			auto r = (EffekseerRendererDX9::RendererImplemented*)m_renderer;
			r->SetBackground(nullptr);
		}

	}

	if (m_recording)
	{
		m_renderer->SetCameraMatrix(m_cameraMatTemp);
		m_renderer->SetProjectionMatrix(m_projMatTemp);
	}
	
	graphics->EndScene();

	return true;
}

bool Renderer::BeginRecord( int32_t width, int32_t height )
{
	assert( !m_recording );
	
	m_recordingWidth = width;
	m_recordingHeight = height;

	graphics->BeginRecord(m_recordingWidth, m_recordingHeight);

	m_recording = true;

	return true;
}

void Renderer::EndRecord(std::vector<Effekseer::Color>& pixels, bool generateAlpha, bool removeAlpha)
{
	assert(m_recording);

	graphics->EndRecord(pixels);

	auto f2b = [](float v) -> uint8_t
	{
		auto v_ = v * 255;
		if (v_ > 255) v_ = 255;
		if (v_ < 0) v_ = 0;
		return v_;
	};

	auto b2f = [](uint8_t v) -> float
	{
		auto v_ = (float)v / 255.0f;
		return v_;
	};

	// 強制透明化
	for (int32_t i = 0; i < m_recordingWidth * m_recordingHeight; i++)
	{
		if (generateAlpha)
		{
			auto rf = b2f(pixels[i].R);
			auto gf = b2f(pixels[i].G);
			auto bf = b2f(pixels[i].B);
			auto oaf = b2f(pixels[i].A);

			rf = rf * oaf;
			gf = gf * oaf;
			bf = bf * oaf;

			auto af = rf;
			af = Effekseer::Max(af, gf);
			af = Effekseer::Max(af, bf);

			if (af > 0.0f)
			{
				pixels[i].R = f2b(rf / af);
				pixels[i].G = f2b(gf / af);
				pixels[i].B = f2b(bf / af);
			}

			pixels[i].A = f2b(af);
		}

		if (removeAlpha)
		{
			pixels[i].A = 255;
		}
	}

	m_recording = false;
}

void Renderer::LoadBackgroundImage(const char16_t* path)
{
	backgroundPath = path;
	
	if (backgroundData != nullptr)
	{
		textureLoader->Unload(backgroundData);
		backgroundData = nullptr;
	}

	backgroundData = textureLoader->Load(path, Effekseer::TextureType::Color);
}

void Renderer::CopyToBackground()
{
	graphics->CopyToBackground();

	if (graphics->GetDeviceType() == efk::DeviceType::OpenGL)
	{
		auto r = (::EffekseerRendererGL::Renderer*)graphics->GetRenderer();
	}
	else
	{
		auto r = (::EffekseerRendererDX9::Renderer*)graphics->GetRenderer();
		r->SetBackground((IDirect3DTexture9*)graphics->GetBack());
	}
}

}
