using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Manager_Scene : MonoBehaviour
{
    // Start is called before the first frame update
    
    public Day_scene day;
    public int CurrentScene = 0;
    public float time;
    public bool ispuse;
    public bool guipuse;
    void Start()
    {
        if(day)
        CurrentScene = day._day_Info;
    }

    // Update is called once per frame
    void Update()
    {
        if(CurrentScene != 0)
        {
            if(Input.GetKeyDown(KeyCode.Escape) && ispuse == false)
            {
                ispuse = true;
            }
            else if (Input.GetKeyDown(KeyCode.Escape) && ispuse == true)
            {
                ispuse = false;
            }
            if (ispuse == true)
            {
                time = 0;
                guipuse = true;

            }
            else if (ispuse == false)
            {
                time = 1f;
                guipuse = false;

            }
        }
    }
    
   public void nextScene()
    {
        GameObject obj = GameObject.FindGameObjectWithTag("Music");
        DontDestroyOnLoad(obj);

        CurrentScene++;
        if (CurrentScene != 4)
        {
            SceneManager.LoadSceneAsync(CurrentScene);
        }
        
    }
    [SerializeField]
    public int GetCurrentScene()
    {
        return CurrentScene;
    }
    void rendiringPause()
    {

    }
}
