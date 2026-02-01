using UnityEngine;

[ExecuteInEditMode]
public class SyncTiledCollider : MonoBehaviour
{
#if UNITY_EDITOR
    BoxCollider2D boxCollider;
    SpriteRenderer spriteRenderer;
    [SerializeField] float OffsetTop;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();
    }
    void Update()
    {
        if (Application.isPlaying) return;

        if(spriteRenderer == null) spriteRenderer = GetComponent<SpriteRenderer>();
        if(boxCollider == null) boxCollider = GetComponent<BoxCollider2D>();

        if (boxCollider == null || spriteRenderer == null) return;

        boxCollider.size = new Vector2(spriteRenderer.size.x, spriteRenderer.size.y - OffsetTop / 2);
        boxCollider.offset = new Vector2(0, -OffsetTop / 2);
    }

#endif
}
