using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;

public class Player_Health : MonoBehaviour {


    public float hpMax = 100;
    public float currentHP;


    Image UI_Health;
    RigidbodyFirstPersonController controller;
    Color UI_ScreenBlood_Color;
    Image UI_ScreenBlood;
    Transform UI_ScreenBlood_Transform;



    // Use this for initialization
    void Start ()
    {
        currentHP = hpMax;
        UI_Health = GameObject.Find("UI_Health").GetComponent<Image>();
        UI_Health.fillAmount = (currentHP / hpMax) * 0.6f + 0.2f;
        controller = GetComponent<RigidbodyFirstPersonController>();
        UI_ScreenBlood = GameObject.Find("UI_ScreenBlood").GetComponent<Image>();
        UI_ScreenBlood_Color = UI_ScreenBlood.color;
        UI_ScreenBlood_Transform = GameObject.Find("UI_ScreenBlood").GetComponent<Transform>();
    }
	



	// Update is called once per frame
	void Update ()
    {
        UI_Health.fillAmount = Mathf.MoveTowards(UI_Health.fillAmount, (currentHP / hpMax) * 0.6f + 0.2f, Time.deltaTime * 2);

        float valueAlpha = Mathf.Clamp((hpMax - currentHP) * 0.01f, 0.6f, 0.99f);
        float valueScale = 1 + (currentHP / hpMax) * 0.5f;

        UI_ScreenBlood_Color.a = Mathf.MoveTowards(UI_ScreenBlood_Color.a, controller.movementSettings.isTakingDamages ? valueAlpha : 0, Time.deltaTime * (controller.movementSettings.isTakingDamages ? 10 : 1));
        UI_ScreenBlood.color = UI_ScreenBlood_Color;
        UI_ScreenBlood_Transform.localScale = Vector3.one * valueScale;

    }




    public void DamagePlayer(int damages)
    {
        currentHP -= damages;
        if (currentHP < 0) currentHP = 0;

        controller.movementSettings.delayAttackSettingsOver = 1f + (hpMax - currentHP) * 0.01f;
        controller.IsTakingDamages();
        Debug.Log(currentHP);
    }
}
