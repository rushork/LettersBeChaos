using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.EventSystems;

public class Medal : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    [Header("Info")]
    public string medalName;
    public int unlocked;
    public bool hiddenUntilUnlocked;
    [TextArea(5, 10)]
    public string info;

    [Space(1)]
    [Header("Images")]
    public Sprite unlockedSprite;
    public Sprite lockedSprite;

    [Space(1)]
    public TextMeshProUGUI medalText;
    private Animator anim;

    void Start() {
        unlocked = PlayerPrefs.GetInt(medalName);

        if (unlocked == 1) {
            GetComponent<Image>().sprite = unlockedSprite;
        } else {
            GetComponent<Image>().sprite = lockedSprite;
        }
        GetComponent<Image>().SetNativeSize();

        anim = GetComponent<Animator>();

        medalText.gameObject.SetActive(false);
    }

    public void onMouseHover() {

        if (!hiddenUntilUnlocked)
        {
            medalText.SetText(info);
        }
        else
        {
            if (unlocked == 1)
            {
                medalText.SetText(info);
            }
            else
            {
                medalText.SetText("Unknown, Play more to find out!");
            }
           
        }
        

    }


    public void OnPointerExit(PointerEventData eventData)
    {

        anim.SetTrigger("Shrink");
        medalText.gameObject.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        medalText.gameObject.SetActive(true);
        anim.SetTrigger("Grow");
        AudioManager.Instance.Play("Tick");
    }
}
