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

						//BlenderBehaviour.instance.currentState = BlenderState.Blend;
					}
				}
			}
		}
	}
}
