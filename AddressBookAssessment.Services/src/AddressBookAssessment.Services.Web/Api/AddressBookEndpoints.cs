using AddressBookAssessment.Core;
using AddressBookAssessment.Core.AddressBook;
using AddressBookAssessment.Services.Web.Mediator;
using AddressBookAssessment.Services.Web.OpenApi;

namespace AddressBookAssessment.Services.Web.Api;

public static class AddressBookEndpoints
{
	public static void MapAddressBookEndpoints(this RouteGroupBuilder api)
	{
		var addressBook = api.MapGroup("addressbook")
			.WithTags("Address Book");

		addressBook.Mediate<CreateAddressBookEntryCommand>(HttpMethod.Post)
			.RequireAuthorization()
			.WithSummary("Creates a new address book entry")
			.WithExample("Create address book entry", _addressBookEntryRequest);

		addressBook.Mediate<ListAddressBookEntry>(HttpMethod.Get)
			.RequireAuthorization()
			.WithSummary("Lists address book entries");

		addressBook.Mediate<DeleteAddressBookEntryCommand>(HttpMethod.Delete)
			.RequireAuthorization()
			.WithSummary("Deletes an address book entry");

		addressBook.Mediate<UpdateAddressBookEntryCommand>(HttpMethod.Put)
			.RequireAuthorization()
			.WithSummary("Updates an address book entry");
	}

	private static readonly AddressBookEntryRequest _addressBookEntryRequest = new()
	{
		Name = "Home",
		Address = new()
		{
			Line1 = "59 Scott Ct.",
			City = "Germantown",
			StateAbbreviation = StateAbbreviation.Create("OH"),
			CountryAbbreviation = CountryAbbreviation.US,
			PostalCode = PostalCode.Create("45327")
		},
		TimezoneId = TimezoneId.Create("America/New_York"),
		Contacts = [
			new()
			{
				Name = "Ryan",
				CellPhoneNumber = PhoneNumber.Create("937-604-1566"),
				OfficePhoneNumber = PhoneNumber.Create("937-415-1728"),
				EmailAddress = EmailAddress.Create("rfoote@daytonfreight.com"),
				IsPrimary = true
			}
		]
	};
}
