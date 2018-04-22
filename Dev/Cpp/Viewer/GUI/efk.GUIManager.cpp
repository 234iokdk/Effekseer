#define NOMINMAX
#ifdef _WIN32
#define GLEW_STATIC 1
#include <GL/glew.h>
#endif

#include "../EffekseerTool/EffekseerTool.Renderer.h"

#include "efk.GUIManager.h"

#include "efk.JapaneseFont.h"

#include "../3rdParty/imgui_addon/fcurve/fcurve.h"

namespace efk
{
	template <size_t size_>
	struct utf8str {
		enum {size = size_};
		char data[size];
		utf8str(const char16_t* u16str) {
			Effekseer::ConvertUtf16ToUtf8((int8_t*)data, size, (const int16_t*)u16str);
		}
		operator const char*() const {
			return data;
		}
	};

	// http://hasenpfote36.blogspot.jp/2016/09/stdcodecvt.html
	static constexpr std::codecvt_mode mode = std::codecvt_mode::little_endian;

	static std::string utf16_to_utf8(const std::u16string& s)
	{
#if defined(_MSC_VER)
		std::wstring_convert<std::codecvt_utf8_utf16<std::uint16_t, 0x10ffff, mode>, std::uint16_t> conv;
		auto p = reinterpret_cast<const std::uint16_t*>(s.c_str());
		return conv.to_bytes(p, p + s.length());
#else
		std::wstring_convert<std::codecvt_utf8_utf16<char16_t, 0x10ffff, mode>, char16_t> conv;
		return conv.to_bytes(s);
#endif
	}

	static std::u16string utf8_to_utf16(const std::string& s)
	{

#if defined(_MSC_VER)
		std::wstring_convert<std::codecvt_utf8_utf16<std::uint16_t, 0x10ffff, mode>, std::uint16_t> conv;
		auto p = reinterpret_cast<const std::uint16_t*>(s.c_str());
		return std::u16string((const char16_t*)conv.from_bytes(s).c_str());
#else
		std::wstring_convert<std::codecvt_utf8_utf16<char16_t, 0x10ffff, mode>, char16_t> conv;
		return conv.from_bytes(s);
#endif
	}

	static ImTextureID ToImTextureID(ImageResource* image)
	{
		if (image != nullptr)
		{
			Effekseer::TextureData* texture = image->GetTextureData();
			if (texture != nullptr)
			{
				if (texture->UserPtr != nullptr)
				{
					return (ImTextureID)texture->UserPtr;
				}
				else
				{
					return (ImTextureID)texture->UserID;
				}
			}
		}
		return nullptr;
	}

	bool DragFloatN(const char* label, float* v, int components, float v_speed, float v_min, float v_max, 
		const char* display_format1, 
		const char* display_format2,
		const char* display_format3,
		float power)
	{
		ImGuiWindow* window = ImGui::GetCurrentWindow();
		if (window->SkipItems)
			return false;

		ImGuiContext& g = *GImGui;
		bool value_changed = false;
		ImGui::BeginGroup();
		ImGui::PushID(label);
		ImGui::PushMultiItemsWidths(components);

		const char* display_formats[] = {
			display_format1,
			display_format2,
			display_format3
		};

		for (int i = 0; i < components; i++)
		{
			ImGui::PushID(i);
			value_changed |= ImGui::DragFloat("##v", &v[i], v_speed, v_min, v_max, display_formats[i], power);
			ImGui::SameLine(0, g.Style.ItemInnerSpacing.x);
			ImGui::PopID();
			ImGui::PopItemWidth();
		}
		ImGui::PopID();

		ImGui::TextUnformatted(label, ImGui::FindRenderedTextEnd(label));
		ImGui::EndGroup();

		return value_changed;
	}

	bool DragIntN(const char* label, int* v, int components, int v_speed, int v_min, int v_max,
		const char* display_format1,
		const char* display_format2,
		const char* display_format3)
	{
		ImGuiWindow* window = ImGui::GetCurrentWindow();
		if (window->SkipItems)
			return false;

		ImGuiContext& g = *GImGui;
		bool value_changed = false;
		ImGui::BeginGroup();
		ImGui::PushID(label);
		ImGui::PushMultiItemsWidths(components);

		const char* display_formats[] = {
			display_format1,
			display_format2,
			display_format3
		};

		for (int i = 0; i < components; i++)
		{
			ImGui::PushID(i);
			value_changed |= ImGui::DragInt("##v", &v[i], v_speed, v_min, v_max, display_formats[i]);
			ImGui::SameLine(0, g.Style.ItemInnerSpacing.x);
			ImGui::PopID();
			ImGui::PopItemWidth();
		}
		ImGui::PopID();

		ImGui::TextUnformatted(label, ImGui::FindRenderedTextEnd(label));
		ImGui::EndGroup();

		return value_changed;
	}

