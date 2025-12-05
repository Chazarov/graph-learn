using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface GraphObjectVisualEffectsInterface
{
    public void StartSelectionAnimation();

    public void StartDeselectionAnimation();

    public void PointThisAnimation();

    public void RemovePointerAnimation();

    public void MarkThisAnimation();

    public void RemoveMarkAnimation();
}
