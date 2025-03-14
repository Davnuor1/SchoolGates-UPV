using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GratitudeStone
{
    public string title;
    [TextArea(2, 4)] public List<string> descriptions;
    public Sprite icon;
}
