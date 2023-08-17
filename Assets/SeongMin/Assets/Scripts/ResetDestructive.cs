using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetDestructive : MonoBehaviour
{
    [SerializeField]
    private GameObject destructiveObj;

    // Start is called before the first frame update
    void Start()
    {
        Reset();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Reset()
    {
        destructiveObj.SetActive(true);
    }
}
