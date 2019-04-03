using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;
using UnityEngine.SceneManagement;

public class Player_Health : MonoBehaviour {


    public float hpMax = 100;
    public float currentHP;
    public GameObject deathScreen;


    Image UI_Health;
    RigidbodyFirstPersonController controller;
    Color UI_ScreenBlood_Color;
    Image UI_ScreenBlood;
    Transform UI_ScreenBlood_Transform;
    bool isDeath = false;



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
        deathScreen = GameObject.Find("DeathScreen");
        if(deathScreen != null)
        deathScreen.SetActive(false);
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

        if(isDeath){
            if(Input.anyKey){
                Scene activeScene= SceneManager.GetActiveScene();
                Time.timeScale = 1f;
                SceneManager.LoadScene(activeScene.buildIndex);
            }
        }

    }

    public void HealPlayer(float healAmmont){
        currentHP += healAmmont;
        Debug.Log("New HP : " + currentHP);
    }

    private void Die(){
        currentHP = 0;
        UI_Health.fillAmount = 0f;
        isDeath = true;
        if(deathScreen != null)
        deathScreen.SetActive(true);
    }


    public void DamagePlayer(int damages)
    {
        currentHP -= damages;
        if (currentHP <= 0f) Die();

        controller.movementSettings.delayAttackSettingsOver = 1f + (hpMax - currentHP) * 0.01f;
        controller.IsTakingDamages();
        Debug.Log(currentHP);
    }
}
