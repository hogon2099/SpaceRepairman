using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Animation : MonoBehaviour
{
    public Animator anim;
    void Start()
    {
        anim.Play("Start_Bad_Robot");
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void pause()
    {
        anim.speed = 0;
    }
    private void resum()
    {
        anim.speed = 1;
    }
}
