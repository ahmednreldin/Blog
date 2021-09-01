/*  Mapper to map [ values from appsetting.json ⇒ objects ] */

namespace OpenSchool.src.Helpers
{
    public class JWT
    {
        public string Key { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public double DurationInDays { get; set; }
    }
}