	GUIManager::GUIManager()
	{}

	GUIManager::~GUIManager()
	{}

	bool GUIManager::Initialize(const char16_t* title, int32_t width, int32_t height, bool isSRGBMode)
	{
		window = new efk::Window();

		if (!window->Initialize(title, width, height, isSRGBMode, true))
		{
			ES_SAFE_DELETE(window);
			return false;
		}

		window->Resized = [this](int x, int y) -> void
		{
			if (this->callback != nullptr)
			{
				this->callback->Resized(x, y);
			}
		};

		window->Focused = [this]() -> void
		{
			if (this->callback != nullptr)
			{
				this->callback->Focused();
			}
		};

		window->Droped = [this](const char* path) -> void
		{
			if (this->callback != nullptr)
			{
				this->callback->SetPath(utf8_to_utf16(path).c_str());
				this->callback->Droped();
			}
		};
		window->MakeCurrent();

#ifdef _WIN32
		glewInit();
#endif
        
		ImGui::CreateContext();
		ImGui_ImplGlfwGL3_Init(window->GetGLFWWindows(), true, nullptr);
		ImGui::StyleColorsDark();

		ImGuiStyle& style = ImGui::GetStyle();

		style.ChildRounding = 3.f;
		style.GrabRounding = 3.f;
		style.WindowRounding = 3.f;
		style.ScrollbarRounding = 3.f;
		style.FrameRounding = 3.f;
		style.WindowTitleAlign = ImVec2(0.5f, 0.5f);

		// mono tone

		for (int32_t i = 0; i < ImGuiCol_COUNT; i++)
		{
			auto v = (style.Colors[i].x + style.Colors[i].y + style.Colors[i].z) / 3.0f;
			style.Colors[i].x = v;
			style.Colors[i].y = v;
			style.Colors[i].z = v;
		}

		style.Colors[ImGuiCol_Text] = ImVec4(0.80f, 0.80f, 0.80f, 1.00f);
		style.Colors[ImGuiCol_TextDisabled] = ImVec4(0.50f, 0.50f, 0.50f, 1.00f);
		style.Colors[ImGuiCol_WindowBg] = ImVec4(0.1f, 0.1f, 0.1f, 0.9f);

		return true;
	}

	void GUIManager::SetTitle(const char16_t* title)
	{
		window->SetTitle(title);
	}

	Vec2 GUIManager::GetSize() const
	{
		return window->GetSize();
	}

	void GUIManager::SetSize(int32_t width, int32_t height)
	{
		window->SetSize(width, height);
	}

	void GUIManager::Terminate()
	{
		ImGui_ImplGlfwGL3_Shutdown();
		ImGui::DestroyContext();

		window->MakeNone();
		window->Terminate();
		ES_SAFE_DELETE(window);
	}

	bool GUIManager::DoEvents()
	{
		return window->DoEvents();
	}

	void GUIManager::Present()
	{
		window->Present();
	}

	void GUIManager::Close()
	{
		window->Close();
	}

	Vec2 GUIManager::GetMousePosition()
	{
		return window->GetMousePosition();
	}

	int GUIManager::GetMouseButton(int32_t mouseButton)
	{
		return window->GetMouseButton(mouseButton);
	}

	int GUIManager::GetMouseWheel()
	{
		return ImGui::GetIO().MouseWheel;
	}

	void GUIManager::SetCallback(GUIManagerCallback* callback)
	{
		this->callback = callback;
	}

	void GUIManager::ResetGUI()
	{
		ImGui_ImplGlfwGL3_NewFrame();
	}

	void GUIManager::RenderGUI()
	{
		ImGui::Render();
		ImGui_ImplGlfwGL3_RenderDrawData(ImGui::GetDrawData());

		glEnable(GL_DEPTH_TEST);
		glDepthMask(GL_TRUE);

		auto bit = GL_DEPTH_BUFFER_BIT | GL_STENCIL_BUFFER_BIT;
		glClearDepth(1.0f);
		glClear(bit);
	}

