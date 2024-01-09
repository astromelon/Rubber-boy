using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEditor;

public class UIManager : MonoBehaviour
{
    public GameObject Swipe2;
    public Text playerSpeedText;
    public Text delayTimeText;
    public Text speedWeightText;
    public Text swipeMagnitudeText;


    void Start()
    {
        
    }

    
    void Update()
    {
        // UI Text에 표시합니다.("F2"는 소수점 둘째 자리까지 표시합니다.)
        playerSpeedText.text = "Player Speed: " + Swipe2.GetComponent<Swipe2>().playerSpeed.ToString("F2"); 
        delayTimeText.text = "Delay Time: " + Swipe2.GetComponent<Swipe2>().delayTime.ToString("F2");
        speedWeightText.text = "Speed Weight: " + Swipe2.GetComponent<Swipe2>().speedWeight.ToString("F2");
        swipeMagnitudeText.text = "Swipe Magnitude: " + Swipe2.GetComponent<Swipe2>().swipeMagnitude.ToString("F2");
        
    }
}
