﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;

public class TextureUpdater_SetPixels32 : TextureUpdater
{
	public override Texture texture { get; protected set; }

	Color32[] colors32rgb;
	Color32[][] colors32rnd = new Color32[RandomCache][];

	void Start()
	{
		texture = new Texture2D(Width, Height, TextureFormat.ARGB32, false, false);
		texture.filterMode = FilterMode.Point;
		texture.mipMapBias = 0;
		texture.wrapMode = TextureWrapMode.Clamp;
	}


	public override void SetRandom()
	{
		int i = Time.frameCount % RandomCache;
		SetColorsToRandom(ref colors32rnd[i]);
		UpdateTexture(texture as Texture2D, colors32rnd[i]);
	}

	public override void SetRgb()
	{
		SetColorsToRGB(ref colors32rgb);
		UpdateTexture(texture as Texture2D, colors32rgb);
	}
	
	public override void Stop()
	{
	}

	static void UpdateTexture(Texture2D tex, Color32[] cols)
	{
		Profiler.BeginSample("TEST: SetPixels32");
		tex.SetPixels32(cols);
		Profiler.EndSample();

		Profiler.BeginSample("TEST: Apply");
		tex.Apply(false, false);
		Profiler.EndSample();
	}


	static void SetColorsToRGB(ref Color32[] cols)
	{
		if (cols != null)
			return;
		cols = new Color32[Width * Height];
			
		int rgb = 0;
		var r = new Color32(255, 0, 0, 255);
		var g = new Color32(0, 255, 0, 255);
		var b = new Color32(0, 0, 255, 255);
		for (int i = 0; i < cols.Length; i++)
		{
			cols[i] = rgb % 3 == 0 ? r : rgb % 3 == 1 ? g : b;
			rgb++;
		}
	}

	static void SetColorsToRandom(ref Color32[] cols)
	{
		if (cols != null)
			return;
		
		cols = new Color32[Width * Height];

		Profiler.BeginSample("TEST: SetColorsToRandom");
		Color32 c = new Color32(0,0,0,255);
		for (int i = 0; i < cols.Length; i++)
		{
			c.r = c.g = c.b = (byte)(i * 255 / cols.Length);
			cols[i] = c;
		}
		Profiler.EndSample();
	}
}