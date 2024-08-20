// using System;
// using System.Linq;
// using System.Reactive;
// using System.Reactive.Linq;
// using DXHRDCHudScaler.Core.Models;
// using DXHRDCHudScaler.Core.Services;
// using DXHRDCHudScaler.Core.Services.SimplePatchService;
// using DXHRDCHudScaler.Models;
// using DynamicData;
// using ReactiveUI;
//
// namespace DXHRDCHudScaler.ViewModels;
//
// public class ExtrasTabViewModel : ViewModelBase, IExtrasTabViewModel
// {
//     private bool _applyBtnChecked;
//     private bool _skipIntroVideosCheckboxChecked;
//     private bool _skipIntroVideosCheckboxEnabled;
//     private bool _skipCutscenesCheckboxChecked;
//     private bool _skipCutscenesCheckboxEnabled;
//
//     private readonly IAppState _appState;
//     private SourceCache<Job, string> _jobCache = new(x => x.Name);
//     private readonly ISimplePatchService _simplePatchService;
//
//     public ExtrasTabViewModel(
//         IScreen screen,
//         IAppState appState,
//         ISimplePatchService simplePatchService
//     )
//     {
//         HostScreen = screen;
//         _simplePatchService = simplePatchService;
//         _appState = appState;
//         _skipIntroVideosCheckboxEnabled = _appState.GameExePathExists();
//         _skipCutscenesCheckboxEnabled = _appState.GameExePathExists();
//
//         this.WhenAnyValue(x => x._appState.GameExePath)
//             .Subscribe(x =>
//             {
//                 _jobCache.Clear();
//                 SkipIntroVideosCheckboxChecked = CheckboxCheckedLogicOnGameExeChanged(
//                     SkipIntroVideosCheckboxEnabled,
//                     SkipIntroVideosCheckboxChecked,
//                     // SimplePatchType.SkipIntroVideos
//                 );
//                 SkipIntroVideosCheckboxEnabled = _appState.GameExePathExists();
//                 SkipCutscenesCheckboxChecked = CheckboxCheckedLogicOnGameExeChanged(
//                     SkipCutscenesCheckboxEnabled,
//                     SkipCutscenesCheckboxChecked,
//                     // SimplePatchType.SkipCutscenes
//                 );
//                 SkipCutscenesCheckboxEnabled = _appState.GameExePathExists();
//             });
//
//         this.WhenAnyValue(x => x.SkipIntroVideosCheckboxChecked)
//             .Subscribe(skipIntroVideosChecked =>
//             {
//                 if (
//                     string.IsNullOrWhiteSpace(_appState.GameExePath)
//                     || !_appState.GameExePathExists()
//                 )
//                 {
//                     return;
//                 }
//
//                 if (skipIntroVideosChecked)
//                 {
//                     _jobCache.AddOrUpdate(
//                         new Job(
//                             "SkipIntroVideosEnable",
//                             () =>
//                                 _simplePatchService.Patch(
//                                     _appState.GetGameType(),
//                                     // SimplePatchType.SkipIntroVideos,
//                                     _appState.GameExePath!
//                                 )
//                         )
//                     );
//                 }
//                 else
//                 {
//                     if (_jobCache.Items.Any(x => x.Name == "SkipIntroVideosEnable"))
//                     {
//                         _jobCache.Remove("SkipIntroVideosEnable");
//                     }
//                     else
//                     {
//                         _jobCache.AddOrUpdate(
//                             new Job(
//                                 "SkipIntroVideosDisable",
//                                 () =>
//                                     _simplePatchService.UnPatch(
//                                         _appState.GetGameType(),
//                                         SimplePatchType.SkipIntroVideos,
//                                         _appState.GameExePath!
//                                     )
//                             )
//                         );
//                     }
//                 }
//             });
//
//         this.WhenAnyValue(x => x.SkipCutscenesCheckboxChecked)
//             .Skip(1)
//             .Subscribe(skipCutscenesCbChecked =>
//             {
//                 if (
//                     string.IsNullOrWhiteSpace(_appState.GameExePath)
//                     || !_appState.GameExePathExists()
//                 )
//                 {
//                     return;
//                 }
//
//                 if (skipCutscenesCbChecked)
//                 {
//                     _jobCache.AddOrUpdate(
//                         new Job(
//                             "SkipCutscenesEnable",
//                             () =>
//                                 _simplePatchService.Patch(
//                                     _appState.GetGameType(),
//                                     SimplePatchType.SkipCutscenes,
//                                     _appState.GameExePath!
//                                 )
//                         )
//                     );
//                 }
//                 else
//                 {
//                     if (_jobCache.Items.Any(x => x.Name == "SkipCutscenesEnable"))
//                     {
//                         _jobCache.Remove("SkipCutscenesEnable");
//                     }
//                     else
//                     {
//                         _jobCache.AddOrUpdate(
//                             new Job(
//                                 "SkipCutscenesDisable",
//                                 () =>
//                                     _simplePatchService.UnPatch(
//                                         _appState.GetGameType(),
//                                         SimplePatchType.SkipCutscenes,
//                                         _appState.GameExePath!
//                                     )
//                             )
//                         );
//                     }
//                 }
//             });
//
//         _jobCache
//             .Connect()
//             .Subscribe(x =>
//             {
//                 CheckIfApplyBtnShouldBeEnabled();
//             });
//         ApplyCmd = ReactiveCommand.Create(() =>
//         {
//             foreach (var job in _jobCache.Items)
//             {
//                 job.Action();
//             }
//
//             _jobCache.Clear();
//         });
//     }
//
//     private bool CheckboxCheckedLogicOnGameExeChanged(
//         bool curCheckboxEnabled,
//         bool curCheckboxChecked,
//         SimplePatchType simplePatchType
//     )
//     {
//         if (!curCheckboxEnabled || string.IsNullOrWhiteSpace(_appState.GameExePath))
//         {
//             return curCheckboxChecked;
//         }
//
//         return _simplePatchService.GetCurrentPatchStatus(
//             _appState.GetGameType(),
//             simplePatchType,
//             _appState.GameExePath
//         ) switch
//         {
//             SimplePatchStatus.Patched => true,
//             SimplePatchStatus.UnPatched => false,
//             _ => throw new ArgumentOutOfRangeException()
//         };
//     }
//
//     public bool ApplyBtnEnabled
//     {
//         get => _applyBtnChecked;
//         set => this.RaiseAndSetIfChanged(ref _applyBtnChecked, value);
//     }
//
//     public bool SkipIntroVideosCheckboxEnabled
//     {
//         get => _skipIntroVideosCheckboxEnabled;
//         set => this.RaiseAndSetIfChanged(ref _skipIntroVideosCheckboxEnabled, value);
//     }
//
//     public bool SkipIntroVideosCheckboxChecked
//     {
//         get => _skipIntroVideosCheckboxChecked;
//         set => this.RaiseAndSetIfChanged(ref _skipIntroVideosCheckboxChecked, value);
//     }
//
//     public bool SkipCutscenesCheckboxEnabled
//     {
//         get => _skipCutscenesCheckboxEnabled;
//         set => this.RaiseAndSetIfChanged(ref _skipCutscenesCheckboxEnabled, value);
//     }
//
//     public bool SkipCutscenesCheckboxChecked
//     {
//         get => _skipCutscenesCheckboxChecked;
//         set => this.RaiseAndSetIfChanged(ref _skipCutscenesCheckboxChecked, value);
//     }
//
//     public ReactiveCommand<Unit, Unit> ApplyCmd { get; }
//     public string? UrlPathSegment { get; } = Guid.NewGuid().ToString()[..5];
//     public IScreen HostScreen { get; }
//
//     /// <summary>
//     /// If the job cache has any items, that means there are changes that could be applied and therefore the apply button should be enabled.
//     /// </summary>
//     private void CheckIfApplyBtnShouldBeEnabled()
//     {
//         ApplyBtnEnabled = _jobCache.Items.Any();
//     }
// }
