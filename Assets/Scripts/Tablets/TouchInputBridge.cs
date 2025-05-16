using UnityEngine;
using PixelCrushers.DialogueSystem;
public class TouchInputBridge : MonoBehaviour
{
    private bool isTouchERequested = false;
    public void OnTouchInteract()
    {
        isTouchERequested = true;
    }



    public void OnTouchOpenMissions()
    {
        // Simula presionar J
        //PlayerInputEvents.TriggerOpenMissions();
        DialogueManager.instance.GetComponentInChildren<QuestLogWindow>().Open();
    }
    void Update()
    {
        if (isTouchERequested)
        {
            VirtualInput.PressVirtualE();
            isTouchERequested = false;
        }
    }
}
