using System.Text.RegularExpressions;

namespace Common
{
    public static class SocialSecurityNumber
    {
        private static Regex _regex = new Regex(@"^(19|20)[0-9]{2}((0[0-9])|(10|11|12))(([0-2][0-9])|(3[0-1])|(([7-8][0-9])|(6[1-9])|(9[0-1])))[0-9]{4}$");

        /// <summary>
        /// Ex. 198506141111
        /// </summary>
        public static bool IsValid(string s)
        {
            return _regex.IsMatch(s);
        }
    }
}
