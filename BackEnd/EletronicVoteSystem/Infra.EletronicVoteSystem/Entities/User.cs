﻿namespace Infra.EletronicVoteSystem.Entities
{
    public class User : BaseEntity
    {
        public string Email { get; set; }

        public string Password { get; set; }
    }
}
