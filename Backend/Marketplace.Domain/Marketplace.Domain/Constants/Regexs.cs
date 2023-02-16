﻿namespace Marketplace.Domain.Constants;

public static class Regexs
{
    public const string EmailRegexPattern =
        @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
        @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$";

    public const string PhoneNumberRegexPatter = @"^\+998\d{2}\d{3}\d{2}\d{2}$";
}