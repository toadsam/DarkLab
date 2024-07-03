[System.Serializable]
public class Diary
{
    public int id;
    public string date;
    public string context;
}

[System.Serializable]
public class DiaryList
{
    public Diary[] diaryList;
}