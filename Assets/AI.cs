using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;


public class AI : MonoBehaviour
{

    public List<GameObject> AllObjects = new List<GameObject>();
    public List<GameObject> DetectedUnits = new List<GameObject>();

    public GameObject[] ExtractTemp = new GameObject[5];
    public GameObject[] TurrTemp = new GameObject[5];

    public GameObject[] ExtractorAdvUpgradeSlots = new GameObject[5];

    public GameObject UnitQ;

    public GameObject Turrent;
    public GameObject Extractor;

    public int TurrentCost;
    public int ExtractorCost;

    public GameObject AiResourceScript;
    public GameObject AiSpawner;
    public GameObject AiMovmentManager;

    public struct ExtractorUpgrade
    {
        public int CurrentTier;
        public GameObject theExtractor;
    }


    public struct BuildingSlots
    {
        public GameObject slot;
        public int SlotNumber;
        public bool Occupied;
    }

    public struct UnitMethod
    {
        public int ID;
        public int SpaawnPlan;
    }

    public struct UnitSpawnManager
    {
        public int AmountOfUnit;
    }

    public struct Situation
    {
        public int EconAlertLevel;
        public int ArmyAlertLevel;
    }

    public struct AiState 
    {
        public int ModuleOne_Right;
        public int ModuleTwo_Right;

        public float ProbabilityTop_Right;
        public float ProbabilityBottom_Right;

        public int Command_Right;
        public int AuxCommand_Right;



        public int ModuleOne_Left;
        public int ModuleTwo_Left;

        public double ProbabilityTop_Left;
        public double ProbabilityBottom_Left;

        public int Command_Left;
        public int AuxCommand_Left;



        public int IgnoreSide; // (0 = Neither)  (1 = Right)  (11 = Left)
        public int Roll;


    }

    ExtractorUpgrade[] AdvExtractorUp = new ExtractorUpgrade[5];

    AiState[,] State = new AiState[6,6];

    Situation[] situation = new Situation[2];

    BuildingSlots[] ExtractorSlots = new BuildingSlots[5];
    BuildingSlots[] TurrentSlots = new BuildingSlots[5];

    UnitMethod[] IdInfo = new UnitMethod[3];
    UnitSpawnManager[] SpawnList = new UnitSpawnManager[3];


    int freeSlots;

    public GameObject WarningSystem;

    int AdvesaryEcon;
    int AdvesaryMilitary;
    int AdversaryMilitary_Carryover;


    int AiMilitary;
    int AiArmy;
    int AiEcono;
    int AiResourcePool;

    public GameObject AdvesaryResManager;
    public GameObject AiResManager;

    public Text AiEnemyEcon;
    public Text AiEcon;

    public int T1Points;
    public int T2Points;
    public int T3Points;

    public Text AiEnemyMil;
    public Text AiMil;

    bool DetectedSniper;
    bool DetectedShielder;
    int SnipCount;
    int ShielCount;

    public int Difficulty;

    public int EasyMode_Timer;
    public int MidMode_Timer;
    public int HardMode_Timer;

    public int EasyMode_Confirms;
    public int MidMode_Confirms;
    public int HardMode_Cofirms;

    int ScanTimer;
    int StatusConfirms;
    float ResetTimer;

    public int T1TurrentStrength;
    public int ExtractorRate;

    public int T3Strenght;
    public int T2Strength;
    public int T1Strength;

    bool HaltBuild;
    bool HaltEcon;
    int OfflineExtractors;
    int Offline_to_Online;

    private Vector3 FrontLineCoord;

    int ScannedDefenseDiffrence;
    bool SD_AttackAdvice;

    int RollSide;
    float Confirm;

    int SideLeft;
    int SideRight;

    int OD_Count;
    int UpdateMilitary = 0;
    int TurrentStrength;

    public bool UsingAi;
    bool ScanDefFirst;

    // Use this for initialization
    void Start()
    {
        FileRead();
        //     TestState();
        ScanDefFirst = true;
        SD_AttackAdvice = false;
        OfflineExtractors = 0;
        Offline_to_Online = 0;
        HaltBuild = false;
        HaltEcon = false;

        SnipCount = 0;
        ShielCount = 0;
        ResetTimer = 0;

        SideLeft = 0;
        SideRight = 0;

        OD_Count = 0;

        FillSlotStruct();

        if (Difficulty == 0)
        {
            Debug.Log("EasyMode..");
            WarningSystem.GetComponent<WarningMessageManager>().RecieveWarning("Easy Mode");
            ScanTimer = EasyMode_Timer;
            StatusConfirms = EasyMode_Confirms;
        }

        if (Difficulty == 1)
        {
            Debug.Log("MediumMode..");
            WarningSystem.GetComponent<WarningMessageManager>().RecieveWarning("Medium Mode");
            ScanTimer = MidMode_Timer;
            StatusConfirms = MidMode_Confirms;

        }

        if (Difficulty == 2)
        {
            Debug.Log("HardMode..");
            WarningSystem.GetComponent<WarningMessageManager>().RecieveWarning("Hard Mode");
            ScanTimer = HardMode_Timer;
            StatusConfirms = HardMode_Cofirms;

        }

        ResetTimer = ScanTimer+1;

    }

    void FixedUpdate()
    {
        ResetTimer += Time.deltaTime;
        Confirm += Time.deltaTime;

        //       Debug.Log(ResetTimer + " vs " + ScanTimer);
        if (UsingAi == false)
        {
            if (ScanTimer < ResetTimer)
            {
                OverseeEcon();
                ScanDefence();
                OverseeMilitary();

                OffenciveBuild();
                    Offence();
                


                ResetTimer = 0;
            }
        }

        if (UsingAi == true)
        {
            if (ScanTimer < ResetTimer)
            {
                Debug.Log("-==CONFIRM==-");
                Debug.Log("id: " + Confirm);

                Confirm++;

                EconSpy();
                MilitarySpy();
                OverseeEcon();
                OverseeMilitary();

                SetFrontline();

                AiProcessor();
                Debug.Log("[..::==SITUATION==::..]");
                Debug.Log("[" + situation[0].ArmyAlertLevel + ", " + situation[0].EconAlertLevel + "] ");

                SetState();
                //            Debug.Log("Roll: " + RollSide);

                ActivateAiState();
                //  Debug.Log("[" + situation[0].ArmyAlertLevel + ", " + situation[0].EconAlertLevel + "] ");

                ResetTimer = 0;
            }
        }

    }
    // Update is called once per frame
    void Update()
    {

    }

    void AiProcessor() //INCOMPLETE
    {
        float Ai_Econ = AiEcono;
        float Ai_Army = AiArmy;
        float AlertMilitartTemp = 0;
        float AlertEconTemp = 0;

        float Adversary_Econ = AdvesaryEcon;
        float Adversary_Military = AdversaryMilitary_Carryover;

        // AlertEconTemp = Adversary_Econ / Ai_Econ   Ai_Econ / Adversary_Econ;
        AlertEconTemp = Ai_Econ / Adversary_Econ;
        if (Ai_Army != 0)
        {
             AlertMilitartTemp = Ai_Army  / Adversary_Military;
        }
        else if (Ai_Army == 0 && Adversary_Military != 0)
        {
            AlertMilitartTemp = .1f;
        }
        else if (Ai_Army == 0 && Adversary_Military == 0)
        {
            AlertMilitartTemp = 1.0f;
        }

//        Debug.Log("ECO: " + Adversary_Econ + "/" + Ai_Econ);
//        Debug.Log("Mili: " + Ai_Army + "/" + Adversary_Military);

 //       Debug.Log("PROCESS ARMY:: " + AlertMilitartTemp);
 //       Debug.Log("PROCESS ECON:: " + AlertEconTemp);

        // ECPN = 0    MILITARY = 1
        if (AlertEconTemp >= .9 && AlertEconTemp <= 1.1)
        {
            Debug.Log("ECONOMY = M");

            situation[0].EconAlertLevel = 3;
        }

        else if (AlertEconTemp <= .9)
        {
            if (AlertEconTemp >= .7)
            {
                Debug.Log("ECONOMY = MH");

                situation[0].EconAlertLevel = 4; // MH
            }

            else // x <=.6
            {
                Debug.Log("ECONOMY = H");

                situation[0].EconAlertLevel = 5; //H

            }
        }

        else //x > 1.1
        {
            if (AlertEconTemp <= 1.3)
            {
                Debug.Log("ECONOMY = ML");

                situation[0].EconAlertLevel = 2; //ML

            }
            else // x > 1.4
            {
                Debug.Log("ECONOMY = L");

                situation[0].EconAlertLevel = 1; //L

            }
        }

        //MILITARY----
        if (AlertMilitartTemp >= .9 && AlertMilitartTemp <= 1.1)
        {
            Debug.Log("MILITART = M");
            situation[0].ArmyAlertLevel = 3;
        }

        else if (AlertMilitartTemp <= .9)
        {
            if (AlertMilitartTemp >= .7)
            {
                Debug.Log("MILITART = MH");

                situation[0].ArmyAlertLevel = 4; // MH
            }

            else // x <=.6
            {
                Debug.Log("MILITART = H");

                situation[0].ArmyAlertLevel = 5; //H

            }
        }

        else //x > 1.1
        {
            if (AlertMilitartTemp <= 1.3)
            {
                Debug.Log("MILITART = ML");

                situation[0].ArmyAlertLevel = 2; //ML

            }
            else // x > 1.4
            {
                Debug.Log("MILITART = L");

                situation[0].ArmyAlertLevel = 1; //L

            }
        }
    }


    void SetState()
    {
        float rand = Random.value;
        int Side = 0;
        Side = State[situation[0].ArmyAlertLevel, situation[0].EconAlertLevel].Roll;
        //9=down roll    6 = right roll  8 = equal roll  60 == 100% right   90 = 100% left 
        //0=down 1=right
        

        switch (Side)
        {
            case 9:
                {
                    int x = (rand < .65 ? 0 : 1);
                    RollSide = x;
                    break;
                }

            case 6:
                {
                    int x = (rand < .65 ? 1 : 0);
                    RollSide = x;
                    break;
                }
            case 8:
                {
                    int x = (rand < .5 ? 1 : 0);
                    RollSide = x;
                    break;
                }
            case 60:
                {
                    RollSide = 1;
                    break;
                }
            case 90:
                {
                    RollSide = 0;
                    break;
                }

        }


    }


