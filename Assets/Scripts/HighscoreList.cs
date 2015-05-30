using UnityEngine;
using System.Collections;
using System.Net;
using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;

public class HighscoreList : MonoBehaviour
{

    public GameObject scorePrefab;

    public class Score
    {
        public String name { get; set; }
        public Int32 score { get; set; }
    }

    void Start()
    {
        List();
    }

    void List()
    {
        List<Score> scores = new List<Score>();

        WebRequest request = WebRequest.Create("http://squasberry.evennode.com/");
        request.Method = "GET";
        request.ContentType = "application/json; charset=utf-8";
        using (var reponse = (HttpWebResponse)request.GetResponse())
        {
            using (var reader = new StreamReader(reponse.GetResponseStream()))
            {
                String resText = reader.ReadToEnd();
                Debug.Log(resText);
                JSONObject res = new JSONObject(resText);
                foreach (var s in res.list)
                    scores.Add(new Score() { name = s["name"].str, score = (int)s["score"].n });

            }

        }

        scores = scores.Take(5).ToList();

        Int32 i = 0;
        foreach (Transform child in transform)
        {
            if (child.name == "Score")
            {
                if (i < scores.Count)
                    child.GetComponent<ScoreBehaviour>().fillData(scores[i]);
                else
                    child.GetComponent<ScoreBehaviour>().fillData(null);
            }
            i++;
        }
    }
}
