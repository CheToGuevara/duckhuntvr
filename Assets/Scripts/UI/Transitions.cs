using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class Transitions : MonoBehaviour
{
    private static Transitions instance = null;

    // Game Instance Singleton
    public static Transitions S
    {
        get
        {
            return instance;
        }
    }

    private void Awake()
    {
        instance = this;

    }

    

    public float timeTransitions = 1.0f;
    public float progress = 0;

    Vector3 scaledestiny;
    Vector3 scaleorigin;
    BoxCollider m_collider;

    
    float step = 0;

    bool showing = false;
    // Start is called before the first frame update
    void Start()
    {
        step = timeTransitions;
        scaledestiny = Vector3.zero;
        scaleorigin = new Vector3(0.0064f, 1, 0.004f);
        m_collider = GetComponent<BoxCollider>();
        

      
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    void ShowWidgets()
    {
        //loadingBar.gameObject.SetActive(true);
        //loadingText.gameObject.SetActive(true);
    }

  


    public bool FadeIn()
    {
        m_collider.enabled = true;
        if (!showing)
        {
            showing = true;
            //NotificationCenter.DefaultCenter().PostNotification(this, "DisableButtonBehaviour");
            progress = 0;
        }
        step +=  Time.deltaTime;
       
        transform.localScale = Vector3.Lerp(scaledestiny, scaleorigin,  step/timeTransitions);
        if (step > timeTransitions)
        {
            ShowWidgets();
            step = timeTransitions;
            m_collider.enabled = true;
            return true;
        }
        else
            return false;
    }
    public bool FadeOut()
    {
        step -= Time.deltaTime;

        transform.localScale = Vector3.Lerp(scaledestiny, scaleorigin, step / timeTransitions);
        if (step < 0)
        {
            
            showing = false;
            //NotificationCenter.DefaultCenter().PostNotification(this, "EnableButtonBehaviour");
            step = 0;
            m_collider.enabled = false;
            return true;
        }
        else
            return false;
    }

}
