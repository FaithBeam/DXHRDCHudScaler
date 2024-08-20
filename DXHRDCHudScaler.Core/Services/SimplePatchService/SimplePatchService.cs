// using DXHRDCHudScaler.Core.Models;
// using PatternFinder;
//
// namespace DXHRDCHudScaler.Core.Services.SimplePatchService;
//
// public enum SimplePatchStatus
// {
//     Patched,
//     UnPatched
// }
//
// public enum SimplePatchType
// {
//     SkipCutscenes,
//     SkipIntroVideos
// }
//
// /// <summary>
// /// Patches that can only be enabled or disabled with no extra parameters
// /// </summary>
// // public class SimplePatchService : ISimplePatchService
// // {
// //     private static int SkipCutsceneOffsetOg = 0x3CAF2E;
// //     private static byte[] SkipCutsceneUnpatchedOg = [0x8A, 0x58, 0x49];
// //     private static readonly Pattern.Byte[] OriginalSkipIntroVideosUnpatchedPatternBytes =
// //         Pattern.Transform("740A83F80273");
// //     private static readonly Pattern.Byte[] OriginalSkipIntroVideosPatchedPatternBytes =
// //         Pattern.Transform("909083F80273");
// //     private static readonly Pattern.Byte[] DcSkipIntroVideosUnpatchedPatternBytes =
// //         Pattern.Transform("740984C07505");
// //     private static readonly Pattern.Byte[] DcSkipIntroVideosPatchedPatternBytes = Pattern.Transform(
// //         "909084C07505"
// //     );
// //
// //     public bool CanPatch(GameType? gameType, SimplePatchType simplePatchType, string pathToExe)
// //     {
// //         if (!File.Exists(pathToExe) || gameType is null)
// //         {
// //             return false;
// //         }
// //
// //         var exeBytes = File.ReadAllBytes(pathToExe);
// //
// //         return (gameType, simplePatchType) switch
// //         {
// //             (GameType.Original, SimplePatchType.SkipCutscenes)
// //                 => Pattern.FindAll(
// //                     exeBytes,
// //                     OriginalSkipCutsceneUnpatchedPatternBytes,
// //                     out var offsets
// //                 )
// //                     && offsets.Count == 1,
// //             (GameType.Original, SimplePatchType.SkipIntroVideos)
// //                 => Pattern.FindAll(
// //                     exeBytes,
// //                     OriginalSkipIntroVideosUnpatchedPatternBytes,
// //                     out var offsets
// //                 )
// //                     && offsets.Count == 1,
// //             (GameType.DirectorsCut, SimplePatchType.SkipCutscenes)
// //                 => Pattern.FindAll(
// //                     exeBytes,
// //                     OriginalSkipCutsceneUnpatchedPatternBytes, // this is intentional
// //                     out var offsets
// //                 )
// //                     && offsets.Count == 1,
// //             (GameType.DirectorsCut, SimplePatchType.SkipIntroVideos)
// //                 => Pattern.FindAll(
// //                     exeBytes,
// //                     DcSkipIntroVideosUnpatchedPatternBytes,
// //                     out var offsets
// //                 )
// //                     && offsets.Count == 1,
// //             _
// //                 => throw new ArgumentOutOfRangeException(
// //                     nameof(simplePatchType),
// //                     simplePatchType,
// //                     null
// //                 )
// //         };
// //     }
// //
// //     public bool CanUnPatch(GameType? gameType, SimplePatchType simplePatchType, string pathToExe)
// //     {
// //         if (!File.Exists(pathToExe) || gameType is null)
// //         {
// //             return false;
// //         }
// //
// //         var exeBytes = File.ReadAllBytes(pathToExe);
// //
// //         return (gameType, simplePatchType) switch
// //         {
// //             (GameType.Original, SimplePatchType.SkipCutscenes)
// //                 => Pattern.FindAll(
// //                     exeBytes,
// //                     OriginalSkipCutscenePatchedPatternBytes,
// //                     out var offsets
// //                 )
// //                     && offsets.Count == 1,
// //             (GameType.Original, SimplePatchType.SkipIntroVideos)
// //                 => Pattern.FindAll(
// //                     exeBytes,
// //                     OriginalSkipIntroVideosPatchedPatternBytes,
// //                     out var offsets
// //                 )
// //                     && offsets.Count == 1,
// //             (GameType.DirectorsCut, SimplePatchType.SkipCutscenes)
// //                 => Pattern.FindAll(
// //                     exeBytes,
// //                     OriginalSkipCutscenePatchedPatternBytes, // this is intentional
// //                     out var offsets
// //                 )
// //                     && offsets.Count == 1,
// //             (GameType.DirectorsCut, SimplePatchType.SkipIntroVideos)
// //                 => Pattern.FindAll(exeBytes, DcSkipIntroVideosPatchedPatternBytes, out var offsets)
// //                     && offsets.Count == 1,
// //             _
// //                 => throw new ArgumentOutOfRangeException(
// //                     nameof(simplePatchType),
// //                     simplePatchType,
// //                     null
// //                 )
// //         };
// //     }
// //
// //     public SimplePatchStatus GetCurrentPatchStatus(
// //         GameType? gameType,
// //         SimplePatchType simplePatchType,
// //         string pathToExe
// //     ) =>
// //         CanPatch(gameType, simplePatchType, pathToExe)
// //             ? SimplePatchStatus.UnPatched
// //             : SimplePatchStatus.Patched;
// //
// //     public void Patch(GameType? gameType, SimplePatchType simplePatchType, string pathToExe)
// //     {
// //         if (gameType is null)
// //         {
// //             throw new Exception("Unknown game type");
// //         }
// //
// //         var exeBytes = File.ReadAllBytes(pathToExe);
// //
// //         switch (simplePatchType)
// //         {
// //             case SimplePatchType.SkipCutscenes:
// //                 exeBytes = PatchSkipCutscenes(gameType, exeBytes);
// //                 break;
// //             case SimplePatchType.SkipIntroVideos:
// //                 exeBytes = PatchSkipIntroVideos(gameType, exeBytes);
// //                 break;
// //             default:
// //                 throw new ArgumentOutOfRangeException(
// //                     nameof(simplePatchType),
// //                     simplePatchType,
// //                     null
// //                 );
// //         }
// //
// //         File.WriteAllBytes(pathToExe, exeBytes);
// //     }
// //
// //     private byte[] PatchSkipIntroVideos(GameType? gameType, byte[] exeBytes)
// //     {
// //         long offset;
// //         _ = gameType switch
// //         {
// //             GameType.Original
// //                 => Pattern.Find(exeBytes, OriginalSkipIntroVideosUnpatchedPatternBytes, out offset),
// //             GameType.DirectorsCut
// //                 => Pattern.Find(exeBytes, DcSkipIntroVideosUnpatchedPatternBytes, out offset),
// //             _ => throw new ArgumentOutOfRangeException(nameof(gameType), gameType, null)
// //         };
// //
// //         exeBytes[offset] = 0x90;
// //         exeBytes[offset + 1] = 0x90;
// //
// //         return exeBytes;
// //     }
// //
// //     private byte[] UnPatchSkipIntroVideos(GameType? gameType, byte[] exeBytes)
// //     {
// //         long offset;
// //         _ = gameType switch
// //         {
// //             GameType.Original
// //                 => Pattern.Find(exeBytes, OriginalSkipIntroVideosPatchedPatternBytes, out offset),
// //             GameType.DirectorsCut
// //                 => Pattern.Find(exeBytes, DcSkipIntroVideosPatchedPatternBytes, out offset),
// //             _ => throw new ArgumentOutOfRangeException(nameof(gameType), gameType, null)
// //         };
// //
// //         exeBytes[offset] = 0x74;
// //         exeBytes[offset + 1] = 0x0A;
// //
// //         return exeBytes;
// //     }
// //
// //     private byte[] PatchSkipCutscenes(GameType? gameType, byte[] exeBytes)
// //     {
// //         long offset;
// //         _ = gameType switch
// //         {
// //             GameType.Original
// //                 => Pattern.Find(exeBytes, OriginalSkipCutsceneUnpatchedPatternBytes, out offset),
// //             GameType.DirectorsCut
// //                 => Pattern.Find(exeBytes, OriginalSkipCutsceneUnpatchedPatternBytes, out offset),
// //             _ => throw new ArgumentOutOfRangeException(nameof(gameType), gameType, null)
// //         };
// //
// //         exeBytes[offset] = 0xB3;
// //         exeBytes[offset + 1] = 0x01;
// //         exeBytes[offset + 2] = 0x90;
// //
// //         return exeBytes;
// //     }
// //
// //     private byte[] UnPatchSkipCutscenes(GameType? gameType, byte[] exeBytes)
// //     {
// //         long offset;
// //         _ = gameType switch
// //         {
// //             GameType.Original
// //                 => Pattern.Find(exeBytes, OriginalSkipCutscenePatchedPatternBytes, out offset),
// //             GameType.DirectorsCut
// //                 => Pattern.Find(exeBytes, OriginalSkipCutscenePatchedPatternBytes, out offset),
// //             _ => throw new ArgumentOutOfRangeException(nameof(gameType), gameType, null)
// //         };
// //
// //         exeBytes[offset] = 0x8A;
// //         exeBytes[offset + 1] = 0x58;
// //         exeBytes[offset + 2] = 0x49;
// //
// //         return exeBytes;
// //     }
// //
// //     public void UnPatch(GameType? gameType, SimplePatchType simplePatchType, string pathToExe)
// //     {
// //         var exeBytes = File.ReadAllBytes(pathToExe);
// //
// //         switch (simplePatchType)
// //         {
// //             case SimplePatchType.SkipCutscenes:
// //                 exeBytes = UnPatchSkipCutscenes(gameType, exeBytes);
// //                 break;
// //             case SimplePatchType.SkipIntroVideos:
// //                 exeBytes = UnPatchSkipIntroVideos(gameType, exeBytes);
// //                 break;
// //             default:
// //                 throw new ArgumentOutOfRangeException(
// //                     nameof(simplePatchType),
// //                     simplePatchType,
// //                     null
// //                 );
// //         }
// //
// //         File.WriteAllBytes(pathToExe, exeBytes);
// //     }
// }
