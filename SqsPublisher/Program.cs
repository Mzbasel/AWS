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

// create a new message
var sendMessageRequest = new SendMessageRequest
{
    QueueUrl = queueUrlResponse.QueueUrl,
    MessageBody = JsonSerializer.Serialize(customer),
    //This is to make sure the message type is a type of CustomerCreated object
    MessageAttributes = new Dictionary<string, MessageAttributeValue>
    {
        {
            "MessageType", new MessageAttributeValue
            {
                DataType = "String",
                StringValue = nameof(CustomerCreated)
            }
        }
    }
};

//send a new message to the queue
var response = await sqsClient.SendMessageAsync(sendMessageRequest);