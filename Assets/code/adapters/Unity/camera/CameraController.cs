using UnityEngine;

namespace GraphMaster.UnityAdapter
{
    /// <summary>
    /// Контроллер камеры для 2D сцены.
    /// Обеспечивает перемещение камеры правой кнопкой мыши и масштабирование колесиком.
    /// Ограничивает камеру заданными границами.
    /// </summary>
    public class CameraController : MonoBehaviour
    {
        [Header("Камера")]
        [SerializeField] private UnityEngine.Camera targetCamera;

        [Header("Границы")]
        [SerializeField] private BoxCollider2D boundsCollider;

        [Header("Настройки перемещения")]
        [SerializeField] private float dragSpeed = 1f;
        [SerializeField] private int dragMouseButton = 1;

        [Header("Настройки масштабирования")]
        [SerializeField] private float zoomSpeed = 1f;
        [SerializeField] private float minZoom = 1f;
        [SerializeField] private float maxZoom = 20f;

        private Vector3 dragOrigin;
        private bool isDragging = false;
        private Bounds currentBounds;
        private bool hasBounds = false;

        private void Awake()
        {
            if (targetCamera == null)
            {
                targetCamera = UnityEngine.Camera.main;
            }

            UpdateBounds();
        }

        private void Update()
        {
            HandleDrag();
            HandleZoom();
        }

        /// <summary>
        /// Обновляет границы на основе коллайдера
        /// </summary>
        public void UpdateBounds()
        {
            if (boundsCollider != null)
            {
                currentBounds = boundsCollider.bounds;
                hasBounds = currentBounds.size.x > 0 && currentBounds.size.y > 0;
            }
            else
            {
                hasBounds = false;
                Debug.LogWarning("CameraController: boundsCollider не назначен. Ограничения камеры отключены.");
            }

            if (hasBounds)
            {
                ClampCameraPosition();
                ClampCameraZoom();
            }
        }

        private void HandleDrag()
        {
            if (Input.GetMouseButtonDown(dragMouseButton))
            {
                isDragging = true;
                dragOrigin = GetMouseWorldPosition();
            }

            if (Input.GetMouseButtonUp(dragMouseButton))
            {
                isDragging = false;
            }

            if (isDragging && Input.GetMouseButton(dragMouseButton))
            {
                Vector3 currentMousePosition = GetMouseWorldPosition();
                Vector3 difference = dragOrigin - currentMousePosition;

                Vector3 newPosition = targetCamera.transform.position + difference * dragSpeed;
                newPosition.z = targetCamera.transform.position.z;

                targetCamera.transform.position = newPosition;
                
                if (hasBounds)
                {
                    ClampCameraPosition();
                }

                dragOrigin = GetMouseWorldPosition();
            }
        }

        private void HandleZoom()
        {
            float scrollDelta = Input.mouseScrollDelta.y;

            if (Mathf.Abs(scrollDelta) > 0.01f)
            {
                Vector3 mouseWorldPosBefore = GetMouseWorldPosition();

                float newSize = targetCamera.orthographicSize - scrollDelta * zoomSpeed;
                newSize = Mathf.Clamp(newSize, minZoom, maxZoom);

                if (hasBounds)
                {
                    float maxAllowedSize = CalculateMaxZoomForBounds();
                    newSize = Mathf.Min(newSize, maxAllowedSize);
                }

                targetCamera.orthographicSize = newSize;

                Vector3 mouseWorldPosAfter = GetMouseWorldPosition();
                Vector3 offset = mouseWorldPosBefore - mouseWorldPosAfter;
                targetCamera.transform.position += offset;

                if (hasBounds)
                {
                    ClampCameraPosition();
                }
            }
        }

        private Vector3 GetMouseWorldPosition()
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = -targetCamera.transform.position.z;
            return targetCamera.ScreenToWorldPoint(mousePos);
        }

