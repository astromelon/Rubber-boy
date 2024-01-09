using System;
using System.Collections;
using System.Collections.Generic;
// using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.UI;
using UnityImage = UnityEngine.UI.Image;
// using MicrosoftImage = Microsoft.Unity.VisualStudio.Editor.Image;


public class Swipe : MonoBehaviour
{
    public GameObject Capsule;
    
    private Vector2 startTouchPosition;     // 터치 시작점
    private Vector2 endTouchPosition;       // 터치 끝지점

    private void Update()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            startTouchPosition = Input.GetTouch(0).position;
        }

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
        {
            endTouchPosition = Input.GetTouch(0).position;

            Vector2 inputVector = endTouchPosition - startTouchPosition;
            if(Mathf.Abs(inputVector.x) > Mathf.Abs(inputVector.y))
            {
                if(inputVector.x > 0)
                {
                    RightSwipe();
                }
                else
                {
                    LeftSwipe();
                }
            }
            else
            {
                if (inputVector.y > 0)
                {
                    UpSwipe();
                }
                else
                {
                    if(inputVector.y < 0){
                        DownSwipe();
                    }
                }
            }

        }
    }

    private void UpSwipe()
    {
        print("up");
        Capsule.transform.position = new Vector3(Capsule.transform.position.x, Capsule.transform.position.y + 1, Capsule.transform.position.z);
    }
    private void DownSwipe()
    {
        print("down");
        Capsule.transform.position = new Vector3(Capsule.transform.position.x, Capsule.transform.position.y - 1, Capsule.transform.position.z);
    }
    private void LeftSwipe()
    {
        print("left");
        Capsule.transform.position = new Vector3(Capsule.transform.position.x - 1, Capsule.transform.position.y, Capsule.transform.position.z);
    }
    private void RightSwipe()
    {
        print("right");
        Capsule.transform.position = new Vector3(Capsule.transform.position.x + 1, Capsule.transform.position.y, Capsule.transform.position.z);
    }
    
    
}
