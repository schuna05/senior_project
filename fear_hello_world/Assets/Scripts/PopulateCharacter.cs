﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
using Mono.Data.Sqlite;
using System.IO;
using System.Collections.Generic;

public class PopulateCharacter : MonoBehaviour
{
    Dictionary<string, string> objectReference = new Dictionary<string, string>();
    

    List<(object teamNum, object charClass, object armor, object shield, object weapon, object health, object leader, object xpos, object zpos)> armyList = new List<(object teamNum, object charClass, object armor, object shield, object weapon, object health, object leader, object xpos, object zpos)>();
    private void Start()
    {
        objectReference.Add("Peasant", "peasantprefab");
        objectReference.Add("Trained Warrior", "warriorprefab");
        objectReference.Add("Magic User", "wizardprefab");
        
        //DuplicateObjects();
        string connection = "URI=file:" + Application.dataPath + "/Data/fearful_data";
        Debug.Log(connection);
        IDbConnection dbcon = new SqliteConnection(connection);
        dbcon.Open();
        IDbCommand cmnd_read = dbcon.CreateCommand();
        IDataReader reader;
        string query = "SELECT a.teamNumber, t.class, ar.armor, art.armor, w.name, a.currentHealth, a.isLeader, a.xpos,a.zpos FROM Army a, Armor ar, Armor art, Troop t, Weapon w Where a.class = t.id and a.armor = ar.id and a.shield = art.id and a.weapon = w.id";
        cmnd_read.CommandText = query;
        reader = cmnd_read.ExecuteReader();
        while (reader.Read())
        {
            
            var competitor = (teamNum: reader[0], charClass: reader[1], armor: reader[2], shield: reader[3], weapon: reader[4], health: reader[5], leader: reader[6], xpos: reader[7], zpos: reader[8]);
            armyList.Add(competitor);
            Debug.Log(objectReference[competitor.charClass.ToString()]);
        }


        dbcon.Close();
        for (var i = 0; i < armyList.Count; i++)
        {
            Debug.Log("Amount is "+armyList[i].xpos+" and type is " + armyList[i].zpos);
            DuplicateObjects(objectReference[armyList[i].charClass.ToString()], armyList[i]);
        }

    }
    private void DuplicateObjects(string prefab, (object teamNum, object charClass, object armor, object shield, object weapon, object health, object leader, object xpos, object zpos) characterInfo)
    {
        //GameObject referenceTile = (GameObject)Instantiate(Resources.Load(prefab));
        GameObject tile = (GameObject)Instantiate(Resources.Load(prefab));
        int xPos = System.Convert.ToInt32(characterInfo.xpos);
        int yPos = 0;
        int zPos = System.Convert.ToInt32(characterInfo.zpos);
        Debug.Log(zPos);
        tile.transform.position = new Vector3(xPos, yPos,zPos);
        CharacterFeatures referenceScript = tile.GetComponent<CharacterFeatures>();
        referenceScript.health = System.Convert.ToInt32(characterInfo.health);
        referenceScript.shield = characterInfo.shield.ToString();
        referenceScript.weapon = characterInfo.weapon.ToString();
        referenceScript.armclass = characterInfo.armor.ToString();
        referenceScript.isLeader = System.Convert.ToInt32(characterInfo.leader);
        referenceScript.charclass = characterInfo.charClass.ToString();


        PlayerMovement canmove = tile.GetComponent<PlayerMovement>();
        canmove.enabled = false;



    }


}