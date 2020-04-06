namespace TasksBoard.Backend.Features.Users
{
    public class PublicUser
    {
        public string Name { get; set; }

        public string Email { get; set; }

        public string Biography { get; set; }

        public string Token { get; set; }
    }

    public class UserEnvelope
    {
        public UserEnvelope(PublicUser user)
        {
            User = user;
        }

        public PublicUser User { get; set; }
    }
}