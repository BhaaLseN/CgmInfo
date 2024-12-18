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
* `CgmInfoGui`: A GUI application written in WPF to present a view on various aspects of the metafile, such as its metafile structure, its application structure and other uses such as hotspot information or [XML Companion file](http://www.w3.org/TR/webcgm20/WebCGM20-XCF.html) creation.

### Components
CgmInfo is fully written in C# and should work on all platforms that implement at least .NET Standard 2.0; with the CLI application CgmInfoCmd requiring .NET 9.0 or later. However, the GUI application CgmInfoGui is written using WPF and uses various third party libraries to provide a functional user interface, which will most likely be limited to run on Windows.
- [Extended WPF Toolkit Community Edition](https://github.com/xceedsoftware/wpftoolkit/) by [Xceed](https://xceed.com/) for
  - [Docking Tabs](https://github.com/xceedsoftware/wpftoolkit/wiki/AvalonDock) for Tabbed UI that can be undocked/positioned as necessary (provided by `AvalonDock`)
  - [Zoombox](https://github.com/xceedsoftware/wpftoolkit/wiki/Zoombox) to provide Zooming/Panning capabilities (experimental branch only for now)
  - [Property Grid](https://github.com/xceedsoftware/wpftoolkit/wiki/PropertyGrid) to provide detailed information on various views
- An updated version of [XML-to-WPF FlowDocument](http://xmlflowdocument.codeplex.com/) by [Chris Cavanagh](https://chriscavanagh.wordpress.com/2008/11/02/rendering-xml-as-a-flowdocument/) that better suits my needs.
