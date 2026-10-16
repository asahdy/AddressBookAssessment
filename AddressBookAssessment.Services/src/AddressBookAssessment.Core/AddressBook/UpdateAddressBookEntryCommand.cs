using AddressBookAssessment.Core.AddressBook.Specifications;
using AddressBookAssessment.Core.Shared;
using Mediator;
using System.Security.Claims;
using System.Data;

namespace AddressBookAssessment.Core.AddressBook;

public record UpdateAddressBookEntryCommand : ICommand<AddressBookEntry>
{
	public required ClaimsPrincipal User { get; set; }

	public Guid Id { get; set; }
	public required AddressBookEntryRequest AddressBookEntryRequest { get; set; }
}

public class UpdateAddressBookEntryCommandHandler(IWorkspace workspace, IReadModel readModel, TimeProvider timeProvider)
 : ICommandHandler<UpdateAddressBookEntryCommand, AddressBookEntry>
{
	public async ValueTask<AddressBookEntry> Handle(UpdateAddressBookEntryCommand command, CancellationToken cancellationToken)
	{
		command.User.AssertIsAdmin();

		var entries = await workspace.Load(new EntityByIdSpecification<AddressBookEntry, Guid>(command.Id), cancellationToken);

		var entry = entries.FirstOrDefault();

		if (entry is null)
		{
			throw new NotFoundException<AddressBookEntry>(command.Id.ToString());
		}

		var duplicates = await readModel.Search(new AddressBookEntryByNameSpecification(command.AddressBookEntryRequest.Name), cancellationToken);

		if (duplicates.Any(duplicate => duplicate.Id != command.Id))
		{
			throw new DuplicateNameException($"Address book entry with name: {command.AddressBookEntryRequest.Name} already exists.");
		}

		var now = timeProvider.GetUtcNow();

		entry.Update(command.AddressBookEntryRequest, now);

		return entry;

	}
}
