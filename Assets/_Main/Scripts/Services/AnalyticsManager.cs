using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_ANALYTICS
using UnityEngine.Analytics;
#endif

public class AnalyticsManager : MonoBehaviour {


#if UNITY_ANALYTICS

    public AdvertisingNetwork adsNetwork = AdvertisingNetwork.UnityAds;


    protected readonly string m_RewardedAdsPlacementID = "rewardedVideo";

    public void AdOffer(bool adsRewarded)
    {
        if (adsRewarded)
        {
            AnalyticsEvent.AdOffer(adsRewarded, adsNetwork, m_RewardedAdsPlacementID);
        }
        else
        {
            AnalyticsEvent.AdOffer(adsRewarded, adsNetwork);
        }
    }

    



#endif


}
