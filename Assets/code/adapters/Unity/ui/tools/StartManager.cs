using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StartManager : MonoBehaviour
{
    [SerializeField] private UnityEvent onStart;
    void Awake()
    {
        onStart.Invoke();
    }
}
