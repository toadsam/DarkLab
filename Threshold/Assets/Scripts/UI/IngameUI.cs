using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IngameUI : MonoBehaviour
{
    public Button diaryBtn;
    public RectTransform collectiblesBtn;
    public GameObject scrollView;

    private bool isScrollViewActive = false;

    void Start()
    {
        diaryBtn.onClick.AddListener(OnDiaryButtonClick);
    }

    void OnDiaryButtonClick()
    {
        if (!isScrollViewActive)
        {
            scrollView.SetActive(true);

            collectiblesBtn.anchoredPosition -= new Vector2(0, scrollView.GetComponent<RectTransform>().rect.height + 180);

            isScrollViewActive = true;
        }
        else
        {
            scrollView.SetActive(false);

            collectiblesBtn.anchoredPosition += new Vector2(0, scrollView.GetComponent<RectTransform>().rect.height + 180);

            isScrollViewActive = false;
        }
    }
}
