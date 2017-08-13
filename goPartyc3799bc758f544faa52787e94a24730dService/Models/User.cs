using System.ComponentModel.DataAnnotations;


namespace goPartyc3799bc758f544faa52787e94a24730dService.CustomAuth.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }
    }
}