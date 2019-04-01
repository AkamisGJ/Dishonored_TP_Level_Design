using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;

public class AI_Behaviour : MonoBehaviour
{

    public bool isAvailableForScripting = true;
    public bool isDead;
    public bool isUnconscious;
    public bool isChoking;
    public bool isKilling;
    public bool knockoutByChoke;
    public bool reachingDestination = true;
    [HideInInspector]
    public int viewPlayer;
    public float awarenessMeter_White = 0;
    public float awarenessMeter_Red = 0;
    public float distanceToPlayer;
    public float angleToPlayer;
    public bool isSeeingPlayer;
    public Transform targetLookAt;
    public bool isAttacking;
    public bool readyToChasePlayer;
    public float duractionIAChasePlayer = 5;

    Material bodyMat;
    Animator anim;
    Collider playerCol;
    Transform chokedPos;
    AI_Head AI_Head;
    float chokedDuration;
    Interaction interaction;
    Image UI_ChokeLoad;
    GameObject UI_Kill;
    GameObject blade;
    [HideInInspector]
    public NavMeshAgent agent;
    UI_AwarenessMeter UI_AwarenessMeter;
    float IKValueLookAt = 0;
    RigidbodyFirstPersonController controller;
    bool bladeInHand;
    Transform playerPos;
    Transform myBlade;


    // Use this for initialization
    void Start()
    {
        bodyMat = transform.Find("Alpha_Surface").GetComponent<Renderer>().material;
        anim = GetComponent<Animator>();
        playerCol = GameObject.Find("RigidBodyFPSController").GetComponent<Collider>();
        chokedPos = GameObject.Find("ChokedPos").GetComponent<Transform>();
        AI_Head = transform.Find("mixamorig:Hips/mixamorig:Spine/mixamorig:Spine1/mixamorig:Spine2/mixamorig:Neck/mixamorig:Head").GetComponent<AI_Head>();
        interaction = GameObject.Find("RigidBodyFPSController").GetComponent<Interaction>();
        UI_ChokeLoad = GameObject.Find("UI_ChokeLoad").GetComponent<Image>();
        UI_Kill = GameObject.Find("UI_Kill");
        blade = GameObject.Find("Blade");
        agent = GetComponent<NavMeshAgent>();
        UI_AwarenessMeter = transform.Find("Canvas").GetComponent<UI_AwarenessMeter>();
        controller = GameObject.Find("RigidBodyFPSController").GetComponent<RigidbodyFirstPersonController>();
        playerPos = GameObject.Find("RigidBodyFPSController").GetComponent<Transform>();
        myBlade = transform.Find("mixamorig:Hips/mixamorig:LeftUpLeg/MyBlade").GetComponent<Transform>();
    }



    // Update is called once per frame
    void Update()
    {
        CheckState();
        if (isDead || isUnconscious)
        {
            bodyMat.color = Color.Lerp(bodyMat.color, new Color(0.4f, 0.32f, 0.32f), Time.deltaTime * 2);
        }

        if (isChoking)
        {
            agent.enabled = false;

            transform.position = Vector3.MoveTowards(transform.position, chokedPos.position, Time.deltaTime * 10);
            transform.rotation = Quaternion.Lerp(transform.rotation, chokedPos.rotation, Time.deltaTime * 10);

            chokedDuration += Time.deltaTime;
            UI_ChokeLoad.fillAmount = chokedDuration * 0.33f;

            if (chokedDuration >= 3)
            {
                isChoking = false;
                knockoutByChoke = true;
                blade.SetActive(true);
                controller.movementSettings.ChokeMultiplier = 1f;
                //interaction.AI_Behaviour = null;
            }
        }

        else

        {
            chokedDuration = 0;
        }


        if (isKilling && !isDead)
        {
            transform.position = Vector3.MoveTowards(transform.position, chokedPos.position, Time.deltaTime * 10);
            transform.rotation = Quaternion.Lerp(transform.rotation, chokedPos.rotation, Time.deltaTime * 10);
        }

        if (!isDead && !isUnconscious && !isKilling && !isChoking && !knockoutByChoke)
        {
            CheckDestination();
            CheckStateForCombat();
        }
    }




