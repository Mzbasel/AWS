using System.Text.Json;
using Amazon.SQS;
using Amazon.SQS.Model;
using SqsPublisher;

var sqsClient = new AmazonSQSClient();

var customer = new CustomerCreated
{
    Id = Guid.NewGuid(),
    Email = "meryem@test.com",
    FullName = "Meryem Sel",
    DateOfBirth = new DateTime(1998, 8, 6),
    GitHubUserName = "Mzbasel"
};

var queueUrlResponse = await sqsClient.GetQueueUrlAsync("customers");

var sendMessageRequest = new SendMessageRequest
{
    QueueUrl = queueUrlResponse.QueueUrl,
    MessageBody = JsonSerializer.Serialize(customer)
};

var response = await sqsClient.SendMessageAsync(sendMessageRequest);

//Console.Write(response);