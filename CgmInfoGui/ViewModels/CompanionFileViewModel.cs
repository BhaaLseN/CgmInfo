using System.Xml.Linq;
using CgmInfo.Commands;
using CgmInfo.Traversal;
using CgmInfoGui.Traversal;
using Dock.Model.Mvvm.Controls;

namespace CgmInfoGui.ViewModels;

public class CompanionFileViewModel : Document, ICommandReceiver
{
    private XDocument? _xcfDocument;
    public XDocument? XCFDocument
    {
        get { return _xcfDocument; }
        set { SetProperty(ref _xcfDocument, value); }
    }

    public CompanionFileViewModel()
    {
        Title = "XCF";
        CanClose = false;
    }

    ICommandAcceptor ICommandReceiver.BeginReceiving() => new Acceptor(this);
    private sealed class Acceptor(CompanionFileViewModel parent) : ICommandAcceptor
    {
        private readonly XCFDocumentBuilderVisitor _visitor = new ();
        private readonly XCFDocumentContext _parameters = new();
        public void Accept(Command command) => command.Accept(_visitor, _parameters);
        public void Dispose() => parent.XCFDocument = _parameters.XCF;
    }
}
