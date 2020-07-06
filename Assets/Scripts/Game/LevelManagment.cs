using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class LevelManagment : MonoBehaviour
{

    public Text TimeLeftText;
    public Text TargetLeftText;
    public Text CountDownText;
    public AudioClip pauseClip;

    AudioSource levelSound;

    bool started = false;
    float gameTime = 120;
    int remainTargets = 30;

    // Start is called before the first frame update
    void Start()
    {
        NotificationCenter.DefaultCenter().AddObserver(this, "StartCountDown");
        NotificationCenter.DefaultCenter().AddObserver(this, "TargetHit");
        

        levelSound = GetComponent<AudioSource>();

        remainTargets = (GameStatus.S.hardLevel) ? GameStatus.UISO.HardTarget : GameStatus.UISO.MedTarget;
        gameTime = (GameStatus.S.hardLevel) ? GameStatus.UISO.HardTime : GameStatus.UISO.MedTime;

        TimeLeftText.text = gameTime.ToString("##.##");
        TargetLeftText.text = remainTargets.ToString();
    }

    void StartCountDown()
    {
        StartCoroutine(CountDown());
    }

    IEnumerator CountDown()
    {
        float CountTime = 3;
        CountDownText.transform.localScale = Vector3.zero;
        CountDownText.text = "3";
        while (CountTime > 2)
        {
            CountDownText.transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, (0.5f - Mathf.Abs(CountTime - 2.5f))*2);
            CountTime -= Time.deltaTime;
            yield return null;
        }
        CountDownText.text = "2";
        while (CountTime > 1)
        {
            CountDownText.transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, (0.5f - Mathf.Abs(CountTime - 1.5f)) * 2);
            CountTime -= Time.deltaTime;
            yield return null;
        }
        CountDownText.text = "1";
        while (CountTime > 0)
        {
            CountDownText.transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, (0.5f - Mathf.Abs(CountTime - 0.5f)) * 2);
            CountTime -= Time.deltaTime;
            yield return null;
        }
        CountDownText.text = "Start";
        StartGame();
        
        while (CountTime > -1)
        {
            CountDownText.transform.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, (Mathf.Abs(CountTime)));
            CountTime -= Time.deltaTime;
            yield return null;
        }

    }


    void StartGame()
    { 
        NotificationCenter.DefaultCenter().PostNotification(this, "StartGame");
        started = true;
    }

    float holdDownPauseTime;
    // Update is called once per frame
    void Update()
    {
        if (started)
        {
            gameTime -= Time.deltaTime;
            TimeLeftText.text = gameTime.ToString("##.##");
            if (gameTime<0)
            {
                started = false;
                NotificationCenter.DefaultCenter().PostNotification(this, "GameOver");
            }

            if (Input.GetButtonDown("Fire1"))
            {
                holdDownPauseTime = Time.time;
            }
            if (Input.GetButtonUp("Fire1"))
            {
                float holdDownTime = Time.time - holdDownPauseTime;
                
                if (holdDownTime > 1.0f)
                {
                    Debug.Log("Pause");
                    NotificationCenter.DefaultCenter().PostNotification(this, "PauseGameToggle");
                    levelSound.clip = pauseClip;
                    levelSound.Play();
                }
                
            }
        }
    }

    void TargetHit()
    {
        remainTargets--;
        TargetLeftText.text = remainTargets.ToString();
        if (remainTargets==0)
        {
            started = false;
            LevelTime newTLevel = new LevelTime();
            newTLevel.newTime = gameTime;
            NotificationCenter.DefaultCenter().PostNotification(this, "LevelComplete", newTLevel);
        }
    }

    
}
