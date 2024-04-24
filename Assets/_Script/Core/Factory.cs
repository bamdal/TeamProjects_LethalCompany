using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PoolObjectType
{
    item = 0,
    Hardware,

}

public class Factory : Singleton<Factory>
{
    //SlimePool slimePool;
    ShovelPool shovelPool;
    FlashPool flashPool;    
    HardwareBarrelPool hardwareBarrelPool;
    HardwareCableDrumPool hardwareCableDrumPool;
    HardwareGarbageCartPool hardwareGarbageCartPool;
    HardwareGasTankPool hardwareGasTankPool;
    HardwarePalletJackPool hardwarePalletJackPool;

    protected override void OnInitialize()
    {
        base.OnInitialize();

        //slimePool = GetComponentInChildren<SlimePool>();
        //if( slimePool != null ) slimePool.Initialize();
        shovelPool = GetComponentInChildren<ShovelPool>();
        if(shovelPool != null )
                shovelPool.Initialize();
        flashPool = GetComponentInChildren<FlashPool>();
        if( flashPool != null )
            flashPool.Initialize();

        hardwareBarrelPool = GetComponentInChildren<HardwareBarrelPool>();
        if(hardwareBarrelPool != null ) hardwareBarrelPool.Initialize();

        hardwareCableDrumPool = GetComponentInChildren<HardwareCableDrumPool>();
        if (hardwareCableDrumPool != null) hardwareCableDrumPool.Initialize();

        hardwareGarbageCartPool = GetComponentInChildren<HardwareGarbageCartPool>();
        if (hardwareGarbageCartPool != null) hardwareGarbageCartPool.Initialize();

        hardwareGasTankPool = GetComponentInChildren<HardwareGasTankPool>();
        if (hardwareGasTankPool != null) hardwareGasTankPool.Initialize();

        hardwarePalletJackPool = GetComponentInChildren<HardwarePalletJackPool>();
        if (hardwarePalletJackPool != null) hardwarePalletJackPool.Initialize();
    }

    /// <summary>
    /// 풀에 있는 게임 오브젝트 하나 가져오기
    /// </summary>
    /// <param name="type">가져올 오브젝트의 종류</param>
    /// <param name="position">오브젝트가 배치될 위치</param>
    /// <param name="angle">오브젝트의 초기 각도</param>
    /// <returns>활성화된 오브젝트</returns>
    public GameObject GetObject(PoolObjectType type, Vector3? position = null, Vector3? euler = null)
    {
        GameObject result = null;
        // switch (type)
        // {
        //     
        // }

        return result;
    }

    /*public GameObject GetHardware(ItemCode itemCode)
    {
        ItemDB data = GameManager.Instance.ItemData[itemCode];
        // ItemObject obj = shovelPool.GetObject();
        // obj.ItemData = data;                    // 풀에서 하나 꺼내고 데이터 설정
        return null;
    }*/

    public ItemBase GetHardware(ItemCode itemCode)
    {
        //return hardwareBarrelPool.GetObject();

        switch (itemCode)
        {
            case ItemCode.Barrel:
                return hardwareBarrelPool.GetObject();
            case ItemCode.CableDrum:
                return hardwareCableDrumPool.GetObject();
            case ItemCode.GarbageCart:
                return hardwareGarbageCartPool.GetObject();
            case ItemCode.GasTank:
                return hardwareGasTankPool.GetObject();
            case ItemCode.PalletJack:
                return hardwarePalletJackPool.GetObject();
            default:
                throw new ArgumentException("Invalid item code", nameof(itemCode));
        }

    }

    public ItemBase GetHardware(ItemCode itemCode, Vector3 position, float angle = 0.0f)
    {
        //return hardwareBarrelPool.GetObject(position, angle * Vector3.forward);

        switch (itemCode)
        {
            case ItemCode.Barrel:
                return hardwareBarrelPool.GetObject(position, angle * Vector3.forward);
            case ItemCode.CableDrum:
                return hardwareCableDrumPool.GetObject(position, angle * Vector3.forward);
            case ItemCode.GarbageCart:
                return hardwareGarbageCartPool.GetObject(position, angle * Vector3.forward);
            case ItemCode.GasTank:
                return hardwareGasTankPool.GetObject(position, angle * Vector3.forward);
            case ItemCode.PalletJack:
                return hardwarePalletJackPool.GetObject(position, angle * Vector3.forward);
            default:
                throw new ArgumentException("Invalid item code", nameof(itemCode));
        }
    }

    public ItemBase GetItem()
    {
        return shovelPool.GetObject();
    }

    public ItemBase GetItem(Vector3 position, float angle = 0.0f)
    {
        return shovelPool.GetObject(position, angle * Vector3.forward);
    }


    /// <summary>
    /// 랜덤한 폐철물 소환 enumValues.Length-1 부분 코드수 늘어나면 조정해야함
    /// </summary>
    /// <param name="position"></param>
    /// <param name="angle"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public ItemBase GetRandomHardware(Vector3 position, float angle = 0.0f)
    {
        var enumValues = Enum.GetValues(enumType: typeof(ItemCode));
        ItemCode itemCode = (ItemCode)enumValues.GetValue(UnityEngine.Random.Range(0, enumValues.Length-1));

        switch (itemCode)
        {
            case ItemCode.Barrel:
                return hardwareBarrelPool.GetObject(position, angle * Vector3.forward);
            case ItemCode.CableDrum:
                return hardwareCableDrumPool.GetObject(position, angle * Vector3.forward);
            case ItemCode.GarbageCart:
                return hardwareGarbageCartPool.GetObject(position, angle * Vector3.forward);
            case ItemCode.GasTank:
                return hardwareGasTankPool.GetObject(position, angle * Vector3.forward);
            case ItemCode.PalletJack:
                return hardwarePalletJackPool.GetObject(position, angle * Vector3.forward);
            default:
                throw new ArgumentException("Invalid item code", nameof(itemCode));
        }
    }

    public ItemBase GetItem(ItemCode itemCode, Vector3 position, float angle = 0.0f)
    {
        switch(itemCode)
        {
            case ItemCode.Shovel:
                return shovelPool.GetObject(position, angle* Vector3.forward);
            case ItemCode.FlashLight:
                return flashPool.GetObject(position, angle* Vector3.forward);
            default:
                throw new ArgumentException("Invalid item code", nameof(itemCode));
      
        }
    }
}