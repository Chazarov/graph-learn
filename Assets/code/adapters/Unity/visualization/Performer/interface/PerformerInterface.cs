using GraphMaster;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public interface PerformerInterface
{
    public void MarkThis(GraphPartInterface graphObject);
    public  void SetAdditionalValue(GraphPartInterface graphObject, string newValue);
    public  void SetAdditionalValueFast(GraphPartInterface graphObject, string newValue);

    public  void SetColor(GraphPartInterface target, System.Drawing.Color color);

    public void HideIt(GraphPartInterface target);

    public void UnmarkItFast(GraphPartInterface target);

    public void HideAdditionalValueFast(GraphPartInterface target);
}