        /// <summary>
        /// Ограничивает позицию камеры заданными границами
        /// </summary>
        private void ClampCameraPosition()
        {
            if (targetCamera == null || !hasBounds) return;

            float cameraHeight = targetCamera.orthographicSize;
            float cameraWidth = cameraHeight * targetCamera.aspect;

            Vector3 cameraPos = targetCamera.transform.position;

            // Вычисляем допустимые границы для центра камеры
            float minX = currentBounds.min.x + cameraWidth;
            float maxX = currentBounds.max.x - cameraWidth;
            float minY = currentBounds.min.y + cameraHeight;
            float maxY = currentBounds.max.y - cameraHeight;

            // Если камера больше границ по X, центрируем
            if (minX > maxX)
            {
                cameraPos.x = currentBounds.center.x;
            }
            else
            {
                cameraPos.x = Mathf.Clamp(cameraPos.x, minX, maxX);
            }

            // Если камера больше границ по Y, центрируем
            if (minY > maxY)
            {
                cameraPos.y = currentBounds.center.y;
            }
            else
            {
                cameraPos.y = Mathf.Clamp(cameraPos.y, minY, maxY);
            }

            targetCamera.transform.position = cameraPos;
        }

        /// <summary>
        /// Ограничивает масштаб камеры, чтобы она не выходила за границы
        /// </summary>
        private void ClampCameraZoom()
        {
            if (targetCamera == null || !hasBounds) return;

            float maxAllowedSize = CalculateMaxZoomForBounds();

            if (targetCamera.orthographicSize > maxAllowedSize)
            {
                targetCamera.orthographicSize = maxAllowedSize;
            }
        }

        /// <summary>
        /// Вычисляет максимальный размер камеры, который вписывается в границы
        /// </summary>
        private float CalculateMaxZoomForBounds()
        {
            if (!hasBounds) return maxZoom;

            float boundsHeight = currentBounds.size.y;
            float boundsWidth = currentBounds.size.x;

            // Максимальный orthographicSize по высоте
            float maxSizeByHeight = boundsHeight / 2f;

            // Максимальный orthographicSize по ширине (учитывая aspect ratio)
            float maxSizeByWidth = boundsWidth / (2f * targetCamera.aspect);

            // Берём меньшее значение, чтобы камера вписывалась полностью
            float maxSize = Mathf.Min(maxSizeByHeight, maxSizeByWidth);

            // Ограничиваем заданным максимумом
            return Mathf.Min(maxSize, maxZoom);
        }

        /// <summary>
        /// Устанавливает границы камеры программно
        /// </summary>
        public void SetBounds(Bounds bounds)
        {
            currentBounds = bounds;
            hasBounds = bounds.size.x > 0 && bounds.size.y > 0;
            
            if (hasBounds)
            {
                ClampCameraPosition();
                ClampCameraZoom();
            }
        }

        /// <summary>
        /// Устанавливает границы камеры из коллайдера
        /// </summary>
        public void SetBoundsFromCollider(BoxCollider2D collider)
        {
            boundsCollider = collider;
            UpdateBounds();
        }

        /// <summary>
        /// Центрирует камеру на границах
        /// </summary>
        public void CenterCamera()
        {
            if (targetCamera == null || !hasBounds) return;

            Vector3 center = currentBounds.center;
            center.z = targetCamera.transform.position.z;
            targetCamera.transform.position = center;

            ClampCameraPosition();
        }

        /// <summary>
        /// Подгоняет камеру, чтобы показать все границы
        /// </summary>
        public void FitToBounds()
        {
            if (targetCamera == null || !hasBounds) return;

            float maxSize = CalculateMaxZoomForBounds();
            targetCamera.orthographicSize = maxSize;
            CenterCamera();
        }

        private void OnValidate()
        {
            // Обновляем границы при изменении в инспекторе
            if (Application.isPlaying && boundsCollider != null)
            {
                UpdateBounds();
            }
        }
    }
}