	void* GUIManager::GetNativeHandle()
	{
		return window->GetNativeHandle();
	}

	bool GUIManager::Begin(const char16_t* name, bool* p_open)
	{
		return ImGui::Begin(utf8str<256>(name), p_open);
	}

	void GUIManager::End()
	{
		ImGui::End();
	}

	bool GUIManager::BeginChild(const char* str_id)
	{
		return ImGui::BeginChild(str_id);
	}

	void GUIManager::EndChild()
	{
		ImGui::EndChild();
	}

	Vec2 GUIManager::GetWindowSize()
	{
		auto v = ImGui::GetWindowSize();
		return Vec2(v.x, v.y);
	}

	void GUIManager::SetNextWindowSize(float size_x, float size_y, Cond cond)
	{
		ImVec2 size;
		size.x = size_x;
		size.y = size_y;

		ImGui::SetNextWindowSize(size, (int)cond);
	}

	void GUIManager::PushItemWidth(float item_width)
	{
		ImGui::PushItemWidth(item_width);
	}

	void GUIManager::PopItemWidth()
	{
		ImGui::PopItemWidth();
	}

	void GUIManager::Separator()
	{
		ImGui::Separator();
	}

	void GUIManager::HiddenSeparator()
	{
		ImGuiWindow* window = ImGui::GetCurrentWindow();
		if (window->SkipItems)
			return;
		ImGuiContext& g = *GImGui;

		ImGuiSeparatorFlags flags = 0;
		if ((flags & (ImGuiSeparatorFlags_Horizontal | ImGuiSeparatorFlags_Vertical)) == 0)
			flags |= (window->DC.LayoutType == ImGuiLayoutType_Horizontal) ? ImGuiSeparatorFlags_Vertical : ImGuiSeparatorFlags_Horizontal;
		IM_ASSERT(ImIsPowerOfTwo((int)(flags & (ImGuiSeparatorFlags_Horizontal | ImGuiSeparatorFlags_Vertical))));   // Check that only 1 option is selected
		if (flags & ImGuiSeparatorFlags_Vertical)
		{
			ImGui::VerticalSeparator();
			return;
		}

		// Horizontal Separator
		if (window->DC.ColumnsSet)
			ImGui::PopClipRect();

		float x1 = window->Pos.x;
		float x2 = window->Pos.x + window->Size.x;
		if (!window->DC.GroupStack.empty())
			x1 += window->DC.IndentX;

		const ImRect bb(ImVec2(x1, window->DC.CursorPos.y), ImVec2(x2, window->DC.CursorPos.y + 4.0f));
		ImGui::ItemSize(ImVec2(0.0f, 0.0f)); // NB: we don't provide our width so that it doesn't get feed back into AutoFit, we don't provide height to not alter layout.
		if (!ImGui::ItemAdd(bb, 0))
		{
			if (window->DC.ColumnsSet)
				ImGui::PushColumnClipRect();
			return;
		}

		window->DrawList->AddLine(bb.Min, ImVec2(bb.Max.x, bb.Min.y), 0);

		if (window->DC.ColumnsSet)
		{
			ImGui::PushColumnClipRect();
			window->DC.ColumnsSet->LineMinY = window->DC.CursorPos.y;
		}
	}

	void GUIManager::SameLine()
	{
		ImGui::SameLine();
	}

	void GUIManager::BeginGroup()
	{
		ImGui::BeginGroup();
	}

	void GUIManager::EndGroup()
	{
		ImGui::EndGroup();
	}

	void GUIManager::SetCursorPosX(float x)
	{
		ImGui::SetCursorPosX(x);
	}

	void GUIManager::SetCursorPosY(float y)
	{
		ImGui::SetCursorPosY(y);
	}

	float GUIManager::GetCursorPosX()
	{
		return ImGui::GetCursorPosX();
	}

	float GUIManager::GetCursorPosY()
	{
		return ImGui::GetCursorPosY();
	}

	float GUIManager::GetTextLineHeight()
	{
		return ImGui::GetTextLineHeight();
	}

	float GUIManager::GetTextLineHeightWithSpacing()
	{
		return ImGui::GetTextLineHeightWithSpacing();
	}

	void GUIManager::Columns(int count, const char* id, bool border)
	{
		ImGui::Columns(count, id, border);
	}

