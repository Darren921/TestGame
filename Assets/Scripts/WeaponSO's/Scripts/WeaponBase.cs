using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public abstract class WeaponBase : MonoBehaviour
{
    [SerializeField] private WeaponSO weaponStats;
    [SerializeField] private TextMeshProUGUI ammoText;
    [SerializeField] private TextMeshProUGUI jamText;

    private Coroutine currentFireTimer;
    private bool isOnCoolDown;
    private WaitUntil coolDownEnforce;
    private WaitForSeconds ReloadTime;
    private float currentChargeTime;
    private bool isReloading;
    private bool isJammed;

    private int ammoLeft;
    private int maxAmmo;
    private bool hasAmmo;


    private void Awake()
    {
        ReloadTime = new WaitForSeconds(weaponStats.reloadTime);
        ammoLeft = weaponStats.magSize;
        maxAmmo = weaponStats.maxAmmo;
        hasAmmo = true;
        UpdateAmmo();
        jamText.text = "";
        coolDownEnforce = new WaitUntil(() => !isOnCoolDown);
    }
    // Start is called before the first frame update
    void Start()
    {
      
    }



    // Update is called once per frame
    void Update()
    {
     
    }


    public void startShooting()
    {
        currentFireTimer = StartCoroutine(ReFireTimer());
    }

    private void jamCheck()
    {
        var jamChance = UnityEngine.Random.Range(0f, 1000f);
        var jamRate = weaponStats.jamRate * 1000;

        if (jamChance < jamRate)
        {
            jamText.text = "Jammed, press \"R\" to attempt fix";
            isJammed = true;
        }
        
    }

    public void stopShooting()
    {
        StopCoroutine(currentFireTimer);
        float percent = currentChargeTime / weaponStats.chargeUpTime;
        if (percent != 0) TryAttack(percent);
       
    }

    private IEnumerator CooldownTimer()
    {
        isOnCoolDown = true;
        yield return weaponStats.coolDownWait;
        isOnCoolDown = false;
    }
    private void TryAttack(float percent)
    {
        jamCheck();

        currentChargeTime = 0;
        if (!CanAttack(percent) || isJammed == true) return;
         
        if (ammoLeft > 0 && isReloading != true && isJammed != true)
        {
            Attack(percent);
            ammoLeft--;
            ammoText.text = ammoLeft.ToString() + " / " + maxAmmo.ToString();
        }
        else return;
        if (maxAmmo == 0 && ammoLeft != 0 ) 
        {
            hasAmmo = false;
        }
       
        StartCoroutine(CooldownTimer());   
        if(weaponStats.isFullAuto && percent >= 1) currentFireTimer = StartCoroutine(ReFireTimer());
    }

    public IEnumerator TryReload()
    {

        if (ammoLeft == weaponStats.magSize && isJammed == false) yield break;
        if (isJammed == true)
        {
            var jamFixed = UnityEngine.Random.Range(0f, 10f);
                if (jamFixed < 7f)
                {
                 jamText.text = "jam fixed";
                jamText.text = "";

                if (ammoLeft != 0)
                    {
                        ammoLeft--;
                        UpdateAmmo();
                    }
                    isJammed = false;
                    yield break;
                }
                else Debug.Log("fix failed");
                yield break;
            }
        if (isReloading == true) yield break; 
        isReloading = true;
        yield return ReloadTime;
        if (hasAmmo == true && maxAmmo >= 0 && isJammed == false && isReloading == true)
        {
            if (ammoLeft == 0)
            {
                maxAmmo -= weaponStats.magSize + ammoLeft;
                ammoLeft = weaponStats.magSize;
                hasAmmo = true;
                isReloading = false;
                UpdateAmmo();

            }
            else if (ammoLeft >= (weaponStats.magSize - ammoLeft))
            {
                maxAmmo = (maxAmmo - weaponStats.magSize) + ammoLeft;
                ammoLeft = weaponStats.magSize;
                hasAmmo = true;
                isReloading = false;
                UpdateAmmo();
            }
            else if (ammoLeft <= (weaponStats.magSize - ammoLeft))
            {
                maxAmmo = 0;
                ammoLeft += maxAmmo;
                hasAmmo = false;
                isReloading = false;
                UpdateAmmo();
            }
        }
        isReloading = false;
    }
    private IEnumerator ReFireTimer()
    {
        print("waiting for cooldown");
        yield return coolDownEnforce;
        print("Post Cooldown");

        while (currentChargeTime < weaponStats.chargeUpTime)
        {
            currentChargeTime += Time.deltaTime;
            yield return null;
        }
        TryAttack(1);
        yield return null;

    }

    
    private void UpdateAmmo()
    {
        ammoText.text = ammoLeft.ToString() + " / " + maxAmmo.ToString();
    }
    protected virtual bool CanAttack(float percent)
    {
        return !isOnCoolDown && percent >= weaponStats.minChargePercent;
    }
    protected abstract void Attack(float percent);
}
