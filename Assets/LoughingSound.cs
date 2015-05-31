using UnityEngine;
using System.Collections;

public class LoughingSound : MonoBehaviour
{
	public BerryBehaviour Blueberry;

	// Use this for initialization
	void Start()
	{
		Blueberry = transform.parent.gameObject.GetComponent<BerryBehaviour>();

		if (Blueberry == null)
		{
			Debug.Log("No parent found");
		}

		StartCoroutine("laughing");
	}

	void Update()
	{
		if (Blueberry.currentState == BerryBehaviour.BerryState.ToBlender)
		{
			StopAllCoroutines();
			GetComponent<AudioSource>().Stop();
		}
	}

	IEnumerator laughing()
	{
		yield return new WaitForSeconds(Random.Range(3, 6));

		GetComponent<AudioSource>().Play();

		StartCoroutine("laughing");
	}
}
