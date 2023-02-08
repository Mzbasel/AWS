﻿using Customers.Api.Domain;
using Customers.Api.Mapping;
using Customers.Api.Messaging;
using Customers.Api.Repositories;
using FluentValidation;
using FluentValidation.Results;

namespace Customers.Api.Services;

public class CustomerService : ICustomerService
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IGitHubService _gitHubService;
    private readonly ISQSMessenger messenger;

    public CustomerService(ICustomerRepository customerRepository, 
        IGitHubService gitHubService,
        ISQSMessenger messenger)
    {
        _customerRepository = customerRepository;
        _gitHubService = gitHubService;
        this.messenger = messenger;
    }

    public async Task<bool> CreateAsync(Customer customer)
    {
        var existingUser = await _customerRepository.GetAsync(customer.Id);
        if (existingUser is not null)
        {
            var message = $"A user with id {customer.Id} already exists";
            throw new ValidationException(message, GenerateValidationError(nameof(Customer), message));
        }

        /*var isValidGitHubUser = await _gitHubService.IsValidGitHubUser(customer.GitHubUsername);
        if (!isValidGitHubUser)
        {
            var message = $"There is no GitHub user with username {customer.GitHubUsername}";
            throw new ValidationException(message, GenerateValidationError(nameof(customer.GitHubUsername), message));
        }*/
        
        var customerDto = customer.ToCustomerDto();
        var response = await _customerRepository.CreateAsync(customerDto);

        if (response)
        {
            await messenger.SendMessageAsync(customer.ToCustomerCreatedMessage);
        }

        return response;
    }

    public async Task<Customer?> GetAsync(string id)
    {
        var customerDto = await _customerRepository.GetAsync(id);
        return customerDto?.ToCustomer();
    }

    public async Task<IEnumerable<Customer>> GetAllAsync()
    {
        var customerDtos = await _customerRepository.GetAllAsync();
        return customerDtos.Select(x => x.ToCustomer());
    }

    public async Task<bool> UpdateAsync(Customer customer)
    {
        var customerDto = customer.ToCustomerDto();
        
        var isValidGitHubUser = await _gitHubService.IsValidGitHubUser(customer.GitHubUsername);
        if (!isValidGitHubUser)
        {
            var message = $"There is no GitHub user with username {customer.GitHubUsername}";
            throw new ValidationException(message, GenerateValidationError(nameof(customer.GitHubUsername), message));
        }
        
        var response = await _customerRepository.UpdateAsync(customerDto);

        if (response)
        {
            await messenger.SendMessageAsync(customer.ToCustomerUpdated);
        }

        return response;
    }

    public async Task<bool> DeleteAsync(string id)
    {
        var response = await _customerRepository.DeleteAsync(id);

        if (response)
        {
            await messenger.SendMessageAsync(new CustomerDeleted
            {
                Id = id
            });
        }

        return response;
    }

    private static ValidationFailure[] GenerateValidationError(string paramName, string message)
    {
        return new []
        {
            new ValidationFailure(paramName, message)
        };
    }
}
