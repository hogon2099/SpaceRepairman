using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Day_scene : MonoBehaviour
{
    // Start is called before the first frame update
    public int _day_Info = 0;
    public  Image Black_Scene;
    public Manager_Scene manager;
    public GameObject Repair_Station;

    void Start()
    {
        GameObject.Find("Day_Info").GetComponent<Text>().text = "Day Info:"; 
        switch (manager.GetCurrentScene())
        {
            case 2:
                GameObject.Find("Day_Info").GetComponent<Text>().text += "Day   first";
                break;
            case 3:
                GameObject.Find("Day_Info").GetComponent<Text>().text += "Day   second";
                break;
            case 4:
                GameObject.Find("Day_Info").GetComponent<Text>().text += "Day    third";
                break;
            case 5:
                GameObject.Find("Day_Info").GetComponent<Text>().text += "Day    fourth";
                break;
            case 6:
                GameObject.Find("Day_Info").GetComponent<Text>().text += "Day     fifth";
                break;
            default:
                GameObject.Find("Day_Info").GetComponent<Text>().text += "No information";
                GameObject.Find("Day_Info").GetComponent<Text>().color = Color.red;
                break;

        }

    }

}
