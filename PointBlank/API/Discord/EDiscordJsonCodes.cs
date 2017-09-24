namespace PointBlank.API.Discord
{
    /// <summary>
    /// Lists of Json codes sent by discord
    /// </summary>
    public enum EDiscordJsonCodes
    {
        UnknownAccount = 10001,
        UnknownApplication = 10002,
        UnknownChannel = 10003,
        UnknownGuild = 10004,
        UnknownIntegration = 10005,
        UnknownInvite = 10006,
        UnknownMember = 10007,
        UnknownMessage = 10008,
        UnknownOverwrite = 10009,
        UnknownProvider = 10010,
        UnknownRole = 10011,
        UnknownToken = 10012,
        UnknownUser = 10013,
        UnknownEmoji = 10014,
        BotsCannotUseEndpoint = 20001,
        OnlyBotsCanUseEnpoint = 20002,
        MaxNumberOfGuilds = 30001,
        MaxNumberOfFriends = 30002,
        MaxNumberOfPins = 30003,
        MaxNumberOfGuildRoles = 30005,
        TooManyReactions = 30010,
        Unauthorized = 40001,
        MissingAccess = 50001,
        InvalidAccountType = 50002,
        CannotExecuteOnDm = 50003,
        EmbededDisabled = 50004,
        CannotEditMessage = 50005,
        CannotSendEmpty = 50006,
        CannotSendToUser = 50007,
        CannotSendToVoice = 50008,
        VerificationTooHigh = 50009,
        Oauth2NoBot = 50010,
        Oauth2Limit = 50011,
        Oauth2Invalid = 50012,
        MissingPermissions = 50013,
        InvalidToken = 50014,
        NoteTooLong = 50015,
        TooFewOrTooManyMessagesToDelete = 50016,
        InvalidPinChannel = 50019,
        CannotExecuteOnSystemMessage = 50021,
        MessageTooOld = 50034,
        InviteBotFail = 50036,
        ReactionBlocked = 90001
    }
}
