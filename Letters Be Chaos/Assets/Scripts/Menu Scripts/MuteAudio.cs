using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MuteAudio : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{

    public AudioSource sound;
    float originalVolume;
    private bool isMuted;
    private Image thisImage;

    [Header("Sprites")]
    public Sprite muted;
    public Sprite unmuted;
    public Sprite closed;

    private void Awake()
    {
        originalVolume = sound.volume;
        thisImage = GetComponent<Image>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (isMuted)
        {
            sound.volume = originalVolume;
            thisImage.sprite = unmuted;
            isMuted = false;
        }
        else
        {
            thisImage.sprite = muted;
            isMuted = true;
            sound.volume = 0;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        AudioManager.Instance.Play("Tick");
        if (isMuted)
        {
            thisImage.sprite = muted;
        }
        else
        {
            thisImage.sprite = unmuted;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        thisImage.sprite = closed;
    }

}
