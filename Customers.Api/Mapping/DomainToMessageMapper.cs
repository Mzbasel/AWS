using System;
using Customers.Api.Domain;
using Customers.Api.Messaging;

namespace Customers.Api.Mapping
{
	public static class DomainToMessageMapper
	{
		public static CustomerCreated ToCustomerCreatedMessage(this Customer customer)
		{
			return new CustomerCreated
			{
				Id = customer.Id,
				Email = customer.Email,
				GitHubUserName = customer.GitHubUsername,
				FullName = customer.FullName,
				DateOfBirth = customer.DateOfBirth
			};
		}

		public static CustomerUpdated ToCustomerUpdated(this Customer customer)
		{
            return new CustomerUpdated
            {
                Id = customer.Id,
                Email = customer.Email,
                GitHubUserName = customer.GitHubUsername,
                FullName = customer.FullName,
                DateOfBirth = customer.DateOfBirth
            };
        }
    }
}

