using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHandler : MonoBehaviour
{
    // Start is called before the first frame update

    public Text NinjaNameLable;
    public Image BackgroundNinjaNameLable;
    public GameObject targetNinja;

    public Text GuardNameLable;
    public Image BackgroundGuardNameLable;
    public GameObject targetGuard;

    

    void Start()
    {
        

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 ninjaNamePLate = Camera.main.WorldToScreenPoint(targetNinja.transform.position);
        NinjaNameLable.transform.position = ninjaNamePLate;
        BackgroundNinjaNameLable.transform.position = ninjaNamePLate;
        NinjaNameLable.text = targetNinja.GetComponent<Rogue>().tree.currNode._name;

        Vector3 guardNamePlate = Camera.main.WorldToScreenPoint(targetGuard.transform.position);
        GuardNameLable.transform.position = guardNamePlate;
        BackgroundGuardNameLable.transform.position = guardNamePlate;
        GuardNameLable.text = targetGuard.GetComponent<Guard>().tree.currNode._name;
    }
}

