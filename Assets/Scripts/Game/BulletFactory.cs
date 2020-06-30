using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletFactory : MonoBehaviour
{
    public GameObject Target;
    public GameObject[] targetList;//Change it to fixed array


    int currentTarget = 0;
    bool fired = false;

    // Start is called before the first frame update
    void Start()
    {
        GameObject BulletParent = new GameObject("BulletParent");
        int numOfTargets = (GameStatus.S.hardLevel) ? 30 : 20;
        targetList = new GameObject[numOfTargets];

        for (int i = 0; i < numOfTargets; i++)
        {
            GameObject newTarget = Instantiate(Target, BulletParent.transform);
            newTarget.SetActive(false);
            targetList[i] = newTarget;
        }
        

    }

    float holdDownPauseTime;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonUp("Fire1"))
        {
            holdDownPauseTime = Time.time;
        }
        if (Input.GetButtonUp("Fire1"))
        {
            float holdDownTime = Time.time - holdDownPauseTime;
            if (holdDownTime > 1.0f)
            {
                Debug.Log("Pause");
                NotificationCenter.DefaultCenter().PostNotification(this, "Pause");
            }else
                Fire();
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
        Invoke("Reload",0.5f);
    }

    void Reload()
    {
        fired = false;
    }
}
