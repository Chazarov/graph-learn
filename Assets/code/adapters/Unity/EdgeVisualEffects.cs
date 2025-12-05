using System.Collections.Generic;
using UnityEngine;
using TMPro;
using GraphMaster.UnityAdapter.UI;

namespace GraphMaster.UnityAdapter
{
    public class EdgeVisualEffects : MonoBehaviour
    {
        [SerializeField] private LineRenderer line;
        [SerializeField] private LineRenderer selectedLine;
        [SerializeField] private EdgeCollider2D edgeCollider;
        [SerializeField] private TextMeshProUGUI weightText;
        [SerializeField] private Canvas edgeToolBar;

        private LineRenderer activeLine;
        private UIPositioned2Node sourceNode;
        private UIPositioned2Node targetNode;

        public void Initialize(int graphEdgesSequenseCount, UIPositioned2Node source, UIPositioned2Node target)
        {
            CheckGameObjectContent();
            SetVisualLayer(graphEdgesSequenseCount);
            activeLine = line;
            selectedLine.gameObject.SetActive(false);
            
            sourceNode = source;
            targetNode = target;
            
            SetupInitialLine();
        }

        public void UpdateFrame(float zPosition)
        {
            if (sourceNode == null || targetNode == null) return;
            
            Vector3 sourcePosition = sourceNode.transform.position;
            Vector3 targetPosition = targetNode.transform.position;
            sourcePosition.z = targetPosition.z = zPosition;
            
            activeLine.SetPosition(0, sourcePosition);
            activeLine.SetPosition(1, targetPosition);
            edgeCollider.SetPoints(new List<Vector2> { sourcePosition, targetPosition });
            UpdateWeightTextPosition(sourcePosition, targetPosition);
        }

        public void StartSelectionAnimation()
        {
            line.gameObject.SetActive(false);
            selectedLine.gameObject.SetActive(true);
            activeLine = selectedLine;
        }

        public void StartDeselectionAnimation()
        {
            selectedLine.gameObject.SetActive(false);
            line.gameObject.SetActive(true);
            activeLine = line;
        }

        public void UpdateWeightDisplay(float weight)
        {
            if (weightText != null)
            {
                weightText.text = weight.ToString();
            }
        }

        private void SetupInitialLine()
        {
            if (sourceNode == null || targetNode == null) return;
            
            Vector3 sourcePosition = sourceNode.transform.position;
            Vector3 targetPosition = targetNode.transform.position;
            
            activeLine.positionCount = 2;
            activeLine.SetPosition(0, sourcePosition);
            activeLine.SetPosition(1, targetPosition);
            selectedLine.positionCount = 2;
            edgeCollider.SetPoints(new List<Vector2> { sourcePosition, targetPosition });
        }

        private void UpdateWeightTextPosition(Vector3 sourcePos, Vector3 targetPos)
        {
            if (edgeToolBar == null) return;

            Vector3 centerPos = (sourcePos + targetPos) / 2f;
            edgeToolBar.transform.position = centerPos;

            Vector3 direction = (targetPos - sourcePos).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            if (angle > 90f || angle < -90f)
            {
                angle += 180f;
            }

            edgeToolBar.transform.rotation = Quaternion.Euler(0, 0, angle);
        }

        private void SetVisualLayer(int graphEdgesSequenseCount)
        {
            edgeToolBar.sortingOrder = -graphEdgesSequenseCount * 2 + 1;
            line.sortingOrder = -graphEdgesSequenseCount * 2;
            selectedLine.sortingOrder = -graphEdgesSequenseCount * 2;
        }

        private void CheckGameObjectContent()
        {
            if (line == null)
            {
                throw new System.Exception(" Line Renderer can't be a null");
            }

            if (selectedLine == null)
            {
                throw new System.Exception(" SelectedLine Renderer can't be a null");
            }

            if (edgeCollider == null)
            {
                throw new System.Exception(" EdgeCollider2D  edgeCollider can't be a null");
            }

            if (weightText == null)
            {
                throw new System.Exception(" TextMeshPro weightText can't be a null");
            }

            if (edgeToolBar == null)
            {
                throw new System.Exception(" Canvas edgeToolBar can't be a null");
            }
        }
    }
}

