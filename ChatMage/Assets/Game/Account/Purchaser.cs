using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;

// Placing the Purchaser class in the CompleteProject namespace allows it to interact with ScoreManager, 
// one of the existing Survival Shooter scripts.
namespace CompleteProject
{
    // Deriving the Purchaser class from IStoreListener enables it to receive messages from Unity Purchasing.
    public class Purchaser
#if UNITY_ANDROID
//        : IStoreListener
#endif
    {
        /*
#if UNITY_ANDROID
        private static IStoreController m_StoreController;          // The Unity Purchasing system.
        private static IExtensionProvider m_StoreExtensionProvider; // The store-specific Purchasing subsystems.

        // Product identifiers for all products capable of being purchased: 
        // "convenience" general identifiers for use with Purchasing, and their store-specific identifier 
        // counterparts for use with and outside of Unity Purchasing. Define store-specific identifiers 
        // also on each platform's publisher dashboard (iTunes Connect, Google Play Developer Console, etc.)

        // General product identifiers for the consumable, non-consumable, and subscription products.
        // Use these handles in the code to reference which product to purchase. Also use these values 
        // when defining the Product Identifiers on the store. Except, for illustration purposes, the 
        // kProductIDSubscription - it has custom Apple and Google identifiers. We declare their store-
        // specific mapping to Unity Purchasing's AddProduct, below.
        public static string smallMoneyAmount = "smallmoneyamount";
        public static string mediumMoneyAmount = "mediummoneyamount";
        public static string largeMoneyAmount = "largemoneyamount";
        public static string fullGame = "fullgame";

        public Sprite coinsLogo;

        // Google Play Store-specific product identifier subscription product.
        //private static string kProductNameGooglePlaySubscription = "com.unity3d.subscription.original";

        public void Init()
        {
            // If we haven't set up the Unity Purchasing reference
            if (m_StoreController == null)
            {
                // Begin to configure our connection to Purchasing
                InitializePurchasing();
            }
        }

        public void InitializePurchasing()
        {
            // If we have already connected to Purchasing ...
            if (IsInitialized())
            {
                // ... we are done here.
                return;
            }

            // Create a builder, first passing in a suite of Unity provided stores.
            var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

            // Add a product to sell / restore by way of its identifier, associating the general identifier
            // with its store-specific identifiers.
            builder.AddProduct(smallMoneyAmount, ProductType.Consumable);
            builder.AddProduct(mediumMoneyAmount, ProductType.Consumable);
            builder.AddProduct(largeMoneyAmount, ProductType.Consumable);
            // Continue adding the non-consumable product.
            builder.AddProduct(fullGame, ProductType.NonConsumable);

            // Kick off the remainder of the set-up with an asynchrounous call, passing the configuration 
            // and this class' instance. Expect a response either in OnInitialized or OnInitializeFailed.
            UnityPurchasing.Initialize(this, builder);
        }


        private bool IsInitialized()
        {
            // Only say we are initialized if both the Purchasing references are set.
            return m_StoreController != null && m_StoreExtensionProvider != null;
        }

        public void BuyConsumable(int coinsAmount)
        {
            if (coinsAmount >= StorePrice.GetPrice(StorePrice.CommandType.smallGoldAmount))
            {
                if (coinsAmount >= StorePrice.GetPrice(StorePrice.CommandType.mediumGoldAmount))
                {
                    if (coinsAmount >= StorePrice.GetPrice(StorePrice.CommandType.largeGoldAmount))
                        BuyLargeConsumable();
                    else
                        BuyMediumConsumable();
                }
                else
                    BuySmallConsumable();
            }
        }

        public void BuySmallConsumable()
        {
            ShopPopUpMenu.ShowShopPopUpMenu("Bill Confirmation", "You are currently in the process of buying a large amount "
                + " of coins. Are you sure ?", coinsLogo, m_StoreController.products.WithID(smallMoneyAmount).metadata.localizedPriceString, StorePrice.GetPrice(StorePrice.CommandType.smallGoldAmount), delegate ()
            {
                // Buy the consumable product using its general identifier. Expect a response either 
                // through ProcessPurchase or OnPurchaseFailed asynchronously.
                BuyProductID(smallMoneyAmount);
            });
        }

        public void BuyMediumConsumable()
        {
            ShopPopUpMenu.ShowShopPopUpMenu("Bill Confirmation", "You are currently in the process of buying a large amount "
                + " of coins. Are you sure ?", coinsLogo, m_StoreController.products.WithID(mediumMoneyAmount).metadata.localizedPriceString, StorePrice.GetPrice(StorePrice.CommandType.mediumGoldAmount), delegate ()
            {
            // Buy the consumable product using its general identifier. Expect a response either 
            // through ProcessPurchase or OnPurchaseFailed asynchronously.
            BuyProductID(mediumMoneyAmount);
            });
        }

        public void BuyLargeConsumable()
        {
            ShopPopUpMenu.ShowShopPopUpMenu("Bill Confirmation", "You are currently in the process of buying a large amount "
                + " of coins. Are you sure ?", coinsLogo, m_StoreController.products.WithID(largeMoneyAmount).metadata.localizedPriceString, StorePrice.GetPrice(StorePrice.CommandType.largeGoldAmount), delegate ()
            {
                // Buy the consumable product using its general identifier. Expect a response either 
                // through ProcessPurchase or OnPurchaseFailed asynchronously.
                BuyProductID(largeMoneyAmount);
            });
        }

        public void BuyTheGame()
        {
            // Buy the non-consumable product using its general identifier. Expect a response either 
            // through ProcessPurchase or OnPurchaseFailed asynchronously.
            BuyProductID(fullGame);
        }

        void BuyProductID(string productId)
        {
            // If Purchasing has been initialized ...
            if (IsInitialized())
            {
                // ... look up the Product reference with the general product identifier and the Purchasing 
                // system's products collection.
                Product product = m_StoreController.products.WithID(productId);

                // If the look up found a product for this device's store and that product is ready to be sold ... 
                if (product != null && product.availableToPurchase)
                {
                    Debug.Log(string.Format("Purchasing product asychronously: '{0}'", product.definition.id));
                    // ... buy the product. Expect a response either through ProcessPurchase or OnPurchaseFailed 
                    // asynchronously.
                    m_StoreController.InitiatePurchase(product);
                }
                // Otherwise ...
                else
                {
                    // ... report the product look-up failure situation  
                    Debug.Log("BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase");
                }
            }
            // Otherwise ...
            else
            {
                // ... report the fact Purchasing has not succeeded initializing yet. Consider waiting longer or 
                // retrying initiailization.
                Debug.Log("BuyProductID FAIL. Not initialized.");
            }
        }

        //  
        // --- IStoreListener
        //

        public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
        {
            // Purchasing has succeeded initializing. Collect our Purchasing references.
            Debug.Log("Purchaser Init");

            // Overall Purchasing system, configured with products for this application.
            m_StoreController = controller;
            // Store specific subsystem, for accessing device-specific store features.
            m_StoreExtensionProvider = extensions;
        }

        public void OnInitializeFailed(InitializationFailureReason error)
        {
            // Purchasing set-up has not succeeded. Check error for reason. Consider sharing this reason with the user.
            PopUpMenu.ShowOKPopUpMenu("Oups", "An Error has occured");
            Debug.Log("OnInitializeFailed InitializationFailureReason:" + error);
        }

        public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
        {
            // A consumable product has been purchased by this user.
            if (String.Equals(args.purchasedProduct.definition.id, smallMoneyAmount, StringComparison.Ordinal))
            {
                Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
                // The consumable item has been successfully purchased
                Account.instance.Command(StorePrice.CommandType.smallGoldAmount);
            }
            // A consumable product has been purchased by this user.
            else if (String.Equals(args.purchasedProduct.definition.id, mediumMoneyAmount, StringComparison.Ordinal))
            {
                Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
                // The consumable item has been successfully purchased
                Account.instance.Command(StorePrice.CommandType.mediumGoldAmount);
            }
            // A consumable product has been purchased by this user.
            else if (String.Equals(args.purchasedProduct.definition.id, largeMoneyAmount, StringComparison.Ordinal))
            {
                Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
                // The consumable item has been successfully purchased
                Account.instance.Command(StorePrice.CommandType.largeGoldAmount);
            }
            // Or ... a non-consumable product has been purchased by this user.
            else if (String.Equals(args.purchasedProduct.definition.id, fullGame, StringComparison.Ordinal))
            {
                Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
                // The non-consumable item has been successfully purchased
                // TODO : get the whole game
            }
            // Or ... an unknown product has been purchased by this user. Fill in additional products here....
            else
            {
                Debug.Log(string.Format("ProcessPurchase: FAIL. Unrecognized product: '{0}'", args.purchasedProduct.definition.id));
            }

            // Return a flag indicating whether this product has completely been received, or if the application needs 
            // to be reminded of this purchase at next app launch. Use PurchaseProcessingResult.Pending when still 
            // saving purchased products to the cloud, and when that save is delayed. 
            return PurchaseProcessingResult.Complete;
        }

        public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
        {
            // A product purchase attempt did not succeed. Check failureReason for more detail. Consider sharing 
            // this reason with the user to guide their troubleshooting actions.
            Debug.Log(string.Format("OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailureReason: {1}", product.definition.storeSpecificId, failureReason));
        }
#endif
*/
    }
}
