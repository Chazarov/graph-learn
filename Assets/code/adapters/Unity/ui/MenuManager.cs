using GraphMaster.UnityAdapter;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MenuManager : MonoBehaviour
{

    [SerializeField] private TMP_InputField setWeightInput;
    [SerializeField] private GraphUI graphVisual;

    private void OnEnable()
    {
        graphVisual.EdgeSelected.AddListener(EdgeSelected);
    }

    public void SetEdgeWeightByInput()
    {
        float value = float.Parse(setWeightInput.text);
        if (value < 0f) {
            value *= -1;
            setWeightInput.text = "" + value;
        }
        graphVisual.SetSelectedEdgesWeight(value);
    }


    public void OnlyPositiveInputCheck()
    {
        float value = float.Parse(setWeightInput.text);
        if (value < 0f)
        {
            setWeightInput.SetTextWithoutNotify("" + (-value));
        }
    }

    void EdgeSelected(string edgeName)
    {
        EdgeUI edge = graphVisual.GetEdge(edgeName);
        setWeightInput.SetTextWithoutNotify(edge.GetWeight().ToString());
    }

    private void OnDisable()
    {
        graphVisual.EdgeSelected.RemoveListener(EdgeSelected);
    }

}
