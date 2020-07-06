﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Effekseer.Data
{
	public class AlphaTextureParameter
	{
		[IO(Export = true)]
		[Name(language = Language.Japanese, value = "アルファ画像")]
		[Name(language = Language.English, value = "α Texture")]
		public Value.PathForImage Texture
		{
			get; private set;
		}

		[IO(Export = true)]
		[Name(language = Language.Japanese, value = "フィルタ(アルファ画像)")]
		[Name(language = Language.English, value = "Filter(α Texture)")]
		public Value.Enum<RendererCommonValues.FilterType> Filter { get; private set; }

		[IO(Export = true)]
		[Name(language = Language.Japanese, value = "外側(アルファ画像)")]
		[Name(language = Language.English, value = "Wrap(α Texture)")]
		public Value.Enum<RendererCommonValues.WrapType> Wrap { get; private set; }

		public AlphaTextureParameter()
		{
			Texture = new Value.PathForImage(Resources.GetString("ImageFilter"), true, "");
			Filter = new Value.Enum<RendererCommonValues.FilterType>(RendererCommonValues.FilterType.Linear);
			Wrap = new Value.Enum<RendererCommonValues.WrapType>(RendererCommonValues.WrapType.Repeat);
		}
	}

	public class UVDistortionTextureParameter
	{
		[IO(Export = true)]
		[Name(language = Language.Japanese, value = "UV歪み画像")]
		[Name(language = Language.English, value = "UV Distortion Texture")]
		public Value.PathForImage Texture { get; private set; }

		[IO(Export = true)]
		[Name(language = Language.Japanese, value = "UV歪み強度")]
		[Name(language = Language.English, value = "UV Distortion Intensity")]
		public Value.Float UVDistortionIntensity { get; private set; }

		[IO(Export = true)]
		[Name(language = Language.Japanese, value = "フィルタ(UV歪み画像)")]
		[Name(language = Language.English, value = "Filter(UV Distortion Texture)")]
		public Value.Enum<RendererCommonValues.FilterType> Filter { get; private set; }

		[IO(Export = true)]
		[Name(language = Language.Japanese, value = "外側(UV歪み画像)")]
		[Name(language = Language.English, value = "Wrap(UV DIstortion Texture)")]
		public Value.Enum<RendererCommonValues.WrapType> Wrap { get; private set; }

		public UVDistortionTextureParameter()
		{
			Texture = new Value.PathForImage(Resources.GetString("ImageFilter"), true, "");
			UVDistortionIntensity = new Value.Float(1.0f, 100.0f, -100.0f);
			Filter = new Value.Enum<RendererCommonValues.FilterType>(RendererCommonValues.FilterType.Linear);
			Wrap = new Value.Enum<RendererCommonValues.WrapType>(RendererCommonValues.WrapType.Repeat);
		}
	}

	public enum AdvancedAlphaBlendType : int
	{
		[Name(language = Language.Japanese, value = "アルファブレンド")]
		[Name(language = Language.English, value = "Alpha Blend")]
		Blend = 0,
		[Key(key = "AlphaBlendType_Add")]
		Add = 1,
		[Key(key = "AlphaBlendType_Sub")]
		Sub = 2,
		[Key(key = "AlphaBlendType_Mul")]
		Mul = 3,
	}

	public class BlendTextureParameter
	{
		[IO(Export = true)]
		[Name(language = Language.Japanese, value = "ブレンド画像")]
		[Name(language = Language.English, value = "Blend Texture")]
		public Value.PathForImage Texture { get; private set; }

		[IO(Export = true)]
		[Name(language = Language.Japanese, value = "ブレンドタイプ")]
		[Name(language = Language.English, value = "Blend Type")]
		public Value.Enum<AdvancedAlphaBlendType> BlendType { get; private set; }

		[IO(Export = true)]
		[Name(language = Language.Japanese, value = "フィルター(ブレンド画像)")]
		[Name(language = Language.English, value = "Filter(Blend Texture)")]
		public Value.Enum<RendererCommonValues.FilterType> Filter { get; private set; }

		[IO(Export = true)]
		[Name(language = Language.Japanese, value = "外側(ブレンド画像)")]
		[Name(language = Language.English, value = "Wrap(Blend Texture)")]
		public Value.Enum<RendererCommonValues.WrapType> Wrap { get; private set; }

		public BlendTextureParameter()
		{
			Texture = new Value.PathForImage(Resources.GetString("ImageFilter"), true, "");
			BlendType = new Value.Enum<AdvancedAlphaBlendType>(AdvancedAlphaBlendType.Blend);
			Filter = new Value.Enum<RendererCommonValues.FilterType>(RendererCommonValues.FilterType.Linear);
			Wrap = new Value.Enum<RendererCommonValues.WrapType>(RendererCommonValues.WrapType.Repeat);
		}
	}

	public class BlendAlphaTextureParameter
	{
		[IO(Export = true)]
		[Name(language = Language.Japanese, value = "ブレンドアルファ画像")]
		[Name(language = Language.English, value = "Blend Alpha Texture")]
		public Value.PathForImage Texture { get; private set; }

		[IO(Export = true)]
		[Name(language = Language.Japanese, value = "フィルター(ブレンドアルファ画像)")]
		[Name(language = Language.English, value = "Filter(Blend Alpha Texture)")]
		public Value.Enum<RendererCommonValues.FilterType> Filter { get; private set; }

		[IO(Export = true)]
		[Name(language = Language.Japanese, value = "外側(ブレンドアルファ画像)")]
		[Name(language = Language.English, value = "Wrap(Blend Alpha Texture)")]
		public Value.Enum<RendererCommonValues.WrapType> Wrap { get; private set; }

		public BlendAlphaTextureParameter()
		{
			Texture = new Value.PathForImage(Resources.GetString("ImageFilter"), true, "");
			Filter = new Value.Enum<RendererCommonValues.FilterType>(RendererCommonValues.FilterType.Linear);
			Wrap = new Value.Enum<RendererCommonValues.WrapType>(RendererCommonValues.WrapType.Repeat);
		}
	}

	public class BlendUVDistortionTextureParameter
	{
		[IO(Export = true)]
		[Name(language = Language.Japanese, value = "ブレンドUV歪み画像")]
		[Name(language = Language.English, value = "Blend UV Distortion Texture")]
		public Value.PathForImage Texture { get; private set; }

		[IO(Export = true)]
		[Name(language = Language.Japanese, value = "ブレンドUV歪み強度")]
		[Name(language = Language.English, value = "Blend UV Distortion Intensity")]
		public Value.Float UVDistortionIntensity { get; private set; }

		[IO(Export = true)]
		[Name(language = Language.Japanese, value = "フィルター(ブレンドUV歪み画像)")]
		[Name(language = Language.English, value = "Filter(Blend UV Distortion Texture)")]
		public Value.Enum<RendererCommonValues.FilterType> Filter { get; private set; }

		[IO(Export = true)]
		[Name(language = Language.Japanese, value = "外側(ブレンドUV歪み画像)")]
		[Name(language = Language.English, value = "Wrap(Blend UV Distortion Texture)")]
		public Value.Enum<RendererCommonValues.WrapType> Wrap { get; private set; }

		public BlendUVDistortionTextureParameter()
		{
			Texture = new Value.PathForImage(Resources.GetString("ImageFilter"), true, "");
			UVDistortionIntensity = new Value.Float(1.0f, 100.0f, -100.0f);
			Filter = new Value.Enum<RendererCommonValues.FilterType>(RendererCommonValues.FilterType.Linear);
			Wrap = new Value.Enum<RendererCommonValues.WrapType>(RendererCommonValues.WrapType.Repeat);
		}
	}

	public class BlendTextureParameters
	{
		[IO(Export = true)]
		public BlendTextureParameter BlendTextureParam { get; private set; }

		[Selector(ID = 400)]
		[IO(Export = true)]
		[Name(language = Language.Japanese, value = "ブレンドアルファ画像を有効")]
		[Name(language = Language.English, value = "Enable Blend Alpha Texture")]
		public Value.Boolean EnableBlendAlphaTexture { get; private set; }

		[Selected(ID = 400, Value = 0)]
		[IO(Export = true)]
		public BlendAlphaTextureParameter BlendAlphaTextureParam { get; private set; }

		[Selector(ID = 500)]
		[IO(Export = true)]
		[Name(language = Language.Japanese, value = "ブレンドUV歪み画像を有効")]
		[Name(language = Language.English, value = "Enable Blend UV Distortion Texture")]
		public Value.Boolean EnableBlendUVDistortionTexture { get; private set; }

		[Selected(ID = 500, Value = 0)]
		[IO(Export = true)]
		public BlendUVDistortionTextureParameter BlendUVDistortionTextureParam { get; private set; }

		public BlendTextureParameters()
		{
			BlendTextureParam = new BlendTextureParameter();

			EnableBlendAlphaTexture = new Value.Boolean(false);
			BlendAlphaTextureParam = new BlendAlphaTextureParameter();

			EnableBlendUVDistortionTexture = new Value.Boolean(false);
			BlendUVDistortionTextureParam = new BlendUVDistortionTextureParameter();
		}
	}

	public class AlphaCutoffParameter
	{
		[Selector(ID = 0)]
		[IO(Export = true)]
		public Value.Enum<ParameterType> Type { get; private set; }

		[Selected(ID = 0, Value = 0)]
		[IO(Export = true)]
		public FixedParameter Fixed { get; private set; }

		[Selected(ID = 0, Value = 1)]
		[IO(Export = true)]
		public FourPointInterpolationParameter FourPointInterpolation { get; private set; }

		[Selected(ID = 0, Value = 2)]
		[IO(Export = true)]
		public FloatEasingParamater Easing { get; private set; }

		[Selected(ID = 0, Value = 3)]
		[Name(language = Language.Japanese, value = "Fカーブ")]
		[Name(language = Language.English, value = "F Curve")]
		[IO(Export = true)]
		public Value.FCurveScalar FCurve { get; private set; }

		[Selected(ID = 0, Value = 0)]
		[Selected(ID = 0, Value = 1)]
		[Selected(ID = 0, Value = 2)]
		[Selected(ID = 0, Value = 3)]
		[IO(Export = true)]
		public EdgeParameter EdgeParam { get; private set; }

		public class FixedParameter
		{
			[Name(language = Language.Japanese, value = "アルファ閾値")]
			[Name(language = Language.English, value = "Alpha Threshold")]
			public Value.Float Threshold { get; private set; }

			internal FixedParameter()
			{
				Threshold = new Value.Float(0.0f, 1.0f, 0.0f, 0.05f);
			}
		}

		public class FourPointInterpolationParameter
		{
			[Name(language = Language.Japanese, value = "生成時アルファ閾値")]
			[Name(language = Language.English, value = "Begin Alpha Threshold")]
			public Value.FloatWithRandom BeginThreshold { get; private set; }

			[Name(language = Language.Japanese, value = "遷移フレーム(生成時 -> 第2)")]
			[Name(language = Language.English, value = "Sequence Frame Num")]
			public Value.IntWithRandom TransitionFrameNum { get; private set; }

			[Name(language = Language.Japanese, value = "第2アルファ閾値")]
			[Name(language = Language.English, value = "Second Alpha Threshold")]
			public Value.FloatWithRandom No2Threshold { get; private set; }

			[Name(language = Language.Japanese, value = "第3アルファ閾値")]
			[Name(language = Language.English, value = "Third Alpha Threshold")]
			public Value.FloatWithRandom No3Threshold { get; private set; }

			[Name(language = Language.Japanese, value = "遷移フレーム(第3 -> 消滅時)")]
			[Name(language = Language.English, value = "Sequence Frame Num")]
			public Value.IntWithRandom TransitionFrameNum2 { get; private set; }

			[Name(language = Language.Japanese, value = "消滅時アルファ閾値")]
			[Name(language = Language.English, value = "End Alpha Threshold")]
			public Value.FloatWithRandom EndThreshold { get; private set; }


			internal FourPointInterpolationParameter()
			{
				BeginThreshold = new Value.FloatWithRandom(0.0f, 1.0f, 0.0f, DrawnAs.CenterAndAmplitude, 0.05f);
				TransitionFrameNum = new Value.IntWithRandom(0, int.MaxValue, 0);
				No2Threshold = new Value.FloatWithRandom(0.0f, 1.0f, 0.0f, DrawnAs.CenterAndAmplitude, 0.05f);
				No3Threshold = new Value.FloatWithRandom(0.0f, 1.0f, 0.0f, DrawnAs.CenterAndAmplitude, 0.05f);
				TransitionFrameNum2 = new Value.IntWithRandom(0, int.MaxValue, 0);
				EndThreshold = new Value.FloatWithRandom(0.0f, 1.0f, 0.0f, DrawnAs.CenterAndAmplitude, 0.05f);
			}
		}

		public class EdgeParameter
		{
			[Name(language = Language.Japanese, value = "エッジ閾値")]
			[Name(language = Language.English, value = "Edge Threshold")]
			[IO(Export = true)]
			public Value.Float EdgeThreshold { get; private set; }

			[Name(language = Language.Japanese, value = "エッジカラー")]
			[Name(language = Language.English, value = "Edge Color")]
			[IO(Export = true)]
			public Value.Color EdgeColor { get; private set; }

			[Name(language = Language.Japanese, value = "エッジカラー倍率")]
			[Name(language = Language.English, value = "Edge Color Scaling")]
			[IO(Export = true)]
			public Value.Int EdgeColorScaling { get; private set; }

			public EdgeParameter()
			{
				EdgeThreshold = new Value.Float(0.0f, 1.0f, 0.0f, 0.01f);
				EdgeColor = new Value.Color(255, 255, 255, 255);
				EdgeColorScaling = new Value.Int(1, int.MaxValue, 0);
			}
		}

		public enum ParameterType : int
		{
			[Name(value = "アルファ閾値", language = Language.Japanese)]
			[Name(value = "Set Alpha Threshold", language = Language.English)]
			Fixed = 0,

			[Name(value = "4点補間", language = Language.Japanese)]
			[Name(value = "Four Point Interpolation", language = Language.English)]
			FourPointInterpolation = 1,

			[Name(value = "イージング", language = Language.Japanese)]
			[Name(value = "Easing", language = Language.English)]
			Easing = 2,

			[Name(value = "アルファ閾値(Fカーブ)", language = Language.Japanese)]
			[Name(value = "F-Curve", language = Language.English)]
			FCurve = 3,
		}

		public AlphaCutoffParameter()
		{
			Type = new Value.Enum<ParameterType>(ParameterType.Fixed);
			Fixed = new FixedParameter();
			FourPointInterpolation = new FourPointInterpolationParameter();
			Easing = new FloatEasingParamater(0.0f, 1.0f, 0.0f);
			FCurve = new Value.FCurveScalar(0.0f, 100.0f);

			Fixed.Threshold.CanSelectDynamicEquation = true;
			Easing.Start.CanSelectDynamicEquation = true;
			Easing.End.CanSelectDynamicEquation = true;

			EdgeParam = new EdgeParameter();
		}
	}

	public class AdvancedRenderCommonValues
	{
#if __EFFEKSEER_BUILD_VERSION16__
		[Selector(ID = 100)]
		[IO(Export = true)]
		[Name(language = Language.Japanese, value = "アルファ画像を有効")]
		[Name(language = Language.English, value = "Enable AlphaTexture")]
		public Value.Boolean EnableAlphaTexture { get; private set; }

		[IO(Export = true)]
		[Selected(ID = 100, Value = 0)]
		public AlphaTextureParameter AlphaTextureParam { get; private set; }


		[Selector(ID = 200)]
		[IO(Export = true)]
		[Name(language = Language.Japanese, value = "UV歪み画像を有効")]
		[Name(language = Language.English, value = "Enable UV Distortion Texture")]
		public Value.Boolean EnableUVDistortionTexture { get; private set; }

		[IO(Export = true)]
		[Selected(ID = 200, Value = 0)]
		public UVDistortionTextureParameter UVDistortionTextureParam { get; private set; }

		[Selector(ID = 300)]
		[IO(Export = true)]
		[Name(language = Language.Japanese, value = "ブレンドテクスチャを有効")]
		[Name(language = Language.English, value = "Enable Blend Texture")]
		public Value.Boolean EnableBlendTexture { get; private set; }

		[Selected(ID = 300, Value = 0)]
		[IO(Export = true)]
		public BlendTextureParameters BlendTextureParams { get; private set; }
#endif

		[IO(Export = true)]
		public AlphaCutoffParameter AlphaCutoffParam { get; private set; }

        public AdvancedRenderCommonValues()
        {
#if __EFFEKSEER_BUILD_VERSION16__
			EnableAlphaTexture = new Value.Boolean(false);
			AlphaTextureParam = new AlphaTextureParameter();

			EnableUVDistortionTexture = new Value.Boolean(false);
			UVDistortionTextureParam = new UVDistortionTextureParameter();

			EnableBlendTexture = new Value.Boolean(false);
			BlendTextureParams = new BlendTextureParameters();
#endif

			AlphaCutoffParam = new AlphaCutoffParameter();
        }
    }
}
