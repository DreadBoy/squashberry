using UnityEngine;
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

	void Awake()
	{

		BlenderLevel.Instaces.Add(this);
	}

	public static void EnableSmoothie()
	{
		Debug.Log("Smoothie Enabled");
		switch (BlenderLevel.Level)
		{
			case 0:
				break;
			case 1:
				Instaces[0].gameObject.transform.Find("Smoothie.004").gameObject.SetActive(true);
				break;
			case 2:
				Instaces[0].gameObject.transform.Find("Smoothie.003").gameObject.SetActive(true);
				break;
			case 3:
				Instaces[0].gameObject.transform.Find("Smoothie.002").gameObject.SetActive(true);
				break;
			case 4:
				Instaces[0].gameObject.transform.Find("Smoothie.001").gameObject.SetActive(true);
				break;
			case 5:
				Instaces[0].gameObject.transform.Find("Smoothie.000").gameObject.SetActive(true);
				break;
		}
	}

	public static void DisableSmoothie()
	{
		switch (BlenderLevel.Level)
		{
			case 0:
				break;
			case 1:
				Instaces[0].gameObject.transform.Find("Smoothie.004").gameObject.SetActive(false);
				break;
			case 2:
				Instaces[0].gameObject.transform.Find("Smoothie.003").gameObject.SetActive(false);
				break;
			case 3:
				Instaces[0].gameObject.transform.Find("Smoothie.002").gameObject.SetActive(false);
				break;
			case 4:
				Instaces[0].gameObject.transform.Find("Smoothie.001").gameObject.SetActive(false);
				break;
			case 5:
				Instaces[0].gameObject.transform.Find("Smoothie.000").gameObject.SetActive(false);
				break;
		}
	}
}
