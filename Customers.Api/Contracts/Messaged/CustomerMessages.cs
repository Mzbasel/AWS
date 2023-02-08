using System;
namespace Customers.Api.Messaging
{
	public class CustomerCreated
	{
		public required string Id { get; init; }
        public required string GitHubUserName { get; init; }
        public required string FullName { get; init; }
        public required string Email { get; init; }
        public required DateTime DateOfBirth { get; init; }
    }

    public class CustomerUpdated
    {
        public required string Id { get; init; }
        public required string GitHubUserName { get; init; }
        public required string FullName { get; init; }
        public required string Email { get; init; }
        public required DateTime DateOfBirth { get; init; }

    }

    public class CustomerDeleted
    {
        public required string Id { get; init; }
    }
}

