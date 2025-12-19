using UnityEngine;
using TMPro;

public class GameTimer : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    public float startTime = 60f;

    private float currentTime;
    private bool timerStarted;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentTime = startTime;
        UpdateTimerUI();
    }

    // Update is called once per frame
    void Update()
    {
        if(!timerStarted && IsPlayerInput())
        {
            timerStarted = true;
        }

        if (timerStarted)
        {
            currentTime -= Time.deltaTime;
            currentTime = Mathf.Clamp(currentTime,0f,startTime);

            UpdateTimerUI();

            if(currentTime <= 0)
            {
                TimerEnded();
            }
        }
    }

    public void UpdateTimerUI()
    {
        timerText.text = "Timer: " + Mathf.CeilToInt(currentTime);
    }

    bool IsPlayerInput()
    {
        return Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0 || Input.GetButtonDown("Jump");
    }

    public void TimerEnded()
    {
        timerStarted = false;
        Debug.Log("Time Up!");
        GameManager.instance.GameOver();
    }
}
