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

    private readonly Dictionary<EmotionId, GameObject> part1Map = new();
    private readonly Dictionary<EmotionId, GameObject> part2Map = new();

    private void Awake()
    {
        foreach (var data in npcEmotionDataList)
            if (data != null && !part1Map.ContainsKey(data.emotion))
                part1Map.Add(data.emotion, data.npcObject);

        foreach (var data in npcEmotionDataList_SecondPart)
            if (data != null && !part2Map.ContainsKey(data.emotion))
                part2Map.Add(data.emotion, data.npcObject);
    }

    public void SpawnFirstNPC(EmotionId id)
    {
        if (!part1Map.TryGetValue(id, out var npc) || npc == null) return;
        npc.transform.position = npcPosition_First.position;
        npc.SetActive(true);

        var child = npc.transform.childCount > 0 ? npc.transform.GetChild(0) : null;
        if (child != null) child.position = actividadPosition_First.position;
    }

    public void SpawnSecondNPC(EmotionId id)
    {
        if (!part1Map.TryGetValue(id, out var npc) || npc == null) return;
        npc.transform.position = npcPosition_Second.position;
        npc.SetActive(true);

        var child = npc.transform.childCount > 0 ? npc.transform.GetChild(0) : null;
        if (child != null) child.position = actividadPosition_Second.position;
    }

    public void SpawnThirdNPC(EmotionId id)
    {
        if (!part2Map.TryGetValue(id, out var npc) || npc == null) return;
        npc.transform.position = npcPosition_Third.position;
        npc.SetActive(true);

        var child = npc.transform.childCount > 0 ? npc.transform.GetChild(0) : null;
        if (child != null) child.position = actividadPosition_Third.position;
    }

    public void SpawnFourthNPC(EmotionId id)
    {
        if (!part2Map.TryGetValue(id, out var npc) || npc == null) return;
        npc.transform.position = npcPosition_Fourth.position;
        npc.SetActive(true);

        var child = npc.transform.childCount > 0 ? npc.transform.GetChild(0) : null;
        if (child != null) child.position = actividadPosition_Fourth.position;
    }

    public GameObject GetSecondPartNPC(EmotionId id)
    {
        part2Map.TryGetValue(id, out var npc);
        return npc;
    }
}

[System.Serializable]
public class NPCEmotionData
{
    public EmotionId emotion;
    public GameObject npcObject;
}
