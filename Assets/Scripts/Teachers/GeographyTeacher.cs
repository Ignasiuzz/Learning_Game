using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GeographyTeacher : MonoBehaviour, IIteractable
{
    public GameObject itemPrefab;

    public float giveCooldown = 1f;
    private bool canGive = true;

    public Sprite normalSprite;
    public Sprite talkingSprite;
    private SpriteRenderer spriteRenderer;

    [System.Serializable]
    public struct CountryData
    {
        public string country;
        public string capital;
    }

    public CountryData[] data;
    private List<string> allCapitals = new List<string>();

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        foreach (var entry in data)
            allCapitals.Add(entry.capital);
    }

    public bool CanInteract() => canGive;

    public void Interact()
    {
        if (!canGive) return;
        StartQuestion();
    }

    private void StartQuestion()
    {
        if (talkingSprite) spriteRenderer.sprite = talkingSprite;

        // Pick random country
        int index = Random.Range(0, data.Length);
        var question = data[index];

        // Generate 3 randomized choices
        var options = GenerateOptions(question.capital);
        string[] optionTexts = options.options;
        int correctIndex = options.correctIndex;

        ChoiceDialogUI.Instance.Show(
            $"WHAT   IS   THE   CAPITAL   OF   {question.country} ?",
            optionTexts[0],
            optionTexts[1],
            optionTexts[2],
            (chosen) =>
            {
                if (chosen == correctIndex)
                    StartCoroutine(GiveRewardRoutine());
                else
                {
                    ChoiceDialogUI.Instance.ShowResult("WRONG !   TRY   AGAIN   NEXT   TIME   :(");
                    ResetSprite();
                }
            });
    }

    private (string[] options, int correctIndex) GenerateOptions(string correctCapital)
    {
        List<string> pool = new List<string>(allCapitals);

        pool.Remove(correctCapital);

        string wrong1 = pool[Random.Range(0, pool.Count)];
        pool.Remove(wrong1);
        string wrong2 = pool[Random.Range(0, pool.Count)];

        List<string> options = new List<string> { correctCapital, wrong1, wrong2 };

        for (int i = 0; i < options.Count; i++)
        {
            int r = Random.Range(i, options.Count);
            (options[i], options[r]) = (options[r], options[i]);
        }

        int correctIndex = options.IndexOf(correctCapital);

        return (options.ToArray(), correctIndex);
    }

    private IEnumerator GiveRewardRoutine()
    {
        canGive = false;

        ChoiceDialogUI.Instance.ShowResult("CORRECT !   HERE'S   OUR   REWARD !");

        if (itemPrefab)
            Instantiate(itemPrefab, transform.position + Vector3.down, Quaternion.identity);

        yield return new WaitForSeconds(1f);
        ResetSprite();
        yield return new WaitForSeconds(giveCooldown);

        canGive = true;
    }

    private void ResetSprite()
    {
        if (normalSprite) spriteRenderer.sprite = normalSprite;
    }
}
