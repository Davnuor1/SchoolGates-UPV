using UnityEngine;

public class TestBotonE : MonoBehaviour
{
    void Update()
    {
        if (VirtualInput.GetKeyDownE())
        {
            Debug.Log("¡Detectado: E pulsado!");
        }
    }
}
