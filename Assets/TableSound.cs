using UnityEngine;
using System.Collections;

public class TableSound : MonoBehaviour
{

	// Use this for initialization
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			Ray ray;
			RaycastHit hit;

			ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(ray, out hit))
			{
				if (hit.collider.gameObject.name == "Table")
				{
					Debug.Log("Table hit");

					GetComponent<AudioSource>().Play();
				}

			}
		}
	}
}
