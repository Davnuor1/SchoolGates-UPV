using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Rendering.Universal;

public class DayTimeController : MonoBehaviour
{
    const float secondInDay = 86400f;
    const float phaseLength = 900f; //15 min in seconds

    float time;
    [SerializeField] Color nightLightColor;
    [SerializeField] AnimationCurve nightTimeCurve;
    [SerializeField] Color dayLightColor=Color.white;
    [SerializeField] TextMeshProUGUI textHour;
    [SerializeField] TextMeshProUGUI textMinutes;
    [SerializeField] TextMeshProUGUI textNumDay;
    [SerializeField] Light2D globalLight;
    [SerializeField] Image dayOfWeekIm;

    [SerializeField] float timeScale = 60f;

    [SerializeField] float startAtTime = 28800f;
    List<TimeAgent> agents;
    public List<Sprite> daySprites;
    private int days=1;
    private int dayOfWeek;

    private void Awake()
    {
        agents = new List<TimeAgent>();
    }
    private void Start()
    {
        time = startAtTime;
    }
    public void Subscribe(TimeAgent timeAgent)
    {
        agents.Add(timeAgent);
    }
    public void Unsubscribe(TimeAgent timeAgent)
    {
        agents.Remove(timeAgent);
    }

    float Hours
    {
        get { return time / 3600f; }
    }
    float Minutes
    {
        get { return time % 3600f/60f; }
    }

    private void Update()
    {
        time += Time.deltaTime * timeScale;
        TimeValueCalculation();
        DayLight();
        if (time > secondInDay)
        {
            NextDay();
        }

        TimeAgents();
    }


    private void TimeValueCalculation()
    {
        int hh = (int)Hours;
        int mm = (int)Minutes;
        textHour.text = hh.ToString("00");
        textMinutes.text =  mm.ToString("00");
    }
    int oldPhase=0;
    private void TimeAgents()
    {
        int currentPhase =(int)(time / phaseLength);
        if (oldPhase != currentPhase)
        {
            oldPhase = currentPhase;
            for (int i = 0; i < agents.Count; i++)
            {
                agents[i].Invoke();
            }
        }
        
    }
    private void DayLight()
    {
        float v = nightTimeCurve.Evaluate(Hours);
        Color c = Color.Lerp(dayLightColor, nightLightColor, v);
        globalLight.color = c;
    }
    private void NextDay()
    {
        time = 0;
        days += 1;
        dayOfWeek = (days-1) % 7;
        textNumDay.text = days.ToString("00");
        dayOfWeekIm.sprite=daySprites[dayOfWeek];
    }
}
