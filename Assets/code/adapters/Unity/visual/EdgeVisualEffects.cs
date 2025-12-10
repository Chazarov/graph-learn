using System.Collections.Generic;
using UnityEngine;
using TMPro;
using GraphMaster.UnityAdapter.UI;

namespace GraphMaster.UnityAdapter.VisualEffects
{
    public class EdgeVisualEffects : MonoBehaviour, GraphObjectVisualEffectsInterface
    {
        [SerializeField] private LineRenderer line;
        [SerializeField] private LineRenderer directedLine;
        [SerializeField] private EdgeCollider2D edgeCollider;
        [SerializeField] private TextMeshProUGUI weightText;
        [SerializeField] private Canvas edgeToolBar;

        [Header("Select animation")]
        [SerializeField] private Color selectColor;
        [SerializeField] private float selectAnimationDuraton = 0.3f;

        [Header("Mark Animation")]
        [SerializeField] private float markAnimationDuration = 0.5f;
        [SerializeField] private Color markColor;
        
        [Header("Point Animation")]
        [SerializeField] private Transform pointerObject;
        [SerializeField] private float pointerAnimationDuration = 0.5f;

        [Header("Directed View")]
        [SerializeField] private int edgeOffsetPositionNumber = 0;
        [SerializeField] private float spreadKof = 1;
        [SerializeField] [Range(3, 100)] private int directedLineSegmentsCount = 5;

        private bool directedView = false;

        private LineRenderer activeLine;
        private NodeUI sourceNode;
        private NodeUI targetNode;


        
        private Coroutine currentMarkCoroutine;
        private Coroutine currentPointerCoroutine;
        private Coroutine selectCoroutine;
        
        private Color initialStartColor;
        private Color initialEndColor;
        private int baseSortingLayer;

        private void Update()
        {
            CalculateEdgeVisualPos();
        }

        public void Initialize(int graphEdgesSequenseCount, NodeUI source, NodeUI target)
        {
            CheckGameObjectContent();
            SetVisualLayer(graphEdgesSequenseCount);
            activeLine = line;
            directedLine.gameObject.SetActive(false);
            
            sourceNode = source;
            targetNode = target;
            
            SetupInitialLine();
        }

        public void SetDirectedView(bool isDirected)
        {
            directedView = isDirected;
            
            if (directedView)
            {
                line.gameObject.SetActive(false);
                directedLine.gameObject.SetActive(true);
                activeLine = directedLine;
                
                initialStartColor = activeLine.startColor;
                initialEndColor = activeLine.endColor;
            }
            else
            {
                directedLine.gameObject.SetActive(false);
                line.gameObject.SetActive(true);
                activeLine = line;
                
                initialStartColor = activeLine.startColor;
                initialEndColor = activeLine.endColor;
            }
            
        }

        public void SetEdgeCenterOffset(int offset)
        {
            edgeOffsetPositionNumber = offset;
        }

        private float CalculateArcOffset(float chordLength, float centerOffset, float distanceFromCenter)
        {
            float halfChord = chordLength / 2f;
            float h = Mathf.Abs(centerOffset);
            if (h < 0.001f) return centerOffset;
            
            float radius = (halfChord * halfChord + h * h) / (2f * h);
            float y = Mathf.Sqrt(radius * radius - distanceFromCenter * distanceFromCenter) - (radius - h);
            
            return y;
        }


