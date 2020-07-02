using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class ScoreText : MonoBehaviour
{
    public bool Hard;
    Text _text;
    bool init = false;
    // Start is called before the first frame update
    void Start()
    {
        _text = GetComponent<Text>();
        init = true;
        updateValue();
    }

    void updateValue()
    {
        if (Hard)
            _text.text = GameStatus.S.playerProgress.secondsHard.ToString("##.##") + "s";
        else
            _text.text = GameStatus.S.playerProgress.secondsMed.ToString("##.##") + "s";
    }

    private void OnEnable()
    {
        if (init)
        {
            updateValue();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
