using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.WSA;

public class FinalDestination : MonoBehaviour, IInterractable
{
    [SerializeField] int capacity;
    [SerializeField] int nbOfCharges;
    [SerializeField] GameObject mask;
    [SerializeField] EnergyBar energyBar;
    [SerializeField] Launcher launcher;

    bool recharging;
    float movementPerCharge;
    float nbOfChargesNeeded;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //// default values
        //recharging = false;
        //nbOfChargesNeeded = 0f;

        //movementPerCharge = mask.GetComponent<SpriteMask>().bounds.size.y / capacity;

        //// lower the volume of not fully filled tanks
        //mask.GetComponent<Transform>().position -= new Vector3(0, (capacity - nbOfCharges) * movementPerCharge, 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Interract()
    {
        //if (launcher.currentEnergy < launcher.maxEnergy)
        //{
        //    // imobilise the player during the recharge and make it look at the station
        //    nbOfChargesNeeded = launcher.maxEnergy - launcher.currentEnergy;

        //    if (nbOfChargesNeeded > nbOfCharges)
        //    {
        //        nbOfChargesNeeded = nbOfCharges;
        //    }

        //    recharging = true;
        //    nbOfCharges += (int)nbOfChargesNeeded;
        //    launcher.currentEnergy -= (int)nbOfChargesNeeded;
        //    mask.GetComponent<Transform>().position += new Vector3(0, nbOfChargesNeeded, 0); // + or - give the same result, I don't get it
        //    energyBar.slider.value -= nbOfChargesNeeded;
        //}

        recharging = true;
        nbOfCharges += (int)nbOfChargesNeeded;
        
        mask.GetComponent<Transform>().position += new Vector3(0, launcher.currentEnergy, 0); // + or - give the same result, I don't get it
        launcher.currentEnergy = 0;
        energyBar.slider.value = 0;
    }
}
