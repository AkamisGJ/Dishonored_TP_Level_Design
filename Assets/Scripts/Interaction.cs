using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;

public class Interaction : MonoBehaviour {

    public bool isInteracted;
    public Image cursor;
    public Sprite basic_Sprite;
    public Sprite interact_Sprite;
    public Text textCursor;
    public Text textDescription;
    public Text textCursorOption;
    public Image barFeedback_Background;
    public Image loadingBarFeedbackInput;

    CarryAndThrow carryAndThrow;
    float timerLoadingInput = 0;
    RigidbodyFirstPersonController globalState;
    bool canInteract = true;
    AI_Behaviour AI_Behaviour;
    bool previousCrouch;
    GameObject UI_ChokeAndKill;
    GameObject blade;
    Air_Assassination air_Assassination;
    AI_Health AI_Health;


    // Use this for initialization
    void Start ()
    {
        carryAndThrow = GetComponent<CarryAndThrow>();
        globalState = GameObject.Find("RigidBodyFPSController").GetComponent<RigidbodyFirstPersonController>();
        UI_ChokeAndKill = GameObject.Find("UI_ChokeAndKill");
        blade = GameObject.Find("Blade");
        air_Assassination = transform.root.GetComponent<Air_Assassination>();
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hitInteract;

        if(globalState.playerKeyholePeek)
        {
            canInteract = false;
        }


        if (!air_Assassination.isAssassinating && canInteract && Physics.Raycast(transform.position, transform.forward, out hitInteract, 3))
        {
            if (hitInteract.transform.GetComponent<InteractableObject>() && hitInteract.transform.GetComponent<InteractableObject>().enabled)
            {
                RectTransform myRectTransform = cursor.rectTransform;
                myRectTransform.sizeDelta = new Vector2(16, 16);
                cursor.color = Color.white;
                cursor.sprite = interact_Sprite;

                if(!carryAndThrow.isCarrying || (carryAndThrow.isCarrying && hitInteract.transform.tag != "Carryable"))
                {
                    isInteracted = true;
                    textCursor.text = hitInteract.transform.GetComponent<InteractableObject>().stringInteraction;
                    textDescription.text = hitInteract.transform.GetComponent<InteractableObject>().stringDescription;
                    textCursorOption.text = hitInteract.transform.GetComponent<InteractableObject>().stringInteractionOption;
                    barFeedback_Background.enabled = (textCursorOption.text == "" ? false : true);
                    loadingBarFeedbackInput.enabled = (textCursorOption.text == "" ? false : true);

                    if (textCursorOption.text == "")
                    {
                        if (Input.GetButtonDown("Interaction"))
                        {
                            hitInteract.transform.GetComponent<InteractableObject>().PlayInteraction();
                        }
                    }

                    else

                    {
                        if (Input.GetButton("Interaction"))
                        {
                            timerLoadingInput += Time.deltaTime * 2;
                            loadingBarFeedbackInput.fillAmount = timerLoadingInput;

                            if(timerLoadingInput >= 1)
                            {
                                hitInteract.transform.GetComponent<InteractableObject>().PlayInteractionOption();
                                timerLoadingInput = 0;
                            }
                        }

                        else

                        {
                            timerLoadingInput = 0;
                            loadingBarFeedbackInput.fillAmount = 0.03f;
                        }

                        if (Input.GetButtonUp("Interaction") && timerLoadingInput < 1)
                        {
                            hitInteract.transform.GetComponent<InteractableObject>().PlayInteraction();
                        }
                    }
                }
                
            }

            else

            {
                RectTransform myRectTransform = cursor.rectTransform;
                myRectTransform.sizeDelta = new Vector2(2, 2);
                cursor.color = Color.white;
                cursor.sprite = basic_Sprite;
                textCursor.text = "";
                textDescription.text = "";
                textCursorOption.text = "";
                barFeedback_Background.enabled = false;
                loadingBarFeedbackInput.enabled = false;
                timerLoadingInput = 0;
                loadingBarFeedbackInput.fillAmount = 0.03f;
                isInteracted = false;
            }

            if (hitInteract.transform.gameObject.layer == 9 && hitInteract.distance < 2) //IA Enemy
            {
                cursor.color = Color.red;
                AI_Behaviour = hitInteract.transform.root.GetComponent<AI_Behaviour>();

                if (Vector3.Angle(transform.root.forward, hitInteract.transform.root.forward) < 60 && !AI_Behaviour.knockoutByChoke && AI_Behaviour.awarenessMeter_Red == 0 && !carryAndThrow.isCarrying && globalState.m_IsGrounded)
                {
                    UI_ChokeAndKill.SetActive(true);

                    if (Input.GetButtonDown("Choke"))
                    {
                        AI_Behaviour = hitInteract.transform.root.GetComponent<AI_Behaviour>();
                        AI_Behaviour.Choke(true);
                        previousCrouch = globalState.movementSettings.isCrouched;
                        globalState.movementSettings.isCrouched = false;
                        blade.SetActive(false);
                    }

                    if(Input.GetButtonDown("Attack") || Input.GetAxis("Attack") > 0.2f)
                    {
                        AI_Health = hitInteract.transform.root.GetComponent<AI_Health>();
                        AI_Health.hp = 10;
                        AI_Behaviour = hitInteract.transform.root.GetComponent<AI_Behaviour>();
                        AI_Behaviour.Kill();
                    }
                }

                else

                if(!air_Assassination.canAssassinate && (!Input.GetButton("Choke") || (AI_Behaviour != null && (AI_Behaviour.isUnconscious || AI_Behaviour.knockoutByChoke))))
                {
                    UI_ChokeAndKill.SetActive(false);
                }
            }

            else

            {
                if(!air_Assassination.canAssassinate && (!Input.GetButton("Choke") || (AI_Behaviour != null && (AI_Behaviour.isUnconscious || AI_Behaviour.knockoutByChoke)))) UI_ChokeAndKill.SetActive(false);
            }
        }

        else

        {
            RectTransform myRectTransform = cursor.rectTransform;
            myRectTransform.sizeDelta = new Vector2(2, 2);
            cursor.color = Color.white;
            cursor.sprite = basic_Sprite;
            textCursor.text = "";
            textDescription.text = "";
            textCursorOption.text = "";
            barFeedback_Background.enabled = false;
            loadingBarFeedbackInput.enabled = false;
            timerLoadingInput = 0;
            loadingBarFeedbackInput.fillAmount = 0.03f;
            isInteracted = false;
            if (!air_Assassination.canAssassinate && (!Input.GetButton("Choke") || (AI_Behaviour != null && (AI_Behaviour.isUnconscious || AI_Behaviour.knockoutByChoke)))) UI_ChokeAndKill.SetActive(false);
        }

        if (!globalState.playerKeyholePeek && Input.GetButtonUp("Interaction"))
        {
            canInteract = true;
        }

        if (Input.GetButtonUp("Choke"))
        {
            if (AI_Behaviour != null && !AI_Behaviour.knockoutByChoke)
            {
                AI_Behaviour.Choke(false);
                AI_Behaviour = null;
            }
            globalState.movementSettings.isCrouched = previousCrouch;
            previousCrouch = globalState.movementSettings.isCrouched;
            if(!carryAndThrow.isCarrying) blade.SetActive(true);
        }
    }
}
