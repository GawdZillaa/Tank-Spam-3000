using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiReources : MonoBehaviour {

   public int BaseEconomy;
   public int EnergyReserves;
    public int BaseLine;
    public int InitialResources;

    float TimeReset;

    public int[] TierStack = new int[7];

	// Use this for initialization


	void Start () {
        TimeReset = 0;
        EnergyReserves = InitialResources;
	}

    void FixedUpdate()
    {
        TimeReset += Time.deltaTime;


        if (TimeReset >= 5)
        {
            EnergyReserves += BaseEconomy;
            TimeReset = 0;
        }


    }

    // Update is called once per frame
    void Update () {
		
	}

    public void killResource(int amount)
    {
        Debug.Log("resources Added..............................");
        Debug.Log(EnergyReserves);

        EnergyReserves = EnergyReserves + amount;
        Debug.Log(EnergyReserves);

    }

    public void UpExtractor(int totals)
    {
        Debug.Log("Recieved Amount: " + totals);
        BaseEconomy += totals;
        Debug.Log(BaseEconomy);


    }

    public void DownExtractor(int tier)
    {
         int Totals = 0;

        Totals = TierStack[tier] * BaseLine;
        Debug.Log("Stack-> " +TierStack[tier]);

        Debug.Log("Recieved Reduction Amount: " + Totals);
        BaseEconomy -= Totals;
        Debug.Log(BaseEconomy);

    }

    public void AiUnitRequest(int Amount)
    {
        Debug.Log("Amout Deducted: " + Amount);
        Debug.Log("Before Deduction: " + EnergyReserves);

        EnergyReserves = EnergyReserves - Amount;
        Debug.Log("After Deduction: " + EnergyReserves);

    }

   
}
