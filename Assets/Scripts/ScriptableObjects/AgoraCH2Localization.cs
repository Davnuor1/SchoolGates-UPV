using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AgoraCH2Localization", menuName = "Localization/AgoraCH2Localization")]
public class AgoraCH2Localization : ScriptableObject
{
    [System.Serializable]
    public class ChestEntry
    {
        public string chestID;
        public Sprite chestImage;
        public string textChest;
        public string textClueAgora;

        [Range(1, 5)]
        public int altarNumber;
    }

    public ChestEntry[] chests;

    [Header("UI Texts")]
    public string fusionErrorText;
    public string fusionSuccessText;
}
