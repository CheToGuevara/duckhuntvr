using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision collision)
    {
        
        gameObject.SetActive(false);
        NotificationCenter.DefaultCenter().PostNotification(this, "TargetHitted");
        Debug.Log("Hitted");
    }
}
