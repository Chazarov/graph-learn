using GraphMaster;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public interface PerformerInterface
{
    public void MarkThis(IGraphPart graphObject);
    public  void SetAdditionalValue(IGraphPart graphObject, string newValue);
    public  void SetAdditionalValueFast(IGraphPart graphObject, string newValue);

    public  void SetColor(IGraphPart target, System.Drawing.Color color);

    public void HideIt(IGraphPart target);

    public void UnmarkItFast(IGraphPart target);

    public void HideAdditionalValueFast(IGraphPart target);
}
