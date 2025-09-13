using UnityEngine;
using System.Collections;

public class GateBootstrap : MonoBehaviour
{
    public GateMarker marker;

    private IEnumerator Start()
    {
        yield return null;
        if (marker != null && UserDataManager.Instance != null)
        {
            UserDataManager.Instance.BeginGateSession(marker.gateId);
        }
    }

}
