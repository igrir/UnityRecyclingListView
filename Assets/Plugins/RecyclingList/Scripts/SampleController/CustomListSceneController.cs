using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomListSceneController : MonoBehaviour
{

    public class CustomDataSample
    {
        public string CustomLabel;
    }

    [SerializeField]
    RecyclingListController recyclingContainer;

    [Header("Test Toggles")]
    public bool AddData;
    public bool DelData;

    List<RecyclingModel> sampleList = new List<RecyclingModel>();
    Dictionary<string, CustomDataSample> customDataDictionary = new Dictionary<string, CustomDataSample>();

    // Use this for initialization
    void Start()
    {
        recyclingContainer.OnButtonClicked += RecyclingContainer_OnButtonClicked;
        recyclingContainer.OnDataSet += RecyclingContainer_OnDataSet;

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

            string label = sampleList.Count.ToString();

            sampleList.Add(new RecyclingModel() { Label = label, Id = label });
            customDataDictionary.Add(label,
                new CustomDataSample()
                {
                    CustomLabel = "Test" + label
                });

            recyclingContainer.SetLabelData(sampleList);
        }

        // decrease samples
        if (DelData)
        {
            DelData = false;
            if (sampleList.Count > 0)
            {
                string lastIndexId = sampleList[sampleList.Count - 1].Id;

                customDataDictionary.Remove(lastIndexId);
                sampleList.RemoveAt(sampleList.Count - 1);

                recyclingContainer.SetLabelData(sampleList);
            }
        }
    }

    void RecyclingContainer_OnButtonClicked(RecyclingButton recyclingButton)
    {
        CustomDataSample customData = null;
        if (customDataDictionary.TryGetValue(recyclingButton.Model.Id, out customData))
        {
            Debug.Log(customData.CustomLabel);
        }
    }

    void RecyclingContainer_OnDataSet(RecyclingButton recyclingButton)
    {
        CustomButton customButton = recyclingButton.GetComponent<CustomButton>();
        if (customButton != null)
        {
            CustomDataSample customData = null;
            if (customDataDictionary.TryGetValue(recyclingButton.Model.Id, out customData))
            {
                customButton.OtherLabel.text = customData.CustomLabel;
            }
        }
    }
}
