using UnityEngine;
using System.Collections;
using System;
using System.Net;
using System.IO;

public class HighscoreSubmit : MonoBehaviour
{
    public void Submit()
    {
        string url = "http://squasberry.evennode.com/" + Settings.saveData.PlayerName + "?score=" + Settings.saveData.Score;

        WWWForm form = new WWWForm();
        form.AddField("var1", "value1");
        WWW www = new WWW(url, form);

        StartCoroutine(WaitForRequest(www));
    }


    IEnumerator WaitForRequest(WWW www)
    {
        yield return www;

        // check for errors
        if (www.error == null)
        {
            Debug.Log("WWW Ok!: " + www.data);
        }
        else
        {
            Debug.Log("WWW Error: " + www.error);
        }
    }
}
