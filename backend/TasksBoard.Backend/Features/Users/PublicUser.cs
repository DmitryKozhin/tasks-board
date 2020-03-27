namespace TasksBoard.Backend.Features.Users
{
    public class Person
    {
        public string Name { get; set; }

        public string Email { get; set; }

        public string Biography { get; set; }

        public string Token { get; set; }
    }

    public class UserEnvelope
    {
        public UserEnvelope(Person user)
        {
            User = user;
        }

        public Person User { get; set; }
    }
}