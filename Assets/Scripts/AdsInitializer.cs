using UnityEditor.PackageManager;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.Advertisements;

public class AdsInitializer : MonoBehaviour, IUnityAdsInitializationListener,IUnityAdsLoadListener,IUnityAdsShowListener
{
    [SerializeField] string _androidGameId;
    [SerializeField] string _iOSGameId;
    [SerializeField] bool _testMode = true;
    private string _gameId;

    //[SerializeField] PlayerController _playerController;
    //[SerializeField] InterstitialButton _interstitialButton;
    void Awake()
    {
        InitializeAds();
    }

    public void InitializeAds()
    {
#if UNITY_IOS
            _gameId = _iOSGameId;
#elif UNITY_ANDROID
        _gameId = _androidGameId;
#elif UNITY_EDITOR
            _gameId = _androidGameId; //Only for testing the functionality in the Editor
#endif
        if (!Advertisement.isInitialized && Advertisement.isSupported)
        {
            Advertisement.Initialize(_gameId, _testMode, this);
        }
    }


    public void OnInitializationComplete()
    {
        Debug.Log("Unity Ads initialization complete.");
        LoadInterstitialAds();
        //_interstitialButton.LoadAd();
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        Debug.Log($"Unity Ads Initialization Failed: {error.ToString()} - {message}");
    }
    public void LoadInterstitialAds()
    {
        Advertisement.Load("InitializeAds", this);
    }

    public void OnUnityAdsAdLoaded(string placementId)
    {
        Advertisement.Show(placementId,this);
    }

    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
    {
        throw new System.NotImplementedException();
    }

    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
    {
        Debug.Log($"Unity Ads Initialization Failed: {error.ToString()} - {message}");
    }
    public void OnUnityAdsShowStart(string placementId)
    {
        Debug.Log($"Unity Ads show start");
    }

    public void OnUnityAdsShowClick(string placementId)
    {
        Debug.Log($"Unity Ads show click:");
    }

    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
    {
        Debug.Log($"Unity Ads show complete:");
    }
}