    void ActivateAiState()
    {
        bool SdActive = false;


        if (RollSide == 0)
        {
            int mod1 = State[situation[0].ArmyAlertLevel, situation[0].EconAlertLevel].ModuleOne_Left;
            int mod2 = State[situation[0].ArmyAlertLevel, situation[0].EconAlertLevel].ModuleTwo_Left;
            int Coms = State[situation[0].ArmyAlertLevel, situation[0].EconAlertLevel].Command_Left;
            int AuxC = State[situation[0].ArmyAlertLevel, situation[0].EconAlertLevel].AuxCommand_Left;

           // int mod1Res =
            double temp = AiResourcePool * (State[situation[0].ArmyAlertLevel, situation[0].EconAlertLevel].ProbabilityTop_Left);
            int mod1Res = System.Convert.ToInt32(temp);
            int mod2Res = AiResourcePool - mod1Res;

            Debug.Log("ROLLSIDE -> LEFT/DOWN");
            Debug.Log("Mod 1 : " + mod1);
            Debug.Log("Mod 2 : " + mod2);
//            Debug.Log("Mod 1 Resources: " + mod1Res);
//            Debug.Log("Mod 2 Resources: " + mod2Res);

            if (AuxC == 11)
            {
                SdActive = true;
                ScanDefence();
            }

            switch (mod1)
            {
                case 1:
                    {
                        
                        Defend(mod1Res);
                        break;
                    }
                case 2:
                    {
                        if (HaltEcon == true)
                        {
                            Debug.Log("Check Offline: " + Offline_to_Online + " vs " + OfflineExtractors);
                            if (Offline_to_Online == OfflineExtractors)
                            {
                                Debug.Log("Econ Module Enabled...");
                                Debug.Log("Ai Econ: " + AiEcono);
                                Debug.Log("Player: " + AdvesaryEcon);


                                InitiateEcon();
                                Offline_to_Online = 0;
                                OfflineExtractors = 0;

                                Economy(mod1Res);
                            }

                        }

                        else
                        {
                            Economy(mod1Res);
                        }
                        break;
                    }
                case 3:
                    {
                        OffenciveBuild();
                        break;
                    }
                case 4:
                    {
                        if (HaltBuild == false)
                        {
                            DefensiveBuild(mod1Res);
                        }
                        break;
                    }
            }

            switch (mod2)
            {
                case 1:
                    {

                        Defend(mod2Res);
                        break;
                    }
                case 2:
                    {
                        if (HaltEcon == true)
                        {
                            Debug.Log("Check Offline: " + Offline_to_Online + " vs " + OfflineExtractors);
                            if (Offline_to_Online == OfflineExtractors)
                            {
                                Debug.Log("Econ Module Enabled...");
                                Debug.Log("Ai Econ: " + AiEcono);
                                Debug.Log("Player: " + AdvesaryEcon);


                                InitiateEcon();
                                Offline_to_Online = 0;
                                OfflineExtractors = 0;

                                Economy(mod2Res);
                            }

                        }

                        else
                        {
                            Economy(mod2Res);
                        }
                        break;
                    }
                case 3:
                    {
                        OffenciveBuild();
                        break;
                    }
                case 4:
                    {
                        if (HaltBuild == false)
                        {
                            DefensiveBuild(mod2Res);
                        }
                        break;
                    }
            }

            switch (Coms)
            {
                case 1111:
                    {
                        ScanDefence();

                        if (SD_AttackAdvice == true)
                        {
                            Offence();
                        }
                        else { FrontLine(); }

                        break;
                    }

                case 0000:
                    {
                        FrontLine();
                        break;
                    }
            }

        }

        if (RollSide == 1)
        {
            int mod1 = State[situation[0].ArmyAlertLevel, situation[0].EconAlertLevel].ModuleOne_Right;
            int mod2 = State[situation[0].ArmyAlertLevel, situation[0].EconAlertLevel].ModuleTwo_Right;
            int Coms = State[situation[0].ArmyAlertLevel, situation[0].EconAlertLevel].Command_Right;
            int AuxC = State[situation[0].ArmyAlertLevel, situation[0].EconAlertLevel].AuxCommand_Right;

            double temp = AiResourcePool * (State[situation[0].ArmyAlertLevel, situation[0].EconAlertLevel].ProbabilityTop_Right);
            int mod1Res = System.Convert.ToInt32(temp);
            int mod2Res = AiResourcePool - mod1Res;

            Debug.Log("ROLLSIDE -> RIGHT");
            Debug.Log("Mod 1 : " + mod1);
            Debug.Log("Mod 2 : " + mod2);
            Debug.Log("Mod 1 Resources: " + mod1Res);
            Debug.Log("Mod 2 Resources: " + mod2Res);


            if (AuxC == 11)
            {
                SdActive = true;
                ScanDefence();
            }

            switch (mod1)
            {
                case 1:
                    {

                        Defend(mod1Res);
                        break;
                    }
                case 2:
                    {
                        if (HaltEcon == true)
                        {
                            Debug.Log("Check Offline: " + Offline_to_Online + " vs " + OfflineExtractors);
                            if (Offline_to_Online == OfflineExtractors)
                            {
                                Debug.Log("Econ Module Enabled...");
                                Debug.Log("Ai Econ: " + AiEcono);
                                Debug.Log("Player: " + AdvesaryEcon);


                                InitiateEcon();
                                Offline_to_Online = 0;
                                OfflineExtractors = 0;

                                Economy(mod1Res);
                            }

                        }

                        else
                        {
                            Economy(mod1Res);
                        }
                        break;
                    }
                case 3:
                    {
                        OffenciveBuild();
                        break;
                    }
                case 4:
                    {
                        if (HaltBuild == false)
                        {
                            DefensiveBuild(mod1Res);
                        }
                        break;
                    }
            }

            switch (mod2)
            {
                case 1:
                    {

                        Defend(mod2Res);
                        break;
                    }
                case 2:
                    {
                        if (HaltEcon == true)
                        {
                            Debug.Log("Check Offline: " + Offline_to_Online + " vs " + OfflineExtractors);
                            if (Offline_to_Online == OfflineExtractors)
                            {
                                Debug.Log("Econ Module Enabled...");
                                Debug.Log("Ai Econ: " + AiEcono);
                                Debug.Log("Player: " + AdvesaryEcon);


                                InitiateEcon();
                                Offline_to_Online = 0;
                                OfflineExtractors = 0;

                                Economy(mod2Res);
                            }

                        }

                        else
                        {
                            Economy(mod2Res);
                        }
                        break;
                    }
                case 3:
                    {
                        OffenciveBuild();
                        break;
                    }
                case 4:
                    {
                        if (HaltBuild == false)
                        {
                            DefensiveBuild(mod2Res);
                        }
                        break;
                    }
            }

            switch (Coms)
            {
                case 1111:
                    {
                        ScanDefence();

                        if (SD_AttackAdvice == true)
                        {
                            Offence();
                        }
                        else { FrontLine(); }
                        break;
                    }

                case 0000:
                    {
                        FrontLine();
                        break;
                    }
            }
        }
    }



    public void HoldBuild()
    {
        HaltBuild = true;
    }
    public void EconComms()
    {
        Offline_to_Online++;
    }

    public void InitiateBuild()
    {
        HaltBuild = false;
    }

    public void EconModLockdown(int offlineAmount)
    {
        Debug.Log("Extractors Offline -> " + offlineAmount);
        OfflineExtractors = offlineAmount;

        if (OfflineExtractors > 0)
        {
            HaltEcon = true;
        }
    }

    public void InitiateEcon()
    {
        Debug.Log("iNITIALIZING...");
        HaltEcon = false;
    }



    void EconSpy()
    {
        AdvesaryEcon = AdvesaryResManager.GetComponent<ResourceManagerG>().BaseEconomy;
        AiEnemyEcon.text = "Ai E_Econ: " + AdvesaryEcon;
    }

    void MilitarySpy()
    {
        Scene scene = SceneManager.GetActiveScene();

        scene.GetRootGameObjects(AllObjects);

        foreach (GameObject listed in AllObjects)
        {
            if (listed.tag == "G Mech Fighter" ||
               listed.tag == "Tank" ||
               listed.tag == "Tank2" ||
               listed.tag == "Gresource" ||
               listed.tag == "TankShG" ||
               listed.tag == "Tank4" ||
               listed.tag == "Tank3")
            {
                DetectedUnits.Add(listed);

                if (listed.tag == "Tank")
                {
 //                   Debug.Log("FOUNDT1");
                    AdvesaryMilitary += T1Points;
                }

                if (listed.tag == "Tank2")
                {
                    AdvesaryMilitary += T2Points;
                }

                if (listed.tag == "Tank3")
                {
                    AdvesaryMilitary += T3Points;
                }

                if (listed.tag == "TankShG")
                {
                    DetectedShielder = true;
                    ShielCount++;
                }

                if (listed.tag == "Tank4")
                {
                    DetectedSniper = true;
                    SnipCount++;
                }
            }
        }

        AdversaryMilitary_Carryover = AdvesaryMilitary;
        if (DetectedSniper == true)
        {
            AiEnemyMil.text = "AiMil_Spy: " + AdvesaryMilitary + " Spec: Sniper x " + SnipCount;
        }

        if (DetectedShielder == true)
        {
            AiEnemyMil.text = "AiMil_Spy: " + AdvesaryMilitary + " Spec: Shield x " + ShielCount;
        }
        if (DetectedShielder == true && DetectedSniper == true)
        {
            AiEnemyMil.text = "AiMil_Spy: " + AdvesaryMilitary + " Spec: Shield x " + ShielCount + " Spec: Sniper x " + SnipCount;
        }

        if (DetectedShielder == false && DetectedSniper == false)
        {
            AiEnemyMil.text = "AiMil_Spy: " + AdvesaryMilitary + " Spec: none";
        }


        DetectedSniper = false;
        DetectedShielder = false;
        SnipCount = 0;
        ShielCount = 0;
        AdvesaryMilitary = 0;
        AllObjects.Clear();
        DetectedUnits.Clear();

    }


