using Common;
using NUnit.Framework;

namespace UnitTests
{
    public class SocialSecurityTests
    {
        [TestCase("198506141111")]
        [TestCase("195501017262")]
        [Test]
        public void ShouldPassTheseSocialSecurityNumbers(string input)
        {
            Assert.IsTrue(SocialSecurityNumber.IsValid(input), input);
        }

        [TestCase("19850614-1111")]
        [TestCase("8506141111")]
        [TestCase("198520141111")]
        [TestCase("198506401111")]
        [TestCase("1985-06-141111")]
        [TestCase(" 198506141111 ")]
        [Test]
        public void ShouldFailTheseSocialSecurityNumbers(string input)
        {
            Assert.IsFalse(SocialSecurityNumber.IsValid(input), input);
        }
    }
}