        private void CalculateEdgeVisualPos()
        {
            if (sourceNode == null || targetNode == null) return;

            Vector3 sourcePosition = sourceNode.transform.position;
            Vector3 targetPosition = targetNode.transform.position;
            float z = sourcePosition.z = targetPosition.z = transform.position.z;

            

            Vector3 textOffset = Vector3.zero;

            if (edgeOffsetPositionNumber > 0)
            {
                int offset = (edgeOffsetPositionNumber / 2 )+ (edgeOffsetPositionNumber%2);
                bool isLocatedToTheRight = edgeOffsetPositionNumber % 2 == 0;
                float offsetF = offset * spreadKof;
                if (!isLocatedToTheRight)
                {
                    offsetF *= -1;
                }

                

                int segmentsCount = Mathf.Clamp(directedLineSegmentsCount, 3, 100);
                activeLine.positionCount = segmentsCount;

                Vector3 direction = (targetPosition - sourcePosition).normalized;
                Vector3 perpendicular = new Vector3(-direction.y, direction.x, z);

                float chordLength = Vector3.Distance(sourcePosition, targetPosition);
                
                List<Vector2> colliderPoints = new List<Vector2>();
                
                for (int i = 0; i < segmentsCount; i++)
                {
                    float t = (float)i / (segmentsCount - 1);
                    Vector3 pointOnLine = Vector3.Lerp(sourcePosition, targetPosition, t);
                    
                    float distanceFromCenter = chordLength * (t - 0.5f);
                    float arcOffset = CalculateArcOffset(chordLength, offsetF, Mathf.Abs(distanceFromCenter));
                    
                    Vector3 finalPosition = pointOnLine + perpendicular * arcOffset;
                    activeLine.SetPosition(i, finalPosition);
                    colliderPoints.Add(finalPosition);
                }

                edgeCollider.SetPoints(colliderPoints);

                Debug.Log($"edge {this.name}    {offsetF},   {perpendicular.x}, {perpendicular.y}");

                textOffset += perpendicular * Mathf.Abs(offsetF);
            }
            else
            {
                activeLine.positionCount = 2;
                activeLine.SetPosition(0, sourcePosition);
                activeLine.SetPosition(1, targetPosition);
                edgeCollider.SetPoints(new List<Vector2> { sourcePosition, targetPosition });
            }

            UpdateWeightTextPosition(sourcePosition, targetPosition, textOffset);
        }


        public void SelectThisAnimation()
        {
            if (selectCoroutine != null)
            {
                StopCoroutine(selectCoroutine);
            }
            selectCoroutine = StartCoroutine(AnimateColorTransition(selectColor, selectAnimationDuraton));
        }

        public void DeselectThisAnimation()
        {
            if (selectCoroutine != null)
            {
                StopCoroutine(selectCoroutine);
            }
            selectCoroutine = StartCoroutine(AnimateColorToInitial(selectAnimationDuraton));
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
            edgeCollider.SetPoints(new List<Vector2> { sourcePosition, targetPosition });
            
            initialStartColor = activeLine.startColor;
            initialEndColor = activeLine.endColor;
        }

        private void UpdateWeightTextPosition(Vector3 sourcePos, Vector3 targetPos, Vector3 offset)
        {
            if (edgeToolBar == null) return;

            Vector3 centerPos = (sourcePos + targetPos) / 2f;
            edgeToolBar.transform.position = centerPos + offset;

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
            this.baseSortingLayer = -graphEdgesSequenseCount * 3;


            edgeToolBar.sortingOrder = baseSortingLayer + 2;
            directedLine.sortingOrder = baseSortingLayer;
            line.sortingOrder = baseSortingLayer;
            
        }

        public void CheckGameObjectContent()
        {
            if (line == null)
            {
                throw new System.Exception(" Line Renderer can't be a null");
            }

            if (directedLine == null)
            {
                throw new System.Exception(" directedLine Renderer can't be a null");
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
            return;
        }

        public void MarkThisAnimation(bool reverseDirection = false)
        {
            if (currentMarkCoroutine != null)
            {
                StopCoroutine(currentMarkCoroutine);
            }
            currentMarkCoroutine = StartCoroutine(AnimateColorTransition(markColor, markAnimationDuration));
        }
        
        public void MarkThisAnimation()
        {
            MarkThisAnimation(false);
        }

        public void RemoveMarkAnimation()
        {
            if (currentMarkCoroutine != null)
            {
                StopCoroutine(currentMarkCoroutine);
            }
            currentMarkCoroutine = StartCoroutine(AnimateColorToInitial(markAnimationDuration));
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

        private System.Collections.IEnumerator AnimateColorTransition(Color targetColor, float duration)
        {
            Color startColorBegin = activeLine.startColor;
            Color endColorBegin = activeLine.endColor;
            float elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                float t = elapsedTime / duration;
                activeLine.startColor = Color.Lerp(startColorBegin, targetColor, t);
                activeLine.endColor = Color.Lerp(endColorBegin, targetColor, t);
                yield return null;
            }

            activeLine.startColor = targetColor;
            activeLine.endColor = targetColor;
        }

        private System.Collections.IEnumerator AnimateColorToInitial(float duration)
        {
            Color startColorBegin = activeLine.startColor;
            Color endColorBegin = activeLine.endColor;
            float elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                float t = elapsedTime / duration;
                activeLine.startColor = Color.Lerp(startColorBegin, initialStartColor, t);
                activeLine.endColor = Color.Lerp(endColorBegin, initialEndColor, t);
                yield return null;
            }

            activeLine.startColor = initialStartColor;
            activeLine.endColor = initialEndColor;
        }
    }
}

