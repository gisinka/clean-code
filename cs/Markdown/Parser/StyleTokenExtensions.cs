﻿using System.Linq;
using Markdown.Tokens;

namespace Markdown.Parser
{
    internal static class StyleTokenExtensions
    {
        internal static void ValidatePlacedCorrectly(this StyleToken token, string text)
        {
            token.IsCorrect = !(token.IsInsideTextWithDigits(text) || token.IsInsideDifferentWords(text) || token.IsTokenEmpty(text));
        }

        internal static bool IsIntersectWith(this StyleToken thisToken, StyleToken otherToken)
        {
            return thisToken.OpenIndex < otherToken.OpenIndex && otherToken.OpenIndex < thisToken.CloseIndex;
        }

        private static bool IsTokenEmpty(this StyleToken token, string text)
        {
            var tokenContent = token.GetTokenContent(text);

            return tokenContent.Length == 0;
        }

        private static bool IsInsideDifferentWords(this StyleToken token, string text)
        {
            var openInsideWord = IsSeparatorInsideWord(token.OpenIndex, token.Separator.Length, text);
            var closeInsideWord = IsSeparatorInsideWord(token.CloseIndex, token.Separator.Length, text);

            var tokenContent = token.GetTokenContent(text);

            return (openInsideWord || closeInsideWord) && tokenContent.Any(x => x == ' ');
        }

        private static bool IsInsideTextWithDigits(this StyleToken token, string text)
        {
            var openInsideWord = IsSeparatorInsideTextWithDigits(token.OpenIndex, token.Separator.Length, text);
            var closeInsideWord = IsSeparatorInsideTextWithDigits(token.CloseIndex, token.Separator.Length, text);

            var tokenContent = token.GetTokenContent(text);

            return (openInsideWord || closeInsideWord) && tokenContent.Any(x => x == ' ');
        }

        private static bool IsSeparatorInsideWord(int index, int separatorLength, string text)
        {
            var isLeftLetter = index > 0 && char.IsLetter(text[index - 1]);

            var isRightLetter = index + separatorLength < text.Length - 1 &&
                                char.IsLetter(text[index + separatorLength]);

            return isLeftLetter && isRightLetter;
        }

        private static bool IsSeparatorInsideTextWithDigits(int index, int separatorLength, string text)
        {
            var isLeftLetter = index > 0 && char.IsDigit(text[index - 1]);

            var isRightLetter = index + separatorLength < text.Length - 1 &&
                                char.IsDigit(text[index + separatorLength]);

            return isLeftLetter && isRightLetter;
        }

        private static string GetTokenContent(this StyleToken token, string text)
        {
            var contentStartIndex = token.OpenIndex + token.Separator.Length;
            var contentLength = token.CloseIndex - contentStartIndex;

            return text.Substring(contentStartIndex, contentLength);
        }
    }
}