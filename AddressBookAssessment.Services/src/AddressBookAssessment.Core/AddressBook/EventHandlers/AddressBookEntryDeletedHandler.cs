using Mediator;
using Microsoft.Extensions.Logging;

namespace AddressBookAssessment.Core.AddressBook.EventHandlers;

public class AddressBookEntryDeletedHandler(ILogger<AddressBookEntryDeletedHandler> logger) : INotificationHandler<AddressBookEntryDeleted>
{
	public ValueTask Handle(AddressBookEntryDeleted notification, CancellationToken cancellationToken)
	{
		logger.LogInformation($"AddressBook entry# {notification.Id} with name: {notification.Name} deleted.");

		return ValueTask.CompletedTask;
	}
}