   public void OffenciveDefence()
    {
        Scene scene = SceneManager.GetActiveScene();

        scene.GetRootGameObjects(AllObjects);

        foreach (GameObject listed in AllObjects)
        {
            if (listed.tag == "G Mech Fighter" ||
               listed.tag == "Tank" ||
               listed.tag == "Tank2" ||
               listed.tag == "Gresource" ||
               listed.tag == "TankShG" ||
               listed.tag == "Tank4" ||
               listed.tag == "Tank3")
            {
                DetectedUnits.Add(listed);

                if (listed.tag == "Tank" &&
                    listed.GetComponent<Movment>().getOnTrigger() == true)
                {
                    //                   Debug.Log("FOUNDT1");
                    OD_Count += T1Points;
                }

                if (listed.tag == "Tank2" &&
                    listed.GetComponent<GoodTankR2Manager>().getOnTrigger() == true)
                {
                    OD_Count += T2Points;
                }

                if (listed.tag == "Tank3" &&
                    listed.GetComponent<T3GoodManager>().getOnTrigger() == true)
                {
                    OD_Count += T3Points;
                }

                //if (listed.tag == "TankShG")
                //{
                //    DetectedShielder = true;
                //    ShielCount++;
                //}

                //if (listed.tag == "Tank4")
                //{
                //    DetectedSniper = true;
                //    SnipCount++;
                //}
            }
        }

        foreach (GameObject listed in AllObjects)
        {
            if (listed.tag == "BadTank" ||
               listed.tag == "BadTankT2" ||
               listed.tag == "BadTankT3")
            {
                DetectedUnits.Add(listed);

                if (listed.tag == "BadTank")
                {
                    UpdateMilitary += T1Points;
                }

                if (listed.tag == "BadTankT2")
                {
                    UpdateMilitary += T2Points;
                }

                if (listed.tag == "BadTankT3")
                {
                    UpdateMilitary += T3Points;
                }


            }
        }

  //      Debug.Log("ATTACKING FORCE -> " + OD_Count);
 //       Debug.Log("DEFENDING FORCE -> " + UpdateMilitary);

        if (UpdateMilitary > T1TurrentStrength ||
            UpdateMilitary == T1TurrentStrength)
        {
            Offence();
        }


        UpdateMilitary = 0;
        OD_Count = 0;
        DetectedSniper = false;
        DetectedShielder = false;
        SnipCount = 0;
        ShielCount = 0;
        AllObjects.Clear();
        DetectedUnits.Clear();

    }

    void OverseeEcon()
    {
        AiEcono = AiResManager.GetComponent<AiReources>().BaseEconomy;
        AiResourcePool = AiResManager.GetComponent<AiReources>().EnergyReserves;

        AiEcon.text = "Ai Econ: " + AiEcono + " Reserves: " + AiResourcePool;
    }

    void OverseeMilitary()
    {
        Scene scene = SceneManager.GetActiveScene();

        scene.GetRootGameObjects(AllObjects);

        foreach (GameObject listed in AllObjects)
        {
            if (listed.tag == "BadTank" ||
               listed.tag == "BadTankT2" ||
               listed.tag == "BadTankT3")
            {
                DetectedUnits.Add(listed);

                if (listed.tag == "BadTank")
                {
                    AiMilitary += T1Points;
                }

                if (listed.tag == "BadTankT2")
                {
                    AiMilitary += T2Points;
                }

                if (listed.tag == "BadTankT3")
                {
                    AiMilitary += T3Points;
                }


            }
        }

        if (DetectedSniper == true)
        {
            AiMil.text = "AiMil_Spy: " + AdvesaryMilitary + " Spec: Sniper x " + SnipCount;
        }

        if (DetectedShielder == true)
        {
            AiMil.text = "AiMil_Spy: " + AdvesaryMilitary + " Spec: Shield x " + ShielCount;
        }
        if (DetectedShielder == true && DetectedSniper == true)
        {
            AiMil.text = "AiMil_Spy: " + AdvesaryMilitary + " Spec: Shield x " + ShielCount + " Spec: Sniper x " + SnipCount;
        }

        if (DetectedShielder == false && DetectedSniper == false)
        {
            AiMil.text = "Ai Mil: " + AiMilitary + " Spec: none";
        }

        AiArmy = AiMilitary;
        DetectedSniper = false;
        DetectedShielder = false;
        SnipCount = 0;
        ShielCount = 0;
        AiMilitary = 0;
        AllObjects.Clear();
        DetectedUnits.Clear();

    }

    void FillSlotStruct()
    {
        Debug.Log("Turrent length " + TurrTemp.Length);
        for (int i = 0; i < TurrTemp.Length; i++)
        {
  //          Debug.Log("Turrent " + i);
            TurrentSlots[i].slot = TurrTemp[i];
            TurrentSlots[i].SlotNumber = i + 1;
            TurrentSlots[i].Occupied = false;
        }

        for (int i = 0; i < ExtractTemp.Length; i++)
        {
            ExtractorSlots[i].slot = ExtractTemp[i];
            ExtractorSlots[i].SlotNumber = i + 1;
            ExtractorSlots[i].Occupied = false;
//            Debug.Log(ExtractorSlots[i].slot);
        }


    }


    void PlaceTurrnet(int SlotPLace)
    {
        TurrentSlots[SlotPLace].Occupied = true;

        Vector3 tempVector = TurrentSlots[SlotPLace].slot.transform.position;

        Instantiate(Turrent, tempVector, Quaternion.identity);
        return;

    }

    void PlaceExtractor(int SlotPlace)
    {
        ExtractorSlots[SlotPlace].Occupied = true;
        GameObject tempEx = ExtractorSlots[SlotPlace].slot;
        Debug.Log(tempEx);
        Vector3 tempVector = ExtractorSlots[SlotPlace].slot.transform.position;

        Instantiate(Extractor, tempVector, Quaternion.identity);

    }


    void Defend()
    {
        for (int i = 0; i < 5; i++)
        {
            if (TurrentSlots[i].Occupied == false)
            {
                freeSlots++;
            }
        }

        if (freeSlots > 1 || freeSlots == 1)
        {
            int Diffrence = 0;
            Diffrence = AdversaryMilitary_Carryover - AiMilitary;
            
            if (Diffrence > 0)
            {
                int AmountOfTurrents = 0;

                AmountOfTurrents = Diffrence / T1TurrentStrength;

                if (AmountOfTurrents > freeSlots)
                {
                    AmountOfTurrents = freeSlots;
                }

                for (int i = 0; i < 4; i++)
                {
                    if (TurrentSlots[i].Occupied == false)
                    {
                        PlaceTurrnet(i);
                    }
                }

                freeSlots = 0;
                return;
            }
            else
            {
                freeSlots = 0;
                return;
            }
        }

        else
        {
            freeSlots = 0;
            return;
        }



    }




