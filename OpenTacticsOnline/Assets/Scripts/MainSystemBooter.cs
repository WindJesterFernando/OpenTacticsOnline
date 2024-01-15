using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainSystemBooter : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StateManager.Init();
        StateManager.PushGameState(new TitleState());
    }


    // Update is called once per frame
    void Update()
    {
        StateManager.Update();
    }
}
