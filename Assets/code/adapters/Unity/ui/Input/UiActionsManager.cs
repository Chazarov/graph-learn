using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "UiActionsManager", menuName = "Events/UiActionsManager")]
public class UiActionsManager : ScriptableObject
{
    [SerializeField] private bool rootReplacementIsAllowed = false;

    public void SetRootReplacementIsAllowed(bool value)
    {
        rootReplacementIsAllowed = value;
    }

    public bool GetRootReplacementIsAllowed()
    {
        return rootReplacementIsAllowed;
    }

}
