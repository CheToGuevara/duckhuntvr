
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class TargetFactory : MonoBehaviour
{
    public GameObject[] targetList;//Change it to fixed array
    AudioSource birdSound;

    int currentTarget = 0;
    int currentActiveTarget = 0;

    GameObject[] activeTarget;

    GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        birdSound = GetComponent<AudioSource>();
        int numOfTargets = (GameStatus.S.hardLevel) ? GameStatus.UISO.HardTarget : GameStatus.UISO.MedTarget;
        targetList = new GameObject[numOfTargets];
        activeTarget = new GameObject[3];

        for (int i = 0; i < numOfTargets; i++)
        {
            GameObject newTarget = Instantiate(GameStatus.UISO.Target, this.transform);
            newTarget.SetActive(false);
            targetList[i] = newTarget;
        }
        activeTarget[0] = targetList[0];
        activeTarget[1] = targetList[1];
        activeTarget[2] = targetList[2];

        NotificationCenter.DefaultCenter().AddObserver(this, "TargetHitted");
        NotificationCenter.DefaultCenter().AddObserver(this, "StartGame");


        //Invoke("StartGame", 0.5f);

    }

    // Update is called once per frame
    void Update()
    {

    }

    void StartGame()
    {

        Debug.Log("TargetStart");
        Invoke("SpamTarget", 0.5f);
    }


    void TargetHitted()
    {
        if (currentTarget == ((GameStatus.S.hardLevel) ? 30 : 20))
        {
            if (checkAllTargetFalse())
            {
                NotificationCenter.DefaultCenter().RemoveObserver(this, "TargetHitted");
                NotificationCenter.DefaultCenter().RemoveObserver(this, "StartGame");
            }
            
        }
        else
        {

            Invoke("SpamTarget", 2.0f);
        }
    }

    bool checkAllTargetFalse()
    {
        foreach(GameObject target in activeTarget)
        {
            if (target.activeSelf)
                return false;
        }

        return true;
    }

    int checkMaxTarget()
    {
        for(int i = 0; i < 3; i++) 
        {
            if (!activeTarget[i].activeSelf)
                return i;
        }
        return -1;
    }

    void SpamTarget()
    {
        int index = checkMaxTarget();
        if (index < 0 || currentTarget >= targetList.Length)
        {
            
            return;
        }

        GameObject target = targetList[currentTarget];
        target.SetActive(true);
        activeTarget[index] = target;
        Vector3 vectorUnitario = Random.onUnitSphere;
        vectorUnitario.y = Mathf.Abs(vectorUnitario.y);
        target.transform.position = vectorUnitario.normalized * Random.Range(1,3) + player.transform.position;
        target.transform.rotation = Quaternion.LookRotation(-target.transform.position);
        currentTarget++;
        birdSound.Play();
        Invoke("SpamTarget", 2.0f);
    }


}
