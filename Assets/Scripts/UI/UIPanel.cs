using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPanel : MonoBehaviour
{
    public Button hardBT;
    public Button medBT;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void change2Hard()
    {
        medBT.interactable = true;
        hardBT.interactable = false;
    }

    public void change2Medium()
    {
        medBT.interactable = false;
        hardBT.interactable = true;
    }

    
}
