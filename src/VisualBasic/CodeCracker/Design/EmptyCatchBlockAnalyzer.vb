﻿Imports System.Collections.Immutable
Imports Microsoft.CodeAnalysis
Imports Microsoft.CodeAnalysis.Diagnostics
Imports Microsoft.CodeAnalysis.VisualBasic
Imports Microsoft.CodeAnalysis.VisualBasic.Syntax

Namespace Design
    <DiagnosticAnalyzer(LanguageNames.VisualBasic)>
    Public Class EmptyCatchBlockAnalyzer
        Inherits DiagnosticAnalyzer

        Public Shared ReadOnly Id As String = DiagnosticId.EmptyCatchBlock.ToDiagnosticId()
        Public Const Title As String = "Catch block cannot be empty"
        Public Const MessageFormat As String = "{0}"
        Public Const Category As String = SupportedCategories.Design
        Public Const Description As String = "An empty catch block suppresses all errors and shouldn't be used.
If the error is expected, consider logging it or changing the control flow such that it is explicit."
        Protected Shared Rule As DiagnosticDescriptor = New DiagnosticDescriptor(
            Id,
            Title,
            MessageFormat,
            Category,
            DiagnosticSeverity.Warning,
            isEnabledByDefault:=True,
            description:=Description,
            helpLinkUri:=HelpLink.ForDiagnostic(DiagnosticId.EmptyCatchBlock))

        Public Overrides ReadOnly Property SupportedDiagnostics() As ImmutableArray(Of DiagnosticDescriptor) = ImmutableArray.Create(Rule)

        Public Overrides Sub Initialize(context As AnalysisContext)
            context.RegisterSyntaxNodeAction(AddressOf Analyzer, SyntaxKind.CatchBlock)
        End Sub

        Private Sub Analyzer(context As SyntaxNodeAnalysisContext)
            Dim catchBlock = DirectCast(context.Node, CatchBlockSyntax)
            If (catchBlock.Statements.Count <> 0) Then Exit Sub
            Dim diag = Diagnostic.Create(Rule, catchBlock.GetLocation(), "Empty Catch Block.")
            context.ReportDiagnostic(diag)
        End Sub

    End Class
End Namespace
