#include "EffectPlatformGL.h"
#include "../../3rdParty/stb/stb_image_write.h"

EffekseerRenderer::Renderer* EffectPlatformGL::CreateRenderer()
{
	return EffekseerRendererGL::Renderer::Create(2000, EffekseerRendererGL::OpenGLDeviceType::OpenGL3);
}

EffectPlatformGL::~EffectPlatformGL() {}

void EffectPlatformGL::InitializeDevice(const EffectPlatformInitializingParameter& param) {}

void EffectPlatformGL::BeginRendering()
{
	glClearColor(0.0f, 0.0f, 0.0f, 1.0f);
	glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);
	glDrawPixels(1280, 720, GL_RGBA, GL_UNSIGNED_BYTE, checkeredPattern_.data());
	glFlush();
	glFinish();
}

void EffectPlatformGL::EndRendering() {}

bool EffectPlatformGL::TakeScreenshot(const char* path)
{
	glFlush();
	glFinish();

	glViewport(0, 0, 1280, 720);
	glReadBuffer(GL_BACK);
	glPixelStorei(GL_UNPACK_ALIGNMENT, 1);

	std::vector<uint8_t> data;
	data.resize(1280 * 720 * 4);
	glReadPixels(0, 0, 1280, 720, GL_RGBA, GL_UNSIGNED_BYTE, (void*)data.data());

	std::vector<uint8_t> temp = data;

	for (int32_t y = 0; y < 720; y++)
	{
		for (int32_t x = 0; x < 1280 * 4; x++)
		{
			data[x + y * 1280 * 4] = temp[x + (720 - 1 - y) * 1280 * 4];
		}
	}

	// HACK for Geforce
	for (int32_t i = 0; i < 1280 * 720; i++)
	{
		data[i * 4 + 3] = 255;
	}

	stbi_write_png(path, 1280, 720, 4, data.data(), 1280 * 4);

	return true;
}
