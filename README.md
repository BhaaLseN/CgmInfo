# CgmInfo
CgmInfo is a fully managed CGM Metafile library based on the ISO/IEC 8632:1999 standard intended for extraction of various informations encoded in either binary or text (as described by Part 3 and 4 of ISO/IEC 8632, respectively). It's licensed under the terms of the GNU General Public License, version 3 or later (GPLv3+).

### Requirements
* Microsoft Visual Studio 2022 (Community Edition or higher to build)
* Workloads for .NET desktop development and .NET Core cross-platform development
* NuGet to build `CgmInfoGui`, which is a GUI tool to display Metafile information

## Technical Details

### Projects
* `CgmInfo`: The main library, targetting [.NET Standard 2.0](https://github.com/dotnet/standard). Entry point is `MetafileReader` in the `CgmInfo` Namespace which attempts to determine on whether to read the file as binary or text.
* `CgmInfoCmd`: A CLI application printing a semi-spammy overview of the Metafile, targetting [.NET 9.0](https://github.com/dotnet/core). Mostly included for testing and reference only.
* `CgmInfoGui`: A GUI application written in [AvaloniaUI](https://github.com/AvaloniaUI/Avalonia/) to present a view on various aspects of the metafile, such as its metafile structure, its application structure and other uses such as hotspot information or [XML Companion file](http://www.w3.org/TR/webcgm20/WebCGM20-XCF.html) creation.

### Components
CgmInfo is fully written in C# and should work on all platforms that implement at least .NET Standard 2.0; with the CLI application CgmInfoCmd and the GUI application CgmInfoGui requiring .NET 9.0 (or later).
- [Avalonia](https://github.com/AvaloniaUI/Avalonia/) for the cross-platform UI framework.
- [AvaloniaEdit](https://github.com/AvaloniaUI/AvaloniaEdit/) for XML rendering, as there is no `FlowDocument` equivalent in Avalonia (yet).
  - An updated version of [XML-to-WPF FlowDocument](http://xmlflowdocument.codeplex.com/) by [Chris Cavanagh](https://chriscavanagh.wordpress.com/2008/11/02/rendering-xml-as-a-flowdocument/) that better suits my needs.
- [AvaloniaHex](https://github.com/Washi1337/AvaloniaHex/) for viewing of unsupported/unexpected commands.
- [Dock.Avalonia](https://github.com/wieslawsoltes/Dock/) for the general layout.
- [ImageSharp](https://github.com/SixLabors/ImageSharp/) for conversion of embedded raster files, since Avalonia (and Skia) don't support some of the formats.
