using GraphMaster.UnityAdapter.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface GraphObjectUiActionsInterface
{
    public void Select();

    public void Deselect();


    public void MarkThis();

    public void RemoveMark();

    public Vector3 GetCenterPosition();


}
