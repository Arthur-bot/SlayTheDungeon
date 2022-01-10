using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


public class JSONParser : MonoBehaviour
{
    public string jsonString;
    public CardData card;
    public string pathToMods;
    private string[] ListOfJSON;
    public List<CardData> ListOfCardData;
    
    // Start is called before the first frame update
    void Awake()
    {
        pathToMods = Application.dataPath + "/Mods";
        if (!Directory.Exists(pathToMods))
            Directory.CreateDirectory(pathToMods);


        jsonString = JsonUtility.ToJson(new ModCardDataStructure());
        Debug.Log(jsonString);
    }
    
    void Start()
    {
        
        ChargeSet("Test");

        //jsonString = JsonUtility.ToJson(ScriptableObject.CreateInstance("CardData"));

        //card = (CardData) ScriptableObject.CreateInstance("CardData");
        //JsonUtility.FromJsonOverwrite(jsonString,card);
        
        
    }

    public bool ChargeSet(string nameOfSet)
    {
        if (! Directory.Exists(pathToMods + "/" + nameOfSet))
        {
            return false; 
        }
        else
        {
            ListOfJSON = Directory.GetFiles(pathToMods + "/" + nameOfSet, "*.json");

            for (int i = 0; i < ListOfJSON.Length; i++)
            {
                StreamReader reader = new StreamReader(ListOfJSON[i]);
                jsonString = reader.ReadToEnd();
                card = (CardData)ScriptableObject.CreateInstance("CardData");
                JsonUtility.FromJsonOverwrite(jsonString, card);
                ListOfCardData.Add(card);
            }
            Debug.Log(ListOfJSON.Length);
            return true;

        }
            
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
