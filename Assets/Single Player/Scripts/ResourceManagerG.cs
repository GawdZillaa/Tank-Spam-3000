using UnityEngine;
using UnityEngine.UI;

using System.Collections;

public class ResourceManagerG : MonoBehaviour {

    public int[] TierStack = new int[7];
    public int BaseLine;

    public Text EconShow;
    public Text resourceText;
    public int InitialResources;
    private int resourcesG;

    public int BaseEconomy;
	// Use this for initialization
	void Start () {
        resourcesG = InitialResources;
        setResources();
    }

    float timer = 0;

	// Update is called once per frame
	void FixedUpdate () {
        timer += Time.deltaTime;

        EconShow.text = "" + BaseEconomy+"/ 5sec";

        if (timer >= 5)
        {
 //           Debug.Log("Add Resources");
            resourcesG += BaseEconomy;
            setResources();
            timer = 0;
        }

    }



    void setResources()
    {
        resourceText.text = "" + resourcesG;

    }

    public  void deductResources(int amount)
    {
        resourcesG = resourcesG - amount;
        setResources();
    }

    public bool checkRequest(int amount)
    {
        if (amount <= resourcesG ||
            amount == resourcesG)
        {
            deductResources(amount);
            return true;
        }
        else
        {
            return false;
        }
    }

    public void killResource(int amount)
    {
        Debug.Log("resources Added..............................");
        Debug.Log(resourcesG);

        resourcesG = resourcesG + amount;
        setResources();

    }


   public void UpExtractor(int totals)
    {
        Debug.Log("--Recieved Amount: " + totals);
        BaseEconomy += totals;
        Debug.Log(BaseEconomy);

    }

    public void DownExtractor(int tier)
    {
     //   int baseLine = 5;

      //  BaseEconomy -= baseLine * stack;


        int Totals = 0;

        Totals = TierStack[tier] * BaseLine;
        Debug.Log("--Stack-> " + TierStack[tier]);

        Debug.Log("--Recieved Reduction Amount: " + Totals);
        Debug.Log(BaseEconomy);

        BaseEconomy -= Totals;
        Debug.Log(BaseEconomy);

    }

    public int getResourceG()
    {
        return resourcesG;
    }
}
