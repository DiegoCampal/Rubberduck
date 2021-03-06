using Rubberduck.Inspections.Abstract;
using Rubberduck.Inspections.Concrete;
using Rubberduck.Parsing.Inspections.Abstract;
using Rubberduck.Parsing.Rewriter;

namespace Rubberduck.Inspections.QuickFixes
{
    /// <summary>
    /// Replaces a late-bound Application.{Member} call to the corresponding early-bound Application.WorksheetFunction.{Member} call.
    /// </summary>
    /// <inspections>
    /// <inspection name="ApplicationWorksheetFunctionInspection" />
    /// </inspections>
    /// <canfix procedure="true" module="true" project="true" />
    /// <example>
    /// <before>
    /// <![CDATA[
    /// Public Sub DoSomething()
    ///     Debug.Print Application.Sum(Sheet1.Range("A1:A10"))
    /// End Sub
    /// ]]>
    /// </before>
    /// <after>
    /// <![CDATA[
    /// Public Sub DoSomething()
    ///     Debug.Print Application.WorksheetFunction.Sum(Sheet1.Range("A1:A10"))
    /// End Sub
    /// ]]>
    /// </after>
    /// </example>
    public sealed class ApplicationWorksheetFunctionQuickFix : QuickFixBase
    {
        public ApplicationWorksheetFunctionQuickFix()
            : base(typeof(ApplicationWorksheetFunctionInspection))
        {}

        public override void Fix(IInspectionResult result, IRewriteSession rewriteSession)
        {
            var rewriter = rewriteSession.CheckOutModuleRewriter(result.QualifiedSelection.QualifiedName);
            rewriter.InsertBefore(result.Context.Start.TokenIndex, "WorksheetFunction.");
        }

        public override string Description(IInspectionResult result) => Resources.Inspections.QuickFixes.ApplicationWorksheetFunctionQuickFix;

        public override bool CanFixInProcedure => true;
        public override bool CanFixInModule => true;
        public override bool CanFixInProject => true;
    }
}
