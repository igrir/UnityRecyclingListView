using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecyclingSampleSceneController : MonoBehaviour
{

    [SerializeField]
    RecyclingListController recyclingContainer;

    // Use this for initialization
    void Start()
    {
        recyclingContainer.SetLabelData(new List<string>
        {
            "A","B","C","D","E","F","G","H","I","J","K","L","M","N","o","P","Q","R","S","T","U","V","W","X","Y","Z"
        });
        recyclingContainer.Init();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
