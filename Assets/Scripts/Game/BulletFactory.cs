﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class BulletFactory : MonoBehaviour
{
    public GameObject[] targetList;//Change it to fixed array


    int currentTarget = 0;
    bool fired = false;
    bool started = false;

    AudioSource gunSound;

    private void Start()
    {

        gunSound = GetComponent<AudioSource>();
        GameObject BulletParent = new GameObject("BulletParent");
        int numOfTargets = 10;
        targetList = new GameObject[numOfTargets];

        for (int i = 0; i < numOfTargets; i++)
        {
            GameObject newTarget = Instantiate(GameStatus.UISO.Bullet, BulletParent.transform);
            newTarget.SetActive(false);
            targetList[i] = newTarget;
        }

        NotificationCenter.DefaultCenter().AddObserver(this, "StartGame");
    }

    // Start is called before the first frame update
    void StartGame()
    {
        Debug.Log("BulletStart");
        started = true;
        
        

    }

    float holdDownPauseTime;
    // Update is called once per frame
    void Update()
    {
        if (started)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                holdDownPauseTime = Time.time;
            }
            if (Input.GetButtonUp("Fire1"))
            {
                float holdDownTime = Time.time - holdDownPauseTime;
                if (holdDownTime < 1.0f)
                    Fire();
            }
        }
    }


    public void Fire()
    {
        if (fired)
            return;
        GameObject target = targetList[currentTarget];
        target.SetActive(true);

        target.transform.position = this.transform.position;
        target.transform.rotation = Quaternion.identity;
        target.GetComponent<Bullet>().Fire(this.transform.forward);
        currentTarget = (currentTarget+1)%targetList.Length;
        fired = true;
        gunSound.Play();
        Invoke("Reload",0.5f);
    }

    void Reload()
    {
        fired = false;
    }
}
