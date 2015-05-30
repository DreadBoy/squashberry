using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class SetBool : MonoBehaviour {

    public Animator animator = null;
    public String boolean;
    public Boolean value;

    void Start()
    {
        GetComponent<Button>().onClick.AddListener(() => { setBool(); });
    }

    public void setBool()
    {
        if (animator == null)
            return;
        animator.SetBool(boolean, value); ;
    }

}