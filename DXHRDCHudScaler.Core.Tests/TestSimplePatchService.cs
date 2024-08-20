// using System.Collections;
// using DXHRDCHudScaler.Core.Models;
// using DXHRDCHudScaler.Core.Services.SimplePatchService;
//
// namespace DXHRDCHudScaler.Core.Tests;
//
// public class TestSimplePatchService
// {
//     private const string TmpPath = "tmp.exe";
//
//     private static readonly byte[] SkipIntroUnpatchedBytes = [0x74, 0x0A, 0x83, 0xF8, 0x02, 0x73];
//     private static readonly byte[] SkipIntroPatchedBytes = [0x90, 0x90, 0x83, 0xF8, 0x02, 0x73];
//
//     private static readonly byte[] SkipCutsceneUnpatchedBytes = [0x8A, 0x58, 0x49];
//     private static readonly byte[] SkipCutscenePatchedBytes = [0xB3, 0x01, 0x90];
//
//     private static IEnumerable CanPatchTestData()
//     {
//         yield return new TestCaseData(
//             GameType.Original,
//             SimplePatchType.SkipIntroVideos,
//             SkipIntroUnpatchedBytes
//         ).SetName("SkipIntro");
//         yield return new TestCaseData(
//             GameType.Original,
//             SimplePatchType.SkipCutscenes,
//             SkipCutsceneUnpatchedBytes
//         ).SetName("SkipCutscene");
//     }
//
//     private static IEnumerable CanUnpatchTestData()
//     {
//         yield return new TestCaseData(
//             GameType.Original,
//             SimplePatchType.SkipIntroVideos,
//             SkipIntroPatchedBytes
//         ).SetName("SkipIntro");
//         yield return new TestCaseData(
//             GameType.Original,
//             SimplePatchType.SkipCutscenes,
//             SkipCutscenePatchedBytes
//         ).SetName("SkipCutscene");
//     }
//
//     private static IEnumerable PatchTestData()
//     {
//         yield return new TestCaseData(
//             GameType.Original,
//             SimplePatchType.SkipIntroVideos,
//             SkipIntroUnpatchedBytes,
//             SkipIntroPatchedBytes
//         ).SetName("SkipIntro");
//         yield return new TestCaseData(
//             GameType.Original,
//             SimplePatchType.SkipCutscenes,
//             SkipCutsceneUnpatchedBytes,
//             SkipCutscenePatchedBytes
//         ).SetName("SkipCutscene");
//     }
//
//     private static IEnumerable UnpatchTestData()
//     {
//         yield return new TestCaseData(
//             GameType.Original,
//             SimplePatchType.SkipIntroVideos,
//             SkipIntroPatchedBytes,
//             SkipIntroUnpatchedBytes
//         ).SetName("SkipIntro");
//         yield return new TestCaseData(
//             GameType.Original,
//             SimplePatchType.SkipCutscenes,
//             SkipCutscenePatchedBytes,
//             SkipCutsceneUnpatchedBytes
//         ).SetName("SkipCutscene");
//     }
//
//     [TestCaseSource(nameof(CanPatchTestData))]
//     public void CanPatch(GameType gameType, SimplePatchType simplePatchType, byte[] bytes)
//     {
//         try
//         {
//             File.WriteAllBytes(TmpPath, bytes);
//
//             var sut = new SimplePatchService();
//             Assert.That(sut.CanPatch(gameType, simplePatchType, TmpPath));
//         }
//         finally
//         {
//             if (File.Exists(TmpPath))
//             {
//                 File.Delete(TmpPath);
//             }
//         }
//     }
//
//     [TestCaseSource(nameof(CanUnpatchTestData))]
//     public void CanUnpatch(GameType gameType, SimplePatchType simplePatchType, byte[] bytes)
//     {
//         try
//         {
//             File.WriteAllBytes(TmpPath, bytes);
//
//             var sut = new SimplePatchService();
//             Assert.That(sut.CanUnPatch(gameType, simplePatchType, TmpPath));
//         }
//         finally
//         {
//             if (File.Exists(TmpPath))
//             {
//                 File.Delete(TmpPath);
//             }
//         }
//     }
//
//     [TestCaseSource(nameof(PatchTestData))]
//     public void Patch(
//         GameType gameType,
//         SimplePatchType simplePatchType,
//         byte[] bytes,
//         byte[] expected
//     )
//     {
//         try
//         {
//             File.WriteAllBytes(TmpPath, bytes);
//
//             var sut = new SimplePatchService();
//             sut.Patch(gameType, simplePatchType, TmpPath);
//
//             Assert.That(File.ReadAllBytes(TmpPath), Is.EquivalentTo(expected));
//         }
//         finally
//         {
//             if (File.Exists(TmpPath))
//             {
//                 File.Delete(TmpPath);
//             }
//         }
//     }
//
//     [TestCaseSource(nameof(UnpatchTestData))]
//     public void Unpatch(
//         GameType gameType,
//         SimplePatchType simplePatchType,
//         byte[] bytes,
//         byte[] expected
//     )
//     {
//         try
//         {
//             File.WriteAllBytes(TmpPath, bytes);
//
//             var sut = new SimplePatchService();
//             sut.UnPatch(gameType, simplePatchType, TmpPath);
//
//             Assert.That(File.ReadAllBytes(TmpPath), Is.EquivalentTo(expected));
//         }
//         finally
//         {
//             if (File.Exists(TmpPath))
//             {
//                 File.Delete(TmpPath);
//             }
//         }
//     }
// }
