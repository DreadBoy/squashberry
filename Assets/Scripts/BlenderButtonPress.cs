using UnityEngine;
using System.Collections;

public class BlenderButtonPress : MonoBehaviour
{
	public GameObject Blender;

	void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

			if (Physics.Raycast(ray, out hit))
			{
				if (hit.collider != null)
				{
					if (hit.collider.gameObject.name == "Button")
					{
						Blender.GetComponent<Animator>().SetTrigger("ButtonPress");
						GetComponent<AudioSource>().Play();

						//BerryBehaviour.BerryState b = BerryBehaviour.BerryState.Die;
						BlenderBehaviour.instance.currentState = BlenderState.Blend;

						StartCoroutine(mixBerries());

					}
				}
			}
		}
	}

	IEnumerator mixBerries()
	{
		yield return new WaitForSeconds(1.5f);

		//foreach (GameObject g in GameObject.FindGameObjectsWithTag("Berry"))
		//{
		//	if (g.GetComponent<BerryBehaviour>().currentState == BerryBehaviour.BerryState.InBlender)
		//	{
		//		Destroy(g);
		//	}
		//}

		foreach (BerryBehaviour n in BlenderBehaviour.berriesInBlender)
		{
			Debug.Log("Meh");
		//	Destroy(n.gameObject);
		}
	}
}
