using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Target : MonoBehaviour
{
    AudioSource hitSound;
    // Start is called before the first frame update
    void Start()
    {
        hitSound = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnCollisionEnter(Collision collision)
    {

        gameObject.SetActive(false);
        NotificationCenter.DefaultCenter().PostNotification(this, "TargetHitted");
        hitSound.Play();
        //Debug.Log("Hitted");
    }
}
