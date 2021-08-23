using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StampingArm : MonoBehaviour
{

    private GameObject art;
    private Vector2 originalPos;
    private Vector2 mouseTarget;
    public Transform target;

    [Header("Coloured overlay")]
    [SerializeField] private SpriteRenderer overlay;
    private List<Color32> colourModes;
    private Color32 currentSelectedStampColor;

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
        
        currentSelectedStampColor = colourModes[0];
        overlay.color = currentSelectedStampColor;
    }

    // Update is called once per frame
    void Update()
    {
        if (armStatus == ArmStatus.idle)
        {
           
            Vector2 mouseY = new Vector2(target.position.x, transform.position.y);
            transform.position = Vector2.MoveTowards(transform.position, mouseY, 10 * Time.deltaTime);
        }

        if (Input.GetMouseButtonDown(0) && armStatus == ArmStatus.idle)
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
                currentSelectedStampColor = colourModes[colourModes.IndexOf(currentSelectedStampColor) + 1];
            }
            catch
            {
                currentSelectedStampColor = colourModes[0];
            }

            overlay.color = currentSelectedStampColor;
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
