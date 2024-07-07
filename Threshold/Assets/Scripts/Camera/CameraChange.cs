using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class CameraChange : MonoBehaviour
{
    private int currentCameraIndex = 0;

    public CinemachineVirtualCamera[] cameras;

    public Volume volume;

    private ColorAdjustments colorAdjustments;
    /* void Start()
     {
         if (volume != null && volume.profile != null)
         {
             // Color Adjustments를 Volume Profile에서 찾기
             if (volume.profile.TryGet<ColorAdjustments>(out colorAdjustments))
             {
                 // Contrast 값을 원하는 값으로 설정
                 SetContrast(50.0f); // 원하는 Contrast 값으로 설정
             }
             else
             {
                 Debug.LogWarning("Color Adjustments not found in the Volume Profile.");
             }
         }
         else
         {
             Debug.LogWarning("Volume or Volume Profile is not assigned.");
         }
     }*/

    public void SetContrast()
    {
        if (colorAdjustments != null)
        {
            if(colorAdjustments.contrast.value == 40.0f)
            colorAdjustments.contrast.value = 80.0f;
            else
            {
                colorAdjustments.contrast.value = 40.0f;
            }
        }
    }

    void Update()
    {

        //Q 키를 누르면 왼쪽 카메라 활성화
        if (Input.GetKeyDown(KeyCode.Q))
            SwitchCamera((currentCameraIndex - 1 + cameras.Length) % cameras.Length);

        //E 키를 누르면 오른쪽 카메라 활성화
        if (Input.GetKeyDown(KeyCode.E))
            if (PlayerInteraction.focusInteraction)
            {
                CloseEyesMode();
                Debug.Log("여기에 들어오면 눈감기");
            }
            else
            {
                SwitchCamera((currentCameraIndex + 1) % cameras.Length);
            }

    }

    void SwitchCamera(int newIndex)
    {
        cameras[currentCameraIndex].gameObject.SetActive(false);
        cameras[newIndex].gameObject.SetActive(true);

        currentCameraIndex = newIndex;
    }

    void CloseEyesMode()
    {
        if (volume != null && volume.profile != null)
        {
            // Color Adjustments를 Volume Profile에서 찾기
            if (volume.profile.TryGet<ColorAdjustments>(out colorAdjustments))
            {
                SetContrast();
            }

            Debug.Log("여기에 들어오면 눈감기");
        }

    }
}


//여기 코드에다가 focus mode가 되어 있을 때랑 아닐 때랑 구분해서 넣기
