using GraphMaster.Visualization.Actions;
using UnityEngine;


namespace GraphMaster.UnityAdapter.VisualEffects 
{ 
    public interface GraphObjectVisualEffectsInterface : GraphMaster.Visualization.GraphObjectVisualEffectsInterface
    {

        public void SetColor(Color color);
        public Vector3 GetCenterPosition();


    }
}



