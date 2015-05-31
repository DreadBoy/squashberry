using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
[RequireComponent(typeof(AudioSource))]

public class SubmitScore : MonoBehaviour {

	//public AudioSource audioSource;

    public void PopulateScore()
    {
		GetComponent<AudioSource>().Play();
        GetComponent<Text>().text = "Score: " + Settings.saveData.Score;
    }
}
