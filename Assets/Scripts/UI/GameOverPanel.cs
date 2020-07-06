using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class GameOverPanel : MonoBehaviour
{
    public Text msgText;

    string text2show;
    // Start is called before the first frame update
    void Awake()
    {
        
        NotificationCenter.DefaultCenter().AddObserver(this, "ResultMsg");
        text2show = "Game Over";
        Debug.Log("Panel");
    }

    

    // Update is called once per frame
    void Update()
    {
        if(msgText)
            msgText.text = text2show;
    }

    void ResultMsg(Notification textMsg)
    {
        Debug.Log("Hola");
        
        text2show = textMsg.data as string;
        Debug.Log(text2show);
    }
}