    void Economy(int funds)
    {
        Debug.Log("~~~~~Economy Module~~~");


        AiResManager.GetComponent<AiReources>();
        int FreeSlots = 0;

        for (int i = 0; i < ExtractorSlots.Length; i++) //Detect How Many Slots Are UnOccupied
        {
//            Debug.Log("Check Slot -> " + ExtractorSlots[i].Occupied);
            if (ExtractorSlots[i].Occupied == false)
            {
                FreeSlots++;
            }
        }

        if (FreeSlots == 0)
        {
            EconomyAdvanced();
            return;
        }

        Debug.Log("Free Slots: " + FreeSlots);

        if (FreeSlots > 0 && funds > ExtractorCost) // If Free Slots Are Sufficient do->
        {
            UnitQ.GetComponent<UnitQ_Enemy>().RecallEfficiency();

            int placedExtractors = 0;
            HaltEcon = true;

            if (Difficulty == 0) //~~~~~~~~~~~~~~~EASY
            {
                Debug.Log("~~Easy Mode Econ~~");
                float rand = Random.value;
                Debug.Log("Roll: " + rand);
                if (rand <= .6)
                {
                    for (int i = 0; i < ExtractorSlots.Length; i++)
                    {
                        Debug.Log("I: " + i);
                        if (ExtractorSlots[i].Occupied == false)
                        {
                            placedExtractors = 1;
                            Vector3 temp = new Vector3(ExtractorSlots[i].slot.transform.position.x, ExtractorSlots[i].slot.transform.position.y +.3f, ExtractorSlots[i].slot.transform.position.z);
                            GameObject ExTemp = Instantiate(Extractor, temp, Quaternion.identity);

                            ExtractorSlots[i].Occupied = true;
                            AiResManager.GetComponent<AiReources>().AiUnitRequest(ExtractorCost);

                            RegisterBuilding(ExTemp, ExtractorSlots[i].SlotNumber, 1);

                            break;
                        }
                    }
                }

                else
                {
                    HaltEcon = false;

                }

                EconModLockdown(placedExtractors);

            }


            else if (Difficulty == 1) //~~~~~~~~~~~~~~~~~~~~~~~~~~~~MED
            {
                Debug.Log("~~Medium Mode Econ~~");

                float rand = Random.value;

                Debug.Log(rand);

                if ((rand <= .4))
                {
                    if (funds < ExtractorCost * 2)
                    {
                        rand = .5f;
                    }
                }

                if ((rand <= .4) && (funds >= ExtractorCost * 2))
                {
                    Debug.Log("Spawn 2");
                    for (int i = 0; i < ExtractorSlots.Length; i++)
                    {
                        if (ExtractorSlots[i].Occupied == false)
                        {
                            placedExtractors++;
                            Vector3 temp = new Vector3(ExtractorSlots[i].slot.transform.position.x, ExtractorSlots[i].slot.transform.position.y + .3f, ExtractorSlots[i].slot.transform.position.z);

                            GameObject ExTemp = Instantiate(Extractor, temp, Quaternion.identity);
                            AiResManager.GetComponent<AiReources>().AiUnitRequest(ExtractorCost);

                            ExtractorSlots[i].Occupied = true;
                            RegisterBuilding(ExTemp, ExtractorSlots[i].SlotNumber, 1);

                        }
                        if (placedExtractors == 2)
                        {
                            Debug.Log("~~~~~~~~~~~~~");
                            break;
                        }
                    }
                }

                else if (rand <= .9 && funds >= ExtractorCost * 1)
                {
                    Debug.Log("Spawn 1");

                    for (int i = 0; i < ExtractorSlots.Length; i++)
                    {
                        if (ExtractorSlots[i].Occupied == false)
                        {
                            placedExtractors++;
                            Vector3 temp = new Vector3(ExtractorSlots[i].slot.transform.position.x, ExtractorSlots[i].slot.transform.position.y + .3f, ExtractorSlots[i].slot.transform.position.z);

                            GameObject ExTemp = Instantiate(Extractor, temp, Quaternion.identity);
                            AiResManager.GetComponent<AiReources>().AiUnitRequest(ExtractorCost);

                            ExtractorSlots[i].Occupied = true;
                            RegisterBuilding(ExTemp, ExtractorSlots[i].SlotNumber, 1);
                        }
                        if (placedExtractors == 1)
                        {
                            break;
                        }
                    }
                }

                else
                {
                    HaltEcon = false;
                    Debug.Log("Spawn none");

                }

                EconModLockdown(placedExtractors);
            }



            else if (Difficulty == 2) //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~HARD
            {
                Debug.Log("--Hard Mode Econ--");

                float rand = Random.value;
                Debug.Log(rand);
                if ((rand <= .4)) //CHECK
                {
                    if (funds >= ExtractorCost * 5)
                    {
                        //Do Nothing
                    }
                    else
                    {
                        if (rand <= .9)
                        {
                            if (funds >= ExtractorCost * 5)
                            {
                                rand = .8f;
                            }

                            else
                            {
                                rand = .95f;
                            }
                        }
                    }
                }

                else if (rand <= .9)
                {
                    if (funds >= ExtractorCost * 5)
                    {
                        //Do Nothing
                    }

                    else
                    {
                        rand = .95f;
                    }
                }                     //CHECK

                if ((rand <= .4) && (funds >= ExtractorCost * 5))
                {
//                    Debug.Log("Attempt Spawn 3");
                    for (int i = 0; i < ExtractorSlots.Length; i++)
                    {
//                        Debug.Log("Slot Check -> " + ExtractorSlots[i].Occupied);
                        if (ExtractorSlots[i].Occupied == false)
                        {
                            placedExtractors++;

 //                           Debug.Log("Spawn:" +  placedExtractors);
                            Vector3 temp = new Vector3(ExtractorSlots[i].slot.transform.position.x, ExtractorSlots[i].slot.transform.position.y + .3f, ExtractorSlots[i].slot.transform.position.z);

                            GameObject ExTemp = Instantiate(Extractor, temp, Quaternion.identity);
                            AiResManager.GetComponent<AiReources>().AiUnitRequest(ExtractorCost);

                            ExtractorSlots[i].Occupied = true;
                            RegisterBuilding(ExTemp, ExtractorSlots[i].SlotNumber, 1);
                        }
                        if (placedExtractors == 5)
                        {
                            break;
                        }
                    }
                }

                else if (rand <= .9 && funds >= ExtractorCost * 4)
                {
                    Debug.Log("Attempt Spawn 2");
                    for (int i = 0; i < ExtractorSlots.Length; i++)
                    {
                        Debug.Log("Slot Check -> " + ExtractorSlots[i].Occupied);

                        if (ExtractorSlots[i].Occupied == false)
                        {
                            placedExtractors++;
                            Vector3 temp = new Vector3(ExtractorSlots[i].slot.transform.position.x, ExtractorSlots[i].slot.transform.position.y + .3f, ExtractorSlots[i].slot.transform.position.z);
                            Debug.Log("Spawn:" + placedExtractors);

                            GameObject ExTemp = Instantiate(Extractor, temp, Quaternion.identity);
                            AiResManager.GetComponent<AiReources>().AiUnitRequest(ExtractorCost);

                            ExtractorSlots[i].Occupied = true;
                            RegisterBuilding(ExTemp, ExtractorSlots[i].SlotNumber, 1);
                        }
                        if (placedExtractors == 5)
                        {
                            break;
                        }
                    }
                }

                else
                {
                    Debug.Log("Attempt Spawn 1");

                    for (int i = 0; i < ExtractorSlots.Length; i++)
                    {
                        Debug.Log("Slot Check -> " + ExtractorSlots[i].Occupied);

                        if (ExtractorSlots[i].Occupied == false)
                        {
                            placedExtractors = 1;
                            Vector3 temp = new Vector3(ExtractorSlots[i].slot.transform.position.x, ExtractorSlots[i].slot.transform.position.y + .3f, ExtractorSlots[i].slot.transform.position.z);
                            Debug.Log("Spawn:" + placedExtractors);

                            GameObject ExTemp = Instantiate(Extractor, temp, Quaternion.identity);
                            AiResManager.GetComponent<AiReources>().AiUnitRequest(ExtractorCost);

                            ExtractorSlots[i].Occupied = true;
                            RegisterBuilding(ExTemp, ExtractorSlots[i].SlotNumber, 1);
                            break;
                        }

                    }
                }

                EconModLockdown(placedExtractors);

            }



        }

        else
        {
            FreeSlots = 0;
            HaltEcon = false;
            return;
        }
    }

   void EconomyAdvanced()
    {
        Debug.Log("~~~~Economy Advanced~~~~");
        Scene scene = SceneManager.GetActiveScene();
        scene.GetRootGameObjects(AllObjects);

        int i = 0;
        int tempTier = 1;
        int LowestTier = 6;
        int MaxTier = 6;
        int MaxCounter = 0;

        foreach (GameObject listed in AllObjects)
        {
            if (listed.tag == "BadResourceCollector")
            {
                AdvExtractorUp[i].theExtractor = listed;
                AdvExtractorUp[i].CurrentTier = listed.GetComponent<AiResourceExtractor>().getTier();
                Debug.Log(listed.GetComponent<AiResourceExtractor>().getTier());
                tempTier = listed.GetComponent<AiResourceExtractor>().getTier();

                if (LowestTier > tempTier)
                {
                    LowestTier = tempTier;
                    Debug.Log("Lowest Tier: " + LowestTier);
                }

                if (tempTier == 6)
                {
                    MaxCounter++;
                }

                i++;
            }



            if (MaxCounter == 5)
            {
                UnitQ.GetComponent<UnitQ_Enemy>().ExtractorEfficiency(LowestTier);
                Debug.Log("<No More Extractor Upgrades>");
                return;
            }


        }

        UnitQ.GetComponent<UnitQ_Enemy>().ExtractorEfficiency(LowestTier);

        int NumberToUpgrade = 0;
        switch (Difficulty)
        {
            case 0:
                {
                    Debug.Log("Easy Adv Upgrade");
                    float rand = Random.value;
                    if ( rand<.5 || rand ==.5) { NumberToUpgrade = 1; }
                    else { NumberToUpgrade = 0; }

                    break;
                }
            case 1:
                {
                    Debug.Log("Medium Adv Upgrade");
                    float rand = Random.value;
                    if (rand < .34 ) { NumberToUpgrade = 0; }
                    if (rand < .66 ) { NumberToUpgrade = 1; }
                    else { NumberToUpgrade = 2; }
                    break;
                }
            case 2:
                {
                    Debug.Log("Hard Adv Upgrade");
                    float rand = Random.value;
                    if (rand < .5 ) { NumberToUpgrade = 5; }
                    else { NumberToUpgrade = 3; }
                    Debug.Log("Rand: " + rand);

                    break;
                }
        }

        int UpgradedSoFar = 0;
        bool canContinue = true;
        Debug.Log("Attempt Place");

        for (int v =0; v < AdvExtractorUp.Length; v++)
        {
            if (LowestTier == 6) { break; }
            Debug.Log(AdvExtractorUp[v].CurrentTier + " vs " + LowestTier);

            if (AdvExtractorUp[v].CurrentTier == LowestTier && canContinue == true && AdvExtractorUp[v].theExtractor.GetComponent<AiResourceExtractor>().isUpgrade == false)
            {
                Debug.Log("Place at: " + v);

                canContinue = AdvExtractorUp[v].theExtractor.GetComponent<AiResourceExtractor>().UpgradeExtractor();
                if (canContinue == false) { break; }
                else { UpgradedSoFar++; }
            }

            if (UpgradedSoFar == NumberToUpgrade) { EconModLockdown(UpgradedSoFar); break; }

            if (UpgradedSoFar != NumberToUpgrade && v == (AdvExtractorUp.Length - 1)) { EconModLockdown(UpgradedSoFar); LowestTier++; v = 0; }

        }



    }