	void GUIManager::NextColumn()
	{
		ImGui::NextColumn();
	}

	float GUIManager::GetColumnWidth(int column_index)
	{
		return ImGui::GetColumnWidth(column_index);
	}

	void GUIManager::SetColumnWidth(int column_index, float width)
	{
		ImGui::SetColumnWidth(column_index, width);
	}

	float GUIManager::GetColumnOffset(int column_index)
	{
		return ImGui::GetColumnOffset(column_index);
	}

	void GUIManager::SetColumnOffset(int column_index, float offset_x)
	{
		ImGui::SetColumnOffset(column_index, offset_x);
	}

	void GUIManager::Text(const char16_t* text)
	{
		ImGui::Text(utf8str<1024>(text));
	}

	void GUIManager::TextWrapped(const char16_t* text)
	{
		ImGui::TextWrapped(utf8str<1024>(text));
	}

	bool GUIManager::Button(const char16_t* label)
	{
		return ImGui::Button(utf8str<256>(label));
	}

	void GUIManager::Image(ImageResource* user_texture_id, float x, float y)
	{
		ImGui::Image(ToImTextureID(user_texture_id), ImVec2(x, y));
	}

	void GUIManager::Image(void* user_texture_id, float x, float y)
	{
		ImGui::Image((ImTextureID)user_texture_id, ImVec2(x, y), ImVec2(0,1), ImVec2(1,0));
	}

	bool GUIManager::ImageButton(ImageResource* user_texture_id, float x, float y)
	{
		return ImGui::ImageButton(ToImTextureID(user_texture_id), ImVec2(x, y));
	}

	bool GUIManager::Checkbox(const char16_t* label, bool* v)
	{
		return ImGui::Checkbox(utf8str<256>(label), v);
	}

	bool GUIManager::RadioButton(const char16_t* label, bool active)
	{
		return ImGui::RadioButton(utf8str<256>(label), active);
	}

	bool GUIManager::InputInt(const char16_t* label, int* v, int step, int step_fast)
	{
		return ImGui::InputInt(utf8str<256>(label), v, step, step_fast);
	}

	bool GUIManager::SliderInt(const char16_t* label, int* v, int v_min, int v_max)
	{
		return ImGui::SliderInt(utf8str<256>(label), v, v_min, v_max);
	}

	bool GUIManager::BeginCombo(const char16_t* label, const char16_t* preview_value, ComboFlags flags, ImageResource* user_texture_id)
	{
		return ImGui::BeginCombo(
			utf8str<256>(label),
			utf8str<256>(preview_value),
			(int)flags, ToImTextureID(user_texture_id));
	}

	void GUIManager::EndCombo()
	{
		ImGui::EndCombo();
	}

	bool GUIManager::DragFloat(const char16_t* label, float* v, float v_speed, float v_min, float v_max, const char* display_format, float power)
	{
		return ImGui::DragFloat(utf8str<256>(label), v, v_speed, v_min, v_max, display_format);
	}

	bool GUIManager::DragFloat2(const char16_t* label, float* v, float v_speed, float v_min, float v_max, const char* display_format, float power)
	{
		return ImGui::DragFloat2(utf8str<256>(label), v, v_speed, v_min, v_max, display_format);
	}

	bool GUIManager::DragFloat3(const char16_t* label, float* v, float v_speed, float v_min, float v_max, const char* display_format, float power)
	{
		return ImGui::DragFloat3(utf8str<256>(label), v, v_speed, v_min, v_max, display_format);
	}

	bool GUIManager::DragFloat4(const char16_t* label, float* v, float v_speed, float v_min, float v_max, const char* display_format, float power)
	{
		return ImGui::DragFloat4(utf8str<256>(label), v, v_speed, v_min, v_max, display_format);
	}

	bool GUIManager::DragFloatRange2(const char16_t* label, float* v_current_min, float* v_current_max, float v_speed, float v_min, float v_max, const char* display_format, const char* display_format_max, float power)
	{
		return ImGui::DragFloatRange2(utf8str<256>(label), v_current_min, v_current_max, v_speed, v_min, v_max, display_format, display_format_max);
	}

	bool GUIManager::DragInt(const char16_t* label, int* v, float v_speed, int v_min, int v_max, const char* display_format)
	{
		return ImGui::DragInt(utf8str<256>(label), v, v_speed, v_min, v_max, display_format);
	}

