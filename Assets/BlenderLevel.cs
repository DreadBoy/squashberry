using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BlenderLevel : MonoBehaviour
{
	public Texture EmptyTexture;
	public Texture[] LevelTextures;

	public static List<BlenderLevel> Instaces;

	void Awake()
	{
		BlenderLevel.Instaces.Add(this);
	}

	public static void SetLevel(int level)
	{
		Instaces[0].GetComponent<Renderer>().material.SetTexture(0, Instaces[0].LevelTextures[level]);
	}
}
