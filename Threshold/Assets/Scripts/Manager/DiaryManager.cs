using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class DiaryManager : MonoBehaviour
{
    public TMP_Text diaryDateText;
    public TMP_Text diaryContextText;
    public Button[] diaryButtons;
    public GameObject diaryObject;

    private DiaryList diaryData;

    void Start()
    {
        LoadDiaryData();
        SetupButtons();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
            diaryObject.SetActive(false);

    }

    void LoadDiaryData()
    {
        TextAsset jsonData = Resources.Load<TextAsset>("diaryData");
        diaryData = JsonUtility.FromJson<DiaryList>(jsonData.text);
    }

    void SetupButtons()
    {
        for (int i = 0; i < diaryButtons.Length; i++)
        {
            //클로저 문제 방지
            int index = i;
            diaryButtons[i].onClick.AddListener(() => OnDiaryButtonClick(index));
        }
    }

    void OnDiaryButtonClick(int index)
    {
        if (index < diaryData.diaryList.Length)
        {
            Diary entry = diaryData.diaryList[index];
            diaryDateText.text = entry.date;
            diaryContextText.text = entry.context;
        }
    }
}