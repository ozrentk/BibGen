namespace BibGen.Svc.Model
{
    public class BibEntry
    {
        public Dictionary<string, string> DataItems { get; set; } =
            new Dictionary<string, string>();

        //[Display(Name = "Dionica")]
        //public string TrackType { get; set; }

        //[Display(Name = "Ime")]
        //public string FirstName { get; set; }

        //[Display(Name = "Prezime")]
        //public string LastName { get; set; }

        //[Display(Name = "Puno ime")]
        //public string FullName { get; set; }

        //[Display(Name = "Startni broj")]
        //public int Number { get; set; }

        //[Display(Name = "Spol")]
        //public string Gender { get; set; }

        //[Display(Name = "Klub")]
        //public string ClubSchoolTeam { get; set; }

        //[Display(Name = "Država")]
        //public string Country { get; set; }

        //[Display(Name = "Kupon")]
        //public string Coupon { get; set; }

        //[Display(Name = "Plaćena")]
        //public string Paid { get; set; }

        //public override string ToString()
        //{
        //    return String.Join('-', Lines);
        //}

        //public string[] Lines =>
        //    (Coupon?.StartsWith("ELITE") ?? false) ?
        //        new[] { $"{Number}", LastName.ToUpper() } :
        //        new[] { $"{Number}" };

        //public string[] Lines =>
        //    (Coupon?.StartsWith("ELITE") ?? false) ?
        //        new[] { $"{Number}" } :
        //        new[] { $"{Number}", LastName.ToUpper() };

        //public static Dictionary<string, PropertyInfo> GetPropertyMap()
        //{
        //    var propertyMap = new Dictionary<string, PropertyInfo>();
        //    var properties = typeof(BibEntry).GetProperties();

        //    foreach (var property in properties)
        //    {
        //        var displayAttribute = property.GetCustomAttribute<DisplayAttribute>();
        //        if (displayAttribute != null)
        //        {
        //            propertyMap[displayAttribute.Name] = property;
        //        }
        //    }

        //    return propertyMap;
        //}
    }
}
