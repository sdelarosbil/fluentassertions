﻿using System.Text;

namespace FluentAssertions.CallerIdentification;

internal class SingleLineCommentParsingStrategy : IParsingStrategy
{
    private bool isCommentContext;

    public ParsingState Parse(char symbol, StringBuilder statement)
    {
        if (isCommentContext)
        {
            return ParsingState.GoToNextSymbol;
        }

#pragma warning disable SA1010 // https://github.com/DotNetAnalyzers/StyleCopAnalyzers/pull/3507
        var doesSymbolStartComment = symbol is '/' && statement is [.., '/'];
#pragma warning restore SA1010

        if (!doesSymbolStartComment)
        {
            return ParsingState.InProgress;
        }

        isCommentContext = true;
        statement.Remove(statement.Length - 1, 1);
        return ParsingState.GoToNextSymbol;
    }

    public bool IsWaitingForContextEnd()
    {
        return isCommentContext;
    }

    public void NotifyEndOfLineReached()
    {
        if (isCommentContext)
        {
            isCommentContext = false;
        }
    }
}
