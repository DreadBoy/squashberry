using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class SubmitScore : MonoBehaviour {

    public void PopulateScore()
    {
        Settings.saveData.Score = 333;
        GetComponent<Text>().text = "Score: " + Settings.saveData.Score;
    }
}
