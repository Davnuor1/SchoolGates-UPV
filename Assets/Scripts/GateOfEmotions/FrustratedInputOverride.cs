using UnityEngine;
using System.Collections.Generic;

public class FrustratedInputOverride : MonoBehaviour
{
    private Dictionary<string, KeyCode> currentMapping = new();

    public void SetControlMapping(Dictionary<string, KeyCode> newMapping)
    {
        currentMapping = newMapping;
    }

    public float GetHorizontal()
    {
        float value = 0f;

        if (Input.GetKey(currentMapping.GetValueOrDefault("right", KeyCode.D)))
        {
            value += 1f;
        }
        if (Input.GetKey(currentMapping.GetValueOrDefault("left", KeyCode.A)))
        {
            value -= 1f;
        }

        return value;
    }

    public float GetVertical()
    {
        float value = 0f;

        if (Input.GetKey(currentMapping.GetValueOrDefault("up", KeyCode.W)))
        {
            value += 1f;
        }
        if (Input.GetKey(currentMapping.GetValueOrDefault("down", KeyCode.S)))
        {
            value -= 1f;
        }

        return value;
    }
}
