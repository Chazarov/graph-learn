using Domain;
using System.Collections;
using System.Collections.Generic;

namespace GraphMaster.UnityAdapter.Visualization
{
    public class BreadthDepthVisualizer : IAlgorithmVisualizer
    {
        private List<ActionInterface> actions;
        private Cursor cursor;

        public BreadthDepthVisualizer(List<ActionInterface> actions, Cursor cursor)
        {
            this.actions = actions;
            this.cursor = cursor;
        }

        public IEnumerator StartVisualisation()
        {
            return cursor.ExecuteActions(actions);
        }

        public void ClearVisualisation()
        {
            cursor.UnmarkAll();
        }
    }
}
