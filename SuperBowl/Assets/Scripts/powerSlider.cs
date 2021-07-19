using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class powerSlider : MonoBehaviour
{
    public testController playerBall;
    public Slider powerSliderUI;
    
    // Update is called once per frame
    void Update()
    {
        playerBall.speed = powerSliderUI.value;
    }
}