    void DefensiveBuild(int funds)       //ALLOWANCE UNIT BUILD
    {

        Debug.Log("---Defensive Build Module---");



        if (Difficulty == 0)
        {
            Debug.Log("Easy Difficulty Build Module....");


                float rand = Random.value;

                Debug.Log("1 Random # -> " + rand);
                if (rand < .5 || rand == .5) //IF RANDOM NUMBER IS FROM 0-5 -> T1  (1,2)
                {
                    IdInfo[0].ID = 1;
                    Debug.Log("in Arr: " + IdInfo[0].ID);


                    rand = Random.value;
                    Debug.Log("2 Random # -> " + rand);

                    if (rand < .6 || rand == .6) //T2
                    {

                        IdInfo[0].SpaawnPlan = 1; //1
                        IdInfo[1].ID = 2;
                        Debug.Log("in Arr: " + IdInfo[1].ID);

                        IdInfo[2].ID = 3;
                        Debug.Log("in Arr: " + IdInfo[2].ID);

                    }
                    else //T3
                    {

                        IdInfo[0].SpaawnPlan = 2; //2
                        IdInfo[1].ID = 3;
                        Debug.Log("in Arr: " + IdInfo[1].ID);

                        IdInfo[2].ID = 2;
                        Debug.Log("in Arr: " + IdInfo[2].ID);

                    }
                }


                else if (rand < .8 || rand == .8) //IF RANDOM NUMBER IS FROM 6-8 ->T2 (3,4)
                {

                    IdInfo[0].ID = 2;
                    Debug.Log("in Arr: " + IdInfo[0].ID);

                    rand = Random.value;
                    Debug.Log("2 Random # -> " + rand);

                    if (rand < .7142 || rand == .7142) //T1
                    {

                        IdInfo[0].SpaawnPlan = 3; //3
                        IdInfo[1].ID = 1;
                        Debug.Log("in Arr: " + IdInfo[1].ID);

                        IdInfo[2].ID = 3;
                        Debug.Log("in Arr: " + IdInfo[2].ID);

                    }
                    else //T3
                    {

                        IdInfo[0].SpaawnPlan = 4; //4
                        IdInfo[1].ID = 3;
                        Debug.Log("in Arr: " + IdInfo[1].ID);

                        IdInfo[2].ID = 1;
                        Debug.Log("in Arr: " + IdInfo[2].ID);

                    }
                }
                else                  // IF RANDOM NUMBER IS FROM 9-10 -> T3 (5,6)
                {

                    IdInfo[0].ID = 3;

                    Debug.Log("in Arr: " + IdInfo[0].ID);


                    rand = Random.value;
                    Debug.Log("2 Random # -> " + rand);

                    if (rand < .7142 || rand == .7142) //T1
                    {

                        IdInfo[0].SpaawnPlan = 5; //5
                        IdInfo[1].ID = 1;
                        Debug.Log("in Arr: " + IdInfo[1].ID);

                        IdInfo[2].ID = 2;
                        Debug.Log("in Arr: " + IdInfo[2].ID);

                    }
                    else  //T2
                    {

                        IdInfo[0].SpaawnPlan = 6; //6
                        IdInfo[1].ID = 2;
                        Debug.Log("in Arr: " + IdInfo[1].ID);

                        IdInfo[2].ID = 1;
                        Debug.Log("in Arr: " + IdInfo[2].ID);

                    }
                }

            
        }

        else if (Difficulty == 1) //DIFFICULTY MEDIUM
        {
            Debug.Log("Medium Difficulty Build Module....");

                float rand = Random.value;

                Debug.Log("1 Random # -> " + rand);
                if (rand < .34 || rand == .34) //IF RANDOM NUMBER IS FROM 0-5 -> T1  (1,2)
                {
                    IdInfo[0].ID = 3;
                    Debug.Log("in Arr: " + IdInfo[0].ID);


                    rand = Random.value;
                    Debug.Log("2 Random # -> " + rand);

                    if (rand < .5 || rand == .5) //T2
                    {

                        IdInfo[1].ID = 2;
                        Debug.Log("in Arr: " + IdInfo[1].ID);

                        IdInfo[2].ID = 1;
                        Debug.Log("in Arr: " + IdInfo[2].ID);

                    }
                    else //T3
                    {

                        IdInfo[0].SpaawnPlan = 1; //2
                        IdInfo[1].ID = 1;
                        Debug.Log("in Arr: " + IdInfo[1].ID);

                        IdInfo[2].ID = 2;
                        Debug.Log("in Arr: " + IdInfo[2].ID);

                    }
                }


                else if (rand < .66 || rand == .66) //IF RANDOM NUMBER IS FROM 6-8 ->T2 (3,4)
                {

                    IdInfo[0].ID = 2;
                    Debug.Log("in Arr: " + IdInfo[0].ID);

                    rand = Random.value;
                    Debug.Log("2 Random # -> " + rand);

                    if (rand < .5 || rand == .5) //T1
                    {

                        IdInfo[0].SpaawnPlan = 3; //3
                        IdInfo[1].ID = 1;
                        Debug.Log("in Arr: " + IdInfo[1].ID);

                        IdInfo[2].ID = 3;
                        Debug.Log("in Arr: " + IdInfo[2].ID);

                    }
                    else //T3
                    {

                        IdInfo[1].ID = 3;
                        Debug.Log("in Arr: " + IdInfo[1].ID);

                        IdInfo[2].ID = 1;
                        Debug.Log("in Arr: " + IdInfo[2].ID);

                    }
                }
                else                  // IF RANDOM NUMBER IS FROM 9-10 -> T3 (5,6)
                {

                    IdInfo[0].ID = 1;

                    Debug.Log("in Arr: " + IdInfo[0].ID);


                    rand = Random.value;
                    Debug.Log("2 Random # -> " + rand);

                    if (rand < .5 || rand == .5) //T1
                    {

                        IdInfo[1].ID = 3;
                        Debug.Log("in Arr: " + IdInfo[1].ID);

                        IdInfo[2].ID = 2;
                        Debug.Log("in Arr: " + IdInfo[2].ID);

                    }
                    else  //T2
                    {

                        IdInfo[1].ID = 2;
                        Debug.Log("in Arr: " + IdInfo[1].ID);

                        IdInfo[2].ID = 3;
                        Debug.Log("in Arr: " + IdInfo[2].ID);

                    }
                }

            
        }

        else
        {
            Debug.Log("Hard Priority");
            IdInfo[0].ID = 3;
            IdInfo[1].ID = 2;
            IdInfo[2].ID = 1;

        }
        Debug.Log("[AI Unit Priority]");
        Debug.Log("Id [0]: " + IdInfo[0].ID);
        Debug.Log("Id [1]: " + IdInfo[1].ID);
        Debug.Log("Id [2]: " + IdInfo[2].ID);



        int t1co = AiSpawner.GetComponent<AiSpawnManager>().t1cost;
        int t2co = AiSpawner.GetComponent<AiSpawnManager>().t2cost;
        int t3co = AiSpawner.GetComponent<AiSpawnManager>().t3cost;

        int MaxType = 0;

        int ProjectedAmount = 0;

        for (int i = 0; i < IdInfo.Length; i++)
        {
            if (IdInfo[i].ID == 1) // Priority 1
            {
                //TempAmount = ArmyDeficit / T1Strength;
                //ArmyDeficit = ArmyDeficit - (TempAmount * T1Strength);
                MaxType = funds / t1co;
                ProjectedAmount = t1co * MaxType;
                funds = funds - ProjectedAmount;
                SpawnList[0].AmountOfUnit = MaxType;

                MaxType = 0;
                ProjectedAmount = 0;
            }

            if (IdInfo[i].ID == 2) //Priority 2
            {
                MaxType = funds / t2co;
                ProjectedAmount = t2co * MaxType;
                funds = funds - ProjectedAmount;

                SpawnList[1].AmountOfUnit = MaxType;

                MaxType = 0;
                ProjectedAmount = 0;
            }

            if (IdInfo[i].ID == 3) //Priority 3
            {
                MaxType = funds / t3co;
                ProjectedAmount = t3co * MaxType;
                funds = funds - ProjectedAmount;

                SpawnList[2].AmountOfUnit = MaxType;

                MaxType = 0;
                ProjectedAmount = 0;
            }

        }

        int T1_Totals = SpawnList[0].AmountOfUnit;
        int T2_Totals = SpawnList[1].AmountOfUnit;
        int T3_Totals = SpawnList[2].AmountOfUnit;

        int PackagePrice = (T1_Totals * t1co) + (T2_Totals * t2co) + (T3_Totals * t3co);


        Debug.Log("===--Outcome--===");
        Debug.Log("T1 Package: " + T1_Totals);
        Debug.Log("T2 Package: " + T2_Totals);
        Debug.Log("T3 Package: " + T3_Totals);


        ContactSpawn(T1_Totals, T2_Totals, T3_Totals, PackagePrice);


    }


