using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimStateEventListener : MonoBehaviour
{
    public Action<string, string> OnOccursAnimStateEvent;
    
    public void OccursAnimStateEvent(string eventName, string parameter)
    {
        OnOccursAnimStateEvent?.Invoke(eventName, parameter);
    }
}
