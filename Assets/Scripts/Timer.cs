using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public Text timerText;
    private float timeElapsed;

    void Update()
    {
        timeElapsed += Time.deltaTime;
        timerText.text = timeElapsed.ToString("F3") + " s";
    }
}