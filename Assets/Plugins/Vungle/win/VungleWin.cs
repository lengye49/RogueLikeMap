using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

#if UNITY_WSA_10_0 || UNITY_WINRT_8_1 || UNITY_METRO
using VungleSDKProxy;

public enum VungleAdOrientation
{
	AutoRotate,
	MatchVideo
}

public class VungleWin
{
	private static VungleAd sdk;
	private static AdConfig cfg;
	private static bool _isSoundEnabled = true;
	private static VungleAdOrientation _orientation = VungleAdOrientation.AutoRotate;
	
	static VungleWin()
	{
	}

	// Starts up the SDK with the given appId
	public static void init( string appId, string version, params string[] placements )
	{
        VungleSDKConfig config = new VungleSDKConfig ();
        config.SetPluginName ("unity");
        config.SetPluginVersion (version);
        config.SetApiEndpoint(new Uri("http://ads.api.vungle.com"));
        sdk = AdFactory.GetInstance(appId, config, placements);
        sdk.addOnEvent(VungleManager.onEvent);
	}

	// Call this when your application is sent to the background
	public static void onPause()
	{
	}

	// Call this when your application resumes
	public static void onResume()
	{
	}

	// Checks to see if a video is available
	public static bool isVideoAvailable(string placement)
	{
        if (sdk != null)
            return sdk.IsAdPlayable(placement);
		return false;
	}
	
	
	// Sets if sound should be enabled or not
	public static void setSoundEnabled( bool isEnabled )
	{
		_isSoundEnabled = isEnabled;
	}
	
	
	// Sets the allowed orientations of any ads that are displayed
	public static void setAdOrientation( VungleAdOrientation orientation )
	{
		_orientation = orientation;
	}
	
	
	// Checks to see if sound is enabled
	public static bool isSoundEnabled()
	{
		return _isSoundEnabled;
	}

    public static void loadAd(string placement)
    {
        if (sdk != null)
            sdk.LoadAd(placement);
    }
	
	// Plays an ad with the given options. The user option is only supported for incentivized ads.
	public static void playAd(string placement)
	{
		if (sdk != null && sdk.IsAdPlayable(placement)) {
			cfg = new AdConfig ();
			cfg.SetUserId ("");
			cfg.SetSoundEnabled (_isSoundEnabled);
			cfg.SetOrientation((_orientation == VungleAdOrientation.AutoRotate)?VungleSDKProxy.DisplayOrientations.AutoRotate:VungleSDKProxy.DisplayOrientations.Landscape);
			sdk.PlayAd(cfg, placement);
		}
	}
	
	public static void playAd( Dictionary<string,object> options, string placement)
	{
		if (sdk != null && sdk.IsAdPlayable(placement)) {
			cfg = new AdConfig ();
			if (options.ContainsKey("userTag") && options["userTag"] is string)
				cfg.SetUserId ((string) options["userTag"]);
			cfg.SetSoundEnabled (_isSoundEnabled);
			if (options.ContainsKey("orientation")) {
				if (options ["orientation"] is bool) {
					cfg.SetOrientation (((bool)options ["orientation"]) ? VungleSDKProxy.DisplayOrientations.Landscape : VungleSDKProxy.DisplayOrientations.AutoRotate);
				}
				if (options ["orientation"] is VungleAdOrientation) {
					cfg.SetOrientation(((VungleAdOrientation)options ["orientation"] == VungleAdOrientation.AutoRotate) ? VungleSDKProxy.DisplayOrientations.AutoRotate : VungleSDKProxy.DisplayOrientations.Landscape);
				}
			} else
				cfg.SetOrientation((_orientation == VungleAdOrientation.AutoRotate) ? VungleSDKProxy.DisplayOrientations.AutoRotate : VungleSDKProxy.DisplayOrientations.Landscape);
			if (options.ContainsKey("alertText") && options["alertText"] is string)
				cfg.SetIncentivizedDialogBody ((string) options["alertText"]);
			if (options.ContainsKey("alertTitle") && options["alertTitle"] is string)
				cfg.SetIncentivizedDialogTitle ((string) options["alertTitle"]);
			if (options.ContainsKey("closeText") && options["closeText"] is string)
				cfg.SetIncentivizedDialogCloseButton ((string) options["closeText"]);
			if (options.ContainsKey("continueText") && options["continueText"] is string)
				cfg.SetIncentivizedDialogContinueButton ((string) options["continueText"]);
			if (options.ContainsKey("backImmediately") && options["backImmediately"] is string)
				cfg.SetBackButtonImmediatelyEnabled ((bool) options["backImmediately"]);
			sdk.PlayAd(cfg, placement);
		}
	}
}
#endif

