using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health_Bar : MonoBehaviour
{
    public static Health_Bar instance { get; private set; }

    public Image mask;
    float originalSize;
   
    void Awake(){
        instance = this;
    }

    void Start(){
        originalSize = mask.rectTransform.rect.width;
    }

    // Update is called once per frame
    public void SetValue(float value){
        mask.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, originalSize * value);   
    }
}
