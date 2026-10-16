using AddressBookAssessment.Core;
using AddressBookAssessment.Core.AddressBook;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AddressBookAssessment.Services.Data;

public class AddressBookEntryConfiguration : IEntityTypeConfiguration<AddressBookEntry>
{
	public void Configure(EntityTypeBuilder<AddressBookEntry> builder)
	{
		builder.ToTable("AddressBookEntries");
		builder.HasIndex(x => x.Name)
			.IsUnique();

		builder.Property(x => x.TimezoneId)
			.HasConversion(
				timezoneId => timezoneId.Value,
				value => TimezoneId.Create(value)
			);

		builder.OwnsOne(x => x.Address, addressBuilder =>
		{
			addressBuilder.Property(a => a.StateAbbreviation)
				.HasConversion(
					state => state.Value,
					value => StateAbbreviation.Create(value)
				);

			addressBuilder.Property(a => a.CountryAbbreviation)
				.HasConversion(
					country => country.Value,
					value => CountryAbbreviation.Create(value)
				);

			addressBuilder.Property(a => a.PostalCode)
				.HasConversion(
					postalCode => postalCode.Value,
					value => PostalCode.Create(value)
				);
		});

		builder.OwnsMany(x => x.Contacts, contactBuilder =>
		{
			contactBuilder.ToTable("AddressBookContacts");

			contactBuilder.Property(c => c.EmailAddress)
				.HasConversion(
					email => email == null ? null : email.Value,
					value => value == null ? null : EmailAddress.Create(value)
				);

			contactBuilder.Property(c => c.OfficePhoneNumber)
				.HasConversion(
					phone => phone == null ? null : phone.Value,
					value => value == null ? null : PhoneNumber.Create(value)
				);

			contactBuilder.Property(c => c.CellPhoneNumber)
				.HasConversion(
					phone => phone == null ? null : phone.Value,
					value => value == null ? null : PhoneNumber.Create(value)
				);
		});

		builder.HasQueryFilter(x => x.DeletedAt == null);
	}
}
