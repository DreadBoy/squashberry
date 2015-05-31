using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class Intro : MonoBehaviour
{
    private static Intro instance = null;
    public static Intro Instance
    {
        get { return instance; }
    }
    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            instance = this;
        }
        DontDestroyOnLoad(this.gameObject);
    }
}
