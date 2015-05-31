using UnityEngine;
using System.Collections;

public class JumpSound : MonoBehaviour
{
	public AudioSource JumpAudio;

	public void PlayJumpSound()
	{
		int rand = Random.Range(0, 10);
		if (rand < 1)
			JumpAudio.Play();
	}
}