    void OffenciveBuild()
    {

        Debug.Log("---Offensive Module---");



        if (Difficulty == 0)
        {
            Debug.Log("Hard Difficulty Offensive Module....");


            float rand = Random.value;

            Debug.Log("1 Random # -> " + rand);
            if (rand < .5 || rand == .5) //IF RANDOM NUMBER IS FROM 0-5 -> T1  (1,2)
            {
                IdInfo[0].ID = 1;
                Debug.Log("in Arr: " + IdInfo[0].ID);


                rand = Random.value;
                Debug.Log("2 Random # -> " + rand);

                if (rand < .6 || rand == .6) //T2
                {

                    IdInfo[0].SpaawnPlan = 1; //1
                    IdInfo[1].ID = 2;
                    Debug.Log("in Arr: " + IdInfo[1].ID);

                    IdInfo[2].ID = 3;
                    Debug.Log("in Arr: " + IdInfo[2].ID);

                }
                else //T3
                {

                    IdInfo[0].SpaawnPlan = 2; //2
                    IdInfo[1].ID = 3;
                    Debug.Log("in Arr: " + IdInfo[1].ID);

                    IdInfo[2].ID = 2;
                    Debug.Log("in Arr: " + IdInfo[2].ID);

                }
            }


            else if (rand < .8 || rand == .8) //IF RANDOM NUMBER IS FROM 6-8 ->T2 (3,4)
            {

                IdInfo[0].ID = 2;
                Debug.Log("in Arr: " + IdInfo[0].ID);

                rand = Random.value;
                Debug.Log("2 Random # -> " + rand);

                if (rand < .7142 || rand == .7142) //T1
                {

                    IdInfo[0].SpaawnPlan = 3; //3
                    IdInfo[1].ID = 1;
                    Debug.Log("in Arr: " + IdInfo[1].ID);

                    IdInfo[2].ID = 3;
                    Debug.Log("in Arr: " + IdInfo[2].ID);

                }
                else //T3
                {

                    IdInfo[0].SpaawnPlan = 4; //4
                    IdInfo[1].ID = 3;
                    Debug.Log("in Arr: " + IdInfo[1].ID);

                    IdInfo[2].ID = 1;
                    Debug.Log("in Arr: " + IdInfo[2].ID);

                }
            }
            else                  // IF RANDOM NUMBER IS FROM 9-10 -> T3 (5,6)
            {

                IdInfo[0].ID = 3;

                Debug.Log("in Arr: " + IdInfo[0].ID);


                rand = Random.value;
                Debug.Log("2 Random # -> " + rand);

                if (rand < .7142 || rand == .7142) //T1
                {

                    IdInfo[0].SpaawnPlan = 5; //5
                    IdInfo[1].ID = 1;
                    Debug.Log("in Arr: " + IdInfo[1].ID);

                    IdInfo[2].ID = 2;
                    Debug.Log("in Arr: " + IdInfo[2].ID);

                }
                else  //T2
                {

                    IdInfo[0].SpaawnPlan = 6; //6
                    IdInfo[1].ID = 2;
                    Debug.Log("in Arr: " + IdInfo[1].ID);

                    IdInfo[2].ID = 1;
                    Debug.Log("in Arr: " + IdInfo[2].ID);

                }
            }


        }

        else if (Difficulty == 1) //DIFFICULTY MEDIUM
        {
            Debug.Log("Medium Difficulty Offencive Build Module....");

            float rand = Random.value;

            Debug.Log("1 Random # -> " + rand);
            if (rand < .34 || rand == .34) //IF RANDOM NUMBER IS FROM 0-5 -> T1  (1,2)
            {
                IdInfo[0].ID = 3;
                Debug.Log("in Arr: " + IdInfo[0].ID);


                rand = Random.value;
                Debug.Log("2 Random # -> " + rand);

                if (rand < .5 || rand == .5) //T2
                {

                    IdInfo[1].ID = 2;
                    Debug.Log("in Arr: " + IdInfo[1].ID);

                    IdInfo[2].ID = 1;
                    Debug.Log("in Arr: " + IdInfo[2].ID);

                }
                else //T3
                {

                    IdInfo[0].SpaawnPlan = 1; //2
                    IdInfo[1].ID = 1;
                    Debug.Log("in Arr: " + IdInfo[1].ID);

                    IdInfo[2].ID = 2;
                    Debug.Log("in Arr: " + IdInfo[2].ID);

                }
            }


            else if (rand < .66 || rand == .66) //IF RANDOM NUMBER IS FROM 6-8 ->T2 (3,4)
            {

                IdInfo[0].ID = 2;
                Debug.Log("in Arr: " + IdInfo[0].ID);

                rand = Random.value;
                Debug.Log("2 Random # -> " + rand);

                if (rand < .5 || rand == .5) //T1
                {

                    IdInfo[0].SpaawnPlan = 3; //3
                    IdInfo[1].ID = 1;
                    Debug.Log("in Arr: " + IdInfo[1].ID);

                    IdInfo[2].ID = 3;
                    Debug.Log("in Arr: " + IdInfo[2].ID);

                }
                else //T3
                {

                    IdInfo[1].ID = 3;
                    Debug.Log("in Arr: " + IdInfo[1].ID);

                    IdInfo[2].ID = 1;
                    Debug.Log("in Arr: " + IdInfo[2].ID);

                }
            }
            else                  // IF RANDOM NUMBER IS FROM 9-10 -> T3 (5,6)
            {

                IdInfo[0].ID = 1;

                Debug.Log("in Arr: " + IdInfo[0].ID);


                rand = Random.value;
                Debug.Log("2 Random # -> " + rand);

                if (rand < .5 || rand == .5) //T1
                {

                    IdInfo[1].ID = 3;
                    Debug.Log("in Arr: " + IdInfo[1].ID);

                    IdInfo[2].ID = 2;
                    Debug.Log("in Arr: " + IdInfo[2].ID);

                }
                else  //T2
                {

                    IdInfo[1].ID = 2;
                    Debug.Log("in Arr: " + IdInfo[1].ID);

                    IdInfo[2].ID = 3;
                    Debug.Log("in Arr: " + IdInfo[2].ID);

                }
            }


        }

        else
        {
            IdInfo[0].ID = 3;
            IdInfo[1].ID = 2;
            IdInfo[2].ID = 1;

        }
        Debug.Log("[AI Unit Priority]");
        Debug.Log("Id [0]: " + IdInfo[0].ID);
        Debug.Log("Id [1]: " + IdInfo[1].ID);
        Debug.Log("Id [2]: " + IdInfo[2].ID);



        int funds = AiResourceScript.GetComponent<AiReources>().EnergyReserves;
        int t1co = AiSpawner.GetComponent<AiSpawnManager>().t1cost;
        int t2co = AiSpawner.GetComponent<AiSpawnManager>().t2cost;
        int t3co = AiSpawner.GetComponent<AiSpawnManager>().t3cost;

        int MaxType = 0;

        int ProjectedAmount = 0;

        for (int i = 0; i < IdInfo.Length; i++)
        {
            if (IdInfo[i].ID == 1) // Priority 1
            {
                //TempAmount = ArmyDeficit / T1Strength;
                //ArmyDeficit = ArmyDeficit - (TempAmount * T1Strength);
                MaxType = funds / t1co;
                ProjectedAmount = t1co * MaxType;
                funds = funds - ProjectedAmount;
                SpawnList[0].AmountOfUnit = MaxType;

                MaxType = 0;
                ProjectedAmount = 0;
            }

            if (IdInfo[i].ID == 2) //Priority 2
            {
                MaxType = funds / t2co;
                ProjectedAmount = t2co * MaxType;
                funds = funds - ProjectedAmount;

                SpawnList[1].AmountOfUnit = MaxType;

                MaxType = 0;
                ProjectedAmount = 0;
            }

            if (IdInfo[i].ID == 3) //Priority 3
            {
                MaxType = funds / t3co;
                ProjectedAmount = t3co * MaxType;
                funds = funds - ProjectedAmount;

                SpawnList[2].AmountOfUnit = MaxType;

                MaxType = 0;
                ProjectedAmount = 0;
            }

        }

        int T1_Totals = SpawnList[0].AmountOfUnit;
        int T2_Totals = SpawnList[1].AmountOfUnit;
        int T3_Totals = SpawnList[2].AmountOfUnit;

        int PackagePrice = (T1_Totals * t1co) + (T2_Totals * t2co) + (T3_Totals * t3co);


        Debug.Log("===--Outcome--===");
        Debug.Log("T1 Package: " + T1_Totals);
        Debug.Log("T2 Package: " + T2_Totals);
        Debug.Log("T3 Package: " + T3_Totals);


        ContactSpawn(T1_Totals, T2_Totals, T3_Totals, PackagePrice);


    }



    void ContactSpawn(int t1T, int t2T, int t3T, int Total)
    {
//        Debug.Log("PACKAGE SENT");
        AiSpawner.GetComponent<AiSpawnManager>().AiSpawnCall(t1T, t2T, t3T, Total);
    }


    void FrontLine()
    {
        Debug.Log("******FRONTLINES******");
        Scene scene = SceneManager.GetActiveScene();

        scene.GetRootGameObjects(AllObjects);

        foreach (GameObject listed in AllObjects)
        {
            if (listed.tag == "BadTank" ||
               listed.tag == "BadTankT2" ||
               listed.tag == "BadTankT3" 
                )
            {
                DetectedUnits.Add(listed);

                if (listed.tag == "BadTank")
                {
//                    Debug.Log("T1 to Frontlines...");
                    listed.GetComponent<EvilTankManager>().AiMoveCommand(FrontLineCoord);
                }

                if (listed.tag == "BadTankT2")
                {
                    listed.GetComponent<Tier2EvilManager>().AiMoveCommand(FrontLineCoord);
                }

                if (listed.tag == "BadTankT3")
                {
                    listed.GetComponent<eT3Manager>().AiMoveCommand(FrontLineCoord);
                }


            }
        }

        AllObjects.Clear();
        DetectedUnits.Clear();
    }


    void Defend(int funds)
    {
        Debug.Log("--=Defend Module=--");

        AiResManager.GetComponent<AiReources>();
        int FreeSlots = 0;
        for (int i = 0; i < TurrentSlots.Length; i++)
        {
 //           Debug.Log("Slot Taken-> "+ TurrentSlots[i].Occupied );

            if (TurrentSlots[i].Occupied == false)
            {
                FreeSlots++;
            }
        }

        if (FreeSlots == 0)
        {

            if (HaltEcon == true)
            {
                Debug.Log("Check Offline: " + Offline_to_Online + " vs " + OfflineExtractors);
                if (Offline_to_Online == OfflineExtractors)
                {
                    Debug.Log("Econ Module Enabled...");
                    Debug.Log("Ai Econ: " + AiEcono);
                    Debug.Log("Player: " + AdvesaryEcon);


                    InitiateEcon();
                    Offline_to_Online = 0;
                    OfflineExtractors = 0;

            Economy(funds);
                }

            }

            else
            {
                Economy(funds);
            }
        }

        Debug.Log("Funds: " + funds + " Slots: " + FreeSlots);
        if (FreeSlots > 0 && funds > TurrentCost)
        {
            Debug.Log("Parameters Accepted...");
            if (Difficulty == 0) //~~~~~~~~~~~~~~~EASY
            {
                Debug.Log("--Easy Mode Defend--");
                float rand = Random.value;
                Debug.Log("Roll: " + rand);
                if (rand <= .6)
                {
                    for (int i = 0; i < TurrentSlots.Length; i++)
                    {
                        if (TurrentSlots[i].Occupied == false)
                        {
                            Vector3 temp = new Vector3(TurrentSlots[i].slot.transform.position.x, TurrentSlots[i].slot.transform.position.y + .85f, TurrentSlots[i].slot.transform.position.z);
                            GameObject TurrTemp =  Instantiate(Turrent, temp, Quaternion.identity);
                            TurrentSlots[i].Occupied = true;
                            AiResManager.GetComponent<AiReources>().AiUnitRequest(50);

                            RegisterBuilding(TurrTemp, TurrentSlots[i].SlotNumber, 0);

                            break;
                        }
                    }
                }

                else
                {

                }
            }

            else if (Difficulty == 1) //~~~~~~~~~~~~~~~~~~~~~~~~~~~~MED
            {
                Debug.Log("--Medium Mode Defend--");

                float rand = Random.value;

                Debug.Log(rand);
                if ((rand <= .4))
                {
                    if (funds < TurrentCost * 2)
                    {
                        rand = .5f;
                    }
                }

                if ((rand <= .4) && (funds >= TurrentCost * 2))
                {
                    int placed = 0;
                    Debug.Log("Spawn 2");
                    for (int i = 0; i < TurrentSlots.Length; i++)
                    {
                        if (TurrentSlots[i].Occupied == false)
                        {
                            placed++;
                            Vector3 temp = new Vector3(TurrentSlots[i].slot.transform.position.x, TurrentSlots[i].slot.transform.position.y + .85f, TurrentSlots[i].slot.transform.position.z);

                            GameObject TurrTemp = Instantiate(Turrent, temp, Quaternion.identity);
                            AiResManager.GetComponent<AiReources>().AiUnitRequest(50);

                            TurrentSlots[i].Occupied = true;
                            RegisterBuilding(TurrTemp, TurrentSlots[i].SlotNumber, 0);

                        }
                        if (placed == 2)
                        {
                            break;
                        }
                    }
                }

                else if (rand <= .9 && funds >= TurrentCost * 1)
                {
                    Debug.Log("Spawn 1");

                    int placed = 0;
                    for (int i = 0; i < TurrentSlots.Length; i++)
                    {
                        if (TurrentSlots[i].Occupied == false)
                        {
                            placed++;
                            Vector3 temp = new Vector3(TurrentSlots[i].slot.transform.position.x, TurrentSlots[i].slot.transform.position.y + .85f, TurrentSlots[i].slot.transform.position.z);

                            GameObject TurrTemp = Instantiate(Turrent, temp, Quaternion.identity);
                            AiResManager.GetComponent<AiReources>().AiUnitRequest(50);

                            TurrentSlots[i].Occupied = true;
                            RegisterBuilding(TurrTemp, TurrentSlots[i].SlotNumber, 0);
                        }
                        if (placed == 1)
                        {
                            break;
                        }
                    }
                }

                else
                {
                    Debug.Log("Spawn none");

                }

            }

            else if (Difficulty == 2) //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~HARD
            {
                Debug.Log("--Hard Mode Defend--");

                float rand = Random.value;

                if ((rand <= .4)) //CHECK
                {
                    if (funds >= TurrentCost * 3)
                    {
                        //Do Nothing
                    }
                    else
                    {
                        if (rand <= .9)
                        {
                            if (funds >= TurrentCost * 2)
                            {
                                rand = .8f;
                            }

                            else
                            {
                                rand = .95f;
                            }
                        }
                    }
                }

                else if (rand <= .9)
                {
                    if (funds >= TurrentCost * 2)
                    {
                        //Do Nothing
                    }

                    else
                    {
                        rand = .95f;
                    }
                }                     //CHECK

                if ((rand <= .4) && (funds >= TurrentCost * 3))
                {
                    int placed = 0;
                    for (int i = 0; i < TurrentSlots.Length; i++)
                    {
                        if (TurrentSlots[i].Occupied == false)
                        {
                            placed++;
                            Vector3 temp = new Vector3(TurrentSlots[i].slot.transform.position.x, TurrentSlots[i].slot.transform.position.y + .85f, TurrentSlots[i].slot.transform.position.z);

                            GameObject TurrTemp = Instantiate(Turrent, temp, Quaternion.identity);
                            AiResManager.GetComponent<AiReources>().AiUnitRequest(TurrentCost);

                            TurrentSlots[i].Occupied = true;
                            RegisterBuilding(TurrTemp, TurrentSlots[i].SlotNumber, 0);
                        }
                        if (placed == 3)
                        {
                            break;
                        }
                    }
                }

                else if (rand <= .9 && funds >= TurrentCost * 2)
                {
                    int placed = 0;
                    for (int i = 0; i < TurrentSlots.Length; i++)
                    {
                        if (TurrentSlots[i].Occupied == false)
                        {
                            placed++;
                            Vector3 temp = new Vector3(TurrentSlots[i].slot.transform.position.x, TurrentSlots[i].slot.transform.position.y + .85f, TurrentSlots[i].slot.transform.position.z);

                            GameObject TurrTemp = Instantiate(Turrent, temp, Quaternion.identity);
                            AiResManager.GetComponent<AiReources>().AiUnitRequest(TurrentCost);

                            TurrentSlots[i].Occupied = true;
                            RegisterBuilding(TurrTemp, TurrentSlots[i].SlotNumber, 0);
                        }
                        if (placed == 2)
                        {
                            break;
                        }
                    }
                }

                else
                {
                    int placed = 0;
                    for (int i = 0; i < TurrentSlots.Length; i++)
                    {
                        if (TurrentSlots[i].Occupied == false)
                        {
                            placed++;
                            Vector3 temp = new Vector3(TurrentSlots[i].slot.transform.position.x, TurrentSlots[i].slot.transform.position.y + .85f, TurrentSlots[i].slot.transform.position.z);

                            GameObject TurrTemp = Instantiate(Turrent, temp, Quaternion.identity);
                            AiResManager.GetComponent<AiReources>().AiUnitRequest(TurrentCost);

                            TurrentSlots[i].Occupied = true;
                            RegisterBuilding(TurrTemp, TurrentSlots[i].SlotNumber, 0);
                            break;
                        }

                    }
                }
            }

            else
                return;
        }
    }

