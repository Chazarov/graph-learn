using GraphMaster.UnityAdapter;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    [SerializeField] private  Toggle setDirectedToggle;

    [SerializeField] private Toggle setParallelEdgesToggle;

    [SerializeField] private GraphUI graph;

    private void Start()
    {
        CheckCompanents();
        syncSettingsParametrs();
    }

    private void syncSettingsParametrs()
    {
        setDirectedToggle.SetIsOnWithoutNotify(graph.GetIsDirected());
        setParallelEdgesToggle.SetIsOnWithoutNotify(graph.GetIsParallel());
    }

    private void CheckCompanents()
    {
        if(setDirectedToggle == null)
        {
            throw new System.Exception("setDirectedToggle can't be a null");
        }

        if(setParallelEdgesToggle == null)
        {
            throw new System.Exception("setParallelEdgesToggle can't be a null");
        }

        if (graph == null)
        {
            throw new System.Exception("graph can't be a null");
        }
    }   

    public void OnSetDirectedChange()
    {
        try
        {
            graph.SetDirected(setDirectedToggle.isOn);
        }
        catch 
        {
            setDirectedToggle.SetIsOnWithoutNotify(graph.GetIsDirected());
            throw;
        }
        
    }

    public void OnSetParallelEdgesChange()
    {
        try
        {
            graph.SetParralel(setParallelEdgesToggle.isOn);
        }
        catch
        {
            setParallelEdgesToggle.SetIsOnWithoutNotify(graph.GetIsParallel());
        }
        
    }
}
