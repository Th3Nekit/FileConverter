# File Converter 2.3 CustomVer

This fork is based on File Converter 2.2 and contains a custom maintenance build focused on fixing the regressions reported after the 2.2 release.

## Issues addressed

- **Word / Office document conversion crashes and stalls**: [#745](https://github.com/Tichau/FileConverter/issues/745), [#728](https://github.com/Tichau/FileConverter/issues/728), [#717](https://github.com/Tichau/FileConverter/issues/717), [#714](https://github.com/Tichau/FileConverter/issues/714), [#705](https://github.com/Tichau/FileConverter/issues/705), [#631](https://github.com/Tichau/FileConverter/issues/631).
- **FFmpeg video/audio conversion freezes and hangs**: [#749](https://github.com/Tichau/FileConverter/issues/749), [#739](https://github.com/Tichau/FileConverter/issues/739), [#724](https://github.com/Tichau/FileConverter/issues/724), [#716](https://github.com/Tichau/FileConverter/issues/716), [#700](https://github.com/Tichau/FileConverter/issues/700), [#740](https://github.com/Tichau/FileConverter/issues/740).
- **MKV/audio conversion regressions related to FFmpeg output/progress handling**: [#729](https://github.com/Tichau/FileConverter/issues/729), [#748](https://github.com/Tichau/FileConverter/issues/748).
- **NVIDIA NVENC + scaling failure**: [#713](https://github.com/Tichau/FileConverter/issues/713).

## Fixed

- **FFmpeg video/audio conversion freezes**
  - Fixed a deadlock caused by redirecting FFmpeg `stdout` without reading it.
  - FFmpeg now drains both `stdout` and `stderr` asynchronously.
  - Progress parsing was updated to handle `out_time`, `out_time_ms`, `out_time_us`, and duration lines without a bitrate value.

- **Video resizing hangs**
  - The resize/conversion pipeline no longer blocks when FFmpeg writes progress data to `stdout`.
  - Remaining-time calculation now ignores invalid progress values such as `NaN` and `Infinity` instead of risking UI errors.

- **NVIDIA NVENC + scaling failure**
  - Kept NVENC hardware encoding, but avoided the problematic CUDA decoding / `scale_cuda` path for transform filters.
  - This prevents `Error reinitializing filters` / `Impossible to convert between the formats supported by the filter` when scaling videos with NVIDIA acceleration enabled.

- **Word document conversion crashes and stalls**
  - Added retry handling for transient Microsoft Office COM errors such as `RPC_E_CALL_REJECTED`, `RPC_E_SERVERCALL_RETRYLATER`, and VBA busy states.
  - Word documents and Word application instances are now closed and disposed more reliably.
  - Conversion worker threads now run in STA apartment state for more stable Office automation.

- **Installed build missing Office automation dependencies**
  - Added NetOffice-related DLLs to the MSI package: `NetOffice.dll`, `OfficeApi.dll`, `WordApi.dll`, `ExcelApi.dll`, `PowerPointApi.dll`, and `VBIDEApi.dll`.

- **Custom version branding**
  - Updated the application and installer version to `2.3 CustomVer` / `2.3.0`.
  - The generated installer is named `FileConverter-2.3-CustomVer-setup.msi`.

## Build

The project was successfully built with MSBuild 17.14 using:

```powershell
& 'C:\Program Files (x86)\Microsoft Visual Studio\2022\BuildTools\MSBuild\Current\Bin\MSBuild.exe' FileConverter.sln /restore /p:Configuration=Release /p:Platform=x64 /m /v:minimal "/clp:Summary;ErrorsOnly"
```

Build result: `0 errors`, `82 warnings`.
