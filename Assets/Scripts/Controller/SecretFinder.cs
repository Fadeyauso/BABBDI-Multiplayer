using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecretFinder : MonoBehaviour
{
    [SerializeField] private GameObject[] secrets;
    private int index;
    public Transform target;

    // Start is called before the first frame update
    void Start()
    {
        index = Random.Range(0, secrets.Length-1);
    }

    // Update is called once per frame
    void Update()
    {
        if (secrets[index] != null)
        {
            if (secrets[index].activeSelf) target = secrets[index].transform;
            else index = Random.Range(0, secrets.Length);
        }
        else 
            index = Random.Range(0, secrets.Length);

        
        
    }
}
