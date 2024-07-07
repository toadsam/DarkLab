using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraChange : MonoBehaviour
{
    private int currentCameraIndex = 0;

    public CinemachineVirtualCamera[] cameras;

	void Update()
    {
        if (!PlayerInteraction.focusInteraction)
        {
            //Q 키를 누르면 왼쪽 카메라 활성화
            if (Input.GetKeyDown(KeyCode.Q))
                SwitchCamera((currentCameraIndex - 1 + cameras.Length) % cameras.Length);

            //E 키를 누르면 오른쪽 카메라 활성화
            if (Input.GetKeyDown(KeyCode.E))
                SwitchCamera((currentCameraIndex + 1) % cameras.Length);
        }
    }

    void SwitchCamera(int newIndex)
    {
        cameras[currentCameraIndex].gameObject.SetActive(false);
        cameras[newIndex].gameObject.SetActive(true);

        currentCameraIndex = newIndex;
    }
}//여기 코드에다가 focus mode가 되어 있을 때랑 아닐 때랑 구분해서 넣기
