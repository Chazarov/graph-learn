using System;
using System.Collections.Generic;

namespace Domain
{
    public interface AlgorithmVisualizerInterface
    {
        void Visualize(List<GraphObjectUiActionsInterface> objectsToPoint, List<GraphObjectUiActionsInterface> objectsToMark);
        void Clear();
        void Stop();
        void Pause();
        void Resume();
        void Cancel();
    }
}


