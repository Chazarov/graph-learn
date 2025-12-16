using GraphMaster;
using GraphMaster.UnityAdapter.VisualEffects;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;


namespace GraphMaster.UnityAdapter.Visualization
{


    public class Cursor : MonoBehaviour, PerformerInterface
    {
        private Vector3 startPosition;

        [SerializeField] [Tooltip("Точное время перемещения к объекту (в секундах)")]
        private float moveDuration = 0.5f;
    
        [SerializeField] [Tooltip("Точное время возврата на начальную позицию (в секундах)")]
        private float returnDuration = 0.5f;
    
        [SerializeField] private Animator animator;
        [SerializeField] private float pointAnimationDuration = 1;

        private Coroutine currentMovementCoroutine = null;

        private List<GraphObjectVisualEffectsInterface> markedObjects = new();

        [SerializeField] private CursorAnimationHandler cursorAnimationHandler;
        public event Action OnMovementComplete;
        private bool whitingTheAnimation = false;

        public bool IsMoving => currentMovementCoroutine != null;


        public IEnumerator ExecuteActions(List<ActionInterface> actions)
        {
            foreach (ActionInterface action in actions)
            {
                action.Execute(this);
                while (IsMoving)
                {
                    yield return null;
                }

            }
        }


        void Start()
        {
            startPosition = transform.position;
            if (animator != null)
            {
                if(cursorAnimationHandler == null)
                {
                    cursorAnimationHandler = animator.GetBehaviour<CursorAnimationHandler>();
                }
                else
                {
                    Debug.LogWarning(" Сursor animation handler can't be a null. Please add this behavior for the animation to work correctly.");
                }
            
                cursorAnimationHandler.OnPointAnimationComplete += onCursorAnimationComplete;
            }
            else
            {
                Debug.LogWarning(" Add animator to the cursor object for animation processing");
            }
        }

        public void BackToStart()
        {
            if (currentMovementCoroutine != null)
            {
                StopCoroutine(currentMovementCoroutine);
            }
            currentMovementCoroutine = StartCoroutine(SmoothMoveRoutine(returnDuration, startPosition));
        }

        public void MarkThis(GraphPartInterface graphObject)
        {
            if (graphObject is GraphObjectUiActionsInterface uiActions)
            {
                var visualEffects = uiActions.GetVisualEffects();
                MarkThisInternal(visualEffects);
            }
            else
            {
                Debug.LogWarning("GraphPartInterface does not implement GraphObjectUiActionsInterface");
            }
        }

        private void MarkThisInternal(GraphObjectVisualEffectsInterface graphObject)
        {
            if (currentMovementCoroutine != null)
            {
                StopCoroutine(currentMovementCoroutine);
            }

            currentMovementCoroutine = StartCoroutine(SmoothHarassmentRoutine(moveDuration,
                graphObject, new MarkItVA()));
            markedObjects.Add(graphObject);
        }

        public void SetAdditionalValue(GraphPartInterface graphObject, string newValue)
        {
            if (graphObject is GraphObjectUiActionsInterface uiActions)
            {
                var visualEffects = uiActions.GetVisualEffects();
                if (visualEffects is GraphObjectVisualEffectsWithAdValueInterface withAdValue)
                {
                    CathUpAndSetAdditionalValueInternal(withAdValue, newValue);
                }
                else
                {
                    Debug.LogWarning("VisualEffects does not implement GraphObjectVisualEffectsWithAdValueInterface");
                }
            }
            else
            {
                Debug.LogWarning("GraphPartInterface does not implement GraphObjectUiActionsInterface");
            }
        }

        public void SetAdditionalValueFast(GraphPartInterface graphObject, string newValue)
        {
            if (graphObject is GraphObjectUiActionsInterface uiActions)
            {
                var visualEffects = uiActions.GetVisualEffects();
                if (visualEffects is GraphObjectVisualEffectsWithAdValueInterface withAdValue)
                {
                    withAdValue.SetAdditionalValue(newValue);
                }
                else
                {
                    Debug.LogWarning("VisualEffects does not implement GraphObjectVisualEffectsWithAdValueInterface");
                }
            }
            else
            {
                Debug.LogWarning("GraphPartInterface does not implement GraphObjectUiActionsInterface");
            }
        }

        private void CathUpAndSetAdditionalValueInternal(GraphObjectVisualEffectsWithAdValueInterface graphObject, string newValue)
        {
            if (currentMovementCoroutine != null)
            {
                StopCoroutine(currentMovementCoroutine);
            }

            currentMovementCoroutine = StartCoroutine(SmoothHarassmentRoutine(moveDuration,
                graphObject, new SetAdditionalValueVA(newValue)));
        }




        private void onCursorAnimationComplete()
        {
            whitingTheAnimation = false;
        }


        public void UnmarkAll()
        {
            foreach(var obj in markedObjects)
            {
                obj.RemoveMark();
            }
            markedObjects.Clear();
        }



        private IEnumerator SmoothMoveRoutine(float duration, Vector3 target)
        {
            Vector3 initialPosition = transform.position;
            float elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                float remainingTime = duration - elapsedTime;


                float t = Mathf.Clamp01(elapsedTime / duration);
                float smoothT = t * t * (3f - 2f * t);

                transform.position = Vector3.Lerp(initialPosition, target, smoothT);
                yield return null;
            }

            transform.position = target;

            if (cursorAnimationHandler != null)
            {
                while (whitingTheAnimation)
                {
                    yield return null;
                }
            }

            currentMovementCoroutine = null;

            OnMovementComplete?.Invoke();
        }

        private IEnumerator SmoothHarassmentRoutine<T>(float duration, T pray, VisualAction<T> action)
    where T : GraphObjectVisualEffectsInterface
        {     
            Vector3 initialPosition = transform.position;
            float elapsedTime = 0f;
            bool animationTriggered = false;
            Vector3 target = pray.GetCenterPosition();

            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                float remainingTime = duration - elapsedTime;
            
                if (!animationTriggered && pray != null && remainingTime <= pointAnimationDuration)
                {
                    if (animator != null)
                    {
                        animator.SetTrigger("Point");
                    }
                    whitingTheAnimation = true;
                    animationTriggered = true;
                }
            
                float t = Mathf.Clamp01(elapsedTime / duration);
                float smoothT = t * t * (3f - 2f * t);
            
                target = pray.GetCenterPosition();
                transform.position = Vector3.Lerp(initialPosition, target, smoothT);
                yield return null;
            }

            transform.position = pray.GetCenterPosition();

            if (cursorAnimationHandler != null)
            {
                while (whitingTheAnimation)
                {
                    transform.position = pray.GetCenterPosition();
                    yield return null;
                }
            }

            if (pray != null)
            {
                action.Execute(pray);
            }

            currentMovementCoroutine = null;

            OnMovementComplete?.Invoke();
        }

        private interface VisualAction<T> where T: GraphObjectVisualEffectsInterface
        {
            public void Execute(T obj);
        }

        private class MarkItVA : VisualAction<GraphObjectVisualEffectsInterface>
        {
            public MarkItVA() { }

            public void Execute(GraphObjectVisualEffectsInterface performer) {
                performer.MarkThis();
            }
        }

        private class SetAdditionalValueVA : VisualAction<GraphObjectVisualEffectsWithAdValueInterface>
        {
            private string newValue;
            public SetAdditionalValueVA(string newValue)
            {
                this.newValue = newValue;
            }

            public void Execute(GraphObjectVisualEffectsWithAdValueInterface performer)
            {
                performer.SetAdditionalValue(newValue);
            }
        }
    }


}
