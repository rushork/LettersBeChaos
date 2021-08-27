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
    [TextArea(5, 10)]
    public string hint;

    [Space(1)]
    [Header("Images")]
    public Sprite unlockedSprite;
    public Sprite lockedSprite;

    [Space(1)]
    private GameObject medalTextBg;
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

        medalTextBg = medalText.transform.parent.gameObject;
        medalTextBg.gameObject.SetActive(false);


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
                if(hint == "")
                {
                    medalText.SetText("Play more to unlock this medal");
                }
                else
                {
                    medalText.SetText(hint);
                }
                
            }
           
        }
        

    }


    public void OnPointerExit(PointerEventData eventData)
    {

        anim.SetTrigger("Shrink");
        medalTextBg.gameObject.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        medalTextBg.gameObject.SetActive(true);
        anim.SetTrigger("Grow");
        AudioManager.Instance.Play("Tick");
    }
}
