using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using MarkdownConvertor;
using MarkdownConvertor.ITokens;
using NUnit.Framework;

namespace MarkdownConvertorTests
{
    [TestFixture]
    public class TagValidatorTests
    {
        private static IEnumerable<TestCaseData> Tests()
        {
            yield return new TestCaseData(new List<List<IToken>>
            {
                new List<IToken>
                {
                    new TextToken("abc"),
                    new TextToken("\n")
                }
            }, new IToken[]
            {
                new TextToken("abc"),
                new TextToken("\n")
            }).SetName("Should be TextTokens when no tags");
            yield return new TestCaseData(new List<List<IToken>>
            {
                new List<IToken>
                {
                    new TextToken("_"),
                    new TextToken("abc"),
                    new TextToken("\n")
                }
            }, new IToken[]
            {
                new TextToken("_"),
                new TextToken("abc"),
                new TextToken("\n")
            }).SetName("Should be TextTokens when non-paired tags");
            yield return new TestCaseData(new List<List<IToken>>
            {
                new List<IToken>
                {
                    new TextToken("__"),
                    new TextToken("abc"),
                    new TextToken("_"),
                    new TextToken("\n")
                }
            }, new IToken[]
            {
                new TextToken("__"),
                new TextToken("abc"),
                new TextToken("_"),
                new TextToken("\n")
            }).SetName("Should be TextTokens when non-paired tags");
            yield return new TestCaseData(new List<List<IToken>>
            {
                new List<IToken>
                {
                    new TagToken("# "),
                    new TextToken("abc"),
                    new TextToken("\n")
                }
            }, new IToken[]
            {
                new TagToken("# "),
                new TextToken("abc"),
                new TextToken("\n")
            }).SetName("Should be valid header tags when tags are correct");
            yield return new TestCaseData(new List<List<IToken>>
            {
                new List<IToken>
                {
                    new TagToken("_", true),
                    new TextToken("dgv"),
                    new TagToken("_"),
                    new TextToken("\n")
                }
            }, new IToken[]
            {
                new TagToken("_", true),
                new TextToken("dgv"),
                new TagToken("_"),
                new TextToken("\n")
            }).SetName("Should be valid italic tags when tags are correct");
            yield return new TestCaseData(new List<List<IToken>>
            {
                new List<IToken>
                {
                    new TagToken("__", true),
                    new TextToken("dgv"),
                    new TagToken("__"),
                    new TextToken("\n")
                }
            }, new IToken[]
            {
                new TagToken("__", true),
                new TextToken("dgv"),
                new TagToken("__"),
                new TextToken("\n")
            }).SetName("Should be valid strong tags when tags are correct");
            yield return new TestCaseData(new List<List<IToken>>
            {
                new List<IToken>
                {
                    new TextToken("as"),
                    new TagToken("# "),
                    new TextToken("sd"),
                    new TextToken("\n")
                }
            }, new IToken[]
            {
                new TextToken("as"),
                new TextToken("# "),
                new TextToken("sd"),
                new TextToken("\n")
            }).SetName("Header should be TextToken when inside word");
            yield return new TestCaseData(new List<List<IToken>>
            {
                new List<IToken>
                {
                    new TagToken("_"),
                    new TextToken("ab "),
                    new TagToken("_"),
                    new TextToken("\n")
                }
            }, new IToken[]
            {
                new TextToken("_"),
                new TextToken("ab "),
                new TextToken("_"),
                new TextToken("\n")
            }).SetName("Should be TextToken when whitespace before closing tag");
            yield return new TestCaseData(new List<List<IToken>>
            {
                new List<IToken>
                {
                    new TagToken("_"),
                    new TextToken(" ab"),
                    new TagToken("_"),
                    new TextToken("\n")
                }
            }, new IToken[]
            {
                new TextToken("_"),
                new TextToken(" ab"),
                new TextToken("_"),
                new TextToken("\n")
            }).SetName("Should be TextToken when whitespace after opening tag");
            yield return new TestCaseData(new List<List<IToken>>
            {
                new List<IToken>
                {
                    new TextToken("ab"),
                    new TagToken("_"),
                    new TextToken("cd fg"),
                    new TagToken("_"),
                    new TextToken("oi"),
                    new TextToken("\n")
                }
            }, new IToken[]
            {
                new TextToken("ab"),
                new TextToken("_"),
                new TextToken("cd fg"),
                new TextToken("_"),
                new TextToken("oi"),
                new TextToken("\n")
            }).SetName("Should be TextToken when tags inside different words");
            yield return new TestCaseData(new List<List<IToken>>
            {
                new List<IToken>
                {
                    new TagToken("_"),
                    new TextToken("abc "),
                    new TagToken("_"),
                    new TextToken("ghd idk"),
                    new TagToken("_"),
                    new TextToken("\n")
                }
            }, new IToken[]
            {
                new TagToken("_", true),
                new TextToken("abc "),
                new TextToken("_"),
                new TextToken("ghd idk"),
                new TagToken("_"),
                new TextToken("\n")
            }).SetName("Should be TextToken when tag is invalid");
            yield return new TestCaseData(new List<List<IToken>>
            {
                new List<IToken>
                {
                    new TextToken("aa "),
                    new TagToken("__"),
                    new TagToken("__"),
                    new TextToken(" aa"),
                    new TextToken("\n")
                }
            }, new IToken[]
            {
                new TextToken("aa "),
                new TextToken("__"),
                new TextToken("__"),
                new TextToken(" aa"),
                new TextToken("\n")
            }).SetName("Should be TextToken when empty string between tags");
            yield return new TestCaseData(new List<List<IToken>>
            {
                new List<IToken>
                {
                    new TextToken("aa "),
                    new TagToken("_"),
                    new TextToken(" "),
                    new TagToken("_"),
                    new TextToken(" aa"),
                    new TextToken("\n")
                }
            }, new IToken[]
            {
                new TextToken("aa "),
                new TextToken("_"),
                new TextToken(" "),
                new TextToken("_"),
                new TextToken(" aa"),
                new TextToken("\n")
            }).SetName("Should be TextToken when whitespaces between tags");
            yield return new TestCaseData(new List<List<IToken>>
            {
                new List<IToken>
                {
                    new TextToken("acd"),
                    new TagToken("_"),
                    new TextToken("12"),
                    new TagToken("_"),
                    new TextToken("3 rf"),
                    new TextToken("\n")
                }
            }, new IToken[]
            {
                new TextToken("acd"),
                new TextToken("_"),
                new TextToken("12"),
                new TextToken("_"),
                new TextToken("3 rf"),
                new TextToken("\n")
            }).SetName("Should be TextToken when tags inside digits");
            yield return new TestCaseData(new List<List<IToken>>
            {
                new List<IToken>
                {
                    new TagToken("_"),
                    new TextToken("ad"),
                    new TagToken("__"),
                    new TextToken("dc"),
                    new TagToken("__"),
                    new TextToken("jn"),
                    new TagToken("_"),
                    new TextToken("\n")
                }
            }, new IToken[]
            {
                new TagToken("_", true),
                new TextToken("ad"),
                new TextToken("__"),
                new TextToken("dc"),
                new TextToken("__"),
                new TextToken("jn"),
                new TagToken("_"),
                new TextToken("\n")
            }).SetName("Should be TextToken when strong tags inside italic");
            yield return new TestCaseData(new List<List<IToken>>
            {
                new List<IToken>
                {
                    new TagToken("__"),
                    new TextToken("ad"),
                    new TagToken("_"),
                    new TextToken("dc"),
                    new TagToken("_"),
                    new TextToken("jn"),
                    new TagToken("__"),
                    new TextToken("\n")
                }
            }, new IToken[]
            {
                new TagToken("__", true),
                new TextToken("ad"),
                new TagToken("_", true),
                new TextToken("dc"),
                new TagToken("_"),
                new TextToken("jn"),
                new TagToken("__"),
                new TextToken("\n")
            }).SetName("Should be TagToken when italic tags inside strong");
            yield return new TestCaseData(new List<List<IToken>>
            {
                new List<IToken>
                {
                    new TagToken("__"),
                    new TextToken("ad"),
                    new TagToken("_"),
                    new TextToken("dc"),
                    new TagToken("__"),
                    new TextToken("jn"),
                    new TagToken("_"),
                    new TextToken("\n")
                }
            }, new IToken[]
            {
                new TextToken("__"),
                new TextToken("ad"),
                new TextToken("_"),
                new TextToken("dc"),
                new TextToken("__"),
                new TextToken("jn"),
                new TextToken("_"),
                new TextToken("\n")
            }).SetName("Should be TextToken when tags intersects");
            yield return new TestCaseData(new List<List<IToken>>
            {
                new List<IToken>
                {
                    new TagToken("_"),
                    new TextToken("ad"),
                    new TagToken("__"),
                    new TextToken("dc"),
                    new TagToken("_"),
                    new TextToken("jn"),
                    new TagToken("__"),
                    new TextToken("\n")
                }
            }, new IToken[]
            {
                new TextToken("_"),
                new TextToken("ad"),
                new TextToken("__"),
                new TextToken("dc"),
                new TextToken("_"),
                new TextToken("jn"),
                new TextToken("__"),
                new TextToken("\n")
            }).SetName("Should be TextToken when tags intersects");
            yield return new TestCaseData(new List<List<IToken>>
            {
                new List<IToken>
                {
                    new ScreenerToken(@"\"),
                    new TagToken("_"),
                    new TagToken("_"),
                    new TextToken("avd"),
                    new TagToken("_"),
                    new TextToken("\n")
                }
            }, new IToken[]
            {
                new TextToken("_"),
                new TagToken("_", true),
                new TextToken("avd"),
                new TagToken("_"),
                new TextToken("\n")
            }).SetName("Should be TextToken when tag is screened");
            yield return new TestCaseData(new List<List<IToken>>
            {
                new List<IToken>
                {
                    new ScreenerToken(@"\"),
                    new ScreenerToken(@"\"),
                    new ScreenerToken(@"\"),
                    new TagToken("_"),
                    new TagToken("_"),
                    new TextToken("ad"),
                    new TextToken("\n")
                }
            }, new IToken[]
            {
                new TextToken(@"\"),
                new TextToken("_"),
                new TextToken("_"),
                new TextToken("ad"),
                new TextToken("\n")
            }).SetName("Should be TextToken when tag and screener are screened");
            yield return new TestCaseData(new List<List<IToken>>
            {
                new List<IToken>
                {
                    new TextToken("as"),
                    new ScreenerToken(@"\"),
                    new TextToken("c"),
                    new ScreenerToken(@"\"),
                    new TextToken(" d"),
                    new ScreenerToken(@"\"),
                    new TextToken("\n")
                }
            }, new IToken[]
            {
                new TextToken("as"),
                new TextToken(@"\"),
                new TextToken("c"),
                new TextToken(@"\"),
                new TextToken(" d"),
                new TextToken(@"\"),
                new TextToken("\n")
            }).SetName("Should be TextToken when screeners don't screen");
            yield return new TestCaseData(new List<List<IToken>>
            {
                new List<IToken>
                {
                    new TagToken("# "),
                    new TagToken("__"),
                    new TextToken("abc"),
                    new TagToken("__"),
                    new TextToken(" "),
                    new TagToken("_"),
                    new TextToken("dc"),
                    new TagToken("_"),
                    new TextToken("\n")
                }
            }, new IToken[]
            {
                new TagToken("# "),
                new TagToken("__", true),
                new TextToken("abc"),
                new TagToken("__"),
                new TextToken(" "),
                new TagToken("_", true),
                new TextToken("dc"),
                new TagToken("_"),
                new TextToken("\n")
            }).SetName("Should be TagToken when italic and strong inside header");
            yield return new TestCaseData(new List<List<IToken>>
            {
                new List<IToken>
                {
                    new TagToken("- "),
                    new TextToken("a"),
                    new TextToken("\n")
                },
                new List<IToken>
                {
                    new TagToken("- "),
                    new TextToken("b"),
                    new TextToken("\n")
                },
                new List<IToken>
                {
                    new TagToken("- "),
                    new TextToken("c"),
                    new TextToken("\n")
                }
            }, new IToken[]
            {
                new TagToken("- "),
                new TextToken("a"),
                new TextToken("\n"),
                new TagToken("- "),
                new TextToken("b"),
                new TextToken("\n"),
                new TagToken("- "),
                new TextToken("c"),
                new TextToken("\n")
            }).SetName("Simple unordered list should be marked by tags");
            yield return new TestCaseData(new List<List<IToken>>
            {
                new List<IToken>
                {
                    new TextToken("abc  "),
                    new ScreenerToken(@"\"),
                    new ScreenerToken(@"\"),
                    new TextToken("\n")
                }
            }, new IToken[]
            {
                new TextToken("abc  "),
                new TextToken(@"\"),
                new TextToken("\n")
            }).SetName("Screeners at the end should be valid");
            yield return new TestCaseData(new List<List<IToken>>
            {
                new List<IToken>
                {
                    new TagToken("__"),
                    new TextToken("abc cdef"),
                    new TagToken("__"),
                    new TextToken("\n")
                }
            }, new IToken[]
            {
                new TagToken("__", true),
                new TextToken("abc cdef"),
                new TagToken("__"),
                new TextToken("\n")
            }).SetName("Double tags at the begin and at the end of text should be valid");
            yield return new TestCaseData(new List<List<IToken>>
            {
                new List<IToken>
                {
                    new TextToken("abc "),
                    new ScreenerToken(@"\"),
                    new TextToken(" "),
                    new ScreenerToken(@"\"),
                    new TextToken(" cdef"),
                    new TextToken("\n")
                }
            }, new IToken[]
            {
                new TextToken("abc "),
                new TextToken(@"\"),
                new TextToken(" "),
                new TextToken(@"\"),
                new TextToken(" cdef"),
                new TextToken("\n")
            }).SetName("Screeners should be text when do not screen");
            yield return new TestCaseData(new List<List<IToken>>
            {
                new List<IToken>
                {
                    new ScreenerToken(@"\"),
                    new ScreenerToken(@"\"),
                    new TagToken("_"),
                    new TextToken("aa bc"),
                    new TagToken("_"),
                    new TextToken("\n")
                }
            }, new IToken[]
            {
                new TextToken(@"\"),
                new TagToken("_", true),
                new TextToken("aa bc"),
                new TagToken("_"),
                new TextToken("\n")
            }).SetName("Screened screener should not screen tag after it");
            yield return new TestCaseData(new List<List<IToken>>
            {
                new List<IToken>
                {
                    new TagToken("__"),
                    new TextToken("a"),
                    new TextToken("\n")
                },
                new List<IToken>
                {
                    new TextToken("b"),
                    new TagToken("__"),
                    new TextToken("\n")
                }
            }, new IToken[]
            {
                new TextToken("__"),
                new TextToken("a"),
                new TextToken("\n"),
                new TextToken("b"),
                new TextToken("__"),
                new TextToken("\n")
            }).SetName("Double tags should not be valid when it in different lines");
        }

        [TestCaseSource(nameof(Tests))]
        public void GetValidTokens_ShouldReturnCorrectTokens(List<List<IToken>> inputTokens,
            IToken[] expectedTokens)
        {
            var tokens = TagValidator.GetValidTokens(inputTokens.ToList()).ToList();
            tokens.Should().BeEquivalentTo(expectedTokens);
        }
    }
}