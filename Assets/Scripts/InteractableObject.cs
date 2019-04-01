using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour {

    public enum Description { Door, Openable, Bottle, Fruit, Paper, Candle, Elixir, Pistol, Body, Audiograph, Coin };
    public Description d;
    public int objectType = 0;
    //Openable  0   Window;

    //Bottle    0   Empty
    //Bottle    1   Chloroform

    //Elixir    0   Red
    //Elixir    1   Blue

    //Door      0   Simple Door
    //Door      1   Double Door

    public AnimationClip[] allAnimations;
    public int currentState;
    [HideInInspector]
    public string stringInteraction;
    [HideInInspector]
    public string stringDescription;
    [HideInInspector]
    public string stringInteractionOption;
    Animation myAnimation;
    [HideInInspector]
    public bool isPickup;
    Transform pocket;
    Transform playerTransform;
    GameObject door_1;
    GameObject door_2;
    CarryAndThrow carryAndThrow;


    // Use this for initialization
    void Start ()
    {
        playerTransform = GameObject.Find("RigidBodyFPSController").GetComponent<Transform>();
        if (GetComponent<Animation>()) myAnimation = GetComponent<Animation>();
        UpdateState(false);
        pocket = GameObject.Find("Pocket").GetComponent<Transform>();
        carryAndThrow = GameObject.Find("MainCamera").GetComponent<CarryAndThrow>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (isPickup) Pickup();
	}


    //When the player take pickup
    void Pickup()
    {
        transform.position = Vector3.MoveTowards(transform.position, pocket.position, Time.deltaTime * 15);
        GetComponent<Collider>().isTrigger = true;
        GetComponent<Rigidbody>().useGravity = false;
    }


    public void PlayInteraction()
    {
        UpdateState(true);
    }



    public void PlayInteractionOption()
    {
        UpdateStateOption();
    }



    void UpdateAnimation()
    {
        if (myAnimation != null && !myAnimation.isPlaying)
        {
            myAnimation.Play(allAnimations[currentState].name);
            currentState++;
            if (currentState >= allAnimations.Length)
            {
                currentState = 0;
            }
        }
    }



    void UpdateState(bool init)
    {
        if (d == Description.Openable)
        {
            if (objectType == 0) //Openable Window
            {
                if(init) UpdateAnimation();

                if (currentState == 0)
                {
                    stringInteraction = "<b>F</b> <i>Use</i>";
                    stringDescription = "Window";
                }

                if (currentState == 1)
                {
                    stringInteraction = "<b>F</b> <i>Use</i>";
                    stringDescription = "Window";
                }
            }

        }

        else if (d == Description.Bottle)
        {
            if (objectType == 0) //Bottle Empty
            {
                stringInteraction = "<b>F</b> <i>Carry</i>";
                stringDescription = "Empty Bottle";
            }

            if (objectType == 1) //Bottle Chloroform
            {
                stringInteraction = "<b>F</b> <i>Carry</i>";
                stringDescription = "Chloroform Bottle";
            }
        }

        else if (d == Description.Elixir)
        {
            if (objectType == 0) //Elixir Red
            {
                stringInteraction = "<b>F</b> <i>Pick up</i>";
                stringDescription = "S&J Health Elixir";
                if (init) isPickup = true;
            }

            if (objectType == 1) //Elixir Blue
            {
                stringInteraction = "<b>F</b> <i>Pick up</i>";
                stringDescription = "Addermine Solution";
                if (init) isPickup = true;
            }
        }

        else if (d == Description.Coin)
        {
            stringInteraction = "<b>F</b> <i>Pick up</i>";
            stringDescription = "Coin";
            if (init) isPickup = true;
        }

        else if (d == Description.Fruit)
        {
            if (objectType == 0) //Fruit Apple
            {
                stringInteraction = "<b>F</b> <i>Eat</i>";
                stringDescription = "Gristol Apple";
            }

            if (objectType == 1) //Fruit Pear
            {
                stringInteraction = "<b>F</b> <i>Eat</i>";
                stringDescription = "Tyvian Pear";
            }

            if (objectType == 2) //Fruit Plantain
            {
                stringInteraction = "<b>F</b> <i>Eat</i>";
                stringDescription = "Serkonan Plantain";
            }

            if (init) isPickup = true;
        }

        else if (d == Description.Body)
        {
            stringInteractionOption = "<b>F</b> <i>Carry</i>";
            stringDescription = "Elite Guard (" + (currentState == 0 ? "dead" : "unconscious") + ")";
        }

        else if (d == Description.Candle)
        {
            if (init)
            {
                transform.Find("CandleFlame_1").gameObject.SetActive(false);
                transform.Find("CandleFlame_2").gameObject.SetActive(false);
                transform.Find("CandleFlame_3").gameObject.SetActive(false);

                transform.Find("PointLightFlame").gameObject.SetActive(false);
            }

            if (transform.Find("PointLightFlame").gameObject.activeSelf)
            {
                stringInteraction = "<b>F</b> <i>Extinguish</i>";
                stringDescription = "Candles";
            }

            else

            {
                stringInteraction = "";
                stringDescription = "";
            }
        }

        else if (d == Description.Door)
        {
            //currentState  0   Close
            //currentState  1   Open Side A
            //currentState  2   Open Side B

            if (!myAnimation.isPlaying)
            {
                Transform side_A = transform.root.Find("Side_A").GetComponent<Transform>();
                Transform side_B = transform.root.Find("Side_B").GetComponent<Transform>();

                if (objectType == 1)
                {
                    door_1 = transform.root.Find("Door_1").gameObject;
                    door_2 = transform.root.Find("Door_2").gameObject;
                }

                if (currentState == 0)
                {
                    stringInteraction = "<b>F</b> <i>Open</i>";
                    stringDescription = "Breakable Door";
                    stringInteractionOption = "<b>F</b> <i>Keyhole Peek</i>";
                    if (init)
                    {
                        stringInteractionOption = "";

                        //Double Door
                        if (objectType == 1)
                        {
                            if (gameObject.name == "Door_1")
                            {
                                door_2.GetComponent<InteractableObject>().stringInteractionOption = "";
                            }

                            else

                            {
                                door_1.GetComponent<InteractableObject>().stringInteractionOption = "";
                            }
                        }
                    }

                    if (init)
                    {
                        if (Vector3.Distance(playerTransform.position, side_A.position) < Vector3.Distance(playerTransform.position, side_B.position))
                        {
                            currentState = 2;
                            if(objectType == 0) myAnimation.Play("DOOR_Open_Side_A");

                            //Double Door
                            if (objectType == 1)
                            {
                                if(gameObject.name == "Door_1")
                                {
                                    myAnimation.Play("DOOR_Open_Side_A");
                                    door_2.GetComponent<InteractableObject>().currentState = 2;
                                    door_2.GetComponent<Animation>().Play("DOOR_Open_Side_B");

                                    door_2.GetComponent<InteractableObject>().stringInteraction = "<b>F</b> <i>Close</i>";
                                    door_2.GetComponent<InteractableObject>().stringDescription = "Breakable Door";
                                }

                                else

                                {
                                    myAnimation.Play("DOOR_Open_Side_B");
                                    door_1.GetComponent<InteractableObject>().currentState = 2;
                                    door_1.GetComponent<Animation>().Play("DOOR_Open_Side_A");

                                    door_1.GetComponent<InteractableObject>().stringInteraction = "<b>F</b> <i>Close</i>";
                                    door_1.GetComponent<InteractableObject>().stringDescription = "Breakable Door";
                                }
                            }
                        }

                        else

                        {
                            currentState = 1;
                            if (objectType == 0) myAnimation.Play("DOOR_Open_Side_B");

                            //Double Door
                            if (objectType == 1)
                            {
                                if (gameObject.name == "Door_1")
                                {
                                    myAnimation.Play("DOOR_Open_Side_B");
                                    door_2.GetComponent<InteractableObject>().currentState = 1;
                                    door_2.GetComponent<Animation>().Play("DOOR_Open_Side_A");

                                    door_2.GetComponent<InteractableObject>().stringInteraction = "<b>F</b> <i>Close</i>";
                                    door_2.GetComponent<InteractableObject>().stringDescription = "Breakable Door";
                                }

                                else

                                {
                                    myAnimation.Play("DOOR_Open_Side_A");
                                    door_1.GetComponent<InteractableObject>().currentState = 1;
                                    door_1.GetComponent<Animation>().Play("DOOR_Open_Side_B");

                                    door_1.GetComponent<InteractableObject>().stringInteraction = "<b>F</b> <i>Close</i>";
                                    door_1.GetComponent<InteractableObject>().stringDescription = "Breakable Door";
                                }
                            }
                        }
                        stringInteraction = "<b>F</b> <i>Close</i>";
                        stringDescription = "Breakable Door";

                    }
                }

                else

                {
                    stringInteraction = "<b>F</b> <i>Close</i>";
                    stringDescription = "Breakable Door";

                    if (init)
                    {
                        if (currentState == 1)
                        {
                            if (objectType == 0) myAnimation.Play("DOOR_Close_Side_B");

                            //Double Door
                            if (objectType == 1)
                            {
                                if (gameObject.name == "Door_1")
                                {
                                    myAnimation.Play("DOOR_Close_Side_B");
                                    door_2.GetComponent<InteractableObject>().currentState = 0;
                                    door_2.GetComponent<Animation>().Play("DOOR_Close_Side_A");

                                    door_2.GetComponent<InteractableObject>().stringInteraction = "<b>F</b> <i>Open</i>";
                                    door_2.GetComponent<InteractableObject>().stringDescription = "Breakable Door";
                                    door_2.GetComponent<InteractableObject>().stringInteractionOption = "<b>F</b> <i>Keyhole Peek</i>";
                                }

                                else

                                {
                                    myAnimation.Play("DOOR_Close_Side_A");
                                    door_1.GetComponent<InteractableObject>().currentState = 0;
                                    door_1.GetComponent<Animation>().Play("DOOR_Close_Side_B");

                                    door_1.GetComponent<InteractableObject>().stringInteraction = "<b>F</b> <i>Open</i>";
                                    door_1.GetComponent<InteractableObject>().stringDescription = "Breakable Door";
                                    door_1.GetComponent<InteractableObject>().stringInteractionOption = "<b>F</b> <i>Keyhole Peek</i>";
                                }
                            }
                        }

                        else

                        {
                            if (objectType == 0) myAnimation.Play("DOOR_Close_Side_A");

                            //Double Door
                            if (objectType == 1)
                            {
                                if (gameObject.name == "Door_1")
                                {
                                    myAnimation.Play("DOOR_Close_Side_A");
                                    door_2.GetComponent<InteractableObject>().currentState = 0;
                                    door_2.GetComponent<Animation>().Play("DOOR_Close_Side_B");

                                    door_2.GetComponent<InteractableObject>().stringInteraction = "<b>F</b> <i>Open</i>";
                                    door_2.GetComponent<InteractableObject>().stringInteractionOption = "<b>F</b> <i>Keyhole Peek</i>";
                                    door_2.GetComponent<InteractableObject>().stringDescription = "Breakable Door";
                                }

                                else

                                {
                                    myAnimation.Play("DOOR_Close_Side_B");
                                    door_1.GetComponent<InteractableObject>().currentState = 0;
                                    door_1.GetComponent<Animation>().Play("DOOR_Close_Side_A");

                                    door_1.GetComponent<InteractableObject>().stringInteraction = "<b>F</b> <i>Open</i>";
                                    door_1.GetComponent<InteractableObject>().stringInteractionOption = "<b>F</b> <i>Keyhole Peek</i>";
                                    door_1.GetComponent<InteractableObject>().stringDescription = "Breakable Door";
                                }
                            }
                        }

                        currentState = 0;
                        stringInteraction = "<b>F</b> <i>Open</i>";
                        stringDescription = "Breakable Door";
                        stringInteractionOption = "<b>F</b> <i>Keyhole Peek</i>";
                    }
                }
            }
        }
    }


    void UpdateStateOption()
    {
        if (d == Description.Body)
        {
            carryAndThrow.CarryBody();
        }

        else

        if(d == Description.Door)
        {
            // Door closed
            if(currentState == 0)
            {
                Transform side_A = transform.root.Find("Side_A").GetComponent<Transform>();
                Transform side_B = transform.root.Find("Side_B").GetComponent<Transform>();

                if (Vector3.Distance(playerTransform.position, side_A.position) < Vector3.Distance(playerTransform.position, side_B.position))
                {
                    playerTransform.position = side_A.position + Vector3.up;
                    playerTransform.rotation = side_A.rotation;
                    transform.root.Find("KeyholePos_B/Camera_B").gameObject.SetActive(true);
                }

                else
                {
                    playerTransform.position = side_B.position + Vector3.up;
                    playerTransform.rotation = side_B.rotation;
                    transform.root.Find("KeyholePos_A/Camera_A").gameObject.SetActive(true);
                }
            }
        }
    }
}
