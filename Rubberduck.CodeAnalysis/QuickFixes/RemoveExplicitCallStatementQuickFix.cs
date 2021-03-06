using Rubberduck.Inspections.Abstract;
using Rubberduck.Inspections.Concrete;
using Rubberduck.Parsing.Grammar;
using Rubberduck.Parsing.Inspections.Abstract;
using Rubberduck.Parsing.Rewriter;

namespace Rubberduck.Inspections.QuickFixes
{
    /// <summary>
    /// Makes a call statement implicit by removing the 'Call' keyword, adjusting argument list parentheses accordingly.
    /// </summary>
    /// <inspections>
    /// <inspection name="ObsoleteCallStatementInspection" />
    /// </inspections>
    /// <canfix procedure="true" module="true" project="true" />
    /// <example>
    /// <before>
    /// <![CDATA[
    /// Option Explicit
    /// 
    /// Public Sub DoSomething()
    ///     Call DoSomethingElse(42)
    /// End Sub
    /// 
    /// Private Sub DoSomethingElse(ByVal value As Long)
    ///     Debug.Print value
    /// End Sub
    /// ]]>
    /// </before>
    /// <after>
    /// <![CDATA[
    /// Option Explicit
    /// 
    /// Public Sub DoSomething()
    ///     DoSomethingElse 42
    /// End Sub
    /// 
    /// Private Sub DoSomethingElse(ByVal value As Long)
    ///     Debug.Print value
    /// End Sub
    /// ]]>
    /// </after>
    /// </example>
    public sealed class RemoveExplicitCallStatementQuickFix : QuickFixBase
    {
        public RemoveExplicitCallStatementQuickFix()
            : base(typeof(ObsoleteCallStatementInspection))
        {}

        public override void Fix(IInspectionResult result, IRewriteSession rewriteSession)
        {
            var rewriter = rewriteSession.CheckOutModuleRewriter(result.QualifiedSelection.QualifiedName);

            var context = (VBAParser.CallStmtContext)result.Context;
            rewriter.Remove(context.CALL());
            rewriter.Remove(context.whiteSpace());

            // The CALL statement only has arguments if it's an index expression.
            if (context.lExpression() is VBAParser.IndexExprContext indexExpr)
            {
                rewriter.Replace(indexExpr.LPAREN(), " ");
                rewriter.Remove(indexExpr.RPAREN());
            }
        }

        public override string Description(IInspectionResult result) => Resources.Inspections.QuickFixes.RemoveObsoleteStatementQuickFix;

        public override bool CanFixInProcedure => true;
        public override bool CanFixInModule => true;
        public override bool CanFixInProject => true;
    }
}
