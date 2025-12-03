using GraphMaster.UnityAdapter;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MenuManager : MonoBehaviour
{

    [SerializeField] private TMP_InputField setWeightInput;
    [SerializeField] private GraphVisual graphVisual;

    public void SetEdgeWeightByInput()
    {
        float value = float.Parse(setWeightInput.text);
        graphVisual.SetSelectedEdgesWeight(value);
    }

}
