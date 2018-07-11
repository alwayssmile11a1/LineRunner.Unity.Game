using UnityEngine;
using UnityEngine.Events;
#if UNITY_ADS
using UnityEngine.Advertisements;
#endif
public class AdvertisementManager : MonoBehaviour
{

#region Static
    public static AdvertisementManager Instance
    {
        get
        {
            if (m_Instance != null) return m_Instance;

            m_Instance = FindObjectOfType<AdvertisementManager>();

            if (m_Instance != null) return m_Instance;

            //create new
            GameObject gameObject = new GameObject("AdvertisementManager");
            gameObject = Instantiate(gameObject);
            m_Instance = gameObject.AddComponent<AdvertisementManager>();

            return m_Instance;
        }

    }
    protected static AdvertisementManager m_Instance;
    #endregion

#if UNITY_ADS

    [Header("Skippable Ads")]
    public UnityEvent OnSkippableAdsInitalized;
    [Space(10)]
    public UnityEvent OnSkippableAdsFinished;
    public UnityEvent OnSkippableAdsSkipped;
    public UnityEvent OnSkippableAdsFailed;


    [Header("Rewarded Ads")]
    public UnityEvent OnRewardedAdsInitalized;
    [Space(10)]
    public UnityEvent OnRewardedAdsFinished;
    public UnityEvent OnRewaredAdsSkipped;
    public UnityEvent OnRewardedAdsFailed;



    protected bool m_SkippableAdsInitializing = false;
    protected bool m_RewardedAdsInitializing = false;

    protected readonly string m_RewardedAdsPlacementID = "rewardedVideo";



    protected void Awake()
    {
        if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }

    }

    public void Update()
    {
        if (m_SkippableAdsInitializing && Advertisement.IsReady())
        {
            OnSkippableAdsInitalized.Invoke();
            m_SkippableAdsInitializing = false;
        }

        if (m_RewardedAdsInitializing && Advertisement.IsReady(m_RewardedAdsPlacementID))
        {
            OnRewardedAdsInitalized.Invoke();
            m_RewardedAdsInitializing = false;
        }
        
    }

    public void ShowRewardedAds()
    {
        if (Advertisement.IsReady(m_RewardedAdsPlacementID))
        {
            ShowOptions showOptions = new ShowOptions() { resultCallback = HandleRewardedAdsResult };
            Advertisement.Show(m_RewardedAdsPlacementID, showOptions);
        }
    }

    public void ShowSkippableAds()
    {
        if (Advertisement.IsReady())
        {
            ShowOptions showOptions = new ShowOptions() { resultCallback = HandleSkippableAdsResult };
            Advertisement.Show(showOptions);
        }
    }

    public void InitializeRewardedAds()
    {
        m_RewardedAdsInitializing = true;
    }

    public void InitializeSkippableAds()
    {
        m_SkippableAdsInitializing = true;
    }

    public bool IsInitializingRewardedAds()
    {
        return m_RewardedAdsInitializing;
    }

    public bool IsInitializingSkippableAds()
    {
        return m_SkippableAdsInitializing;
    }

    protected void HandleSkippableAdsResult(ShowResult result)
    {
        switch (result)
        {
            case ShowResult.Finished:
                OnSkippableAdsFinished.Invoke();
                break;
            case ShowResult.Skipped:
                OnSkippableAdsSkipped.Invoke();
                break;
            case ShowResult.Failed:
                OnSkippableAdsFailed.Invoke();
                break;
        }
    }

    protected void HandleRewardedAdsResult(ShowResult result)
    {
        switch (result)
        {
            case ShowResult.Finished:
                OnRewardedAdsFinished.Invoke();
                break;
            case ShowResult.Skipped:
                OnRewaredAdsSkipped.Invoke();
                break;
            case ShowResult.Failed:
                OnRewardedAdsFailed.Invoke();
                break;
        }

    }

#endif

}
