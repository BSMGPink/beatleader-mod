﻿using System.Linq;
using VRUIControls;
using UnityEngine;
using Zenject;

namespace BeatLeader.Replayer
{
    public class SceneTweaksManager : MonoBehaviour
    {
        [Inject] private readonly PauseMenuManager _pauseMenuManager;
        [Inject] private readonly LocksController _softLocksController;
        [Inject] private readonly StandardLevelGameplayManager _gameplayManager;
        [Inject] private readonly PauseController _pauseController;
        [Inject] private readonly MainCamera _mainCamera;

        [Inject] private readonly IMenuButtonTrigger _pauseButtonTrigger;
        [Inject] private readonly IVRPlatformHelper _vrPlatformHelper;
        [Inject] private readonly ILevelStartController _levelStartController;

        private void Start()
        {
            ApplyTweaks();
            UnsubscribeEvents();
        }
        private void ApplyTweaks()
        {
            _softLocksController.InstallLock(_gameplayManager);
            _softLocksController.InstallLock(_pauseMenuManager);
            _softLocksController.InstallLock(Resources.FindObjectsOfTypeAll<CuttingManager>().First());
            _mainCamera.gameObject.SetActive(false);
            Resources.FindObjectsOfTypeAll<VRLaserPointer>().First().gameObject.SetActive(!InputManager.IsInFPFC);
            Resources.FindObjectsOfTypeAll<SaberBurnMarkArea>().First().gameObject.SetActive(!InputManager.IsInFPFC);
        }
        private void UnsubscribeEvents()
        {
            _pauseButtonTrigger.menuButtonTriggeredEvent -= _pauseController.HandleMenuButtonTriggered;
            _vrPlatformHelper.inputFocusWasCapturedEvent -= _pauseController.HandleInputFocusWasCaptured;
            _vrPlatformHelper.hmdUnmountedEvent -= _pauseController.HandleHMDUnmounted;
            _pauseMenuManager.didFinishResumeAnimationEvent -= _pauseController.HandlePauseMenuManagerDidFinishResumeAnimation;
            _pauseMenuManager.didPressContinueButtonEvent -= _pauseController.HandlePauseMenuManagerDidPressContinueButton;
            _pauseMenuManager.didPressRestartButtonEvent -= _pauseController.HandlePauseMenuManagerDidPressRestartButton;
            //_pauseMenuManager.didPressMenuButtonEvent -= _pauseController.HandlePauseMenuManagerDidPressMenuButton;
            _levelStartController.levelDidStartEvent -= _pauseController.HandleLevelDidStart;
            _levelStartController.levelWillStartIntroEvent -= _pauseController.HandleLevelWillStartIntro;
        }
    }
}