    void SetFrontline()
    {
        bool hasTurrents = false;
        int SlotNumber = 0;
        for (int  i = 0; i < TurrentSlots.Length; i++)
        {
            if (TurrentSlots[i].Occupied == true)
            {
                Vector3 keepy = new Vector3(TurrentSlots[i].slot.transform.position.x, AiSpawner.transform.position.y,
                                            AiSpawner.transform.position.z);

                FrontLineCoord = keepy;
                SlotNumber = i + 1;

                hasTurrents = true;
            }
        }

        Debug.Log("New Front Line Slot: " + SlotNumber);

        if (hasTurrents == true)
        {
            if (SlotNumber != 0)
            {
                AiMovmentFrontLineUodate(FrontLineCoord);
            }
            else
            {
                FrontLineCoord = AiSpawner.transform.position;
                AiMovmentFrontLineUodate(FrontLineCoord);

            }
        }

        else
        {
            for (int i = 0; i < TurrentSlots.Length; i++)
            {
                if (ExtractorSlots[i].Occupied == true)
                {
                    Vector3 keepy = new Vector3(ExtractorSlots[i].slot.transform.position.x, AiSpawner.transform.position.y,
                                                AiSpawner.transform.position.z);

                    FrontLineCoord = keepy;
                    SlotNumber = i + 1;

                    hasTurrents = true;
                }
            }

            if (SlotNumber != 0)
            {
                AiMovmentFrontLineUodate(FrontLineCoord);
            }
            else
            {
                FrontLineCoord = AiSpawner.transform.position;
                AiMovmentFrontLineUodate(FrontLineCoord);

            }
        }
    }

    void AiMovmentFrontLineUodate(Vector3 lines) //Called in SetFrontLine
    {
        Debug.Log("Updating Ai Movment Manager...");
        AiMovmentManager.GetComponent<AiMovmentState>().UpdateFrontLine(lines);
        AiMovmentManager.GetComponent<AiMovmentState>().ChangeMovmentState(false);
    }

    void ScanDefence()
    {
        int AdversaryTurrent_Potential = 0;
        Debug.Log("AI mIL - > " + AdversaryMilitary_Carryover);



        if (ScanDefFirst == false)
        {
            Scene scene = SceneManager.GetActiveScene();

            scene.GetRootGameObjects(AllObjects);

            foreach (GameObject listed in AllObjects)
            {
                if (listed.tag == "Turr")
                {
                    AdversaryTurrent_Potential += 90;
                }
            }

            int TotalPlayerDefensiveStrenght = AdversaryMilitary_Carryover + AdversaryTurrent_Potential;
            int PlayerPriority = AdversaryMilitary_Carryover + 100;

            //  ScannedDefenseDiffrence = TotalPlayerDefensiveStrenght - AiArmy;
            ScannedDefenseDiffrence = PlayerPriority - AiArmy;

            if (ScannedDefenseDiffrence < -10)
            {
                SD_AttackAdvice = true;
            }
            else if (ScannedDefenseDiffrence > -10 && ScannedDefenseDiffrence < 0)
            {
                SD_AttackAdvice = true;
            }
            else if (ScannedDefenseDiffrence == 0)
            {
                SD_AttackAdvice = true;
            }
            else if (ScannedDefenseDiffrence < 10 && ScannedDefenseDiffrence > 0)
            {
                SD_AttackAdvice = false;
            }
            else if (ScannedDefenseDiffrence > 10)
            {
                SD_AttackAdvice = false;
            }

            Debug.Log("Ai Offnc Str: " + AiMilitary + " Player Def Str: " + TotalPlayerDefensiveStrenght);
            Debug.Log("[Advisor] Shoudl Attack: " + SD_AttackAdvice);
        }

        else if (ScanDefFirst == true)
        {
            SD_AttackAdvice = false;

            ScanDefFirst = false;
        }



    }



    void Offence()
    {
 //       Debug.Log("******OFFENCE******");
        Scene scene = SceneManager.GetActiveScene();

        scene.GetRootGameObjects(AllObjects);

        foreach (GameObject listed in AllObjects)
        {
            if (listed.tag == "BadTank" ||
               listed.tag == "BadTankT2" ||
               listed.tag == "BadTankT3"
                )
            {
                DetectedUnits.Add(listed);

                if (listed.tag == "BadTank")
                {
               
                    listed.GetComponent<EvilTankManager>().AiOffenceCommand();
                }

                if (listed.tag == "BadTankT2")
                {
                    listed.GetComponent<Tier2EvilManager>().AiOffenceCommand();
                }

                if (listed.tag == "BadTankT3")
                {
                    listed.GetComponent<eT3Manager>().AiOffenceCommand();
                }


            }
        }


    }

    public void RegisterBuilding(GameObject building, int slot, int buildingType) //Type 0 -> Turrent Type 1 -> Extractor
    {
        if (buildingType == 0)
        {
            building.GetComponent<BadTurrentManager>().AssignSlotId(slot);
        }
        else if (buildingType == 1)
        {
            building.GetComponent<AiResourceExtractor>().AssignSlotId(slot);
        }
    }

   public void UnRegisterBuilding(int type, int slot)
    {
        if (type == 0)
        {
            for (int i = 0; i < TurrentSlots.Length; i++)
            {
                if (TurrentSlots[i].SlotNumber == slot)
                {
                    TurrentSlots[i].Occupied = false;
                    Debug.Log("Un Reg Turr: " + TurrentSlots[i].SlotNumber);

                }

            }
        }
        else if (type == 1)
        {
            for (int i = 0; i < ExtractorSlots.Length; i++)
            {
                Debug.Log("Slot Find: " + slot);

                Debug.Log("Un Reg Check Arr -> " + ExtractorSlots[i].SlotNumber);
                if (ExtractorSlots[i].SlotNumber == slot)
                {
                    ExtractorSlots[i].Occupied = false;
                    Debug.Log("Un Reg Extra: " + ExtractorSlots[i].SlotNumber);

                }

            }
        }

    }


