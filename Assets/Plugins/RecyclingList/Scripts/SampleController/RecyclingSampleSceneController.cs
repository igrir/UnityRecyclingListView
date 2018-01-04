using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecyclingSampleSceneController : MonoBehaviour
{

    [SerializeField]
    RecyclingListController recyclingContainer;

    [Header("Test Toggles")]
    public bool AddData;
    public bool DelData;

    List<RecyclingModel> sampleList = new List<RecyclingModel>();

    // Use this for initialization
    void Start()
    {
        ApplyDataIntoList();
    }

    void ApplyDataIntoList()
    {
        recyclingContainer.SetLabelData(null);
    }

    // Update is called once per frame
    void Update()
    {
        // adding more data sample
        if (AddData)
        {
            AddData = false;
            sampleList.Add(new RecyclingModel() { Label = sampleList.Count.ToString(), Id = sampleList.Count.ToString() });
            recyclingContainer.SetLabelData(sampleList);
        }

        // decrease samples
        if (DelData)
        {
            DelData = false;
            if (sampleList.Count > 0)
            {
                sampleList.RemoveAt(sampleList.Count - 1);
                recyclingContainer.SetLabelData(sampleList);
            }
        }
    }
}
