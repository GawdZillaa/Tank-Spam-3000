using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SelectFalcon : MonoBehaviour
{

 //   public GameObject[] SelectedUnits;
    public List<GameObject> SelectedUnits = new List<GameObject>();
    public GameObject[] Displays;
    public Text[] TextFields;
    public Image[] ImageField;
    public GameObject Panels;


    public GameObject t1Canvas;
    public Sprite sprite1;
    public Sprite sprite2;
    public Sprite sprite3;
    public Sprite spriteRes;
    public Sprite spriteSniper;
    public Sprite spriteShield;
    public Sprite spriteMechT1;

    public Text T1Text;
    public Image Image1;


    void Start()
    {
        Falcon.AInputManager.instance.selector.onFinishSelecting += MyFinishSelectingFunction;
        t1Canvas.SetActive(false);

       
        for (int i =0; i < 6; i++)
        {
            Displays[i].SetActive(false);
        }

  //      Panels.SetActive(true);
   //     t1Canvas.SetActive(true);
         t1 = false;
         t2 = false;
         t3 = false;
        Ts = false;
         res = false;
        Sh = false;
        Tm = false;

         T1Counter = 0;
         T2Counter = 0;
         T3Counter = 0;
        tSCounter = 0;
         ResCounter = 0;
        ShCounter = 0;
        TmCounter = 0;
        StartTextCount();
    }

    void OnDisable()
    {
        if (Falcon.AInputManager.instance != null)
        {
            Falcon.AInputManager.instance.selector.onFinishSelecting -= MyFinishSelectingFunction;
        }
    }

    bool FailSafe = false;

    void MyFinishSelectingFunction(Falcon.Selectable[] selectableArray)
    {
        foreach (Falcon.Selectable selectable in selectableArray)
        {
 //           Debug.Log("Selected: " + selectable.gameObject.name);
            GameObject foundUnit = selectable.gameObject;
            SelectedUnits.Add(foundUnit);
        }

        //STORE FOUND UNITS IN PERSONAL LIST
        foreach (GameObject listed in SelectedUnits)
        {
//            Debug.Log("In List: " + listed.gameObject.name);

        }

        if (FailSafe == false)
        {
            DisplaySelected();
            FailSafe = true;
        }
    }

    void update()
    {
        bool isRightDown = Input.GetMouseButtonDown(1);

        if (isRightDown == true)
        {
            Debug.Log("old");
        }


 //       checkUnitArray(selectableArray);
    }
    bool shouldMove;
    void checkUnitArray(List<GameObject> SelectedUnits, float newX)
    {
//        Debug.Log(SelectedUnits.Count);
        if (SelectedUnits.Count == 0)
        {
            Debug.Log("Array Empty...");

        }
        if (SelectedUnits.Count > 0)
        {
 //           Debug.Log("Array Presence...");

            foreach (GameObject listed in SelectedUnits)
            {
                shouldMove = true;

                if (listed.gameObject.tag == "Gresource")
                {
                    if (listed.gameObject != null)
                    {
                        listed.gameObject.GetComponent<ResourceBuilderGoodManager>().ListMover(newX, shouldMove);
                    }
                }

                    if (listed.gameObject.tag == "Tank")
                    {
                        if (listed.gameObject != null)
                        {
                            listed.gameObject.GetComponent<Movment>().ListMover(newX, shouldMove);
                        }
                    }


                if (listed.gameObject.tag == "Tank2")
                {
                    if (listed.gameObject != null)
                    {
                        listed.gameObject.GetComponent<GoodTankR2Manager>().ListMover(newX, shouldMove);
                    }
                }

                if (listed.gameObject.tag == "Tank3")
                {
                    if (listed.gameObject != null)
                    {
                        listed.gameObject.GetComponent<T3GoodManager>().ListMover(newX, shouldMove);
                    }
                }

                if (listed.gameObject.tag == "Tank4")
                {
                    if (listed.gameObject != null)
                    {
                        listed.gameObject.GetComponent<SniperTankManager>().ListMover(newX, shouldMove);
                    }
                }

                if (listed.gameObject.tag == "TankShG")
                {
                    if (listed.gameObject != null)
                    {
                        listed.gameObject.GetComponent<ShieldTankG>().ListMover(newX, shouldMove);
                    }
                }


                if (listed.gameObject.tag == "MechG")
                {
                    if (listed.gameObject != null)
                    {
                        listed.gameObject.GetComponent<MechAR_G_Manager>().ListMover(newX, shouldMove);
                    }
                }

            }

        }
    }
    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

 //           Debug.Log(hit.point.x);

            if (hit != false)
            {
                
                checkUnitArray(SelectedUnits, hit.point.x);
                
            }
//            Debug.Log("Pressed right click.!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
            t1Canvas.SetActive(false);
             Reset();
             FailSafe = false;



            Falcon.AInputManager.instance.selector.UnselectAll();
            SelectedUnits.Clear();
       }

        if (Input.GetMouseButtonDown(0))
            {

            Falcon.AInputManager.instance.selector.UnselectAll();
            SelectedUnits.Clear();

            Reset();
             FailSafe = false;




        }
    }


    bool t1;
    bool t2;
    bool t3;
    bool Ts;
    bool res;
    bool Sh;
    bool Tm;

   private int T1Counter;
    int DisplayCount = 0;
    int T2Counter;
    int T3Counter;
    int tSCounter;
    int ResCounter;
    int ShCounter;
    int TmCounter;

    public struct info
    {
       public int counter;
       public Sprite infoSprite;


    };
    info[] SelectedTanks = new info[6];


    int DisplayCounter(bool one, bool two, bool three, bool resB,bool four, bool Sh, bool Tm,
        int T1, int T2, int T3, int res, int ts, int Tsh, int TmC)
    {
        int inc = 0;


        int count = 0;

        if (one == true)
        {
            SelectedTanks[inc].counter = T1;
            SelectedTanks[inc].infoSprite = sprite1;
            inc++;
            count++;
        }
        if (two == true)
        {
            SelectedTanks[inc].counter = T2;
            SelectedTanks[inc].infoSprite = sprite2;
            inc++;
            count++;
        }
        if (three == true)
        {
            SelectedTanks[inc].counter = T3;
            SelectedTanks[inc].infoSprite = sprite3;
            inc++;
            count++;
        }
        if (resB == true)
        {
            SelectedTanks[inc].counter = res;
            SelectedTanks[inc].infoSprite = spriteRes;
            inc++;
            count++;
        }

        if (four == true)
        {
            SelectedTanks[inc].counter = ts;
            SelectedTanks[inc].infoSprite = spriteSniper;
            inc++;
            count++;
        }

        if (Sh == true)
        {
            SelectedTanks[inc].counter = Tsh;
            SelectedTanks[inc].infoSprite = spriteShield;
            inc++;
            count++;
        }

        if (Tm == true)
        {
            SelectedTanks[inc].counter = TmC;
            SelectedTanks[inc].infoSprite = spriteMechT1;
            inc++;
            count++;
        }

        return count;
    }


    void DisplaySelected()
    {
        foreach (GameObject listed in SelectedUnits)
        {
            if (listed.gameObject.tag == "Tank")
            {
//                Debug.Log("Detected DisplaySelected");
                t1 = true;
                T1Counter++;
            }

            if (listed.gameObject.tag == "Tank2")
            {
//                Debug.Log("Detected DisplaySelected");
                t2 = true;
                T2Counter++;
            }

            if (listed.gameObject.tag == "Tank3")
            {
//                Debug.Log("Detected DisplaySelected");
                t3 = true;
                T3Counter++;
            }

            if (listed.gameObject.tag == "Gresource")
            {
//                Debug.Log("Detected DisplaySelected");
                res = true;
                ResCounter++;
            }

            if (listed.gameObject.tag == "Tank4")
            {
 //               Debug.Log("Detected DisplaySelected");
                Ts = true;
                tSCounter++;
            }

            if (listed.gameObject.tag == "TankShG")
            {
//                Debug.Log("Detected DisplaySelected");
                Sh = true;
                ShCounter++;
            }

            if (listed.gameObject.tag == "MechG")
            {
//                Debug.Log("Detected DisplaySelected");
                Tm = true;
                TmCounter++;
            }



        }


        DisplayCount =  DisplayCounter(t1, t2, t3, res,Ts, Sh, Tm, T1Counter, T2Counter, T3Counter, ResCounter, tSCounter, ShCounter, TmCounter);


        if (DisplayCount == 1)
        {
            if (t1 == true && t2 == false && t3 == false && res == false && Ts == false && Sh == false && Tm == false)
            {
                Debug.Log("~~~~~~~~~Set Text~~~~~~~");
                Debug.Log(T1Counter);

                t1Canvas.SetActive(true);

                T1Text.text = T1Counter + "";
                Image1.sprite = sprite1;
            }

            if (t1 == false && t2 == true && t3 == false && res == false && Ts == false && Sh == false && Tm == false)
            {

                t1Canvas.SetActive(true);

                T1Text.text = T2Counter + "";
                Image1.sprite = sprite2;
            }

            if (t1 == false && t2 == false && t3 == true && res == false && Ts == true && Sh == false && Tm == false)
            {
                Debug.Log(T1Counter);

                t1Canvas.SetActive(true);

                T1Text.text = T3Counter + "";
                Image1.sprite = sprite3;
            }

            if (t1 == false && t2 == false && t3 == false && res == true && Ts == true && Sh == false && Tm == false)
            {

                t1Canvas.SetActive(true);

                T1Text.text = ResCounter + "";
                Image1.sprite = spriteRes;
            }

            if (t1 == false && t2 == false && t3 == false && res == false && Ts == true && Sh == false && Tm == false)
            {

                t1Canvas.SetActive(true);

                T1Text.text = tSCounter + "";
                Image1.sprite = spriteSniper;
            }

            if (t1 == false && t2 == false && t3 == false && res == false && Ts == false && Sh == true && Tm == false)
            {

                t1Canvas.SetActive(true);

                T1Text.text = ShCounter + "";
                Image1.sprite = spriteShield;
            }

            if (t1 == false && t2 == false && t3 == false && res == false && Ts == false && Sh == false && Tm == true)
            {

                t1Canvas.SetActive(true);

                T1Text.text = TmCounter + "";
                Image1.sprite = spriteMechT1;
            }
        }

//        Debug.Log(DisplayCount + "~~~~~~~~~~~~~~~");
        if (DisplayCount > 1)
        {
            for (int i=0; i < DisplayCount; i++)
            {
                Displays[i].SetActive(true);
                TextFields[i].text = "" + SelectedTanks[i].counter;
                ImageField[i].sprite = SelectedTanks[i].infoSprite;

            }
        }

    }

    void StartTextCount()
    {
  //      T1Text = t1Canvas.GetComponent<Text>();
        T1Text.text = "" + T1Counter;
    }


    void Reset()
    {
        T1Counter = 0;
        T2Counter = 0;
        T3Counter = 0;
        tSCounter = 0;
        ResCounter = 0;
        ShCounter = 0;
        TmCounter = 0;

        t1 = false;
        t2 = false;
        t3 = false;
        Ts = false;
        res = false;
        Sh = false;
        Tm = false;

        for (int i = 0; i < 6; i++)
        {
            Displays[i].SetActive(false);
        }

        for (int i = 0; i < 6; i++)
        {
            ImageField[i].sprite = null;
        }

        for (int i = 0; i < 6; i++)
        {
            TextFields[i].text = null;
        }

        for (int i = 0; i < 6; i++)
        {
            SelectedTanks[i].counter = 0;
            SelectedTanks[i].infoSprite = null;
        }
    }

}
