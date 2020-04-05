using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class RepairStation : MonoBehaviour
{
    public int Repair_Robot_Success = 0;
   public Manager_Scene manager;
    public GameObject day_Inf;
    public GameObject Black_BG;
    public GameObject Black_Text;
    public GameObject Bad_Scene;
    public GameObject Detail;
    public Animator end_Scene;
    public Animator Bad_Robot;
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Repair_Robot_Success == 1)
        {
            StartCoroutine(NextOk());
        }
        else if (Repair_Robot_Success == 2)
        {
            StartCoroutine(NextFail());
        }
    }
    IEnumerator NextOk()
    {
        day_Inf.SetActive(false);
        Black_BG.SetActive(true);

        end_Scene.Play("Start_Next_Scene");
        yield return new WaitForSeconds(2);
        manager.nextScene();
    }
    IEnumerator NextFail ()
    {
        day_Inf.SetActive(false);
        Bad_Scene.SetActive(true);

        end_Scene.Play("Start_Next_Scene");
        yield return new WaitForSeconds(2);
        manager.nextScene();
    }
}
