using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ContinueDialogueVisual : MonoBehaviour
{
    public TextMeshProUGUI tmp;
    public float glowTimer;
    public float glowSpeed;
    public float glowAmount;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        glowTimer += Time.deltaTime * glowSpeed;

        this.tmp.fontMaterial.SetFloat(ShaderUtilities.ID_LightAngle, 3f + Mathf.Sin(glowTimer) * glowAmount);
    }
}
