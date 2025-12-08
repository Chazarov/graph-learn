using System.Collections.Generic;
using UnityEngine;
using TMPro;
using GraphMaster.UnityAdapter.UI;

namespace GraphMaster.UnityAdapter.VisualEffects
{
    public class EdgeVisualEffects : MonoBehaviour, GraphObjectVisualEffectsInterface
    {
        [SerializeField] private LineRenderer line;
        [SerializeField] private LineRenderer selectedLine;
        [SerializeField] private EdgeCollider2D edgeCollider;
        [SerializeField] private TextMeshProUGUI weightText;
        [SerializeField] private Canvas edgeToolBar;
        
        [Header("Mark Animation")]
        [SerializeField] private LineRenderer markLine;
        [SerializeField] private float markAnimationDuration = 1f;
        
        [Header("Point Animation")]
        [SerializeField] private Transform pointerObject;
        [SerializeField] private float pointerAnimationDuration = 0.5f;

        private LineRenderer activeLine;
        private NodeUI sourceNode;
        private NodeUI targetNode;
        
        private Coroutine currentMarkCoroutine;
        private Coroutine currentPointerCoroutine;
        private bool isMarkAnimationReversed = false;


        private void Update()
        {
            UpdateFrame();
        }

        public void Initialize(int graphEdgesSequenseCount, NodeUI source, NodeUI target)
        {
            CheckGameObjectContent();
            SetVisualLayer(graphEdgesSequenseCount);
            activeLine = line;
            selectedLine.gameObject.SetActive(false);
            markLine.gameObject.SetActive(false);
            
            sourceNode = source;
            targetNode = target;
            
            SetupInitialLine();
        }

        public void UpdateFrame()
        {
            if (sourceNode == null || targetNode == null) return;
            
            Vector3 sourcePosition = sourceNode.transform.position;
            Vector3 targetPosition = targetNode.transform.position;
            sourcePosition.z = targetPosition.z = transform.position.z;
            
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
            int baseSortingLayer = -graphEdgesSequenseCount * 3;


            edgeToolBar.sortingOrder = baseSortingLayer + 2;
            markLine.sortingOrder = baseSortingLayer + 1;
            line.sortingOrder = baseSortingLayer;
            selectedLine.sortingOrder = baseSortingLayer;
            
        }

        public void CheckGameObjectContent()
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

        public void PointThisAnimation()
        {
            if (pointerObject != null)
            {
                if (currentPointerCoroutine != null)
                {
                    StopCoroutine(currentPointerCoroutine);
                }
                currentPointerCoroutine = StartCoroutine(AnimatePointerToCenter());
            }
        }

        public void RemovePointerAnimation()
        {
            // Функция отката анимации пустая согласно требованиям
        }

        public void MarkThisAnimation(bool reverseDirection = false)
        {
            if (markLine != null)
            {
                if (currentMarkCoroutine != null)
                {
                    StopCoroutine(currentMarkCoroutine);
                }
                
                isMarkAnimationReversed = reverseDirection;
                markLine.gameObject.SetActive(true);
                currentMarkCoroutine = StartCoroutine(AnimateMarkLine(reverseDirection));
            }
        }
        
        public void MarkThisAnimation()
        {
            MarkThisAnimation(false);
        }

        public void RemoveMarkAnimation()
        {
            if (markLine != null)
            {
                if (currentMarkCoroutine != null)
                {
                    StopCoroutine(currentMarkCoroutine);
                }
                
                currentMarkCoroutine = StartCoroutine(AnimateMarkLineReverse(!isMarkAnimationReversed));
            }
        }

        private System.Collections.IEnumerator AnimateMarkLine(bool reverseDirection)
        {
            if (sourceNode == null || targetNode == null) yield break;

            Vector3 startPos = reverseDirection ? targetNode.transform.position : sourceNode.transform.position;
            Vector3 endPos = reverseDirection ? sourceNode.transform.position : targetNode.transform.position;
            
            markLine.positionCount = 2;
            float elapsedTime = 0f;

            while (elapsedTime < markAnimationDuration)
            {
                elapsedTime += Time.deltaTime;
                float t = elapsedTime / markAnimationDuration;
                
                Vector3 currentEndPos = Vector3.Lerp(startPos, endPos, t);
                
                markLine.SetPosition(0, startPos);
                markLine.SetPosition(1, currentEndPos);
                
                yield return null;
            }

            markLine.SetPosition(0, startPos);
            markLine.SetPosition(1, endPos);
        }

        private System.Collections.IEnumerator AnimateMarkLineReverse(bool reverseDirection)
        {
            if (sourceNode == null || targetNode == null) yield break;

            Vector3 startPos = reverseDirection ? targetNode.transform.position : sourceNode.transform.position;
            Vector3 endPos = reverseDirection ? sourceNode.transform.position : targetNode.transform.position;
            
            float elapsedTime = 0f;

            while (elapsedTime < markAnimationDuration)
            {
                elapsedTime += Time.deltaTime;
                float t = 1f - (elapsedTime / markAnimationDuration);
                
                Vector3 currentEndPos = Vector3.Lerp(startPos, endPos, t);
                
                markLine.SetPosition(0, startPos);
                markLine.SetPosition(1, currentEndPos);
                
                yield return null;
            }

            markLine.gameObject.SetActive(false);
        }

        private System.Collections.IEnumerator AnimatePointerToCenter()
        {
            if (pointerObject == null || sourceNode == null || targetNode == null) yield break;

            Vector3 startPosition = pointerObject.position;
            Vector3 edgeCenter = (sourceNode.transform.position + targetNode.transform.position) / 2f;
            float elapsedTime = 0f;

            while (elapsedTime < pointerAnimationDuration)
            {
                elapsedTime += Time.deltaTime;
                float t = elapsedTime / pointerAnimationDuration;
                pointerObject.position = Vector3.Lerp(startPosition, edgeCenter, t);
                yield return null;
            }

            pointerObject.position = edgeCenter;
        }
    }
}

