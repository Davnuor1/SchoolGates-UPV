using System;

[Serializable]
public class UserData
{
    public string tan;
    public float totalPlayTime;
    public int currentWorldIndex;
    public int[] challengesCompleted;
    public int totalChallengesCompleted;
    public int timesGameOpened;

    public string dialogueSystemSaveData; // NUEVO
}
