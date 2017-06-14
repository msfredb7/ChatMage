using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorePrice {

    public enum CommandType
    {
        none = 0,
        smallLootboxCost = 1,
        mediumLootboxCost = 2,
        largeLootboxCost = 3,
        smallLootboxGoldifyCost = 4,
        mediumLootboxGoldifyCost = 5,
        largeLootboxGoldifyCost = 6,
        smallGoldAmount = 7,
        mediumGoldAmount = 8,
        largeGoldAmount = 9,
        customGoldAmount = 10,
        slotCost = 11,
        adReward = 12,
    }

    public static int smallLootboxCost = -5;
    public static int mediumLootboxCost = -10;
    public static int largeLootboxCost = -15;

    public static int smallLootboxGoldifyCost = -5;
    public static int mediumLootboxGoldifyCost = -10;
    public static int largeLootboxGoldifyCost = -15;

    public static int smallGoldAmount = 10;
    public static int mediumGoldAmount = 50;
    public static int largeGoldAmount = 100;
    public static int customGoldAmount = 1;

    public static int slotCost = 1;

    public static int adReward = 5;

    public static int GetPrice(CommandType type)
    {
        switch (type)
        {
            case CommandType.none:
                return 0;
            case CommandType.smallLootboxCost:
                return smallLootboxCost;
            case CommandType.mediumLootboxCost:
                return mediumLootboxCost;
            case CommandType.largeLootboxCost:
                return largeLootboxCost;
            case CommandType.smallLootboxGoldifyCost:
                return smallLootboxGoldifyCost;
            case CommandType.mediumLootboxGoldifyCost:
                return mediumLootboxGoldifyCost;
            case CommandType.largeLootboxGoldifyCost:
                return largeLootboxGoldifyCost;
            case CommandType.smallGoldAmount:
                return smallGoldAmount;
            case CommandType.mediumGoldAmount:
                return mediumGoldAmount;
            case CommandType.largeGoldAmount:
                return largeGoldAmount;
            case CommandType.customGoldAmount:
                return customGoldAmount;
            case CommandType.slotCost:
                return slotCost;
            case CommandType.adReward:
                return adReward;
        }
        return 0;
    }
}
