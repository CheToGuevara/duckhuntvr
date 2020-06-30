
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

public class TargetFactory : MonoBehaviour
{
    public GameObject Target;
    public GameObject[] targetList;//Change it to fixed array


    int currentTarget = 0;

    // Start is called before the first frame update
    void Start()
    {
        int numOfTargets = (GameStatus.S.hardLevel) ? 30 : 20;
        targetList = new GameObject[numOfTargets];

        for (int i= 0; i< numOfTargets; i++)
        {
            GameObject newTarget = Instantiate(Target, this.transform);
            newTarget.SetActive(false);
            targetList[i] =newTarget;
        }
        Invoke("StartGame",0.5f);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void StartGame()
    {
        InvokeRepeating("SpamTarget", 0.5f, 4.0f);
    }

    void SpamTarget()
    {
        GameObject target = targetList[currentTarget];
        target.SetActive(true);
        
        target.transform.position = (Random.onUnitSphere * 2) + Vector3.one;
        target.transform.rotation = Quaternion.LookRotation(-target.transform.position);
        currentTarget++;
    }


}
