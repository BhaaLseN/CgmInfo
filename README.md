# CgmInfo
CgmInfo is a fully managed CGM Metafile library based on the ISO/IEC 8632:1999 standard intended for extraction of various informations encoded in either binary or text (as described by Part 3 and 4 of ISO/IEC 8632, respectively). It's licensed under the terms of the GNU General Public License, version 3 or later (GPLv3+).

### Requirements
* Microsoft Visual Studio 2017 (Community Edition or higher to build)
* NuGet to build ```CgmInfoGui```, which is a GUI tool to display Metafile information

## Technical Details

### Projects
* ```CgmInfo```: The main library. Entry point is ```MetafileReader``` in the ```CgmInfo``` Namespace which attempts to determine on whether to read the file as binary or text.
* ```CgmInfoCmd```: A CLI application printing a semi-spammy overview of the Metafile. Mostly included for testing and reference only.
* ```CgmInfoGui```: A GUI application written in WPF to present a view on various aspects of the metafile, such as its metafile structure, its application structure and other uses such as hotspot information or [XML Companion file](http://www.w3.org/TR/webcgm20/WebCGM20-XCF.html) creation.

### Components
CgmInfo is fully written in C# and might aswell work within PCL constraints (which are not provided as of now). However, its GUI application CgmInfoGui uses various third party libraries to provide a functional user interface.
- [Extended WPF Toolkit Community Edition](http://wpftoolkit.codeplex.com/) by [Xceed](http://wpftoolkit.com/) for
  - [Docking Tabs](http://wpftoolkit.codeplex.com/wikipage?title=AvalonDock) for Tabbed UI that can be undocked/positioned as necessary (provided by ```AvalonDock```)
  - [Zoombox](http://wpftoolkit.codeplex.com/wikipage?title=Zoombox) to provide Zooming/Panning capabilities (experimental branch only for now)
  - [Property Grid](http://wpftoolkit.codeplex.com/wikipage?title=PropertyGrid) to provide detailed information on various views
- An updated version of [XML-to-WPF FlowDocument](http://xmlflowdocument.codeplex.com/) by [Chris Cavanagh](https://chriscavanagh.wordpress.com/2008/11/02/rendering-xml-as-a-flowdocument/) that better suits my needs.
