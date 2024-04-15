using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class Timer : MonoBehaviour
{
    public float duration = 5f;

    private float timeRemaining;
    private bool isTimerActive = false;

    public TextMeshProUGUI timerText1;
    public TextMeshProUGUI timerText2;
    public TextMeshProUGUI timerText3;

    public UnityEvent onTenSecondsLeft;
    public UnityEvent onTimeUp;

    private bool tenSecondsEventTriggered = false;

    void Start()
    {
        if (onTenSecondsLeft == null)
            onTenSecondsLeft = new UnityEvent();

        if (onTimeUp == null)
            onTimeUp = new UnityEvent();

        // MulaiTimer();
    }

    void Update()
    {
        if (isTimerActive)
        {
            if (timeRemaining > 0f)
                timeRemaining -= Time.deltaTime;

            if (timeRemaining <= 10 && !tenSecondsEventTriggered)
            {
                onTenSecondsLeft.Invoke();
                tenSecondsEventTriggered = true;
            }

            if (timerText1 != null && timerText2 != null)
            {
                if (timeRemaining > 0f)
                {
                    int menit = Mathf.FloorToInt(timeRemaining / 60);
                    int detik = Mathf.FloorToInt(timeRemaining % 60);
                    timerText1.text = string.Format("{0:00}:{1:00}", menit, detik);
                    timerText2.text = string.Format("{0:00}:{1:00}", menit, detik);
                    timerText3.text = string.Format("{0:00}:{1:00}", menit, detik);
                }
            }

            if (timeRemaining <= 0)
            {
                if (isTimerActive)
                {
                    isTimerActive = false;
                    timeRemaining = 0;
                    onTimeUp.Invoke();
                }
            }
        }
    }

    public void StartTimer()
    {
        if (duration > 0)
        {
            timeRemaining = duration;
            isTimerActive = true;
            tenSecondsEventTriggered = false;
        }
        else
        {
            Debug.LogError("Durasi timer harus lebih besar dari 0");
        }
    }
}
