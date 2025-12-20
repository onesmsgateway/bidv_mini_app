namespace PaymentPackageTelco.api.Common
{
    public enum AreaType
    {
        domestic = 0, //vietnam
        internation = 1,
    }
    public enum AreaInternation
    {
        nation = 2, //hongkong, china, usa
        area = 3, //asean
    }

    public enum PackageType
    {
        topupdata = 0,
        combo = 1
    }

    public enum DateUse
    {
        time = 0,
        onetosevendays = 1,
        seventofiveteendays = 2,
        fiveteentothirtydays = 3,
    }

    public enum DataVolume
    {
        smallthan1gb = 0,
        from1gbto2gb = 1,
        from2gbto4gb = 2,
        from4gbto8gb = 3,
        morethan8gb = 4
    }

    public enum SocialNet
    {
        facebook = 0,
        tiktok = 1,
        youtube = 2
    }

    public enum UtilityType
    {
        data = 0,
        social_network = 1
    }

    public static class EnumUtil
    {
        public static string ToUtilityTypeName(int? value)
        {
            if (Enum.IsDefined(typeof(UtilityType), value == null ? 0 : value))
            {
                if (value == null) value = 0;
                return ((UtilityType)value).ToString();
            }
            return "";
        }

        public static string ToPackageTypeName(int? value)
        {
            if (Enum.IsDefined(typeof(PackageType), value == null ? 0 : value))
            {
                if (value == null) value = 0;
                return ((PackageType)value).ToString();
            }
            return "";
        }
    }
}
