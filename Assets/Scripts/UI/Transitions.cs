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

    public Image imageFade;

    public float timeTransitions = 1.0f;
    public float progress = 0;

    Color colorFadeIn;
    Color colorFadeOut;
    float step = 0;

    bool showing = false;
    // Start is called before the first frame update
    void Start()
    {
        colorFadeIn = Color.white;
        colorFadeOut = Color.white;
        colorFadeOut.a = 0;

        imageFade.color = colorFadeOut;
        HideWidgets();
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

    void HideWidgets()
    {
        
    }


    public bool FadeIn()
    {

        if (!showing)
        {
            showing = true;
            NotificationCenter.DefaultCenter().PostNotification(this, "DisableButtonBehaviour");
            progress = 0;
        }
        step +=  Time.deltaTime;

        imageFade.color = Color.Lerp(colorFadeOut, colorFadeIn, step/timeTransitions);
        if (step > timeTransitions)
        {
            ShowWidgets();
            step = timeTransitions;
            return true;
        }
        else
            return false;
    }
    public bool FadeOut()
    {
        step -= Time.deltaTime;
        HideWidgets();

        imageFade.color = Color.Lerp(colorFadeOut, colorFadeIn, step / timeTransitions);
        if (step < 0)
        {
            
            showing = false;
            NotificationCenter.DefaultCenter().PostNotification(this, "EnableButtonBehaviour");
            step = 0;
            return true;
        }
        else
            return false;
    }

}
