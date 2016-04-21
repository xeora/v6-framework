Module Module1

    Sub Main()
        ' CAPTURE REGULAR EXPRESSIONS
        '\$  ( ( ([#]+|[\^\-\+\*\~])?(\w+) | (\w+\.)*\w+\@[\w+\.]+  ) \$ | [\.\w\-]+\:\{ | \w(\#\d+(\+)?)?(\[[\.\w\-]+\])?\: ( [\.\w\-]+\$ | [\.\w\-]+\:\{ | [\.\w\-]+\?[\.\w\-]+   (\,   (   (\|)?    ( [#\.\^\-\+\*\~]*\w+  |  \=[\S]+  |  (\w+\.)*\w+\@[\w+\.]+ )?   )*  )?  \$ )) | \}\:[\.\w\-]+\:\{ | \}\:[\.\w\-]+ \$           [\w\.\,\-\+]
        Dim CaptureRegEx As String = "\$((([#]+|[\^\-\+\*\~])?(\w+)|(\w+\.)*\w+\@[#\-]*[\w+\.]+)\$|[\.\w\-]+\:\{|\w(\#\d+(\+)?)?(\[[\.\w\-]+\])?\:([\.\w\-]+\$|[\.\w\-]+\:\{|[\.\w\-]+\?[\.\w\-]+(\,((\|)?([#\.\^\-\+\*\~]*([\w+][^\$]*)|\=([\S+][^\$]*)|(\w+\.)*\w+\@[#\-]*[\w+\.]+)?)*)?\$))|\}\:[\.\w\-]+\:\{|\}\:[\.\w\-]+\$"
        Dim BracketedRegExOpening As String = "\$((?<ItemID>\w+)|(?<DirectiveType>\w)(\#\d+(\+)?)?(\[[\.\w\-]+\])?\:(?<ItemID>[\.\w\-]+))\:\{"
        Dim BracketedRegExSeparator As String = "\}:(?<ItemID>[\.\w\-]+)\:\{"
        Dim BracketedRegExClosing As String = "\}:(?<ItemID>[\.\w\-]+)\$"
        ' !---

        Dim MainCapturePattern As Text.RegularExpressions.RegexCompilationInfo =
             New Text.RegularExpressions.RegexCompilationInfo(CaptureRegEx, Text.RegularExpressions.RegexOptions.Multiline, "MainCapturePattern", "Xeora.Web.RegularExpressions", True)
        Dim BracketedControllerOpenPattern As Text.RegularExpressions.RegexCompilationInfo =
            New Text.RegularExpressions.RegexCompilationInfo(BracketedRegExOpening, Text.RegularExpressions.RegexOptions.Multiline, "BracketedControllerOpenPattern", "Xeora.Web.RegularExpressions", True)
        Dim BracketedControllerSeparatorPattern As Text.RegularExpressions.RegexCompilationInfo =
            New Text.RegularExpressions.RegexCompilationInfo(BracketedRegExSeparator, Text.RegularExpressions.RegexOptions.Multiline, "BracketedControllerSeparatorPattern", "Xeora.Web.RegularExpressions", True)
        Dim BracketedControllerClosePattern As Text.RegularExpressions.RegexCompilationInfo =
            New Text.RegularExpressions.RegexCompilationInfo(BracketedRegExClosing, Text.RegularExpressions.RegexOptions.Multiline, "BracketedControllerClosePattern", "Xeora.Web.RegularExpressions", True)

        Dim AssemblyName As Reflection.AssemblyName =
            New Reflection.AssemblyName("Xeora.Web.RegularExpressions, Version=6.0.0.1001, Culture=neutral, PublicKeyToken=null")

        Text.RegularExpressions.Regex.CompileToAssembly(
            New Text.RegularExpressions.RegexCompilationInfo() {
                MainCapturePattern,
                BracketedControllerOpenPattern,
                BracketedControllerSeparatorPattern,
                BracketedControllerClosePattern},
            AssemblyName
        )
    End Sub

End Module
