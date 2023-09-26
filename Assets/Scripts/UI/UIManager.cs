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

    [Header("UI")]
    [SerializeField]
    private Image[] hpIcons;

    [Header("FX")]
    [SerializeField]
    private GameObject playerSpawnFX;

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

    public void DamageUI(int index)
    {
        GameObject hpIcon = hpIcons[index].gameObject;
        hpIcon.GetComponent<Animator>().enabled = true;
    }

    public void RespawnFX(Vector2 playerPos)
    {
        Vector2 offset = playerSpawnFX.GetComponent<Offset>().offsetPos;
        playerSpawnFX.transform.position = offset + playerPos;
        playerSpawnFX.SetActive(false);
        playerSpawnFX.SetActive(true);
    }
}