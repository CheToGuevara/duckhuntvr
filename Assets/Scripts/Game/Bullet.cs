using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour
{
    Rigidbody rb;
    public float timeAlive = 2;
    public float speed = 10;
    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Fire(Vector3 forward)
    {
        rb.velocity = forward * speed;
        Invoke("Stop", timeAlive);
    }

    void OnCollisionEnter(Collision collision)
    {
        NotificationCenter.DefaultCenter().PostNotification(this, "TargetHit");
        Debug.Log("Target Hit");
        Stop();
    }

    void Stop()
    {
        rb.velocity = Vector3.zero;
        CancelInvoke("Stop");
        this.gameObject.SetActive(false);
    }
}