    void CheckState()
    {
        if(isDead || isUnconscious || isChoking || isKilling || knockoutByChoke || awarenessMeter_Red > 0)
        {
            isAvailableForScripting = false;
        }
    }



    public void Choke(bool startChocking)
    {
        if (startChocking)
        {
            agent.enabled = false;
            anim.CrossFade("Choked", 0.05f);
            isChoking = true;
            UI_Kill.SetActive(false);
            controller.movementSettings.ChokeMultiplier = 0.33f;

            Collider[] bodies = GetComponentsInChildren<Collider>();
            foreach (Collider col in bodies)
            {
                if(col.gameObject.layer == 9 || col.gameObject.layer == 10) Physics.IgnoreCollision(col, playerCol);
            }
        }

        else

        {
            agent.enabled = true;
            anim.CrossFade("Looking Behind", 0.05f);
            if (isChoking) controller.movementSettings.ChokeMultiplier = 1f;
            isChoking = false;
            UI_Kill.SetActive(true);

            Collider[] bodies = GetComponentsInChildren<Collider>();
            foreach (Collider col in bodies)
            {
                if (col.gameObject.layer == 9 || col.gameObject.layer == 10) Physics.IgnoreCollision(col, playerCol, false);
            }
        }
        UI_ChokeLoad.fillAmount = 0;
    }



    public void Kill()
    {
        agent.enabled = false;
        anim.Play("Idle");
        isKilling = true;
        UI_Kill.SetActive(false);
        if(isChoking) controller.movementSettings.ChokeMultiplier = 1f;
        isChoking = false;

        Collider[] bodies = GetComponentsInChildren<Collider>();
        foreach (Collider col in bodies)
        {
            if (col.gameObject.layer == 9 || col.gameObject.layer == 10) Physics.IgnoreCollision(col, playerCol);
        }
    }



    public void KnockoutByChoke()
    {
        agent.enabled = false;
        if (isChoking) controller.movementSettings.ChokeMultiplier = 1f;
        isChoking = false;
        if (!isDead) AI_Head.Knockout();
        Debug.Log("KnockoutByChoke");
        UI_ChokeLoad.fillAmount = 0;
        UI_Kill.SetActive(true);
    }



    public void AssassinationByChoke()
    {
        agent.enabled = false;
        anim.CrossFade("Air Choke", 0.05f);

        Collider[] bodies = GetComponentsInChildren<Collider>();
        foreach (Collider col in bodies)
        {
            if (col.gameObject.layer == 9 || col.gameObject.layer == 10) Physics.IgnoreCollision(col, playerCol);
        }
    }


    public void AssassinationByKill()
    {
        agent.enabled = false;
        anim.CrossFade("Air Kill", 0.05f);

        Collider[] bodies = GetComponentsInChildren<Collider>();
        foreach (Collider col in bodies)
        {
            if (col.gameObject.layer == 9 || col.gameObject.layer == 10) Physics.IgnoreCollision(col, playerCol);
        }
    }



    public void SetDestination(Transform destination, bool isRunning)
    {
        if (agent.enabled)
        {
            reachingDestination = false;
            agent.SetDestination(destination.position);
            agent.speed = isRunning ? 6f : 1.25f;
            agent.acceleration = isRunning ? 16 : 2;
            AnimatorStateInfo animStateInfo;
            animStateInfo = anim.GetNextAnimatorStateInfo(0);
            if (!animStateInfo.IsName("Base Layer.Run") && !animStateInfo.IsName("Base Layer.Standard Walk"))
            {
                anim.CrossFade(isRunning ? "Run" : "Standard Walk", 0.05f);
            }
        }
    }



