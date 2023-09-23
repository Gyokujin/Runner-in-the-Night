using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance = null;

    [Header("Slide")]
    [SerializeField]
    private Button slideButton;
    [SerializeField]
    private Text slideCoolText;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    public void ButtonCooldown(string buttonName, float coolTime)
    {
        switch (buttonName)
        {
            case "slide":
                StartCoroutine("SlideCool", coolTime);
                break;
        }
    }

    IEnumerator SlideCool(float cool)
    {
        slideButton.interactable = false;
        slideCoolText.gameObject.SetActive(true);
        float time = cool;

        while (time > 0)
        {
            time -= Time.deltaTime;
            slideCoolText.text = ((int)time).ToString();
            yield return null;
        }

        slideButton.interactable = true;
        slideCoolText.gameObject.SetActive(false);
    }
}