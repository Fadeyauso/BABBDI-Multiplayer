using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SavePopup : MonoBehaviour
{
    private float timer = -5;
    [SerializeField] private TMP_Text textLabel;
    public TextMeshProUGUI tmp;
    public float glowTimer;
    public float glowSpeed;
    public float glowAmount;
    private Color c;
    private GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }
    // Start is called before the first frame update
    void Awake()
    {
        c = textLabel.color;
        c.a = 0;
    }

    // Update is called once per frame
    void Update()
    {
        textLabel.color = c;

        glowTimer += Time.deltaTime * glowSpeed;

        timer -= Time.deltaTime;
        this.tmp.fontMaterial.SetFloat(ShaderUtilities.ID_LightAngle, 3f + Mathf.Sin(glowTimer) * glowAmount);
        if (gameManager.savePopup == true)
        {
            glowTimer = 0;
            c.a = 1f;
            timer = 3f;
            gameManager.savePopup = false;
        }
        if (timer < 0) 
        {
            if (c.a >= 0) c.a -= 1.5f * Time.deltaTime;
        }
    }
}
