using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirtualInputReset : MonoBehaviour
{
    void LateUpdate()
    {
        VirtualInput.ResetInput();
    }
}

