// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PorterStemmer.cs" company="">
//   
// </copyright>
// <summary>
//   The porter stemmer.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace WebApi.Tools.Parser
{
    using System.Text.RegularExpressions;

    /// <summary>
    ///     The porter stemmer.
    /// </summary>
    internal static class PorterStemmer
    {
        /// <summary>
        ///     The i.
        /// </summary>
        public static readonly Regex I = new Regex("и$");

        /// <summary>
        ///     The p.
        /// </summary>
        public static readonly Regex P = new Regex("ь$");

        /// <summary>
        ///     The adjective.
        /// </summary>
        private static readonly Regex Adjective = new Regex(
            "(ее|ие|ые|ое|ими|ыми|ей|ий|ый|ой|ем|им|ым|ом|его|ого|ему|ому|их|ых|ую|юю|ая|яя|ою|ею)$");

        /// <summary>
        ///     The der.
        /// </summary>
        private static readonly Regex Der = new Regex("ость?$");

        /// <summary>
        ///     The derivational.
        /// </summary>
        private static readonly Regex Derivational = new Regex(".*[^аеиоуыэюя]+[аеиоуыэюя].*ость?$");

        /// <summary>
        ///     The nn.
        /// </summary>
        private static readonly Regex Nn = new Regex("нн$");

        /// <summary>
        ///     The noun.
        /// </summary>
        private static readonly Regex Noun = new Regex(
            "(а|ев|ов|ие|ье|е|иями|ями|ами|еи|ии|и|ией|ей|ой|ий|й|иям|ям|ием|ем|ам|ом|о|у|ах|иях|ях|ы|ь|ию|ью|ю|ия|ья|я)$");

        /// <summary>
        ///     The participle.
        /// </summary>
        private static readonly Regex Participle = new Regex("((ивш|ывш|ующ)|((?<=[ая])(ем|нн|вш|ющ|щ)))$");

        /// <summary>
        ///     The perfectiveground.
        /// </summary>
        private static readonly Regex Perfectiveground =
            new Regex("((ив|ивши|ившись|ыв|ывши|ывшись)|((<;=[ая])(в|вши|вшись)))$");

        /// <summary>
        ///     The reflexive.
        /// </summary>
        private static readonly Regex Reflexive = new Regex("(с[яь])$");

        /// <summary>
        ///     The rvre.
        /// </summary>
        private static readonly Regex Rvre = new Regex("^(.*?[аеиоуыэюя])(.*)$");

        /// <summary>
        ///     The superlative.
        /// </summary>
        private static readonly Regex Superlative = new Regex("(ейше|ейш)$");

        /// <summary>
        ///     The verb.
        /// </summary>
        private static readonly Regex Verb = new Regex(
            "((ила|ыла|ена|ейте|уйте|ите|или|ыли|ей|уй|ил|ыл|им|ым|ен|ило|ыло|ено|ят|ует|уют|ит|ыт|ены|ить|ыть|ишь|ую|ю)|((?<=[ая])(ла|на|ете|йте|ли|й|л|ем|н|ло|но|ет|ют|ны|ть|ешь|нно)))$");

        /// <summary>
        /// The replace first.
        /// </summary>
        /// <param name="collection">
        /// The collection.
        /// </param>
        /// <param name="part">
        /// The part.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string ReplaceFirst(MatchCollection collection, string part)
        {
            if (collection.Count == 0) return part;

            var stringTemp = part;
            for (var i = 0; i < collection.Count; i++)
            {
                var groupCollection = collection[i].Groups;
                if (!stringTemp.Contains(groupCollection[i].ToString())) continue;

                var deletePart = groupCollection[i].ToString();
                stringTemp = stringTemp.Replace(deletePart, string.Empty);
            }

            return stringTemp;
        }

        /// <summary>
        /// The transforming word.
        /// </summary>
        /// <param name="word">
        /// The word.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string TransformingWord(string word)
        {
            word = word.ToLower();
            word = word.Replace('ё', 'е');
            var m = Rvre.Matches(word);
            if (m.Count <= 0) return word;

            var match = m[0]; // only one match in this case
            var groupCollection = match.Groups;
            var pre = groupCollection[1].ToString();
            var rv = groupCollection[2].ToString();

            var temp = Perfectiveground.Matches(rv);
            var stringTemp = ReplaceFirst(temp, rv);

            if (stringTemp.Equals(rv))
            {
                var tempRV = Reflexive.Matches(rv);
                rv = ReplaceFirst(tempRV, rv);
                temp = Adjective.Matches(rv);
                stringTemp = ReplaceFirst(temp, rv);
                if (!stringTemp.Equals(rv))
                {
                    rv = stringTemp;
                    tempRV = Participle.Matches(rv);
                    rv = ReplaceFirst(tempRV, rv);
                }
                else
                {
                    temp = Verb.Matches(rv);
                    stringTemp = ReplaceFirst(temp, rv);
                    if (stringTemp.Equals(rv))
                    {
                        tempRV = Noun.Matches(rv);
                        rv = ReplaceFirst(tempRV, rv);
                    }
                    else
                    {
                        rv = stringTemp;
                    }
                }
            }
            else
            {
                rv = stringTemp;
            }

            var tempRv = I.Matches(rv);
            rv = ReplaceFirst(tempRv, rv);
            if (Derivational.Matches(rv).Count > 0)
            {
                tempRv = Der.Matches(rv);
                rv = ReplaceFirst(tempRv, rv);
            }

            temp = P.Matches(rv);
            stringTemp = ReplaceFirst(temp, rv);
            if (stringTemp.Equals(rv))
            {
                tempRv = Superlative.Matches(rv);
                rv = ReplaceFirst(tempRv, rv);
                tempRv = Nn.Matches(rv);
                rv = ReplaceFirst(tempRv, rv);
            }
            else
            {
                rv = stringTemp;
            }

            word = pre + rv;

            return word;
        }
    }
}