using UnityEngine;
using System.Collections.Generic;

public class GateOfEmotionsNPCManager : MonoBehaviour
{
    [System.Serializable]
    public class EmotionNPC
    {
        public string emotionKey; // Por ejemplo: "anger", "fear", etc.
        public GameObject npcObject;
    }

    [Header("NPCs por emoción")]
    public List<EmotionNPC> emotionNPCs = new List<EmotionNPC>();

    [Header("Puntos de destino")]
    public Transform npcPosition_First;
    public Transform npcPosition_Second;

    private Dictionary<string, GameObject> emotionToNPC = new Dictionary<string, GameObject>();

    private void Awake()
    {
        foreach (var npc in emotionNPCs)
        {
            string key = npc.emotionKey.ToLower();
            if (!emotionToNPC.ContainsKey(key))
            {
                emotionToNPC.Add(key, npc.npcObject);
            }
        }
    }

    public void SpawnFirstNPC(string emotionKey)
    {
        string key = emotionKey.ToLower();
        if (emotionToNPC.ContainsKey(key))
        {
            emotionToNPC[key].transform.position = npcPosition_First.position;
            emotionToNPC[key].SetActive(true);
        }
        else
        {
            Debug.LogWarning("No se encontró NPC para la emoción: " + emotionKey);
        }
    }

    public void SpawnSecondNPC(string emotionKey)
    {
        string key = emotionKey.ToLower();
        if (emotionToNPC.ContainsKey(key))
        {
            emotionToNPC[key].transform.position = npcPosition_Second.position;
            emotionToNPC[key].SetActive(true);
        }
        else
        {
            Debug.LogWarning("No se encontró NPC para la emoción: " + emotionKey);
        }
    }
}
