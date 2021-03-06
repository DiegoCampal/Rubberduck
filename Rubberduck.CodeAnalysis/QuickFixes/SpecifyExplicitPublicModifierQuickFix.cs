using Rubberduck.Inspections.Abstract;
using Rubberduck.Inspections.Concrete;
using Rubberduck.Parsing.Inspections.Abstract;
using Rubberduck.Parsing.Rewriter;

namespace Rubberduck.Inspections.QuickFixes
{
    /// <summary>
    /// Introduces an explicit 'Public' access modifier for a procedure that is implicitly public.
    /// </summary>
    /// <inspections>
    /// <inspection name="ImplicitPublicMemberInspection" />
    /// </inspections>
    /// <canfix procedure="false" module="true" project="true" />
    /// <example>
    /// <before>
    /// <![CDATA[
    /// Option Explicit
    /// 
    /// Sub DoSomething()
    ///     Debug.Print 42
    /// End Sub
    /// ]]>
    /// </before>
    /// <after>
    /// <![CDATA[
    /// Option Explicit
    /// 
    /// Public Sub DoSomething()
    ///     Debug.Print 42
    /// End Sub
    /// ]]>
    /// </after>
    /// </example>
    public sealed class SpecifyExplicitPublicModifierQuickFix : QuickFixBase
    {
        public SpecifyExplicitPublicModifierQuickFix()
            : base(typeof(ImplicitPublicMemberInspection))
        {}

        public override void Fix(IInspectionResult result, IRewriteSession rewriteSession)
        {
            var rewriter = rewriteSession.CheckOutModuleRewriter(result.Target.QualifiedModuleName);
            rewriter.InsertBefore(result.Context.Start.TokenIndex, "Public ");
        }

        public override string Description(IInspectionResult result) => Resources.Inspections.QuickFixes.SpecifyExplicitPublicModifierQuickFix;

        public override bool CanFixInProcedure => false;
        public override bool CanFixInModule => true;
        public override bool CanFixInProject => true;
    }
}