    void FileRead()
    {
        
        StreamReader Read = new StreamReader("AiCommands.txt");

        List<int> Coords = new List<int>() {1,2,8,9};  //1,2,8,9
        int Coords1 = 1;
        int Coords2 = 2;


        List<int> Modules = new List<int>() { 3, 4, 10, 11 };  //3,4,10,11
        int Mod1_R = 3;
        int Mod2_R = 4;

        int Mod1_L = 10;
        int Mod2_L = 11;

        List<int> Probability = new List<int> {5,12};
        float Prob_R = 5;
        float Prob2_R = 5;

        float Prob_L = 12;
        float Prob2_L = 12;


        List<int> Command = new List<int> {6,13};
        int Command_R = 6;
        int Command_L = 13;

        List<int> AuxCommand = new List<int> {7,14};
        int AuxComm_R = 7;
        int AuxComm_L = 14;

        int Roll = 15;

        bool ignoreLeft = false;
        bool ignoreRight = false;
        bool ignoreNeither = false;
        bool skipProbR = false;
        bool skipProbL = false;

        

        string s = Read.ReadLine();

        //      while (s != null)
        //       {
        int lol = 1;
         while (s != null)
        {
            char nextInput = ('/');

            string[] inText = s.Split(nextInput);

            lol++;
//            Debug.Log("Pre: [" + inText[0] + ", " + inText[1]);
 //           Debug.Log("Pre: [" + inText[7] + ", " + inText[8]);

            ignoreLeft = false;
            ignoreRight = false;
            ignoreNeither = false;
            skipProbR = false;
            skipProbL = false;


            if (inText[0] == "x" || inText[1] == "x"||
                inText[7] == "x" || inText[8] == "x")
            {
                if (inText[0] == "x" || inText[1] == "x") //Right Side = X
                {
 //                   Debug.Log("Right Side");
 //                   Debug.Log(inText[7] + " , " + inText[8]);

                    Coords1 = int.Parse(inText[7]);
  //                  Debug.Log(Coords1);

                    Coords2 = int.Parse(inText[8]);
                    ignoreRight = true;

                }

                if (inText[7] == "x" || inText[8] == "x") //Right Side = X
                {
  //                  Debug.Log("Left Side");

                    Coords1 = int.Parse(inText[0]);
                    Coords2 = int.Parse(inText[1]);
                    ignoreLeft = true;

                }


            }

            else { ignoreNeither = true; }



 //           Debug.Log("Out...");

            if (ignoreNeither == true)
            {
                Coords1 = int.Parse(inText[0]);
                Coords2 = int.Parse(inText[1]);
  //              Debug.Log("1");

                if (inText[2] != "x" && inText[3] != "x") // If R Mods != X
                {
                    Mod1_R = int.Parse(inText[2]); //Possible x
                    Mod2_R = int.Parse(inText[3]);
                }
                else if (inText[3] == "x")
                {
                    Mod1_R = int.Parse(inText[2]);
                    Mod2_R = 0;
                    skipProbR = true;
                }

 //               Debug.Log("2");


                if (inText[9] != "x" && inText[10] != "x") //If LMods != X
                {
                    Mod1_L = int.Parse(inText[9]); //Possible x
                    Mod2_L = int.Parse(inText[10]);
                }
                else if (inText[10] == "x")
                {
                    Mod1_L = int.Parse(inText[9]);
                    Mod2_L = 0;
                    skipProbL = true;

                }
  //              Debug.Log("3");

                if (skipProbR == false) //If Both R Mods Are Active
                {
                    Prob_R = float.Parse(inText[4]);
                    Prob2_R = (1 - Prob_R);

                }  
                else { Prob_R = 1; Prob2_R = 0; }
   //             Debug.Log("4");


                if (skipProbL == false) //If Both L Mods Are Active
                {
 //                   Debug.Log("Prob In.....");
                    Prob_L = float.Parse(inText[11]);
                    Prob2_L = (1 - Prob_L);
                }
                else
                {
     //               Debug.Log("Prob 2 In.....");
                    Prob_L = 1; Prob2_R = 0; }



                Command_R = int.Parse(inText[5]);
                Command_L = int.Parse(inText[12]);

                if (inText[6] != "x")
                {
                    AuxComm_R = int.Parse(inText[6]);
                }
                else { AuxComm_R = 0; }

                if (inText[13] != "x")
                {
                    AuxComm_L = int.Parse(inText[13]);
                }
                else { AuxComm_L = 0; }

                Roll = int.Parse(inText[14]);


     //           Debug.Log("ProbR1: " + Prob_R);
    //            Debug.Log("ProbR2: " + Prob2_R);
     //           Debug.Log("ProbL1: " + Prob_L);
     //           Debug.Log("ProbL2: " + Prob2_L);

                LoadAiCommands(Coords1, Coords2, Mod1_R, Mod2_R, Mod1_L, Mod2_L, Prob_R, Prob2_R, Prob_L, Prob2_L
                               , Command_R, Command_L, AuxComm_R, AuxComm_L, Roll, 0);
                                
            }


            else if (ignoreRight == true)                         //IGNORE RIGHT
            {
    //            Debug.Log("Ignore Right");
                Mod1_R = 0;
                Mod2_R = 0;
                
                if (inText[9] != "x" && inText[10] != "x") //If LMods != X
                {
                    Mod1_L = int.Parse(inText[9]); //Possible x
                    Mod2_L = int.Parse(inText[10]);

                }
                else if (inText[10] == "x")
                {
                    Mod1_L = int.Parse(inText[9]);
                    Mod2_L = 0;
                    skipProbL = true;

                }
 //               Debug.Log("1");


                Prob_R = 0;
                Prob2_R = 0;

                

                if (skipProbL == false) //If Both L Mods Are Active
                {
                    Prob_L = float.Parse(inText[11]);
                    Prob2_L = (1 - Prob_L);
         //           Debug.Log("Prob -> " + Prob_L);

                }
                else { Prob_L = 1; Prob2_L = 0; }
        //        Debug.Log("Prob -> " + Prob_L);

         //       Debug.Log("2");

        //        Debug.Log(inText[12]);
                Command_R = 0;
                Command_L = int.Parse(inText[12]);
      //          Debug.Log("2");

                AuxComm_R = 0;
      //          Debug.Log("2.5");


                if (inText[13] != "x")
                {
                    AuxComm_L = int.Parse(inText[13]);
                }
                else { AuxComm_L = 0; }
 //               Debug.Log("3");

                Roll = int.Parse(inText[14]);

                //                Debug.Log("Ignore Right Bottom");
                //              Debug.Log("CHECK");
                //            Debug.Log("Coords: " + Coords1 + " " + Coords2);
                //          Debug.Log("Mods: " + Mod1_R + " " + Mod2_R);
                //        Debug.Log("Mods: " + Mod1_L + " " + Mod2_L);


    //            Debug.Log("ProbR1: " + Prob_R);
    //            Debug.Log("ProbR2: " + Prob2_R);
    //            Debug.Log("ProbL1: " + Prob_L);
      //          Debug.Log("ProbL2: " + Prob2_L);

                LoadAiCommands(Coords1, Coords2, Mod1_R, Mod2_R, Mod1_L, Mod2_L, Prob_R, Prob2_R, Prob_L, Prob2_L
               , Command_R, Command_L, AuxComm_R, AuxComm_L, Roll, 1);
            }


            else if (ignoreLeft == true)                          //IGNORE LEFT
            {
  //              Debug.Log("Ignore Left");
                if (inText[2] != "x" && inText[3] != "x") // If R Mods != X
                {
                    Mod1_R = int.Parse(inText[2]); //Possible x
                    Mod2_R = int.Parse(inText[3]);
                }
                else if (inText[3] == "x")
                {
                    Mod1_R = int.Parse(inText[2]);
                    Mod2_R = 0;
                    skipProbR = true;
                }
 //               Debug.Log("1");

                Mod1_L = 0; //Possible x
                Mod2_L = 0;
                
                if (skipProbR == false) //If Both R Mods Are Active
                {
                    Prob_R = float.Parse(inText[4]);
                    Prob2_R = (1 - Prob_R);

                }
                else { Prob_R = 1; Prob2_R = 0; }

   //             Debug.Log("2");

                Prob_L = 0;
                Prob2_L = 0;

      //          Debug.Log("2.5");

      //          Debug.Log(inText[5]);
                Command_R = int.Parse(inText[5]);
                Command_L = 0;

     //           Debug.Log("2.55");

                if (inText[6] != "x")
                {
                    AuxComm_R = int.Parse(inText[6]);
                }
                else { AuxComm_R = 0; }

                AuxComm_L = 0;

                Roll = int.Parse(inText[14]);

   //             Debug.Log("3");

   //             Debug.Log("ProbR1: " + Prob_R );
   //             Debug.Log("ProbR2: " + Prob2_R );
   //             Debug.Log("ProbL1: " + Prob_L );
   //             Debug.Log("ProbL2: " + Prob2_L);

                LoadAiCommands(Coords1, Coords2, Mod1_R, Mod2_R, Mod1_L, Mod2_L, Prob_R, Prob2_R, Prob_L, Prob2_L
               , Command_R, Command_L, AuxComm_R, AuxComm_L, Roll, 11);

            }
             s = Read.ReadLine();

        }
    }


    void LoadAiCommands(int Coord1, int Coord2, int modR1, int modR2, int modL1, int modL2
                        , float probR1, float probR2, float probL1, float probL2, int commR, int commL
                        , int auxComR, int auxComL, int roll, int ignore)
    {
      //  Debug.Log("Coord: " + Coord1+ " " + Coord2);

        State[Coord1, Coord2].ModuleOne_Right = modR1; //MODS
        State[Coord1, Coord2].ModuleTwo_Right = modR2;

        State[Coord1, Coord2].ModuleOne_Left = modL1;
        State[Coord1, Coord2].ModuleTwo_Left = modL2;

        State[Coord1, Coord2].ProbabilityTop_Right = probR1; //PROBS
        State[Coord1, Coord2].ProbabilityBottom_Right = probR2;

        State[Coord1, Coord2].ProbabilityTop_Left = probL1;
        State[Coord1, Coord2].ProbabilityBottom_Left = probL2;

        State[Coord1, Coord2].Command_Right = commR;
        State[Coord1, Coord2].Command_Left = commL;

        State[Coord1, Coord2].AuxCommand_Right = auxComR;
        State[Coord1, Coord2].AuxCommand_Left = auxComL;

        State[Coord1, Coord2].IgnoreSide = ignore;

        State[Coord1, Coord2].Roll = roll;


    }

    void TestState()
    {

        for(int i = 1; i < 6; i ++)
        {
            for (int v = 1; v < 6; v++)
            {
                Debug.Log("Coord: [" + i +", " + v + "]");
                Debug.Log("Module Right:" + State[i, v].ModuleOne_Right + " and " + State[i, v].ModuleTwo_Right);
                Debug.Log("Probability Right:" + State[i, v].ProbabilityTop_Right + " and " + State[i, v].ProbabilityBottom_Right);
                Debug.Log("Command Right:" + State[i, v].Command_Right);
                Debug.Log("Aux Command Right:" + State[i, v].AuxCommand_Right);

                Debug.Log("Module Left:" + State[i, v].ModuleOne_Left + " and " + State[i, v].ModuleTwo_Left);
                Debug.Log("Probability Left:" + State[i, v].ProbabilityTop_Left + " and " + State[i, v].ProbabilityBottom_Left);
                Debug.Log("Command Right:" + State[i, v].Command_Left);
                Debug.Log("Aux Command Right:" + State[i, v].AuxCommand_Left);

                Debug.Log("Ignore:" + State[i, v].IgnoreSide);
                Debug.Log("Roll:" + State[i, v].Roll);

            }
        }
    }


    void testStuff()
    {
        int x = 9;

        if (new int[] {9,8,7,1}.Equals(x))
        {
            Debug.Log("Test Worked");
        }
        else
        {
            Debug.Log("Test Failed");

        }
    }


}



