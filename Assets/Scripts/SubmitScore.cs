using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class SubmitScore : MonoBehaviour {

    public void PopulateScore()
    {
        GetComponent<Text>().text = "Score: " + Settings.saveData.Score;
    }
}
