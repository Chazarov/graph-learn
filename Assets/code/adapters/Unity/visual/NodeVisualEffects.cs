using UnityEngine;
using TMPro;
using JetBrains.Annotations;

namespace GraphMaster.UnityAdapter.VisualEffects
{

    public interface GraphObjectVisualEffectsWithAdValueInterface : GraphObjectVisualEffectsInterface
    {
        public void SetAdditionalValue(string NewValue);

        public void HideAdditionalValue();
    }

    public class NodeVisualEffects : MonoBehaviour, GraphObjectVisualEffectsWithAdValueInterface
    {
        [SerializeField] private TextMeshProUGUI nameVisual;
        [SerializeField] private Canvas nodeToolBar;

        [Header("Selection Animation")]
        [SerializeField] private Color selectedColor;
        [SerializeField] private Color selectedTextColor;
        [SerializeField] private Vector2 selectedScale;
        private Color defaultColor;
        private Color defaultTextColor;
        private Vector2 defaultScale;

        [SerializeField] private SpriteRenderer nodeSpriteRenderer;

        [Header("Mark Animation")]
        [SerializeField] private SpriteRenderer markSpriteRenderer;
        [SerializeField] private Animator markAnimator;


        [Header("Root Animation")]
        [SerializeField] private GameObject rootMark;

        [Header("Hide it animation")]
        [SerializeField] private Color hideColor;


        [Header("Additional Values")]
        public AdditionalValueController AdditionalValueController;


        private Vector3 startMarkRootScale;
        private bool isRoot = false;
        private bool isMarked = false;


        private void Start()
        {
            defaultColor = nodeSpriteRenderer.color;
            defaultTextColor = nameVisual.color;
            defaultScale = transform.localScale;
        }

        private void Awake()
        {
            rootMark = GameObject.FindGameObjectWithTag("RootMark");

            if (rootMark == null)
            {
                Debug.LogError("Root mark cant be a null! Please add  Game Object  with tag 'RootMark' for the animation to work correctly.");
            }
        }

        public void Initialize(int squenseCount)
        {
            CheckGameObjectContent();
            SetVisualLayer(squenseCount);
        }

        public void CheckGameObjectContent()
        {
            if (nameVisual == null)
            {
                throw new System.Exception("TextMeshProUGUI nameVisual can't be a null");
            }

            if (nodeToolBar == null)
            {
                throw new System.Exception("Canvas nodeToolBar can't be a null");
            }

            if (nodeSpriteRenderer == null)
            {
                throw new System.Exception("SpriteRenderer nodeSpriteRenderer can't be a null");
            }

            if (markSpriteRenderer == null)
            {
                throw new System.Exception("SpriteRenderer markSpriteRenderer can't be a null");
            }
        }

        public void SetAdditionalValue(string NewValue)
        {
            AdditionalValueController.SetValue(NewValue);
        }

        public void HideAdditionalValue()
        {
            AdditionalValueController.RemoveValue();
        }


        public void MarkAsRootAnimation()
        {

            isRoot = true;
            startMarkRootScale = rootMark.transform.localScale;
            this.rootMark.transform.SetParent(null);

            this.rootMark.transform.position = this.transform.position;

            this.rootMark.transform.SetParent(this.transform);
        }

        public void RemoveRootMarkAnimation()
        {

            if (isRoot)
            {
                this.rootMark.transform.SetParent(null);
                rootMark.transform.localScale = startMarkRootScale;
                isRoot = false;
            }

        }

        public void SelectThisAnimation()
        {

            nodeSpriteRenderer.color = selectedColor;
            nameVisual.color = selectedTextColor;
            transform.localScale = selectedScale;


        }

        public void DeselectThisAnimation()
        {
            nodeSpriteRenderer.color = defaultColor;
            nameVisual.color = defaultTextColor;
            transform.localScale = defaultScale;
        }

        public void UpdateNameDisplay(string name)
        {
            if (nameVisual != null)
            {
                nameVisual.text = name;
            }
        }

        private void SetVisualLayer(int graphEdgesSequenseCount)
        {
            int baseVisualLayer = graphEdgesSequenseCount * 3;
            nodeToolBar.sortingOrder = baseVisualLayer + 2;
            markSpriteRenderer.sortingOrder = baseVisualLayer + 1;
            nodeSpriteRenderer.sortingOrder = baseVisualLayer;
        }

        public Vector3 GetCenterPosition()
        {
            return nodeToolBar.transform.position;
        }


        public void MarkThis()
        {
            isMarked = true;
            if (markAnimator != null)
            {
                markAnimator.SetBool("Mark", true);
            }
        }

        public void RemoveMark()
        {
            isMarked = false;
            if (markAnimator != null)
            {
                markAnimator.SetBool("Mark", false);
            }
        }

        public void SetColor(System.Drawing.Color color)
        {
            this.SetColor( ToUnityColor(color));
        }

        public void SetColor(Color color)
        {
            if(this.isMarked)
            {
                RemoveMark();
            }
            this.nodeSpriteRenderer.color = color;
        }

        public void SetColorTobase()
        {
            this.nodeSpriteRenderer.color = this.defaultColor;
        }

        public void HideIt()
        {
            SetColor(this.hideColor);
        }

        public void ShowIt()
        {
            SetColorTobase();
        }

        private Color ToUnityColor(System.Drawing.Color sysColor)
        {
            return new Color(sysColor.R / 255f, sysColor.G / 255f, sysColor.B / 255f, sysColor.A / 255f);
        }

    }
}

