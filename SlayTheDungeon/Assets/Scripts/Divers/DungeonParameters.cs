using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DungeonParameters : MonoBehaviour
{
    private int numberOfRoom;
    private int fireCampFrequency;
    private int chestFrequency;
    private int monsterFrequency;
    private bool revealMap;
    public static DungeonParameters Instance;
    [SerializeField] Slider nbRoomsSlider;
    [SerializeField] Slider chestSlider;
    [SerializeField] Slider monsterSlider;
    [SerializeField] Slider fireSlider;
    [SerializeField] Toggle revealToggle;

    public int FireCampFrequency { get => fireCampFrequency;}
    public int ChestFrequency { get => chestFrequency;}
    public int MonsterFrequency { get => monsterFrequency;}
    public int NumberOfRoom { get => numberOfRoom; set => numberOfRoom = value; }
    public bool RevealMap { get => revealMap;}

    void Awake()
    {
        if (!Instance)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(Instance.gameObject);
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    private void Start()
    {
        SetNumberOfRooms();
        SetMonsters();
        SetFirecamp();
        SetChest();
    }
    public void SetNumberOfRooms()
    {
        numberOfRoom = (int) nbRoomsSlider.value;
    }
    public void SetFirecamp()
    {
        fireCampFrequency = (int) fireSlider.value;
    }
    public void SetChest()
    {
        chestFrequency = (int)chestSlider.value;
    }
    public void SetMonsters()
    {
        monsterFrequency = (int)monsterSlider.value;
    }
    public void SetReveal()
    {
        revealMap = revealToggle.isOn;
    }
}
