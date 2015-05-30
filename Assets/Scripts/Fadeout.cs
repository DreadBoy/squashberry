using UnityEngine;
using System.Collections;

public class Fadeout : MonoBehaviour {

	private float bornTime;
	private float opacity = 1;

	// Use this for initialization
	void Awake () {
		bornTime = Time.time;
	}
	
	void Update () {
		if( Time.time - bornTime > 2 )
		{
			// Change opacity value
			opacity -= 0.01f;

			// Set color
			Color color = GetComponent<Renderer>().material.color;
			GetComponent<Renderer>().material.color = color * new Vector4(1,1,1,opacity);

			// Destroy
			if( opacity <= 0.5f ){
				Destroy( this );
			}
		}
	}
}
