using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class BookOfCluesManager : MonoBehaviour
{
    [Header("Referencias")]
    public GameObject pagesContainer;
    public Button buttonNext;
    public Button buttonPrevious;

    [Header("Configuración")]
    public List<GameObject> pageSets = new List<GameObject>();

    private int currentPageIndex = -1;
    private int maxUnlockedIndex = -1;
    private List<int> unlockedIndices = new List<int>();

    void Start()
    {
        buttonNext.onClick.AddListener(NextPage);
        buttonPrevious.onClick.AddListener(PreviousPage);

        HideAllPageSets();
        UpdateButtons();
    }

    public void UnlockPageSet(int index)
    {
        if (index >= 0 && index < pageSets.Count)
        {
            if (!unlockedIndices.Contains(index))
            {
                unlockedIndices.Add(index);
                unlockedIndices.Sort();

                if (index > maxUnlockedIndex)
                    maxUnlockedIndex = index;
            }
        }
    }

    public void OpenBook()
    {
        if (unlockedIndices.Count > 0)
        {
            ShowPageSet(unlockedIndices[0]); // Mostrar el set desbloqueado más bajo
        }
        else
        {
            HideAllPageSets(); // Nada desbloqueado vacío
            currentPageIndex = -1;
            UpdateButtons();
        }
    }

    private void ShowPageSet(int index)
    {
        if (index < 0 || index >= pageSets.Count) return;

        HideAllPageSets();
        pageSets[index].SetActive(true);
        currentPageIndex = index;
        UpdateButtons();
    }

    private void HideAllPageSets()
    {
        foreach (GameObject pageSet in pageSets)
        {
            pageSet.SetActive(false);
        }
    }

    public void NextPage()
    {
        int nextIndex = unlockedIndices.Find(i => i > currentPageIndex);
        if (nextIndex != -1)
        {
            ShowPageSet(nextIndex);
        }
    }

    public void PreviousPage()
    {
        for (int i = unlockedIndices.Count - 1; i >= 0; i--)
        {
            if (unlockedIndices[i] < currentPageIndex)
            {
                ShowPageSet(unlockedIndices[i]);
                break;
            }
        }
    }

    private void UpdateButtons()
    {
        bool hasPage = currentPageIndex >= 0;

        bool canGoNext = false;
        bool canGoPrevious = false;

        foreach (int index in unlockedIndices)
        {
            if (index > currentPageIndex) canGoNext = true;
            if (index < currentPageIndex) canGoPrevious = true;
        }

        buttonNext.interactable = hasPage && canGoNext;
        buttonPrevious.interactable = hasPage && canGoPrevious;
    }

}