    void CheckDestination()
    {
        if (agent.enabled && !reachingDestination)
        {
            ExtraRotation();
            if (agent.remainingDistance < 0.1f)
            {
                //DoStuffAtDestination();
                reachingDestination = true;
            }
        }
    }



    void DoStuffAtDestination()
    {
        anim.CrossFade("Idle", 0.05f);
    }



    void ExtraRotation()
    {
        Vector3 lookrotation = agent.steeringTarget - transform.position;
        lookrotation.y = 0;

        if(lookrotation != Vector3.zero) transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(lookrotation), (agent.speed > 3 ? 270 : 180) * Time.deltaTime);
    }




    void RotationToTarget(Vector3 orientedToTarget)
    {
        if (!agent.hasPath)
        {
            Vector3 lookrotation = orientedToTarget - transform.position;
            lookrotation.y = 0;

            if (lookrotation != Vector3.zero) transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(lookrotation), 270 * Time.deltaTime);
        }
    }



    public void ViewPlayer(int value)
    {
        viewPlayer += value;
    }


    void LookAtHead()
    {
        if (targetLookAt != null)
        {
            anim.SetLookAtWeight(IKValueLookAt, 0.35f, 1f);
            anim.SetLookAtPosition(targetLookAt.position);
        }

        if (awarenessMeter_Red > 0)
        {
            IKValueLookAt = Mathf.MoveTowards(IKValueLookAt, 1, Time.deltaTime * 2);
        }

        else

        {
            IKValueLookAt = Mathf.MoveTowards(IKValueLookAt, 0, Time.deltaTime * 2);
        }
    }


    private void OnAnimatorIK(int layerIndex)
    {
        LookAtHead();
    }




    void CheckStateForCombat()
    {
        if(awarenessMeter_Red > 0 && !bladeInHand)
        {
            SwordInHand();
            Invoke("ReadyToChasePlayer", 3f);
        }

        if(awarenessMeter_Red >= 1)
        {
            //Rotate entire body to player when not moving by Navigation
            if (!isAttacking) RotationToTarget(playerPos.position);

            if (readyToChasePlayer)
            {
                if (!agent.hasPath || (agent.hasPath && agent.remainingDistance > 2.5))
                {
                    SetDestination(playerPos, true);
                }

                else

                if (!isAttacking)
                {
                    AttackSword();
                    isAttacking = true;
                }
            }
        }
    }




    void ReadyToChasePlayer()
    {
        readyToChasePlayer = true;
    }



    void ReadyToAttack()
    {
        isAttacking = false;
    }




    void AttackSword()
    {
        agent.ResetPath();
        anim.CrossFade("AttackSword", 0.1f);
        Invoke("ReadyToAttack", 2f);
        Invoke("ReadyToChasePlayer", 4f);
        readyToChasePlayer = false;
    }



    public void SwordPositionHandOrPocket(int inHand)
    {
        if (inHand == 1)
        {
            myBlade.parent = transform.Find("mixamorig:Hips/mixamorig:Spine/mixamorig:Spine1/mixamorig:Spine2/mixamorig:RightShoulder/mixamorig:RightArm/mixamorig:RightForeArm/mixamorig:RightHand");
            myBlade.localPosition = new Vector3(0.1f, -0.03f, 0);
            myBlade.localRotation = Quaternion.Euler(0, -72, 96.6f);
        }

        else

        {
            myBlade.parent = transform.Find("mixamorig: Hips / mixamorig:LeftUpLeg");
            myBlade.localPosition = new Vector3(-0.1f, 0.05f, 0.07f);
            myBlade.localRotation = Quaternion.Euler(28.8f, 7.8f, 3.1f);
        }
    }



    public void SwordInHand()
    {
        if(!bladeInHand)
        {
            anim.CrossFade("Withdrawing Sword", 0.05f);
            bladeInHand = true;
        }
    }
}
