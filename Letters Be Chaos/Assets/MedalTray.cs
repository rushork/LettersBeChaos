using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedalTray : MonoBehaviour
{
    private SpriteRenderer medalSprite;
    private Animator animator;

    private void Awake()
    {
        medalSprite = transform.Find("Tray").Find("MedalSlot").GetComponent <SpriteRenderer>();
        animator = transform.Find("Tray").GetComponent<Animator>();
    }

    public void OpenTray(Sprite medalToUnlock)
    {
        medalSprite.sprite = medalToUnlock;
        animator.SetTrigger("OpenTray");
    }

    public void CloseTray()
    {
        animator.SetTrigger("CloseTray");
    }
}
