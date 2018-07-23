namespace Api.EletronicVoteSystem.Auth
{
    public class TokenConfiguration
    {
        public string Audience { get; set; }

        public string Issuer { get; set; }

        public uint Seconds { get; set; }
    }
}
