using GraphMaster;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public interface PerformerInterface
{
    void MarkThis(GraphPartInterface graphObject);
    void SetAdditionalValue(GraphPartInterface graphObject, string newValue);
    void SetAdditionalValueFast(GraphPartInterface graphObject, string newValue);

    void SetColor(GraphPartInterface target, System.Drawing.Color color);

    void HideIt(GraphPartInterface target);
}
