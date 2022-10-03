using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SetQuality : MonoBehaviour
{
    [SerializeField] private TMPro.TMP_Dropdown qualityDropdown;

    public void SetQualityLevelDropdown(int index)
    {
        index = qualityDropdown.value;
        QualitySettings.SetQualityLevel(index, false);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
