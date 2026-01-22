using GraphMaster.Visualization;
using System;
using System.Collections.Generic;

namespace Domain
{
    public interface AlgorithmVisualizerInterface
    {
        void Visualize(List<GraphObjectVisualEffectsInterface> objectsToPoint, List<GraphObjectVisualEffectsInterface> objectsToMark);
        void Clear();
        void Stop();
        void Pause();
        void Resume();
        void Cancel();
    }
}


