using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MainMenuEffect : MonoBehaviour
{
    private float timer = -5;
    [SerializeField] private TMP_Text textLabel;
    public TextMeshProUGUI tmp;
    public float glowTimer;
    public float glowSpeed;
    public float glowAmount;
    private Color c;
    // Start is called before the first frame update
    void Awake()
    {
        c = textLabel.color;
    }

    // Update is called once per frame
    void Update()
    {
        textLabel.color = c;

        glowTimer += Time.deltaTime * glowSpeed;

        timer -= Time.deltaTime;
        this.tmp.fontMaterial.SetFloat(ShaderUtilities.ID_LightAngle, 3f + Mathf.Sin(glowTimer) * glowAmount);
    }
}
