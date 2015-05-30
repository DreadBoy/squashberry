using UnityEngine;
using System.Collections;

public class LoughingSound : MonoBehaviour
{

	// Use this for initialization
	void Start()
	{
		StartCoroutine("laughing");
	}

	IEnumerator laughing()
	{
		yield return new WaitForSeconds(Random.Range(3, 6));

		GetComponent<AudioSource>().Play();
	}
}