	bool GUIManager::DragInt2(const char16_t* label, int* v, float v_speed, int v_min, int v_max, const char* display_format)
	{
		return ImGui::DragInt2(utf8str<256>(label), v, v_speed, v_min, v_max, display_format);
	}

	bool GUIManager::DragInt3(const char16_t* label, int* v, float v_speed, int v_min, int v_max, const char* display_format)
	{
		return ImGui::DragInt3(utf8str<256>(label), v, v_speed, v_min, v_max, display_format);
	}

	bool GUIManager::DragInt4(const char16_t* label, int* v, float v_speed, int v_min, int v_max, const char* display_format)
	{
		return ImGui::DragInt4(utf8str<256>(label), v, v_speed, v_min, v_max, display_format);
	}

	bool GUIManager::DragIntRange2(const char16_t* label, int* v_current_min, int* v_current_max, float v_speed, int v_min, int v_max, const char* display_format, const char* display_format_max)
	{
		return ImGui::DragIntRange2(utf8str<256>(label), v_current_min, v_current_max, v_speed, v_min, v_max, display_format, display_format_max);
	}

	bool GUIManager::DragFloat1EfkEx(const char16_t* label, float* v, float v_speed, float v_min, float v_max, const char16_t* display_format1, float power)
	{
		return DragFloatN(
			utf8str<256>(label), v, 1, v_speed, v_min, v_max, 
			utf8str<256>(display_format1), 
			nullptr, 
			nullptr,
			power);
	}

	bool GUIManager::DragFloat2EfkEx(const char16_t* label, float* v, float v_speed, float v_min, float v_max, const char16_t* display_format1, const char16_t* display_format2, float power)
	{
		return DragFloatN(
			utf8str<256>(label), v, 2, v_speed, v_min, v_max,
			utf8str<256>(display_format1),
			utf8str<256>(display_format2),
			nullptr,
			power);
	}

	bool GUIManager::DragFloat3EfkEx(const char16_t* label, float* v, float v_speed, float v_min, float v_max, const char16_t* display_format1, const char16_t* display_format2, const char16_t* display_format3, float power)
	{
		return DragFloatN(
			utf8str<256>(label), v, 3, v_speed, v_min, v_max,
			utf8str<256>(display_format1),
			utf8str<256>(display_format2),
			utf8str<256>(display_format3),
			power);
	}

	bool GUIManager::DragInt2EfkEx(const char16_t* label, int* v, int v_speed, int v_min, int v_max, const char16_t* display_format1, const char16_t* display_format2)
	{
		return DragIntN(
			utf8str<256>(label), v, 2, v_speed, v_min, v_max,
			utf8str<256>(display_format1),
			utf8str<256>(display_format2),
			nullptr);
	}

	static std::u16string inputTextResult;

	bool GUIManager::InputText(const char16_t* label, const char16_t* text)
	{
		auto text_ = utf8str<1024>(text);

		char buf[260];
		memcpy(buf, text_.data, std::min((int32_t)text_.size, 250));
		buf[std::min((int32_t)text_.size, 250)] = 0;

		auto ret = ImGui::InputText(utf8str<256>(label), buf, 260);
	
		inputTextResult = utf8_to_utf16(buf);
	
		return ret;
	}

	const char16_t* GUIManager::GetInputTextResult()
	{
		return inputTextResult.c_str();
	}

	bool GUIManager::ColorEdit4(const char16_t* label, float* col, ColorEditFlags flags)
	{
		return ImGui::ColorEdit4(utf8str<256>(label), col, (int)flags);
	}

	bool GUIManager::TreeNode(const char16_t* label)
	{
		return ImGui::TreeNode(utf8str<256>(label));
	}

	bool GUIManager::TreeNodeEx(const char16_t* label, TreeNodeFlags flags)
	{
		return ImGui::TreeNodeEx(utf8str<256>(label), (int)flags);
	}

	void GUIManager::TreePop()
	{
		ImGui::TreePop();
	}

	bool GUIManager::Selectable(const char16_t* label, bool selected, SelectableFlags flags, ImageResource* user_texture_id)
	{
		return ImGui::Selectable(utf8str<256>(label), selected, (int)flags, ImVec2(0, 0), ToImTextureID(user_texture_id));
	}

