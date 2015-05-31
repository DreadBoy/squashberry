using UnityEngine;
using System.Collections;

public class Tab : MonoBehaviour
{
	public AudioSource Table;
	public BlenderButtonPress Button;

	void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			Debug.Log("Mouse Down");
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

			if (Physics.Raycast(ray, out hit))
			{
				if (hit.collider != null)
				{
					Debug.Log(hit.collider.gameObject.name);

					switch (hit.collider.gameObject.name)
					{
						case "Bone":
							GameObject berry = hit.collider.gameObject.transform.parent.gameObject.transform.parent.gameObject;

							if (berry.GetComponent<BerryBehaviour>().currentState == BerryBehaviour.BerryState.Move)
							{
								berry.GetComponent<BerryBehaviour>().currentState = BerryBehaviour.BerryState.Squash;
								Debug.Log("Squash");
							}
							else
							{
								//Table.Play();
							}
							break;
						case "Button":
							Button.ButtonPress();
							Debug.Log("ButtonPress");
							break;
						default:
							Table.Play();
							Debug.Log("TableHit");
							break;
					}
				}
			}
		}
	}
}
