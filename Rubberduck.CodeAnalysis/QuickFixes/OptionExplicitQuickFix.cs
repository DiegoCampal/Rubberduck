using System;
using Rubberduck.Inspections.Abstract;
using Rubberduck.Inspections.Concrete;
using Rubberduck.Parsing.Grammar;
using Rubberduck.Parsing.Inspections.Abstract;
using Rubberduck.Parsing.Rewriter;

namespace Rubberduck.Inspections.QuickFixes
{
    /// <summary>
    /// Adds 'Option Explicit' to the top of code modules.
    /// </summary>
    /// <inspections>
    /// <inspection name="OptionExplicitInspection" />
    /// </inspections>
    /// <canfix procedure="false" module="false" project="true" />
    /// <example>
    /// <before>
    /// <![CDATA[
    /// 
    /// Public Sub DoSomething()
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
    public sealed class OptionExplicitQuickFix : QuickFixBase
    {
        public OptionExplicitQuickFix()
            : base(typeof(OptionExplicitInspection))
        {}

        public override void Fix(IInspectionResult result, IRewriteSession rewriteSession)
        {
            var rewriter = rewriteSession.CheckOutModuleRewriter(result.QualifiedSelection.QualifiedName);
            rewriter.InsertBefore(0, Tokens.Option + ' ' + Tokens.Explicit + Environment.NewLine);
        }

        public override string Description(IInspectionResult result) => Resources.Inspections.QuickFixes.OptionExplicitQuickFix;
        

        public override bool CanFixInProcedure => false;
        public override bool CanFixInModule => false;
        public override bool CanFixInProject => true;
    }
}