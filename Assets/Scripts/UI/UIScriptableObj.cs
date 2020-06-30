using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/UISO", fileName = "UISO.asset")]
[System.Serializable]
public class UIScriptableObj : ScriptableObject
{

    static public UIScriptableObj instance; // This Scriptable Object is an unprotected Singleton

    public UIScriptableObj()
    {
        instance = this; // Assign the Singleton as part of the constructor.
    }

    public Sprite MainBGImg;


    public GameObject Target;



    
}
