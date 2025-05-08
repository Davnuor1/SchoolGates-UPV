using System.Collections.Generic;
using UnityEngine;

public class GateOfEmotionsNPCManager : MonoBehaviour
{
    [Header("Posiciones de los NPC (Parte 1)")]
    public Transform npcPosition_First;
    public Transform npcPosition_Second;

    [Header("Posiciones de actividad (Parte 1)")]
    public Transform actividadPosition_First;
    public Transform actividadPosition_Second;

    [Header("Posiciones de los NPC (Parte 2)")]
    public Transform npcPosition_Third;
    public Transform npcPosition_Fourth;

    [Header("Posiciones de actividad (Parte 2)")]
    public Transform actividadPosition_Third;
    public Transform actividadPosition_Fourth;

    [Header("NPCs de la primera parte")]
    public List<NPCEmotionData> npcEmotionDataList;

    [Header("NPCs de la segunda parte")]
    public List<NPCEmotionData> npcEmotionDataList_SecondPart;

    private Dictionary<string, GameObject> emotionToNPC = new();
    private Dictionary<string, GameObject> emotionToNPC_SecondPart = new();

    private void Awake()
    {
        foreach (var data in npcEmotionDataList)
        {
            string key = data.emotion.ToLower();
            if (!emotionToNPC.ContainsKey(key))
                emotionToNPC.Add(key, data.npcObject);
        }

        foreach (var data in npcEmotionDataList_SecondPart)
        {
            string key = data.emotion.ToLower();
            if (!emotionToNPC_SecondPart.ContainsKey(key))
                emotionToNPC_SecondPart.Add(key, data.npcObject);
        }
    }

    public void SpawnFirstNPC(string emotionKey)
    {
        string key = emotionKey.ToLower();
        if (emotionToNPC.ContainsKey(key))
        {
            GameObject npc = emotionToNPC[key];
            npc.transform.position = npcPosition_First.position;
            npc.SetActive(true);

            Transform child = npc.transform.childCount > 0 ? npc.transform.GetChild(0) : null;
            if (child != null) child.position = actividadPosition_First.position;
        }
    }

    public void SpawnSecondNPC(string emotionKey)
    {
        string key = emotionKey.ToLower();
        if (emotionToNPC.ContainsKey(key))
        {
            GameObject npc = emotionToNPC[key];
            npc.transform.position = npcPosition_Second.position;
            npc.SetActive(true);

            Transform child = npc.transform.childCount > 0 ? npc.transform.GetChild(0) : null;
            if (child != null) child.position = actividadPosition_Second.position;
        }
    }

    public void SpawnThirdNPC(string emotionKey)
    {
        string key = emotionKey.ToLower();
        if (emotionToNPC_SecondPart.ContainsKey(key))
        {
            GameObject npc = emotionToNPC_SecondPart[key];
            npc.transform.position = npcPosition_Third.position;
            npc.SetActive(true);

            Transform child = npc.transform.childCount > 0 ? npc.transform.GetChild(0) : null;
            if (child != null) child.position = actividadPosition_Third.position;
        }
    }

    public void SpawnFourthNPC(string emotionKey)
    {
        string key = emotionKey.ToLower();
        if (emotionToNPC_SecondPart.ContainsKey(key))
        {
            GameObject npc = emotionToNPC_SecondPart[key];
            npc.transform.position = npcPosition_Fourth.position;
            npc.SetActive(true);

            Transform child = npc.transform.childCount > 0 ? npc.transform.GetChild(0) : null;
            if (child != null) child.position = actividadPosition_Fourth.position;
        }
    }

    public GameObject GetSecondPartNPC(string emotion)
    {
        string key = emotion.ToLower();
        if (emotionToNPC_SecondPart.ContainsKey(key))
        {
            return emotionToNPC_SecondPart[key];
        }

        Debug.LogWarning("No se encontró NPC de segunda parte para emoción: " + emotion);
        return null;
    }
}

[System.Serializable]
public class NPCEmotionData
{
    public string emotion;
    public GameObject npcObject;
}
