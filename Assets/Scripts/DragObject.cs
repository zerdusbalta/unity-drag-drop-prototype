using UnityEngine;
using TMPro;

public class DragObject : MonoBehaviour
{
    private bool isDragging;
    private Vector3 offset;
    private Vector3 startPosition;
    private SpriteRenderer sr;
    private DropZone dropZone;

    private bool isSnapping;
    private Vector3 snapTarget;
    public float snapSpeed = 8f;

    [SerializeField] private TextMeshProUGUI successText;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        dropZone = FindObjectOfType<DropZone>();
        startPosition = transform.position;

        if (successText != null)
        {
            successText.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (isSnapping)
        {
            transform.position = Vector3.Lerp(transform.position, snapTarget, snapSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, snapTarget) < 0.01f)
            {
                transform.position = snapTarget;
                isSnapping = false;
            }
        }
        Vector3 mouseWorld = GetMouseWorldPosition();

        if (Input.GetMouseButtonDown(0))
        {
            Collider2D hit = Physics2D.OverlapPoint(mouseWorld);

            if (hit != null && hit.gameObject == gameObject)
            {
                isSnapping = false;
                isDragging = true;
                offset = transform.position - mouseWorld;
                sr.color = Color.green;

                if (successText != null)
                {
                    successText.gameObject.SetActive(false);
                }
            }
        }

        if (Input.GetMouseButton(0) && isDragging)
        {
            transform.position = mouseWorld + offset;
        }

        if (Input.GetMouseButtonUp(0) && isDragging)
        {
            isDragging = false;
            sr.color = Color.white;

            if (dropZone != null && dropZone.IsMoreThanHalfInside(GetComponent<Collider2D>()))
            {
                snapTarget = dropZone.GetSnapPosition();
                isSnapping = true; sr.color = Color.yellow;

                if (successText != null)
                {
                    successText.gameObject.SetActive(true);
                }
            }
            else
            {
                transform.position = startPosition;

                if (successText != null)
                {
                    successText.gameObject.SetActive(false);
                }
            }
        }
    }

    private Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = 10f;
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        worldPosition.z = 0f;
        return worldPosition;
    }
}