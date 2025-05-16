using System;
using UnityEngine;

public static class PlayerInputEvents
{
    public static Action OnInteract;
    //public static Action OnOpenMenu;
    public static Action OnOpenMissions;

    public static void TriggerInteract() => OnInteract?.Invoke();
    //public static void TriggerOpenMenu() => OnOpenMenu?.Invoke();
    public static void TriggerOpenMissions() => OnOpenMissions?.Invoke();
}
