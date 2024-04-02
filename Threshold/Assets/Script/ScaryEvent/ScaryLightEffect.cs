using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ScaryLightEffect : ScaryEffect
{
    // 대상이 되는 라이트
    public Light targetLight;

    // 대상 설정 값
    public Color targetColor;
    public float targetIntensity;
    public float targetIndirectMultiplier;
    public LightShadows targetShadowType;
    public bool targetDrawHalo;

    public void ColorOff()
    {
        this.GetComponent<Light>().intensity = 10;
    }
}
