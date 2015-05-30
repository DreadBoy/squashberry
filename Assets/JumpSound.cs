using UnityEngine;
using System.Collections;

public class JumpSound : MonoBehaviour
{
	public AudioSource JumpAudio;

	public void PlayJumpSound()
	{
		JumpAudio.Play();
	}
}
