using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public WeaponManager testWm;

    private static GameManager instance;
    private DataBase weaponDB;
    private WeaponFactory weaponFact;

    // Start is called before the first frame update
    void Awake()
    {
        CheckGameObject();
        CheckSingle();
    }

    void Start()
    {
        InitWeaponDB();
        InitWeaponFactory();

        
        testWm.UpdateWeaponCollider("R", weaponFact.CreateWeapon("Sword", "R", testWm));
        testWm.ChangeDualHands(false);
    }

    // Update is called once per frame
    //void Update()
    //{
    //    
    //}

    private void OnGUI()
    {
        if (GUI.Button(new Rect(10, 10, 150, 30), "R:Sword"))
        {
            testWm.UnloadWeapon("R");
            testWm.UpdateWeaponCollider("R", weaponFact.CreateWeapon("Sword", "R", testWm));
            testWm.ChangeDualHands(false);
        }
        if (GUI.Button(new Rect(10, 50, 150, 30), "R:Falchion"))
        {
            testWm.UnloadWeapon("R");
            testWm.UpdateWeaponCollider("R", weaponFact.CreateWeapon("Falchion", "R", testWm));
            testWm.ChangeDualHands(true);
        }
        if (GUI.Button(new Rect(10, 90, 150, 30), "R:Mace"))
        {
            testWm.UnloadWeapon("R");
            testWm.UpdateWeaponCollider("R", weaponFact.CreateWeapon("Mace", "R", testWm));
            testWm.ChangeDualHands(false);
        }
    }

    private void InitWeaponDB()
    {
        weaponDB = new DataBase();
    }
    

    private void InitWeaponFactory()
    {
        weaponFact = new WeaponFactory(weaponDB);
    }

    private void CheckSingle()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            return;
        }
        Destroy(this);
    }

    private void CheckGameObject()
    {
        if (tag == "GM")
        {
            return;
        }
        Destroy(this);
    }
}
