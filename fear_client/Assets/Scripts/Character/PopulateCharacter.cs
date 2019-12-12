﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
using System.IO;

public class PopulateCharacter : MonoBehaviour
{
    public List<GameObject> p0Chars, p1Chars;
    private void Start()
    {



    }

    /// <summary>
    /// This function is designed to create all the objects fed in by the database
    /// it is currently only set up to create the objects for player 1 and another
    /// functions needs to be added to deal with a second player.
    /// </summary>
    /// <param name="prefab">The name of prefab</param>
    /// <param name="xpos"></param>
    /// <param name="zpos"></param>
    /// <param name="teamNum"></param>
    /// <param name="health"></param>
    /// <param name="attack"></param>
    /// <param name="damageBonus"></param>
    /// <param name="movement"></param>
    /// <param name="perception"></param>
    /// <param name="armorBonus"></param>
    /// <param name="armorStealth"></param>
    /// <param name="damage"></param>
    /// <param name="leader"></param>

    //This function is desgined to create each instance of the game objects. It is called for every character created by the game and populates all of the characteristics of the character.
    public void DuplicateObjects(string prefab,float xpos,float zpos, int teamNum, int health, int attack,
        int damageBonus, int movement, int perception, int armorBonus,int armorStealth, int damage, int leader)
    {
        //GameObject referenceTile = (GameObject)Instantiate(Resources.Load(prefab));
        GameObject tile = (GameObject)Instantiate(Resources.Load(prefab));
        GameObject circle = (GameObject)Instantiate(Resources.Load("circleprefab"));
        GameObject circle2 = (GameObject)Instantiate(Resources.Load("circleprefab"));
        float floating = 0.2F;
        tile.transform.position = new Vector3(xpos,0,zpos);
        circle.transform.position = new Vector3(xpos,floating, zpos);
        circle.transform.localScale = new Vector3(21, 21, 21);
        circle2.transform.position = new Vector3(xpos, floating, zpos);
        circle2.transform.localScale = new Vector3(9, 9, 9);
        circle.GetComponent<Renderer>().enabled = false;
        circle2.GetComponent<Renderer>().enabled = false;
        CharacterFeatures referenceScript = tile.GetComponent<CharacterFeatures>();
        referenceScript.team = System.Convert.ToInt32(teamNum);
        referenceScript.health = System.Convert.ToInt32(health);
        referenceScript.attack = System.Convert.ToInt32(attack);
        referenceScript.damageBonus = System.Convert.ToInt32(damageBonus);
        referenceScript.movement = System.Convert.ToInt32(movement);
        referenceScript.perception = System.Convert.ToInt32(perception);
        //referenceScript.magicattack = System.Convert.ToInt32(characterInfo.magicAttack);
        //referenceScript.magicdamage = System.Convert.ToInt32(characterInfo.magicDamage);
        referenceScript.bonus = System.Convert.ToInt32(armorBonus);
        referenceScript.stealth = System.Convert.ToInt32(armorStealth);
        referenceScript.damage = System.Convert.ToInt32(damage);
        referenceScript.isLeader = System.Convert.ToInt32(leader);
        referenceScript.myCircle = circle;
        referenceScript.attackRange = circle2;
        referenceScript.character = tile;
        referenceScript.isFocused = false;


        //tile.GetComponent<PlayerMovement>().enabled = false;
        //tile.GetComponent<PlayerAttack>().enabled = false;
        //if (teamNum == 0)
        //{
        //    p0Chars.Add(tile);
        //}
        //else
        //{
        //    //p1Chars.Add(tile);
        //}


    }


}
