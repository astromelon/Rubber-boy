using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GG.Infrastructure.Utils.Swipe;

public class Swipe2 : MonoBehaviour
{
    [SerializeField] private SwipeListener swipeListener;
    [SerializeField] private Transform palyerTransform;
    [SerializeField] public float playerSpeed;     // 이동속도
    private float timer;    // 움직이는 시간측정 타이머
    [SerializeField] public float delayTime;     // 움직이는 딜레이 시간
    [SerializeField] public float speedWeight;   // 속도 가중치
    [SerializeField] public float delayTimeWeight;     // 움직이는 딜레이 시간 가중치

    private Vector2 playerDirection = Vector2.zero;
    
    private Vector2 fingerDownPosition;
    private Vector2 fingerUpPosition;
    private float minSwipeDistance; // 최소 스와이프 거리
    public float swipeMagnitude;    // 스와이프 세기

    
    

    private void OnEnable () {
        swipeListener.OnSwipe.AddListener (OnSwipe);
        
    }

    private void Start()
    {
       // 변수 초기화
       speedWeight = 1000.0f;    // 속도 가중치
       timer = 0.0f;    // 움직이는 시간측정 타이머
       minSwipeDistance = 20f;  // 최소 스와이프 거리
       swipeMagnitude = 0.0f;   // 스와이프 세기
       delayTimeWeight = 0.00125f;  // 움직이는 딜레이 시간 가중치
    }
    
    private void OnSwipe (string swipe) {
        // Debug.Log (swipe);
        switch (swipe) {
            case "Left":
                playerDirection = Vector2.left;
                break;
            case "Right":
                playerDirection = Vector2.right;
                break;
            case "Up":
                playerDirection = Vector2.up;
                break;
            case "Down":
                playerDirection = Vector2.down;
                break;

            // 대각선
            case "UpLeft":
                playerDirection = new Vector2 (-1f, 1f);
                break;
            case "UpRight":
                playerDirection = new Vector2 (1f, 1f);
                break;
            case "DownLeft":
                playerDirection = new Vector2 (-1f, -1f);
                break;
            case "DownRight":
                playerDirection = new Vector2 (1f, -1f);
                break;

        }
    }
    
    private void Update()
    {
        DetectSwipe();

        
        
        // 스와이프 세기에 따른 움직인 거리 조절 = 정해진 시간안에 세기가 쌜 수록 스피드가 빠름
        // 움직임 시작
        if (swipeMagnitude > minSwipeDistance)
        {
            timer += Time.deltaTime; // 시간 측정
            // Debug.Log(timer + " 초");
            
            // 딜레이 시간 계산(거리가 짧을수록 시간은 줄어든다)
            delayTime = swipeMagnitude * delayTimeWeight;
            // 속도 계산(시간이 갈수록 속도는 줄어든다.)
            playerSpeed = swipeMagnitude / (timer * speedWeight);

            // 몇초 후 움직임 종료
            if (timer < delayTime)
            {
                palyerTransform.position += (Vector3)playerDirection * playerSpeed * Time.deltaTime;
            }
            else
            {
                // 멈추고 시간 초기화
                playerDirection = Vector2.zero;
                timer = 0f;
                playerSpeed = 0.0f;
                swipeMagnitude = 0.0f;
            }

        }

    }

    private void OnDisable () {
        swipeListener.OnSwipe.RemoveListener (OnSwipe);
    }

    void DetectSwipe()
    {
        foreach (Touch touch in Input.touches)
        {
            if (touch.phase == TouchPhase.Began)
            {
                fingerDownPosition = touch.position;
                fingerUpPosition = touch.position;
            }

            if (touch.phase == TouchPhase.Moved)
            {
                fingerUpPosition = touch.position;
            }

            if (touch.phase == TouchPhase.Ended)
            {
                float swipeDistance = Vector2.Distance(fingerDownPosition, fingerUpPosition);

                if (swipeDistance > minSwipeDistance)
                {
                    Vector2 swipeDirection = fingerUpPosition - fingerDownPosition;
                    swipeMagnitude = swipeDirection.magnitude;

                    // 스와이프 세기 출력
                    Debug.Log("스와이프 세기: " + swipeMagnitude);
                }
            }
        }
    }
}
