using UnityEngine;
using System.Collections;

public class Teacher : MonoBehaviour, IIteractable
{
    public GameObject itemPrefab;
    public float giveCooldown = 1f;
    private bool canGive = true;
    public Sprite normalSprite;
    public Sprite talkingSprite;

    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public bool CanInteract()
    {
        return canGive;
    }

    public void Interact()
    {
        if (!CanInteract()) return;
        StartCoroutine(GiveItemRoutine());
    }

    private IEnumerator GiveItemRoutine()
    {
        canGive = false;
        if (talkingSprite && spriteRenderer)
        {
            spriteRenderer.sprite = talkingSprite;
        }
            
        // Spawn the item
        if (itemPrefab)
        {
            GameObject droppedItem = Instantiate(itemPrefab, transform.position + Vector3.down, Quaternion.identity);
            droppedItem.GetComponent<BounceEffect>()?.StartBounce();
        }

        yield return new WaitForSeconds(1f);

        if (normalSprite && spriteRenderer)
        {
            spriteRenderer.sprite = normalSprite;
        }

        yield return new WaitForSeconds(giveCooldown - 1f);

        canGive = true;
    }
}
