using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Medal : MonoBehaviour
{

    [Header("Info")]
    public string medalName;
    public int unlocked;
    [TextArea(5, 10)]
    public string info;

    [Space(1)]
    [Header("Images")]
    public Sprite unlockedSprite;
    public Sprite lockedSprite;

    [Space(1)]
    public TextMeshProUGUI medalText;

    void Start() {
        unlocked = PlayerPrefs.GetInt(medalName);

        if (unlocked == 1) {
            GetComponent<Image>().sprite = unlockedSprite;
        } else {
            GetComponent<Image>().sprite = lockedSprite;
        }
        GetComponent<Image>().SetNativeSize();
    }

    public void onMouseHover() {
        if (unlocked == 1) {
            medalText.SetText(info);
        } else {
            medalText.SetText("???");
        }
    }

}
