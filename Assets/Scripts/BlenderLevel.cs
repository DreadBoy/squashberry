﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BlenderLevel : MonoBehaviour
{
	public Texture[] LevelTextures;

	public static List<BlenderLevel> Instaces = new List<BlenderLevel>();

	private static int _level;
	public static int Level
	{
		get
		{
			return _level;
		}
		set
		{
			if (value < 0 || value > 5)
			{
				Debug.Log("Blender level is out of range");
				GameObject.Find("Compartment").GetComponent<Renderer>().material.SetTexture(0, Instaces[0].LevelTextures[0]);
			}
			else
			{
				GameObject.Find("Compartment").GetComponent<Renderer>().material.SetTexture(0, Instaces[0].LevelTextures[value]);
				_level = value;
				Debug.Log("Texture set");
			}
		}
	}

	void Start()
	{

		BlenderLevel.Instaces.Add(this);
	}
}