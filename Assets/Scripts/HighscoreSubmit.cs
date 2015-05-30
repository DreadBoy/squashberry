using UnityEngine;
using System.Collections;
using System;
using System.Net;
using System.IO;

public class HighscoreSubmit : MonoBehaviour {
    public void Submit()
    {

        WebRequest request = WebRequest.Create("http://squasberry.evennode.com/" + Settings.saveData.playerName + "?score=" + 0);
        request.Method = "POST";
        request.ContentType = "application/json; charset=utf-8";
        using (var reponse = (HttpWebResponse)request.GetResponse())
        {

            using (var reader = new StreamReader(reponse.GetResponseStream()))
            {
                var objText = reader.ReadToEnd();
                Debug.Log(objText);
            }

        }
    }
}
