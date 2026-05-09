# File Converter

## Description

**File Converter** is a very simple tool which allows you to convert and compress one or several file(s) using the context menu of windows explorer.

![File Converter Usage](Resources/FileConverterUsage.gif)

You can download it here: [file-converter.io](https://file-converter.io/?from=readme.md).

You can find more information about what's in File converter and how to use it on the [wiki](https://github.com/Tichau/FileConverter/wiki).

## File Converter 2.3 CustomVer fork

This fork provides a custom maintenance build based on File Converter 2.2. It focuses on fixing the main regressions reported after the 2.2 release: FFmpeg conversion freezes, video resizing hangs, NVIDIA NVENC scaling failures, and Microsoft Word document conversion crashes/stalls.

Download the custom build here: [FileConverter-2.3-CustomVer-setup.msi](https://github.com/Th3Nekit/FileConverter/releases/download/v2.3-CustomVer/FileConverter-2.3-CustomVer-setup.msi).

Full technical notes are available in [CUSTOMVER_CHANGES.md](CUSTOMVER_CHANGES.md).

### Issues addressed by 2.3 CustomVer

- **Word / Office document conversion crashes and stalls**: [#745](https://github.com/Tichau/FileConverter/issues/745), [#728](https://github.com/Tichau/FileConverter/issues/728), [#717](https://github.com/Tichau/FileConverter/issues/717), [#714](https://github.com/Tichau/FileConverter/issues/714), [#705](https://github.com/Tichau/FileConverter/issues/705), [#631](https://github.com/Tichau/FileConverter/issues/631).
- **FFmpeg video/audio conversion freezes and hangs**: [#749](https://github.com/Tichau/FileConverter/issues/749), [#739](https://github.com/Tichau/FileConverter/issues/739), [#724](https://github.com/Tichau/FileConverter/issues/724), [#716](https://github.com/Tichau/FileConverter/issues/716), [#700](https://github.com/Tichau/FileConverter/issues/700), [#740](https://github.com/Tichau/FileConverter/issues/740).
- **MKV/audio conversion regressions related to FFmpeg output/progress handling**: [#729](https://github.com/Tichau/FileConverter/issues/729), [#748](https://github.com/Tichau/FileConverter/issues/748).
- **NVIDIA NVENC + scaling failure**: [#713](https://github.com/Tichau/FileConverter/issues/713).

## Donate

File Converter is a personal open source project started in 2014. I have put hundreds of hours adding, refining and tuning File Converter with the goal of making the conversion and compression of files an easy task for everyone.

You can help me by [contributing to the project](https://github.com/Tichau/FileConverter/wiki#contribute), by [making a donation](https://www.paypal.com/donate/?cmd=_donations&business=3BDWQTYTTA3D8&item_name=File+Converter+Donations&currency_code=EUR&Z3JncnB0=) or just by [saying thanks](https://saythanks.io/to/Tichau) :).

## Troubleshooting

If you encounter any problem with File Converter, you can:

* See the already known problems in the [troubleshooting section of the documentation](https://github.com/Tichau/FileConverter/wiki/Troubleshooting).
* Or report an issue on the [bug tracker](https://github.com/Tichau/FileConverter/issues).

## Setup development environment

### Requirements

For File Converter and its explorer extension:

* Visual Studio 2022

For the installer:

* [Wix 5](http://wixtoolset.org/) (will be installed by nuget)
  * [Community Visual Studio Extension](https://marketplace.visualstudio.com/items?itemName=FireGiant.FireGiantHeatWaveDev17)
* [Windows SDK Signing Tools for Desktop Apps](https://developer.microsoft.com/fr-fr/windows/downloads/windows-10-sdk)

## Thanks

Thanks to all the contributors of File Converter project.

### Localization

* Thanks to **Khidreal** and **hugok79** for the Portuguese localization.
* Thanks to **Marhc** for the Brazilian localization.
* Thanks to **Chachak** for the Spanish localization.
* Thanks to **Davide** for the Italian localization.
* Thanks to **nikotschierske** for the German localization.
* Thanks to **Snoopy1866** for the Simplified Chinese localization.
* Thanks to **MayaC0re** for the Turkish localization.
* Thanks to **vishveshjain** for the Hindi localization.
* Thanks to **Mahmoud0Sultan** for the Arabic localization.
* Thanks to **Sedimentary-Rock**, **NeKoOuO** and **PeterDaveHello** for the Traditional Chinese localization.
* Thanks to **CrisBalGreece** for the Greek localization.
* Thanks to **AshiVered** for the Hebrew localization.
* Thanks to **MrHero118** and **Mehrdad32** for the Persian localization.
* Thanks to **crnobog69** for the Serbian localizations.
* Thanks to **oogamiyuta** for the Japanese localization.
* Thanks to **AidyTheWeird** for the Czech localization.
* Thanks to **Alanimdeo** for the Korean localization.
* Thanks to **vrykolakas166** and **thaovd** for the Vietnamese localization.
* Thanks to **iliamak** for the Russian localization.
* Thanks to **itsmefdil** for the Indonesian localization.
* Thanks to **hamzaharoon1314** for the Urdu localization.
* Thanks to **Zyvrec7** and **stohlferenc** for the Hungarian localization.
* Thanks to **Maerek** and **MrPrince419** for the Polish localization.
* Thanks to **rkalitta** for the Swedish localization.

## Middlewares

File converter uses the following middlewares:

**ffmpeg** (v8.0.1) as file conversion software.
Thanks to ffmpeg devs for this awesome open source file conversion tool. [Web site link](https://ffmpeg.org)

**ImageMagick** (v14.10) as image edition and conversion software.
Thanks to image magick devs for this awesome open source image edition software suite.  [Web site link](http://imagemagick.net)
And thanks to dlemstra for the C# wrapper of this software. [Github link](https://github.com/ImageMagick/ImageMagick)

**Ghostscript** (10.02.1) as pdf edition software.
Thanks to ghostscript devs. [Download link](https://www.ghostscript.com/download/gsdnld.html)

**SharpShell** to easily create windows context menu extensions.
Thanks to Dave Kerr for his work on SharpShell. [GitHub link](https://github.com/dwmkerr/sharpshell)

**Ripper** and **yeti.mmedia** for CD Audio extraction.
Thanks to Idael Cardoso for his work on CD Audio ripper. [Code project link](https://www.codeproject.com/Articles/5458/C-Sharp-Ripper)

**Markdown.XAML** for markdown rendering in the wpf application.
Thanks to Bevan Arps for his work on Markdown.XAML. [GitHub link](https://github.com/theunrepentantgeek/Markdown.XAML)

**WpfAnimatedGif** for animated gif rendering in the wpf application.
Thanks to Thomas Levesque for his work on WpfAnimatedGif. [GitHub link](https://github.com/XamlAnimatedGif/WpfAnimatedGif)

## License

File Converter is licensed under the GPL version 3 License.
For more information check the LICENSE.md file in your installation folder or the [gnu website](https://www.gnu.org/licenses/gpl.html).
