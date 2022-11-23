using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class TimerText : MonoBehaviour
{
    private Text _text;

    void Start()
    {
        _text = GetComponent<Text>();

        GameContext.OnTimerTick += OnTimerTick;
    }

    private void OnTimerTick(float t)
    {
        _text.text = $"{t}";
    }

    private void OnDestroy()
    {
        GameContext.OnTimerTick -= OnTimerTick;
    }
}
