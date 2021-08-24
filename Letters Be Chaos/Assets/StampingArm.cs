using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StampingArm : MonoBehaviour
{

    [Header("Table Highlighting Zones")]
    public GameObject highlightRed;
    public GameObject highlightBlue;
    public GameObject highlightGreen;
    public GameObject highlightOrange;

   

    private GameObject art;
    private Vector2 originalPos;
    private Vector2 mouseTarget;
    [Space]
    public Transform target;

    [Header("Coloured overlay")]
    [SerializeField] private SpriteRenderer overlay;
    private List<Color32> colourModes;
    private List<GameObject> highlightZones;
    private Color32 currentSelectedStampColor;
    public bool isOverUI;

    public enum ArmStatus
    {
        idle,
        extending,
        retracting,
        stamping
    }
    
    private ArmStatus armStatus;

    private void Awake()
    {
        art = transform.Find("art").gameObject;
        armStatus = ArmStatus.idle;
        
    }

    private void Start()
    {
        colourModes = new List<Color32>();
        
        colourModes.Add(GameSettings.Instance.blue);
        colourModes.Add(GameSettings.Instance.green);
        colourModes.Add(GameSettings.Instance.red);
        colourModes.Add(GameSettings.Instance.delete);

        highlightZones = new List<GameObject>();
        highlightZones.Add(highlightBlue);
        highlightZones.Add(highlightGreen);
        highlightZones.Add(highlightRed);
        highlightZones.Add(highlightOrange);

        currentSelectedStampColor = colourModes[0];
        overlay.color = currentSelectedStampColor;
        SetTableHighlight(0);
    }

    // Update is called once per frame
    void Update()
    {
        if (armStatus == ArmStatus.idle)
        {
           
            Vector2 mouseY = new Vector2(target.position.x, transform.position.y);
            transform.position = Vector2.MoveTowards(transform.position, mouseY, 10 * Time.deltaTime);
        }

        if (Input.GetMouseButtonDown(0) && armStatus == ArmStatus.idle && !isOverUI)
        {

            mouseTarget = new Vector2(art.transform.position.x, target.position.y - 0.7f);
            originalPos = art.transform.position;
            armStatus = ArmStatus.extending;
        }

        if (armStatus == ArmStatus.extending)
        {
            art.transform.position = Vector2.MoveTowards(art.transform.position, mouseTarget, 11 * Time.deltaTime);

            if(Vector2.Distance(art.transform.position, mouseTarget) <= 0.2f)
            {
                mouseTarget = originalPos;
                armStatus = ArmStatus.retracting;
                
            }
        }
        else if (armStatus == ArmStatus.retracting)
        {
            art.transform.position = Vector2.MoveTowards(art.transform.position, originalPos, 11 * Time.deltaTime);

            if (Vector2.Distance(art.transform.position, originalPos) <= 0.1f)
            {
                art.transform.position = originalPos;
                armStatus = ArmStatus.idle;
            }
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            try
            {
                int index = colourModes.IndexOf(currentSelectedStampColor) + 1;
                currentSelectedStampColor = colourModes[index];
                SetTableHighlight(index);

            }
            catch
            {
                currentSelectedStampColor = colourModes[0];
                SetTableHighlight(0);
            }

            overlay.color = currentSelectedStampColor;
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            currentSelectedStampColor = colourModes[2];
            overlay.color = currentSelectedStampColor;
            SetTableHighlight(2);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            currentSelectedStampColor = colourModes[1];
            overlay.color = currentSelectedStampColor;
            SetTableHighlight(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            currentSelectedStampColor = colourModes[0];
            overlay.color = currentSelectedStampColor;
            SetTableHighlight(0);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            currentSelectedStampColor = colourModes[3];
            overlay.color = currentSelectedStampColor;
            SetTableHighlight(3);
        }

    }

    //Draw zones on the table
    private void SetTableHighlight(int value)
    {
        foreach(GameObject object_i in highlightZones)
        {
            if(object_i != highlightZones[value])
            {
                object_i.SetActive(false);
            }
            else
            {
                object_i.SetActive(true);
            }
        }
    }

    public Color32 GetColor()
    {
        return currentSelectedStampColor;
    }

    public bool IsIdle()
    {
        if(armStatus == ArmStatus.idle)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
