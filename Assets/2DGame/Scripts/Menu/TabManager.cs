using UnityEngine;
using UnityEngine.UI;

public class TabManager : MonoBehaviour
{
    public Image[] tabImages;
    public GameObject[] pages;

    private int currentIndex;

    void Start()
    {
        ActivateTab(0);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
            ActivateTab((currentIndex + 1) % tabImages.Length);
    }

    public void ActivateTab(int index)
    {
        currentIndex = index;
        for (int i = 0; i < tabImages.Length; i++)
        {
            tabImages[i].color = i == index ? Color.white : Color.gray;
            pages[i].SetActive(i == index);
        }
    }
}
