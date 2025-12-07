using UnityEngine;
using System.Collections;

public class MathTeacher : MonoBehaviour, IIteractable
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
        StartMathChallenge();
    }

    private void StartMathChallenge()
    {
        if (talkingSprite && spriteRenderer)
            spriteRenderer.sprite = talkingSprite;

        // Generate 2 numbers and operator
        int a = Random.Range(1, 20);
        int b = Random.Range(1, 20);
        int correctAnswer = a + b;

        string question = $"WHAT   IS   {a}  +  {b} ?";

        MathDialogUI.Instance.Show(question, (playerAnswer) =>
        {
            if (int.TryParse(playerAnswer, out int result) && result == correctAnswer)
            {
                StartCoroutine(GiveItemRoutine());
            }
            else
            {
                MathDialogUI.Instance.ShowMessage("INCORRECT !   TRY   AGAIN   NEXT   TIME   :(");
                ResetSprite();
            }
        });
    }

    private IEnumerator GiveItemRoutine()
    {
        canGive = false;
        
        if (itemPrefab)
        {
            GameObject droppedItem = Instantiate(itemPrefab, transform.position + Vector3.down, Quaternion.identity);
            droppedItem.GetComponent<BounceEffect>()?.StartBounce();
        }

        MathDialogUI.Instance.ShowMessage("CORRECT !   HERE'S   YOUR   REWARD !");
        yield return new WaitForSeconds(1f);

        ResetSprite();

        yield return new WaitForSeconds(giveCooldown);
        canGive = true;
    }

    private void ResetSprite()
    {
        if (normalSprite && spriteRenderer)
            spriteRenderer.sprite = normalSprite;
    }
}
