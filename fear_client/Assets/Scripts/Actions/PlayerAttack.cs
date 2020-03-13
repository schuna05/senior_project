﻿using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAttack : MonoBehaviour
{
    public LayerMask whatCanBeClickedOn;
    public GameObject attackObject;
    public bool canAttack, timeToDistroy;
    private Camera camera1;
    private GameObject xbutton, attackbutton, cancelbutton;
    private GameObject uiCanvas;
    private GameObject scripts;

    // Start is called before the first frame update
    void Start()
    {
        
        //uiCanvas = GameObject.FindGameObjectWithTag("PlayerAction");
        ActivateObjects();
    }

    public void ActivateObjects(){
        scripts = GameObject.FindGameObjectWithTag("scripts");
        uiCanvas = scripts.GetComponent<PlayerSpotlight>().attackcanvas;
        camera1 = scripts.GetComponent<PlayerSpotlight>().camera1;
        xbutton = scripts.GetComponent<PlayerSpotlight>().xbutton;
        attackbutton = scripts.GetComponent<PlayerSpotlight>().attackbutton;
        cancelbutton = scripts.GetComponent<PlayerSpotlight>().cancelbutton;
        timeToDistroy = false;
    }



    // Update is called once per frame
    /// <summary>
    /// This function is called once per frame while attack is active. It is similar to player spotlight
    /// in that it is constantly checking to see if someone has been clicked on and updating the UI.
    /// </summary>
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && Client.Instance.hasControl)
        {
            RaycastHit hit;
            Ray ray = camera1.ScreenPointToRay(Input.mousePosition);
            //Debug.Log(Physics.Raycast(ray, out hit, 100.0f));
            if (Physics.Raycast(ray, out hit, 100.0f, layerMask:1<<10))
            {

                if (hit.transform != null)
                {
                    attackObject = hit.transform.gameObject;
                    
                    CharacterFeatures referenceScript = attackObject.GetComponent<CharacterFeatures>();

                    if (referenceScript.team != gameObject.GetComponent<CharacterFeatures>().team)
                    {
                        //if(WithinRange(3.5F, attackObject.transform.position[0],attackObject.transform.position[2], myGameObject.transform.position[0], myGameObject.transform.position[2], false))
                        if(Vector3.Distance(gameObject.transform.position,attackObject.transform.position) < gameObject.GetComponent<CharacterFeatures>().attackRange)
                        {
                            string text = $"Name: Roman\nHealth: {referenceScript.health}\nClass: {referenceScript.charclass}\nDefense: {referenceScript.damageBonus}\nWithin Range: Yes";
                            scripts.GetComponent<BfieldUIControl>().ChangeText(
                                scripts.GetComponent<BfieldUIControl>().attackPanelEnemyInfo, text);
                            canAttack = true;
                        }
                        else
                        {
                            string text = $"Name: Roman\nHealth: {referenceScript.health}\nClass: {referenceScript.charclass}\nDefense: {referenceScript.damageBonus}\nWithin Range: No";
                            scripts.GetComponent<BfieldUIControl>().ChangeText(
                                scripts.GetComponent<BfieldUIControl>().attackPanelEnemyInfo, text);
                            canAttack = false;
                        }

                    }
                    else
                    {
                        string text = $"You can not attack\nyour own team.";
                        scripts.GetComponent<BfieldUIControl>().ChangeText(
                            scripts.GetComponent<BfieldUIControl>().attackPanelEnemyInfo, text);
                    }
                    
                }
            }
        }

    }

    /// <summary>
    /// This function is used to decide with the point on the screen clicked. This is used to view if the player clicked can be attacked from where the player is currently.
    /// </summary>
    /// <param name="maxDistance"></param>
    /// <param name="newPointX"></param>
    /// <param name="newPointZ"></param>
    /// <param name="curPointX"></param>
    /// <param name="curPointZ"></param>
    /// <param name="ranged">There are special resitrictions for ranged attacks</param>
    /// <returns></returns>
    //public bool WithinRange(float maxDistance, float newPointX, float newPointZ, float curPointX, float curPointZ, bool ranged)
    //{

    //    float squaredX = (newPointX - curPointX) * (newPointX - curPointX);
    //    float squaredZ = (newPointZ - curPointZ) * (newPointZ - curPointZ);
    //    float result = Mathf.Sqrt(squaredX + squaredZ);
    //    Debug.Log("IAMHERE");
    //    Debug.Log(result);
    //    if (maxDistance >= result)
    //    {
    //        return true;
    //    }

    //    return false;
    //}

    /// <summary>
    /// This function is called to modify the game to "attack mode". It activates the attacking click options and deactivates the player spotlight clicked.
    /// This function is always the first function called to get to other functions.
    /// </summary>
    public void ActivateAttack()
    {
        scripts = GameObject.FindGameObjectWithTag("scripts");
        scripts.GetComponent<PlayerSpotlight>().enabled = false;
        CharacterFeatures referenceScript = gameObject.GetComponent<CharacterFeatures>();
        referenceScript.isAttacking = true;

    }
    /// <summary>
    /// This is the base funciton for all attacks. The trigger for this function to be used is a button on the UI canvas.
    /// Once the button is triggered the player must have already selected a figure to attack or it will get an error. Additionally,
    /// this function will take away any health if need be or it can even trigger the destruction of an object. 
    /// </summary>
    public void Attack()
    {
        System.Random random = new System.Random();
        CharacterFeatures referenceScript = attackObject.GetComponent<CharacterFeatures>();
        CharacterFeatures referenceScript2 = gameObject.GetComponent<CharacterFeatures>();
        GameLoop gamevars = scripts.GetComponent<GameLoop>();
        if (canAttack)
        {
            
            if (random.Next(0,20)+referenceScript2.attack >= referenceScript.armorBonus)
            {
                int damageTaken = random.Next(1, (referenceScript2.damage))+referenceScript2.damageBonus;
                if ((referenceScript.health - damageTaken) <= 0)
                {
                    string text = $"You have dealt fatal damage\nto the player named Roman ";
                    scripts.GetComponent<BfieldUIControl>().ChangeText(
                        scripts.GetComponent<BfieldUIControl>().attackPanelEnemyInfo, text);
                    timeToDistroy = true;

                    gamevars.PlayerRemoval("Attack", attackObject.GetComponent<CharacterFeatures>().troopId,2);
                    //Destroy(attackObject);
                    Client.Instance.SendRetreatData(referenceScript.troopId,1);
                    //Destroy(attackObject.GetComponent<CharacterFeatures>().myCircle);
                    //Destroy(attackObject.GetComponent<CharacterFeatures>().attackRange);
                }
                else
                {
                    referenceScript.health = System.Convert.ToInt32(referenceScript.health - damageTaken);
                    if (Client.Instance != null){
                        Client.Instance.SendAttackData(referenceScript.troopId,damageTaken);
                    }
                    string text = $"You attack was a success \nand you have dealt {damageTaken} damage\nto the player named Roman ";
                    scripts.GetComponent<BfieldUIControl>().ChangeText(
                        scripts.GetComponent<BfieldUIControl>().attackPanelEnemyInfo, text);
                }
                
            }
            else
            {
                string text = $"You could not get passed their armor\nyour attack has failed";
                scripts.GetComponent<BfieldUIControl>().ChangeText(
                    scripts.GetComponent<BfieldUIControl>().attackPanelEnemyInfo, text);
            }
            xbutton.SetActive(true);
            attackbutton.SetActive(false);
            cancelbutton.SetActive(false);
        }
        else
        {
            string text = $"You can not attack this target\nthey are not in range. Select \nanother fighter to attack.";
            scripts.GetComponent<BfieldUIControl>().ChangeText(
                scripts.GetComponent<BfieldUIControl>().attackPanelEnemyInfo, text);
        }
    }
    /// <summary>
    /// This function is used after an attack and is used to enable the player spotlight script and deactivate this script.
    /// The function will also destroy any object that has been destroyed.
    /// </summary>

    public void DeactivateAttack()
    {
        scripts = GameObject.FindGameObjectWithTag("scripts");
        scripts.GetComponent<PlayerSpotlight>().enabled = true;
        CharacterFeatures referenceScript = gameObject.GetComponent<CharacterFeatures>();
        attackbutton.SetActive(true);
        cancelbutton.SetActive(true);
        referenceScript.isAttacking = false;
        string text = $"Name: Health: \nClass: \nDefense: \nWithin Range: ";
        scripts.GetComponent<BfieldUIControl>().ChangeText(
            scripts.GetComponent<BfieldUIControl>().attackPanelEnemyInfo, text);
        if (timeToDistroy)
        {
            Debug.Log("__________________________________________________________" + timeToDistroy);
            timeToDistroy = false;
            Destroy(attackObject.GetComponent<CharacterFeatures>().myCircle);
            //Destroy(attackObject.GetComponent<CharacterFeatures>().attackRange);
            Destroy(attackObject);
        }
    }
}
