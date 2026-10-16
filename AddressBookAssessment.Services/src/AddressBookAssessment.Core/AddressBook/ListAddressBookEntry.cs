using AddressBookAssessment.Core.AddressBook.Specifications;
using AddressBookAssessment.Core.Shared;
using Mediator;
using System.Security.Claims;

namespace AddressBookAssessment.Core.AddressBook;

public record ListAddressBookEntry : IQuery<IReadOnlyList<AddressBookEntry>>
{
	public required ClaimsPrincipal User { get; set; }
}

public class ListAddressBookEntryHandler(IReadModel readModel)
	: IQueryHandler<ListAddressBookEntry, IReadOnlyList<AddressBookEntry>>
{
	public async ValueTask<IReadOnlyList<AddressBookEntry>> Handle(ListAddressBookEntry command, CancellationToken cancellationToken)
	{
		command.User.AssertIsAdmin();

		var getAllEntries = await readModel.Search(new AddressBookNonDeletedSpecification(), cancellationToken);

		if (getAllEntries == null)
		{
			throw new Exception("There are no entries to list. Please create an entry.");
		}

		return getAllEntries;
	}
}