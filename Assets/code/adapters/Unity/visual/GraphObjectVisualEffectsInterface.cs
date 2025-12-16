using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface GraphObjectVisualEffectsInterface
{
    public void SelectThisAnimation();

    public void DeselectThisAnimation();

    public void MarkThis();

    public void RemoveMark();

    public Vector3 GetCenterPosition();

    
}
