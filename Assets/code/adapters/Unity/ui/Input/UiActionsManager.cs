using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "UiActionsManager", menuName = "Events/UiActionsManager")]
public class UiActionsManager : ScriptableObject
{
    [SerializeField] private bool rootReplacementIsAllowed = false;

    [SerializeField] private bool selectGraphObjectsAllowed = false;

    public void SetRootReplacementIsAllowed(bool value)
    {
        rootReplacementIsAllowed = value;
    }

    public bool GetRootReplacementIsAllowed()
    {
        return rootReplacementIsAllowed;
    }

    public void SetSelectGraphObjectsAllowed(bool value)
    {
        selectGraphObjectsAllowed = value;
    }

    public bool GetSelectGraphObjectsAllowed()
    {
        return selectGraphObjectsAllowed;
    }

}
