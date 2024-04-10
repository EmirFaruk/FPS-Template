using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class LevelCountdownController : MonoBehaviour
{
    #region VARIABLES
    public static Action OnLevelExpiration = () => { };
    public static Action<bool> OnLevelTimeReloadStart = _ => { };
    public static Action OnLevelTimeReloadEnd = () => { };
    public static Action<int> OnLevelTimeRemaining = _ => { };
    public static Action OnCriticalTime = () => { };
    public static Action<float> OnResetCountdown = _ => { };
    public static Action OnGainTime = () => { };

    [SerializeField] private int timeRemaining = 50;
    private int defaultRemainingTime = 0;
    private const float criticalTimePercentage = .2f;

    private Color defaultColor;

    private Queue<float> CountdownTimes = new Queue<float>();

    private bool countdownInUpdating = false;
    private bool canCountdown => timeRemaining >= 0 && !countdownInUpdating && !destroyCancellationToken.IsCancellationRequested;
    #endregion

    #region UNITY EVENT FUNCTIONS
    private void OnEnable()
    {
        OnResetCountdown += ControlCountdownTasks;
    }

    private void OnDisable()
    {
        OnResetCountdown -= ControlCountdownTasks;
    }

    void Start()
    {
        InitializeCountdown();
        CountdownAsync();
    }
    #endregion

    #region METHODS

    #region Initialize
    private void InitializeCountdown()
    {
        defaultRemainingTime = timeRemaining;

        UpdateCountdownDisplay();
    }
    #endregion

    #region Countdown Base
    private void StartCountdown(int targetTime)
    {
        if (timeRemaining == targetTime) CountdownAsync();
    }

    async void CountdownAsync()
    {
        while (canCountdown)
        {
            UpdateCountdownDisplay();

            if (IsTimeCritical()) OnCriticalTime?.Invoke();

#if UNITY_EDITOR
            await Task.Delay(Input.GetKey(KeyCode.T) ? 100 : 1000);
#else
            await Task.Delay(1000);
#endif
            timeRemaining--;
        }
    }

    private void UpdateCountdownDisplay()
    {
        OnLevelTimeRemaining.Invoke(timeRemaining);
    }

    private bool IsTimeCritical()
    {
        return timeRemaining <= Math.Max(10, defaultRemainingTime * criticalTimePercentage);
    }
    #endregion

    #region Time Joint Tasks
    private void ControlCountdownTasks(float time)
    {
        CountdownTimes.Enqueue(time);
        if (CountdownTimes.Count == 1 && !countdownInUpdating)
        {
            UpdateCountdown(CountdownTimes.First());
        }
    }

    private void ControlTasksEnding(int targetTime)
    {
        CountdownTimes.Dequeue();

        if (CountdownTimes.Count > 0)
            UpdateCountdown(CountdownTimes.First());
        else
        {
            OnLevelTimeReloadEnd.Invoke();
            countdownInUpdating = false;
            StartCountdown(targetTime);
        }
    }

    private async void UpdateCountdown(float time)
    {
        countdownInUpdating = true;

        int speed = 200;

        int targetTime = time != Mathf.Infinity ? ((int)(time + timeRemaining)) : defaultRemainingTime;
        int sign = targetTime - timeRemaining > 0 ? 1 : -1;

        OnLevelTimeReloadStart.Invoke(targetTime - timeRemaining >= 0);

        while (timeRemaining != targetTime && !destroyCancellationToken.IsCancellationRequested)
        {
            timeRemaining += sign;
            print(timeRemaining + " / " + targetTime);
            UpdateCountdownDisplay();
            await Task.Delay(Math.Max(1, speed -= 5));
        }

        timeRemaining = targetTime;
        await Task.Delay(500);

        ControlTasksEnding(targetTime);
    }
    #endregion

    #region Time Ended Methods
    private void TriggerExpiration()
    {
        Invoke(nameof(InvokeLevelExpiration), 1);
        Invoke(nameof(Quit), 1);
    }

    private void InvokeLevelExpiration()
    {
        OnLevelExpiration.Invoke();
    }

    public async void Quit()
    {
        PrepareForQuit();

        for (int i = 10; i >= 0; i--, await Task.Delay(1000))
            //countdownText.text = "Quit in\n" + i;

            Application.Quit();
    }

    private void PrepareForQuit()
    {
        //Invoke
        //countdownText.fontSize = 42;
        //countdownText.color = Color.red;
    }
    #endregion

    #endregion
}