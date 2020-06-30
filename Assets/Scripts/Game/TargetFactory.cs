
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

public class TargetFactory : MonoBehaviour
{
    public GameObject Target;
    public GameObject[] targetList;//Change it to fixed array


    int currentTarget = 0;
    int currentActiveTarget = 0;

    GameObject[] activeTarget;

    // Start is called before the first frame update
    void Start()
    {
        int numOfTargets = (GameStatus.S.hardLevel) ? 30 : 20;
        targetList = new GameObject[numOfTargets];
        activeTarget = new GameObject[3];

        for (int i = 0; i < numOfTargets; i++)
        {
            GameObject newTarget = Instantiate(Target, this.transform);
            newTarget.SetActive(false);
            targetList[i] = newTarget;
        }
        activeTarget[0] = targetList[0];
        activeTarget[1] = targetList[1];
        activeTarget[2] = targetList[2];

        NotificationCenter.DefaultCenter().AddObserver(this, "TargetHitted");

        Invoke("StartGame", 0.5f);

    }

    // Update is called once per frame
    void Update()
    {

    }

    void StartGame()
    {
        Invoke("SpamTarget", 0.5f);
    }


    void TargetHitted()
    {
        if (currentTarget == ((GameStatus.S.hardLevel) ? 30 : 20))
        {
            if (checkAllTargetFalse())
            {
                NotificationCenter.DefaultCenter().RemoveObserver(this, "TargetHitted");
                NotificationCenter.DefaultCenter().PostNotification(this, "LevelComplete");
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
        if (index < 0)
        {
            
            return;
        }

        GameObject target = targetList[currentTarget];
        target.SetActive(true);
        activeTarget[index] = target;

        target.transform.position = (Random.onUnitSphere * 2) + Vector3.one;
        target.transform.rotation = Quaternion.LookRotation(-target.transform.position);
        currentTarget++;
        Invoke("SpamTarget", 2.0f);
    }


}
