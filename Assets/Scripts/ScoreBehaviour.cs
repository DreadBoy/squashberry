using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScoreBehaviour : MonoBehaviour
{

    public Text Name;
    public Text Score;
    public void fillData(HighscoreList.Score score)
    {
        if (Name == null || Score == null)
            return;

        if (score == null)
        {
            gameObject.SetActive(false);
            return;
        }

        Name.text = score.name;
        Score.text = score.score.ToString();

    }
}