	void GUIManager::SetTooltip(const char16_t* text)
	{
		ImGui::SetTooltip(utf8str<256>(text));
	}

	bool GUIManager::BeginMainMenuBar()
	{
		return ImGui::BeginMainMenuBar();
	}

	void GUIManager::EndMainMenuBar()
	{
		ImGui::EndMainMenuBar();
	}

	bool GUIManager::BeginMenuBar()
	{
		return ImGui::BeginMenuBar();
	}

	void GUIManager::EndMenuBar()
	{
		return ImGui::EndMenuBar();
	}

	bool GUIManager::BeginMenu(const char16_t* label, bool enabled)
	{
		return ImGui::BeginMenu(utf8str<256>(label), enabled);
	}

	void GUIManager::EndMenu()
	{
		ImGui::EndMenu();
	}

	bool GUIManager::MenuItem(const char16_t* label, const char* shortcut, bool selected, bool enabled, ImageResource* icon)
	{
		return ImGui::MenuItem(utf8str<256>(label), shortcut, selected, enabled, ToImTextureID(icon));
	}

	bool GUIManager::MenuItem(const char16_t* label, const char* shortcut, bool* p_selected, bool enabled, ImageResource* icon)
	{
		return ImGui::MenuItem(utf8str<256>(label), shortcut, p_selected, enabled, ToImTextureID(icon));
	}

	void GUIManager::OpenPopup(const char* str_id)
	{
		ImGui::OpenPopup(str_id);
	}

	bool GUIManager::BeginPopup(const char* str_id, WindowFlags extra_flags)
	{
		return ImGui::BeginPopup(str_id, (int)extra_flags);
	}

	bool GUIManager::BeginPopupModal(const char16_t* name, bool* p_open, WindowFlags extra_flags)
	{
		return ImGui::BeginPopupModal(utf8str<256>(name), p_open, (int)extra_flags);
	}

	bool GUIManager::BeginPopupContextItem(const char* str_id, int mouse_button)
	{
		return ImGui::BeginPopupContextItem(str_id, mouse_button);
	}

	void GUIManager::EndPopup()
	{
		ImGui::EndPopup();
	}

	bool GUIManager::IsPopupOpen(const char* str_id)
	{
		return ImGui::IsPopupOpen(str_id);
	}

	void GUIManager::CloseCurrentPopup()
	{
		ImGui::CloseCurrentPopup();
	}

	void GUIManager::SetItemDefaultFocus()
	{
		ImGui::SetItemDefaultFocus();
	}

	void GUIManager::AddFontFromFileTTF(const char* filename, float size_pixels)
	{
		ImGuiIO& io = ImGui::GetIO();
		io.Fonts->AddFontFromFileTTF(filename, size_pixels, nullptr, glyphRangesJapanese);
	}

	bool GUIManager::BeginChildFrame(uint32_t id, const Vec2& size, WindowFlags flags)
	{
		return ImGui::BeginChildFrame(id, ImVec2(size.X, size.Y), (int32_t)flags);
	}

	void GUIManager::EndChildFrame()
	{
		ImGui::EndChildFrame();
	}

	bool GUIManager::IsKeyDown(int user_key_index)
	{
		return ImGui::IsKeyDown(user_key_index);
	}

	bool GUIManager::IsMouseDoubleClicked(int button)
	{
		return ImGui::IsMouseDoubleClicked(button);
	}

	bool GUIManager::IsItemHovered()
	{
		return ImGui::IsItemHovered();
	}

	bool GUIManager::IsItemActive()
	{
		return ImGui::IsItemActive();
	}

	bool GUIManager::IsItemFocused()
	{
		return ImGui::IsItemFocused();
	}

	bool GUIManager::IsItemClicked(int mouse_button)
	{
		return ImGui::IsItemClicked(mouse_button);
	}

	bool GUIManager::IsWindowHovered()
	{
		return ImGui::IsWindowHovered();
	}

	bool GUIManager::IsAnyWindowHovered()
	{
		return ImGui::IsAnyWindowHovered();
	}

