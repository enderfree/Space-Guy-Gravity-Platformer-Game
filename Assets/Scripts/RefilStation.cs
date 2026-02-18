using Unity.VisualScripting;
using UnityEngine;

public class Refil : MonoBehaviour
{
    [SerializeField] int capacity;
    [SerializeField] int nbOfCharges;
    [SerializeField] GameObject mask;
    [SerializeField] EnergyBar energyBar;

    bool recharging;
    float movementPerCharge;
    float nbOfChargesNeeded;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // default values
        recharging = false;
        nbOfChargesNeeded = 0f;

        movementPerCharge = mask.GetComponent<SpriteMask>().bounds.size.y / capacity;

        // lower the volume of not fully filled tanks
        mask.GetComponent<Transform>().position -= new Vector3(0, (capacity - nbOfCharges) * movementPerCharge, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (recharging)
        {
            float change = movementPerCharge * Time.deltaTime;

            mask.GetComponent<Transform>().position -= new Vector3(0, change, 0);
            energyBar.slider.value += change;
            nbOfChargesNeeded -= change;

            if (nbOfChargesNeeded <= 0f)
            {
                nbOfChargesNeeded = 0f;
                recharging = false;
                // unstuck player
            }
        }
    }

    public void Refill(Launcher launcher)
    {
        if (launcher.currentEnergy < launcher.maxEnergy)
        {
            // imobilise the player during the recharge and make it look at the station
            nbOfChargesNeeded = launcher.maxEnergy - launcher.currentEnergy;

            if (nbOfChargesNeeded > nbOfCharges)
            {
                nbOfChargesNeeded = nbOfCharges;
            }

            recharging = true;
            nbOfCharges -= (int)nbOfChargesNeeded;
            launcher.currentEnergy += (int)nbOfChargesNeeded;
        }


    }
}
