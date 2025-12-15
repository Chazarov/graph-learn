using Domain;
using System.Collections;
using System.Collections.Generic;

namespace GraphMaster.UnityAdapter.Visualization
{
    public class BreadthDepthVisualizer : IAlgorithmVisualizer
    {
        private List<GraphObjectUiActionsInterface> objectsToMark;
        private List<GraphObjectUiActionsInterface> processedObjects = new List<GraphObjectUiActionsInterface>();
        private Cursor cursor;

        public BreadthDepthVisualizer(List<GraphObjectUiActionsInterface> objectsToMark, Cursor cursor)
        {
            this.objectsToMark = objectsToMark;
            this.cursor = cursor;
        }

        public IEnumerator StartVisualisation()
        {
            for (int i = 0; i < objectsToMark.Count; i++)
            {
                var currentObject = objectsToMark[i];
                
                cursor.MarkObject(currentObject);
                processedObjects.Add(currentObject);

                while (cursor.IsMoving)
                {
                    yield return null;
                }
            }

            cursor.BackToStart();
            
            while (cursor.IsMoving)
            {
                yield return null;
            }
        }

        public void ClearVisualisation()
        {
            foreach (var obj in processedObjects)
            {
                obj.RemoveMark();
            }
            processedObjects.Clear();
            cursor?.BackToStart();
        }
    }
}