	bool GUIManager::BeginFullscreen(const char16_t* label)
	{
		ImVec2 windowSize;
		windowSize.x = ImGui::GetIO().DisplaySize.x;
		windowSize.y = ImGui::GetIO().DisplaySize.y - 25;

		ImGui::SetNextWindowSize(windowSize);
		ImGui::SetNextWindowPos(ImVec2(0, 25));
		const ImGuiWindowFlags flags = (ImGuiWindowFlags_NoMove | ImGuiWindowFlags_NoBringToFrontOnFocus | ImGuiWindowFlags_NoResize | ImGuiWindowFlags_NoScrollbar | ImGuiWindowFlags_NoSavedSettings | ImGuiWindowFlags_NoTitleBar);
		const float oldWindowRounding = ImGui::GetStyle().WindowRounding; ImGui::GetStyle().WindowRounding = 0;
		const bool visible = ImGui::Begin(utf8str<256>(label), NULL, ImVec2(0, 0), 1.0f, flags);
		ImGui::GetStyle().WindowRounding = oldWindowRounding;
		return visible;
	}

	void GUIManager::SetNextDock(DockSlot slot)
	{
		ImGui::SetNextDock((ImGuiDockSlot)slot);
	}

	void GUIManager::BeginDockspace()
	{
		ImGui::BeginDockspace();
	}

	void GUIManager::EndDockspace()
	{
		ImGui::EndDockspace();
	}

	bool GUIManager::BeginDock(const char16_t* label, bool* p_open, WindowFlags extra_flags, Vec2 default_size)
	{
		return ImGui::BeginDock(utf8str<256>(label), p_open, (int32_t)extra_flags, ImVec2(default_size.X, default_size.Y));
	}

	void GUIManager::EndDock()
	{
		ImGui::EndDock();
	}

	void GUIManager::SetNextDockRate(float rate)
	{
		ImGui::SetNextDockRate(rate);
	}

	void GUIManager::ResetNextParentDock()
	{
		ImGui::ResetNextParentDock();
	}

	void GUIManager::SaveDock(const char* path)
	{
		ImGui::SaveDock(path);
	}
	
	void GUIManager::LoadDock(const char* path)
	{
		ImGui::LoadDock(path);
	}

	void GUIManager::ShutdownDock()
	{
		ImGui::ShutdownDock();
	}

	bool GUIManager::BeginFCurve(int id)
	{
		return ImGui::BeginFCurve(id);
	}

	void GUIManager::EndFCurve()
	{
		ImGui::EndFCurve();
	}

	bool GUIManager::FCurve(
		int fcurve_id,
		float* keys, float* values,
		float* leftHandleKeys, float* leftHandleValues,
		float* rightHandleKeys, float* rightHandleValues,
		int* interporations,
		FCurveEdgeType startEdge,
		FCurveEdgeType endEdge,
		uint8_t* kv_selected,
		int count,
		float defaultValue,
		bool isLocked,
		bool canControl,
		uint32_t col,
		bool selected,
		int* newCount,
		bool* newSelected,
		float* movedX,
		float* movedY,
		int* changedType)
	{
		return ImGui::FCurve(
			fcurve_id,
			keys,
			values,
			leftHandleKeys,
			leftHandleValues,
			rightHandleKeys,
			rightHandleValues,
			(ImGui::ImFCurveInterporationType*)interporations,
			(ImGui::ImFCurveEdgeType)startEdge,
			(ImGui::ImFCurveEdgeType)endEdge,
			(bool*)kv_selected,
			count,
			defaultValue,
			isLocked,
			canControl,
			col,
			selected,
			newCount,
			newSelected,
			movedX,
			movedY,
			changedType);
	}

	bool GUIManager::BeginDragDropSource()
	{
		return ImGui::BeginDragDropSource();
	}

	bool GUIManager::SetDragDropPayload(const char* type, uint8_t* data, int size)
	{
		return ImGui::SetDragDropPayload(type, data, size);
	}

	void GUIManager::EndDragDropSource()
	{
		ImGui::EndDragDropSource();
	}

	bool GUIManager::BeginDragDropTarget()
	{
		return ImGui::BeginDragDropTarget();
	}

	bool GUIManager::AcceptDragDropPayload(const char* type, uint8_t* data_output, int data_output_size, int* size)
	{
		auto pyload = ImGui::AcceptDragDropPayload(type);
		if (pyload == nullptr)
		{
			*size = 0;
			return false;
		}

		auto max_size = std::min(data_output_size, pyload->DataSize);
		memcpy(data_output, pyload->Data, max_size);
		*size = max_size;

		return true;
	}

	void GUIManager::EndDragDropTarget()
	{
		ImGui::EndDragDropTarget();
	}
}
