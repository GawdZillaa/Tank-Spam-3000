using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

using UnityEngine;

public class ExtractorLink : MonoBehaviour
{

    public List<GameObject> AllObjects = new List<GameObject>();

    public GameObject UQ;
    public struct ExtractorUpgrade
    {
        public int CurrentTier;
        public GameObject theExtractor;
    }

    List<ExtractorUpgrade> AdvExtractorUpL = new List<ExtractorUpgrade>();

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StartupLinkScan()
    {

        Debug.Log("<><><>EXTRACTOR SYNC<><><>");
        Scene scene = SceneManager.GetActiveScene();
        scene.GetRootGameObjects(AllObjects);

        ExtractorUpgrade[] AdvExtractorUp = new ExtractorUpgrade[10];



        int i = 0;
        int tempTier = 1;
        int MaxTier = 6;


        foreach (GameObject listed in AllObjects)
        {
            if (listed.tag == "GoodResourceCollector")
            {
                AdvExtractorUp[i].theExtractor = listed;
                AdvExtractorUp[i].CurrentTier = listed.GetComponent<ResourceExtractor>().getTier();
                Debug.Log(listed.GetComponent<ResourceExtractor>().getTier());
                tempTier = listed.GetComponent<ResourceExtractor>().getTier();

                i++;
            }

        }
        Debug.Log("i= " + i);

        if (i >= 5)
        {
            Debug.Log("Accepted...");
            int[] TierTemp = new int[i];
            Debug.Log("Array Tier Length: " + i);

            for (int p = 0; p < i; p++)
            {
                TierTemp[p] = AdvExtractorUp[p].CurrentTier;
                Debug.Log(TierTemp[p]);
            }

            System.Array.Sort(TierTemp);

            Debug.Log(i - 1);
            Debug.Log((i - 1)-4);
            Debug.Log(TierTemp.Length);
            int tempS = i - 6;
            for (int search = i - 1; search > tempS; search--)
            {
                Debug.Log(search);

                MaxTier = TierTemp[search];
            }

            Debug.Log("Max -> " + MaxTier);
            UQ.GetComponent<UnitQ>().ExtractorEfficiency(MaxTier);
        }

        else { Debug.Log("Not Enough Extractors"); }

    }



}  
