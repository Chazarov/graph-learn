using System.Collections;

namespace GraphMaster.UnityAdapter.Visualization
{
    public interface IAlgorithmVisualizer
    {
        IEnumerator StartVisualisation();
        void ClearVisualisation();
    }
}

