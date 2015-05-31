using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BlenderButtonPress : MonoBehaviour
{
	public GameObject Blender;

	public void ButtonPress()
	{
		Blender.GetComponent<Animator>().SetTrigger("ButtonPress");
		GetComponent<AudioSource>().Play();
		Blender.GetComponent<AudioSource>().Play();

		//BerryBehaviour.BerryState b = BerryBehaviour.BerryState.Die;
		BlenderBehaviour.instance.currentState = BlenderState.Blend;

		StartCoroutine(mixBerries());
	}

	IEnumerator mixBerries()
	{
		GetComponent<BoxCollider>().enabled = false;

		yield return new WaitForSeconds(1.5f);

		//foreach (GameObject g in GameObject.FindGameObjectsWithTag("Berry"))
		//{
		//	if (g.GetComponent<BerryBehaviour>().currentState == BerryBehaviour.BerryState.InBlender)
		//	{
		//		Destroy(g);
		//	}
		//}

		Debug.Log(BlenderBehaviour.berriesInBlender.Count);

		int berriesInBlender = BlenderBehaviour.berriesInBlender.Count;

		foreach (BerryBehaviour n in BlenderBehaviour.berriesInBlender)
		{
			Destroy(n.gameObject);
			Debug.Log("Berries destroyed");
		}

		BlenderLevel.EnableSmoothie();

		yield return new WaitForSeconds(1.8f);

		GetComponent<BoxCollider>().enabled = true;

		Debug.Log("Score multiplier");
		Settings.saveData.Score += berriesInBlender * BlenderLevel.Level * 10;
		GameObject.Find("Settings").GetComponent<Settings>().scoreDisplay.GetComponent<Text>().text = "Score: " + Settings.saveData.Score;

		BlenderLevel.DisableSmoothie();
	}
}
