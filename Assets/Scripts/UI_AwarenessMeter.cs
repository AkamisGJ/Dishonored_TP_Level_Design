using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_AwarenessMeter : MonoBehaviour {

    public AudioClip SFX_Awareness_White;
    public AudioClip SFX_Awareness_Red;
    public GameObject SFX_AudioSource;
    [HideInInspector]
    public CanvasGroup canvasGroup;
    AI_Behaviour AI_Behaviour;
    Image UI_AwarenessMeter_White;
    Image UI_AwarenessMeter_Red;
    Transform camPlayer;
    AI_View AI_MainView;

    float timerAwareness = 0;
    int awarenessSound = 0;
    float delayAwareness = 0;
    float duractionIAChasePlayer;


    // Use this for initialization
    void Start ()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0;
        AI_Behaviour = transform.root.GetComponent<AI_Behaviour>();
        UI_AwarenessMeter_White = transform.Find("UI_AwarenessMeter/UI_AwarenessMeter_White").GetComponent<Image>();
        UI_AwarenessMeter_Red = transform.Find("UI_AwarenessMeter/UI_AwarenessMeter_Red").GetComponent<Image>();

        UI_AwarenessMeter_White.fillAmount = AI_Behaviour.awarenessMeter_White;
        UI_AwarenessMeter_Red.fillAmount = AI_Behaviour.awarenessMeter_Red;

        camPlayer = GameObject.Find("MainCamera").GetComponent<Transform>();
        AI_MainView = transform.root.Find("mixamorig:Hips/mixamorig:Spine/mixamorig:Spine1/mixamorig:Spine2/mixamorig:Neck/mixamorig:Head/MainView").GetComponent<AI_View>();

        duractionIAChasePlayer = AI_Behaviour.duractionIAChasePlayer;
    }
	




	// Update is called once per frame
	void Update ()
    {
        if (!AI_Behaviour.isDead && !AI_Behaviour.isUnconscious && !AI_Behaviour.isKilling && !AI_Behaviour.isChoking && !AI_Behaviour.knockoutByChoke)
        {
            DisplayAwarenessMeter();
        }

        else

        {
            canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, 0, Time.deltaTime * 4);
        }
	}




    void DisplayAwarenessMeter()
    {
        if (AI_Behaviour.isSeeingPlayer)
        {
            if (Vector3.Angle((AI_MainView.transform.position - camPlayer.position), camPlayer.forward) < 90)
            {
                canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, 1, Time.deltaTime * 4);
            }

            else

            {
                canvasGroup.alpha = 0;
            }

            timerAwareness = 0;

            //Distance to player affects awareness
            float multiByDistance;
            multiByDistance = (1 + 0.05f * 3) - (Mathf.Max(3.0f, AI_Behaviour.distanceToPlayer) * 0.045f);

            //Angle to player affect awareness
            float multiByAngle;
            multiByAngle = 1 - (AI_Behaviour.angleToPlayer / 180);

            delayAwareness += Time.deltaTime;

            if (delayAwareness >= 0.25f)
            {
                if (AI_Behaviour.awarenessMeter_White < 1)
                {
                    AI_Behaviour.awarenessMeter_White = Mathf.Clamp01(AI_Behaviour.awarenessMeter_White + (Time.deltaTime * 0.8f * multiByDistance * multiByAngle));

                    //Player detected faster if less than 3 meters and in 30° range
                    if (AI_Behaviour.distanceToPlayer <= 3 && awarenessSound == 0 && AI_Behaviour.angleToPlayer > 30) AI_Behaviour.awarenessMeter_White = 0.5f;

                    UI_AwarenessMeter_White.fillAmount = AI_Behaviour.awarenessMeter_White;

                    if (AI_Behaviour.awarenessMeter_White >= 0.5f && awarenessSound == 0)
                    {
                        GameObject MySFX_AudioSource = Instantiate(SFX_AudioSource, transform.position, Quaternion.identity);
                        MySFX_AudioSource.GetComponent<AudioSource>().clip = SFX_Awareness_White;
                        MySFX_AudioSource.GetComponent<AudioSource>().spatialBlend = 0;
                        MySFX_AudioSource.GetComponent<AudioSource>().Play();
                        awarenessSound = 1;
                    }
                }

                else

                if (AI_Behaviour.awarenessMeter_Red < 1)
                {
                    AI_Behaviour.awarenessMeter_Red = Mathf.Clamp01(AI_Behaviour.awarenessMeter_Red + (Time.deltaTime * 2f * multiByDistance * multiByAngle));

                    UI_AwarenessMeter_Red.fillAmount = AI_Behaviour.awarenessMeter_Red;

                    if (awarenessSound == 1)
                    {
                        GameObject MySFX_AudioSource = Instantiate(SFX_AudioSource, transform.position, Quaternion.identity);
                        MySFX_AudioSource.GetComponent<AudioSource>().clip = SFX_Awareness_Red;
                        MySFX_AudioSource.GetComponent<AudioSource>().spatialBlend = 0;
                        MySFX_AudioSource.GetComponent<AudioSource>().Play();
                        awarenessSound = 2;
                    }
                }
            }
        }

        else

        if (AI_Behaviour.awarenessMeter_White != 0)
        {
            delayAwareness = 0;

            RaycastHit hitPlayer;

            if (Physics.Linecast(AI_MainView.transform.position, camPlayer.position, out hitPlayer, AI_MainView.layerViewPlayer))
            {
                //See obstacle between player and view
                if (hitPlayer.transform.gameObject.layer != 11)
                {
                    if(timerAwareness > 1.5f) canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, 0, Time.deltaTime);
                }

                else

                if (Vector3.Angle((AI_MainView.transform.position - camPlayer.position), camPlayer.forward) < 90)
                {
                    canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, 1, Time.deltaTime * 4);
                }

                else

                {
                    canvasGroup.alpha = 0;
                }
            }

            timerAwareness += Time.deltaTime;

            if (timerAwareness >= duractionIAChasePlayer)
            {
                if (AI_Behaviour.awarenessMeter_Red > 0)
                {
                    AI_Behaviour.awarenessMeter_Red = Mathf.Clamp01(AI_Behaviour.awarenessMeter_Red - Time.deltaTime * 0.5f);
                    UI_AwarenessMeter_Red.fillAmount = AI_Behaviour.awarenessMeter_Red;
                }

                else

                if (AI_Behaviour.awarenessMeter_White > 0)
                {
                    AI_Behaviour.awarenessMeter_White = Mathf.Clamp01(AI_Behaviour.awarenessMeter_White - Time.deltaTime * 0.5f);
                    UI_AwarenessMeter_White.fillAmount = AI_Behaviour.awarenessMeter_White;
                }
            }
        }

        else

        {
            canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, 0, Time.deltaTime);
            delayAwareness = 0;
            awarenessSound = 0;
        }
    }
}
